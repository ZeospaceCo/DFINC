var Grade = {};


/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        Grade.LoadData();
    });
});

/**************************************************
작성자 : shine
작성일자 : 20112-05-21
작성목적 : 데이터 초기화
***************************************************/
Grade.LoadData = function () {
    Grade.GetPageList();
}

/**************************************************
작성자 : shine
작성일자 : 2012-05-21
작성목적 : 데이터 가져오기
***************************************************/
Grade.GetPageList = function () {

    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/General/Grade.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'communityID': CommunityID,
        'order': Order
    };

    var obj = eval("Grade.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**************************************************
작성자 : shine
작성일자 : 2012-05-21
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
Grade.SuccessPageList = function (data, status) {
    $('#phGrade').setTemplateURL('/DeskPlusEIP/Community/Jt/Site/Admin/General/JTGrade.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phGrade').processTemplate(data.d);

    Grade.Resource();
}

/**************************************************
작성자 : shine
작성일자 : 2012-05-21
작성목적 : 다국어
***************************************************/
Grade.Resource = function () {
    $("#r_number")[0].innerHTML = R_Number;
    $("#r_grade")[0].innerHTML = R_GradeName;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

    if ($(".GradeImg") != undefined) { $(".GradeImg").attr('src', ImagePath + '/Icon/ico_tree_listEdit.gif'); }
}


/**************************************************
작성자 : shine
작성일자 : 2012-05-21
작성목적 : 수정할 포인트 선택 
***************************************************/
Grade.GetGrade = function (_communityID, _groupID) {

    ParamArray = {
        'communityID': _communityID,
        'groupID': _groupID
    };

    CallEncrypt(CallGetParam());

    var url = "./Common/GetGrade.aspx?p=" + GetEncryptValue;
    PopupWin(400, 387, url, "50", "", true);
}