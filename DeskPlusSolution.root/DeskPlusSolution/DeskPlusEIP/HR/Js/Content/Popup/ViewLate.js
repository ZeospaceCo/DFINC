ViewLate = {}

$(function () {

    $(document).ready(function () {

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        $('#ddlUser').change(function () {
            UserID = $('#ddlUser').val();
            ViewLate.GoPageLoad();
        });

        ViewLate.Init();
        //데이터 로드
        ViewLate.LoadData("");
    });
});


/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-21
작성목적 : 초기화
***************************************************/
ViewLate.Init = function () {

}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 초기화
***************************************************/
ViewLate.LoadData = function (gubun) {

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        ViewLate.GetPageList();
    }, 100);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 호출
**********************************************************************************************/
ViewLate.GetPageList = function () {
    var url = "/DeskPlusEIP/HR/Ws/Content/Popup/ViewLate.asmx/GetLateList";

    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'pathID': PathID,
        'deptBase': DeptBase,
        'startDate': StartDate,
        'endDate': EndDate,
        'minClosingID': MinClosingID,
        'maxClosingID': MaxClosingID,
        'gubun': Gubun
    };

    var obj = eval("ViewLate.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
ViewLate.SuccessPageList = function (data, status) {
    if (Gubun == "1") {
        $('#placeholder').setTemplateURL('/DeskPlusEIP/HR/Jt/Content/Popup/JTListViewLate.htm?seq=' + Guids(), null, { filter_data: false });
    }
    else if (Gubun == "2") {
        $('#placeholder').setTemplateURL('/DeskPlusEIP/HR/Jt/Content/Popup/JTListViewEarly.htm?seq=' + Guids(), null, { filter_data: false });
    }
    else if (Gubun == "3") {
        $('#placeholder').setTemplateURL('/DeskPlusEIP/HR/Jt/Content/Popup/JTListViewOver.htm?seq=' + Guids(), null, { filter_data: false });
    }

    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ViewLate.Resource();
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ViewLate.Resource = function () {

    $("#r_Gubun")[0].innerHTML = R_Gubun;     //구분
    $("#r_DeptName")[0].innerHTML = R_DeptName;     //부서명
    $("#r_Name")[0].innerHTML = R_Name;     //이름
    $("#r_TitleName")[0].innerHTML = R_TitleName;     //직급
    $("#r_CalcDate")[0].innerHTML = R_CalcDate;     //일자
    $("#r_Time")[0].innerHTML = R_Time;     //시간
    $("#r_Reason2")[0].innerHTML = R_Reason2;     //사유
    if ($("#r_docview").length > 0) {
        $("#r_docview")[0].innerHTML = R_DocView; //결재승인
    }

    //데이터없음
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-04-17
작성목적 : 휴가원 문서보기
***************************************************/
ViewLate.DocView = function (docID) {

    ParamArray = {
        'documentID': docID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;

    window.open(url, "View" + docID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2016-02-19
작성목적 : 페이지이동 처리
**********************************************************************************************/
ViewLate.GoPageLoad = function () {

    ParamArray = {
        'userID': UserID,
        'userName': UserName,
        'deptID': DeptID,
        'pathID': PathID,
        'deptBase': DeptBase,
        'startDate': StartDate,
        'endDate': EndDate,
        'minClosingID': MinClosingID,
        'maxClosingID': MaxClosingID,
        'gubun': Gubun
    };

    CallEncrypt(CallGetParam());
    location = "/DeskPlusEIP/HR/Content/Popup/ViewLate.aspx?p=" + GetEncryptValue;
}