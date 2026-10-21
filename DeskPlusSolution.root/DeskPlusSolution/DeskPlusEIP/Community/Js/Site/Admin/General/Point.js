var Point = {};


/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        Point.LoadData();
    });
});

/**************************************************
작성자 : shine
작성일자 : 20112-05-21
작성목적 : 데이터 초기화
***************************************************/
Point.LoadData = function () {
    Point.GetPageList();
}

/**************************************************
작성자 : shine
작성일자 : 2012-05-21
작성목적 : 데이터 가져오기
***************************************************/
Point.GetPageList = function () {

    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/General/Point.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'communityID': CommunityID,
        'order': Order
    };

    var obj = eval("Point.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**************************************************
작성자 : shine
작성일자 : 2012-05-21
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
Point.SuccessPageList = function (data, status) {
    $('#phPoint').setTemplateURL('/DeskPlusEIP/Community/Jt/Site/Admin/General/JTPoint.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phPoint').processTemplate(data.d);

    Point.Resource();
}

/**************************************************
작성자 : shine
작성일자 : 2012-05-21
작성목적 : 다국어
***************************************************/
Point.Resource = function () {

    $("#r_number")[0].innerHTML = R_Number;
    $("#r_category")[0].innerHTML = R_CategoryName;
    $("#r_point")[0].innerHTML = R_PointScore;
    $("#r_writeName")[0].innerHTML = R_Registrant;
    $("#r_writeDate")[0].innerHTML = R_RegDate;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

    if ($(".PointImg") != undefined) { $(".PointImg").attr('src', ImagePath + '/Icon/ico_tree_listEdit.gif'); }
}
/**************************************************
작성자 : shine
작성일자 : 2012-05-21
작성목적 : 수정할 포인트 선택 
***************************************************/
Point.GetPoint = function (_communityID, _type) {

    ParamArray = {
        'communityID': _communityID,
        'type': _type
    };

    CallEncrypt(CallGetParam());

    var url = "./Common/GetPoint.aspx?p=" + GetEncryptValue;
    PopupWin(300, 123, url, "50", "", true);
}