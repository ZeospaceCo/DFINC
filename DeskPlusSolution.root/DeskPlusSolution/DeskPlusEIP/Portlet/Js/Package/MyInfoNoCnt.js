
$(document).ready(function () {

    var portletDom = myInfo_tabPortletID1 + "_" + myInfo_portletID1;
   
    if (myInfo_IsAuth1 != "True") {
        $("#basicMenu_" + portletDom + " .del").remove();

    }

    //최소화 여부
    if (myInfo_IsMini1 == "1") {
        $("#ptContent_" + portletDom).fadeOut("slow");
    }

    $("#ptConfig_" + portletDom).contextMenu({ menu: "basicMenu_" + portletDom, leftButton: true }, function (action, el, pos) {

        var pID = el[0].id.replace('ptConfig_', '');
        MyInfo_ContextMenuWork(pID, action, el, pos);
    });

    //새로고침
    $("#ptReload_" + portletDom).on("click", function (e) {
        e.preventDefault();


        setTimeout(function () {
            //함수 호출
            MyInfo_GetContent();

        }, 1);
    });

    //겸직 변경하기
    $("#ChangeDept_" + portletDom).toggle(

        function () {
            $("#myUserDept_" + portletDom).show();
        },
        function () {
            $("#myUserDept_" + portletDom).hide();
        }
    );

    $("#ptPreview_" + portletDom).on("click", function (e) {
        e.preventDefault();

        Main.Popup();
    });

    $("#ptListConfig_" + portletDom).hide();

    //setTimeout(function () {
    //MyInfo_GetContent();
    //}, 1);

});


function MyInfo_Event() {
    //메일 바로가기
    $("#myInfo_NewMail").on("click", function (e) {
        e.preventDefault();
        parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webmail";
    });

    //일정바로가기
    $("#myInfo_TodayS").on("click", function (e) {
        e.preventDefault();
        parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webpims";
        //parent.frmTop.Menu.GoParam("pim");
    });

    //할일바로가기
    $("#myInfo_TodayT").on("click", function (e) {
        e.preventDefault();
        parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webpims";
        //parent.frmTop.Menu.GoParam("pim");
    });

    //업무협업바로가기
    $("#myInfo_Task").on("click", function (e) {
        e.preventDefault();
        parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=TASK";
        //parent.frmTop.Menu.GoParam("pim");
    });

    //전자결재바로가기
    $("#myInfo_AppDoc").on("click", function (e) {
        e.preventDefault();
        ParamArray = { 'folderStatusID': 'Process' };
        CallEncrypt(CallGetParam());
        document.location.href = MyInfo_GetApprovalStatusMenuUrl() + "?p=" + GetEncryptValue;
        parent.frmTop.Menu.GoParam("approvalStatus");

        //parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=approvalStatus";
    });

    //전자결재바로가기
    $("#myInfo_AppApp").on("click", function (e) {
        e.preventDefault();
        ParamArray = { 'folderStatusID': 'Approve' };
        CallEncrypt(CallGetParam());
        document.location.href = MyInfo_GetApprovalStatusMenuUrl() + "?p=" + GetEncryptValue;
        parent.frmTop.Menu.GoParam("approvalStatus");
        //parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=approvalStatus";
    });

    //전자결재바로가기
    $("#myInfo_AppDept").on("click", function (e) {
        e.preventDefault();
        ParamArray = { 'folderStatusID': 'Proxy' };
        CallEncrypt(CallGetParam());
        document.location.href = MyInfo_GetApprovalStatusMenuUrl() + "?p=" + GetEncryptValue;
        parent.frmTop.Menu.GoParam("approvalStatus");
        //parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=approvalStatus";
    });

    //전자결재바로가기
    $("#myInfo_AppCom").on("click", function (e) {
        e.preventDefault();
        ParamArray = { 'folderStatusID': 'Receive' };
        CallEncrypt(CallGetParam());
        document.location.href = MyInfo_GetApprovalStatusMenuUrl() + "?p=" + GetEncryptValue;
        parent.frmTop.Menu.GoParam("approvalStatus");
        //parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=approvalStatus";
    });

}

/**************************************************
작성자: freeman
작성일:2012-02-07
작성목적 : 포틀릿 Context Menu
***************************************************/
function MyInfo_ContextMenuWork(id, action, el, pos) {
    switch (action) {
        case "del":
            {
                Main.DelTabPortlet(id);
                if (PortletAuth == "True") {
                    $("#portlet_" + id).fadeOut("slow");
                }
                break;
            }
        case "min":
            {
                Main.ModMinMaxPortlet(myInfo_tabPortletID1, myInfo_portletID1, "1");
                $("#ptContent_" + id).fadeOut("slow");
                break;
            }
        case "max":
            {
                Main.ModMinMaxPortlet(myInfo_tabPortletID1, myInfo_portletID1, "0");
                $("#ptContent_" + id).fadeIn("slow");
                break;
            }
        case "info":
            {
                alert(R_NoSettingInfo);
                break;
            }
    }
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 
**********************************************************************************************/
function MyInfo_GetContent() {
    var url = "/DeskPlusEIP/Portlet/Ws/Package/MyInfo.asmx/GetMyInfo";
    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
    };

    var obj = eval("MyInfo_SuccessGetContent");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 
**********************************************************************************************/
function MyInfo_SuccessGetContent(data, status) {
    $(".mytoday").hide();

    var objList = data.d;
    $.each(objList, function (index, objListInfo) {
        var cnt = Number(objListInfo.NewMailCnt);
        cnt += Number(objListInfo.TodayPimSchCnt);
        //cnt += Number(objListInfo.TodayPimDoCnt);
        cnt += Number(objListInfo.TaskCnt);
        cnt += Number(objListInfo.DocmentCnt);
        cnt += Number(objListInfo.ApprovalCnt);
        cnt += Number(objListInfo.DeptSusinCnt);
        cnt += Number(objListInfo.CompleteCnt);

        if (cnt > 0) {
            $(".mytoday").show();
        }
    });

    var portletDom = myInfo_tabPortletID1 + "_" + myInfo_portletID1;

    $("#ptList_" + portletDom).setTemplateURL('/DeskPlusEIP/Portlet/Jt/Package/JTMyInfo.htm?seq=' + Guids(), null, { filter_data: false });
    $("#ptList_" + portletDom).processTemplate(data.d);

    MyInfo_Resource();

    MyInfo_Event();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 다국어 
**********************************************************************************************/

function MyInfo_Resource() {

    if ($(".r_count") != undefined) {
        $(".r_count").text(R_Count);
    }


    $("ul.myinfo li.mynew ul.mynewlist li.my1").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to1.gif)");
    $("ul.myinfo li.mynew ul.mynewlist li.my1on").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to1on.gif)");

    $("ul.myinfo li.mynew ul.mynewlist li.my2").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to2.gif)");
    $("ul.myinfo li.mynew ul.mynewlist li.my2on").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to2on.gif)");

    $("ul.myinfo li.mynew ul.mynewlist li.my8").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to8.gif)");
    $("ul.myinfo li.mynew ul.mynewlist li.my8on").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to8on.gif)");

    $("ul.myinfo li.mynew ul.mynewlist li.my4").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to4.gif)");
    $("ul.myinfo li.mynew ul.mynewlist li.my4on").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to4on.gif)");

    $("ul.myinfo li.mynew ul.mynewlist li.my5").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to5.gif)");
    $("ul.myinfo li.mynew ul.mynewlist li.my5on").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to5on.gif)");

    $("ul.myinfo li.mynew ul.mynewlist li.my6").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to6.gif)");
    $("ul.myinfo li.mynew ul.mynewlist li.my6on").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to6on.gif)");

    $("ul.myinfo li.mynew ul.mynewlist li.my7").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to7.gif)");
    $("ul.myinfo li.mynew ul.mynewlist li.my7on").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to7on.gif)");

    if ($("#myInfo_NewMail") != undefined) { $("#myInfo_NewMail").attr("title", R_MyInfo_Title1); }
    if ($("#myInfo_TodayS") != undefined) { $("#myInfo_TodayS").attr("title", R_MyInfo_Title2); }
    if ($("#myInfo_Task") != undefined) { $("#myInfo_Task").attr("title", R_MyInfo_Title3); }
    if ($("#myInfo_AppDoc") != undefined) { $("#myInfo_AppDoc").attr("title", R_MyInfo_Title4); }
    if ($("#myInfo_AppApp") != undefined) { $("#myInfo_AppApp").attr("title", R_MyInfo_Title5); }
    if ($("#myInfo_AppDept") != undefined) { $("#myInfo_AppDept").attr("title", R_MyInfo_Title6); }
    if ($("#myInfo_AppCom") != undefined) { $("#myInfo_AppCom").attr("title", R_MyInfo_Title7); }
}

/**************************************************
작성자   : freeman
작성일자 : 2012-05-10
작성목적 : 사용자 정보 수정
***************************************************/
function MyInfo_Mod() {
    var url = "/DeskPlusEIP/Account/MyInfo/Mod.aspx";
    var WindowObjectReference = PopupWinOpen(url, "", "MYINFO", "1000", "618");
}




/**************************************************
작성자   : freeman
작성일자 : 2012-05-10
작성목적 : SNS 오픈
***************************************************/
MyInfo_OpenSNS = function (type, id, pwd) {
    var opt = "menubar=yes,location=yes,resizable=yes,scrollbars=yes,status=yes"
    switch (type.toLowerCase()) {
        case "facebook":
            var url = "http://www.facebook.com/" + id;
            var param = "";
            PopupWinOpen(url, param, 'facebook', 1024, 768, opt);
            break;
        case "twitter":
            var url = "http://twitter.com/" + id;
            var param = "";
            PopupWinOpen(url, param, 'facebook', 1024, 768, opt);
            break;
        case "me2day":
            var url = "http://me2day.net/" + id;
            var param = "";
            PopupWinOpen(url, param, 'facebook', 1024, 768, opt);
            break;
        case "naverblog":
            var url = "http://blog.naver.com/" + id;
            var param = "";
            PopupWinOpen(url, param, 'naverBlog', 1024, 768, opt);
            break;
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2015-10-01
작성목적 : 겸직 수정
***************************************************/
MyInfo_ChangeUserDept = function (obj) {

    if (obj.value == "") return;

    MyInfo_SetBaseDept(
        obj.value.split(',')[0],
        obj.value.split(',')[1],
        obj.value.split(',')[2],
        obj.value.split(',')[3],
        obj.value.split(',')[4],
        obj.value.split(',')[5]
    );
}

/**************************************************
작성자   : 심재운
작성일자 : 2015-10-01
작성목적 : 겸직 수정
***************************************************/
MyInfo_SetBaseDept = function (companyID, userID, deptID, titleID, dutyID, loginID) {
    var url = "/DeskPlusEIP/EIP/Ws/Menu/Menu.asmx/ModUserBaseDept";

    ParamArray = {
        'companyID': companyID,
        'userID': userID,
        'deptID': deptID,
        'titleID': titleID,
        'dutyID': dutyID
    };

    var obj = eval("MyInfo_SuccessSetBaseDept");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

    MyInfo_CallSLO(companyID, userID, loginID);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2015-10-01
작성목적 : 성공시  로그인 체크
**********************************************************************************************/
MyInfo_SuccessSetBaseDept = function (data, status) {
    if (data.d == true) {
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2015-10-01
작성목적 : slo 인증 쿠키 재생성
***************************************************/
MyInfo_CallSLO = function (companyID, userID, loginID) {
    var url = "/DeskPlusEIP/Account/MyInfo/SLO.aspx";
    ParamArray = {
        'companyID': companyID,
        'userID': userID,
        'loginID': loginID,
        'subcall': "1"
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    url = url + "?p=" + GetEncryptValue;

    $("#ifrmSLO").attr("src", url);
}

/**************************************************
작성자   : 심재운
작성일자 : 2015-10-01
작성목적 : 리로드
***************************************************/
ReLoad = function () {
    top.location = "/DeskPlusEIP/";
}

function MyInfo_GetApprovalStatusMenuUrl() {
    return (typeof ApprovalStatusMenuUrl != "undefined" && ApprovalStatusMenuUrl != "") ? ApprovalStatusMenuUrl : "/deskplusEIP/Approval/Status/Default.aspx";
}
