ViewHistory = {}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    $(document).ready(function () {

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        ViewHistory.LoadData();
    });
});


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
ViewHistory.LoadData = function () {


    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        ViewHistory.GetPageList();
    }

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        loadBoardData();
    }, 100);
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 리스트 호출
**********************************************************************************************/
ViewHistory.GetPageList = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Holiday/Popup/ViewHistory.asmx/GetList";

    ParamArray = {
        'userID': UserID,
        'startDate': StartDate,
        'type': Type        
    };

    var obj = eval("ViewHistory.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
ViewHistory.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/ETC/Jt/Holiday/Popup/JTViewHistory.htm?seq=' + Guids());
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ViewHistory.Resource();

    $("#scrollDiv").css({ 'overflow-y': 'auto', 'min-height': '320px', 'height': '320px' });
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

ViewHistory.Resource = function () {

    $("#r_holititle")[0].innerHTML = R_HoliTitle;
    $("#r_holicount")[0].innerHTML = R_YearText;
    $("#r_modname")[0].innerHTML = R_ModName;
    $("#r_moddate")[0].innerHTML = R_ModDate;

    if ($(".r_holitype") != undefined) {
        if (Type == "1") {
            $(".r_holitype").text(R_HoliType1);
        }
        else if (Type == "2") {
            $(".r_holitype").text(R_HoliType2);
        }
        else if (Type == "3") {
            $(".r_holitype").text(R_HoliType3);
        }
    }

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}
