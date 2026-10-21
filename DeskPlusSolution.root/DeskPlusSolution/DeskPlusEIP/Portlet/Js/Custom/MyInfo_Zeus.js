
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

        var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
        $(this).html(loadingPath);

        setTimeout(function () {
            //함수 호출
            MyInfo_GetContent();
            var loadingPath = "<img src='" + ImagePath + "/Main/ico_ref.gif' />";
            $("#ptReload_" + portletDom).html(loadingPath);
        }, 1);
    });


    $("#ptPreview_" + portletDom).on("click", function (e) {
        e.preventDefault();

        Main.Popup();
    });

    $("#ptListConfig_" + portletDom).hide();

    setTimeout(function () {
        MyInfo_GetContent();
    }, 1000*1);

});

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
                alert("설정 정보가 없습니다.");
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

    var url = "/DeskPlusEIP/Portlet/Ws/Custom/MyInfo_Zeus.asmx/GetMyInfo";
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

    var portletDom = myInfo_tabPortletID + "_" + myInfo_portletID;

    $("#ptList_" + portletDom).setTemplateURL('/DeskPlusEIP/Portlet/Jt/Custom/JTMyInfo_Zeus.htm?seq=' + Guids(), null, { filter_data: false });
    $("#ptList_" + portletDom).processTemplate(data.d);

    MyInfo_Resource();
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

    if ($(".r_count") != undefined) { $(".r_count").text(R_Count); }



    if ($(".IconMyInfot1") != undefined) { $(".IconMyInfot1").attr('src', ImagePath + '/Lang/' + PreLang + '/ico_myinfo_t1.gif'); }
    if ($(".IconMyInfot2") != undefined) { $(".IconMyInfot2").attr('src', ImagePath + '/Lang/' + PreLang + '/ico_myinfo_t2.gif'); }
    if ($(".IconMyInfot3") != undefined) { $(".IconMyInfot3").attr('src', ImagePath + '/Lang/' + PreLang + '/ico_myinfo_t3.gif'); }
    if ($(".IconMyInfot4") != undefined) { $(".IconMyInfot4").attr('src', ImagePath + '/Lang/' + PreLang + '/ico_myinfo_t4.gif'); }
    if ($(".IconMyInfot5") != undefined) { $(".IconMyInfot5").attr('src', ImagePath + '/Lang/' + PreLang + '/ico_myinfo_t5.gif'); }
    if ($(".IconMyInfot6") != undefined) { $(".IconMyInfot6").attr('src', ImagePath + '/Lang/' + PreLang + '/ico_myinfo_t6.gif'); }
    if ($(".IconMyInfot7") != undefined) { $(".IconMyInfot7").attr('src', ImagePath + '/Lang/' + PreLang + '/ico_myinfo_t7.gif'); }
    if ($(".IconMyInfot8") != undefined) { $(".IconMyInfot8").attr('src', ImagePath + '/Lang/' + PreLang + '/ico_myinfo_t8.gif'); }
    if ($(".IconMyInfot9") != undefined) { $(".IconMyInfot9").attr('src', ImagePath + '/Lang/' + PreLang + '/ico_myinfo_t9.gif'); }
    if ($(".IconMyInfot10") != undefined) { $(".IconMyInfot10").attr('src', ImagePath + '/Lang/' + PreLang + '/ico_myinfo_t10.gif'); }
    if ($(".IconMyInfot11") != undefined) { $(".IconMyInfot11").attr('src', ImagePath + '/Lang/' + PreLang + '/ico_myinfo_t11.gif'); }
    if ($(".MyInfo_Proposal") != undefined) { $(".MyInfo_Proposal").attr('src', ImagePath + '/Lang/' + PreLang + '/Icon_MyInfo_Proposal.gif'); }
    if ($(".MyInfo_MBO") != undefined) { $(".MyInfo_MBO").attr('src', ImagePath + '/Lang/' + PreLang + '/Icon_MyInfo_MBO.gif'); }
    if ($(".MyInfo_VoluntaryService") != undefined) { $(".MyInfo_VoluntaryService").attr('src', ImagePath + '/Lang/' + PreLang + '/Icon_MyInfo_VoluntaryService.gif'); }
    if ($(".MyInfo_TotalCheckup") != undefined) { $(".MyInfo_TotalCheckup").attr('src', ImagePath + '/Lang/' + PreLang + '/Icon_MyInfo_TotalCheckup.gif'); }


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