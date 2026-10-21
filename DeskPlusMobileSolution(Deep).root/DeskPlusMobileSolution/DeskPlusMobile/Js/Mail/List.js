List = {}

/**************************************************

작성일자 : 2011-07-08
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    if (Code == "1") {
        $('#listTitle').html("<i class='fas fa-envelope dropbtn '></i>" + R_ReceiveMail);
    }
    else if (Code == "0800") {
        $('#listTitle').html("<i class='fas fa-envelope dropbtn '></i>" + R_ToMeMail);
    }
    else if (Code == "3") {
        $('#listTitle').html("<i class='fas fa-envelope dropbtn '></i>" + R_SendMail);
    }
    else if (Code == "4") {
        $('#listTitle').html("<i class='fas fa-envelope dropbtn '></i>" + R_DraftMail);
    }
    else {
        $('#listTitle').html("<i class='fas fa-envelope dropbtn '></i>" + R_PrivateMail);
    }

    // 문서 리스트 가져오기
    setTimeout("List.getList();", 400);

    $('#btAll').click(function (e) {
        NoticeHide();
        List.checkAll();
    });

    $('#btDel').click(function (e) {
        List.del();
    });

    $('#btSend').click(function (e) {
        List.send();
    });

    $('#pvNum').click(function (e) {
        e.preventDefault();
        List.pvNum();
    });

    $('#nxNum').click(function (e) {
        e.preventDefault();
        List.nxNum();
    });

    $('#listTitle').click(function (e) {
        if ((Code == "1") || (Code == "3") || (Code == "4") || (Code == "0800")) {
            location.href = "Folder.aspx";
        }
        else {
            location.href = "Private.aspx";
        }
    });

    $('#search').click(function (e) {
        e.preventDefault();
        List.search();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    $("#tbSearch").attr("placeholder", R_InputMail);

});


/**************************************************

작성일자 : 2011-07-08
작성목적 : 메일 리스트 가져오기 (웹서비스 호출)
***************************************************/
List.getList = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Mail/Ws/GetMail.asmx/GetMailList";

    //파라미터값 배열에 담기
    ParamArray = {
        'Code': Code,
        'PageNo': PageNo,
        'PageSize': PageSize,
        'MSv': SearchValue.securityText()
    };

    var obj = eval("List.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************

작성일자 : 2011-07-08
작성목적 : 메일 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
List.succeeded = function (data, status) {

    $('#phMailList').setTemplateURL('/DeskPlusMobile/Mail/Jt/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phMailList').processTemplate(data.d);

    setTimeout(function () {
        List.Resource();
    }, 100);
}


/**********************************************************************************************

작성일자 : 2012-03-30
작성목적 : 다국어 처리
**********************************************************************************************/
List.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }

    $(".receive").each(function () {
        $(this).text(R_Receive);
    });
    $(".refer").each(function () {
        $(this).text(R_Refer);
    });
    $(".refer2").each(function () {
        $(this).text(R_Refer2);
    });
}

/**************************************************

작성일자 : 2011-11-11
작성목적 : 메일 보기 화면으로 가기 
***************************************************/
List.viewList = function (mailsn, read_flag) {

    if (Code == "4") {
        ParamArray = {
            'Mail_sn': mailsn,
            'Code': Code,
            'PageNo': PageNo,
            'Read_flag': read_flag,
            'MSv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "DraftView.aspx?p=" + GetEncryptValue;
    }
    else {
        ParamArray = {
            'Mail_sn': mailsn,
            'Code': Code,
            'PageNo': PageNo,
            'Read_flag': read_flag,
            'MSv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "View.aspx?p=" + GetEncryptValue;
    }
}

/**************************************************

작성일자 : 2011-11-14
작성목적 : 메일 삭제하기  (웹서비스 호출)
***************************************************/
List.del = function () {

    var tmpValue = "";
    for (var i = 0; i < $("input:checkbox").length; i++) {
        if ($("input:checkbox")[i].checked == true) {
            tmpValue += $("input:checkbox")[i].value + ",";
        }
    }

    if (tmpValue == "") {
        NoticeShow("!", R_DelNoMail);
        return;
    }

    action_popup.confirm(R_DelList, function (res) {
        if (res) {
            LoadingShow(true);

            var url = "/DeskPlusMobile/Mail/Ws/SetMail.asmx/Del";

            //파라미터값 배열에 담기
            ParamArray = {
                'Code': Code,
                'Mail_sn': tmpValue
            };

            var obj = eval("List.succeededDel");
            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxAsyncService(url, data, obj);
        }
    })
}


/**************************************************

작성일자 : 2011-07-08
작성목적 : 메일 삭제하기 (웹서비스 값 받기)
***************************************************/
List.succeededDel = function (data, status) {
    LoadingShow(false);

    ParamArray = {
        'Code': Code,
        'PageNo': PageNo,
        'MSv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}


/**************************************************

작성일자 : 2011-07-11
작성목적 : 메일 답장, 전달하기 
***************************************************/
List.send = function () {

    ParamArray = {
        'Code': Code,
        'PageNo': PageNo,
        'History': '1',
        'MSv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "Send.aspx?p=" + GetEncryptValue;

}

/**************************************************

작성일자 : 2011-07-14
작성목적 : 전체선택 / 전체해제
***************************************************/
List.checkAll = function () {

    if ($("#allcheck").text() == R_SelAll) {
        if ($("input:checkbox").length > 0) {
            $("#allcheck").text(R_SelNoAll);

            for (var i = 0; i < $("input:checkbox").length; i++) {
                $("input:checkbox")[i].checked = true;
            }
        }
    }
    else {
        if ($("input:checkbox").length > 0) {
            $("#allcheck").text(R_SelAll);

            for (var i = 0; i < $("input:checkbox").length; i++) {
                $("input:checkbox")[i].checked = false;
            }
        }
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-29
작성목적 : 이전 페이지 이동
***************************************************/
List.pvNum = function () {

    if (parseInt(PageNo) > 0) {
        PageNo = parseInt(PageNo) - 1;

        ParamArray = {
            'Code': Code,
            'PageNo': PageNo,
            'MSv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-29
작성목적 : 다음 페이지 이동
***************************************************/
List.nxNum = function () {

    var tmpTotalNum = $("#TotalNum").html();

    if ((parseInt(PageNo) + 1) < parseInt(tmpTotalNum)) {
        PageNo = parseInt(PageNo) + 1;

        ParamArray = {
            'Code': Code,
            'PageNo': PageNo,
            'MSv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2014-08-25
작성목적 : 메일 검색
***************************************************/
List.search = function () {

    SearchValue = $("#tbSearch").val();

    ParamArray = {
        'Code': Code,
        'PageNo': 0,
        'MSv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;

}

/**************************************************
작성자   : famo23
작성일자 : 2014-08-25
작성목적 : 검색
***************************************************/
List.SetSearch = function () {

    List.search();

    return false;
}