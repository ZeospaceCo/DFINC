ViewAdmin = {}



/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ViewAdmin.LoadData("");
        }
    }

    $(document).ready(function () {

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            recordPerPage: RecordPerPage,
            currentPage: CurrentPage,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            ViewAdmin.SavePageSize();
            ViewAdmin.PageSize();
        });

        //엑셀로보기
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            ViewAdmin.ExcelDown();
        });

        ViewAdmin.LoadData("");
    });
});


/**************************************************
작성자   : 김형석
작성일자 : 2012-07-27
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
ViewAdmin.SavePageSize = function () {

    var url = "/DeskPlusEIP/ETC/Ws/Holiday/Popup/ViewAdmin.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Holiday",
        'pageSize': $("#ddlListCount").val(),
        'types': "ViewAdmin"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ViewAdmin.SuccessSavePageSize");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : 김형석
작성일자 : 2012-07-27
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
ViewAdmin.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}


/**************************************************
작성자   : 김형석
작성일자 : 2011-04-11
작성목적 : 페이지 사이즈에 이동하기
***************************************************/
ViewAdmin.PageSize = function () {
    var iPageUrl = $.url(PageUrl);
    var iPath = iPageUrl.attr('path');

    ParamArray = {
        'startDate': StartDate,
        'endDate': EndDate,
        'userID': UserID,
        'name': Name,
        'recordPerPage': $('#ddlListCount').val()
    };

    CallEncrypt(CallGetParam());

    //이동하기    
    location = iPath + "?p=" + GetEncryptValue;
}


/**********************************************************************************************
작성자   : 김형석
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
ViewAdmin.Resource = function () {
    $("#r_startdate")[0].innerHTML = R_StartDate;
    $("#r_starthalf")[0].innerHTML = R_StartHalf;
    $("#r_enddate")[0].innerHTML = R_EndDate;
    $("#r_endhalf")[0].innerHTML = R_EndHalf;
    $("#r_duration")[0].innerHTML = R_Duration;
    $("#r_gubun")[0].innerHTML = R_Gubun;
    $("#r_docview")[0].innerHTML = R_DocView;
    $("#r_holidel")[0].innerHTML = R_HoliDel;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}


/**************************************************
작성자   : 김형석
작성일자 : 2011-04-11
작성목적 : 초기화
***************************************************/
ViewAdmin.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
    $('#loading').html(loadingPath);

    setTimeout(function () {
        ViewAdmin.GetPageList();
    }, 100);
}


/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 :  리스트 호출
**********************************************************************************************/
ViewAdmin.GetPageList = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Holiday/Popup/ViewAdmin.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'startDate': StartDate,
        'endDate': EndDate,
        'userID': UserID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage
    };

    var obj = eval("ViewAdmin.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
ViewAdmin.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/ETC/Jt/Holiday/Popup/JTViewAdmin.htm?seq=' + Guids());
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ViewAdmin.Resource();

    $("#scrollDiv").css({ 'overflow-y': 'auto', 'min-height': '320px', 'height': '320px' });
}


/**************************************************
작성자   : 김형석
작성일자 : 2011-04-11
작성목적 : 휴가 내역
***************************************************/
ViewAdmin.DelHoli = function (usedID) {

    if (confirm(R_MSG_DELSUCCESS)) {

        var url = "/DeskPlusEIP/ETC/Ws/Holiday/Popup/ViewAdmin.asmx/DelHoli";

        ParamArray = {
            'usedID': usedID
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ViewAdmin.SuccessDelHoli");

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : 김형석
작성일자 : 2011-04-14
작성목적 : 성공시, 카테고리별 사용자 정보 저장하기
**********************************************************************************************/
ViewAdmin.SuccessDelHoli = function (data, status) {

    if (data.d == true) {
        ViewAdmin.PageSize();
    }
    else {
        alert(R_MSG_DELFAIL);
    }
}


/**************************************************
작성자   : 김형석
작성일자 : 2011-04-11
작성목적 : 휴가원 문서보기
***************************************************/
ViewAdmin.DocView = function (docID) {

    ParamArray = {
        'documentID': docID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;

    window.open(url, "View" + docID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}


/**********************************************************************************************
작성자   : 김형석  
작성일자 : 2011-11-24
작성목적 : 엑셀
**********************************************************************************************/
ViewAdmin.ExcelDown = function () {
    ParamArray = {
        'userID': UserID,
        'startDate': StartDate,
        'endDate': EndDate
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    $("#hdexcel").val(GetEncryptValue);

    __doPostBack('lbtnexcel', "");
}