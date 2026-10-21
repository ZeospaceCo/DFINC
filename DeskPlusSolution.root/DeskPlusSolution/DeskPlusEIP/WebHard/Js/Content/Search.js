Search = {};

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            Search.LoadData("");
        }
    }
    $(function () {
        $(document).ready(function () {

            $("#searchValue").val(TxtSearchValue);
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

            //검색입력필드 엔터 이벤트 기술
            $("#cbtnSearch").click(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault();
                    Search.Search();
                }
                Search.Search();
            });

            //검색입력필드 엔터 이벤트 기술
            $("#searchValue").keydown(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault();
                    Search.Search();
                }
            });

            //페이지 카운트
            $('#ddlListCount').change(function () {
                Search.ModCategoryUser("1");
            });

            Search.init();
            Search.LoadData("");

        })
    });
});

Search.init = function () {
    __addEventListener(document, "click", Search.click);
   
};



/**************************************************
작성자   : 이지원
작성일자 : 2017-07-06
작성목적 : 리스트 초기화
***************************************************/
Search.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        Search.List();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        Search.GetCount(); //검색 총 카운트 가져오기
        Search.List(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                Search.LoadData("search");
            }
        }

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
    }

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        if (gubun == "search") {

        }
        else {
            loadBoardData();
        }
    }, 100);
}




/**************************************************
작성자   : 이지원
작성일자 : 2017-07-06
작성목적 : 검색버튼 클릭시 페이지 다시 호출
***************************************************/ 

Search.Search = function () {
    
    var validator = new Validator($("#form1"));
    validator.checkRequired("searchValue", R_LengthOver, true);
    if (!validator.validate()) return;

    var searchVal = $("#searchValue").val();

    ParamArray = {       
        'searchValue': searchVal
    };

    CallEncrypt(CallGetParam());
    var param = "p=" + GetEncryptValue;

    location.href = "/DeskPlusEIP/WebHard/Content/Search.aspx?" + param;
};

/**************************************************
작성자   : 이지원
작성일자 : 2017-07-06
작성목적 : Home화면에서 검색버튼 클릭 했을때
***************************************************/
Search.List = function () {

    var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetSearchList";

    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchValue': TxtSearchValue
    };

    var obj = eval("Search.SuccessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
};

/**************************************************
작성자   : 이지원
작성일자 : 2017-07-06
작성목적 : 검색리스트 성공
***************************************************/
Search.SuccessList = function (data, status) {
    $('#phSearchList').setTemplateURL('/DeskPlusEIP/WebHard/Jt/Content/JTSearch.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phSearchList').processTemplate(data.d);
    Search.Resource();
};

/**************************************************
작성자   : 이지원
작성일자 : 2017-07-06
작성목적 : 새창으로 게시물 보기
***************************************************/
Search.Resource = function () {

    $("#r_folders")[0].innerHTML = R_Folders;
    $("#r_fileName")[0].innerHTML = R_FileName;
    $("#r_download")[0].innerHTML = R_Download;
    $("#r_fileLength")[0].innerHTML = R_FileLength;
    $("#r_fileType")[0].innerHTML = R_FileType;
    $("#r_regUser")[0].innerHTML = R_RegUser;
    $("#r_writeDate")[0].innerHTML = R_RegDate;

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }

    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
};

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-06
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
Search.GetCount = function () {
    var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetSearchCount";
    
    ParamArray = {
        'searchValue': SearchValue        
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Search.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-06
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
Search.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**************************************************
작성자   : 이지원
작성일자 : 2017-07-06
작성목적 : 카테고리별 사용자 설정 정보 저장하기
***************************************************/
Search.ModCategoryUser = function (gubun) {

    var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/ModFolderUser";

    ParamArray = {
        'folderID': "0",
        'listType': gubun,
        'pageSize': $("#ddlListCount").val(),
        'types': "SE"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Search.SuccessModFolderUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-06
작성목적 : 성공시, 카테고리별 사용자 정보 저장하기
**********************************************************************************************/
Search.SuccessModFolderUser = function (data, status) {
    if (data.d == true) {

        var searchVal = $("#searchValue").val();

        ParamArray = {
            'searchValue': searchVal,
            'currentPage': "1"
        };

        CallEncrypt(CallGetParam());
        var param = "p=" + GetEncryptValue;

        location.href = "/DeskPlusEIP/WebHard/Content/Search.aspx?" + param;
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-07-06
작성목적 : 클릭 이벤트 분기
***************************************************/
Search.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": Search.DownLoad(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

/**************************************************
작성자   : 이지원
작성일자 : 2017-07-06
작성목적 : 첨부파일 열기
***************************************************/
Search.DownLoad = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var attachFilePath = el.getAttribute("FilePath");
    attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + attachFilePath;

    window.open(attachFilePath);
}
