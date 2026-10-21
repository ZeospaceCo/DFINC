ReadList = {}

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ReadList.LoadData("");
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
            ReadList.PageSize();
        });

        ReadList.LoadData("");
    });
});

/**************************************************
작성자   : maniaysw
작성일자 : 2015-05-13
작성목적 : 페이지 사이즈에 이동하기
***************************************************/
ReadList.PageSize = function () {
    
    var iPageUrl = $.url(PageUrl);
    var iPath = iPageUrl.attr('path');
    
    ParamArray = {
        'listID': ListID,
        'recordPerPage': $('#ddlListCount').val()
    };

    CallEncrypt(CallGetParam());

    //이동하기
    location = iPath + "?p=" + GetEncryptValue;
}



/**************************************************
작성자   : maniaysw
작성일자 : 2015-05-12
작성목적 : 초기화
***************************************************/
ReadList.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
    $('#loading').html(loadingPath);

    setTimeout(function () {
        ReadList.GetPageList();
    }, 100);
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-05-13
작성목적 : 다국어 처리
**********************************************************************************************/
ReadList.Resource = function () {
    $("#j_DeptName")[0].innerHTML = R_DeptName;
    $("#j_TitleName")[0].innerHTML = R_TitleName;
    $("#j_Name")[0].innerHTML = R_Name;
    $("#j_ViewingDate")[0].innerHTML = R_ViewingDate;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}


/**************************************************
작성자   : maniaysw
작성일자 : 2015-05-12
작성목적 : 웹서비스 리스트 가져오기
***************************************************/
ReadList.GetPageList = function () {
    var url = "/DeskPlusEIP/Task/Ws/Common/ReadList.asmx/GetPageListViewer";

    //파라미터값 배열에 담기
    ParamArray = {       
        'listID': ListID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage
        
    };

    var obj = eval("ReadList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
};


/**************************************************
작성자   : maniaysw
작성일자 : 2015-05-12
작성목적 : 웹서비스 리스트 가져오기 성공
***************************************************/
ReadList.SuccessPageList = function (data, status) {
    
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Task/Jt/Common/JTReadList.htm?seq=' + Guids());
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ReadList.Resource();
}


