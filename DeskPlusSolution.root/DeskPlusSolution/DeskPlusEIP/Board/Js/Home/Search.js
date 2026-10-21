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
                Search.GoPageListType("1"); 
            });

            Search.init();
            Search.LoadData("");
            //setTimeout(Search.List, 1000);

        })
    });
});

Search.init = function () {
    __addEventListener(document, "click", Search.click);
   
};



/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
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
            //loadBoardSearchData();          
        }
        else {         
            loadBoardData();
        }
    }, 100);
}




/**************************************************
작성자   : 윤석원
작성일자 : 2016-03-16
작성목적 : 검색버튼 클릭시 페이지 다시 호출
***************************************************/ 

Search.Search = function () {
    
    var validator = new Validator($("#form1"));
    //validator.checkRequired("searchValue", "검색어를 입력해 주세요", true);
    validator.checkRequired("searchValue", R_LengthOver, true);
    if (!validator.validate()) return;

    var searchVal = $("#searchValue").val();

    ParamArray = {       
        'searchValue': searchVal
    };

    CallEncrypt(CallGetParam());
    var param = "p=" + GetEncryptValue;   

    location.href = "/DeskPlusEIP/Board/Home/Search.aspx?" + param;
};

/**************************************************
작성자   : 윤석원
작성일자 : 2016-03-16
작성목적 : Home화면에서 검색버튼 클릭 했을때
***************************************************/
Search.List = function () {

    var url = "/DeskPlusEIP/Board/Ws/Home/Search.asmx/GetSearchList";

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
작성자   : 윤석원
작성일자 : 2016-03-16
작성목적 : 검색리스트 성공
***************************************************/
Search.SuccessList = function (data, status) {
    //alert(data.d);

    $('#phSearchList').setTemplateURL('/DeskPlusEIP/Board/Jt/Home/JTSearch.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phSearchList').processTemplate(data.d);

    Search.Resource();
};

/**************************************************
작성자   : 윤석원
작성일자 : 2016-03-16
작성목적 : 새창으로 게시물 보기
***************************************************/
Search.Resource = function () {

    $("#r_number")[0].innerHTML = R_Number;
    $("#r_folder")[0].innerHTML = R_CategoryName;
    $("#r_title")[0].innerHTML = R_Title;
    $("#r_attachFile")[0].innerHTML = R_AttachName;
    $("#r_readCount")[0].innerHTML = R_ViewCount;
    $("#r_writeName")[0].innerHTML = R_Register;
    $("#r_writeDate")[0].innerHTML = R_RegDate;

    if ($("#W_NoListData")[0] != undefined) {
        $("#W_NoListData")[0].innerHTML = R_NotExistList;
    }

};


/**************************************************
작성자   : 윤석원
작성일자 : 2016-03-16
작성목적 : 새창으로 게시물 보기
***************************************************/
Search.PopupView = function (p_categoryID, p_listID) {
    ParamArray = {
        'catID': p_categoryID,
        'categoryID': p_categoryID,
        'listID': p_listID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Board/Content/ViewPopup.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "ShowImage", "1200", "800", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}



/**************************************************
작성자   : 윤석원
작성일자 : 2016-03-16
작성목적 : 클릭 이벤트 분기
***************************************************/
Search.click = function (e) {    
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": Search.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

Search.GetAttachFileListMenu = function (p_categoryID, p_listID) {
    var url = "/DeskPlusEIP/Board/Ws/Home/Search.asmx/GetAttachFileListMenu";
    //var data = "{categoryID :" + categoryID + ",listID:" + listID + "}";
    var data = "categoryID=" + p_categoryID + "&listID=" + p_listID;
    var obj = eval("Search.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 윤석원
작성일자 : 2016-03-16
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
Search.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}

/**************************************************
작성자   : 윤석원
작성일자 : 2016-03-16
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Search.ShowAttachContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var categoryID = el.getAttribute("CategoryID");
    var listID = el.getAttribute("ListID");

    Search.GetAttachFileListMenu(categoryID, listID);

    if (AttachFileInfoList == "0") {
        alert(R_NoFileData);
        return;
    }
    else if (AttachFileInfoList == "1") {
        alert(R_ErrorAttachFile);
        return;
    }
    else {
        oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
        oContextMenuCtrl.init();

        var attachArr = AttachFileInfoList.split("|");
        var attachFile = "";
        var attachFileName = "";
        var attachFilePath = "";

        /*
        if ($.browser.msie) {
            if (attachArr.length > 1) {
                oItem = oContextMenuCtrl.addItem(R_AllDownLoad, Search.TotalDownLoad, ImagePath + "/Icon/folders.gif");
                oItem.txt.setAttribute("arg", AttachFileInfoList, 0);
                oContextMenuCtrl.addBlank();
            }
        }
        */

        for (i = 0; i < attachArr.length; i++) {
            attachFile = attachArr[i].split(":");
            attachFileName = attachFile[0];
            attachFilePath = attachFile[1];

            attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + attachFilePath;

            oItem = oContextMenuCtrl.addItem(attachFileName, Search.DownLoad, "EXT");
            oItem.txt.setAttribute("arg", attachFilePath, 0);
        }
        oContextMenuCtrl.show(e);
    }
}

/**************************************************
작성자   : 윤석원
작성일자 : 2016-03-16
작성목적 : 첨부파일 열기
***************************************************/
Search.DownLoad = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        //$("#ifrmAttachFile")[0].src = arg;
        window.open(arg);
    }
}

/**************************************************
작성자   : 윤석원
작성일자 : 2016-03-16
작성목적 : 첨부파일 전체 다운로드
***************************************************/
Search.TotalDownLoad = function (_e) {
    var evt = new __event(_e);

    if (evt.target) {
        var arg = evt.target.getAttribute("arg");

        $("#fileList").val(arg);

        document.getElementById("frmTotalDownLoad").action = "/DeskPlusEIP/Common/DownLoad/TotalDownLoad.aspx";
        //$("#frmTotalDownLoad").action = "/DeskPlusEIP/Common/DownLoad/TotalDownLoad.aspx";

        PopupWinOpen("", "", "openWin", "600", "400")
        //window.open("", "openWin", "width=506px,height=388px,scrollbars=no,status=no,toolbar=no,resizable=0,location=no,menu=no");
        //$("#frmTotalDownLoad").submit();
        document.getElementById("frmTotalDownLoad").submit();
    }
}


/**********************************************************************************************
작성자   : 윤석원
작성일자 : 2016-03-16
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
Search.GetCount = function () {
    var url = "/DeskPlusEIP/Board/Ws/Home/Search.asmx/GetSearchCount";
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'searchValue': SearchValue        
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Search.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 윤석원
작성일자 : 2016-03-16
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
Search.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**************************************************
작성자   : 윤석원
작성일자 : 2016-03-16
작성목적 : 카테고리별 사용자 설정 정보 저장하기
***************************************************/
Search.ModCategoryUser = function (gubun) {

    var url = "/DeskPlusEIP/Board/Ws/Home/MoreList.asmx/ModCategoryUser";
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'catID': 0,
        'listType': gubun,
        'pageSize': $("#ddlListCount").val(),
        'types': 'BL'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Search.SuccessModCategoryUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 윤석원
작성일자 : 2016-03-16
작성목적 : 성공시, 카테고리별 사용자 정보 저장하기
**********************************************************************************************/
Search.SuccessModCategoryUser = function (data, status) {
    if (data.d == true) {
    }
}



/**********************************************************************************************
작성자   : 윤석원
작성일자 : 2016-03-16
작성목적 : 리스트 타입선택시 이동하기
**********************************************************************************************/
Search.GoPageListType = function (gubun) {
    //alert(HomeCategory);
    var iPageUrl = $.url(PageUrl);
    var iPath = iPageUrl.attr('path');   

    switch (gubun) {
        case "1":
            iPath = iPath.replace("/BlogList.aspx", "/Search.aspx");            
            break;      
    }

    CurrentPage = 1; //1페이지로 이동처리

    ParamArray = {
        'currentPage': CurrentPage,
        'searchValue': TxtSearchValue
       
    };

    CallEncrypt(CallGetParam());

    //이동하기
    location = iPath + "?p=" + GetEncryptValue;
}

/**************************************************
작성목적 : 새창으로 댓글리스트 보기
***************************************************/
Search.PopupShort = function (p_categoryID, p_listID) {
    ParamArray = {
        'categoryID': p_categoryID,
        'listID': p_listID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Board/Content/ViewShortReplyList.aspx?p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, "", "ViewShortReplyList" + Guids(), "600", "690", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

/**************************************************
작성목적 : 조회현황
***************************************************/
Search.ReadList = function (p_categoryID, p_listID) {
    ParamArray = {
        'categoryID': p_categoryID,
        'listID': p_listID
    };

    CallEncrypt(CallGetParam())

    var url = "/DeskPlusEIP/Board/Content/ReadList.aspx?p=" + GetEncryptValue;

    PopupWin(500, 400, url, "", "", true);
}
