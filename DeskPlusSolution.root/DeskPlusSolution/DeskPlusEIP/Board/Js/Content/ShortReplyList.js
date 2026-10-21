ShortReplyList = {}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ShortReplyList.LoadData();
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
            ShortReplyList.PageSize();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        $('#cbtnShortReply').click(function (e) {
            e.preventDefault();
            ShortReplyList.Add();
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
            ShortReplyList.MaxLang();
        }

        ShortReplyList.LoadData();
        $("#textarea").select();
    });
});

/**************************************************
작성자   : 심재운
작성일자 : 2015.10.19
작성목적 : 날짜 정렬
***************************************************/
ShortReplyList.Regdate = function (e) {

    if ($(e).hasClass("desc")) {
        $(e).removeClass("desc");
        $(e).addClass("asc");
        Order = "0";
        
    } else {
        $(e).removeClass("asc");
        $(e).addClass("desc");
        Order = "1";
    }
    
    CurrentPage = 1; //1페이지로 이동처리
    ShortReplyList.LoadData();
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 글자 제한
***************************************************/
ShortReplyList.MaxLang = function () {
    var onEditCallback = function (remaining) {
        //$(this).siblings('.charsRemaining').text("Characters remaining: " + remaining);
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
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 초기화
***************************************************/
ShortReplyList.LoadData = function () {
    ShortReplyList.GetCount(); //검색 총 카운트 가져오기

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        ShortReplyList.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ShortReplyList.LoadData();
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

    setTimeout(function () {
        loadBoardSearchData();
    }, 100);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
ShortReplyList.GetPageList = function () {
    var url = "/DeskPlusEIP/Board/Ws/Content/ShortReplyList.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'order' : Order
    };

    var obj = eval("ShortReplyList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
ShortReplyList.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Board/Jt/Content/JTShortReplyList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (shimpark)

    ShortReplyList.Resource();

    $("#scrollDiv").css({ 'overflow-y': 'auto', 'min-height': '230px', 'height': '230px' });
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 페이지 사이즈에 이동하기
***************************************************/
ShortReplyList.PageSize = function () {
    var iPageUrl = $.url(PageUrl);
    var iPath = iPageUrl.attr('path');

    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID,
        'recordPerPage': $('#ddlListCount').val()
    };

    CallEncrypt(CallGetParam());

    //이동하기
    location = iPath + "?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
ShortReplyList.Resource = function () {
    $("#r_Name")[0].innerHTML = R_Name;
    $("#r_Content")[0].innerHTML = R_Contents;
    $("#r_RegDate")[0].innerHTML = R_RegDate;

    if ($(".r_mod") != undefined) {
        $(".r_mod").text(R_Modify);
    }

    if ($(".r_del") != undefined) {
        $(".r_del").text(R_Del);
    }

    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

    //웹서비스 연동으로 jtemplate 가 초기화 되므로 다시 설정함. shimpark
    if (Order == "0") {    
        $("#a_RegDate").addClass("asc");
    } else {
        $("#a_RegDate").addClass("desc");
    }
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
ShortReplyList.GetCount = function () {
    var url = "/DeskPlusEIP/Board/Ws/Content/ShortReplyList.asmx/GetCount";

    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ShortReplyList.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
ShortReplyList.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-22
작성목적 : 댓글 삭제
**********************************************************************************************/
ShortReplyList.DelReple = function (shortReplyID) {
    if (confirm(R_ReallyApplyDelete)) {
        ParamArray = {
            'categoryID': CategoryID,
            'listID': ListID,
            'ShortReplyID': shortReplyID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ShortReplyList.SuccessDelShortReply");
        var url = "/DeskPlusEIP/Board/Ws/Content/View.asmx/DelShortReply";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 댓글 추가 성공
**********************************************************************************************/
ShortReplyList.SuccessDelShortReply = function (data, status) {
    if (data.d) {
        alert(R_Processing);
        CurrentPage = 1; //1페이지로 이동처리
        ShortReplyList.LoadData();
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 댓글 저장
***************************************************/
ShortReplyList.Add = function () {
    var validator = new Validator($("#form1"));
    validator.checkRequired("textarea", R_Add_ReplyInputContent, true);
    if (!validator.validate()) return;

    var val = $("#textarea").val()
        .replace(/[&]/gi, '＆').replace(/["]/gi, '＂')
        .replace(/[']/gi, '＇').replace(/[(]/gi, '（')
        .replace(/[)]/gi, '）').replace(/[+]/gi, '＋')
        .replace(/[§]/gi, '※').replace(/[=]/gi, '＝');

    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID,
        'contents': val
    };

    var url = "/DeskPlusEIP/Board/Ws/Content/ShortReply.asmx/AddShortReply";
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ShortReplyList.SuccessAdd");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 댓글 추가 성공시 페이지 표시
**********************************************************************************************/
ShortReplyList.SuccessAdd = function (data, status) {
    if (data.d == true) {
        alert(R_Processing);

        $("#textarea").html("");

        CurrentPage = 1; //1페이지로 이동처리
        ShortReplyList.LoadData();
    }
}