ShortReplyList = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 초기화 
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

        ShortReplyList.LoadData();
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
                    //alert(maxlength + " limited");
                    alert(R_LimitShortReply);
                    $this.val($this.val().slice(0, maxlength));
                    $('.charsRemaining').text("0");
                    return;
                }
            });
        }
        else {
            ShortReplyList.MaxLang();
        }


        if (CheckAuth == "") {
            $("#textarea").focus();
        }
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2012-06-22
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
작성자   : famo23
작성일자 : 2012-06-11
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
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 짧은 답글 리스트 (웹서비스 호출)
**********************************************************************************************/
ShortReplyList.GetPageList = function () {
    var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Board/ShortReplyList.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'boardID': BoardID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage
    };

    var obj = eval("ShortReplyList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 짧은 답글 리스트 (웹서비스 값 받기)
**********************************************************************************************/
ShortReplyList.SuccessPageList = function (data, status) {
    $('#phShortRelpyList').setTemplateURL('/DeskPlusEIP/Community/Jt/Site/Apps/Board/JTShortReplyList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phShortRelpyList').processTemplate(data.d);

    ShortReplyList.Resource();

    $("#scrollDiv").css({ 'overflow-y': 'auto', 'min-height': '230px', 'height': '230px' });
}


/**************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 페이지 사이즈에 이동하기
***************************************************/
ShortReplyList.PageSize = function () {

    CurrentPage = 1; //1페이지로 이동처리  
    RecordPerPage = $('#ddlListCount').val();
    ShortReplyList.LoadData();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 다국어 처리
**********************************************************************************************/
ShortReplyList.Resource = function () {
    $("#j_Name")[0].innerHTML = R_UserName;
    $("#j_Content")[0].innerHTML = R_Contents;
    $("#j_RegDate")[0].innerHTML = R_RegDate;

    if ($(".r_del") != undefined) {
        $(".r_del").text(R_Del);
    }

    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 짧은 댓글 카운트 (웹 서비스 호출)
**********************************************************************************************/
ShortReplyList.GetCount = function () {
    var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Board/ShortReplyList.asmx/GetCount";

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'boardID': BoardID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ShortReplyList.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 짧은 댓글 카운트 (웹 서비스 값 받기)
**********************************************************************************************/
ShortReplyList.SuccessCount = function (data, status) {
    
    TotalItemCount = data.d;
}



/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 짧은댓글 삭제 (웹 서비스 호출)
**********************************************************************************************/
ShortReplyList.DelReple = function (shortReplyID, userID) {

    if (confirm(R_ReallyApplyDelete)) {

        ParamArray = {
            'communityID': CommunityID,
            'menuID': MenuID,
            'boardID': BoardID,
            'shortReplyID': shortReplyID,
            'userID': userID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ShortReplyList.SuccessDelShortReply");
        var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Board/ShortReplyList.asmx/DelShortReply";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 짧은댓글 추가 성공 (웹 서비스 값 받기)
**********************************************************************************************/
ShortReplyList.SuccessDelShortReply = function (data, status) {
    if (data.d) {
        alert(R_CompleteConfirm);
        CurrentPage = 1; //1페이지로 이동처리  
        ShortReplyList.LoadData();
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 짧은 답글 저장 (웹 서비스 호출)
***************************************************/
ShortReplyList.Add = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("textarea", R_ReplyInputContent, true);
    if (!validator.validate()) return;

    var val = $("#textarea").val()
        .replace(/[&]/gi, '＆').replace(/["]/gi, '＂')
        .replace(/[']/gi, '＇').replace(/[(]/gi, '（')
        .replace(/[)]/gi, '）').replace(/[+]/gi, '＋');

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'boardID': BoardID,
        'grade': Grade,
        'listType': ListType,
        'contents': escape(val)
    };


    var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Board/ShortReplyList.asmx/AddShortReply";
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ShortReplyList.SuccessAdd");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 짧은 답글 저장 (웹 서비스 값 받기)
**********************************************************************************************/
ShortReplyList.SuccessAdd = function (data, status) {

    if (data.d == true) {
        alert(R_CompleteConfirm);

        $("#textarea").html("");

        CurrentPage = 1; //1페이지로 이동처리  
        ShortReplyList.LoadData();
    }
}
