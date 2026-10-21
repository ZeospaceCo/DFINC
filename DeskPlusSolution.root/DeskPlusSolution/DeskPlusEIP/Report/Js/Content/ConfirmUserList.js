ConfirmUserList = {}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {
    $(document).ready(function () {
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            ConfirmUserList.Confirm();
        });

        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        ConfirmUserList.LoadData();
    });
});

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 초기화
***************************************************/
ConfirmUserList.LoadData = function (gubun) {
    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
    $('#loading').html(loadingPath);

    setTimeout(function () {
        ConfirmUserList.GetPageList();
    }, 100);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
ConfirmUserList.Resource = function () {
    $("#j_Name")[0].innerHTML = R_ReportUser;
    $("#j_Flag")[0].innerHTML = R_Gubun;

    if ($(".flag") != undefined) { $(".flag").text("-"); }
    if ($(".flag1") != undefined) { $(".flag1").text(R_Save); }
    if ($(".flag2") != undefined) { $(".flag2").text(R_ConfirmRequest); }
    if ($(".flag3") != undefined) { $(".flag3").text(R_WorkConfirm); }
    if ($(".showlist") != undefined) { $(".showlist").text(R_WorkView); }

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
ConfirmUserList.GetPageList = function () {
    var url = "/DeskPlusEIP/Report/Ws/Content/ConfirmUserList.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        "startDate": StartDate,
        "gubun": Gubun
    };

    var obj = eval("ConfirmUserList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
ConfirmUserList.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Report/Jt/Content/JTConfirmUserList.htm?seq=' + Guids());
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ConfirmUserList.Resource();

    $("#scrollDiv").css({ 'overflow-y': 'auto', 'min-height': '320px', 'height': '320px' });
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 화면 보기
**********************************************************************************************/
ConfirmUserList.Viewer = function (p_listID) {
    ParamArray = {
        'listID': p_listID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Report/Content/ViewPopup.aspx";
    var param = "p=" + GetEncryptValue;

    WindowObjectReference = PopupWinOpen(url, param, "Viewer" + Guids(), "1200", "650", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 전체 체크박스
***************************************************/
ConfirmUserList.Checked = function () {
    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 선택된 값 가져오기
***************************************************/
ConfirmUserList.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 선택된 값 넘기기
***************************************************/
ConfirmUserList.Confirm = function () {
    var selValue = ConfirmUserList.CheckedValue();
    if (selValue != "") {
        parent.GetReportUser(selValue);
    }

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}