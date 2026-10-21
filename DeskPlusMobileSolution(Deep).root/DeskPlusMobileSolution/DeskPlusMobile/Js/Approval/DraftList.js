DraftList = {}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    // 문서 리스트 가져오기
    setTimeout("DraftList.getList();", 400);

    $('#pvNum').click(function (e) {
        e.preventDefault();
        DraftList.pvNum();
    });

    $('#nxNum').click(function (e) {
        e.preventDefault();
        DraftList.nxNum();
    });

    $('#listTitle').click(function (e) {
        location.href = "WFolder.aspx";
    });

    $('#btAll').click(function (e) {
        NoticeHide();
        DraftList.checkAll();
    });

    $('#btDel').click(function (e) {
        DraftList.del();
    });

    $('#search').click(function (e) {
        e.preventDefault();
        DraftList.search();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    $("#tbSearch").attr("placeholder", R_PrivateDraftName);

});


/**************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 문서 리스트 가져오기 (웹서비스 호출)
***************************************************/
DraftList.getList = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetDraftList";

    //파라미터값 배열에 담기
    ParamArray = {
        'PageNo': PageNo,
        'PageSize': PageSize,
        'Sv': SearchValue.securityText()
    };

    var obj = eval("DraftList.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 문서 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
DraftList.succeeded = function (data, status) {

    $('#phApproval').setTemplateURL('/DeskPlusMobile/Approval/Jt/JTDraftList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phApproval').processTemplate(data.d);

    setTimeout(function () {
        DraftList.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 다국어 처리
**********************************************************************************************/
DraftList.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 상신 화면으로 가기 
***************************************************/
DraftList.Add = function (draftID) {

    ParamArray = {
        'DraftID': draftID,
        'PageNo': PageNo,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());
    document.location.href = "DraftAdd.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 검색
***************************************************/
DraftList.search = function () {

    SearchValue = $("#tbSearch").val();

    ParamArray = {
        'PageNo': 1,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "DraftList.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 검색
***************************************************/
DraftList.SetSearch = function () {

    DraftList.search();

    return false;
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 이전 페이지 이동
***************************************************/
DraftList.pvNum = function () {

    if (parseInt(PageNo) > 1) {
        PageNo = parseInt(PageNo) - 1;

        ParamArray = {
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "DraftList.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 다음 페이지 이동
***************************************************/
DraftList.nxNum = function () {

    var tmpTotalNum = $("#TotalNum").html();

    if (parseInt(PageNo) < parseInt(tmpTotalNum)) {
        PageNo = parseInt(PageNo) + 1;

        ParamArray = {
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "DraftList.aspx?p=" + GetEncryptValue;
    }
}
/**************************************************
작성자   : famo23
작성일자 : 2019-08-14
작성목적 : 전체선택 / 전체해제
***************************************************/
DraftList.checkAll = function () {

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
작성일자 : 2019-08-14
작성목적 : 삭제하기
***************************************************/
DraftList.del = function () {

    var tmpValue = "";
    for (var i = 0; i < $("input:checkbox").length; i++) {
        if ($("input:checkbox")[i].checked == true) {
            tmpValue += $("input:checkbox")[i].value + ",";
        }
    }

    if (tmpValue == "") {
        NoticeShow("!", R_MSG_SelDel);
        return;
    }

    action_popup.confirm(R_DelList, function (res) {
        if (res) {
            LoadingShow(true);

            var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/DelDraft";

            //파라미터값 배열에 담기
            ParamArray = {
                'DraftID': tmpValue
            };

            var obj = eval("DraftList.succeededDel");
            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxAsyncService(url, data, obj);
        }
    })
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-14
작성목적 : 삭제하기 (웹서비스 값 받기)
***************************************************/
DraftList.succeededDel = function (data, status) {
    LoadingShow(false);

    ParamArray = {
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "DraftList.aspx?p=" + GetEncryptValue;
}