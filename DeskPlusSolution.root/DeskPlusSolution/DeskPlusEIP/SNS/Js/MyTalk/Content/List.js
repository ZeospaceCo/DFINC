List = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            List.LoadData("");
        }
    }

    $(document).ready(function () {
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

        // 담벼락글작성버튼
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        // 담벼락글 저장
        $('#cbtnShortReplyAdd').click(function (e) {
            e.preventDefault();
            List.ReplyAdd();
        });

        // 공개대상관리
        $('#cbtnOpen').click(function (e) {
            e.preventDefault();
            List.OpenUser();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                List.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            List.Search();
        });

        //새로고침
        $('#CbtnReload').click(function (e) {
            e.preventDefault();
            List.Search();
        });

        // 기간
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            List.RegDate();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            List.ModFolderUser();
            List.Search();
        });

        //공유 담벼락으로 이동
        $('#cbtnShare').click(function (e) {
            e.preventDefault();
            List.ShareList();
        });

        //내 담벼락으로 이동
        $('#cbtnMy').click(function (e) {
            e.preventDefault();
            List.MyList(UserID);
        });

        //파일선택
        $('#cbtnAddFile').click(function (e) {
            e.preventDefault();
            List.FileUpload();
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
                    alert(R_Msg_ErrOpinion);
                    $this.val($this.val().slice(0, maxlength));
                    $('.charsRemaining').text("0");
                    return;
                }
            });
        }
        else {
            List.MaxLang();
        }

        List.Init();

        $(".clsDate").keydown(function (e) { return false; });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);
    });
});

/**************************************************
작성자   :
작성일자 :
작성목적 : 글자 제한
***************************************************/
List.MaxLang = function () {
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
작성일자 : 2013-05-13
작성목적 : 초기화 하기
***************************************************/
List.Init = function () {
    // 댓글공개 체크
    $("#chkOpen").attr("checked", true);

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

    List.FileList();
    List.LoadData("");
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 날짜 세팅
***************************************************/
List.RegDate = function () {
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
작성일자 : 2013-05-13
작성목적 : 검색
***************************************************/
List.Search = function () {
    var response = List.SettingSearch();
    if (response == false) return;

    RecordPerPage = $("#ddlListCount").val();
    CurrentPage = 1; //1페이지로 이동처리

    List.LoadData("search");
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 검색 필드 설정하기
***************************************************/
List.SettingSearch = function () {
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
            var start_dates = getStartDate.dateCharRemove();
            var end_dates = getEndDate.dateCharRemove();

            if ((end_dates - start_dates) < 0) {
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
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadSNSData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadSNSSearchData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                //if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                List.LoadData("search");
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

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        if (gubun == "search") {
            loadSNSSearchData();
        }
        else {
            loadSNSData();
        }
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/SNS/Ws/MyTalk/Content/List.asmx/GetPageList"

    //파라미터값 배열에 담기
    ParamArray = {
        'rUserID': RUserID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#phList').setTemplateURL('/DeskPlusEIP/SNS/Jt/MyTalk/Content/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    if ($(".NoAuthListData") != undefined) { $(".NoAuthListData").text(R_NoAuthListData); }

    $(".AddMsg").text(R_AddShortReply);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/SNS/Ws/MyTalk/Content/List.asmx/GetListCount";
    ParamArray = {
        'rUserID': RUserID,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 글등록 팝업
**********************************************************************************************/
List.Add = function () {
    if (MyTalkCheck == "1") {
        $("#MyTalkAdd").css("display", "");
        MyTalkCheck = "2";
    }
    else {

        $("#MyTalkAdd").css("display", "none");
        $("#textarea").val('');
        MyTalkCheck = "1";

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
            List.MaxLang();
        }

        //첨부파일삭제
        List.DelSpoolFile();
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2013-06-10
작성목적 : 댓글 저장
***************************************************/
List.ReplyAdd = function () {
    var validator = new Validator($("#form1"));
    validator.checkRequired("textarea", R_NeedContents, true);
    if (!validator.validate()) return;

    var val = $("#textarea").val()
        .replace(/[&]/gi, '＆').replace(/["]/gi, '＂')
        .replace(/[']/gi, '＇').replace(/[(]/gi, '（')
        .replace(/[)]/gi, '）').replace(/[+]/gi, '＋')
        .replace(/[§]/gi, '※').replace(/[=]/gi, '＝');

    var security = "1";
    if ($("#chkOpen").attr("checked") == "checked")
        security = "0";

    ParamArray = {
        'jobDate': "",
        'contents': val,
        'rUserID': RUserID,
        'security': security,
        'swfUploadGUID': SwfUploadGUID
    };

    var url = "/DeskPlusEIP/SNS/Ws/MyTalk/Content/List.asmx/AddToMyTalk";
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessReplyAdd");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-06-10
작성목적 : 댓글 추가 성공시 페이지 표시
**********************************************************************************************/
List.SuccessReplyAdd = function (data, status) {
    if (data.d == true) {
        alert(R_CompleteConfirm);

        List.Search();
        //첨부파일삭제
        List.DelSpoolFile();

        //$("#MyTalkAdd").css("display", "none");
        //MyTalkCheck = "1";
        $("#textarea").val('');

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
            List.MaxLang();
        }
    }
    else {
        alert(R_ProcessingError);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-14
작성목적 : 사용자 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
List.ModFolderUser = function () {
    var url = "/DeskPlusEIP/SNS/Ws/MyTalk/Content/List.asmx/ModFolderUser";
    ParamArray = {
        'pageSize': $("#ddlListCount").val(),
        'types': 'M'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessModFolderUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-14
작성목적 : 사용자 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessModFolderUser = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-14
작성목적 : 마이톡 삭제
**********************************************************************************************/
List.DelReple = function (myTalkID, groupID) {
    if (confirm(R_ReallyApplyDelete)) {
        if (myTalkID == groupID) {
            ParamArray = {
                'groupID': groupID
            };

            CallEncrypt(CallGetParam())

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("List.SuccessDelMyTalk");
            var url = "/DeskPlusEIP/SNS/Ws/MyTalk/Content/List.asmx/DelGroupMyTalk";
            AjaxService(url, data, obj);
        }
        else {
            ParamArray = {
                'myTalkID': myTalkID
            };

            CallEncrypt(CallGetParam())

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("List.SuccessDelMyTalk");
            var url = "/DeskPlusEIP/SNS/Ws/MyTalk/Content/List.asmx/DelMyTalk";
            AjaxService(url, data, obj);
        }
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-14
작성목적 : 마이톡 삭제
**********************************************************************************************/
List.SuccessDelMyTalk = function (data, status) {
    if (data.d) {
        alert(R_CompleteConfirm);
        List.Search();
    }
    else
        alert(R_ProcessingError);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-14
작성목적 : 공개대상관리
**********************************************************************************************/
List.OpenUser = function () {
    var url = "GetUser.aspx";
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "ShortReplyShare", "800", "500")
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-06-14
작성목적 : 담벼락 글에 댓글 추가
**********************************************************************************************/
List.GroupAdd = function (groupID, p_rUserID) {
    var replyChk = $("#ReplyCheck_" + groupID)[0].innerHTML;

    if (replyChk == "") {
        var tmpVal = "<div><div class='layout_subbox'><table><tr><td><span class='m_talk'></span>&nbsp;";
        tmpVal += "<span class='subtext'>" + R_ReplyAdd + "</span>&nbsp;( <span id='charsRemaining_" + groupID + "'></span> )</td>";
        tmpVal += "<td><table><tr><td>&nbsp;&nbsp;<span style='float:right;padding-right:10px;'><input id='chkOpen_" + groupID + "' type='checkbox' name='chkOpen' checked='checked'>" + R_ReplyOpen + "</span></td>";
        tmpVal += "<td><ul class='btn_small'><li><a href='#' onclick='List.GroupReplyAdd(" + groupID + "," + p_rUserID + ")') class='btn btn-sm'><span>" + R_Save + "</span></a></li>";
        tmpVal += "</ul></td></tr></table></td></tr></table></div>";
        tmpVal += "<textarea id='textarea_" + groupID + "' class='textarea' style='width: 99%; height: 55px; border: 1px solid rgb(210, 210, 210);' maxlength='200'></textarea>";
        tmpVal += "</div>";

        $("#AddReply_" + groupID)[0].innerHTML = tmpVal;
        $("#ReplyCheck_" + groupID)[0].innerHTML = "1";

        $('#charsRemaining_' + groupID).text($('#textarea_' + groupID).attr('maxlength'));
        $('#textarea_' + groupID).bind('keyup', function () {
            $(this).css('border', '1px solid #d2d2d2');
            var $this = $(this);
            var len = $this.val().length;
            var maxlength = $this.attr('maxlength')

            $('#charsRemaining_' + groupID).text(maxlength - len);
            if (maxlength && len > maxlength) {
                $(this).css('border', '1px solid #f00');
                alert(R_Msg_ErrOpinion);
                $this.val($this.val().slice(0, maxlength));
                $('#charsRemaining_' + groupID).text("0");
                return;
            }
        });

        //입력값 체크하기
        /*
        if (navigator.appName == 'Microsoft Internet Explorer') {
            $('#charsRemaining_' + groupID).text($('#textarea_' + groupID).attr('maxlength'));
            $('#textarea_' + groupID).bind('keyup', function () {
                $(this).css('border', '1px solid #d2d2d2');
                var $this = $(this);
                var len = $this.val().length;
                var maxlength = $this.attr('maxlength')

                $('#charsRemaining_' + groupID).text(maxlength - len);
                if (maxlength && len > maxlength) {
                    $(this).css('border', '1px solid #f00');
                    alert(R_Msg_ErrOpinion);
                    $this.val($this.val().slice(0, maxlength));
                    $('#charsRemaining_' + groupID).text("0");
                    return;
                }
            });
        }
        else {
            var onEditCallback = function (remaining) {
                $('#charsRemaining_' + groupID).text(remaining);

                if (remaining > 0) {
                    $(this).css('border', '1px solid #d2d2d2');
                }
            }

            var onLimitCallback = function () {
                $(this).css('border', '1px solid #f00');
            }

            $('textarea_' + groupID + '[maxlength]').limitMaxlength({
                onEdit: onEditCallback,
                onLimit: onLimitCallback
            });
        }
        */


    }
    else {
        $("#AddReply_" + groupID)[0].innerHTML = "";
        $("#ReplyCheck_" + groupID)[0].innerHTML = "";
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2013-06-14
작성목적 : 그룹의 댓글 추가
***************************************************/
List.GroupReplyAdd = function (groupID, p_rUserID) {
    var validator = new Validator($("#form1"));
    validator.checkRequired("textarea_" + groupID, R_NeedContents, true);
    if (!validator.validate()) return;

    var val = $("#textarea_" + groupID).val()
        .replace(/[&]/gi, '＆').replace(/["]/gi, '＂')
        .replace(/[']/gi, '＇').replace(/[(]/gi, '（')
        .replace(/[)]/gi, '）').replace(/[+]/gi, '＋')
        .replace(/[§]/gi, '※').replace(/[=]/gi, '＝');

    var security = "1";
    if ($("#chkOpen_" + groupID).attr("checked") == "checked")
        security = "0";

    TmpGroup = groupID;

    ParamArray = {
        'jobDate': "",
        'contents': val,
        'rUserID': p_rUserID,
        'security': security,
        'groupID': groupID
    };

    var url = "/DeskPlusEIP/SNS/Ws/MyTalk/Content/List.asmx/AddToMyTalk";
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessGroupReplyAdd");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-06-14
작성목적 : 그룹의 댓글 추가 성공시 페이지 표시
**********************************************************************************************/
List.SuccessGroupReplyAdd = function (data, status) {
    if (data.d == true) {
        alert(R_CompleteConfirm);

        $("#AddReply_" + TmpGroup)[0].innerHTML = "";
        $("#ReplyCheck_" + TmpGroup)[0].innerHTML = "";

        var tmpCount = $("#ReplyCount_" + TmpGroup)[0].innerHTML;
        tmpCount = parseInt(tmpCount) + 1;
        $("#ReplyCountStr_" + TmpGroup)[0].innerHTML = R_ShortReply + "[" + tmpCount + "]";

        $("#GroupReply_" + TmpGroup)[0].innerHTML = "";
        $("#GroupReplyCheck_" + TmpGroup)[0].innerHTML = "";
    }
    else {
        alert(R_ProcessingError);
    }

    TmpGroup = "";
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-06-14
작성목적 : 담벼락 글의 댓글 보기
**********************************************************************************************/
List.ReplyView = function (groupID) {
    var groupReplyChk = $("#GroupReplyCheck_" + groupID)[0].innerHTML;

    if (groupReplyChk == "") {
        $("#GroupReplyCheck_" + groupID)[0].innerHTML = "1";
        TmpGroup = groupID;

        ParamArray = {
            'groupID': groupID
        };

        var url = "/DeskPlusEIP/SNS/Ws/MyTalk/Content/List.asmx/GetShortReply";
        CallEncrypt(CallGetParam());
        data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("List.SuccessReplyView");

        AjaxService(url, data, obj);
    }
    else {
        $("#GroupReply_" + groupID)[0].innerHTML = "";
        $("#GroupReplyCheck_" + groupID)[0].innerHTML = "";
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-06-14
작성목적 : 담벼락 글의 댓글 보기 성공시 페이지 표시
**********************************************************************************************/
List.SuccessReplyView = function (data, status) {
    var tmpHTML = "";

    $.each(data.d, function (index, list) {

        if (list.View == "Y") {
            tmpHTML += "<li><table class='blog_cont_table'><caption>My talk</caption><colgroup><col width='90px'><col></colgroup><tbody><tr><td class='blog_img' rowspan='2'>";
            tmpHTML += "<table width='100%'><tbody><tr><td width='48px' style='text-align:right;'>" + list.Depth + "</td><td style='text-align:right;'><a onclick='List.MyList(" + list.UserID + ");'>" + list.PhotoImage + "</a></td>";
            tmpHTML += "</tr></tbody></table></td><td><div class='tk_b'><p class='lb'><span class='subtext'><a onclick='List.MyList(" + list.UserID + ");'>" + list.UserName + "</a> | " + list.WriteDateYMD + "</span>&nbsp;&nbsp;";

            if (list.ShowNew == "Y") {
                tmpHTML += "<span class='m_talk'></span>&nbsp;";
            }

            if (list.IsAuth == "Y") {
                tmpHTML += "<a onclick='List.DelReple(" + list.MyTalkID + "," + list.GroupID + ");'><img src='/DeskPlusEIP/Design/C/Img/Icon/ico_tree_listDel.gif' /></a>";
            }

            tmpHTML += "<br>" + list.Content + "</p><div class='arr'></div></div></td></tr></tbody></table></li>";
        }
        else {

            tmpHTML += "<li><table class='blog_cont_table'><caption>My talk</caption><colgroup><col width='90px'><col></colgroup><tbody><tr><td class='blog_img' rowspan='2'>";
            tmpHTML += "<table width='100%'><tbody><tr><td width='48px' style='text-align:right;'>" + list.Depth + "</td><td style='text-align:right;'></td>";
            tmpHTML += "</tr></tbody></table></td><td><div class='tk_b'><p class='lb'>";
            tmpHTML += "<br>" + R_NoAuthListData + "</p><div class='arr'></div></div></td></tr></tbody></table></li>";
        }
    });

    $("#GroupReply_" + TmpGroup)[0].innerHTML = tmpHTML;

    TmpGroup = "";
}

/**************************************************************
작성자 : famo23
작성일자 : 2013-06-14
작성목적 : 공유 담벼락으로 이동
**************************************************************/
List.ShareList = function () {
    ParamArray = {
        'rUserID': RUserID
    };

    CallEncrypt(CallGetParam());

    location.href = "ShareList.aspx?p=" + GetEncryptValue;
}

/**************************************************************
작성자 : famo23
작성일자 : 2013-06-14
작성목적 : 내 담벼락으로 이동
**************************************************************/
List.MyList = function (p_UserID) {
    ParamArray = {
        'rUserID': p_UserID
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-12
작성목적 : 파일 업로드 (웹서비스 호출)
***************************************************/
List.FileUpload = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'SNS',
        'callBackEvent': "List.SpoolUpload",
        'fileSizePerUser': '0'
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-03-12
작성목적 : 파일 업로드 (웹서비스 값 받기)
**********************************************************************************************/
List.SpoolUpload = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessUpload");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-03-12
작성목적 : 파일 업로드 리스트 보여주기
**********************************************************************************************/
List.SuccessUpload = function (data, status) {

    $('#phFile').setTemplateURL('/DeskPlusEIP/SNS/Jt/MyTalk/Content/JTSpoolFile.htm?seq=' + Guids());
    $('#phFile').processTemplate(data.d);

    setTimeout(function () {
        List.FileResource();
    }, 100);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-03-12
작성목적 : 첨부파일 리스트 초기화 보여주기
**********************************************************************************************/
List.FileList = function () {
    $('#phFile').setTemplateURL('/DeskPlusEIP/SNS/Jt/MyTalk/Content/JTSpoolFile.htm?seq=' + Guids());
    $('#phFile').processTemplate("");

    setTimeout(function () {
        List.FileResource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-03-12
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.FileResource = function () {
    /*
    $("#j_spool_fileName")[0].innerHTML = R_FileName;
    $("#j_spool_fileType")[0].innerHTML = R_FileType;
    $("#j_spool_fileLen")[0].innerHTML = R_FileLength;
    $("#j_spool_filedelete")[0].innerHTML = R_FileDel;
    //$("#j_spool_fileorder")[0].innerHTML = R_FileOrder;
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    //if ($(".BtnUp") != undefined) { $(".BtnUp").attr('src', ImagePath + '/Btn/sort_down.gif'); }
    //if ($(".BtnDown") != undefined) { $(".BtnDown").attr('src', ImagePath + '/Btn/sort_on.gif'); }
    */

    if ($(".fa-times-circle").length > 0) {
        $(".fa-times-circle").attr("title", R_FileDel);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-03-12
작성목적 : 파일 삭제
**********************************************************************************************/
function DelSpoolPath(fileName) {

    if (window.confirm(R_MsgDel)) {
        ParamArray = {
            'fileName': fileName,
            'swfUploadGUID': SwfUploadGUID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("List.SpoolUpload");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-13
작성목적 : Spool폴더에 있는 파일 삭제
***************************************************/
List.DelSpoolFile = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    var url = "/DeskPlusEIP/SNS/Ws/MyTalk/Content/List.asmx/DelSpoolFile";
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.FileList");

    AjaxService(url, data, obj);
}

