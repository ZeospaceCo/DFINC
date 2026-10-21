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

        //검색조건 변화
        $("#ddlSearchYear").change(function (e) {
            e.preventDefault();
            List.GetPageList();
        });

        //검색조건 변화
        $("#ddlCategory").change(function (e) {
            e.preventDefault();
            List.GetPageList();
        });


        ///나의일정
        $('#CbtnMySchedule').click(function (e) {
            e.preventDefault();
            parent.document.location.href = "/DeskPlusEIP/Pim/Default.aspx";
        });

        List.GetPageList();

    });
});


/***********************
작성자   : DANIEL  
작성일자 : 2011년 8월 19일
작성목적 : 검색조건 설정
**********************/
List.SearchSetting = function () {
    Searchkey = $("#ddlCategory").val();
    SearchField = $("#ddlSearchYear").val();
}

/**********************************************************************************************
작성자   : DANIEL  
작성일자 : 2011년 8월 16일
작성목적 : 리스트 가져오기
**********************************************************************************************/
List.GetPageList = function () {

    List.SearchSetting();

    var url = "/DeskPlusEIP/Pim/Ws/Admin/Holiday/List.asmx/GetHolyDay";
    var StarDate = $("#ddlSearchYear").val() + "-01-01";

    ParamArray = {
        'StarDate': StarDate,
        'SearchKey': Searchkey
    };

    var obj = eval("List.SuccessHolyDayList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}


List.SuccessHolyDayList = function (data, status) {

    $('#placeholder').setTemplateURL('/DeskPlusEIP/Pim/Jt/Admin/Holiday/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");
    List.Resource();

}


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 
***************************************************/
List.Add = function () {
    List.SearchSetting();

    ParamArray = {
        'SearchField': SearchField,
        'Searchkey': Searchkey
    };

    CallEncrypt(CallGetParam());
    location.href = "AddN.aspx?p=" + GetEncryptValue;

}

List.Resource = function () {

    $("#r_kindDesk")[0].innerHTML = col1;
    $("#r_title")[0].innerHTML = col2;
    $("#r_startTime")[0].innerHTML = col3;
    $("#r_userName")[0].innerHTML = col4;
    $("#idempty").text(TextEmpty);
    
}

List.View = function (sid, did) {

    List.SearchSetting();
    
    ParamArray = {
        'ScheduleID': sid,
        'DisplayID': did,
        'SearchField': SearchField,
        'Searchkey': Searchkey
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;

}


