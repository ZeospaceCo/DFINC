/// <reference path="../../../Html/Js/jQuery/jquery.js" />

Reply = {}

var LinkURLArray = []; //LinkURL

/**************************************************
작성자   : shimpark
작성목적 : 초기화
***************************************************/
$(function () {

    $(document).ready(function () {
        //공지사항 등록 선택시, 기간 보이기
        $("input[id=cbOptionTop]").click(function () {
            if (this.checked) {
                $("#ddlDay").show();
            }
            else {
                $("#ddlDay").hide();
            }
        });

        //임시 게시물 추가
        $('#cbtnTemp').click(function (e) {
            e.preventDefault();
            Reply.Write("Temp");
        });

        //게시물 추가
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Reply.Write("Register");
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            Reply.Cancel();
        });

        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            Reply.AttachSwfFile();
        });

        //권한버튼 클릭
        $('#cbtnInfo').click(function () {
            Reply.AuthInfo();
        });

        //알람대상자추가
        $('#cbtnAlarmUser').click(function (e) {
            e.preventDefault();
            Reply.AlarmUser();
        });

        //전사추가
        $('#cbtnWhole').click(function (e) {
            e.preventDefault();
            Reply.AlarmUserWhole();
        });

        $('#cbtnSwfFileAdd2').click(function (e) {
            e.preventDefault();
            $('html, body').scrollTop(document.body.scrollHeight)
            Reply.AttachSwfFile();
        });

        if (OptionUseDept == "0" && allUser == "1") {
            $("#trAlarmUser").show();
            $("#trAlarmUserOption").show();
            $("#cbxAlarmUser").attr("checked", true);

            if (OptionAlarm == "1" || OptionAlarm == "4") {
                $("#spnAlarmUser").attr("disabled", true);
                Reply.AlarmUserWhole();
            }
        }
        else {
            $("#trAlarmUser").hide();
            $("#trAlarmUserOption").hide();
            $("#cbxAlarmUser").attr("checked", false);
        }

        //링크 URL 추가
        $('#cbtnLinkURL').click(function (e) {
            e.preventDefault();
            Reply.AttachLinkURL();
        });

        if (OptionLinkURL == "1") {
            $("#cbtnLinkURL").show();
            $("#liLinkURL").show();       
        }
        else {
            $("#cbtnLinkURL").hide();
            $("#liLinkURL").hide();       
        }

        //알람대상자 삭제 클릭용
        __addEventListener(document, "click", Reply.click);

        Reply.GetSpoolFileList();

        if (UseReplyBeforeContent == "Y") {
            xfe.setHtmlValue($("#contents").val());
        }
        else {
            xfe.setHtmlValue($("#tbHiddenMime_contents").val());
        }

        /*
        //ByFreeman 20120106 X-Free Editor - 템플릿 추가
        if (NonActiveX == "T") {
            if (UseReplyBeforeContent == "Y") {
                xfe.setHtmlValue($("#contents").val());
            }
            else {
                xfe.setHtmlValue($("#tbHiddenMime_contents").val());
            }
            var agent = navigator.userAgent.toLowerCase();
            if ((navigator.appName == 'Netscape' && navigator.userAgent.search('Trident') != -1) || (agent.indexOf("msie") != -1)) {

            }
            else {
                $("#ActiveX").hide();
            }
            $('#ActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "Reply.aspx?" + ClientQueryString;
                }
            });
        } else {
            $('#NonActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "ReplyN.aspx?" + ClientQueryString;
                }
            });
        }
        */

    });
});

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 게시물 추가
***************************************************/
Reply.Write = function (_arg) {
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
    validator.checkRequired("ctbName", R_Add_InputTitle, true);
    if (!validator.validate()) return;

    //공지설정 시, 기간 설정 확인
    if ($("#cbOptionTop").is(':checked')) {
        validator.checkRequired("ddlDay", R_Add_SelectOptionTop, true);
    }

    var contents = $("#tbHiddenText_contents").val();
    var contentHtml = $("#tbHiddenHTML_contents").val();
    if (contentHtml.indexOf("<IMG") > -1) {
        contents = "<img"; //dummy (if you save only image in editor)
    }

    if (contents == "") {
        alert(R_Add_InputContent);
        return;
    }

    if (OptionListType != "1" && GoPage.indexOf("/List.aspx") == -1) {
        var imageRex = false;
        var screenShotVal = $(".radioSpoolFile:checked").val();

        if (screenShotVal != "" && screenShotVal != undefined) {
            var arr = new Array('.bmp', '.tif', '.jpeg', '.jpg', '.gif', '.png', '.pjpeg', '.pjpg');

            for (i = 0; i <= arr.length - 1; i++) {
                if (arr[i] == screenShotVal.toLowerCase().indexOf(arr[i]) != -1) {
                    imageRex = true;
                    break;
                }
            }

            if (imageRex == true) {
                $("#tbScreenShot").val(screenShotVal);
            }
            else {
                alert(R_NotImageFormat);
                return;
            }
        }
    }

    //알람발송자 설정
    $("#hdAlarmUser").val(Reply.GetList("InputAlarmUser", "oid"));
    $("#spnAlarmUser").attr("disabled", false);

    //링크URL 저장처리
    var arrhiddenLinkURLName = "";
    var arrhiddenLinkURL = "";

    if (LinkURLArray.length != 0) {
        for (var i = 0; i < LinkURLArray.length; i++) {
            arrhiddenLinkURLName += LinkURLArray[i].name + "|";
            arrhiddenLinkURL += LinkURLArray[i].url + "|";
        }
    }

    $("#hiddenLinkURLName").val(arrhiddenLinkURLName);
    $("#hiddenLinkURL").val(arrhiddenLinkURL);

    if (TrySaveCnt == 0) {
        __doPostBack('lbtnWrite', _arg);
        TrySaveCnt += 1;
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 게시물 추가 취소시
***************************************************/
Reply.Cancel = function () {
    ParamArray = {
        'dcparm': Dcparm,
        'menuCatID': MenuCatID,
        'catID': CatID,
        'categoryID': CategoryID,
        'currentPage': CurrentPage,
        'searchGroup': SearchGroup,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'pGubun': PGubun
    };

    CallEncrypt(CallGetParam());
    location.href = GoPage + "?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : flash 첨부파일 추가
***************************************************/
Reply.AttachSwfFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'BOARD',
        'callBackEvent': "Reply.GetSpoolFileList"
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
Reply.GetSpoolFileList = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Reply.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Reply.SuccessSpoolFileList = function (data, status) {
    if (OptionListType == "1" || GoPage.indexOf("/List.aspx") != -1) {
        $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Board/Jt/Content/JTSpoolFileL.htm?seq=' + Guids());
    }
    else {
        $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Board/Jt/Content/JTSpoolFile.htm?seq=' + Guids());
    }

    $('#placeholder_file').processTemplate(data.d);

    setTimeout(function () {
        Reply.Resource();
    }, 100);

    /*
    if (RowMax > 0) {
        LinkURLInit();
    }  
    */
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
Reply.Resource = function () {
    if ($(".j_screenshot") != undefined) { $(".j_screenshot").text(R_ScreenShot); }

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
        var obj = eval("Reply.SuccessSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2017-03-07
작성목적 : 권한보여주기
**********************************************************************************************/
Reply.AuthInfo = function () {
    
    ParamArray = {
        'categoryID': CatID
    }

    CallEncrypt(CallGetParam());

    var url = "AuthInfo.aspx?p=" + GetEncryptValue;
    PopupWin(500, 395, url, "-320", "0", false);

}

/**************************************************
작성자   : 이지원
작성일자 : 2017-05-12
작성목적 : 알람 사용자 추가 팝업
***************************************************/
Reply.AlarmUser = function () {    

    EleDom = "InputAlarmUser";

    ParamArray = {
        'callBackEvent': "SetUserDept"
    };

    CallEncrypt(CallGetParam());

    var url = "";

    if (BoaComID == "0") {
        url = "/DeskPlusEIP/Board/Common/GetUserDeptAll.aspx?p=" + GetEncryptValue;
    }
    else {
        url = "/DeskPlusEIP/Board/Common/GetUserDept.aspx?p=" + GetEncryptValue;
    }

    PopupWin(800, 500, url, "-500", "", true);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-05-12
작성목적 : 전사 추가
***************************************************/
Reply.AlarmUserWhole = function () {

    EleDom = "InputAlarmUser";

    var targetObj = document.getElementById(EleDom);

    var gubun = "G";
    var oid = "0";
    var did = "0";
    var tname = Whole;

    var sHtml = " <a href='#' action='del' id='G_" + oid + "' nowrap type='" + gubun + "' oid='G_0_" + did + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\">" + tname + ",</a>";

    targetObj.innerHTML = sHtml;

    $("#cbxAlarmUser").attr("checked", true);
    $("#spnAlarmUser").attr("disabled", true);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-05-12
작성목적 : 사용자 추가값 가져오기 
***************************************************/
function SetUserDept(rtnVal) {    

    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {

        //전사가 아닐결우 하위분류포함 선택가능
        $("#spnAlarmUser").attr("disabled", false);

        var targetObj = document.getElementById(EleDom);
        var sHtml = "";

        if (Reply.GetList("InputAlarmUser", "oid").indexOf("G_0_0") == -1) {
            sHtml = targetObj.innerHTML;
        }

        var sFailueList = "";

        var response = unescape(rtnVal);
        var tmpArr = response.split(";");

        //구분$OwnerID$DeptID$이름
        for (var i = 0; i < tmpArr.length; i++) {
            var tArr = tmpArr[i].split("$");

            gubun = tArr[0];
            oid = tArr[1];
            did = tArr[2];
            tname = tArr[3];

            if (oid != "") {
                if (!Reply.ExistDeptUser(gubun + "_" + oid, EleDom)) {
                    if (gubun == "G") {
                        sHtml += " <a href='#' action='del' id='G_" + oid + "' nowrap type='" + gubun + "' oid='G_0_" + did + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\">" + tname + ",</a>";
                    }
                    else {
                        sHtml += " <a href='#' action='del' id='M_" + oid + "' nowrap type='" + gubun + "' oid='M_" + oid + "_" + did + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\">" + tname + ",</a>";
                    }
                }
                else {
                    if (sFailueList.length > 0) sFailueList += "','";
                    sFailueList += tname;
                }
            }
        }

        targetObj.innerHTML = sHtml;

        if (sFailueList.length > 0)
            alert(R_AleadyRegisted + "\n\n'" + sFailueList + "'");
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-05-12
작성목적 : 부서/사용자가 선택이 되었는지
***************************************************/
Reply.ExistDeptUser = function (oid, target) {
    var targetObj = document.getElementById(target);
    oChildrens = targetObj.childNodes;
    for (var i = 0; i < oChildrens.length; i++) {
        if (oChildrens[i].id == oid)
            return true;
    }
    return false;
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-05-12
작성목적 : 클릭 이벤트 분기 (컨텍스트 메뉴 변경)
***************************************************/
Reply.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "del": Reply.ShowDelContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

/**************************************************
작성자   : 이지원
작성일자 : 2017-05-12
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Reply.ShowDelContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;
    var tmpID = el.id;

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    oContextMenuCtrl.addNameItem(R_Del, "Reply.DelItem('" + tmpID + "')", ImagePath + "/Icon/ContextMenu/del.png");
    oContextMenuCtrl.show(e);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-05-12
작성목적 : 부서/사용자 삭제
***************************************************/
Reply.DelItem = function (id) {
    $("#" + id).remove();

    $("#hdAlarmUser").val(Reply.GetList("InputAlarmUser", "oid"));
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-05-12
작성목적 : 선택된 부서/사용자 정보 가져오기
***************************************************/
Reply.GetList = function (target, tag) {
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


/**************************************************
작성자   : 이지원
작성일자 : 2017-03-17
작성목적 : 링크URL 추가 링크
***************************************************/
Reply.AttachLinkURL = function () {
    var url = "LinkURL.aspx";
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "LinkURL", "500", "223")
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-17
작성목적 : 링크URL 추가시
***************************************************/
function SuccessLinkURL(_tmpLinkURLName, _tmpLinkURL) {

    //var row = $("#NoListData");
    //row.remove();

    //RowMax = RowMax + 1;
    RowSeq = RowSeq + 1;

    $("#placeholder_URLfile").append(
          "<li id='LinkURLAdd" + RowSeq + "'>"
        + "<span>" + _tmpLinkURLName + " ( " + _tmpLinkURL + " )</span>"
        + "<span class='mount'>URL</span>"
        + "<a onclick=Reply.DelRow(" + RowSeq + ") style='display:inline-block;' title='" + R_Del + "'><i class='fas fa-times-circle'></i></a>"
        + "</li>"
    );

    Reply.AddLinkURLArray(RowSeq, _tmpLinkURLName, _tmpLinkURL);
    
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-17
작성목적 : 링크URL 특정 ROW 삭제
***************************************************/
Reply.DelRow = function (index) {

    if (window.confirm(R_ApplyDelete)) {
        var row = $("#LinkURLAdd" + index);

        for (var i = 0; i < LinkURLArray.length; i++) {
            if (LinkURLArray[i].id == index) {
                LinkURLArray.splice(i, 1);
                row.remove();
                //RowMax = RowMax - 1;
                break;
            }
        }

        /*
        if (RowMax == 0 && RowMaxFile == 0) {
            Reply.AttchLinkURLResoList();
        }
        */
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-03-20
작성목적 : 링크URL 리스트 초기화 보여주기
**********************************************************************************************/
Reply.AttchLinkURLResoList = function () {
    $("#contentTable").append(
		"<tr id='NoListData'>"
			+ "<td colspan='4' class='center'>"
			+ R_NoContent
			+ "</td>"
		+ "</tr>"
	);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-20
작성목적 : 배열값저장
***************************************************/
Reply.AddLinkURLArray = function (id, name, url) {
    LinkURLArray.push({
        'id': id,
        'name': name,
        'url': url
    });
}

