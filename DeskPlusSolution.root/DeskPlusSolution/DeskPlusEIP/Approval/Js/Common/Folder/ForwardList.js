ForwardList = {}

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ForwardList.LoadData("");
        }
    }


    $(document).ready(function () {

        $('#CbtnClose').click(function (e) {
            e.preventDefault();

            if (opener) { self.close(); }
            else {
                if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
            }
        });


        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            RecordPage: RecordPage,
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
            ForwardList.PageSize();
        });

        ForwardList.LoadData("");
    });
});

/**************************************************
페이지 사이즈에 이동하기
***************************************************/
ForwardList.PageSize = function () {

    var iPageUrl = $.url(PageUrl);
    var iPath = iPageUrl.attr('path');

    ParamArray = {
        'childDocID': DocumentID,
        'RecordPage': $('#ddlListCount').val()
    };
    CallEncrypt(CallGetParam());

    //이동하기
    location = iPath + "?p=" + GetEncryptValue;
}


/**************************************************
초기화
***************************************************/
ForwardList.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
    $('#loading').html(loadingPath);

    setTimeout(function () {
        ForwardList.GetPageList();
    }, 100);
}


/**********************************************************************************************
다국어 처리
**********************************************************************************************/
ForwardList.Resource = function () {
    $("#j_SendName")[0].innerHTML = R_Forwarder;
    $("#j_Name")[0].innerHTML = R_Name;
    $("#j_ForwardingDate")[0].innerHTML = R_ForwardingDate;
    $("#j_ViewingDate")[0].innerHTML = R_ViewingDate;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}



/**************************************************
웹서비스 리스트 가져오기
***************************************************/
ForwardList.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Common/Folder/ForwardList.asmx/GetPageListViewer";

    //파라미터값 배열에 담기
    ParamArray = {
        'documentID': DocumentID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPage,
        'pageSizePageNo': CurrentPage
    };

    var obj = eval("ForwardList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
};


/**************************************************
웹서비스 리스트 가져오기 성공
***************************************************/
ForwardList.SuccessPageList = function (data, status) {

    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Common/Folder/JTForwardList.htm?seq=' + Guids());
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ForwardList.Resource();
}

