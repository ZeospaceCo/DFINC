$(document).ready(function () {

    var portletDom = myspace_tabPortletID + "_" + myspace_portletID;

    if (myspace_IsAuth != "True") {
        $("#basicMenu_" + portletDom + " .del").remove();
    }

    //최소화 여부
    if (myspace_IsMini == "1") {
        $("#ptContent_" + portletDom).fadeOut("slow");
    }


    $("#ptConfig_" + portletDom).contextMenu({ menu: "basicMenu_" + portletDom, leftButton: true }, function (action, el, pos) {
        var pID = el[0].id.replace('ptConfig_', '');
        MySpace_ContextMenuWork(pID, action, el, pos);
    });

    //새로고침
    $("#ptReload_" + portletDom).on("click", function (e) {

        e.preventDefault();

        var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
        $(this).html(loadingPath);

        setTimeout(function () {
            MySpace_GetContent();

            var loadingPath = "<img src='" + ImagePath + "/Main/ico_ref.gif' />";
            $("#ptReload_" + portletDom).html(loadingPath);

        }, 1);
    });

    $("#ptPreview_" + portletDom).on("click", function (e) {
        e.preventDefault();
        Main.Popup();
    });

    //setTimeout(function () {
        MySpace_GetContent();
    //}, 1);


});

/**************************************************
작성자: freeman
작성일:2012-02-07
작성목적 : 포틀릿 Context Menu
***************************************************/
function MySpace_ContextMenuWork(id, action, el, pos) {
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
                Main.ModMinMaxPortlet(myspace_tabPortletID, myspace_portletID, "1");
                $("#ptContent_" + id).fadeOut("slow");
                break;
            }
        case "max":
            {
                Main.ModMinMaxPortlet(myspace_tabPortletID, myspace_portletID, "0");
                $("#ptContent_" + id).fadeIn("slow");
                break;
            }
        case "info":
            {
                break;
            }
    }
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 
**********************************************************************************************/
function MySpace_GetContent() {    

    var url = "/DeskPlusEIP/Portlet/Ws/Package/MySpace.asmx/GetContent";

    ParamArray = {
    };

    var obj = eval("MySpace_SuccessGetContent");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 
**********************************************************************************************/
function MySpace_SuccessGetContent(data, status) {
    var portletDom = myspace_tabPortletID + "_" + myspace_portletID;    

    $("#ptList_" + portletDom).setTemplateURL('/DeskPlusEIP/Portlet/Jt/Package/JTMySpace.htm?seq=' + Guids(), null, { filter_data: false });
    $("#ptList_" + portletDom).processTemplate(data.d);

    MySpace_Resource();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 다국어
**********************************************************************************************/
function MySpace_Resource() { 
    $("#r_mail")[0].innerHTML = R_MailTitle;
    $("#r_webhard")[0].innerHTML = R_WebHardTitle;
    $("#r_board")[0].innerHTML = R_BoardTitle;
    $("#r_approval")[0].innerHTML = R_ApprovalTitle;
}
