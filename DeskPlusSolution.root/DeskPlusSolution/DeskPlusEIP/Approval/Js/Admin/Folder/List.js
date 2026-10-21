List = {}

$(function () {
    $(document).ready(function () {

        //event
        //$("#ddlType").change(function (e) { e.preventDefault(); ModSetting.SelFolderType(); });

        // [추가]
        $('#folderAdd').click(function (e) { e.preventDefault(); List.FolderAdd(); });
        // [수정]
        $('#folderMod').click(function (e) { e.preventDefault(); List.FolderMod(); });
        // [삭제]
        $('#folderDel').click(function (e) { e.preventDefault(); List.FolderDel(); });
        // [하위순서]
        $('#folderSeq').click(function (e) { e.preventDefault(); List.FolderSeq(); });
        // [권한지정-조직도]
        $('#folderSet').click(function (e) { e.preventDefault(); List.FolderSet(); });


        $("#divList").hide();
    });
});

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 폴더추가
***************************************************/
List.FolderAdd = function () { //alert("폴더추가");

    if (FolderID == 0) { alert(R_Msg_ErrSelFolder); return; }

    ParamArray = { 'folderID': FolderID, 'folderPath': FolderPath, 'isAdd': '1', 'typeInfo': TypeInfo };
    CallEncrypt(CallGetParam());
    var url = "Add.aspx?p=" + GetEncryptValue;
    PopupWin("400", "445", url, "", "", true);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 폴더수정
***************************************************/
List.FolderMod = function () {
    if (FolderID == 0) { alert(R_Msg_ErrSelFolder); return; }

    ParamArray = { 'folderID': FolderID, 'folderPath': FolderPath, 'isAdd': '0', 'typeInfo': TypeInfo };
    CallEncrypt(CallGetParam());
    var url = "Add.aspx?p=" + GetEncryptValue;
    PopupWin("400", "445", url, "", "", true);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 폴더삭제
***************************************************/
List.FolderDel = function () {

    if (FolderID == 0) { alert(R_Msg_ErrSelFolder); return; }
    if (confirm(R_Msg_Del)) {

        var url = "/DeskPlusEIP/Approval/Ws/Admin/Folder/List.asmx/GetDel";
        ParamArray = { 'folderID': FolderID };
        var obj = eval("List.ScsFolderDel");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}
List.ScsFolderDel = function (data, status) {
    var rtnval = data.d;    //성공여부-하위폴더여부-소속양식여부

    if (rtnval.split("-")[0] == "T") {
        alert(R_MSG_SUCCESS);
        document.location.reload();
        //document.location.href = "/DeskPlusEIP/Approval/Admin/Folder/List.aspx";
    } else {
        if (rtnval.split("-")[1] == "1") {
            alert(R_MSG_ErrDelSubFolder); return;
        }
        if (rtnval.split("-")[2] == "1") {
            alert(R_MSG_ErrDelForm); return;
        }
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 하위순서
***************************************************/
List.FolderSeq = function () {
    if (FolderID == 0) { alert(R_Msg_ErrSelFolder); return; }

    ParamArray = { 'folderID': FolderID };
    CallEncrypt(CallGetParam());
    var url = "ModSeq.aspx?p=" + GetEncryptValue;
    PopupWin("400", "400", url, "", "", true);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 권한지정-조직도
***************************************************/
List.FolderSet = function () {
    if (FolderID == 0) { alert(R_Msg_ErrSelFolder); return; }

    ParamArray = { 'gubun': '1' };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Approval/Common/Popup/GetDept.aspx?p=" + GetEncryptValue;
    PopupWin(400, 517, url, "-500", "", true);
}

/**************************************************
작성자   : 
작성일자 : 신현숙 (수정 : 심재운)
작성목적 : 권한추가
***************************************************/
function GetDept(_deptList, _values, _isSub) { //alert(_deptList)

    if (_deptList == 0) { return; }

    var url = "/DeskPlusEIP/Approval/Ws/Admin/Folder/List.asmx/AddAuthList";
    ParamArray = { 'folderID': FolderID, 'id': unescape(_deptList), 'isSub': _isSub };
    var obj = eval("List.GetFolderAuth");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 권한삭제
***************************************************/
List.DelFolderAuth = function (_authID) {
    if (_authID < 0) { return; }

    if (confirm(R_Msg_Del)) {

        var url = "/DeskPlusEIP/Approval/Ws/Admin/Folder/List.asmx/GetAuthDel";
        ParamArray = { 'authID': _authID };
        var obj = eval("List.GetFolderAuth");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 폴더별 권한리스트 가져오기
***************************************************/
List.GetFolderAuth = function () {
    if (FolderID == 0) { return; }

    var url = "/DeskPlusEIP/Approval/Ws/Admin/Folder/List.asmx/GetFolderAuthList";
    ParamArray = { 'folderID': FolderID };
    var obj = eval("List.ScsFolderAuth");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
List.ScsFolderAuth = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Admin/Folder/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    List.Resource();    //다국어처리
}
List.Resource = function () {
    $("#r_authname").text(R_AuthName);
    $("#r_jtNoData").text(R_JtNoData);
}


/**************************************************
onNodeSelect 트리 노드 선택
버튼처리 : folderAdd, folderMod, folderDel, folderSeq, folderSet
- 자사인경우만 리스트(divList) / 권한지정 버튼이 보임
- 시스템노드의 경우, 수정/삭제/권한지정 버튼 안보임.
***************************************************/
function onNodeSelect(node) {       //alert(node.ID + "," + node.Text + "," + typeInfo + "," + isSys);

    FolderID = node.ID;
    TypeInfo = node.GetProperty("TypeInfo");
    FolderPath = node.GetProperty("PathNameBase");

    var isSys = node.GetProperty("IsSys");

    if (TypeInfo == "2") {
        $("#divList").show(); $("#folderSet").show(); List.GetFolderAuth(); //폴더별 권한리스트 가져오기
    } else {
        $("#divList").hide(); $("#folderSet").hide();
    }

    if (isSys == "1") {
        $("#folderAdd").show(); $("#folderMod").hide(); $("#folderDel").hide(); $("#folderSeq").show();
    } else {
        $("#folderAdd").show(); $("#folderMod").show(); $("#folderDel").show(); $("#folderSeq").show();
    }
}

