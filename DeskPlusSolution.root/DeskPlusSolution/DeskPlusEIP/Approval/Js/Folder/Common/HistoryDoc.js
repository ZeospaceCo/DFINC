HistoryDoc = {}



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

            if (opener) { self.close(); }
            else {
                if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
            }
        });

        HistoryDoc.LoadData();

    });
});


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
HistoryDoc.LoadData = function () {


    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        HistoryDoc.GetPageList();
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
HistoryDoc.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Common/HistoryDoc.asmx/GetList";

    ParamArray = {
        'childDocID': ChildDocID
    };

    var obj = eval("HistoryDoc.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
HistoryDoc.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Folder/Common/JTHistoryDoc.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    HistoryDoc.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

HistoryDoc.Resource = function () {

    $("#j_subject")[0].innerHTML = R_Subject;
    $("#j_username")[0].innerHTML = R_BaseName;
    $("#j_writedate")[0].innerHTML = R_WriteDate;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 결재 보기
***************************************************/
HistoryDoc.View = function (p_docID, p_historyID) {

    ParamArray = {
        'docID': p_docID,
        'historyID': p_historyID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Folder/Common/ViewHistory.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "_blank", "1200", "800")
}

