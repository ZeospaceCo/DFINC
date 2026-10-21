$(document).ready(function () {

    var portletDom = opinion_tabPortletID + "_" + opinion_portletID;

    if (opinion_IsAuth != "True") { $("#ptConfig_" + portletDom).remove(); }

    //최소화 여부
    if (opinion_IsMini == "1") {
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

    // 포틀릿 삭제
    $("#cbtnPDel_" + portletDom).click(function (e) {
        e.preventDefault();

        Main.DelTabPortlet(portletDom);
        if (PortletAuth == "True") {
            $("#portlet_" + portletDom).fadeOut("slow");
        }
    });
        

    //새로고침
    $("#ptReload_" + portletDom).on("click", function (e) {
        e.preventDefault(); Opinion_Reload();
    });

    $("#ptPreview_" + portletDom).on("click", function (e) {
        e.preventDefault();
        document.location.href = "/DeskPlusEIP/Opinion/Default.aspx";
        parent.frmTop.Menu.GoParam("opinion");
    });

    //setTimeout(function () {
        Opinion_GetContent();
    //}, 1);

    
});

function Opinion_Reload() {
    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
    $(this).html(loadingPath);

    setTimeout(function () {
        Opinion_GetContent();
        var loadingPath = "<img src='" + ImagePath + "/Main/ico_ref.gif' />";
        $("#ptReload_" + portletDomID).html(loadingPath);
    }, 1);
}

/**************************************************
작성자: freeman
작성일:2012-02-07
작성목적 : 포틀릿 Context Menu
***************************************************/
function Opinion_ContextMenuWork(id, action, el, pos) {
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
                Main.ModMinMaxPortlet(opinion_tabPortletID, opinion_portletID, "1");
                $("#ptContent_" + id).fadeOut("slow");
                break;
            }
        case "max":
            {
                Main.ModMinMaxPortlet(opinion_tabPortletID, opinion_portletID, "0");
                $("#ptContent_" + id).fadeIn("slow");
                break;
            }
        case "info":
            {
                $("#ptOpinionConfig").slideToggle(600);
                break;
            }
    }
}

/**********************************************************************************************
작성자   : withsuki
작성일자 : 2012-03-15
작성목적 : 설문대상리스트 가져오기
**********************************************************************************************/
function Opinion_GetContent() {
    var url = "/DeskPlusEIP/Portlet/Ws/Package/Opinion.asmx/GetQuesList";
    //파라미터값 배열에 담기    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {    };

    var obj = eval("Opinion_ScsGetContent");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
function Opinion_ScsGetContent(data, status) {
    var portletDom = opinion_tabPortletID + "_" + opinion_portletID;

    $("#ptList_" + portletDom).setTemplateURL('/DeskPlusEIP/Portlet/Jt/Package/JTOpinion.htm?seq=' + Guids(), null, { filter_data: false });
    $("#ptList_" + portletDom).processTemplate(data.d);

    Opinion_Resource();    //다국어처리
}
function Opinion_Resource() {
    $(".jtOpinionNoData").text(R_JTOpinionNoData);
    $(".jtOpinionR1").text(R_JTOpinionR1);
    $(".jtOpinionR2").text(R_JTOpinionR2);
    //$("#titOpinion").text(R_MenuF1);

    if ($(".IconAdd1") != undefined) { $(".IconAdd1").attr('src', ImagePath + '/Lang/' + PreLang + '/ico_opinion_add1.gif'); }
    if ($(".IconAdd2") != undefined) { $(".IconAdd2").attr('src', ImagePath + '/Lang/' + PreLang + '/ico_opinion_add2.gif'); }
}
/**********************************************************************************************
작성자   : withsuki
작성일자 : 2012-03-21
작성목적 : 특정 설문 항목리스트 가져오기
**********************************************************************************************/
function Opinion_ItemShow(_quesID) {

    $("#divItem_" + _quesID).html("");  //초기화
    $("#divItem_" + _quesID).show();
    $("#opiShow_" + _quesID).hide(); $("#opiHide_" + _quesID).show();

    var url = "/DeskPlusEIP/Portlet/Ws/Package/Opinion.asmx/GetQuesUnitList";
    ParamArray = { 'quesID': _quesID }

    var obj = eval("Opinion_ScsItemList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
function Opinion_ScsItemList(data, status) {
    if (data.d.length > 0) {
        var quesID = data.d[0]["QuesID"];
        $("#divItem_" + quesID).setTemplateURL('/DeskPlusEIP/Portlet/Jt/Package/JTOpinionItem.htm?seq=' + Guids(), null, { filter_data: false });
        $("#divItem_" + quesID).processTemplate(data.d);

        $("#divMore_" + quesID).show(); //투표하기,결과보기 display
    }

    //다국어처리
    $(".jtOpinionItemNoData").text(R_JTOpinionItemNoData);
    
    if ($(".IconBullet") != undefined) { $(".IconBullet").attr('src', ImagePath + '/main/ico_bullet2.gif'); }
}
function Opinion_ItemHide(_quesID) {
    $("#divItem_" + _quesID).hide();
    $("#opiShow_" + _quesID).show(); $("#opiHide_" + _quesID).hide(); $("#divMore_" + _quesID).hide();
   
}

/**********************************************************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 투표시->투표결과페이지 / 미 투표시->투표페이지 이동
**********************************************************************************************/
Opinion_View = function (_quesID, _isVote) {
    ParamArray = { 'quesID': _quesID };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Opinion/Content/";
    url += (_isVote == "0") ? "VotePopup.aspx" : "ResultPopup.aspx";
//    url += "?p=" + GetEncryptValue;
//    WindowObjectReference = window.open(url, "Portlet", "menubar=no,location=no,resizable=auto,scrollbars=auto,status=no");
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "Popup", "1200", "800", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no")
}