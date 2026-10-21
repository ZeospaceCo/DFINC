Trash = {}

/**************************************************

작성일자 : 2011-07-08
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {
    // 문서 리스트 가져오기
    setTimeout("Trash.getList();", 400);

    $('#btRestore').click(function (e) {
        Trash.restore();
    });

    $('#btAll').click(function (e) {
        NoticeHide();
        Trash.checkAll();
    });

    $('#btDel').click(function (e) {
        Trash.del();
    });

    $('#pvNum').click(function (e) {
        e.preventDefault();
        Trash.pvNum();
    });

    $('#nxNum').click(function (e) {
        e.preventDefault();
        Trash.nxNum();
    });

    $('#listTitle').click(function (e) {
        location.href = "Folder.aspx";
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });
});

/**************************************************

작성일자 : 2011-07-08
작성목적 : 메일 리스트 가져오기 (웹서비스 호출)
***************************************************/
Trash.getList = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Mail/Ws/GetMail.asmx/GetTrashList";

    //파라미터값 배열에 담기
    ParamArray = {
        'PageNo': PageNo,
        'PageSize': PageSize
    };

    var obj = eval("Trash.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************

작성일자 : 2011-07-08
작성목적 : 메일 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
Trash.succeeded = function (data, status) {
    $('#phMailList').setTemplateURL('/DeskPlusMobile/Mail/Jt/JTTrash.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phMailList').processTemplate(data.d);

    setTimeout(function () {
        Trash.Resource();
    }, 100);
}

/**********************************************************************************************

작성일자 : 2012-03-30
작성목적 : 다국어 처리
**********************************************************************************************/
Trash.Resource = function () {
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

작성일자 : 2011-11-14
작성목적 : 메일 보기 화면으로 가기
***************************************************/
Trash.viewList = function (mailsn, read_flag, code) {
    ParamArray = {
        'Mail_sn': mailsn,
        'Code': 6,
        'PageNo': PageNo,
        'Read_flag': read_flag
    };

    CallEncrypt(CallGetParam());

    location.href = "ViewTrash.aspx?p=" + GetEncryptValue;
}

/**************************************************

작성일자 : 2011-07-08
작성목적 : 메일 복원하기  (웹서비스 호출)
***************************************************/
Trash.restore = function () {
    LoadingShow(true);

    var tmpValue = "";
    for (var i = 0; i < $("input:checkbox").length; i++) {
        if ($("input:checkbox")[i].checked == true) {
            tmpValue += $("input:checkbox")[i].value + ",";
        }
    }

    if (tmpValue == "") {
        NoticeShow("!", R_RestoreNoMail);
        LoadingShow(false);
        return;
    }

    var url = "/DeskPlusMobile/Mail/Ws/SetMail.asmx/Restore";

    //파라미터값 배열에 담기
    ParamArray = {
        'Mail_sn': tmpValue
    };

    var obj = eval("Trash.succeededRestore");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************

작성일자 : 2011-07-08
작성목적 : 메일 복원하기 (웹서비스 값 받기)
***************************************************/
Trash.succeededRestore = function (data, status) {
    LoadingShow(false);

    ParamArray = {
        'PageNo': PageNo
    };

    CallEncrypt(CallGetParam());

    location.href = "Trash.aspx?p=" + GetEncryptValue;
}

/**************************************************

작성일자 : 2011-07-19
작성목적 : 전체선택 / 전체해제
***************************************************/
Trash.checkAll = function () {
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
작성자   : shimpark
작성일자 : 2014-10-20
작성목적 : 메일 삭제하기  (웹서비스 호출)
***************************************************/
Trash.del = function () {
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
                'Code': 6,
                'Mail_sn': tmpValue
            };

            var obj = eval("Trash.succeededDel");
            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxAsyncService(url, data, obj);
        }
    })
}

/**************************************************
작성자   : shimpark
작성일자 : 2014-10-20
작성목적 : 메일 삭제하기 (웹서비스 값 받기)
***************************************************/
Trash.succeededDel = function (data, status) {
    LoadingShow(false);

    ParamArray = {
        'PageNo': PageNo
    };

    CallEncrypt(CallGetParam());

    location.href = "Trash.aspx?p=" + GetEncryptValue;
}



/**************************************************
작성자   : famo23
작성일자 : 2015-10-29
작성목적 : 이전 페이지 이동
***************************************************/
Trash.pvNum = function () {

    if (parseInt(PageNo) > 0) {
        PageNo = parseInt(PageNo) - 1;

        ParamArray = {
            'PageNo': PageNo
        };

        CallEncrypt(CallGetParam());

        location.href = "Trash.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-29
작성목적 : 다음 페이지 이동
***************************************************/
Trash.nxNum = function () {

    var tmpTotalNum = $("#TotalNum").html();

    if ((parseInt(PageNo) + 1) < parseInt(tmpTotalNum)) {
        PageNo = parseInt(PageNo) + 1;

        ParamArray = {
            'PageNo': PageNo
        };

        CallEncrypt(CallGetParam());

        location.href = "Trash.aspx?p=" + GetEncryptValue;
    }
}