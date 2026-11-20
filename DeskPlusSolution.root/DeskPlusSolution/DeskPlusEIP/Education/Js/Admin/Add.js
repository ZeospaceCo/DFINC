Add = {}

/*********************************************************************************************/

$(function () {
    $(document).ready(function () {

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
            Add.AttachSwfFile();
        });

        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            Add.Save();
        });

        $('#cbtnList').click(function (e) {
            e.preventDefault();

            if (typeof ReturnDashBoardUrl != "undefined" && ReturnDashBoardUrl) {
                window.location.href = ReturnDashBoardUrl;
                return;
            }

            ParamArray = {
                'searchTarget': SearchTarget,
                'searchItem': SearchItem,
                'searchDateValue': SearchDateValue
            };

            CallEncrypt(CallGetParam());

            location = "DashBoard.aspx?p=" + GetEncryptValue;

        });

        $("#tbxStartDate").datepick($.datepicker.regional[PreLang]);
        $("#tbxEndDate").datepick($.datepicker.regional[PreLang]);        

        setTimeout(function () {
            var editorHtml = $("#tbHiddenHTML_contents").val();
            if (!editorHtml) editorHtml = $("#tbHiddenMime_contents").val();
            xfe.setHtmlValue(editorHtml);
        }, 1000);

        Add.SetSpoolFile();

        __addEventListener(document, "click", Add.click);

    });
});

/********************************************************************************************/
Add.SetSpoolFile = function () {

    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate("");
}

/***************************************************/
Add.ExistDeptUser = function (oid, target) {
    var targetObj = document.getElementById(target);
    oChildrens = targetObj.childNodes;
    for (var i = 0; i < oChildrens.length; i++) {
        if (oChildrens[i].id == oid)
            return true;
    }
    return false;
}

/**************************************************/
function SetUserDept(rtnVal) {
    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {

        var targetObj = document.getElementById("InputTO");
        var sHtml = targetObj.innerHTML;
        var sFailueList = "";

        var response = unescape(rtnVal);
        var tmpArr = response.split(";");

        //구분$OwnerID$DeptID$이름
        for (var i = 0; i < tmpArr.length; i++) {
            var tArr = tmpArr[i].split("$");

            var gubun = tArr[0];
            var oid = tArr[1];
            var did = tArr[2];
            var tname = tArr[3];

            if (oid != "") {
                if (!Add.ExistDeptUser(gubun + "_" + oid, "InputTO")) {
                    if (gubun == "G") {
                        sHtml += " <a href='#' action='del' id='G_" + oid + "' nowrap type='" + gubun + "' oid='G_0_" + did + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\" title=\"" + tname + "\">" + tname + ",</a>";
                    }
                    else {
                        sHtml += " <a href='#' action='del' id='M_" + oid+ "' nowrap type='" + gubun + "' oid='M_" + oid + "_" + did + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\" title=\"" + tname + "\">" + tname + ",</a>";
                    }
                }
                else {
                    if (sFailueList.length > 0) sFailueList += "','";
                    sFailueList += tname;
                }
            }
        }

        if (sFailueList.length > 0) {
            alert("이미 등록된 사용자(부서)입니다." + "\n\n'" + sFailueList + "'");
            return false;
        }

        targetObj.innerHTML = sHtml;
        return true;
    }

    return false;
}


/****************************************************/
Add.AttachSwfFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'EDUCATION',
        'callBackEvent': "Add.GetSpoolFileList"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}

/***********************************************************************************************/
Add.GetSpoolFileList = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/*********************************************************************************************/
Add.SuccessSpoolFileList = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);
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
        var obj = eval("Add.SuccessDelSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}

/*********************************************************************************************/
Add.SuccessDelSpoolFileList = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);
}


/*************************************************/
Add.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "del": Add.ShowDelContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

/***************************************************/
Add.ShowDelContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;
    var tmpID = el.id;

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    oContextMenuCtrl.addNameItem("삭제", "Add.DelItem('" + tmpID + "')", ImagePath + "/Icon/ContextMenu/del.png");
    var menu = $(oContextMenuCtrl.oEl);
    menu.css("width", "auto");
    menu.find("table").css("width", "auto");
    menu.find("td").css({ padding: "2px 6px", whiteSpace: "nowrap" });
    menu.find("td:last-child").hide();
    oContextMenuCtrl.show(e);
}

/************************************************/
Add.DelItem = function (id) {
    $("#" + id).remove();
}


/*****************************************************/
Add.GetList = function (target, tag) {
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
Add.ParseDate = function (value, culture) {
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
Add.GetEditorHtmlValue = function () {
    var editorBody = typeof xfe.getBody == "function" ? xfe.getBody() : null;
    var bodyStyle = editorBody && editorBody.style;
    if (!bodyStyle) return xfe.getHtmlValue();

    var height = bodyStyle.height;
    var minHeight = bodyStyle.minHeight;
    var heightPriority = bodyStyle.getPropertyPriority ? bodyStyle.getPropertyPriority("height") : "";
    var minHeightPriority = bodyStyle.getPropertyPriority ? bodyStyle.getPropertyPriority("min-height") : "";
    try {
        return xfe.getHtmlValue();
    } finally {
        // HTML 추출이 편집 영역의 높이를 지워도 이후 클릭·입력을 위한 화면 크기는 유지합니다.
        if (bodyStyle.setProperty) {
            bodyStyle.setProperty("height", height, heightPriority);
            bodyStyle.setProperty("min-height", minHeight, minHeightPriority);
        } else {
            bodyStyle.height = height;
            bodyStyle.minHeight = minHeight;
        }
    }
}

/***********************************************************************************************/
Add.Save = function () {

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
        var startDateValue = Add.ParseDate(getStartDate, PreLangReal);
        var endDateValue = Add.ParseDate(getEndDate, PreLangReal);

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

    if (alarmDay > 0) {
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
        var todayValue = Add.ParseDate(todayText.substr(0, 4) + "-"
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

    $("#hdReceiveID").val(Add.GetList("InputTO", "oid"));

    if ($("#hdReceiveID").val() == "") {
        alert("대상자를 추가해주세요.");
        return;
    }

    var validator = new Validator($("#form1"));
    validator.checkRequired("tbxTitle", "과정명을 입력하세요", true);
    if (!validator.validate()) return;

    if (NonActiveX == "T") {
        $("#tbHiddenHTML_contents").val(Add.GetEditorHtmlValue());
        $("#tbHiddenMime_contents").val("");
        $("#tbHiddenText_contents").val(xfe.getTextValue());
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
    if (contentHtml.indexOf("<IMG") > -1) {
        contents = "<img"; //dummy (if you save only image in editor)
    }

    if (contents == "") {
        alert("내용이 없습니다.");
        if (NonActiveX == "T" && typeof xfe.setFocus == "function") xfe.setFocus();
        return;
    }

    BtnCheck = "Y";

    __doPostBack('lbtnAdd', 'ADD');
}

