View = {}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {
    DWidth = $(document).width();

    setTimeout("View.getData();", 400);

    $('#btDel').click(function (e) {
        View.del();
    });

    $('#btAdd').click(function (e) {
        View.add();
    })

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

});


/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 댓글 내용 가져오기 (웹서비스 호출)
***************************************************/
View.getData = function () {

    var url = "/DeskPlusMobile/MyTalk/Ws/GetMyTalk.asmx/GetMyTalkView";

    //파라미터값 배열에 담기
    ParamArray = {
        'GroupID': GroupID,
        'DWidth': DWidth
    };

    var obj = eval("View.succeededData");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 댓글 내용 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededData = function (data, status) {

    var myObj = data.d;

    $("#date").empty();
    $("#userInfo").empty();
    $("#content").empty();

    $("#date").append(myObj[0].WriteDateYMD);
    $("#userInfo").append(myObj[0].UserName);
    $("#content").append(myObj[0].Content);
    $("#content").append("<br>");
    $("#content").append(myObj[0].FileImage);

    RUserID = myObj[0].UserID;

    if (myObj[0].IsAuth == "Y") {
        jQuery.each($("#btDel"), function () { this.style.display = ""; });
    }

    var tmpSR = "";

    if (myObj.length > 1) {
        jQuery.each($("#reply"), function () { this.style.display = ""; });
    }
    else {
        jQuery.each($("#reply"), function () { this.style.display = "none"; });
    }

    for (var i = 1; i < myObj.length; i++) {

        if (myObj[i].View == "Y") {
            tmpSR += "<div class='title'><div>";
            tmpSR += "<table border='0' cellpadding='0' cellspacing='0' style='width:100%'>";
            tmpSR += "<tr><td align='center'>&nbsp;</td><td class='lif'>";
            tmpSR += myObj[i].UserName + "&nbsp;| " + myObj[i].WriteDateYMD;
            tmpSR += "<br />";
            tmpSR += myObj[i].Content;

            if (myObj[i].IsAuth == "Y") {
                tmpSR += "&nbsp;<img src='/DeskPlusMobile/Content/ko/Base/Img/ico_tree_listDel.gif' class='icon_Talk' onclick=\"javascript:View.DelMyTalk(" + myObj[i].MyTalkID + ")\" />";
            }

            tmpSR += "</td>";
            tmpSR += "<td>&nbsp;</td></tr><tr>"
            tmpSR += "<td height='1' colspan='3' align='center' bgcolor='#CCCCCC'></td></tr></table>";
            tmpSR += "</table>";
            tmpSR += "</div></div>";
        }
        else {
            tmpSR += "<div class='title'><div>";
            tmpSR += "<table border='0' cellpadding='0' cellspacing='0' style='width:100%'>";
            tmpSR += "<tr><td align='center'>&nbsp;</td><td class='lif'>";
            tmpSR += R_SecurityMsg;
            tmpSR += "</td>";
            tmpSR += "<td>&nbsp;</td></tr><tr>"
            tmpSR += "<td height='1' colspan='3' align='center' bgcolor='#CCCCCC'></td></tr></table>";
            tmpSR += "</table>";
            tmpSR += "</div></div>";
        }
    }

    $("#reply").empty();
    $("#reply").append(tmpSR);

}


/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 목록 페이지 이동
***************************************************/
View.goBefore = function () {
    ParamArray = {
        'PageNo': PageNo
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 마이톡 댓글 등록 화면으로 가기 
***************************************************/
View.add = function () {

    ParamArray = {
        'GroupID': GroupID,
        'PageNo': PageNo,
        'RUserID': RUserID
    };

    CallEncrypt(CallGetParam());

    location.href = "Add.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 댓글 삭제하기  (웹서비스 호출)
***************************************************/
View.del = function () {
    action_popup.confirm(R_DelList, function (res) {
        if (res) {

            LoadingShow(true);

            var url = "/DeskPlusMobile/MyTalk/Ws/SetMyTalk.asmx/DelGroupMyTalk";

            //파라미터값 배열에 담기
            ParamArray = {
                'GroupID': GroupID
            };

            var obj = eval("View.succeededDel");
            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxAsyncService(url, data, obj);
        }
    })
}


/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 댓글 삭제하기 (웹서비스 값 받기)
***************************************************/
View.succeededDel = function (data, status) {
    LoadingShow(false);

    if (data.d) {
        View.goBefore();
    }
    else {
        NoticeShow("!", R_ProcessingError);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-14
작성목적 : 댓글 삭제하기 - 댓글의 댓글 (웹서비스 호출)
***************************************************/
View.DelMyTalk = function (_myTalkID) {
    action_popup.confirm(R_DelList, function (res) {
        if (res) {

            LoadingShow(true);

            var url = "/DeskPlusMobile/MyTalk/Ws/SetMyTalk.asmx/DelMyTalk";

            //파라미터값 배열에 담기
            ParamArray = {
                'myTalkID': _myTalkID
            };

            var obj = eval("View.succeededDelMyTalk");
            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxAsyncService(url, data, obj);
        }
    })
}


/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 댓글 삭제하기 (웹서비스 값 받기)
***************************************************/
View.succeededDelMyTalk = function (data, status) {
    LoadingShow(false);

    if (data.d) {
        View.getData();
    }
    else {
        NoticeShow("!", R_ProcessingError);
    }
}