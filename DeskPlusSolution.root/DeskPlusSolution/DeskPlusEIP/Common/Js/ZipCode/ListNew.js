List = {}



/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    $(document).ready(function () {

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (event) {
            List.KeyEvents();
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            List.GetList();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            List.Close();
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);
    });
});


List.Resource = function () {
    $("#header_1")[0].innerHTML = "우편번호"; //R_ZipCode;
    $("#header_2")[0].innerHTML = "시도"; // R_Country;
    $("#header_3")[0].innerHTML = "시군구"; // R_City;
    $("#header_4")[0].innerHTML = "도로명"; // R_State;
    $("#header_5")[0].innerHTML = "건물번호본번"; // R_Detail;
    $("#header_6")[0].innerHTML = "건물번호부번";// R_HouseNumber;

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NotExistList;
    }
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
List.GetList = function () {

    SearchValue = $("#ctbSearch").val().securityText();
    var url = "/DeskPlusEIP/Common/Ws/ZipCode/ZipCode.asmx/GetZipCodeNew";

    //파라미터값 배열에 담기
    ParamArray = {
        'dongName': SearchValue
    };

    var obj = eval("List.SuccessGetList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
List.SuccessGetList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Common/Jt/ZipCode/JTListNew.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 키이벤트
***************************************************/
List.KeyEvents = function () {
    if (event.keyCode == 13) {
        List.GetList();
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 우편번호 호출페이로 보내기
***************************************************/
List.GetAddress = function (zipcode, ido, si, street, num1, num2) {

    ido = encodeURIComponent(ido);
    si = encodeURIComponent(si);
    street = encodeURIComponent(street);
    num1 = encodeURIComponent(num1);
    num2 = num2 == "0" ? "" : encodeURIComponent("-" + num2);
    var dong = street + " " + num1 + " " + num2;

    //    var obj = eval("parent." + CallBackPage);
    //    obj(zipcode, ido, si, street);
    //    parent.disablePopup();

    if (parent.disablePopup) {
        var obj = eval("parent." + CallBackPage);
        obj(zipcode, ido, si, dong);
        parent.disablePopup();
    } else {
        var obj = eval("opener." + CallBackPage);
        obj(zipcode, ido, si, dong);
        self.close();
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 닫기
***************************************************/
List.Close = function () {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 구/신 우편번호 선택 시 해당 페이지 이동
***************************************************/
List.GoZipCode = function () {
    var zipCode = $('input[name="rbLZipCode"]:checked').val();
    var loc = "ZipCode.aspx";

    if (zipCode == "2") loc = "ZipCodeNew.aspx";

    ParamArray = {
        'callBackPage': CallBackPage
    };

    CallEncrypt(CallGetParam());
    var url = loc + "?p=" + GetEncryptValue;
    location = url;
}