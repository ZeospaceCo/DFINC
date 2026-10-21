List = {}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/
$(function () {
    $(document).ready(function () {

        //기념일 추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        //검색버튼
        $("#cbtnSearch").click(function (e) {
            e.preventDefault();
            List.GetPageList();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                List.GetPageList();
            }
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        List.GetPageList();
    });
});


/***********************
작성자   : 이지원  
작성일자 : 2017-01-20
작성목적 : 검색조건 설정
**********************/
List.SearchSetting = function () {
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
}

/**********************************************************************************************
작성자   : 이지원  
작성일자 : 2017-01-20
작성목적 : 리스트 가져오기
**********************************************************************************************/
List.GetPageList = function () {

    List.SearchSetting();

    var url = "/DeskPlusEIP/Pim/Ws/Admin/HolidayD/List.asmx/GetList";

    ParamArray = {
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    var obj = eval("List.SuccessHolyDayList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이지원  
작성일자 : 2017-01-20
작성목적 : 성공시
***************************************************/
List.SuccessHolyDayList = function (data, status) {

    $('#placeholder').setTemplateURL('/DeskPlusEIP/Pim/Jt/Admin/HolidayD/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");
    List.Resource();

}

/**************************************************
작성자   : 이지원  
작성일자 : 2017-01-20
작성목적 : 리소스
***************************************************/
List.Resource = function () {

    $("#r_holiday_IsHolidayKind2")[0].innerHTML = Holiday_IsHolidayKind2;
    $("#r_holiday_SolarLunar")[0].innerHTML = Holiday_SolarLunar;
    $("#r_holiday_HoliDay2")[0].innerHTML = Holiday_HoliDay2;
    $("#r_holiday_Title")[0].innerHTML = Holiday_Title;
    $("#r_wRITER")[0].innerHTML = WRITER;

    $(".r_holiday_Solar").text(Holiday_Solar);
    $(".r_holiday_Lunar").text(Holiday_Lunar);

    if ($(".textEmpty") != undefined) { $(".textEmpty").text(TextEmpty); }
}

/**************************************************
작성자   : 이지원  
작성일자 : 2017-01-20
작성목적 : 추가
***************************************************/
List.Add = function () {
    List.SearchSetting();

    ParamArray = {
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
 
    location.href = "Add.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 이지원  
작성일자 : 2017-01-20
작성목적 : 삭제
***************************************************/
List.Mod = function (holiDayID) {

    List.SearchSetting();
    
    ParamArray = {
        'holiDayID': holiDayID,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());

    location.href = "Mod.aspx?p=" + GetEncryptValue;

}


