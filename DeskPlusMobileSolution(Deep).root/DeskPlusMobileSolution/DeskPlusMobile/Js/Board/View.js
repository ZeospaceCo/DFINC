View = {}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-13
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $('#btnAdd').click(function (e) {
        View.add();
    });

    $('#minus').click(function (e) {
        e.preventDefault();
        View.minus();
    });

    $('#plus').click(function (e) {
        e.preventDefault();
        View.plus();
    });

    $('#before').click(function (e) {
        View.goBefore();
    });

    $('#listTitle').click(function (e) {
        View.goBefore();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    if (OptionUseShortReply == "1") {
        setTimeout(function () {
            View.getShortReplyCount();
            View.getShortReplyList();
        }, 100);
    }

    if (window.history && window.history.pushState) {
        window.history.replaceState(null, null, '');
        $(window).on('popstate', function (event) {
            LoadingShow(false);
        });
    }
});

/**************************************************
작성자   : kkzz52
작성일자 : 2018-10-17
작성목적 : 게시판 리스트 카운트 가져오기 (웹서비스 호출)
***************************************************/
View.getShortReplyCount = function () {

    var url = "/DeskPlusMobile/Board/Ws/GetBoard.asmx/GetShortReplyCount";

    //파라미터값 배열에 담기
    ParamArray = {
        'CID': CID,
        'ID': ID
    };

    var obj = eval("View.succeededCount");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-10-17
작성목적 : 게시판 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededCount = function (data, status) {
    if (data.d > 0) {
        $('#TitleShortReply').css("display", "");
        $(".ReplyRowCnt").text(data.d);
    }
    else {
        $('#TitleShortReply').css("display", "none");
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-10-17
작성목적 : 게시판 리스트 가져오기 (웹서비스 호출)
***************************************************/
View.getShortReplyList = function () {

    var url = "/DeskPlusMobile/Board/Ws/GetBoard.asmx/GetShortReplyList";

    //파라미터값 배열에 담기
    ParamArray = {
        'CID': CID,
        'ID': ID,
        'optionShortReplyGood': OptionShortReplyGood,
        'optionShortReplyTop': OptionShortReplyTop
    };

    var obj = eval("View.succeededShortReply");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-10-17
작성목적 : 게시판 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededShortReply = function (data, status) {

    $('#phShortList').setTemplateURL('/DeskPlusMobile/Board/Jt/JTShortReply.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phShortList').processTemplate(data.d);

    setTimeout(function () {
        View.Resource();
    }, 100);

    // 댓글 추천/비추천 기능 사용 안할 경우
    if (OptionShortReplyGood == "0") {
        if ($(".goodbtn") != undefined) { $(".goodbtn").css("display", "none"); }
    }

    if ($('#dvAdd').length == 0) {
        if ($(".replyGroupAdd") != undefined) { $(".replyGroupAdd").css("display", "none"); }
    }
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2018-10-17
작성목적 : 다국어 처리
**********************************************************************************************/
View.Resource = function () {

    if ($(".replyGroupAdd") != undefined) {
        $(".replyGroupAdd").text(R_AddShortReply);
    }
    if ($(".delReply") != undefined) {
        $(".delReply").text("[" + R_DelReply + "]");
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2011-07-13
작성목적 : 이전 글 가기
***************************************************/
View.minus = function () {
    if (BeforeListID != "0") {
        ParamArray = {
            'PCatID': PCatID,
            'MainCID': MainCID,
            'PageNo': PageNo,
            'CID': BeforeCatID,
            'ID': BeforeListID,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "View.aspx?p=" + GetEncryptValue;
    }
    else {
        action_popup.alert(R_NoList);
        return;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-13
작성목적 : 다음 글 가기
***************************************************/
View.plus = function () {
    if (NextListID != "0") {
        ParamArray = {
            'PCatID': PCatID,
            'MainCID': MainCID,
            'PageNo': PageNo,
            'CID': NextCatID,
            'ID': NextListID,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "View.aspx?p=" + GetEncryptValue;
    }
    else {
        action_popup.alert(R_NoList);
        return;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-13
작성목적 : 목록 페이지 이동
***************************************************/
View.goBefore = function () {

    ParamArray = {
        'MainCID': MainCID,
        'PCatID': PCatID,
        'PageNo': PageNo,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-13
작성목적 : 댓글 등록 페이지 이동
***************************************************/
View.add = function () {

    ParamArray = {
        'PCatID': PCatID,
        'MainCID': MainCID,
        'PageNo': PageNo,
        'CID': CID,
        'ID': ID,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "Add.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-13
작성목적 : 첨부파일 읽기 (웹서비스 호출)
***************************************************/
View.goAttach = function (path, fileName) {

    var url = "/DeskPlusMobile/Common/Ws/GetCommon.asmx/CopyAttach";

    //파라미터값 배열에 담기
    ParamArray = {
        'Path': path,
        'FileName': encodeURIComponent(fileName),
        'OptionDownload': OptionDownload
    };

    var obj = eval("View.succeededGoAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-13
작성목적 : 첨부파일 읽기 (웹서비스 값 받기)
***************************************************/
View.succeededGoAttach = function (data, status) {
    if (data.d == "!NULL") {
        NoticeShow("!", R_ProcessingError);
        return;
    }
    else if (data.d == "FAIL") {
        NoticeShow("!", R_NoDownload);
        return;
    }
    else {
        var tmpAttachName = data.d;
        var lastidx = tmpAttachName.lastIndexOf(".");
        var tmpFiletype = tmpAttachName.substring(lastidx + 1, tmpAttachName.length);

        if (tmpFiletype.toLowerCase() != "mht") {
            if (MobileType == "ios") {

                ParamArray = {
                    'companyID': CompanyID,
                    'userID': UserID,
                    'attachName': encodeURIComponent(tmpAttachName)
                };

                CallEncrypt(CallGetParam());

                var url = HttpType + MobileDOMAIN + "/DeskPlusMobile/Common/AttachUrl.aspx?p=" + GetEncryptValue;
                //window.webkit.messageHandlers.open_safari.postMessage(url);
                openSafari(url);
            }
            else {
                location.href = "/DeskPlusMobile/FileData/" + CompanyID + "/users/" + UserID + "/Spool/Mobile/" + encodeURIComponent(tmpAttachName);
            }
        }
        else {
            ParamArray = {
                'AttachName': encodeURIComponent(tmpAttachName)
            };

            CallEncrypt(CallGetParam());

            location.href = "/DeskPlusMobile/Common/AttachView.aspx?p=" + GetEncryptValue;
        }
    }
}

/**************************************************
작성자   : 이중호
작성일자 : 2018-04-13
작성목적 : 문서 스트링 포
***************************************************/
View.goDS = function (fileID) {
    var url = "http://sp.deskplus.co.kr:9979/docviewer.melita";

    var form = document.createElement("form");
    var parm = new Array();
    var input = new Array();

    form.action = url;
    //    form.target = "_blank";
    form.method = "post";

    parm.push(['FILE_ID', fileID]);
    parm.push(['client', 'mobile_web']);

    for (var i = 0; i < parm.length; i++) {
        input[i] = document.createElement("input");
        input[i].setAttribute("type", "hidden");
        input[i].setAttribute('name', parm[i][0]);
        input[i].setAttribute("value", parm[i][1]);
        form.appendChild(input[i]);
    }

    document.body.appendChild(form);
    form.submit();
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-10-11
작성목적 : 댓글의 댓글 입력창
***************************************************/
View.GroupAdd = function (_companyID, _shortReplyID) {

    ParamArray = {
        'companyID': _companyID,
        'PCatID': PCatID,
        'MainCID': MainCID,
        'PageNo': PageNo,
        'CID': CID,
        'ID': ID,
        'shortReplyID': _shortReplyID,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "AddShortReply.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-10-15
작성목적 : 댓글 수정
***************************************************/
View.ModShortReply = function (_companyID, _shortReplyID) {

    ParamArray = {
        'companyID': _companyID,
        'PCatID': PCatID,
        'MainCID': MainCID,
        'PageNo': PageNo,
        'CID': CID,
        'ID': ID,
        'shortReplyID': _shortReplyID,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "ModShortReply.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-10-15
작성목적 : 댓글 삭제
***************************************************/
View.DelReply = function (_companyID, _shortReplyID, _groupID) {

    action_popup.confirm(R_DelList, function (res) {
        if (res) {

            LoadingShow(true);

            var url = "/DeskPlusMobile/Board/Ws/SetBoard.asmx/DelShortReply";

            //파라미터값 배열에 담기
            ParamArray = {
                'companyID': _companyID,
                'CID': CID,
                'ID': ID,
                'shortReplyID': _shortReplyID,
                'groupID': _groupID
            };

            var obj = eval("View.succeeded");
            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxAsyncService(url, data, obj);
        }
    })
}

/**************************************************
작성자   : kkzz52
작성일자 : 2011-07-13
작성목적 : 댓글 삭제 (웹서비스 값 받기)
***************************************************/
View.succeeded = function (data, status) {

    if (data.d) {
        View.getShortReplyCount();
        View.getShortReplyList();

        LoadingShow(false);
    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-09-14
작성목적 : 댓글 추천/비추천 참여 여부
***************************************************/
View.GoodCount = function (_companyID, _shortReplyID, _good, _userID) {

    if (UserID == _userID) {
        action_popup.alert(R_MyComment); // 자신의 댓글은 추천/비추천할 수 없습니다.
        return;
    }

    $("#hdCompanyID").val(_companyID);
    $("#hdShortReplyID").val(_shortReplyID);
    $("#hdGood").val(_good);

    ParamArray = {
        'companyID': _companyID,
        'CID': CID,
        'ID': ID,
        'shortReplyID': _shortReplyID
    };

    var url = "/DeskPlusMobile/Board/Ws/SetBoard.asmx/GetGoodCount";

    var obj = eval("View.SuccessGoodCount");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2018-09-14
작성목적 : 댓글 추천/비추천 참여 여부 성공시 
**********************************************************************************************/
View.SuccessGoodCount = function (data, status) {
    if (data.d == 0) {
        if ($("#hdGood").val() == "1") {
            View.GoodReple($("#hdShortReplyID").val());
        }
        else if ($("#hdGood").val() == "0") {
            View.BadReple($("#hdShortReplyID").val());
        }
    }
    else {
        action_popup.alert(R_AlreadyComment); // 이미 참여한 댓글입니다.
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-09-14
작성목적 : 댓글 추천/비추천
***************************************************/
View.GoodReple = function (_shortReplyID) {

    ParamArray = {
        'companyID': $("#hdCompanyID").val(),
        'CID': CID,
        'ID': ID,
        'shortReplyID': _shortReplyID,
        'good': '1',
        'bad': '0'
    };

    var url = "/DeskPlusMobile/Board/Ws/SetBoard.asmx/AddShortReplyGood";

    var obj = eval("View.SuccessGoodReple");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2018-09-14
작성목적 : 댓글 추천/비추천 성공시 페이지 표시
**********************************************************************************************/
View.SuccessGoodReple = function (data, status) {
    action_popup.alert(R_Recommend); // 추천하였습니다.
    View.getShortReplyList();
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-09-14
작성목적 : 댓글 추천/비추천
***************************************************/
View.BadReple = function (_shortReplyID) {

    action_popup.confirm(R_Decommendation, function (res) {
        if (res) {
            ParamArray = {
                'companyID': $("#hdCompanyID").val(),
                'CID': CID,
                'ID': ID,
                'shortReplyID': _shortReplyID,
                'good': '0',
                'bad': '1'
            };

            var url = "/DeskPlusMobile/Board/Ws/SetBoard.asmx/AddShortReplyGood";

            var obj = eval("View.SuccessBadReple");
            CallEncrypt(CallGetParam());
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxAsyncService(url, data, obj);
        }
    })
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2018-09-14
작성목적 : 댓글 추천/비추천 성공시 페이지 표시
**********************************************************************************************/
View.SuccessBadReple = function (data, status) {
    View.getShortReplyList();
}