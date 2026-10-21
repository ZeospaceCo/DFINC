Mod = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Mod.Save();
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            Mod.Cancel();
        });

        //책임자명 검색
        $('#cbtnDeptUserTree').click(function (e) {
            e.preventDefault();
            EleDom = "InputDiv1";
            Mod.PopupUser('2');
        });

        //관련부서검색
        $('#cbtnDeptUserTree2').click(function (e) {
            e.preventDefault();
            EleDom = "InputDiv3";
            Mod.PopupDeptUserTree('1');
        });

        //담당부서검색
        $('#cbtnDeptUserTree1').click(function (e) {
            e.preventDefault();
            EleDom = "InputDiv2";
            Mod.PopupDeptUserTree('1');
        });

        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            Mod.AttachSwfFile();
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

        Mod.GetSpoolFileList();

        $("#tbxPlanStartDate").datepick($.datepicker.regional[PreLang]);
        $("#tbxPlanEndDate").datepick($.datepicker.regional[PreLang]);
        $("#tbxStartDate").datepick($.datepicker.regional[PreLang]);
        $("#tbxEndDate").datepick($.datepicker.regional[PreLang]);

        xfe.setHtmlValue($("#contents").val());
        /*
        //ByFreeman 20120106 X-Free Editor - 템플릿 추가
        if (NonActiveX == "T") {
            xfe.setHtmlValue($("#contents").val());
            var agent = navigator.userAgent.toLowerCase();
            if ((navigator.appName == 'Netscape' && navigator.userAgent.search('Trident') != -1) || (agent.indexOf("msie") != -1)) {

            }
            else {
                $("#ActiveX").hide();
            }
            $('#ActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "Mod.aspx?" + ClientQueryString;
                }
            });
        } else {
            $('#NonActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "ModN.aspx?" + ClientQueryString;
                }
            });
        }
        */

        Mod.Init();

        __addEventListener(document, "click", Mod.click);
    });
});

/**********************************************************************************************
초기화
**********************************************************************************************/
Mod.Init = function () {
    //수정모드일경우
    if (UnitID != "0") {
        if (chargerName != "") {
            EleDom = "InputDiv1";
            sHtml = Mod.GetDeptUserDiv(chargerName);
            var targetObj = document.getElementById("InputDiv1");
            targetObj.innerHTML = sHtml;
        }

        if (linkedDeptName != "") {
            EleDom = "InputDiv3";
            sHtml = Mod.GetDeptUserDiv(linkedDeptName);
            var targetObj = document.getElementById("InputDiv3");
            targetObj.innerHTML = sHtml;
        }

        if (chargerDeptName != "") {
            EleDom = "InputDiv2";
            sHtml = Mod.GetDeptUserDiv(chargerDeptName);
            var targetObj = document.getElementById("InputDiv2");
            targetObj.innerHTML = sHtml;
        }
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 클릭 이벤트 분기 (컨텍스트 메뉴 변경)
***************************************************/
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

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Mod.ShowDelContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var gubun = el.getAttribute("type"); //구분자(부서:G, 사용자 M)
    var oid = el.getAttribute("oid"); //코드값
    var objID = el.getAttribute("objID"); //코드값

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    oContextMenuCtrl.addItem(R_Del, "Mod.DelItem('" + gubun + "_" + oid + "_" + objID + "')", ImagePath + "/Icon/ContextMenu/del.png");
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
Mod.DelItem = function (id) {
    $("#" + id).remove();
    /*
    var oEl = document.getElementById(id);
    if (oEl) {
    oEl.removeNode(true);
    }
    */
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 부서/사용자 선택하기
***************************************************/
Mod.GetDeptUserDiv = function (sList) {
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
            if (!Mod.ExistDeptUser(gubun + "_" + oid + "_" + EleDom, EleDom)) {
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
Mod.ExistDeptUser = function (oid, target) {
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
Mod.GetList = function (target, tag) {
    var sList = "";
    var targetObj = document.getElementById(target);
    if (targetObj) {
        var oChildrens = targetObj.childNodes;

        var sList = "";
        for (var i = 0; i < oChildrens.length; i++) {
            if (oChildrens[i].tagName == "A") {
                if (sList.length > 0) sList += ",";
                //oChildrens[i].type + "|" +
                sList += oChildrens[i].getAttribute(tag);
            }
        }
    }
    return sList;
}

/**********************************************************************************************
[템플릿 선택시]
**********************************************************************************************/
Mod.TemplateChange = function () {
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

            location.href = "ModN.aspx?p=" + GetEncryptValue;
        }
    }
}

/**********************************************************************************************
[책임자명 리스트]
**********************************************************************************************/
Mod.PopupUser = function (gubun) {
    //    dialogURL = "../Module/Popup/GetUser.aspx?" + ProjParam + "&ran=" + Guids();
    //	rtnVal = window.showModalDialog (dialogURL, window, "dialogWidth:300px; dialogHeight:470px; status: off; center: Yes; help: No; resizable: No; status: No; scroll:No");
    //
    //	if(rtnVal != "undefined"  && unescape(rtnVal) != "" && rtnVal != null)
    //	{
    //	    var response = unescape(rtnVal);
    //	    var responseStr = response.split("‡");
    //
    //
    //        sHtml = Mod.GetDeptUserDiv(response);
    //
    //        var targetObj = document.getElementById(EleDom);
    //        targetObj.innerHTML = sHtml;
    //        return;
    //	}

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
        sHtml = Mod.GetDeptUserDiv(response);
        var targetObj = document.getElementById(EleDom);
        targetObj.innerHTML = sHtml;
        return;
    }
}

/**********************************************************************************************
[조직도]
**********************************************************************************************/
Mod.PopupDeptUserTree = function (gubun) {
    //    ParamArray = {
    //        'gubun': gubun
    //    };

    //    CallEncrypt(CallGetParam());
    //    var url = "../Module/Popup/GetDept.aspx?p=" + GetEncryptValue;
    //    PopupWin(300, 470, url, "-500", "", true);

    //    rtnVal = window.showModalDialog(dialogURL, window, "dialogWidth:300px; dialogHeight:470px; status: off; center: Yes; help: No; resizable: No; status: No; scroll:No");

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

        sHtml = Mod.GetDeptUserDiv(response);

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

/**********************************************************************************************
[저장]
**********************************************************************************************/
Mod.Save = function () {
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
    if (!validator.validate()) {
        $("#tbxTitle").select();
        return;
    }

    validator.checkRequired("tbxPlanStartDate", Unit_Register_Request2, true);
    if (!validator.validate()) {
        $("#tbxPlanStartDate").select();
        return;
    }

    validator.checkRequired("tbxPlanEndDate", Unit_Register_Request3, true);
    if (!validator.validate()) {
        $("#tbxPlanEndDate").select();
        return;
    }

    var getStartDate = $("#tbxStartDate").val();
    var getEndDate = $("#tbxEndDate").val();

    if (getStartDate == "" || getEndDate == "") {
        alert(R_ApplyRegDate);
        return false;
    }
    else {
        var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

        if (dtResponse == false) {
            alert(R_StartDateEndDate);
            return false;
        }
    }

    form1.hdChargerID.value = Mod.GetList("InputDiv1", "oid");
    form1.hdChargetName.value = Mod.GetList("InputDiv1", "title");

    form1.hdChargeDeptName.value = Mod.GetList("InputDiv2", "oid");
    form1.hdRelateDeptName.value = Mod.GetList("InputDiv3", "oid");

    __doPostBack('lbtnWrite', '');
}

/**********************************************************************************************
[취소]
**********************************************************************************************/
Mod.Cancel = function () {
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
Mod.AttachSwfFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'BOARD',
        'callBackEvent': "Mod.GetSpoolFileList"
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

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Mod.SuccessSpoolFileList = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    setTimeout(function () {
        Mod.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
Mod.Resource = function () {
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
        var obj = eval("Mod.SuccessSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}