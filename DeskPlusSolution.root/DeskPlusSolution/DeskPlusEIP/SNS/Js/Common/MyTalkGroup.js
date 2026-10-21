MyTalkGroup = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-06-19
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            MyTalkGroup.LoadData("");
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
            MyTalkGroup.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                MyTalkGroup.Search();
            }
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            MyTalkGroup.PageSize();
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            MyTalkGroup.Search();
        });

        //새로고침
        $('#CbtnReload').click(function (e) {
            e.preventDefault();
            MyTalkGroup.Search();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        // 댓글 추가
        $('#cbtnShortReply').click(function (e) {
            e.preventDefault();
            MyTalkGroup.Add();
        });

        //목록
        $('#CbtnList').click(function (e) {
            e.preventDefault();
            MyTalkGroup.GoList();
        });


        //파일선택
        $('#cbtnAddFile').click(function (e) {
            e.preventDefault();
            MyTalkGroup.FileUpload();
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
            MyTalkGroup.MaxLang();
        }

        //날짜검색
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            MyTalkGroup.RegDate();
        });

        //MyTalkGroup.LoadData();
        $("#textarea").select();

        MyTalkGroup.Init();

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
MyTalkGroup.Init = function () {
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

    MyTalkGroup.FileList();
    MyTalkGroup.LoadData("");

    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : famo23
작성일자 : 2013-06-19
작성목적 : 등록일자 활성화 여부
***************************************************/
MyTalkGroup.RegDate = function () {
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
작성일자 : 2013-06-19
작성목적 : 글자 제한
***************************************************/
MyTalkGroup.MaxLang = function () {
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
작성일자 : 2013-06-19
작성목적 : 초기화
***************************************************/
MyTalkGroup.LoadData = function (gubun) {
    MyTalkGroup.GetCount(); //검색 총 카운트 가져오기

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        MyTalkGroup.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        MyTalkGroup.GetCount(); //검색 총 카운트 가져오기
        MyTalkGroup.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                MyTalkGroup.LoadData("search");
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
작성일자 : 2013-06-19
작성목적 : 검색
***************************************************/
MyTalkGroup.Search = function () {
    var response = MyTalkGroup.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리

    MyTalkGroup.LoadData("search");
}

/**************************************************
작성자   : famo23
작성일자 : 2013-06-19
작성목적 : 검색 필드 설정하기
***************************************************/
MyTalkGroup.SettingSearch = function () {
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
작성일자 : 2013-06-19
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
MyTalkGroup.GetPageList = function () {
    var url = "/DeskPlusEIP/SNS/Ws/Common/MyTalkGroup.asmx/GetPageList";

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

    var obj = eval("MyTalkGroup.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-06-19
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
MyTalkGroup.SuccessPageList = function (data, status) {
    $('#phMyTalkGroupGroup').setTemplateURL('/DeskPlusEIP/SNS/Jt/Common/JTMyTalkGroup.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phMyTalkGroupGroup').processTemplate(data.d);

    $('#loading').html("");

    MyTalkGroup.Resource();
    $("#scrollDiv").css({ 'overflow-y': 'auto', 'min-height': '230px', 'height': '230px' });

    /*
    if (data.d == "") {
    TmpRUserID = "0";
    }
    else {
    TmpRUserID = data.d[0].UserID;
    }
    */
}

/**************************************************
작성자   : famo23
작성일자 : 2013-06-19
작성목적 : 페이지 사이즈에 이동하기
***************************************************/
MyTalkGroup.PageSize = function () {
    CurrentPage = 1; //1페이지로 이동처리
    RecordPerPage = $('#ddlListCount').val();
    MyTalkGroup.LoadData("search");
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-06-19
작성목적 : 다국어 처리
**********************************************************************************************/
MyTalkGroup.Resource = function () {
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    if ($(".NoAuthListData") != undefined) { $(".NoAuthListData").text(R_SecurityMsg); }

    $(".AddMsg").text(R_AddShortReply);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-06-19
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
MyTalkGroup.GetCount = function () {
    var url = "/DeskPlusEIP/SNS/Ws/Common/MyTalkGroup.asmx/GetCount";

    ParamArray = {
        'rUserID': RUserID,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("MyTalkGroup.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-06-19
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
MyTalkGroup.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
}

/**************************************************
작성자   : famo23
작성일자 : 2013-06-19
작성목적 : 댓글 저장
***************************************************/
MyTalkGroup.Add = function () {
    /*
    if (TmpRUserID == "0") {
    alert(R_MyTalkOne);
    return;
    }
    */

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
        'groupID': GroupID,
        'swfUploadGUID': SwfUploadGUID
    };

    var url = "/DeskPlusEIP/SNS/Ws/Common/MyTalk.asmx/AddToMyTalk";
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("MyTalkGroup.SuccessAdd");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-06-19
작성목적 : 댓글 추가 성공시 페이지 표시
**********************************************************************************************/
MyTalkGroup.SuccessAdd = function (data, status) {
    if (data.d == true) {
        alert(R_Processing);

        $("#textarea").html("");

        CurrentPage = 1; //1페이지로 이동처리
        MyTalkGroup.LoadData("search");
        MyTalkGroup.DelSpoolFile();

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
            MyTalkGroup.MaxLang();
        }
    }
    else {
        alert(R_MsgError);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-06-19
작성목적 : 댓글 삭제
**********************************************************************************************/
MyTalkGroup.DelReple = function (myTalkID, groupID) {
    if (confirm(R_ApplyDelete)) {
        if (myTalkID == groupID) {
            ParamArray = {
                'groupID': groupID
            };

            CallEncrypt(CallGetParam())

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("MyTalkGroup.SuccessDelMyTalk");
            var url = "/DeskPlusEIP/SNS/Ws/Common/MyTalkGroup.asmx/DelGroupMyTalk";
            AjaxService(url, data, obj);
        }
        else {
            ParamArray = {
                'myTalkID': myTalkID
            };

            CallEncrypt(CallGetParam())

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("MyTalkGroup.SuccessDelMyTalk");
            var url = "/DeskPlusEIP/SNS/Ws/Common/MyTalkGroup.asmx/DelMyTalk";
            AjaxService(url, data, obj);
        }
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-06-19
작성목적 : 댓글 삭제 성공
**********************************************************************************************/
MyTalkGroup.SuccessDelMyTalk = function (data, status) {
    if (data.d) {
        alert(R_Processing);
        CurrentPage = 1; //1페이지로 이동처리
        MyTalkGroup.LoadData("search");
    }
    else
        alert(R_MsgError);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-06-19
작성목적 : 댓글 삭제 성공
**********************************************************************************************/
MyTalkGroup.SuccessDelGroupShortReply = function (data, status) {
    if (data.d) {
        alert(R_Processing);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else
        alert(R_MsgError);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-06-19
작성목적 : 담벼락 글의 댓글 보기
**********************************************************************************************/
MyTalkGroup.ReplyView = function (groupID) {
    var groupReplyChk = $("#GroupReplyCheck_" + groupID)[0].innerHTML;

    if (groupReplyChk == "") {
        $("#GroupReplyCheck_" + groupID)[0].innerHTML = "1";
        TmpGroup = groupID;

        ParamArray = {
            'groupID': groupID
        };

        var url = "/DeskPlusEIP/SNS/Ws/Common/MyTalkGroup.asmx/GetShortReply";
        CallEncrypt(CallGetParam());
        data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("MyTalkGroup.SuccessReplyView");

        AjaxService(url, data, obj);
    }
    else {
        $("#GroupReply_" + groupID)[0].innerHTML = "";
        $("#GroupReplyCheck_" + groupID)[0].innerHTML = "";
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-06-19
작성목적 : 담벼락 글의 댓글 보기 성공시 페이지 표시
**********************************************************************************************/
MyTalkGroup.SuccessReplyView = function (data, status) {
    var tmpHTML = "";

    $.each(data.d, function (index, list) {
        if (list.View == "Y") {
            tmpHTML += "<li><table class='blog_cont_table'><caption>My talk</caption><colgroup><col width='90px'><col></colgroup><tbody><tr><td class='blog_img' rowspan='2'>";
            tmpHTML += "<table width='100%'><tbody><tr><td width='48px' style='text-align:right;'>" + list.Depth + "</td><td style='text-align:right;'><a onclick='List.MyList(" + list.UserID + ");'>" + list.PhotoImage + "</a></td>";
            tmpHTML += "</tr></tbody></table></td><td><div class='tk_b'><p class='lb'><span class='subtext'><a onclick='AccountInfoWin(" + list.UserID + ");'>" + list.UserName + "</a> | " + list.WriteDateYMD + "</span>&nbsp;&nbsp;";

            if (list.ShowNew == "Y") {
                tmpHTML += "<span class='m_talk'></span>&nbsp;";
            }

            if (list.IsAuth == "Y") {
                tmpHTML += "<a onclick='MyTalkGroup.DelReple(" + list.MyTalkID + "," + list.GroupID + ");'><img src='/DeskPlusEIP/Design/C/Img/Icon/ico_tree_listDel.gif' /></a>";
            }

            tmpHTML += "<br>" + list.Content + "</p><div class='arr'></div></div></td></tr></tbody></table></li>";
        }
        else {

            tmpHTML += "<li><table class='blog_cont_table'><caption>My talk</caption><colgroup><col width='90px'><col></colgroup><tbody><tr><td class='blog_img' rowspan='2'>";
            tmpHTML += "<table width='100%'><tbody><tr><td width='48px' style='text-align:right;'>" + list.Depth + "</td><td style='text-align:right;'></td>";
            tmpHTML += "</tr></tbody></table></td><td><div class='tk_b'><p class='lb'>";
            tmpHTML += "<br>" + R_SecurityMsg + "</p><div class='arr'></div></div></td></tr></tbody></table></li>";
        }
    });

    $("#GroupReply_" + TmpGroup)[0].innerHTML = tmpHTML;

    TmpGroup = "";
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-06-19
작성목적 : 담벼락 글에 댓글 추가
**********************************************************************************************/
MyTalkGroup.GroupAdd = function (groupID, p_rUserID) {
    var replyChk = $("#ReplyCheck_" + groupID)[0].innerHTML;

    if (replyChk == "") {
        var tmpVal = "<div><div class='layout_subbox'><table><tr><td><span class='m_talk'></span>&nbsp;";
        tmpVal += "<span class='subtext'>" + R_ReplyAdd + "</span>&nbsp;( <span id='charsRemaining_" + groupID + "'></span> )</td>";
        tmpVal += "<td><table><tr><td>&nbsp;&nbsp;<span style='float:right;padding-right:10px;'><input id='chkOpen_" + groupID + "' type='checkbox' name='chkOpen' checked='checked'>" + R_ReplyOpen + "</span></td>";
        tmpVal += "<td><ul class='btn_small'><li><a href='#' onclick='MyTalkGroup.GroupReplyAdd(" + groupID + "," + p_rUserID + ")')><span>" + R_Save + "</span></a></li>";
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
작성일자 : 2013-06-19
작성목적 : 그룹의 댓글 추가
***************************************************/
MyTalkGroup.GroupReplyAdd = function (groupID, p_rUserID) {
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

    var url = "/DeskPlusEIP/SNS/Ws/Common/MyTalkGroup.asmx/AddToMyTalk";
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("MyTalkGroup.SuccessGroupReplyAdd");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-06-19
작성목적 : 그룹의 댓글 추가 성공시 페이지 표시
**********************************************************************************************/
MyTalkGroup.SuccessGroupReplyAdd = function (data, status) {
    if (data.d == true) {
        alert(R_Processing);

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

/**************************************************
작성자   : famo23
작성일자 : 2015-03-13
작성목적 : 파일 업로드 (웹서비스 호출)
***************************************************/
MyTalkGroup.FileUpload = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'SNS',
        'callBackEvent': "MyTalkGroup.SpoolUpload",
        'fileSizePerUser': '0'
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-03-13
작성목적 : 파일 업로드 (웹서비스 값 받기)
**********************************************************************************************/
MyTalkGroup.SpoolUpload = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("MyTalkGroup.SuccessUpload");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-03-13
작성목적 : 파일 업로드 리스트 보여주기
**********************************************************************************************/
MyTalkGroup.SuccessUpload = function (data, status) {

    $('#phFile').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#phFile').processTemplate(data.d);

    setTimeout(function () {
        MyTalkGroup.FileResource();
    }, 100);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-03-13
작성목적 : 첨부파일 리스트 초기화 보여주기
**********************************************************************************************/
MyTalkGroup.FileList = function () {
    $('#phFile').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#phFile').processTemplate("");

    setTimeout(function () {
        MyTalkGroup.FileResource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-03-13
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
MyTalkGroup.FileResource = function () {
    //첨부파일
    $("#j_spool_fileName")[0].innerHTML = R_FileName;
    $("#j_spool_fileType")[0].innerHTML = R_FileType;
    $("#j_spool_fileLen")[0].innerHTML = R_FileLength;
    $("#j_spool_filedelete")[0].innerHTML = R_FileDel;
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-03-13
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
        var obj = eval("MyTalkGroup.SpoolUpload");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-03-13
작성목적 : Spool폴더에 있는 파일 삭제
***************************************************/
MyTalkGroup.DelSpoolFile = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    var url = "/DeskPlusEIP/SNS/Ws/MyTalk/Content/List.asmx/DelSpoolFile";
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("MyTalkGroup.FileList");

    AjaxService(url, data, obj);
}