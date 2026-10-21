Content = {}
var oContextMenuCtrl = null;
/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-07
작성목적 : 초기화 
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            Content.LoadData("");
        }
    }

    $(document).ready(function () {

        // 대문관리
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            parent.document.location.href = "../Admin/Default.aspx";
            return;
        });


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

        //페이지 카운트
        $('#ddlListCount').change(function () {
            Content.ModSetting();
            RecordPerPage = $("#ddlListCount").val();
            if (TabGubun == "2") {
                Content.JoinSearch();
            }
            else {
                Content.Search();
            }

        });

        Content.LoadData("");

        $(".clsDate").keydown(function (e) { return false; });

    });
});


/**************************************************
작성자   : famo23
작성일자 : 2012-05-07
작성목적 : 리스트 초기화
***************************************************/
Content.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadCMNData = function () {
        Content.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadCMNSearchData = function () {
        Content.GetCount(); //검색 총 카운트 가져오기
        Content.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                Content.LoadData("search");
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
            loadCMNSearchData();
        }
        else {
            loadCMNData();
        }
    }, 100);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-07
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Content.GetPageList = function () {

    var url = "/DeskPlusEIP/Community/Ws/Main/Content.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'order': Order
    };

    var obj = eval("Content.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-07
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Content.SuccessPageList = function (data, status) {

    $('#phList').setTemplateURL('/DeskPlusEIP/Community/Jt/Main/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);

    Content.Resource();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-07
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Content.Resource = function () {
    $("#r_name")[0].innerHTML = R_CommunityName;
    $("#r_writeUserName")[0].innerHTML = R_ComWriteName;
    $("#r_description")[0].innerHTML = R_CommunityDes;
    $("#r_count")[0].innerHTML = R_Membership;
    $("#r_boaCount")[0].innerHTML = R_TodayCount;    
    $("#r_type")[0].innerHTML = R_CommunityType;


    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-07
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Content.GetCount = function () {
    var url = "/DeskPlusEIP/Community/Ws/Main/Content.asmx/GetListCount";
    var obj = eval("Content.SuccessCount");

    AjaxService(url, "", obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-07
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Content.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-05-07
작성목적 : 검색
***************************************************/
Content.Search = function () {

    CurrentPage = 1; //1페이지로 이동처리    

    Content.LoadData("search");
}


/**************************************************
작성자   : famo23
작성일자 : 2012-05-07
작성목적 : 가입 신청한 커뮤니티 리스트 초기화
***************************************************/
Content.LoadJoinData = function (gubun) {

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadCMNJoinSearchData = function () {
        Content.GetJoinCount(); //검색 총 카운트 가져오기
        Content.GetJoinPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                Content.LoadJoinData("search");
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
        loadCMNJoinSearchData();

    }, 100);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-07
작성목적 : 가입 신청한 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Content.GetJoinPageList = function () {

    var url = "/DeskPlusEIP/Community/Ws/Main/Content.asmx/GetJoinPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'order': 'ApplyDate desc'
    };

    var obj = eval("Content.SuccessJoinPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-07
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Content.SuccessJoinPageList = function (data, status) {

    $('#phJoin').setTemplateURL('/DeskPlusEIP/Community/Jt/Main/JTJoinList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phJoin').processTemplate(data.d);

    Content.JoinResource();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-07
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Content.JoinResource = function () {
    $("#r_cname")[0].innerHTML = R_CommunityName;    
    $("#r_joindes")[0].innerHTML = R_JoinDes;
    $("#r_applyDateStr")[0].innerHTML = R_ApplyDate;
    $("#r_cancel")[0].innerHTML = R_Cancel;
    $(".btn_del").text(R_Del);

    if ($("#NoJoinListData")[0] != undefined) {
        $("#NoJoinListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-07
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Content.GetJoinCount = function () {
    var url = "/DeskPlusEIP/Community/Ws/Main/Content.asmx/GetJoinListCount";
    var obj = eval("Content.SuccessJoinCount");

    AjaxService(url, "", obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-07
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Content.SuccessJoinCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-05-07
작성목적 : 검색
***************************************************/
Content.JoinSearch = function () {

    CurrentPage = 1; //1페이지로 이동처리    

    Content.LoadJoinData("search");
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-07
작성목적 : 탭 디자인 적용
**********************************************************************************************/
Content.SetTab = function (arg) {
    if (arg == "1") {
        $("#tabDivInfo").attr("class", "tab_on");
        $("#divInfo").show();


        $("#tabDivJoin").attr("class", "");
        $("#divJoin").hide();

        TabGubun = "1";
        Content.Search();

    } else {
        $("#tabDivInfo").attr("class", "");
        $("#divInfo").hide();

        $("#tabDivJoin").attr("class", "tab_on");
        $("#divJoin").show();

        TabGubun = "2";
        Content.JoinSearch();
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-08
작성목적 : 커뮤니티 가기 , 커뮤니티 가입하기 
**********************************************************************************************/
Content.View = function (_communityID, _type, _grade) {

    // 가입한 커뮤니티일 경우
    if (_grade != "0") {

        CommunityID = _communityID;
        Grade = _grade;

        var url = "/DeskPlusEIP/Community/Ws/Main/Content.asmx/AddVisitPoint";
        ParamArray = {
            'communityID': _communityID,
            'type': '6',
            'menuID': '0'
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Content.SuccessAddVisitPoint");

        AjaxService(url, data, obj);
        return;
    }

    var AdminGrade = "0";
    var AdminCheck = "N";

    if (parent.frmLeft.$("#cbShowImage").is(":checked")) {
        AdminGrade = "2";
        AdminCheck = "Y";
    }

    // 관리자 모드로 입장 
    if (AdminGrade == "2") {
        ParamArray = {
            'communityID': _communityID,
            'grade': AdminGrade,
            'adminCheck': AdminCheck
        };

        CallEncrypt(CallGetParam());
        parent.location.href = "../Site/Default.aspx?p=" + GetEncryptValue;
        return;
    }

    // 커뮤니티 공개 (type 1) , 비공개 (type 2) , 퀴즈 ( type 3)
    if (_type == "1") {
        ParamArray = {
            'communityID': _communityID            
        };

        CallEncrypt(CallGetParam());
        parent.location.href = "../Site/Default.aspx?p=" + GetEncryptValue;
    }
    else if (_type == "2") {
        ParamArray = {
            'communityID': _communityID
        };

        CallEncrypt(CallGetParam());
        location.href = "Join.aspx?p=" + GetEncryptValue;
    }
    else if (_type == "3") {
        ParamArray = {
            'communityID': _communityID
        };

        CallEncrypt(CallGetParam());
        location.href = "Join.aspx?p=" + GetEncryptValue;
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-10
작성목적 : 폴더별 사용자 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
Content.ModCategoryUser = function (gubun) {

    var url = "/DeskPlusEIP/RM/Ws/Admin/Content/List.asmx/ModCategoryUser";
    ParamArray = {
        'categoryID': CategoryID,
        'pageSize': $("#ddlListCount").val(),
        'types': "L"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessModCategoryUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-10
작성목적 : 폴더별 사용자 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
Content.SuccessModCategoryUser = function (data, status) {
    if (data.d) {
        RecordPerPage = $("#ddlListCount").val();
        Content.Search();
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-08
작성목적 : 가입 신청한 커뮤니티 취소하기 (웹서비스 호출)
**********************************************************************************************/
Content.DelUserApply = function (communityID, userID) {
    if (window.confirm(R_DelJoin)) {

        var url = "/DeskPlusEIP/Community/Ws/Main/Content.asmx/DelUserApply";
        ParamArray = {
            'communityID': communityID,
            'userID': userID
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Content.SuccessDelUserApply");

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-08
작성목적 : 가입 신청한 커뮤니티 취소하기 (웹서비스 값 받기)
**********************************************************************************************/
Content.SuccessDelUserApply = function (data, status) {
    if (data.d) {
        Content.JoinSearch();
        alert(R_CompleteConfirm);
        return;
    }
    else {
        alert(R_ProcessingError);
        return;
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-31
작성목적 : 커뮤니티 화면가기 - 방문 카운트 넣기 (웹서비스 값 받기)
**********************************************************************************************/
Content.SuccessAddVisitPoint = function (data, status) {

    if (data.d) {

        var AdminCheck = "N";

        if (parent.frmLeft.$("#cbShowImage").is(":checked")) {
            Grade = "2";
            AdminCheck = "Y";
        }

        ParamArray = {
            'communityID': CommunityID,
            'grade': Grade,
            'adminCheck': AdminCheck
        };

        CallEncrypt(CallGetParam());

        parent.location.href = "../Site/Default.aspx?p=" + GetEncryptValue;
    }
    else {
        alert(R_ProcessingError);
        return;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-18
작성목적 : 리스트별 사용자 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
Content.ModSetting = function (gubun) {

    var url = "/DeskPlusEIP/Community/Ws/Main/Content.asmx/ModSetting";
    ParamArray = {
        'communityID': CommunityID,
        'menuID':'0',
        'pageSize': $("#ddlListCount").val(),
        'types': 'ML'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Content.SuccessSetting");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-18
작성목적 : 리스트별 사용자 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
Content.SuccessSetting = function (data, status) {
    if (data.d == true) {
    }
}
