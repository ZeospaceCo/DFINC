DraftView = {}

/**************************************************
작성자   : famo23
작성일자 : 2014-08-26
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    if (Read_flag == "1") {
        setTimeout("DraftView.modData();", 300);
    }
    else {
        //LoadingShow(true);
        setTimeout("DraftView.getData();", 300);
    }

    $('#btDel').click(function (e) {
        DraftView.del();
    });

    $('#btSend').click(function (e) {
        DraftView.send("1");
    });

    $('#minus').click(function (e) {
        e.preventDefault();
        DraftView.minus();
    });

    $('#plus').click(function (e) {
        e.preventDefault();
        DraftView.plus();
    });

    $('#before').click(function (e) {
        e.preventDefault();
        DraftView.goBefore();
    });


    $('#listTitle').click(function (e) {
        e.preventDefault();
        DraftView.goBefore();
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
작성일자 : 2014-08-26
작성목적 : 메일 읽은 표시 하기 (웹서비스 호출)
***************************************************/
DraftView.modData = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Mail/Ws/SetMail.asmx/Mod";

    //파라미터값 배열에 담기
    ParamArray = {
        'Code': Code,
        'Mail_sn': Mail_sn
    };

    var obj = eval("DraftView.succeededMod");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2014-08-26
작성목적 : 메일 읽은 표시 하기 (웹서비스 값 받기)
***************************************************/
DraftView.succeededMod = function (data, status) {
    DraftView.getData();
}

/**************************************************
작성자   : famo23
작성일자 : 2014-08-26
작성목적 : 메일 내용 가져오기 (웹서비스 호출)
***************************************************/
DraftView.getData = function () {
    var url = "/DeskPlusMobile/Mail/Ws/GetMail.asmx/GetTotalContent";

    //파라미터값 배열에 담기
    ParamArray = {
        'Code': Code,
        'Mail_sn': Mail_sn,
        'MSv': SearchValue.securityText()
    };

    var obj = eval("DraftView.succeededGet");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2014-08-26
작성목적 : 메일 내용 가져오기 (웹서비스 값 받기)
***************************************************/
DraftView.succeededGet = function (data, status) {

    $("#subject").empty();
    $("#fromList").empty();
    $("#dateList").empty();
    $("#contentList").empty();
    $("#toList").empty();
    $("#ccList").empty();



    $("#subject").append(data.d[0].subject);
    $("#fromList").append(data.d[0].from);
    $("#dateList").append(data.d[0].sendDate);
    $("#contentList").append(data.d[0].content);
    //$("#toList").append(data.d[0].to);
    $("#toList").text(data.d[0].to);

    if (data.d[0].cc != "") {
        jQuery.each($("#dtcc"), function () { this.style.display = "block"; });
        $("#ccList").text(data.d[0].cc);
    }

    NextUid = data.d[0].nextUid;
    PrevUid = data.d[0].prevUid;


    //첨부파일 표시
    $("#attachList").empty();

    if (data.d[0].attachLists.length > 0) {
        jQuery.each($("#att_line"), function () { this.style.display = ""; });
        var tmpTag = "";

        for (var i = 0; i < data.d[0].attachLists.length; i++) {
            tmpTag += "<table border='0' cellpadding='0' cellspacing='0' style='width:100%'>";
            //tmpTag += "<tr><td class='dva'><a href=\"" + data.d[0].attachLists[i].downloadUrl + "&fileName=" + data.d[0].attachLists[i].name + "\">" + data.d[0].attachLists[i].name + "</a>";
            tmpTag += "<tr><td class='dva'><a onclick=\"DraftView.goMailAttach('" + data.d[0].attachLists[i].downloadUrl + "&fileName=" + data.d[0].attachLists[i].name + "')\">" + data.d[0].attachLists[i].name + "</a>";
            tmpTag += "<span class='iste'>&nbsp;" + data.d[0].attachLists[i].size + "byte</td></tr>";
            tmpTag += "</table>";
        }
        $("#attachList").append(tmpTag);
    }
    else {
        jQuery.each($("#att_line"), function () { this.style.display = "none"; });
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2014-08-26
작성목적 : 메일 첨부파일 읽기 (웹서비스 호출)
***************************************************/
DraftView.goAttach = function (path, fileName) {

    var url = "/DeskPlusMobile/Common/Ws/GetCommon.asmx/CopyAttach";

    //파라미터값 배열에 담기
    ParamArray = {
        'Path': path,
        'FileName': fileName.securityText()
    };

    var obj = eval("DraftView.succeededGoAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}

/**************************************************
작성자   : famo23
작성일자 : 2014-08-26
작성목적 : 메일 첨부파일 읽기 (웹서비스 값 받기)
***************************************************/
DraftView.succeededGoAttach = function (data, status) {
    if (data.d == "!NULL") {
        NoticeShow("!", R_Error);
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
                LoadingShow(true);
            }
            location.href = "/DeskPlusMobile/FileData/" + CompanyID + "/users/" + UserID + "/Spool/Mobile/" + data.d;
        }
        else {
            ParamArray = {
                'AttachName': tmpAttachName
            };

            CallEncrypt(CallGetParam());

            location.href = "/DeskPlusMobile/Common/AttachView.aspx?p=" + GetEncryptValue;
        }
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2014-08-26
작성목적 : 목록 페이지 이동
***************************************************/
DraftView.goBefore = function () {
    ParamArray = {
        'Code': Code,
        'PageNo': PageNo,
        'MSv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2014-08-26
작성목적 : 이전 글 가기
***************************************************/
DraftView.minus = function () {
    DraftView.getBeforeNext('Down');
}

/**************************************************
작성자   : famo23
작성일자 : 2014-08-26
작성목적 : 다음 글 가기
***************************************************/
DraftView.plus = function () {
    DraftView.getBeforeNext('Top');
}

/**************************************************
작성자   : famo23
작성일자 : 2014-08-26
작성목적 : 이전 , 다음 글 가기 (웹서비스 호출)
***************************************************/
DraftView.getBeforeNext = function (gubun) {

    Mail_sn = (gubun == "Top") ? PrevUid : NextUid;

    if (Mail_sn == "") {
        //var tmpMsg = (gubun == "Top") ? R_NextMailMessage : R_PrevMailMessage;
        //NoticeShow("", tmpMsg);
        action_popup.alert(R_NoList);
        return;
    }

    //파라미터값 배열에 담기
    ParamArray = {
        'Mail_sn': Mail_sn,
        'Code': Code,
        'PageNo': PageNo,
        'Read_flag': "1",
        'MSv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;
}



/**************************************************
작성자   : famo23
작성일자 : 2014-08-26
작성목적 : 메일 삭제하기  (웹서비스 호출)
***************************************************/
DraftView.del = function () {
    action_popup.confirm(R_DelList, function (res) {
        if (res) {

            LoadingShow(true);

            var url = "/DeskPlusMobile/Mail/Ws/SetMail.asmx/Del";

            //파라미터값 배열에 담기
            ParamArray = {
                'Code': Code,
                'Mail_sn': Mail_sn
            };

            var obj = eval("DraftView.succeededDel");
            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxAsyncService(url, data, obj);
        }
    })
}


/**************************************************
작성자   : famo23
작성일자 : 2014-08-26
작성목적 : 메일 삭제하기 (웹서비스 값 받기)
***************************************************/
DraftView.succeededDel = function (data, status) {
    LoadingShow(false);
    DraftView.goBefore();
}

/**************************************************
작성자   : famo23
작성일자 : 2014-08-26
작성목적 : 메일 발송
***************************************************/
DraftView.send = function (action) {
    ParamArray = {
        'Action': action,
        'Code': Code,
        'Mail_sn': Mail_sn,
        'PageNo': PageNo,
        'History': '2',
        'MSv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "DraftSend.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2018-12-26
작성목적 : 메일 첨부파일 읽기
***************************************************/
DraftView.goMailAttach = function (url) {
    if (MobileType == "ios") {
        //window.webkit.messageHandlers.open_safari.postMessage(url);
        openSafari(url);
    }
    else {
        location.href = url;
    }

    /*
    if (AppVersion == "2") {
    ParamArray = {
    'url': url.split("&").join("^")
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusMobile/Common/AttachMail.aspx?p=" + GetEncryptValue;

    }
    else {
    location.href = url;
    }
    */
}
