BoardView = {}

/**************************************************
작성자   : famo23
작성일자 : 2012-07-12
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    if (ReadAuth == "False") {
        BoardView.goBefore();
        return;
    }

    if (WriteAuth == "False") {
        $("#btnAdd").hide();
    }

    $('#btnAdd').click(function (e) {
        BoardView.add();
    });

    $('#minus').click(function (e) {
        e.preventDefault();
        BoardView.minus();
    });

    $('#plus').click(function (e) {
        e.preventDefault();
        BoardView.plus();
    });

    $('#before').click(function (e) {
        BoardView.goBefore();
    });

    $('#listTitle').click(function (e) {
        BoardView.goBefore();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    if (window.history && window.history.pushState) {
        window.history.replaceState(null, null, '');
        $(window).on('popstate', function (event) {
            LoadingShow(false);
        });
    }

});


/**************************************************
작성자   : famo23
작성일자 : 2012-07-12
작성목적 : 이전 글 가기
***************************************************/
BoardView.minus = function () {
    if (BeforeBoardID != "0") {
        ParamArray = {
            'CommunityID': CommunityID,
            'MenuID': MenuID,
            'BoardID': BeforeBoardID,
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "BoardView.aspx?p=" + GetEncryptValue;
    }
    else {
        action_popup.alert(R_NoList);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-07-12
작성목적 : 다음 글 가기
***************************************************/
BoardView.plus = function () {
    if (NextBoardID != "0") {
        ParamArray = {
            'CommunityID': CommunityID,
            'MenuID': MenuID,
            'BoardID': NextBoardID,
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "BoardView.aspx?p=" + GetEncryptValue;
    }
    else {
        action_popup.alert(R_NoList);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-07-12
작성목적 : 목록 페이지 이동
***************************************************/
BoardView.goBefore = function () {

    ParamArray = {
        'CommunityID': CommunityID,
        'MenuID': MenuID,
        'PageNo': PageNo,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "BoardList.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2012-07-12
작성목적 : 댓글 등록 페이지 이동
***************************************************/
BoardView.add = function () {

    ParamArray = {
        'CommunityID': CommunityID,
        'MenuID': MenuID,
        'BoardID': BoardID,
        'PageNo': PageNo,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "Add.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2012-07-10
작성목적 : 첨부파일 읽기 (웹서비스 호출)
***************************************************/
BoardView.goAttach = function (path, fileName) {
    var url = "/DeskPlusMobile/Common/Ws/GetCommon.asmx/CopyAttach";

    //파라미터값 배열에 담기
    ParamArray = {
        'Path': path,
        'FileName': encodeURIComponent(fileName)
    };

    var obj = eval("BoardView.succeededGoAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}


/**************************************************
작성자   : famo23
작성일자 : 2012-07-10
작성목적 : 첨부파일 읽기 (웹서비스 값 받기)
***************************************************/
BoardView.succeededGoAttach = function (data, status) {
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
작성목적 : 첨부파일 읽기 (웹서비스 호출)
***************************************************/
BoardView.goDS = function (fileID) {
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