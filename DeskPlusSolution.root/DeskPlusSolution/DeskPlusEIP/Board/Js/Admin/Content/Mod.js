Mod = {}

var LinkURLArray = []; //LinkURL

$(function () {

    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Mod.Add();
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            Mod.Cancel();
        });

        //공지사항 등록 선택시, 기간 보이기
        $("input[id=cbOptionTop]").click(function () {
            if (this.checked) {
                $("#ddlDay").show();
            }
            else {
                $("#ddlDay").hide();
            }
        });

        //팝업설정 선택시, 팝업기간 보이기
        $("input[id=cbPopUp]").click(function () {
            if (this.checked) {
                $("#startDate").show();
                $("#endDate").show();
                $("#seperate").text("-");
                $("#cbMainPopUp").attr("checked", false);

                $("#MainPopOption").hide();

            }
            else {
                $("#startDate").hide();
                $("#endDate").hide();
                $("#seperate").text("");
            }
        });

        //메인팝업사용 선택시, 팝업기간 보이기
        $("input[id=cbMainPopUp]").click(function () {
            if (this.checked) {
                $("#startDate").show();
                $("#endDate").show();
                $("#seperate").text("-");
                $("#cbPopUp").attr("checked", false);

                $("#MainPopOption").show();

            }
            else {
                $("#startDate").hide();
                $("#endDate").hide();
                $("#seperate").text("");

                $("#MainPopOption").hide();

            }
        });

        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            Mod.AttachSwfFile();
        });

        //공지사항 등록 활성화 여부
        if ($("#cbOptionTop").is(':checked')) {
            $("#ddlDay").show();
        };

        // 팝업등록 활성화 여부
        if ($("#cbPopUp").is(':checked')) {
            $("#startDate").show();
            $("#endDate").show();
            $("#seperate").text("-");
        };

        // 메인팝업사용 활성화 여부
        if ($("#cbMainPopUp").is(':checked')) {
            $("#startDate").show();
            $("#endDate").show();
            $("#seperate").text("-");

            $("#MainPopOption").show();

        };


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

        // 달력 때문에....
        $('#startDate').focus(function (e) {
            $('#tdDate').css('height', '220px');
        });

        $('#endDate').focus(function (e) {
            $('#tdDate').css('height', '220px');
        });

        $('.chkNum').onlyNumber();

        Mod.Init();

        $(".clsDate").keydown(function (e) { return false; });

        //알람대상자추가
        $('#cbtnAlarmUser').click(function (e) {
            e.preventDefault();
            Mod.AlarmUser();
        });

        //전사추가
        $('#cbtnWhole').click(function (e) {
            e.preventDefault();
            Mod.AlarmUserWhole();
        });

        //링크 URL 추가
        $('#cbtnLinkURL').click(function (e) {
            e.preventDefault();
            Mod.AttachLinkURL();
        });

        //알람대상자 삭제 클릭용
        __addEventListener(document, "click", Mod.click);
    });
});

/**************************************************
작성자   : shine
작성일자 :2011-06-15
작성목적 : 초기화 하기
***************************************************/
Mod.Init = function () {
    $("#startDate").datepick($.datepicker.regional[PreLang]);
    $("#endDate").datepick($.datepicker.regional[PreLang]);

    if (OptionPopUp == "1") {
        $("#tdDate").show();
    }
    else {
        $("#tdDate").hide();
    }

    if (AlarmUser != "") {
        sHtml = Mod.SetUserDeptDiv(AlarmUser);
        var targetObj = document.getElementById("InputAlarmUser");
        targetObj.innerHTML = sHtml;
    }

    if (OptionUseDept == "0" && allUser == "1") {
        $("#trAlarmUser").show();
        $("#trAlarmUserOption").show();
        $("#trAlarmUserOption2").hide();
        
        if (Mod.GetList("InputAlarmUser", "oid").indexOf("G_0_0") != -1) {
            $("#spnAlarmUser").attr("disabled", true);
        }

        if (IsLowRank == "1") {
            $("#cbxAlarmUser").attr("checked", true);
        }
        else {
            $("#cbxAlarmUser").attr("checked", false);
        }
    }
    else {
        $("#trAlarmUser").hide();
        $("#trAlarmUserOption").hide();

        if (OptionUseDept == "0" && (OptionAlarm == "1" || OptionAlarm == "4")) {
            $("#trAlarmUserOption2").show();
        }
        else {
            $("#trAlarmUserOption2").hide();
        }
    }

    if (OptionLinkURL == "1") {
        $("#cbtnLinkURL").show();
        $("#liLinkURL").show();  
    }
    else {
        $("#cbtnLinkURL").hide();
        $("#liLinkURL").hide(); 
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 수정 취소시
***************************************************/
Mod.Cancel = function () {
    ParamArray = {
        'menuCatID': MenuCatID,
        'catID': CatID,
        'categoryID': CategoryID,
        'listID': ListID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchGroup': SearchGroup,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 수정
***************************************************/
Mod.Add = function () {
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

    //팝업설정 시, 기간 설정 확인
    if ($("#cbPopUp").is(':checked') || $("#cbMainPopUp").is(':checked')) {
        var getStartDate = $("#startDate").val();
        var getEndDate = $("#endDate").val();

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

    if (OptionListType != "1") {
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
    $("#hdAlarmUser").val(Mod.GetList("InputAlarmUser", "oid"));
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

    __doPostBack('lbtnWrite', '');
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 파일  리스트 호출
**********************************************************************************************/
Mod.GetAttachFileList = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID,
        'screenShot': ScreenShot
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessAttachFile");
    var url = "/DeskPlusEIP/Board/Ws/Admin/Content/Mod.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
Mod.SuccessAttachFile = function (data, status) {
    if (OptionListType == "1") {
        $('#placeholder_fileList').setTemplateURL('/DeskPlusEIP/Board/Jt/Content/JTDelFile.htm?seq=' + Guids());
    } else {
        $('#placeholder_fileList').setTemplateURL('/DeskPlusEIP/Board/Jt/Content/JTDelScreenFile.htm?seq=' + Guids());
    }

    $('#placeholder_fileList').processTemplate(data.d);

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

    if ($(".j_screenshot") != undefined) { $(".j_screenshot").text(R_ScreenShot); }
   
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
    if (OptionListType == "1") {
        $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Board/Jt/Content/JTSpoolFileL.htm?seq=' + Guids());
    }
    else {
        $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Board/Jt/Content/JTSpoolFile.htm?seq=' + Guids());
    }

    $('#placeholder_file').processTemplate(data.d);

    /*
    RowMaxFile = data.d.length;

    if (RowMax > 0) {
        LinkURLInit();
    }
    */

    Mod.Resource();
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

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 게시판 실제 파일 삭제 처리
**********************************************************************************************/
function DelFile(p_attachFileID) {
    if (window.confirm(R_DelAttach)) {

        ParamArray = {
            'categoryID': CategoryID,
            'listID': ListID,
            'attachFileID': p_attachFileID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Mod.SuccessFileList");
        var url = "/DeskPlusEIP/Board/Ws/Admin/Content/Mod.asmx/DelAttachFile";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
Mod.SuccessFileList = function (data, status) {
    if (data.d == true) {
        Mod.GetAttachFileList();
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 메인팝업으로 등록된 게시물이 있는지 체크
***************************************************/
Mod.GetMainPopUpCount = function () {

    var url = "/DeskPlusEIP/Board/Ws/Content/Mod.asmx/GetMainPopUpCount";


    ParamArray = {
        'catID': CatID,
        'listID': ListID,
        'startDate': $("#startDate").val(),
        'endDate': $("#endDate").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessGetMainPopUpCount");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 게시물 카운트 전역변수에 할당
***************************************************/
Mod.SuccessGetMainPopUpCount = function (data, status) {

    MainPopUpCount = data.d;
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-02-07
작성목적 : 알람 사용자 추가 팝업
***************************************************/
Mod.AlarmUser = function () {

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
작성일자 : 2017-02-07
작성목적 : 전사 추가
***************************************************/
Mod.AlarmUserWhole = function () {

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
작성일자 : 2017-02-07
작성목적 : 사용자 추가값 가져오기 
***************************************************/
function SetUserDept(rtnVal) {
    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {

        //전사가 아닐결우 하위분류포함 선택가능
        $("#spnAlarmUser").attr("disabled", false);

        var targetObj = document.getElementById(EleDom);
        var sHtml = "";

        if (Mod.GetList("InputAlarmUser", "oid").indexOf("G_0_0") == -1) {
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
                if (!Mod.ExistDeptUser(gubun + "_" + oid, EleDom)) {
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
작성일자 : 2017-02-07
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
작성자   : 이지원
작성일자 : 2017-02-07
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
작성자   : 이지원
작성일자 : 2017-02-07
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Mod.ShowDelContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;
    var tmpID = el.id;

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    oContextMenuCtrl.addNameItem(R_Del, "Mod.DelItem('" + tmpID + "')", ImagePath + "/Icon/ContextMenu/del.png");
    oContextMenuCtrl.show(e);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-02-07
작성목적 : 부서/사용자 삭제
***************************************************/
Mod.DelItem = function (id) {
    $("#" + id).remove();
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-02-07
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
                sList += oChildrens[i].getAttribute(tag);
            }
        }
    }

    return sList;
}


/**************************************************
작성자   : 이지원
작성일자 : 2017-02-07
작성목적 : 저장된어 있는 알람대상자 정보 설정하기 
***************************************************/
Mod.SetUserDeptDiv = function (rtnVal, type) {

    var sHtml = "";

    //isGroup + "‡" + userID  + "‡" + deptID + "‡" + name;
    var sList = unescape(rtnVal);
    var tmpArr = sList.split("‡");

    var groupArr = tmpArr[0].split(",");
    var uidArr = tmpArr[1].split(",");
    var didArr = tmpArr[2].split(",");
    var nameArr = tmpArr[3].split(",");

    for (var i = 0; i < uidArr.length; i++) {
        uid = uidArr[i];
        did = didArr[i];
        tname = nameArr[i];
        gubun = groupArr[i];

        if (uid == "0" && did == "0") {
            tname = "전사";
        }

        if (uid != "") {
            if (gubun == "G") {
                sHtml += " <a href='#' action='del' id='G_" + did + "' nowrap type='" + gubun + "' oid='G_0_" + did + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\">" + tname + ",</a>";
            }
            else {
                sHtml += " <a href='#' action='del' id='M_" + uid + "' nowrap type='" + gubun + "' oid='M_" + uid + "_" + did + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\">" + tname + ",</a>";
            }
        }
    }

    return sHtml;
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-03-20
작성목적 : 링크URL 리스트 호출
**********************************************************************************************/
Mod.GetLinkURLList = function () {

    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessLinkURLList");
    var url = "/DeskPlusEIP/Board/Ws/Content/Mod.asmx/GetLinkURLList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-03-20
작성목적 : 링크URL 리스트 보여주기
**********************************************************************************************/
Mod.SuccessLinkURLList = function (data, status) {

    $("#placeholder_URLfileList").html("");

    if (data.d.length > 0) {

        for (var i = 0; i < data.d.length; i++) {

            $("#placeholder_URLfileList").append(
                  "<li>"
                + "<span>" + data.d[i].URLName + " ( " + data.d[i].URL + " )</span>"
                + "<span class='mount'>URL</span>"
                + "<a onclick=Mod.DelInsertRow(" + data.d[i].LinkURLID + ") style='display:inline-block;' title='" + R_Del + "'><i class='fas fa-times-circle'></i></a>"
                + "</li>"
             );
        }

    }
    else {
        $("#liLinkURLList").hide();
    }  

}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-03-20
작성목적 : 링크 URL 데이터 삭제
**********************************************************************************************/
Mod.DelInsertRow = function (p_linkURLID) {

    if (window.confirm(R_DelAttach)) {

        ParamArray = {
            'categoryID': CategoryID,
            'listID': ListID,
            'linkURLID': p_linkURLID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Mod.SuccessDelInsertRow");
        var url = "/DeskPlusEIP/Board/Ws/Content/Mod.asmx/DelLinkURL";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-03-20
작성목적 : 링크 URL 삭제 데이터 성공시
**********************************************************************************************/
Mod.SuccessDelInsertRow = function (data, status) {
    if (data.d == true) {
        //Mod.GetAttachFileList();
        Mod.GetLinkURLList();
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-03-20
작성목적 : 링크URL 링크처리
**********************************************************************************************/
Mod.LinkURL = function (linkUrl) {

    if (linkUrl.indexOf("http://") == -1) {
        linkUrl = "http://" + linkUrl;
    }

    window.open(linkUrl);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-20
작성목적 : 링크URL 임시 추가 링크
***************************************************/
Mod.AttachLinkURL = function () {
    var url = "../../Content/LinkURL.aspx";
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "LinkURL", "500", "223")
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-20
작성목적 : 링크URL 임시 추가시
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
        + "<a onclick=Mod.DelRow(" + RowSeq + ") style='display:inline-block;' title='" + R_Del + "'><i class='fas fa-times-circle'></i></a>"
        + "</li>"
    );

    Mod.AddLinkURLArray(RowSeq, _tmpLinkURLName, _tmpLinkURL);
    
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-17
작성목적 : 링크URL 임시 특정 ROW 삭제
***************************************************/
Mod.DelRow = function (index) {

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
            Mod.AttchLinkURLResoList();
        }
        */
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-03-20
작성목적 : 링크URL 임시 리스트 초기화 보여주기 - 사용X
**********************************************************************************************/
Mod.AttchLinkURLResoList = function () {
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
작성목적 : 링크URL 배열값저장
***************************************************/
Mod.AddLinkURLArray = function (id, name, url) {
    LinkURLArray.push({
        'id': id,
        'name': name,
        'url': url
    });
}
