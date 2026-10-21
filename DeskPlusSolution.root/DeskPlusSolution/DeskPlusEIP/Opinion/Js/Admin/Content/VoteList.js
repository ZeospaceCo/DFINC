VoteList = {};

$(function () {

    //[닫기]
    $('#cbtnClose').click(function (e) { e.preventDefault(); if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); } });

    VoteList.LoadData();     //페이지가져오기

});


/***리스트 처리 및 페이징 시작***/

//리스트내용가져오기
VoteList.GetPageList = function () {
    var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/VoteList.asmx/GetVotePageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'quesID': QuesID,
        'unitID': UnitID,
        'unitItemID': UnitItemID,
        'unitItemCont': UnitItemCont,
        'type': Type,
        'pageSize': PageSize,
        'pageNo': PageNo
    };

    var obj = eval("VoteList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
VoteList.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Opinion/Jt/Admin/Content/JtVoteList.htm?seq=' + Guids(), null, { filter_data: false });

    $('#placeholder').processTemplate(data.d);
    //$('#loading').html("");

    VoteList.Resource();    //다국어처리
}

//리스트총개수가져오기
VoteList.GetPageCnt = function () {
    var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/VoteList.asmx/GetVotePageCnt";
    ParamArray = {
        'quesID': QuesID,
        'unitID': UnitID,
        'unitItemID': UnitItemID,
        'unitItemCont': UnitItemCont,
        'type': Type
    };

    var obj = eval("VoteList.SuccessPageCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
VoteList.SuccessPageCnt = function (data, status) {
    TotCnt = data.d;
    var pageCount = $("#pager").pager.refresh(TotCnt, PageSize);
    //$("#pageInfo").text(PageNo + " / " + pageCount);
}

//리스트가져오기
VoteList.LoadData = function () {
    //var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif'/>";
    //$('#loading').html(loadingPath);

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (sPage) {
            PageNo = sPage;
            VoteList.LoadData();
        }
    }

    // 페이저 초기화 (실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.)
    $("#pager").pager({
        pager: "#pager",        //pageID
        pagerMode: "text",      //text or image
        totalItemCount: TotCnt, //총카운트
        recordPerPage: PageSize, //페이지 보여질 개수
        currentPage: PageNo,    //페이지번호(1부터시작)
        pageIndexChanged: Pager.pageIndexChanged,   //event
        prev10Text: "Before", //이전
        next10Text: "After",     //다음
        prev10ImgSrc: "images/Prev10.gif",
        next10ImgSrc: "images/next10.gif",  //"<img src='" + ImagePath + "/Icon/loading.gif'/>";
        selectedPageFontColor: "orange"
    });

    setTimeout(function () { VoteList.GetPageList(); }, 100);
}
/***리스트 처리 및 페이징 끝***/

//다국어처리
VoteList.Resource = function () {
    $("#jtVL_F0").text(R_TitUnitAddF2);
    $("#jtVL_F1").text(R_JTF1);
    $("#jtVL_F2").text(R_JTF2);
    $(".jtNoData").text(R_JTNoData);
}