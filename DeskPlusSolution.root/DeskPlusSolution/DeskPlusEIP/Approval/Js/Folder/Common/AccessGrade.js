AccessGrade = {}



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

        AccessGrade.LoadData();

    });
});


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
AccessGrade.LoadData = function () {
    

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        AccessGrade.GetPageList();
    }    

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        loadBoardData();
    }, 100);
}



/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-06-13
작성목적 : 리스트 호출
**********************************************************************************************/
AccessGrade.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Admin/AccessGrade/List.asmx/GetListIsAccess";

    var obj = eval("AccessGrade.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-06-13
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
AccessGrade.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Folder/Common/JTAccessGrade.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    AccessGrade.Resource();
}


/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-06-13
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

AccessGrade.Resource = function () {

    $("#r_accessGrades")[0].innerHTML = R_AccessGrades;
    $("#r_titleName2")[0].innerHTML = R_TitleName2;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}
