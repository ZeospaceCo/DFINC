ShortReplyList = {}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 
**********************************************************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ShortReplyList.LoadData("");
        }
    }

    $(document).ready(function () {

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
            ShortReplyList.Search();
        });

        //날짜검색
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            ShortReplyList.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ShortReplyList.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ShortReplyList.Search();
        });

        //[닫기]
        $('#cbtnCancel').click(function (e) { e.preventDefault(); ShortReplyList.Close(); });

        //[댓글추가]
        $('#cbtnShortReply').click(function (e) { e.preventDefault(); ShortReplyList.Add(); });

        $(".clsDate").keydown(function (e) { return false; });

        //입력값 체크하기
        if (navigator.appName == 'Microsoft Internet Explorer') {
            $('.charsRemaining').text($('#textarea').attr('maxlength'));
            $('#textarea').bind('keyup', function () {
                $(this).css('border', '1px solid #d2d2d2');
                var $this = $(this);
                var len = $this.val().length;
                var maxlength = $this.attr('maxlength')

                $('.charsRemaining').text(maxlength - len);
                if (maxlength && len > maxlength) {
                    $(this).css('border', '1px solid #f00');
                    alert(R_Msg_ErrOpinion);
                    $this.val($this.val().slice(0, maxlength));
                    $('.charsRemaining').text("0");
                    return;
                }
            });
        }
        else {
            ShortReplyList.MaxLang();
        }

        ShortReplyList.Init();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $("#textarea").select();        
    });
});

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 글자 제한
***************************************************/
ShortReplyList.MaxLang = function () {

    var onEditCallback = function (remaining) {
        $('.charsRemaining').text(remaining);

        if (remaining > 0) {
            $(this).css('border', '1px solid #d2d2d2');
        }
    }

    var onLimitCallback = function () {
        $(this).css('border', '1px solid #f00');
    }

    $('textarea[maxlength]').limitMaxlength({
        onEdit: onEditCallback,
        onLimit: onLimitCallback
    });
}

/**************************************************
작성자   : 
작성일자 :
작성목적 : 초기화 하기
***************************************************/
ShortReplyList.Init = function () {

    $("#startEndDate").hide(); //기간

    if (SearchTerm.toUpperCase() == "PERIOD") {
        $("#startEndDate").show(); //기간   
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);

        if (SearchTermValue != "") {
            $("#startDate").val(SearchTermValue.split(",")[0]);
            $("#endDate").val(SearchTermValue.split(",")[1]);
        }
    }

    ShortReplyList.LoadData("");
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 등록일자 활성화 여부
***************************************************/
ShortReplyList.RegDate = function () {
    //기간 선택
    var sField = $("#ddlSearchTerm").val();
    if (sField == "period") {
        $("#startEndDate").show(); //기간   
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);
    } else {
        $("#startEndDate").hide(); //기간 이외
    }

    $("#startDate").val("");
    $("#endDate").val("");
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 리스트 초기화
***************************************************/
ShortReplyList.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        ShortReplyList.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        ShortReplyList.GetCount(); //검색 총 카운트 가져오기
        ShortReplyList.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ShortReplyList.LoadData("search");
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
            loadBoardSearchData();
        }
        else {
            loadBoardData();
        }
    }, 100);
}



/**************************************************
작성자   : 
작성일자 : 
작성목적 : 검색 필드 설정하기
***************************************************/
ShortReplyList.SettingSearch = function () {

    SearchTerm = $("#ddlSearchTerm").val();
    SearchField = $("#ddlSearchField").val();

    if (SearchTerm == "period") {
        var getStartDate = $("#startDate").val();
        var getEndDate = $("#endDate").val();

        if (getStartDate == "" || getEndDate == "") {
            alert(R_ApplyRegDate);
            return false;
        }
        else {

            var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

            if (dtResponse == false) {
                alert(R_StartDateEndDate);
                return false;
            }
        }

        SearchTermValue = getStartDate + "," + getEndDate;
    }

    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    return true;
}


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 검색
***************************************************/
ShortReplyList.Search = function () {

    var response = ShortReplyList.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  
    ShortReplyList.LoadData("search");
}


/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
ShortReplyList.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Common/Popup/ShortReplyList.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'totalItemCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'docID': DocumentID
    };

    var obj = eval("ShortReplyList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
ShortReplyList.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Common/Popup/JTShortReplyList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ShortReplyList.Resource();
}


/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ShortReplyList.Resource = function () {

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
ShortReplyList.GetCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Common/Popup/ShortReplyList.asmx/GetListCount";

    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'docID': DocumentID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ShortReplyList.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
ShortReplyList.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 정렬
**********************************************************************************************/
ShortReplyList.Sort = function (col) {

    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;

    ShortReplyList.LoadData("search");
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 삭제
**********************************************************************************************/
ShortReplyList.Del = function (_shortReplyID) {

    if (_shortReplyID == 0) return;

    if (confirm(R_Msg_Del)) {
        var url = "/DeskPlusEIP/Approval/Ws/Common/Popup/ShortReplyList.asmx/Del";
        ParamArray = { 'shortReplyID': _shortReplyID };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ShortReplyList.SuccessDel");

        AjaxService(url, data, obj);
    }
}
ShortReplyList.SuccessDel = function (data, status) { //alert(data)
    if (data.d) {
        CurrentPage = 1; //1페이지로 이동처리  
        ShortReplyList.LoadData("search");
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [취소]
***************************************************/
ShortReplyList.Close = function () {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 추가
**********************************************************************************************/
ShortReplyList.Add = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("textarea", R_NeedContents, true);
    if (!validator.validate()) return;

    var val = $("#textarea").val()
        .replace(/[&]/gi, '＆').replace(/["]/gi, '＂')
        .replace(/[']/gi, '＇').replace(/[(]/gi, '（')
        .replace(/[)]/gi, '）').replace(/[+]/gi, '＋')
        .replace(/[§]/gi, '※').replace(/[=]/gi, '＝');
    
    var url = "/DeskPlusEIP/Approval/Ws/Common/Popup/ShortReplyList.asmx/Add";
    ParamArray = {
        'docID': DocumentID
        , 'cont' : val
        , 'groupID' : GroupID
        , 'accountTag' : AccountTag
        , 'isComp' :IsComp
     };
    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ShortReplyList.SuccessAdd");
    AjaxService(url, data, obj);
}
ShortReplyList.SuccessAdd = function (data, status) { 
    if (data.d) {
        alert(R_MSG_SUCCESS);
        $("#textarea").html("");
        CurrentPage = 1; //1페이지로 이동처리  
        ShortReplyList.LoadData("search");
    }
}