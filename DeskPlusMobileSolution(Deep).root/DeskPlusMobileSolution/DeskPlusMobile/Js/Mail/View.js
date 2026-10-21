View = {}

/**************************************************

작성일자 : 2011-07-08
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {
    if (Read_flag == "1") {
        setTimeout("View.modData();", 300);
        //메일 읽음 표시
    }
    else {
        //LoadingShow(true);
        setTimeout("View.getData();", 300);
    }

    $('#btDel').click(function (e) {
        View.del();
    });

    $('#btRec').click(function (e) {
        View.send('1');
    });

    $('#btFor').click(function (e) {
        View.send('2');
    });

    $('#btAllRec').click(function (e) {
        View.send('3');
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
        e.preventDefault();
        View.goBefore();
    });

    $('#btOriginal').click(function (e) {
        View.btOriginal();
    });

    $('#listTitle').click(function (e) {
        e.preventDefault();
        View.goBefore();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    $('#btToCc').click(function (e) {
        e.preventDefault();
        if (ShowToCc == "N") {
            $("#btToCc").html("(" + R_ViewToCc + " ▲)");
            $("#ToCc").show();
            ShowToCc = "Y";
        }
        else {
            $("#btToCc").html("(" + R_ViewToCc + " ▼)");
            $("#ToCc").hide();
            ShowToCc = "N";
        }
    });

    if (window.history && window.history.pushState) {
        window.history.replaceState(null, null, '');
        $(window).on('popstate', function (event) {
            LoadingShow(false);
        });
    }
});


/**************************************************

작성일자 : 2011-11-11
작성목적 : 메일 읽은 표시 하기 (웹서비스 호출)
***************************************************/
View.modData = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Mail/Ws/SetMail.asmx/Mod";

    //파라미터값 배열에 담기
    ParamArray = {
        'Code': Code,
        'Mail_sn': Mail_sn
    };

    var obj = eval("View.succeededMod");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************

작성일자 : 2011-11-14
작성목적 : 메일 읽은 표시 하기 (웹서비스 값 받기)
***************************************************/
View.succeededMod = function (data, status) {
    View.getData();
}

/**************************************************
작성자   : freeman
작성일자 : 2011-07-09
작성목적 : 원본 메일 보기
***************************************************/
View.btOriginal = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Mail/Ws/GetMail.asmx/GetContent";

    //파라미터값 배열에 담기
    ParamArray = {
        'Code': Code,
        'Mail_sn': Mail_sn
    };

    var obj = eval("View.succeededOriginal");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}


/**************************************************
작성자   : freeman
작성일자 : 2011-07-09
작성목적 : 원본 메일 내용 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededOriginal = function (data, status) {
    var conObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경  

    var tmp = conObj.GetContent[0].body;
    $("#contentList").html(tmp);
}


/**************************************************

작성일자 : 2011-11-11
작성목적 : 메일 내용 가져오기 (웹서비스 호출)
***************************************************/
View.getData = function () {
    var url = "/DeskPlusMobile/Mail/Ws/GetMail.asmx/GetTotalContent";

    //파라미터값 배열에 담기
    ParamArray = {
        'Code': Code,
        'Mail_sn': Mail_sn,
        'MSv': SearchValue.securityText()
    };

    var obj = eval("View.succeededGet");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************

작성일자 : 2011-11-11
작성목적 : 메일 내용 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededGet = function (data, status) {

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
            tmpTag += "<tr><td class='dva'><a onclick=\"View.goMailAttach('" + data.d[0].attachLists[i].downloadUrl + "&fileName=" + data.d[0].attachLists[i].name + "')\">" + data.d[0].attachLists[i].name + "</a>";
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

작성일자 : 2011-07-09
작성목적 : 메일 첨부파일 읽기 (웹서비스 호출)
***************************************************/
View.goAttach = function (path, fileName) {

    var url = "/DeskPlusMobile/Common/Ws/GetCommon.asmx/CopyAttach";

    //파라미터값 배열에 담기
    ParamArray = {
        'Path': path,
        'FileName': fileName.securityText()
    };

    var obj = eval("View.succeededGoAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}

/**************************************************

작성일자 : 2011-07-09
작성목적 : 메일 첨부파일 읽기 (웹서비스 값 받기)
***************************************************/
View.succeededGoAttach = function (data, status) {
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

작성일자 : 2011-07-09
작성목적 : 목록 페이지 이동
***************************************************/
View.goBefore = function () {
    ParamArray = {
        'Code': Code,
        'PageNo': PageNo,
        'MSv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}


/**************************************************

작성일자 : 2011-07-09
작성목적 : 이전 글 가기
***************************************************/
View.minus = function () {
    View.getBeforeNext('Down');
}

/**************************************************

작성일자 : 2011-07-09
작성목적 : 다음 글 가기
***************************************************/
View.plus = function () {
    View.getBeforeNext('Top');
}

/**************************************************

작성일자 : 2011-07-09
작성목적 : 이전 , 다음 글 가기 
***************************************************/
View.getBeforeNext = function (gubun) {

    Mail_sn = (gubun == "Top") ? PrevUid : NextUid;
    //Mail_sn = (gubun == "Top") ? NextUid : PrevUid;
    //Mail_sn = (gubun == "Down") ? PrevUid : NextUid;    

    if (Mail_sn == "") {
        //var tmpMsg = (gubun == "Top") ? R_NextMailMessage : R_PrevMailMessage;
        //NoticeShow("", tmpMsg);
        action_popup.alert(R_NoList);
        return;
    }

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

작성일자 : 2011-11-14
작성목적 : 메일 삭제하기  (웹서비스 호출)
***************************************************/
View.del = function () {
    action_popup.confirm(R_DelList, function (res) {
        if (res) {

            LoadingShow(true);

            var url = "/DeskPlusMobile/Mail/Ws/SetMail.asmx/Del";

            //파라미터값 배열에 담기
            ParamArray = {
                'Code': Code,
                'Mail_sn': Mail_sn
            };

            var obj = eval("View.succeededDel");
            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxAsyncService(url, data, obj);
        }
    })
}


/**************************************************

작성일자 : 2011-07-09
작성목적 : 메일 삭제하기 (웹서비스 값 받기)
***************************************************/
View.succeededDel = function (data, status) {
    LoadingShow(false);
    View.goBefore();
}

/**************************************************

작성일자 : 2011-07-09
작성목적 : 메일 답장, 전달하기 
***************************************************/
View.send = function (action) {

    ParamArray = {
        'Action': action,
        'Code': Code,
        'Mail_sn': Mail_sn,
        'PageNo': PageNo,
        'History': '2',
        'MSv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "Send.aspx?p=" + GetEncryptValue;

}


/**************************************************
작성자   : famo23
작성일자 : 2018-12-26
작성목적 : 메일 첨부파일 읽기
***************************************************/
View.goMailAttach = function (url) {
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