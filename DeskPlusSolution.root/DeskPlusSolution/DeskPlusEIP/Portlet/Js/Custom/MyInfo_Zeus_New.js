
$(document).ready(function () {

    var portletDom = myInfo_tabPortletID + "_" + myInfo_portletID;

    if (myInfo_IsAuth != "True") {
        $("#basicMenu_" + portletDom + " .del").remove();

    }

    //최소화 여부
    if (myInfo_IsMini == "1") {
        $("#ptContent_" + portletDom).fadeOut("slow");
    }

    $("#ptConfig_" + portletDom).contextMenu({ menu: "basicMenu_" + portletDom, leftButton: true }, function (action, el, pos) {

        var pID = el[0].id.replace('ptConfig_', '');
        MyInfo_ContextMenuWork(pID, action, el, pos);
    });

    //새로고침
    $("#ptReload_" + portletDom).on("click", function (e) {
        e.preventDefault();

        //var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
        //$(this).html(loadingPath);

        setTimeout(function () {
            //함수 호출
            MyInfo_GetContent();
            MyInfo_GetContentAdd();
            //var loadingPath = "<img src='" + ImagePath + "/Main/ico_ref.gif' />";
            //$("#ptReload_" + portletDom).html(loadingPath);
        }, 1);
    });


    $("#ptPreview_" + portletDom).on("click", function (e) {
        e.preventDefault();

        Main.Popup();
    });

    $("#ptListConfig_" + portletDom).hide();

    //setTimeout(function () {
    MyInfo_GetContent();
    //}, 1);

});

/*
function MyInfo_ShortCut(id) {

switch (id) {
case "myInfo_NewMail":
parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webmail";
break;
case "myInfo_TodayS":
parent.frmTop.Menu.GoParam("webpims");
document.location.href = "/DeskPlusEIP/Pim/Default.aspx";
break;
case "myInfo_TodayT":
parent.frmTop.Menu.GoParam("webpims");
document.location.href = "/DeskPlusEIP/Pim/Default.aspx";
break;
case "myInfo_AppDoc":
parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=approvalStatus";
break;
case "myInfo_AppApp":
parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=approvalStatus";
break;
case "myInfo_AppDept":
parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=approvalStatus";
break;
case "myInfo_AppCom":
parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=approvalStatus";
break;
}
}
*/

function MyInfo_Event() {
    //메일 바로가기
    $("#myInfo_NewMail").on("click", function (e) {
        e.preventDefault();
        parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webmail";
    });

    //일정바로가기
    $("#myInfo_TodayS").on("click", function (e) {
        e.preventDefault();
        document.location.href = "/DeskPlusEIP/Pim/Default.aspx";
        parent.frmTop.Menu.GoParam("pim");
    });

    //할일바로가기
    $("#myInfo_TodayT").on("click", function (e) {
        e.preventDefault();
        document.location.href = "/DeskPlusEIP/Pim/Default.aspx";
        parent.frmTop.Menu.GoParam("pim");
    });

    //전자결재바로가기
    $("#myInfo_AppDoc").on("click", function (e) {
        e.preventDefault();
        ParamArray = { 'folderStatusID': 'Process' };
        CallEncrypt(CallGetParam());
        document.location.href = "/deskplusEIP/Approval/Status/Default.aspx?p=" + GetEncryptValue;
        parent.frmTop.Menu.GoParam("approvalStatus");

        //parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=approvalStatus";
    });

    //전자결재바로가기
    $("#myInfo_AppApp").on("click", function (e) {
        e.preventDefault();
        ParamArray = { 'folderStatusID': 'Approve' };
        CallEncrypt(CallGetParam());
        document.location.href = "/deskplusEIP/Approval/Status/Default.aspx?p=" + GetEncryptValue;
        parent.frmTop.Menu.GoParam("approvalStatus");
        //parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=approvalStatus";
    });

    //전자결재바로가기
    $("#myInfo_AppDept").on("click", function (e) {
        e.preventDefault();
        ParamArray = { 'folderStatusID': 'Approve' };
        CallEncrypt(CallGetParam());
        document.location.href = "/deskplusEIP/Approval/Status/Default.aspx?p=" + GetEncryptValue;
        parent.frmTop.Menu.GoParam("approvalStatus");
        //parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=approvalStatus";
    });

    //전자결재바로가기
    $("#myInfo_AppCom").on("click", function (e) {
        e.preventDefault();
        ParamArray = { 'folderStatusID': 'Receive' };
        CallEncrypt(CallGetParam());
        document.location.href = "/deskplusEIP/Approval/Status/Default.aspx?p=" + GetEncryptValue;
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
                Main.ModMinMaxPortlet(myInfo_tabPortletID, myInfo_portletID, "1");
                $("#ptContent_" + id).fadeOut("slow");
                break;
            }
        case "max":
            {
                Main.ModMinMaxPortlet(myInfo_tabPortletID, myInfo_portletID, "0");
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
    var url = "/DeskPlusEIP/Portlet/Ws/Custom/MyInfo_Zeus_New.asmx/GetMyInfo";
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
        cnt += Number(objListInfo.TodayPimDoCnt);
        cnt += Number(objListInfo.DocmentCnt);
        cnt += Number(objListInfo.ApprovalCnt);
        cnt += Number(objListInfo.DeptSusinCnt);
        cnt += Number(objListInfo.CompleteCnt);

        if (cnt > 0) {
            $(".mytoday").show();
        }
    });

    //alert(data.d[0].MBO);
    var portletDom = myInfo_tabPortletID + "_" + myInfo_portletID;

    $("#ptList_" + portletDom).setTemplateURL('/DeskPlusEIP/Portlet/Jt/Custom/JTMyInfo_Zeus_New.htm?seq=' + Guids(), null, { filter_data: false });
    $("#ptList_" + portletDom).processTemplate(data.d);

    $("#ptList_Zeus_" + portletDom).setTemplateURL('/DeskPlusEIP/Portlet/Jt/Custom/JTMyInfo_Zeus_New2.htm?seq=' + Guids(), null, { filter_data: false });
    $("#ptList_Zeus_" + portletDom).processTemplate(data.d);

    MyInfo_Resource();

    MyInfo_Event();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 다국어 
상신문서 ico_myinfo_t2.gif 
결재문서 ico_myinfo_t3.gif 
대결문서 ico_myinfo_t9.gif 
수신문서 ico_myinfo_t4.gif 
참조문서 ico_myinfo_t10.gif 
회람문서 ico_myinfo_t11.gif 
**********************************************************************************************/

function MyInfo_Resource() {

    if ($(".r_count") != undefined) {
        $(".r_count").text(R_Count);
    }


    
    if (PreLang != "ko") {
        $("ul.myinfo li.mynew ul.mynewlist li.my1").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to1.gif)");
        $("ul.myinfo li.mynew ul.mynewlist li.my1on").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to1on.gif)");

        $("ul.myinfo li.mynew ul.mynewlist li.my2").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to2.gif)");
        $("ul.myinfo li.mynew ul.mynewlist li.my2on").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to2on.gif)");

        $("ul.myinfo li.mynew ul.mynewlist li.my3").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to3.gif)");
        $("ul.myinfo li.mynew ul.mynewlist li.my3on").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to3on.gif)");

        $("ul.myinfo li.mynew ul.mynewlist li.my4").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to4.gif)");
        $("ul.myinfo li.mynew ul.mynewlist li.my4on").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to4on.gif)");

        $("ul.myinfo li.mynew ul.mynewlist li.my5").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to5.gif)");
        $("ul.myinfo li.mynew ul.mynewlist li.my5on").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to5on.gif)");

        $("ul.myinfo li.mynew ul.mynewlist li.my6").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to6.gif)");
        $("ul.myinfo li.mynew ul.mynewlist li.my6on").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to6on.gif)");

        $("ul.myinfo li.mynew ul.mynewlist li.my7").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to7.gif)");
        $("ul.myinfo li.mynew ul.mynewlist li.my7on").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/to7on.gif)");


    }
    /*
    if (PreLang == "ko") {
        $("ul.myinfo li.mynew_zeus ul.mynewlist_zeus li.my8").css("background-image;", "url(" + ImagePath + "/Lang/" + PreLang + "/Icon_MyInfo_Proposal.gif)");
        $("ul.myinfo li.mynew_zeus ul.mynewlist_zeus li.my8on").css("background-image;background-repeat:no-repeat", "url(" + ImagePath + "/Lang/" + PreLang + "/Icon_MyInfo_Proposal.gif)");

        $("ul.myinfo li.mynew_zeus ul.mynewlist_zeus li.my9").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/Icon_MyInfo_MBO.gif)");
        $("ul.myinfo li.mynew_zeus ul.mynewlist_zeus li.my9on").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/Icon_MyInfo_MBO.gif)");

        $("ul.myinfo li.mynew_zeus ul.mynewlist_zeus li.my10").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/Icon_MyInfo_VoluntaryService.gif)");
        $("ul.myinfo li.mynew_zeus ul.mynewlist_zeus li.my10on").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/Icon_MyInfo_VoluntaryService.gif)");

        $("ul.myinfo li.mynew_zeus ul.mynewlist_zeus li.my11").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/Icon_MyInfo_TotalCheckup.gif)");
        $("ul.myinfo li.mynew_zeus ul.mynewlist_zeus li.my11on").css("background-image", "url(" + ImagePath + "/Lang/" + PreLang + "/Icon_MyInfo_TotalCheckup.gif)");

    }
    */

    if ($(".my8") != undefined) { $(".my8").attr('src', ImagePath + "/Lang/" + PreLang + "/Icon_MyInfo_Proposal.gif"); }
    if ($(".my9") != undefined) { $(".my9").attr('src', ImagePath + "/Lang/" + PreLang + "/Icon_MyInfo_MBO.gif"); }
    if ($(".my10") != undefined) { $(".my10").attr('src', ImagePath + "/Lang/" + PreLang + "/Icon_MyInfo_VoluntaryService.gif"); }
    if ($(".my11") != undefined) { $(".my11").attr('src', ImagePath + "/Lang/" + PreLang + "/Icon_MyInfo_TotalCheckup.gif"); }
    
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