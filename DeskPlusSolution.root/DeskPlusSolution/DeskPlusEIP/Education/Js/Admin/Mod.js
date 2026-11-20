Mod = {}

/*********************************************************************************************/

$(function () {
    $(document).ready(function () {
        if (!HasEducation) return;

        $('#cbtnReceive').click(function (e) {
            e.preventDefault();

            ParamArray = {
                'callBackEvent': "SetUserDept"
            };

            CallEncrypt(CallGetParam());
            var url = "GetUserDept.aspx?p=" + GetEncryptValue;
            PopupWin(800, 485, url, "-500", "", true);
        });
      
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            Mod.AttachSwfFile();
        });

        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            Mod.Save();
        });

        $('#cbtnList').click(function (e) {
            e.preventDefault();

            if (ReturnViewUrl) window.location.href = ReturnViewUrl;

        });

        $("#tbxStartDate").datepick($.datepicker.regional[PreLang]);
        $("#tbxEndDate").datepick($.datepicker.regional[PreLang]);        

        if (NonActiveX == "T") {
            // 기준 HTML을 확보할 때까지 화면만 숨기고 에디터의 기본 편집 모드는 유지합니다.
            $("#divXFE").css("visibility", "hidden");
            xfe.render("divXFE");
        }

        Mod.SetSpoolFile();
        Mod.GetSpoolFileList();
        $('[data-remove-attach]').click(function (e) {
            e.preventDefault();
            Mod.RemoveExistingFile($(this).attr('data-remove-attach'));
        });

        if (typeof __addEventListener == "function") __addEventListener(document, "click", Mod.click);
        else $(document).click(Mod.click);

    });
});

/********************************************************************************************/
Mod.GetEditorHtmlValue = function (editor) {
    var editorBody = typeof editor.getBody == "function" ? editor.getBody() : null;
    var bodyStyle = editorBody && editorBody.style;
    if (!bodyStyle) return editor.getHtmlValue();

    var height = bodyStyle.height;
    var minHeight = bodyStyle.minHeight;
    var heightPriority = bodyStyle.getPropertyPriority ? bodyStyle.getPropertyPriority("height") : "";
    var minHeightPriority = bodyStyle.getPropertyPriority ? bodyStyle.getPropertyPriority("min-height") : "";
    try {
        return editor.getHtmlValue();
    } finally {
        // 본문 비교용 HTML을 읽어도 편집 영역의 높이와 클릭 가능한 범위는 유지합니다.
        if (bodyStyle.setProperty) {
            bodyStyle.setProperty("height", height, heightPriority);
            bodyStyle.setProperty("min-height", minHeight, minHeightPriority);
        } else {
            bodyStyle.height = height;
            bodyStyle.minHeight = minHeight;
        }
    }
}

/********************************************************************************************/
Mod.InitializeEditor = function (editor) {
    if (Mod.EditorInitializing || Mod.EditorReady) return;
    Mod.EditorInitializing = true;
    Mod.EditorReady = false;
    Mod.EditorInitializationFailed = false;
    Mod.EditorOriginalHtml = $("#tbHiddenHTML_contents").val();
    Mod.EditorOriginalText = $("#tbHiddenText_contents").val();
    Mod.EditorOriginalMime = $("#tbHiddenMime_contents").val();
    var editorHtml = Mod.EditorOriginalHtml || Mod.EditorOriginalMime;
    var attempts = 0;

    try {
        // 읽기 전용 모드를 수동으로 전환하면 XFE 본문의 편집 상태가 잠긴 채 남을 수 있습니다.
        // 화면은 초기화가 끝날 때까지 숨겨져 있으므로 기본 편집 방식으로 내용을 불러옵니다.
        editor.setHtmlValue(editorHtml);
    }
    catch (error) {
        Mod.EditorInitializing = false;
        Mod.EditorInitializationFailed = true;
        return;
    }

    var finishInitialization = function () {
        // XFE의 setHtmlValue는 비동기이며 완료 시 processing을 해제합니다.
        // 임의의 지연 후 값을 기준으로 삼으면 초기화 전/사용자 편집 후 값이 섞일 수 있습니다.
        if (!editor.xfeStackObject || editor.xfeStackObject.processing) {
            if (++attempts < 200) setTimeout(finishInitialization, 100);
            else {
                Mod.EditorInitializing = false;
                Mod.EditorInitializationFailed = true;
            }
            return;
        }
        try {
            Mod.EditorBaselineHtml = Mod.GetEditorHtmlValue(editor);
            if (typeof Mod.EditorBaselineHtml != "string") throw new Error("Invalid editor content.");
            Mod.EditorReady = true;
            Mod.EditorInitializing = false;
            $("#divXFE").css("visibility", "visible");
        }
        catch (error) {
            Mod.EditorInitializing = false;
            Mod.EditorInitializationFailed = true;
        }
    };
    // onLoad 직후에는 XFE 자체 초기화가 processing을 먼저 해제할 수 있습니다.
    // setHtmlValue의 실제 본문 적용(Chrome 300ms, IE/Firefox 400ms) 이후부터 확인해야
    // 빈 기본 문서를 원본 기준으로 잡아 설정만 수정해도 재알림이 발생하는 것을 막습니다.
    setTimeout(finishInitialization, 500);
};

/********************************************************************************************/
Mod.SetSpoolFile = function () {

    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate("");
}

/***************************************************/
Mod.ExistDeptUser = function (oid, target) {
    var targetObj = document.getElementById(target);
    var oChildrens = targetObj.childNodes;
    for (var i = 0; i < oChildrens.length; i++) {
        if (oChildrens[i].id == oid)
            return true;
    }
    return false;
}

/**************************************************/
function SetUserDept(rtnVal) {
    if (rtnVal == null || typeof rtnVal == "undefined" || rtnVal == "") return false;

    var entries = unescape(rtnVal).split(";");
    var selections = [];
    var failures = [];
    var seen = {};
    for (var i = 0; i < entries.length; i++) {
        if (!entries[i]) continue;
        var values = entries[i].split("$");
        if (values.length < 4 || (values[0] != "G" && values[0] != "M")
            || !Mod.IsPositiveID(values[1]) || !Mod.IsPositiveID(values[2])) return false;
        var id = values[0] + "_" + values[1];
        if (seen[id] || Mod.ExistDeptUser(id, "InputTO")) failures.push(values[3]);
        else {
            seen[id] = true;
            selections.push({ id: id, type: values[0],
                oid: values[0] + "_" + (values[0] == "G" ? "0" : values[1]) + "_" + values[2],
                label: values.slice(3).join("$") });
        }
    }
    if (failures.length > 0) {
        alert("이미 등록된 사용자(부서)입니다.\n\n" + failures.join(", "));
        return false;
    }
    if (selections.length == 0) return false;

    for (var j = 0; j < selections.length; j++) {
        var selection = selections[j];
        $("<a></a>").attr({ href: "#", action: "del", id: selection.id,
            type: selection.type, oid: selection.oid, title: selection.label })
            .css({ cursor: "pointer", whiteSpace: "nowrap", lineHeight: "14px", marginRight: "5px" })
            .text(selection.label + ",").appendTo("#InputTO");
    }
    return true;
}

Mod.IsPositiveID = function (value) {
    return /^[0-9]+$/.test(String(value)) && Number(value) > 0 && Number(value) <= 2147483647;
};

Mod.RemoveExistingFile = function (attachID) {
    if (!HasEducation || !Mod.IsPositiveID(ListID) || !Mod.IsPositiveID(attachID)) return;
    if (BtnCheck == "Y" || Mod.DeletingAttachment) {
        alert("처리중입니다.");
        return;
    }
    if (!window.confirm("실제 파일을 삭제하시겠습니까?")) return;

    Mod.DeletingAttachment = true;
    var deleteButton = $("#existing_attach_" + attachID + " [data-remove-attach]");
    deleteButton.attr("aria-disabled", "true");
    try {
        ParamArray = { listID: ListID, attachID: attachID };
        CallEncrypt(CallGetParam());
        $.ajax({
            type: "POST",
            url: "/DeskPlusEIP/Education/Ws/Admin/Mod.asmx/DelAttachFile",
            data: JSON.stringify({ p: GetEncryptValue }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data && data.d === true) $("#existing_attach_" + attachID).remove();
                else alert("공지첨부을 삭제하지 못했습니다. 화면을 새로고침한 후 다시 시도해 주세요.");
            },
            error: function (xhr) {
                // ASMX가 HttpException을 500 JSON 응답으로 전달하는 경우도 확인합니다.
                var response = xhr.responseJSON;
                if (!response && xhr.responseText) {
                    try { response = JSON.parse(xhr.responseText); }
                    catch (error) { }
                }
                var message = response && response.Message ? String(response.Message) : "";
                if (xhr.status == 403 || message.indexOf("공지첨부을 삭제할 권한이 없습니다.") >= 0)
                    alert("공지첨부을 삭제할 권한이 없습니다. 로그인과 권한을 확인해 주세요.");
                else alert("공지첨부 삭제 결과를 확인할 수 없습니다. 화면을 새로고침해 주세요.");
            },
            complete: function () {
                Mod.DeletingAttachment = false;
                deleteButton.removeAttr("aria-disabled");
            }
        });
    }
    catch (error) {
        Mod.DeletingAttachment = false;
        deleteButton.removeAttr("aria-disabled");
        alert("공지첨부 삭제를 요청하지 못했습니다. 다시 시도해 주세요.");
    }
};

Mod.StoreTargetLabels = function () {
    var labels = {};
    $("#InputTO a[oid]").each(function () {
        var label = $(this).text().replace(/,$/, "");
        labels[$(this).attr("oid")] = label;
    });
    $("#hdReceiveLabels").val(JSON.stringify(labels));
};

/****************************************************/
Mod.AttachSwfFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'EDUCATION',
        'callBackEvent': "Mod.GetSpoolFileList"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}

/***********************************************************************************************/
Mod.GetSpoolFileList = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/*********************************************************************************************/
Mod.SuccessSpoolFileList = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);
}

/***********************************************************************************************/
Mod.Resource = function () {
    $("#j_spool_fileName")[0].innerHTML = "파일명";
    $("#j_spool_fileType")[0].innerHTML = "파일타입";
    $("#j_spool_fileLen")[0].innerHTML = "파일크기";
    $("#j_spool_filedelete")[0].innerHTML = "삭제";

    if ($(".NoListData") != undefined) { $(".NoListData").text("공지첨부이 존재하지 않습니다."); }
}

/**********************************************************************************************/
function DelSpoolPath(fileName) {
    if (window.confirm("파일을 삭제하시겠습니까?")) {
        ParamArray = {
            'fileName': fileName,
            'swfUploadGUID': SwfUploadGUID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Mod.SuccessDelSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}

/*********************************************************************************************/
Mod.SuccessDelSpoolFileList = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);
}


/*************************************************/
Mod.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "del": Mod.ShowDelContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

/***************************************************/
Mod.ShowDelContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;
    var tmpID = el.id;

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    oContextMenuCtrl.addNameItem("삭제", "Mod.DelItem('" + tmpID + "')", ImagePath + "/Icon/ContextMenu/del.png");
    var menu = $(oContextMenuCtrl.oEl);
    menu.css("width", "auto");
    menu.find("table").css("width", "auto");
    menu.find("td").css({ padding: "2px 6px", whiteSpace: "nowrap" });
    menu.find("td:last-child").hide();
    oContextMenuCtrl.show(e);
}

/************************************************/
Mod.DelItem = function (id) {
    $("#" + id).remove();
}


/*****************************************************/
Mod.GetList = function (target, tag) {
    var sList = "";
    var targetObj = document.getElementById(target);
    if (targetObj) {
        var oChildrens = targetObj.childNodes;

        var sList = "";
        for (var i = 0; i < oChildrens.length; i++) {
            if (oChildrens[i].tagName == "A") {
                if (sList.length > 0) sList += ",";
                sList += oChildrens[i].getAttribute(tag);
            }
        }
    }

    return sList;
}

/***********************************************************************************************/
Mod.ParseDate = function (value, culture) {
    var normalizedCulture = (culture || "").toLowerCase();
    var dateValue = value || "";
    var dateParts;
    var yearPart;
    var monthPart;
    var dayPart;

    if (normalizedCulture == "en" || normalizedCulture == "en-us") {
        dateParts = dateValue.match(/^(\d{1,2})([-\/.])(\d{1,2})\2(\d{4})$/);
        if (dateParts == null) return null;
        monthPart = dateParts[1];
        dayPart = dateParts[3];
        yearPart = dateParts[4];
    }
    else if (normalizedCulture == "en-gb" || normalizedCulture.indexOf("fr") == 0) {
        dateParts = dateValue.match(/^(\d{1,2})([-\/.])(\d{1,2})\2(\d{4})$/);
        if (dateParts == null) return null;
        dayPart = dateParts[1];
        monthPart = dateParts[3];
        yearPart = dateParts[4];
    }
    else {
        dateParts = dateValue.match(/^(\d{4})([-\/.])(\d{1,2})\2(\d{1,2})$/);
        if (dateParts == null) return null;
        yearPart = dateParts[1];
        monthPart = dateParts[3];
        dayPart = dateParts[4];
    }

    var year = parseInt(yearPart, 10);
    var month = parseInt(monthPart, 10);
    var day = parseInt(dayPart, 10);
    var parsedDate = new Date(year, month - 1, day);

    if (isNaN(year) || isNaN(month) || isNaN(day)
        || parsedDate.getFullYear() != year
        || parsedDate.getMonth() != month - 1
        || parsedDate.getDate() != day) {
        return null;
    }

    return (year * 10000) + (month * 100) + day;
}

/***********************************************************************************************/
Mod.Save = function () {

    if (NonActiveX == "T" && !Mod.EditorReady) {
        alert(Mod.EditorInitializationFailed
            ? "본문을 불러오지 못했습니다. 화면을 새로고침한 후 다시 시도해 주세요."
            : "본문을 불러오는 중입니다. 잠시 후 다시 시도해 주세요.");
        return;
    }

    if (Mod.DeletingAttachment) {
        alert("공지첨부 삭제가 처리 중입니다. 잠시 후 다시 시도해 주세요.");
        return;
    }

    if (BtnCheck == "Y") {
        alert("처리중입니다.");
        return;
    }

    var getStartDate = $("#tbxStartDate").val();
    var getEndDate = $("#tbxEndDate").val();

    if (getStartDate == "" || getEndDate == "") {
        alert("기간을 입력해 주세요.");
        return false;
    }
    else {
        var startDateValue = Mod.ParseDate(getStartDate, PreLangReal);
        var endDateValue = Mod.ParseDate(getEndDate, PreLangReal);

        if (startDateValue == null || endDateValue == null) {
            alert("날짜 형식이 올바르지 않습니다.");
            return false;
        }

        if (endDateValue < startDateValue) {
            alert("기간 설정이 잘못되었습니다");
            return false;
        }
    }

    var getAlarmDay = $("#ddlAlarmDay").val();
    if (!/^\d+$/.test(getAlarmDay)) {
        alert("알람 예정일은 종료일 기준 유효한 일자만 선택 가능합니다.");
        $("#ddlAlarmDay").focus();
        return false;
    }

    var alarmDay = parseInt(getAlarmDay, 10);
    if (isNaN(alarmDay) || alarmDay > 2147483647) {
        alert("알람 예정일은 종료일 기준 유효한 일자만 선택 가능합니다.");
        $("#ddlAlarmDay").focus();
        return false;
    }

    var unchangedAlarmSchedule = typeof OriginalStartDateValue == "number"
        && typeof OriginalEndDateValue == "number" && typeof OriginalAlarmDayValue == "number"
        && startDateValue === OriginalStartDateValue && endDateValue === OriginalEndDateValue
        && alarmDay === OriginalAlarmDayValue;
    // 변경하지 않은 기존 일정은 유지하며 새로 설정한 일정에만 예정일 검증을 적용합니다.
    if (alarmDay > 0 && !unchangedAlarmSchedule) {
        var startDateUtc = Date.UTC(Math.floor(startDateValue / 10000),
            Math.floor(startDateValue / 100) % 100 - 1, startDateValue % 100);
        var endDateUtc = Date.UTC(Math.floor(endDateValue / 10000),
            Math.floor(endDateValue / 100) % 100 - 1, endDateValue % 100);
        var educationDays = (endDateUtc - startDateUtc) / 86400000;

        if (typeof EducationToday == "undefined" || !/^\d{8}$/.test(String(EducationToday))) {
            alert("알람 예정일은 종료일 기준 유효한 일자만 선택 가능합니다.");
            return false;
        }

        var todayText = String(EducationToday);
        var todayValue = Mod.ParseDate(todayText.substr(0, 4) + "-"
            + todayText.substr(4, 2) + "-" + todayText.substr(6, 2), "ko");
        if (todayValue == null) {
            alert("알람 예정일은 종료일 기준 유효한 일자만 선택 가능합니다.");
            return false;
        }

        var todayUtc = Date.UTC(Math.floor(todayValue / 10000),
            Math.floor(todayValue / 100) % 100 - 1, todayValue % 100);
        var remainingDays = (endDateUtc - todayUtc) / 86400000;

        if (remainingDays === alarmDay) {
            alert("알람 예정일은 당일로 선택할 수 없습니다. 해당 교육 내 미완료알람발송 기능을 이용 바랍니다.");
            $("#ddlAlarmDay").focus();
            return false;
        }
        if (!isFinite(educationDays) || educationDays < alarmDay
            || !isFinite(remainingDays) || remainingDays < alarmDay) {
            alert("알람 예정일은 종료일 기준 유효한 일자만 선택 가능합니다.");
            $("#ddlAlarmDay").focus();
            return false;
        }
    }

    $("#hdReceiveID").val(Mod.GetList("InputTO", "oid"));
    Mod.StoreTargetLabels();

    if ($("#hdReceiveID").val() == "") {
        alert("대상자를 추가해주세요.");
        return;
    }

    var validator = new Validator($("#form1"));
    validator.checkRequired("tbxTitle", "과정명을 입력하세요", true);
    if (!validator.validate()) return;

    if (NonActiveX == "T") {
        var currentEditorHtml = Mod.GetEditorHtmlValue(xfe);
        // 에디터의 초기 HTML 정규화만으로 본문 변경 알람이 발생하지 않게 원본을 유지합니다.
        // 실제 변경 여부와 알람 발송 여부는 서버에서 저장 전후 DB 값을 비교해 결정합니다.
        if (currentEditorHtml !== Mod.EditorBaselineHtml) {
            $("#tbHiddenHTML_contents").val(currentEditorHtml);
            $("#tbHiddenMime_contents").val("");
            $("#tbHiddenText_contents").val(xfe.getTextValue());
        }
        else {
            $("#tbHiddenHTML_contents").val(Mod.EditorOriginalHtml);
            $("#tbHiddenMime_contents").val(Mod.EditorOriginalMime);
            $("#tbHiddenText_contents").val(Mod.EditorOriginalText);
        }
    } else {
        if (!EditorSecurity(form1.twe)) return;

        var strHtml = form1.twe.HtmlValue;
        if (strHtml.indexOf(".js") > -1 || strHtml.indexOf('=\"xss:expression') > -1) {
            strHtml = strHtml.replace(".js\"", "\"");
            strHtml = strHtml.replace('=\"xss:expression', "");

            form1.twe.HtmlValue = strHtml;

            alert("본문에 Script 관련 의심구문이 있어 내용이 변경되실 수 있습니다.");
        }

        $("#tbHiddenHTML_contents").val(form1.twe.HtmlValue);
        $("#tbHiddenMime_contents").val(form1.twe.MimeEnValue);
        $("#tbHiddenText_contents").val(form1.twe.TextValue);
    }

    var contents = $("#tbHiddenText_contents").val();
    var contentHtml = $("#tbHiddenHTML_contents").val();
    if (/<img\b/i.test(contentHtml)) {
        contents = "<img"; //dummy (if you save only image in editor)
    }

    if (contents == "") {
        alert("내용이 없습니다.");
        if (NonActiveX == "T" && typeof xfe.setFocus == "function") xfe.setFocus();
        return;
    }

    BtnCheck = "Y";

    try { __doPostBack('lbtnMod', 'MOD'); }
    catch (error) { BtnCheck = "N"; alert("수정을 요청하지 못했습니다. 다시 시도해 주세요."); }
}



