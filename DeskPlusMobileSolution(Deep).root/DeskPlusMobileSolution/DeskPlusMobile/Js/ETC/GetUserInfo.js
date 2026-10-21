GetUserInfo = {}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    //대행자지우기
    $("#btClean").click(function (e) {
        e.preventDefault();
        GetUserInfo.Clean();
    });

    $('#btConfirm').click(function (e) {
        e.preventDefault();
        GetUserInfo.Confirm();
    });

    $('#btClose').click(function (e) {
        e.preventDefault();
        GetUserInfo.Close();
    });

    //검색
    $('#search').click(function (e) {
        e.preventDefault();
        GetUserInfo.search();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    //검색입력필드 엔터 이벤트 기술
    $("#tbSearch").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            GetUserInfo.search();
        }
    });

    $("#tbSearch").attr("placeholder", R_TagDic_UPNM);

});

/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : [닫기]
**********************************************************************************************/
GetUserInfo.Close = function () {

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : 검색
***************************************************/
GetUserInfo.SetSearch = function () {

    GetUserInfo.search();

    return false;
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : 검색  (웹서비스 호출)
***************************************************/
GetUserInfo.search = function () {

    var searchValue = $("#tbSearch").val();

    if (jQuery.trim(searchValue) != "") {

        var url = "/DeskPlusMobile/ETC/Ws/GetETC.asmx/GetUser";

        //파라미터값 배열에 담기
        ParamArray = {
            'Sv': searchValue.securityText()
        };

        var obj = eval("GetUserInfo.succeeded");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxAsyncService(url, data, obj);
    }
    else {
        $("#lbUser").empty();
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : 검색 (웹서비스 값 받기)
***************************************************/
GetUserInfo.succeeded = function (data, status) {

    $("#lbUser").empty();

    if (data != null) {
        $.each(data.d, function (i, o) {
            $("#lbUser").append("<span class='CDeptUser' id='" + i + "_DeptUser'><a onclick=\"javascript:GetUserInfo.SetUserInfo('" + i + "','" + data.d[i].NameValue + "')\" style='cursor:pointer'>" + data.d[i].NameText + "</a></span><br>");
        });
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : 사용자 지정
***************************************************/
GetUserInfo.SetUserInfo = function (_valID, _val) {

    if (_val == "") return;
    if (_val == undefined) return;

    $(".CDeptUser").css("background-color", "");
    $("#" + _valID + "_DeptUser").css("background-color", "#d3d3d3");

    var tmpval = _val.split('|');

    $("#hdnUserID").val(tmpval[0]);
    $("#ReceivePerson").text(tmpval[1]);
    $("#hdnDeptID").val(tmpval[2]);
    $("#hdnDeptName").val(tmpval[3]);

}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : 문서양식안에 지정되어 있는 업무대행자 지우기
***************************************************/
GetUserInfo.Clean = function () {

    parent.ifrmProcessing.CleanUserInfo();

    GetUserInfo.Close();
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : 확인
***************************************************/
GetUserInfo.Confirm = function () {

    var userName = $("#ReceivePerson").text();
    var userID = $("#hdnUserID").val();
    var deptID = $("#hdnDeptID").val();
    var deptName = $("#hdnDeptName").val();

    if (userName != "") {

        parent.ifrmProcessing.GetUserInfo(userID, userName, deptName, deptID);

        GetUserInfo.Close();
    }
    else {
        parent.action_popup.alert(R_MsgPerson);
    }
}
