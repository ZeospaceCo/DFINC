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

        //구 바인딩 
        $('#ddlSi').change(function () {
            List.GetGu();
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        List.GetSi();
    });
});

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 시 가져오기
**********************************************************************************************/
List.GetSi = function () {
    var url = "/DeskPlusEIP/Common/Ws/ZipCode/ZipCode.asmx/GetSiCode";

    $("select[id='ddlSi']").find('option').each(function () {
        $(this).remove();
    });

    $("select[id='ddlGu']").find('option').each(function () {
        $(this).remove();
    });

    if (ZipCode == "1") {
        var obj = eval("List.SuccessGetSi");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
    else {

        $("#ddlSi").append("<option value='강원도'>강원도</option>");
        $("#ddlSi").append("<option value='경기도'>경기도</option>");
        $("#ddlSi").append("<option value='경상남도'>경상남도</option>");
        $("#ddlSi").append("<option value='경상북도'>경상북도</option>");
        $("#ddlSi").append("<option value='광주광역시'>광주광역시</option>");
        $("#ddlSi").append("<option value='대전광역시'>대전광역시</option>");
        $("#ddlSi").append("<option value='대구광역시'>대구광역시</option>");
        $("#ddlSi").append("<option value='부산광역시'>부산광역시</option>");
        $("#ddlSi").append("<option value='서울특별시'>서울특별시</option>");
        $("#ddlSi").append("<option value='세종특별자치시'>세종특별자치시</option>");
        $("#ddlSi").append("<option value='울산광역시'>울산광역시</option>");
        $("#ddlSi").append("<option value='인천광역시'>인천광역시</option>");
        $("#ddlSi").append("<option value='전라남도'>전라남도</option>");
        $("#ddlSi").append("<option value='전라북도'>전라북도</option>");
        $("#ddlSi").append("<option value='제주특별자치도'>제주특별자치도</option>");
        $("#ddlSi").append("<option value='충청남도'>충청남도</option>");
        $("#ddlSi").append("<option value='충청북도'>충청북도</option>");

        $("#ddlSi").val("강원도");
    }
    List.GetGu();
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 성공시 시 바인딩
**********************************************************************************************/
List.SuccessGetSi = function (data, status) {

    $("select[id='ddlSi']").find('option').each(function () {
        $(this).remove();

    });

    for (var i = 0; i < data.d.length; i++) {
        $("#ddlSi").get(0).options[i] = new Option(data.d[i].Si, data.d[i].Si);
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 구 가져오기
**********************************************************************************************/
List.GetGu = function () {
    SiName = $("#ddlSi").val();

    if (SiName != "") {
        ParamArray = {
            'siName': SiName,
            'gubun': ZipCode
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("List.SuccessGetGu");
        var url = "/DeskPlusEIP/Common/Ws/ZipCode/ZipCode.asmx/GetGuCode";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 성공시 시 바인딩
**********************************************************************************************/
List.SuccessGetGu = function (data, status) {

    $("select[id='ddlGu']").find('option').each(function () {
        $(this).remove();
    });

    $("#ddlGu").get(0).options[0] = new Option(R_SelGu, "");

    for (var i = 0; i < data.d.length; i++) {
        $("#ddlGu").get(0).options[i + 1] = new Option(data.d[i].Gu, data.d[i].Gu);
    }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어
**********************************************************************************************/
List.Resource = function () {

    $("#header_1")[0].innerHTML = R_ZipCode;

    if (ZipCode == "1") {        
        $("#header_2")[0].innerHTML = R_Country;
        $("#header_3")[0].innerHTML = R_City;
        $("#header_4")[0].innerHTML = R_State;
        $("#header_5")[0].innerHTML = R_Detail;
        $("#header_7")[0].innerHTML = R_Building;
        $("#header_6")[0].innerHTML = R_HouseNumber;
    }
    else {

        $("#header_2")[0].innerHTML = R_StreetName;
    }



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

    var guName = $("#ddlGu").val();
    SearchValue = $("#ctbSearch").val().securityText();
    var url = "/DeskPlusEIP/Common/Ws/ZipCode/ZipCode.asmx/GetZipCode";

    if (SearchValue == "") {
        if (ZipCode == "1") {
            alert(R_SearchValue1);
        }
        else {
            alert(R_SearchValue2);
        }
        $("#ctbSearch").focus();
        return;
    }
    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        //파라미터값 배열에 담기
        ParamArray = {
            'siName': SiName,
            'guName': guName,
            'dongName': SearchValue,
            'gubun': ZipCode
        };


        var obj = eval("List.SuccessGetList");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }, 100);

}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
List.SuccessGetList = function (data, status) {

    if (ZipCode == "1") {
        $('#placeholder').setTemplateURL('/DeskPlusEIP/Common/Jt/ZipCode/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    }
    else {
        $('#placeholder').setTemplateURL('/DeskPlusEIP/Common/Jt/ZipCode/JTListNew.htm?seq=' + Guids(), null, { filter_data: false });
    }
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
List.GetAddress = function (zipcode, ido, si, dong, ri, building, bunji) {

    ido = encodeURIComponent(ido);
    si = encodeURIComponent(si);
    dong = encodeURIComponent(dong);
    ri = encodeURIComponent(ri);
    building = encodeURIComponent(building);
    bunji = encodeURIComponent(bunji);

    if (ri != "") {
        dong = dong + " " + ri;
    }

    if (building != "") {
        dong = dong + " " + building;
    }

    //도로명 우편번호만 적용됨 (shimpark)
    if (ZipCode != "1") {
        if (bunji != "") {
            dong = dong + " " + bunji;
        }
    }

    //    var obj = eval("parent." + CallBackPage);
    //    obj(zipcode, ido, si, dong);
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
작성목적 : 도로명 검색 시 ID값으로 주소 가져오기
***************************************************/
List.GetNewAddress = function (p_zipcode, p_si, p_gu, p_dong, p_ri, p_building, p_bunji) {
    zipcode = p_zipcode;
    ido = p_si;
    si = p_gu;
    dong = p_dong;
    ri = p_ri;
    building = p_building;
    bunji = p_bunji;

    List.GetAddress(zipcode, ido, si, dong, ri, building, bunji);
}

/*
List.GetNewAddress = function (code) {
    if (code != "") {
        var url = "/DeskPlusEIP/Common/Ws/ZipCode/ZipCode.asmx/GetNewAddress";

        ParamArray = {
            'code': code
        };


        var obj = eval("List.SuccessGetNewAddress");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}
*/

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 주소 갖고오기 성공
**********************************************************************************************/
/*
List.SuccessGetNewAddress = function (data, status) {

    zipcode = data.d[0].ZipCode;
    ido = data.d[0].Si;
    si = data.d[0].Gu;
    dong = data.d[0].Dong;
    ri = data.d[0].Ri;
    building = data.d[0].Building;
    bunji = data.d[0].Bunji
    alert(zipcode + " : " + ido + " : " + si + " : " + dong + " : " + ri + " : " + building + " : " + bunji);
    List.GetAddress(zipcode, ido, si, dong, ri, building, bunji);
}
*/

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
    ZipCode = $('input[name="rbLZipCode"]:checked').val();

    // 검색어 초기화(동)
    $("#ctbSearch").val("");
    // 리스 트초기화
    $('#placeholder').html("");
    // 시 바인딩
    List.GetSi();
}