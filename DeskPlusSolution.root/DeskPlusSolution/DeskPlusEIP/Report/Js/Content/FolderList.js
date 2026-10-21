FolderList = {}

$(function () {
    $(document).ready(function () {
        //event
        //$("#ddlType").change(function (e) { e.preventDefault(); ModSetting.SelFolderType(); });

        // [추가]
        $('#folderAdd').click(function (e) { e.preventDefault(); FolderList.FolderAdd(); });
        // [수정]
        $('#folderMod').click(function (e) { e.preventDefault(); FolderList.FolderMod(); });
        // [삭제]
        $('#folderDel').click(function (e) { e.preventDefault(); FolderList.FolderDel(); });
        // [하위순서]
        $('#folderSeq').click(function (e) { e.preventDefault(); FolderList.FolderSeq(); });
        // [권한지정-조직도]
        $('#folderSet').click(function (e) { e.preventDefault(); FolderList.FolderSet(); });

        $("#divList").hide();
    });
});

/**************************************************
작성자   :
작성일자 :
작성목적 : 폴더추가
***************************************************/
FolderList.FolderAdd = function () { //alert("폴더추가");
    ParamArray = { 'folderID': FolderID, 'isAdd': '1' };
    CallEncrypt(CallGetParam());
    var url = "FolderAdd.aspx?p=" + GetEncryptValue;
    PopupWin("400", "440", url, "", "", true);
}

/**************************************************
작성자   :
작성일자 :
작성목적 : 폴더수정
***************************************************/
FolderList.FolderMod = function () {
    if (FolderID == 0) { alert(R_Msg_ErrSelFolder); return; }

    ParamArray = { 'folderID': FolderID, 'isAdd': '0' };
    CallEncrypt(CallGetParam());
    var url = "FolderAdd.aspx?p=" + GetEncryptValue;
    PopupWin("400", "440", url, "", "", true);
}

/**************************************************
작성자   :
작성일자 :
작성목적 : 폴더삭제
***************************************************/
FolderList.FolderDel = function () {
    if (FolderID == 0) { alert(R_Msg_ErrSelFolder); return; }
    if (ChildCnt > 0) { alert(R_MSG_ErrDelSubFolder); return; }

    if (confirm(R_Msg_Del)) {
        var url = "/DeskPlusEIP/Report/Ws/Content/FolderList.asmx/DelFolder";
        ParamArray = { 'folderID': FolderID };
        var obj = eval("FolderList.SuccessFolderDel");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   :
작성일자 :
작성목적 : 성공시, 폴더삭제
***************************************************/
FolderList.SuccessFolderDel = function (data, status) {
    var rtnval = data.d;

    if (data.d == true) {
        alert(R_MSG_SUCCESS);
        location = "/DeskPlusEIP/Report/Content/FolderList.aspx";
    }
    else {
        alert("error");
    }
}

/**************************************************
작성자   :
작성일자 :
작성목적 : 하위순서
***************************************************/
FolderList.FolderSeq = function () {
    ParamArray = { 'folderID': FolderID };
    CallEncrypt(CallGetParam());
    var url = "FolderOrder.aspx?p=" + GetEncryptValue;
    PopupWin("400", "380", url, "", "", true);
}

/**************************************************
작성자   :
작성일자 :
작성목적 : 권한지정-조직도
***************************************************/
FolderList.FolderSet = function () {
    if (FolderID == 0) { alert(R_Msg_ErrSelFolder); return; }

    ParamArray = { 'func': 'GetUserDept' };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Opinion/Folder/GetUserDept.aspx?p=" + GetEncryptValue;    
    WindowObjectReference = window.open(url, "FolderSet_" + Guids(), "width=500px,height=477px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}

/**************************************************
작성자   :
작성일자 : 신현숙 (수정 : 심재운)
작성목적 : 권한추가
***************************************************/
function GetUserDept(_values) { //alert(_deptList)

    /* 구분-아이디;구분-아이디; */
    var ownerList = "";

    if (_values != "") {
        
        var selStr = _values.split(';');
        $.each(selStr, function (i, v) {
            var arrVal = v.split('|');
            ownerList += arrVal[0] + "-" + arrVal[1] + ";";        
        });
    }

    if (ownerList == "") { return; }

    var url = "/DeskPlusEIP/Report/Ws/Content/FolderList.asmx/AddFolderAuth";
    ParamArray = { 'folderID': FolderID, 'ownerList': ownerList };
    var obj = eval("FolderList.GetFolderAuth");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   :
작성일자 :
작성목적 : 폴더별 권한리스트 가져오기
***************************************************/
FolderList.GetFolderAuth = function () {
    if (FolderID == 0) { return; }

    $("#divList").show();

    var url = "/DeskPlusEIP/Report/Ws/Content/FolderList.asmx/GetFolderAuthList";
    ParamArray = { 'folderID': FolderID };
    var obj = eval("FolderList.SuccessFolderAuth");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   :
작성일자 :
작성목적 : 성공시, 폴더별 권한리스트 가져오기
***************************************************/
FolderList.SuccessFolderAuth = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Report/Jt/Content/JTFolderAuthList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    FolderList.Resource();    //다국어처리
}

/**************************************************
작성자   :
작성일자 :
작성목적 : 폴더의 사용자 권한 삭제
***************************************************/
FolderList.DelFolderAuth = function (p_gubun, p_ownerID) {
    if (p_gubun == "" || p_ownerID == "") { return; }

    if (confirm(R_Msg_Del)) {
        var url = "/DeskPlusEIP/Report/Ws/Content/FolderList.asmx/DelFolderAuth";
        ParamArray = { 'folderID': FolderID, 'gubun': p_gubun, 'ownerID': p_ownerID };
        var obj = eval("FolderList.GetFolderAuth");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   :
작성일자 :
작성목적 : 리소스 가져오기
***************************************************/
FolderList.Resource = function () {
    
    $("#r_name").text(R_AuthName);
    $("#r_gubun").text(R_Gubun);
    $("#r_del").text(R_Del);

    $(".Group").text(R_Dept);
    $(".Member").text(R_User);

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**************************************************
onNodeSelect 트리 노드 선택
버튼처리 : folderAdd, folderMod, folderDel, folderSeq, folderSet
- 자사인경우만 리스트(divList) / 권한지정 버튼이 보임
- 시스템노드의 경우, 수정/삭제/권한지정 버튼 안보임.
***************************************************/
function onNodeSelect(node) {
    FolderID = node.ID;
    ChildCnt = node.GetProperty("ChildCnt");
    FolderList.GetFolderAuth();
}