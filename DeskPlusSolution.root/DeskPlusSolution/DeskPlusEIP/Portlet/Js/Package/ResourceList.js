$(document).ready(function () {

    var portletDom = resourceList_tabPortletID + "_" + resourceList_portletID;

    if (resourceList_IsAuth != "True") {
        $("#ptConfig_" + portletDom).remove();
    }

    //최소화 여부
    if (resourceList_IsMini == "1") {
        $("#ptSlide_" + portletDom).children('i').toggleClass('fa-chevron-up');
        $("#ptContent_" + portletDom).fadeOut("slow"); 
    }

    //포틀릿 최소화/최대화
    $("#ptSlide_" + portletDom).on("click", function (e) {
        if ($("#hdnTabSlide_" + portletDom).val() == "1") {
            //최대화
            Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "0");
            $("#ptContent_" + portletDom).fadeIn("slow");
            $("#hdnTabSlide_" + portletDom).val("0");
        }
        else {
            //최소화
            Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "1");
            $("#ptContent_" + portletDom).fadeOut("slow");
            $("#hdnTabSlide_" + portletDom).val("1");
        }
    });


    //포틀릿 설정
    $("#ptConfig_" + portletDom).on("click", function (e) {
        $("#ptListConfig_" + portletDom).slideToggle(600);
    });


    //새로고침
    $("#ptReload_" + portletDom).on("click", function (e) {

        e.preventDefault();

        var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
        $(this).html(loadingPath);

        setTimeout(function () {

            LoadRMList();
            var loadingPath = "<img src='" + ImagePath + "/Main/ico_ref.gif' />";
            $("#ptReload_" + portletDom).html(loadingPath);

        }, 1);
    });

    // 더보기
    $("#ptPreview_" + portletDom).on("click", function (e) {
        e.preventDefault();

        //Main.Popup();
        parent.frmTop.Menu.GoParam("Resource");
        document.location.href = "/DeskPlusEIP/RM/Default.aspx";
    });

    // 포틀릿 삭제
    $("#cbtnPDel_" + portletDom).click(function (e) {
        e.preventDefault();

        Main.DelTabPortlet(portletDom);
        if (PortletAuth == "True") {
            $("#portlet_" + portletDom).fadeOut("slow");
        }
    });

    //setTimeout(function () {
        LoadRMList();
    //}, 1);
    

});

/**************************************************
작성자: famo23
작성일:    2012-03-19
작성목적 : 페이지 가져오기
***************************************************/
function LoadRMList() {
    var portletDom = resourceList_tabPortletID + "_" + resourceList_portletID;

    //게시판 리스트
    var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Resource.asmx/GetRMList";

    $.ajax({
        type: "POST",
        url: urlPage,
        async: false,
        data: '',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#plList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/JTResourceList.htm?seq=" + Guids(), null, { filter_data: false });
            $('#plList_' + portletDom).processTemplate(data.d);


            setTimeout(function () {
                Resource_Resource();
            }, 1);
        }
        ,
        error: function (data, status, err) {
            alert(R_ErrorConnection + data);
        }
    });
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-21
작성목적 : 다국어 처리
**********************************************************************************************/
function Resource_Resource() {

    if ($("#Re_NoListData")[0] != undefined) {
        $("#Re_NoListData")[0].innerHTML = R_NoResources;
    }

    //if ($(".IconBullet") != undefined) { $(".IconBullet").attr('src', ImagePath + '/main/ico_bullet2.gif'); }

}

/**************************************************
작성목적 : 자원관리 상세보기
***************************************************/
function TabReMain_PopupMyViewList(p_reservationID) {

    ParamArray = { 'reservationID': p_reservationID };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/RM/Content/ViewPopup.aspx?p=" + GetEncryptValue;
    window.open(url, "Re" + p_reservationID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}  