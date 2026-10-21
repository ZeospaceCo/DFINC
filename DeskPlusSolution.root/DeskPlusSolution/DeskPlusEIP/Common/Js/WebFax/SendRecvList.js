SendRecvList = {}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            SendRecvList.LoadData("");
        }
    }

    $(document).ready(function () {

        //닫기
        $('#CbtnClose').click(function (e) {
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
            RecordPerPage = $('#ddlListCount').val();

            CurrentPage = 1; //1페이지로 이동처리  
            SendRecvList.LoadData();
        });

        SendRecvList.LoadData("");
    });
});


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
SendRecvList.Resource = function () {
    $("#j_recvName")[0].innerHTML = R_FaxRecvName;
    $("#j_nationcode")[0].innerHTML = R_FaxNationalCode;
    $("#j_faxnum")[0].innerHTML = R_FaxNumber;
    $("#j_endtime")[0].innerHTML = R_FaxSendDate;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 초기화
***************************************************/
SendRecvList.LoadData = function (gubun) {

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
    $('#loading').html(loadingPath);

    setTimeout(function () {
        SendRecvList.GetCount();
        SendRecvList.GetPageList();
    }, 100);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
SendRecvList.GetPageList = function () {
    
    var url = "/DeskPlusEIP/Common/Ws/WebFax/SendRecvList.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'guid': Guid,
        'totalCount': TotalItemCount,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage
    };

    var obj = eval("SendRecvList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
SendRecvList.SuccessPageList = function (data, status) {
    
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Common/Jt/WebFax/JTSendRecvList.htm?seq=' + Guids());
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    SendRecvList.Resource();

    $("#scrollDiv").css({ 'overflow-y': 'auto', 'min-height': '320px', 'height': '320px' });
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
SendRecvList.GetCount = function () {

    var url = "/DeskPlusEIP/Common/Ws/WebFax/SendRecvList.asmx/GetListCount";

    ParamArray = {
        'guid': Guid
    };

    CallEncrypt(CallGetParam());    
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("SendRecvList.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
SendRecvList.SuccessCount = function (data, status) {
    TotalItemCount = data.d;

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}