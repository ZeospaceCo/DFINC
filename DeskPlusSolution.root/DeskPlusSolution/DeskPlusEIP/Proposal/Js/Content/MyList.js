MyList = {}

/**************************************************
초기화
***************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            //MyList.LoadData("");
            MyList.GoPageLoad();
        }
    }

    $(document).ready(function () {

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            ListCount: ListCount,
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
            ListCount = $('#ddlListCount').val();
            CurrentPage = 1; //1페이지로 이동처리  
            MyList.LoadData("search");
        });

        ListCount = $("#ddlListCount").val();

        MyList.Init();
    });
});

/**************************************************
작성자   : shine
작성일자 :2011-06-15
작성목적 : 초기화 하기
***************************************************/
MyList.Init = function () {

    MyList.LoadData();
    __addEventListener(document, "click", MyList.click);

    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 리스트 초기화
***************************************************/
MyList.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, ListCount);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadProposalData = function () {
        MyList.GetCount(); //검색 총 카운트 가져오기
        MyList.GetPageList(); //검색 리스트 가져오기
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadProposalSearchData = function () {
        MyList.GetCount(); //검색 총 카운트 가져오기
        MyList.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                //MyList.LoadData("search");
                MyList.GoPageLoad();
            }
        }

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            ListCount: ListCount,
            currentPage: CurrentPage,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });
    }

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        if (gubun == "search") {
            loadProposalSearchData();
        }
        else {
            loadProposalData();
        }
    }, 100);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
MyList.GetCount = function () {

    var url = "/DeskPlusEIP/Proposal/Ws/Content/MyList.asmx/GetPageCount";

    ParamArray = {
        'gubun': 'User'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("MyList.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
MyList.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, ListCount);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
MyList.GetPageList = function () {

    var url = "/DeskPlusEIP/Proposal/Ws/Content/MyList.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': ListCount,
        'pageSizePageNo': CurrentPage,
        'order': Order,
        'gubun': 'User'
    };

    var obj = eval("MyList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
MyList.SuccessPageList = function (data, status) {

    $('#phMyList').setTemplateURL('/DeskPlusEIP/Proposal/Jt/Content/JTMyList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phMyList').processTemplate(data.d);
    //$('#loading').html("");

    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }

}

/**********************************************************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 제안보기
**********************************************************************************************/
MyList.View = function (p_proposalNo) {

    var listPage = "../Content/View.aspx";

    ParamArray = {
        'currentPage': CurrentPage,
        'order': Order,
        'listCount': ListCount,
        'proposalNo': p_proposalNo,
        'menuID': 'MyList'
    };

    CallEncrypt(CallGetParam());
    parent.frmContent.location = listPage + "?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
MyList.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": MyList.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}


var oContextMenuCtrl = null;

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-22
작성목적 : 페이지이동 처리
**********************************************************************************************/
MyList.GoPageLoad = function () {


    ParamArray = {
        'currentPage': CurrentPage,
        'order': Order,
        'listCount': ListCount
    };

    CallEncrypt(CallGetParam());
    location = "MyList.aspx?p=" + GetEncryptValue;
}


