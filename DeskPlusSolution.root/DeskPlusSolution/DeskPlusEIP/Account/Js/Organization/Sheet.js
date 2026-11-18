(function () {
    "use strict";
    function id(name) { return document.getElementById(name); }
    function node(tag, className, text) {
        var result = document.createElement(tag);
        if (className) result.className = className;
        if (text !== undefined) result.textContent = text;
        return result;
    }
    function message(text, error) {
        var box = id("sheet-message");
        box.textContent = text || ""; box.hidden = !text; box.classList.toggle("is-error", !!error);
    }
    var boot;
    try { boot = JSON.parse(id("sheet-data").textContent); }
    catch (error) { boot = { error: true }; }
    var Model = window.OrganizationSheetModel;
    if (boot.error || !Model) {
        id("sheet-state").textContent = "불러오기 실패";
        message("조직도를 불러오지 못했습니다. 새로고침 후 다시 확인해 주세요.", true);
        return;
    }
    var state = boot.state, scale = 1, autoFit = true, uploading = false, nativeWidth = 0, nativeHeight = 0;
    var viewport = id("sheet-viewport"), canvas = id("sheet-canvas"), page = document.querySelector(".sheet-page");
    function refreshState() {
        id("sheet-upload").hidden = id("sheet-upload-help").hidden = !boot.canUpload;
        id("sheet-upload").disabled = uploading || !state.storageReady;
        id("sheet-workbook").disabled = uploading;
        id("sheet-upload").textContent = uploading ? "업로드 중…" : "엑셀 업로드";
        page.classList.toggle("is-uploading", uploading);
        id("sheet-state").textContent = uploading ? "엑셀을 읽고 있습니다." : state.isTemplate ? "기본 조직도" : "현재 조직도";
        id("sheet-source").textContent = state.document.sourceFileName ? state.document.sourceFileName + " · " + state.document.sourceSheetName + " 시트" : "";
        id("sheet-modified").textContent = state.modifiedAt ? "최근 갱신 " + new Date(state.modifiedAt).toLocaleString("ko-KR", { timeZone: "Asia/Seoul" }) : "";
        if (boot.canUpload && !state.storageReady) message("조직도 저장소가 아직 설치되지 않았습니다. 서버 설치를 완료하면 업로드할 수 있습니다.", true);
    }
    function resize() {
        if (autoFit && nativeWidth) scale = Math.max(.25, Math.min(1, (viewport.clientWidth - 36) / nativeWidth));
        canvas.style.left = Math.max(18, (viewport.clientWidth - nativeWidth * scale) / 2) + "px";
        canvas.style.transform = "scale(" + scale + ")";
        id("sheet-stage").style.width = Math.max(viewport.clientWidth, nativeWidth * scale + 36) + "px";
        id("sheet-stage").style.height = Math.max(viewport.clientHeight, nativeHeight * scale + 36) + "px";
        id("sheet-zoom-value").textContent = Math.round(scale * 100) + "%";
        id("sheet-zoom-out").disabled = scale <= .25; id("sheet-zoom-in").disabled = scale >= 1.5;
    }
    function render() {
        var documentModel = state.document, x = Model.offsets(documentModel.columns), y = Model.offsets(documentModel.rows);
        nativeWidth = x[x.length - 1]; nativeHeight = y[y.length - 1];
        var calculated = Model.calculate(documentModel);
        canvas.textContent = ""; canvas.style.width = nativeWidth + "px"; canvas.style.height = nativeHeight + "px";
        var fragment = document.createDocumentFragment();
        documentModel.cells.forEach(function (cell) {
            var value = calculated.values[Model.key(cell.r, cell.c)], rawText = value === undefined || value === null ? "" : String(value);
            var numeric = typeof value === "number" || /^-?\d+(\.\d+)?$/.test(rawText);
            var item = node("div", "sheet-cell"); item.dataset.cell = Model.key(cell.r, cell.c);
            item.appendChild(node("span", "", numeric ? (cell.prefix || "") + rawText + (cell.suffix || "") : rawText));
            var style = documentModel.styles[cell.style];
            item.style.left = x[cell.c] + "px"; item.style.top = y[cell.r] + "px";
            item.style.width = (x[cell.c + cell.cs] - x[cell.c]) + "px"; item.style.height = (y[cell.r + cell.rs] - y[cell.r]) + "px";
            item.style.fontSize = style.fontSize + "px"; item.style.fontWeight = style.bold ? "700" : "400";
            item.style.color = style.color; item.style.backgroundColor = style.fill; item.style.textAlign = style.align;
            item.style.justifyContent = style.align === "left" ? "flex-start" : style.align === "right" ? "flex-end" : "center";
            item.style.alignItems = style.vertical === "top" ? "flex-start" : style.vertical === "bottom" ? "flex-end" : "center";
            ["top", "right", "bottom", "left"].forEach(function (side) { item.style["border" + side.charAt(0).toUpperCase() + side.slice(1)] = style[side].width + "px solid " + style[side].color; });
            if (value === "#ERROR!") item.classList.add("is-invalid");
            fragment.appendChild(item);
        });
        var svg = document.createElementNS("http://www.w3.org/2000/svg", "svg");
        svg.setAttribute("class", "sheet-lines"); svg.setAttribute("width", nativeWidth); svg.setAttribute("height", nativeHeight); svg.setAttribute("aria-hidden", "true");
        documentModel.lines.forEach(function (line) {
            var shape = document.createElementNS(svg.namespaceURI, "line");
            shape.setAttribute("x1", x[line.start.c] + line.start.dx); shape.setAttribute("y1", y[line.start.r] + line.start.dy);
            shape.setAttribute("x2", x[line.end.c] + line.end.dx); shape.setAttribute("y2", y[line.end.r] + line.end.dy);
            shape.setAttribute("stroke", line.color); shape.setAttribute("stroke-width", line.width); svg.appendChild(shape);
        });
        fragment.appendChild(svg);
        documentModel.images.forEach(function (image) {
            var picture = node("img", "sheet-image"); picture.src = image.src; picture.alt = "조직도 이미지"; picture.draggable = false;
            picture.style.left = (x[image.anchor.c] + image.anchor.dx) + "px"; picture.style.top = (y[image.anchor.r] + image.anchor.dy) + "px";
            picture.style.width = image.width + "px"; picture.style.height = image.height + "px"; fragment.appendChild(picture);
        });
        canvas.appendChild(fragment); refreshState(); resize();
        if (calculated.errors.length) message("기존 조직도에 집계 오류가 있습니다. 엑셀에서 계산 후 다시 업로드해 주세요.", true);
    }
    async function upload(file) {
        if (!boot.canUpload || uploading || !file) return;
        if (!/\.xlsx$/i.test(file.name)) { message("엑셀에서 .xlsx 형식으로 저장한 파일을 선택해 주세요.", true); return; }
        if (!file.size || file.size > 10 * 1024 * 1024) { message("비어 있지 않은 10MB 이하 엑셀 파일을 선택해 주세요.", true); return; }
        var data = new FormData(); data.append("workbook", file); data.append("revision", state.revision); data.append("token", boot.token);
        uploading = true; refreshState(); message("엑셀을 업로드하고 있습니다. 완료될 때까지 기다려 주세요.");
        try {
            var response = await fetch(boot.endpoint, { method: "POST", credentials: "same-origin", cache: "no-store", body: data });
            if (response.redirected || (response.headers.get("Content-Type") || "").indexOf("application/json") < 0) throw new Error("로그인 상태를 확인한 뒤 다시 업로드해 주세요.");
            var result = await response.json();
            if (!response.ok || !result.ok || !result.state || !result.state.document) throw new Error(result.message || "엑셀을 업로드하지 못했습니다. 기존 조직도는 유지됩니다.");
            state = result.state; render(); message("엑셀을 업로드했습니다. 조직도가 갱신되었습니다.");
        } catch (error) {
            message(error instanceof TypeError || error instanceof SyntaxError ? "서버 연결을 확인한 뒤 다시 업로드해 주세요. 기존 조직도는 유지됩니다." : error.message || "업로드하지 못했습니다. 기존 조직도는 유지됩니다.", true);
        } finally { uploading = false; id("sheet-workbook").value = ""; refreshState(); }
    }
    id("sheet-upload").addEventListener("click", function () { if (boot.canUpload && !uploading && state.storageReady) id("sheet-workbook").click(); });
    id("sheet-workbook").addEventListener("change", function () { upload(this.files[0]); });
    id("sheet-zoom-out").addEventListener("click", function () { autoFit = false; scale = Math.max(.25, scale - .1); resize(); });
    id("sheet-zoom-in").addEventListener("click", function () { autoFit = false; scale = Math.min(1.5, scale + .1); resize(); });
    id("sheet-fit").addEventListener("click", function () { autoFit = true; resize(); viewport.scrollLeft = 0; });
    id("sheet-print").addEventListener("click", function () { window.print(); });
    var printView;
    window.addEventListener("beforeprint", function () { printView = { scale: scale, autoFit: autoFit }; autoFit = false; scale = Math.min(1, 1450 / nativeWidth); resize(); });
    window.addEventListener("afterprint", function () { if (printView) { scale = printView.scale; autoFit = printView.autoFit; resize(); } });
    window.addEventListener("resize", resize);
    render();
}());
