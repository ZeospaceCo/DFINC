Mod = {}

var targetTag = "";
/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //구분 (0:일반결재,1:수신결재,2:참조결재)
        Mod.GetList("ZD");   
        Mod.GetList("ZP");
        Mod.GetList("ZC");
    });
});

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 타입별 결재선 리스트 가져오기 - 구분 (0:일반결재,1:수신결재,2:참조결재)
**********************************************************************************************/
Mod.GetList = function (_tag) {

    targetTag = _tag;

    var section = "0";
    if (targetTag == "ZP" || targetTag == "ZQ") { section = "1"; }
    if (targetTag == "ZC") { section = "2"; }

    ParamArray = {
        'favSeqID': FavSeqID
        , 'section': section
    };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessGetList");
    var url = "/DeskPlusEIP/Approval/Ws/Preference/Flow/Mod.asmx/GetList";
    AjaxService(url, data, obj);
}
Mod.SuccessGetList = function (data, status) {

    var listObj = "placeholder_ZD";
    if (targetTag == "ZP" || targetTag == "ZQ") { listObj = "placeholder_ZP"; }
    if (targetTag == "ZC") { listObj = "placeholder_ZC"; }

    $('#' + listObj).setTemplateURL('/DeskPlusEIP/Approval/Jt/Preference/Flow/JTListLine.htm?seq=' + Guids());
    $('#' + listObj).processTemplate(data.d);

    Mod.Resource();
}

Mod.Resource = function () {

    $(".r_number").text(R_Number);
    $(".r_tag").text(R_FavTag);
    $(".r_isDept").text(R_FavSec);
    $(".r_deptName").text(R_DeptName);
    $(".r_titleName").text(R_TitleName);
    $(".r_dutyName").text(R_DutyName);
    $(".r_userName").text(R_UserName);
    $(".r_del").text(R_Del);

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoData); }
    //if ($(".BtnDel") != undefined) { $(".BtnDel").attr('src', ImagePath + '/Icon/delete.gif'); }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 항목삭제
***************************************************/
Mod.DelItem = function (_favSeqLineID, _tag) {    //alert(_favSeqLineID + ":" + _tag);

    if (window.confirm(R_Msg_Del)) {

        targetTag = _tag;

        ParamArray = {
            'id': _favSeqLineID
        };
        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Mod.SuccessDelItem");
        var url = "/DeskPlusEIP/Approval/Ws/Preference/Flow/Mod.asmx/DelItem";
        AjaxService(url, data, obj);
    }
}
Mod.SuccessDelItem = function (data, status) {
    if (data.d == true) {
        Mod.GetList(targetTag);
    }
}

// 수신프로세스 체크
Mod.IsCheckedZQ = function (_targetTag) {

    var objs = $('#placeholder_ZP > table > tbody > tr');
    if (objs.length > 0 && $(".tags", objs).length > 0) {        //alert($(".tags", objs)[0].innerHTML + "==" + _targetTag);
        if ($(".tags", objs)[0].innerHTML != _targetTag) {
            return false;
        }
    }
    return true;
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [일반결재/합의결재/협조결재/참조결재/수신결재]
***************************************************/
Mod.ModTag = function (_tag) {                      // alert(_tag)

    targetTag = _tag;
    var gubun = (targetTag == "ZC") ? "3" : "2";    // 2 : 사용자 만, 3 : 부서+사용자

    /* 수신의 경우, 체크 
    - ZP/ZQ둘중 하나만 지정되도록
    - 동일부서의 사용자가 여러명 지정되지 않도록
    */
    if (targetTag == "ZP" || targetTag == "ZQ") {

        if (!Mod.IsCheckedZQ(targetTag)) {
            alert(R_MSG_AlertProcess);
            return;
        }
    }

    ParamArray = {
        'gubun': gubun
        , 'callback': 'Mod.SetApprovalTag'
    };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Approval/Common/Popup/GetDeptUser.aspx?p=" + GetEncryptValue;
    PopupWin(400, 507, url, "-500", "", true);
}
Mod.SetApprovalTag = function (_response) {     //alert(targetTag);

    if (_response != "") {
        // 대상결재정보
        var selvals = unescape(_response); 
        //alert(selvals); return;

        //하위부서 지정여부
        var isSub = "0";
        if (selvals.indexOf("G,") > 0) {
            if (window.confirm(R_MSG_AlertIsSub)) {
                isSub = "1";
            }
        }

        //구분(프로세스별 그룹정보)
        var section = "0";
        if (targetTag == "ZP" || targetTag == "ZQ") { section = "1"; }
        if (targetTag == "ZC") { section = "2"; }

        ParamArray = {
            'favSeqID': FavSeqID
            , 'isSub': isSub
            , 'tag': targetTag
            , 'section': section
            , 'selvals': selvals
        };
        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Mod.SuccessSetApprovalTag");
        var url = "/DeskPlusEIP/Approval/Ws/Preference/Flow/Mod.asmx/AddItem";
        AjaxService(url, data, obj);
    }
}
Mod.SuccessSetApprovalTag = function (data, status) {
    if (data.d == true) {
        Mod.GetList(targetTag);
    } else {
        alert(R_MSG_FAIL);
    }
}


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 순서변경
***************************************************/
Mod.ModSeq = function (_tag) {     //alert(_tag);
    targetTag = _tag;

    var section = "0";
    if (targetTag == "ZP" || targetTag == "ZQ") { section = "1"; }
    if (targetTag == "ZC") { section = "2"; }

    ParamArray = {
        'favSeqID': FavSeqID
        , 'section': section
    };
    CallEncrypt(CallGetParam());
    var url = "ModSequence.aspx?p=" + GetEncryptValue;
    PopupWin("400", "400", url, "", "", true);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 확인
***************************************************/
Mod.Confirm = function () {

    //필수입력
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbSubject", R_Msg_ErrSubj, true);

    ParamArray = {
        'favSeqID': FavSeqID
            , 'subject': $("#ctbSubject").val()
            , 'memo': $("#ctbMemo").val()
    };
    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessConfirm");
    var url = "/DeskPlusEIP/Approval/Ws/Preference/Flow/Mod.asmx/ModFav";
    AjaxService(url, data, obj);
}
Mod.SuccessConfirm = function (data, status) {
    if (data.d == true) {
        alert(R_MSG_SUCCESS);
        Mod.Close();
    } else {
        alert(R_MSG_FAIL);
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 닫기
***************************************************/
Mod.Close = function () {
    ParamArray = {
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "List.aspx?p=" + GetEncryptValue;
}