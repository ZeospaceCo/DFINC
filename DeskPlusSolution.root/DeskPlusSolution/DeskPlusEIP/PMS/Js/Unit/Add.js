Add = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Add.Save();
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            Add.Cancel();
        });

        //책임자명 검색
        $('#cbtnDeptUserTree').click(function (e) {
            e.preventDefault();
            EleDom = "InputDiv1";
            Add.PopupUser('2');
        });

        //관련부서검색
        $('#cbtnDeptUserTree2').click(function (e) {
            e.preventDefault();
            EleDom = "InputDiv3";
            Add.PopupDeptUserTree('1');
        });

        //담당부서검색
        $('#cbtnDeptUserTree1').click(function (e) {
            e.preventDefault();
            EleDom = "InputDiv2";
            Add.PopupDeptUserTree('1');
        });

        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            Add.AttachSwfFile();
        });

        //계획시작일
        $('#cbxPlanStartDate').click(function () {
            if ($('#cbxPlanStartDate').is(":checked")) {
                $("#tbxStartDate").val($('#tbxPlanStartDate').val());
            } else {
                $("#tbxStartDate").val("");
            }
        });

        //계획완료일
        $('#cbxPlanEndDate').click(function () {
            if ($('#cbxPlanEndDate').is(":checked")) {
                $("#tbxEndDate").val($('#tbxPlanEndDate').val());
            } else {
                $("#tbxEndDate").val("");
            }
        });

        Add.GetSpoolFileList();

        $("#tbxPlanStartDate").datepick($.datepicker.regional[PreLang]);
        $("#tbxPlanEndDate").datepick($.datepicker.regional[PreLang]);
        $("#tbxStartDate").datepick($.datepicker.regional[PreLang]);
        $("#tbxEndDate").datepick($.datepicker.regional[PreLang]);

        //        $.datepicker.setDefaults($.extend({ showMonthAfterYear: false }, $.datepicker.regional[preLangReal]));

        //        $("#tbxPlanStartDate").datepicker({
        //            changeMonth: true, changeYear: true
        //        });
        //        $("#tbxPlanEndDate").datepicker({
        //            changeMonth: true,
        //            changeYear: true
        //        });
        //        $("#tbxStartDate").datepicker({
        //            changeMonth: true,
        //            changeYear: true
        //        });
        //        $("#tbxEndDate").datepicker({
        //            changeMonth: true,
        //            changeYear: true
        //        });

        xfe.setHtmlValue($("#tbHiddenMime_contents").val());
        /*
        //ByFreeman 20120106 X-Free Editor - 템플릿 추가
        if (NonActiveX == "T") {
            xfe.setHtmlValue($("#tbHiddenMime_contents").val());
            var agent = navigator.userAgent.toLowerCase();
            if ((navigator.appName == 'Netscape' && navigator.userAgent.search('Trident') != -1) || (agent.indexOf("msie") != -1)) {

            }
            else {
                $("#ActiveX").hide();
            }
            $('#ActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "Add.aspx?" + ClientQueryString;
                }
            });
        } else {
            $('#NonActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "AddN.aspx?" + ClientQueryString;
                }
            });
        }
        */

        __addEventListener(document, "click", Add.click);
    });
});

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 클릭 이벤트 분기 (컨텍스트 메뉴 변경)
***************************************************/
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

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Add.ShowDelContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var gubun = el.getAttribute("type"); //구분자(부서:G, 사용자 M)
    var oid = el.getAttribute("oid"); //코드값
    var objID = el.getAttribute("objID"); //코드값

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    oContextMenuCtrl.addItem(R_Del, "Add.DelItem('" + gubun + "_" + oid + "_" + objID + "')", ImagePath + "/Icon/ContextMenu/del.png");
    oContextMenuCtrl.show(e);
}

function checkForNumber() {
    var key = event.keyCode;

    if (!(key == 8 || key == 9 || key == 13 || key == 46 || key == 144 ||

			   (key >= 48 && key <= 57) || key == 110 || key == 190)) {
        event.returnValue = false;
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 부서/사용자 삭제
***************************************************/
Add.DelItem = function (id) {
    $("#" + id).remove();
    /*
    var oEl = document.getElementById(id);
    if (oEl) {
    oEl.removeNode(true);
    }
    */
}

/**********************************************************************************************
[사용자 추가값] 새창
**********************************************************************************************/
Add.PopupDeptUserTree = function (gubun) {
    ParamArray = {
        'gubun': gubun
    };

    CallEncrypt(CallGetParam());
    var url = "../Module/Popup/GetDept.aspx?p=" + GetEncryptValue;
    PopupWin(350, 530, url, "-500", "", true);
}

/**************************************************
작성자   :
작성일자 : 심재운
작성목적 : 사용자 추가값 가져오기
***************************************************/
function GetDept(rtnVal) {
    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
        var response = unescape(rtnVal);

        var responseStr = response.split("‡");

        sHtml = Add.GetDeptUserDiv(response);

        var targetObj = document.getElementById(EleDom);
        targetObj.innerHTML = sHtml;

        if (EleDom == "InputDiv1") {
            $("#tbxCharger").val(responseStr[1]); //부서명(구분값,)
            $("#tbxHiddenCharger").val(responseStr[0]); //코드값(구분값,)
        }
        else if (EleDom == "InputDiv2") {
            $("#tbxChargeDeptName").val(responseStr[1]);
            $("#tbxHiddenChargerDept").val(responseStr[0]);
        }
        else if (EleDom == "InputDiv3") {
            $("#tbxRelateDeptName").val(responseStr[1]);
            $("#tbxHiddenChargerLink").val(responseStr[0]);
        }
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 부서/사용자 선택하기
***************************************************/
Add.GetDeptUserDiv = function (sList) {
    var targetObj = document.getElementById(EleDom);
    var sHtml = targetObj.innerHTML;
    var sFailueList = "";
    var tmpArr = sList.split("‡");
    var idArr = tmpArr[0].split(",");
    var nameArr = tmpArr[1].split(",");
    var catArr = tmpArr[2].split(",");

    for (var i = 0; i < idArr.length; i++) {
        oid = idArr[i];
        name = nameArr[i];
        gubun = catArr[i];

        if (oid != "") {
            if (!Add.ExistDeptUser(gubun + "_" + oid + "_" + EleDom, EleDom)) {
                if (gubun == "G") {
                    sHtml += "<a href='#' action='del' id=\"G_" + oid + "_" + EleDom + "\" nowrap type='G' oid='" + oid + "'  objID='" + EleDom + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\" title=\"" + name + "\">" + name + "</a>";
                } else {
                    sHtml += "<a href='#' action='del' id=\"M_" + oid + "_" + EleDom + "\" nowrap type='M' oid='" + oid + "'  objID='" + EleDom + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\" title=\"" + name + "\">" + name + "</a>";
                }
            }
            else {
                if (sFailueList.length > 0) sFailueList += "','";
                sFailueList += name;
            }
        }
    }
    if (sFailueList.length > 0)
        alert(MSG_DUF_USERDEPT + "\n\n'" + sFailueList + "'");

    return sHtml;
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 부서/사용자가 선택이 되었는지
***************************************************/
Add.ExistDeptUser = function (oid, target) {
    var targetObj = document.getElementById(target);
    oChildrens = targetObj.childNodes;
    for (var i = 0; i < oChildrens.length; i++) {
        if (oChildrens[i].id == oid)
            return true;
    }
    return false;
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 선택된 부서/사용자 정보 가져오기
***************************************************/
Add.GetList = function (target, tag) {
    var sList = "";
    var targetObj = document.getElementById(target);
    if (targetObj) {
        var oChildrens = targetObj.childNodes;

        var sList = "";
        for (var i = 0; i < oChildrens.length; i++) {
            if (oChildrens[i].tagName == "A") {
                if (sList.length > 0) sList += ",";
                //oChildrens[i].type + "|" +
                //sList += oChildrens[i].getAttribute(tag);
                sList += oChildrens[i].getAttribute(tag);
            }
        }
    }

    return sList;
}

/**********************************************************************************************
[템플릿 선택시]
**********************************************************************************************/
Add.TemplateChange = function () {
    var templateID = $("#ddlTemplate").val();
    if (templateID != "0") {
        if (window.confirm(Unit_Register_LoadTemplate)) {
            ParamArray = {
                'projectID': ProjectID,
                'menuID': MenuID,
                'menuType': MenuType,
                'rqProjectID': RqProjectID,
                'unitID': UnitID,
                'recordPerPage': RecordPerPage,
                'searchTerm': SearchTerm,
                'searchTermValue': SearchTermValue,
                'searchField': SearchField,
                'searchValue': SearchValue,
                'order': Order,
                'status': Status
            };

            CallEncrypt(CallGetParam());

            location.href = "AddN.aspx?p=" + GetEncryptValue;
        }
    }
}

/**********************************************************************************************
[책임자명 새창]
**********************************************************************************************/
Add.PopupUser = function (gubun) {
    //    dialogURL = "../Module/Popup/GetUser.aspx?" + ProjParam + "&ran=" + Guids();
    //    rtnVal = window.showModalDialog(dialogURL, window, "dialogWidth:300px; dialogHeight:470px; status: off; center: Yes; help: No; resizable: No; status: No; scroll:No");

    //    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
    //        var response = unescape(rtnVal);
    //        var responseStr = response.split("‡");
    //        sHtml = Add.GetDeptUserDiv(response);
    //        var targetObj = document.getElementById(EleDom);
    //        targetObj.innerHTML = sHtml;
    //        return;
    //    }

    ParamArray = {
        'projectID': ProjectID
    };

    CallEncrypt(CallGetParam());
    var url = "../Module/Popup/GetUser.aspx?p=" + GetEncryptValue;
    PopupWin(300, 470, url, "-500", "", true);
}

/**********************************************************************************************
[책임자명 새창] 가져오기
**********************************************************************************************/
function SelectedGetUser(rtnVal) {
    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
        var response = unescape(rtnVal);
        var responseStr = response.split("‡");
        sHtml = Add.GetDeptUserDiv(response);
        var targetObj = document.getElementById(EleDom);
        targetObj.innerHTML = sHtml;
        return;
    }
}

/**********************************************************************************************
[저장]
**********************************************************************************************/
Add.Save = function () {
    //에디터 내용 히든박스에 저장
    //ByFreeman 20120106 NonActiveX Test
    if (NonActiveX == "T") {
        $("#tbHiddenHTML_contents").val(xfe.getHtmlValue());
        $("#tbHiddenMime_contents").val("");
        $("#tbHiddenText_contents").val(xfe.getTextValue());
    } else {
        if (!EditorSecurity(form1.twe)) return;

        var strHtml = form1.twe.HtmlValue;
        if (strHtml.indexOf(".js") > -1 || strHtml.indexOf('=\"xss:expression') > -1) {
            strHtml = strHtml.replace(".js\"", "\"");
            strHtml = strHtml.replace('=\"xss:expression', "");

            form1.twe.HtmlValue = strHtml;

            alert(R_DoubtStcipt);
        }

        $("#tbHiddenHTML_contents").val(form1.twe.HtmlValue);
        $("#tbHiddenMime_contents").val(form1.twe.MimeEnValue);
        $("#tbHiddenText_contents").val(form1.twe.TextValue);
    }

    var validator = new Validator($("#form1"));
    validator.checkRequired("tbxTitle", Unit_Register_Request1, true);
    if (!validator.validate()) return;

    var getStartDate = $("#tbxPlanStartDate").val();
    var getEndDate = $("#tbxPlanEndDate").val();

    if (getStartDate == "" || getEndDate == "") {
        alert(R_InputDate);
        return false;
    }
    else {
        var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

        if (dtResponse == false) {
            alert(R_CheckDate);
            return false;
        }
    }

    getStartDate = $("#tbxStartDate").val();
    getEndDate = $("#tbxEndDate").val();

    if (getStartDate == "" || getEndDate == "") {
        alert(R_InputStartDate);
        return false;
    }
    else {
        var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

        if (dtResponse == false) {
            alert(R_CheckEndDate);
            return false;
        }
    }

    form1.hdChargerID.value = Add.GetList("InputDiv1", "oid");
    form1.hdChargetName.value = Add.GetList("InputDiv1", "title");

    form1.hdChargeDeptName.value = Add.GetList("InputDiv2", "oid");
    form1.hdRelateDeptName.value = Add.GetList("InputDiv3", "oid");

    //    alert("hdChargerID : " + form1.hdChargerID.value); //1,56
    //    alert("hdChargetName : " + form1.hdChargetName.value); //책임자 이름
    //    alert("hdChargeDeptName : " + form1.hdChargeDeptName.value); //담당부서명
    //    alert("hdRelateDeptName : " + form1.hdRelateDeptName.value); //관련부서명

    //    return;

    __doPostBack('lbtnWrite', '');
}

/**********************************************************************************************
[취소]
**********************************************************************************************/
Add.Cancel = function () {
    ParamArray = {
        'projectID': ProjectID,
        'menuID': MenuID,
        'menuType': MenuType,
        'rqProjectID': RqProjectID,
        'recordPerPage': RecordPerPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'status': Status
    };

    CallEncrypt(CallGetParam());
    //var url = "Add.aspx?p=" + GetEncryptValue;

    location.href = "JList.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : flash 첨부파일 추가
***************************************************/
Add.AttachSwfFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'BOARD',
        'callBackEvent': "Add.GetSpoolFileList"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : SPOOL 파일  리스트 호출
**********************************************************************************************/
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

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Add.SuccessSpoolFileList = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    setTimeout(function () {
        Add.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
Add.Resource = function () {
    $("#j_spool_fileName")[0].innerHTML = R_FileName;
    $("#j_spool_fileType")[0].innerHTML = R_FileType;
    $("#j_spool_fileLen")[0].innerHTML = R_FileLen;
    $("#j_spool_filedelete")[0].innerHTML = R_FileDel;

    if ($("#j_spool_screenshot")[0] != undefined) {
        $("#j_spool_screenshot")[0].innerHTML = R_ScreenShot;
    }

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoAttachData); }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 임시파일 삭제 처리
**********************************************************************************************/
function DelSpoolPath(fileName) {
    if (window.confirm(R_ApplyDelete)) {
        ParamArray = {
            'fileName': fileName,
            'swfUploadGUID': SwfUploadGUID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Add.SuccessSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}