UserTree = {};
UserTree.TreeFrame = parent.frmLeft;
UserTree.ContentFrame = parent.frmContent;
UserTree.Control = null;

/**************************************************
작성자 : 이지원
작성일자 : 2018-01-10
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        UserTree.Init();
    });
});

/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}

/**************************************************
작성자 : 이지원
작성일자 : 2018-01-10
작성목적 : 초기화
***************************************************/
UserTree.Init = function () {
    ParamArray = {
        'deptID': "0",
        'accessAuth': AccessAuth
    };

    CallEncrypt(CallGetParam());
    parent.frmFolderContent.location.href = "/DeskPlusEIP/ETC/Attendance/UserList.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자 : 이지원
작성일자 : 2018-01-10
작성목적 : onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {

    var selID = node.ID;

    ParamArray = {
        'deptID': selID,
        'accessAuth': AccessAuth
    };

    CallEncrypt(CallGetParam());
    parent.frmFolderContent.location.href = "/DeskPlusEIP/ETC/Attendance/UserList.aspx?p=" + GetEncryptValue;
}