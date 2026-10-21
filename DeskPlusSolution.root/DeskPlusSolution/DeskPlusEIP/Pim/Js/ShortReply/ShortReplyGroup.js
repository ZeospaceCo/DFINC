ShortReplyGroup = {}



/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-11-16
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ShortReplyGroup.LoadData("");
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

        //날짜검색
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            ShortReplyGroup.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ShortReplyGroup.Search();
            }
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            ShortReplyGroup.PageSize();
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ShortReplyGroup.Search();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        // 댓글 추가
        $('#cbtnShortReply').click(function (e) {
            e.preventDefault();
            ShortReplyGroup.Add();
        });

        //목록
        $('#CbtnList').click(function (e) {
            e.preventDefault();
            ShortReplyGroup.GoList();
        });

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
                    alert(maxlength + " limited");
                    $this.val($this.val().slice(0, maxlength));
                    $('.charsRemaining').text("0");
                    return;
                }
            });
        }
        else {
            ShortReplyGroup.MaxLang();
        }

        //날짜검색
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            ShortReplyGroup.RegDate();
        });


        //ShortReplyGroup.LoadData();
        $("#textarea").select();

        ShortReplyGroup.Init();

        $(".clsDate").keydown(function (e) { return false; });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2012-11-16
작성목적 : 초기화 하기
***************************************************/
ShortReplyGroup.Init = function () {

    // 댓글공개 체크
    $("#chkOpen").attr("checked", true);

    // 목록 버튼 보이기 처리
    if (RUserID > 0) {        
        $("#CbtnList").attr("style", "display:");        
    }
    else {
        $("#CbtnList").attr("style", "display:none");
    }

    $("#startEndDate").hide(); //기간

    if (SearchTerm == "period") {
        $("#startEndDate").show(); //기간   
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);

        if (SearchTermValue != "") {
            $("#startDate").val(SearchTermValue.split(",")[0]);
            $("#endDate").val(SearchTermValue.split(",")[1]);
        }
    }

    ShortReplyGroup.LoadData("");

    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-16
작성목적 : 등록일자 활성화 여부
***************************************************/
ShortReplyGroup.RegDate = function () {
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
작성자   : famo23
작성일자 : 2012-11-16
작성목적 : 글자 제한
***************************************************/
ShortReplyGroup.MaxLang = function () {

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
작성자   : famo23
작성일자 : 2012-11-16
작성목적 : 초기화
***************************************************/
ShortReplyGroup.LoadData = function (gubun) {

    ShortReplyGroup.GetCount(); //검색 총 카운트 가져오기

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        ShortReplyGroup.GetPageList();
    }


    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        ShortReplyGroup.GetCount(); //검색 총 카운트 가져오기 
        ShortReplyGroup.GetSearchPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ShortReplyGroup.LoadData("search");
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
작성자   : famo23
작성일자 : 2012-11-20
작성목적 : 검색
***************************************************/
ShortReplyGroup.Search = function () {
    var response = ShortReplyGroup.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  

    ShortReplyGroup.LoadData("search");
}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-20
작성목적 : 검색 필드 설정하기
***************************************************/
ShortReplyGroup.SettingSearch = function () {

    SearchTerm = $("#ddlSearchTerm").val();
    SearchGroup = $("#ddlSearchGroup").val();
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

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-11-16
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
ShortReplyGroup.GetPageList = function () {
    var url = "/DeskPlusEIP/Pim/Ws/ShortReply/ShortReplyGroup.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'groupID': GroupID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    var obj = eval("ShortReplyGroup.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-11-16
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
ShortReplyGroup.SuccessPageList = function (data, status) {
    $('#phShortRelpyGroup').setTemplateURL('/DeskPlusEIP/Pim/Jt/ShortReply/JTShortReplyGroup.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phShortRelpyGroup').processTemplate(data.d);

    $('#loading').html("");

    TmpRUserID = data.d[0].UserID;

    ShortReplyGroup.Resource();

    $("#scrollDiv").css({ 'overflow-y': 'auto', 'min-height': '230px', 'height': '230px' });
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-11-21
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
ShortReplyGroup.GetSearchPageList = function () {
    var url = "/DeskPlusEIP/Pim/Ws/ShortReply/ShortReplyGroup.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'groupID': GroupID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    var obj = eval("ShortReplyGroup.SuccessSearchPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-11-20
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
ShortReplyGroup.SuccessSearchPageList = function (data, status) {
    $('#phShortRelpyGroup').setTemplateURL('/DeskPlusEIP/Pim/Jt/ShortReply/JTShortReplyGroup.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phShortRelpyGroup').processTemplate(data.d);

    $('#loading').html("");

    ShortReplyGroup.Resource();

    $("#scrollDiv").css({ 'overflow-y': 'auto', 'min-height': '230px', 'height': '230px' });
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 페이지 사이즈에 이동하기
***************************************************/
ShortReplyGroup.PageSize = function () {
    CurrentPage = 1; //1페이지로 이동처리  
    RecordPerPage = $('#ddlListCount').val();
    //ShortReplyGroup.LoadData();
    ShortReplyGroup.LoadData("search");
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-11-16
작성목적 : 다국어 처리
**********************************************************************************************/
ShortReplyGroup.Resource = function () {
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
    if ($(".NoListData") != undefined) {$(".NoListData").text(R_NoContent); }
    if ($(".NoAuthListData") != undefined) { $(".NoAuthListData").text(R_SecurityMsg); }

    if ($(".BtnDel") != undefined) { $(".BtnDel").text("[삭제]"); }

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-11-16
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
ShortReplyGroup.GetCount = function () {
    var url = "/DeskPlusEIP/Pim/Ws/ShortReply/ShortReplyGroup.asmx/GetCount";

    ParamArray = {
        'groupID': GroupID,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ShortReplyGroup.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-11-16
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
ShortReplyGroup.SuccessCount = function (data, status) {

    TotalItemCount = data.d;
}


/**************************************************
작성자   : famo23
작성일자 : 2012-11-20
작성목적 : 댓글 저장
***************************************************/
ShortReplyGroup.Add = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("textarea", R_Add_ReplyInputContent, true);
    if (!validator.validate()) return;

    var val = $("#textarea").val()
        .replace(/[&]/gi, '＆').replace(/["]/gi, '＂')
        .replace(/[']/gi, '＇').replace(/[(]/gi, '（')
        .replace(/[)]/gi, '）').replace(/[+]/gi, '＋')
        .replace(/[§]/gi, '※').replace(/[=]/gi, '＝');

    var security = "1";
    if ($("#chkOpen").attr("checked") == "checked")
        security = "0";

    if (TmpRUserID == "0") return;

    ParamArray = {
        'jobDate': "",
        'contents': val,
        'rUserID': TmpRUserID,
        'security': security,
        'groupID': GroupID
    };

    var url = "/DeskPlusEIP/Pim/Ws/ShortReply/ShortReplyGroup.asmx/AddToShortReply";
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ShortReplyGroup.SuccessAdd");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-11-20
작성목적 : 댓글 추가 성공시 페이지 표시
**********************************************************************************************/
ShortReplyGroup.SuccessAdd = function (data, status) {

    if (data.d == true) {
        alert(R_Processing);

        $("#textarea").html("");

        CurrentPage = 1; //1페이지로 이동처리  
        ShortReplyGroup.LoadData("search");
    }
    else {
        alert(R_MsgError);
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-11-20
작성목적 : 댓글 삭제
**********************************************************************************************/
ShortReplyGroup.DelReple = function (replyID) {

    if (confirm(R_ReallyApplyDelete)) {

        if (replyID == GroupID) {

            ParamArray = {
                'groupID': GroupID
            };

            CallEncrypt(CallGetParam())

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("ShortReplyGroup.SuccessDelGroupShortReply");
            var url = "/DeskPlusEIP/Pim/Ws/ShortReply/ShortReplyGroup.asmx/DelGroupShortReply";
            AjaxService(url, data, obj);
        }
        else {
            ParamArray = {
                'replyID': replyID
            };

            CallEncrypt(CallGetParam())

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("ShortReplyGroup.SuccessDelShortReply");
            var url = "/DeskPlusEIP/Pim/Ws/ShortReply/ShortReplyGroup.asmx/DelShortReply";
            AjaxService(url, data, obj);
        }
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-11-20
작성목적 : 댓글 삭제 성공
**********************************************************************************************/
ShortReplyGroup.SuccessDelShortReply = function (data, status) {
    if (data.d) {
        alert(R_Processing);
        CurrentPage = 1; //1페이지로 이동처리  
        ShortReplyGroup.LoadData("search");
    }
    else
        alert(R_MsgError);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-11-20
작성목적 : 댓글 삭제 성공
**********************************************************************************************/
ShortReplyGroup.SuccessDelGroupShortReply = function (data, status) {
    if (data.d) {
        alert(R_Processing);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else
        alert(R_MsgError);
}



/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-11-20
작성목적 : 댓글 리스트 화면으로 이동
**********************************************************************************************/
ShortReplyGroup.GoList = function () {

    ParamArray = {
        'rUserID': RUserID        
    };


    CallEncrypt(CallGetParam());
    location.href = "ShortReplyList.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : famo23  
작성일자 : 2012-12-12
작성목적 : 상대방 담벼락가기
**********************************************************************************************/
ShortReplyGroup.goUserList = function (p_userID) {

    ParamArray = {
        'rUserID': p_userID
    };

    CallEncrypt(CallGetParam());
    location.href = "ShortReplyList.aspx?p=" + GetEncryptValue;
}
