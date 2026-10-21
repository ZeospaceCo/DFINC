var Menu = {};

/**************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 초기화
***************************************************/
$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (AccordionActive == "0") {
                CurrentPage1 = selectedPage;

                $("#pager2").pager({
                    pager: "#pager2",
                    pagerMode: "text", //text, image 중 선택
                    totalItemCount: TotalItemCount1,
                    recordPerPage: RecordPerPage1,
                    currentPage: CurrentPage1,
                    pageIndexChanged: Pager.pageIndexChanged,
                    prev10Text: R_Previous,
                    next10Text: R_Next,
                    prev10ImgSrc: "images/Prev10.gif",
                    next10ImgSrc: "images/next10.gif",
                    selectedPageFontColor: "orange"
                });

                Menu.LoadOpenData("");
            }
            else {
                CurrentPage = selectedPage;

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

                Menu.LoadData("");
            }
        }
    }

    $(document).ready(function () {
        RecordPerPage = $("#ddlListCount").val();
        RecordPerPage1 = $("#ddlListCount1").val();

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager2").pager({
            pager: "#pager2",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount1,
            recordPerPage: RecordPerPage1,
            currentPage: CurrentPage1,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });

        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            //event: "mouseover",
            active: AccordionActive
        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () { $("#accordion").accordion("resize"); }
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                Menu.Search();
            }
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch1").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                Menu.OpenSearch();
            }
        });

        //검색
        $('#CbtnDeptUserSearch').click(function (e) {
            e.preventDefault();
            Menu.Search();
        });

        $('#CbtnDeptUserSearch1').click(function (e) {
            e.preventDefault();
            Menu.OpenSearch();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            Menu.ModFolderUser('T');
            Menu.Search();
        });

        //페이지 카운트
        $('#ddlListCount1').change(function () {
            Menu.ModFolderUser('O');
            Menu.OpenSearch();
        });

        //ShareUser
        $('#ShareUser').click(function () {
            AccordionActive = 1;

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

            if (AccordionCheck == "0") {
                AccordionCheck = "1";
                Menu.Search();
            }
        });

        $('#OpenUser').click(function () {
            AccordionActive = 0;

            $("#pager2").pager({
                pager: "#pager2",
                pagerMode: "text", //text, image 중 선택
                totalItemCount: TotalItemCount1,
                recordPerPage: RecordPerPage1,
                currentPage: CurrentPage1,
                pageIndexChanged: Pager.pageIndexChanged,
                prev10Text: R_Previous,
                next10Text: R_Next,
                prev10ImgSrc: "images/Prev10.gif",
                next10ImgSrc: "images/next10.gif",
                selectedPageFontColor: "orange"
            });
        });


        //검색어입력
        $("#ctbSearch1").attr("placeholder", R_PlaceholderText);
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        Menu.Init();
    });
});

/**************************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 초기화
**************************************************************/
Menu.Init = function () {
    Menu.ContentFrame = parent.frmContent;
    //Menu.GoListPage();

    Menu.LoadOpenData("");
    //Menu.LoadData("");
}

/**************************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 리스트 페이지 호출하기
**************************************************************/
Menu.GoListPage = function () {
    ParamArray = {
        'rUserID': RUserID
    };

    CallEncrypt(CallGetParam());

    var listPage = "../Content/List.aspx";

    if (listPage != "" && Menu.ContentFrame) {
        Menu.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 리스트 초기화
***************************************************/
Menu.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadSNSData = function () {
        Menu.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadSNSSearchData = function () {
        Menu.GetCount(); //검색 총 카운트 가져오기
        Menu.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                Menu.LoadData("search");
            }
        }

        RecordPerPage = $("#ddlListCount").val();

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

    setTimeout(function () {
        if (gubun == "search") {
            loadSNSSearchData();
        }
        else {
            loadSNSData();
        }
    }, 100);
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-16
작성목적 : 리스트 초기화 (나의 담벼락을 보는 사람)
***************************************************/
Menu.LoadOpenData = function (gubun) {
    var pageCount = $("#pager2").pager.refresh(TotalItemCount1, RecordPerPage1);
    $("#pageInfo1").text(CurrentPage1 + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadSNSOpenData = function () {
        Menu.GetOpenPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadSNSOpenSearchData = function () {
        Menu.GetOpenCount(); //검색 총 카운트 가져오기
        Menu.GetOpenPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage1 = selectedPage;
                Menu.LoadOpenData("search");
            }
        }

        RecordPerPage1 = $("#ddlListCount1").val();

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager2").pager({
            pager: "#pager2",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount1,
            recordPerPage: RecordPerPage1,
            currentPage: CurrentPage1,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });
    }

    setTimeout(function () {
        if (gubun == "search") {
            loadSNSOpenSearchData();
        }
        else {
            loadSNSOpenData();
        }
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Menu.GetPageList = function () {
    var url = "/DeskPlusEIP/SNS/Ws/MyTalk/Menu/Menu.asmx/GetPageList"

    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchValue': SearchValue
    };

    var obj = eval("Menu.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Menu.SuccessPageList = function (data, status) {
    $('#phShareList').setTemplateURL('/DeskPlusEIP/SNS/Jt/MyTalk/Menu/JTShareList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phShareList').processTemplate(data.d);
    $('#loading').html("");

    Menu.Resource();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Menu.Resource = function () {
    $("#r_name")[0].innerHTML = R_Name;
    $("#r_photo")[0].innerHTML = R_Photo;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Menu.GetCount = function () {
    var url = "/DeskPlusEIP/SNS/Ws/MyTalk/Menu/Menu.asmx/GetListCount";
    ParamArray = {
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Menu.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Menu.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-16
작성목적 : 나의 담벼락을 보는 사람 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Menu.GetOpenPageList = function () {
    var url = "/DeskPlusEIP/SNS/Ws/MyTalk/Menu/Menu.asmx/GetOpenPageList"

    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount1,
        'pageSize': RecordPerPage1,
        'pageSizePageNo': CurrentPage1,
        'searchValue': SearchValue1
    };

    var obj = eval("Menu.SuccessOpenPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-16
작성목적 : 나의 담벼락을 보는 사람 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Menu.SuccessOpenPageList = function (data, status) {
    $('#phOpenList').setTemplateURL('/DeskPlusEIP/SNS/Jt/MyTalk/Menu/JTOpenList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phOpenList').processTemplate(data.d);
    $('#loading').html("");

    Menu.ResourceOpen();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Menu.ResourceOpen = function () {
    $("#r_nameOpen")[0].innerHTML = R_Name;
    $("#r_photoOpen")[0].innerHTML = R_Photo;

    if ($(".NoOpenListData") != undefined) { $(".NoOpenListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-16
작성목적 : 나의 담벼락을 보는 사람 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Menu.GetOpenCount = function () {
    var url = "/DeskPlusEIP/SNS/Ws/MyTalk/Menu/Menu.asmx/GetOpenListCount";
    ParamArray = {
        'searchValue': SearchValue1
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Menu.SuccessOpenCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-16
작성목적 : 나의 담벼락을 보는 사람 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Menu.SuccessOpenCount = function (data, status) {
    TotalItemCount1 = data.d;
    var pageCount = $("#pager1").pager.refresh(TotalItemCount1, RecordPerPage1);
    $("#pageInfo1").text(CurrentPage1 + " / " + pageCount);
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 검색
***************************************************/
Menu.Search = function () {
    var response = Menu.SettingSearch();
    if (response == false) return;

    RecordPerPage = $("#ddlListCount").val();
    CurrentPage = 1; //1페이지로 이동처리

    Menu.LoadData("search");
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 검색 필드 설정하기
***************************************************/
Menu.SettingSearch = function () {
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    return true;
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 검색
***************************************************/
Menu.OpenSearch = function () {
    var response = Menu.SettingOpenSearch();
    if (response == false) return;

    RecordPerPage1 = $("#ddlListCount1").val();
    CurrentPage1 = 1; //1페이지로 이동처리

    Menu.LoadOpenData("search");
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 검색 필드 설정하기
***************************************************/
Menu.SettingOpenSearch = function () {
    SearchValue1 = $("#ctbSearch1").val().securityText();
    $("#ctbSearch1").val(SearchValue1);

    return true;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-14
작성목적 : 사용자 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
Menu.ModFolderUser = function (_types) {
    var tmpPageSize = "";
    if (_types == "T") {
        tmpPageSize = $("#ddlListCount").val();
    }
    else {
        tmpPageSize = $("#ddlListCount1").val();
    }

    var url = "/DeskPlusEIP/SNS/Ws/MyTalk/Menu/Menu.asmx/ModFolderUser";
    ParamArray = {
        'pageSize': tmpPageSize,
        'types': _types
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Menu.SuccessModFolderUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-14
작성목적 : 사용자 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
Menu.SuccessModFolderUser = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-16
작성목적 : 사용자 선택시에
**********************************************************************************************/
Menu.GoPopMyTalkList = function (p_userID) {
    ParamArray = {
        'rUserID': p_userID
    };

    CallEncrypt(CallGetParam());

    var listPage = "../Content/List.aspx";

    if (listPage != "" && Menu.ContentFrame) {
        Menu.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}