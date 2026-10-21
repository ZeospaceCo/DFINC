$(document).ready(function () {

    var portletDom = birthDay_tabPortletID + "_" + birthDay_portletID;

    // 검색 아이콘
    if ($(".IconSearch") != undefined) { $(".IconSearch").attr('src', ImagePath + '/Icon/icon_search.gif'); }

    //관리자 여부
    if (birthDay_IsAuth != "True") {
        $("#cbtnPDel_" + portletDom).remove();
    }

    //최소화 여부
    if (birthDay_IsMini == "1") {
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

    // 포틀릿 삭제
    $("#cbtnPDel_" + portletDom).click(function (e) {
        e.preventDefault();

        Main.DelTabPortlet(portletDom);
        if (PortletAuth == "True") {
            $("#portlet_" + portletDom).fadeOut("slow");
        }
    });

    //포틀릿 설정
    $("#ptConfig_" + portletDom).on("click", function (e) {
        $("#ptListConfig_" + portletDom).slideToggle(600);
    });


    //    $("#ptPreview_" + portletDom).on("click", function (e) {
    //        e.preventDefault();
    //        Main.Popup();
    //    });


    //검색입력필드 엔터 이벤트 기술
    $("#searchValue").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            BirthDay_Search();
        }
    });

    //$("#" + CID_BirthDayStartDate).datepick($.datepicker.regional[PreLang]);
    //$("#" + CID_BirthDayEndDate).datepick($.datepicker.regional[PreLang]);

    //$("#" + CID_BirthDayStartDate).keydown(function (e) { return false; });
    //$("#" + CID_BirthDayEndDate).keydown(function (e) { return false; });

    //setTimeout(function () {
    BirthDay_LoadDate();
    //}, 1);

});

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 리스트 가져오기
***************************************************/
function BirthDay_LoadDate() {
    BirthDay_GetBirthDayList(); //검색 리스트 가져오기 
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 웹서비스 리스트 호출
***************************************************/
function BirthDay_GetBirthDayList() {

    var url = "/DeskPlusEIP/Portlet/Ws/Package/BirthDay.asmx/GetBirthDayList";

    birthDay_AnniversaryStartMonth = $("#" + CID_BirthDayStartMonth).val();
    birthDay_AnniversaryStartDay = $("#" + CID_BirthDayStartDay).val();
    birthDay_AnniversaryEndMonth = $("#" + CID_BirthDayEndMonth).val();
    birthDay_AnniversaryEndDay = $("#" + CID_BirthDayEndDay).val();

    birthDay_SearchValue = $("#searchValue").val().securityText();
    $("#searchValue").val(birthDay_SearchValue);

    ParamArray = {
        'listCount': birthDay_ListCount,
        'anniversaryStartMonth': birthDay_AnniversaryStartMonth,
        'anniversaryStartDay': birthDay_AnniversaryStartDay,
        'anniversaryEndMonth': birthDay_AnniversaryEndMonth,
        'anniversaryEndDay': birthDay_AnniversaryEndDay,
        'searchValue': birthDay_SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("BirthDay_SuccessList");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 리스트 표시
**********************************************************************************************/
function BirthDay_SuccessList(data, status) {
    $('#phBirthDay').setTemplateURL('/DeskPlusEIP/Portlet/Jt/Package/JTBirthDayList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phBirthDay').processTemplate(data.d);
    BirthDay_Resource();
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 리소스
***************************************************/
function BirthDay_Resource() {
    if ($("#NoContents")[0] != undefined) {
        $("#NoContents")[0].innerHTML = R_NoContents;
    }
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 웹서비스 리스트 호출
***************************************************/
function BirthDay_GetDateInfo() {
    var url = "/DeskPlusEIP/Portlet/Ws/Package/BirthDay.asmx/GetDateInfo";
    birthDay_AnniversaryDate = $("#" + CID_BirthDayStartDate).val();

    ParamArray = {
        'anniversaryDate': birthDay_AnniversaryDate,
        'scope': birthDay_Scope
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("BirthDay_DateInfoSuccessList");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-18
작성목적 : 성공시 , 리스트 표시
**********************************************************************************************/
function BirthDay_DateInfoSuccessList(data, status) {
    $("#" + CID_BirthDayStartDate).val(data.d);
    BirthDay_GetBirthDayList();
}



/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-18
작성목적 : 검색
**********************************************************************************************/
function BirthDay_Search() {
    /*
    var getStartDate = $("#" + CID_BirthDayStartDate).val();
    var getEndDate = $("#" + CID_BirthDayEndDate).val();
    */

    var getStartDate = $("#" + CID_BirthDayStartMonth).val() + $("#" + CID_BirthDayStartDay).val();
    var getEndDate = $("#" + CID_BirthDayEndMonth).val() + $("#" + CID_BirthDayEndDay).val();

    if (getStartDate == "" || getEndDate == "") {
        alert(R_InputDate);
        return;
    }

    var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

    if (dtResponse == false) {
        alert(R_StartDateEndDate);
        return;
    }

    BirthDay_LoadDate();
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-18
작성목적 : ◀▶ 클릭 시 날짜 바인딩
**********************************************************************************************/
function BirthDay_DateChange(arg) {
    birthDay_Scope = arg;
    BirthDay_GetDateInfo();
}
