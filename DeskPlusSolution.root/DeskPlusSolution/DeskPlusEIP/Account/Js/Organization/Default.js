(function () {
    "use strict";

    function byId(id) { return document.getElementById(id); }
    function element(tag, className, text) {
        var node = document.createElement(tag);
        if (className) node.className = className;
        if (text !== undefined) node.textContent = text;
        return node;
    }
    function button(className, text, action) {
        var node = element("button", className, text);
        node.type = "button";
        node.addEventListener("click", action);
        return node;
    }
    function normalize(value) { return String(value || "").toLocaleLowerCase().replace(/\s+/g, " ").trim(); }
    function role(member) { return member.title || ""; }

    byId("refresh-chart").addEventListener("click", function () { window.location.reload(); });
    var data;
    try { data = JSON.parse(byId("organization-data").textContent); }
    catch (error) { data = { error: true }; }
    if (data.error) {
        byId("chart-error").hidden = false;
        byId("chart-viewport").hidden = true;
        byId("chart-search-form").hidden = true;
        document.querySelector(".org-tree-tools").hidden = true;
        return;
    }

    var departments = data.departments, index = Object.create(null), users = Object.create(null);
    var roots = [], collapsed = Object.create(null), expandedMembers = Object.create(null);
    var cardIndex = Object.create(null), keyword = "", matches = [], currentMatch = -1;
    var scale = 1, canvasLeft = 0, lastFocus = null;
    var viewport = byId("chart-viewport"), canvas = byId("chart-canvas"), stage = byId("chart-stage");

    departments.forEach(function (dept) {
        dept.children = [];
        dept.members = [];
        index[dept.id] = dept;
    });
    // 비활성/누락 부모는 최상위로 올리고, 잘못된 순환 참조도 끊는다.
    departments.forEach(function (dept) {
        if (!index[dept.parentId] || dept.parentId === dept.id) dept.parentId = 0;
    });
    departments.forEach(function (dept) {
        var parent = dept, seen = Object.create(null);
        while (parent) {
            if (seen[parent.id]) { dept.parentId = 0; break; }
            seen[parent.id] = true;
            parent = index[parent.parentId];
        }
    });
    departments.forEach(function (dept) {
        if (index[dept.parentId]) index[dept.parentId].children.push(dept);
        else roots.push(dept);
    });
    data.members.forEach(function (member) {
        if (!index[member.deptId]) {
            if (!index[-1]) {
                index[-1] = { id: -1, parentId: 0, name: "부서 미배정", children: [], members: [] };
                roots.push(index[-1]);
            }
            member.deptId = -1;
        }
        var assignments = users[member.id] || (users[member.id] = []);
        if (assignments.some(function (entry) { return entry.deptId === member.deptId; })) return;
        assignments.push(member);
        index[member.deptId].members.push(member);
    });
    function countMembers(dept) {
        var seen = Object.create(null), stack = [dept];
        while (stack.length) {
            var item = stack.pop();
            item.members.forEach(function (member) { seen[member.id] = true; });
            stack = stack.concat(item.children);
        }
        dept.count = Object.keys(seen).length;
        dept.children.forEach(countMembers);
    }
    roots.forEach(countMembers);
    byId("department-count").textContent = departments.length;
    byId("member-count").textContent = Object.keys(users).length;
    byId("chart-empty").hidden = roots.length > 0;
    byId("my-department").disabled = !index[data.currentDeptId];

    function personMatches(member) {
        return keyword && normalize([member.name, member.title, member.email,
            member.officePhone, member.mobile].join(" ")).indexOf(keyword) !== -1;
    }
    function memberAvatar(member, className) {
        var avatar = element("span", className, (member.name || "?").substring(0, 1));
        avatar.setAttribute("aria-hidden", "true");
        if (member.photo) {
            var photo = element("img");
            photo.alt = "";
            photo.loading = className === "org-avatar" ? "lazy" : "eager";
            photo.addEventListener("error", function () { if (photo.parentNode) photo.parentNode.removeChild(photo); });
            avatar.appendChild(photo);
            photo.src = member.photo;
        }
        return avatar;
    }
    function person(member) {
        var node = button("org-person" + (personMatches(member) ? " is-match" : ""), undefined, function () { showProfile(member, node); });
        var text = element("span", "org-person-text");
        text.appendChild(element("span", "org-person-name", member.name || "이름 없음"));
        text.appendChild(element("span", "org-person-role", role(member) || "직위 미등록"));
        node.appendChild(memberAvatar(member, "org-avatar"));
        node.appendChild(text);
        if (users[member.id].length > 1) node.title = "겸직 포함 " + users[member.id].length + "개 부서 소속";
        return node;
    }

    function renderNode(dept, depth) {
        var item = element("li", "org-node");
        var card = element("article", "org-card " + (depth === 0 ? "level-root" : depth === 1 ? "level-division" : "level-team"));
        card.setAttribute("aria-label", dept.name + ", " + dept.count + "명");
        if (dept.id === -1) card.classList.add("is-unassigned");
        if (matches.indexOf(dept.id) !== -1) card.classList.add("is-match");
        if (matches[currentMatch] === dept.id) card.classList.add("is-current");
        cardIndex[dept.id] = card;
        var header;
        if (dept.children.length) {
            header = button("org-card-header", undefined, function () {
                collapsed[dept.id] = !collapsed[dept.id];
                render();
                cardIndex[dept.id].querySelector(".org-card-header").focus({ preventScroll: true });
            });
            header.setAttribute("aria-expanded", String(!collapsed[dept.id]));
            header.setAttribute("aria-controls", "org-children-" + dept.id);
            header.title = "하위 부서 " + (collapsed[dept.id] ? "펼치기" : "접기");
        } else header = element("div", "org-card-header");
        var title = element("span", "org-card-title");
        if (dept.children.length) title.appendChild(element("span", "org-branch-indicator", collapsed[dept.id] ? "▸" : "▾"));
        title.appendChild(document.createTextNode(dept.name || "이름 없는 부서"));
        header.appendChild(title);
        header.appendChild(element("span", "org-card-count", dept.count + "명"));
        card.appendChild(header);

        var body = element("div", "org-card-body");
        if (dept.members.length) {
            body.appendChild(person(dept.members[0]));
            if (dept.members.length > 1) {
                var open = !!expandedMembers[dept.id];
                var toggle = button("org-members-toggle", (open ? "▲ 팀원 접기" : "▼ 팀원 " + (dept.members.length - 1) + "명 보기"), function () {
                    expandedMembers[dept.id] = !expandedMembers[dept.id];
                    render();
                    cardIndex[dept.id].querySelector(".org-members-toggle").focus({ preventScroll: true });
                });
                toggle.setAttribute("aria-expanded", String(open));
                toggle.setAttribute("aria-controls", "org-members-" + dept.id);
                body.appendChild(toggle);
                var members = element("ul", "org-members");
                members.id = "org-members-" + dept.id;
                members.hidden = !open;
                // 펼칠 때만 사진과 상세 버튼을 생성한다.
                if (open) dept.members.slice(1).forEach(function (member) {
                    var memberItem = element("li");
                    memberItem.appendChild(person(member));
                    members.appendChild(memberItem);
                });
                body.appendChild(members);
            }
        } else body.appendChild(element("p", "org-no-members", "직접 소속된 구성원 없음"));
        if (collapsed[dept.id] && dept.children.length) body.appendChild(element("p", "org-collapsed-count", "하위 부서 " + dept.children.length + "개 · 펼치려면 부서명 선택"));
        card.appendChild(body);
        item.appendChild(card);
        if (dept.children.length) {
            var children = element("ul", "org-children");
            children.id = "org-children-" + dept.id;
            children.hidden = !!collapsed[dept.id];
            if (!collapsed[dept.id]) dept.children.forEach(function (child) { children.appendChild(renderNode(child, depth + 1)); });
            item.appendChild(children);
        }
        return item;
    }
    function render() {
        var tree = byId("chart-tree");
        tree.textContent = "";
        cardIndex = Object.create(null);
        roots.forEach(function (dept) { tree.appendChild(renderNode(dept, 0)); });
        resizeStage();
    }
    function resizeStage() {
        var width = canvas.offsetWidth * scale, height = canvas.offsetHeight * scale;
        canvasLeft = Math.max(24, (viewport.clientWidth - width) / 2);
        canvas.style.left = canvasLeft + "px";
        canvas.style.top = "24px";
        canvas.style.transform = "scale(" + scale + ")";
        stage.style.width = Math.max(viewport.clientWidth, width + 48) + "px";
        stage.style.height = Math.max(viewport.clientHeight, height + 48) + "px";
        byId("zoom-value").textContent = Math.round(scale * 100) + "%";
        byId("zoom-out").disabled = scale <= .25;
        byId("zoom-in").disabled = scale >= 1.5;
    }
    function centerOn(id) {
        var card = cardIndex[id];
        if (!card) return;
        var bounds = card.getBoundingClientRect(), view = viewport.getBoundingClientRect();
        viewport.scrollTo({ left: viewport.scrollLeft + bounds.left - view.left - (viewport.clientWidth - bounds.width) / 2,
            top: viewport.scrollTop + bounds.top - view.top - 35, behavior: "smooth" });
    }
    function openAncestors(id) {
        var parent = index[index[id].parentId];
        while (parent) { collapsed[parent.id] = false; parent = index[parent.parentId]; }
    }
    function setScale(value) {
        var x = (viewport.scrollLeft + viewport.clientWidth / 2 - canvasLeft) / scale;
        var y = (viewport.scrollTop + viewport.clientHeight / 2 - 24) / scale;
        scale = Math.max(.25, Math.min(1.5, value));
        resizeStage();
        viewport.scrollLeft = x * scale + canvasLeft - viewport.clientWidth / 2;
        viewport.scrollTop = y * scale + 24 - viewport.clientHeight / 2;
    }

    function runSearch(event) {
        if (event) event.preventDefault();
        keyword = normalize(byId("chart-search").value);
        matches = [];
        if (keyword) Object.keys(index).forEach(function (id) {
            var dept = index[id], memberMatch = dept.members.some(personMatches);
            if (normalize(dept.name).indexOf(keyword) !== -1 || memberMatch) {
                matches.push(dept.id);
                openAncestors(dept.id);
                if (memberMatch) expandedMembers[dept.id] = true;
            }
        });
        currentMatch = matches.length ? 0 : -1;
        byId("search-result").hidden = !keyword;
        updateSearchStatus();
        render();
        if (matches.length) centerOn(matches[0]);
    }
    function updateSearchStatus() {
        byId("search-status").textContent = matches.length ? "검색 결과 " + matches.length + "개 부서 (" + (currentMatch + 1) + " / " + matches.length + ")" : "검색 결과가 없습니다. 다른 부서명이나 이름을 입력해 주세요.";
        byId("previous-result").disabled = matches.length < 2;
        byId("next-result").disabled = matches.length < 2;
    }
    function moveResult(step) {
        if (!matches.length) return;
        currentMatch = (currentMatch + step + matches.length) % matches.length;
        openAncestors(matches[currentMatch]);
        updateSearchStatus();
        render();
        centerOn(matches[currentMatch]);
    }
    function profileGroup(title) {
        var group = element("div", "org-profile-group");
        group.appendChild(element("h3", "", title));
        byId("dialog-content").appendChild(group);
        return group;
    }
    function contact(group, label, value, scheme) {
        if (!value) return;
        var paragraph = element("p");
        paragraph.appendChild(document.createTextNode(label + " "));
        var link = element("a", "", value);
        link.href = scheme + (scheme === "tel:" ? value.replace(/[^\d+*#()-]/g, "") : encodeURIComponent(value));
        paragraph.appendChild(link);
        group.appendChild(paragraph);
    }
    function showProfile(member, trigger) {
        lastFocus = trigger;
        var previousAvatar = byId("dialog-avatar");
        if (previousAvatar) previousAvatar.parentNode.removeChild(previousAvatar);
        var avatar = memberAvatar(member, "org-avatar org-profile-avatar");
        avatar.id = "dialog-avatar";
        byId("dialog-name").parentNode.insertBefore(avatar, byId("dialog-name"));
        byId("dialog-name").textContent = member.name || "이름 없음";
        byId("dialog-content").textContent = "";
        var assignments = profileGroup("소속 · 직위");
        users[member.id].forEach(function (entry) {
            var paragraph = element("div", "org-profile-assignment", index[entry.deptId].name);
            paragraph.appendChild(element("small", "", entry.primary ? "주부서" : users[member.id].length > 1 ? "겸직" : ""));
            if (role(entry)) paragraph.appendChild(element("p", "", role(entry)));
            assignments.appendChild(paragraph);
        });
        var contacts = profileGroup("연락처");
        contact(contacts, "사무실", member.officePhone, "tel:");
        if (member.extension) contacts.appendChild(element("p", "", "내선 " + member.extension));
        contact(contacts, "휴대전화", member.mobile, "tel:");
        var emails = Object.create(null);
        users[member.id].forEach(function (entry) {
            if (entry.email && !emails[entry.email]) { contact(contacts, "이메일", entry.email, "mailto:"); emails[entry.email] = true; }
        });
        if (contacts.children.length === 1) contacts.appendChild(element("p", "", "등록된 연락처가 없습니다."));
        byId("member-dialog").hidden = false;
        document.body.classList.add("org-modal-open");
        byId("close-dialog").focus();
    }
    function closeProfile() {
        byId("member-dialog").hidden = true;
        document.body.classList.remove("org-modal-open");
        if (lastFocus && document.body.contains(lastFocus)) lastFocus.focus({ preventScroll: true });
    }

    byId("chart-search-form").addEventListener("submit", runSearch);
    byId("chart-search").addEventListener("search", function () { if (!this.value) runSearch(); });
    byId("clear-search").addEventListener("click", function () { byId("chart-search").value = ""; runSearch(); });
    byId("previous-result").addEventListener("click", function () { moveResult(-1); });
    byId("next-result").addEventListener("click", function () { moveResult(1); });
    byId("my-department").addEventListener("click", function () { openAncestors(data.currentDeptId); render(); centerOn(data.currentDeptId); });
    byId("expand-all").addEventListener("click", function () { collapsed = Object.create(null); render(); });
    byId("collapse-all").addEventListener("click", function () {
        Object.keys(index).forEach(function (id) { collapsed[id] = true; });
        render();
        if (roots.length) centerOn(roots[0].id);
    });
    byId("zoom-out").addEventListener("click", function () { setScale(scale - .1); });
    byId("zoom-in").addEventListener("click", function () { setScale(scale + .1); });
    byId("fit-chart").addEventListener("click", function () {
        setScale(Math.min(1, (viewport.clientWidth - 48) / canvas.offsetWidth, (viewport.clientHeight - 48) / canvas.offsetHeight));
        viewport.scrollLeft = 0;
        viewport.scrollTop = 0;
    });
    byId("close-dialog").addEventListener("click", closeProfile);
    byId("member-dialog").addEventListener("click", function (event) { if (event.target === this) closeProfile(); });
    byId("member-dialog").addEventListener("keydown", function (event) {
        if (event.key === "Escape") { event.preventDefault(); closeProfile(); }
        if (event.key === "Tab") {
            var controls = this.querySelectorAll("button, a[href]"), first = controls[0], last = controls[controls.length - 1];
            if (event.shiftKey && document.activeElement === first) { event.preventDefault(); last.focus(); }
            else if (!event.shiftKey && document.activeElement === last) { event.preventDefault(); first.focus(); }
        }
    });
    window.addEventListener("resize", resizeStage);
    render();
    if (roots.length) centerOn(roots[0].id);
}());
