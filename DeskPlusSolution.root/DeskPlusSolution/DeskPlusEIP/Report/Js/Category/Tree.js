Tree = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () {
                $("#accordion").accordion("resize");
            }
        });

        //Tree.Init();
    });
});

/**************************************************
초기화
***************************************************/
Tree.Init = function () {
    //1이면 우측 보기페이지에서 갱신처리한것
    if (ReLoad == "") {
        var listType = $("#hdnCabinetID").val();
        if (listType != "") {
            Tree.GoList(listType);
        }
    }
}

/**************************************************
페이지 이동하기
***************************************************/
Tree.GoList = function (listType) {
    var targetPage = "";

    switch (listType) {
        case "PrivateList":
            targetPage = "/DeskPlusEIP/Report/Content/PrivateList.aspx";
            break;
        case "InformList":
            targetPage = "/DeskPlusEIP/Report/Content/InformList.aspx";
            break;
        case "DeptList":
            targetPage = "/DeskPlusEIP/Report/Content/DeptList.aspx";
            break;
        case "ConfirmList":
            targetPage = "/DeskPlusEIP/Report/Content/ConfirmList.aspx";
            break;
        case "ShareList":
            targetPage = "/DeskPlusEIP/Report/Content/ShareList.aspx";
            break;
        case "AuthList":
            targetPage = "/DeskPlusEIP/Report/Content/AuthList.aspx";
            break;
        case "AdminFolder":
            targetPage = "/DeskPlusEIP/Report/Content/AdminList.aspx";
            break;
        case "AdminList":
            targetPage = "/DeskPlusEIP/Report/Content/AdminList.aspx";
            break;
        case "FolderList":
            targetPage = "/DeskPlusEIP/Report/Content/FolderList.aspx";
            break;
    }

    if (targetPage != "") {
        ParamArray = {
            'listType': listType
        };

        CallEncrypt(CallGetParam());
        parent.frmContent.location.href = targetPage + "?p=" + GetEncryptValue;
    }
}

/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {
    var targetPage = node.GetProperty("TargetPage");

    if (targetPage != "" && targetPage != undefined) {
        Tree.GoList(targetPage);
    }
}

/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}