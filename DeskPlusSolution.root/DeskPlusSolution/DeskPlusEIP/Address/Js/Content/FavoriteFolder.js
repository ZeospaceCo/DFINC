FavoriteFolder = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            FavoriteFolder.Confirm();
        });

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });
    });
});


/**************************************************
추가
***************************************************/
FavoriteFolder.Confirm = function () {

    if (FolderID == 0) {
        alert(R_SelectFolder);
    }
    else {
        var url = "/DeskPlusEIP/Address/Ws/Content/FavoriteFolder.asmx/AddFavoCategory";

        ParamArray = {
            'parentCategoryID': FolderID,
            'folderID': CategoryID,
            'types': Types
        };
        
        var obj = eval("FavoriteFolder.Add");
        CallEncrypt(CallGetParam());
        data = "{p:'" + GetEncryptValue + "'}";

        AjaxService(url, data, obj);
    }
}

/**************************************************
즐겨찾기 그룹에 추가
***************************************************/
FavoriteFolder.Add = function (data, status) {
    if (data.d) {
        alert(R_Complete);
//        parent.disablePopup();
//        parent.parent.frmLeft.location.href = "/DeskPlusEIP/Address/Folder/Tree.aspx";

        if (parent.disablePopup) { parent.disablePopup(); parent.parent.frmLeft.location.href = "/DeskPlusEIP/Address/Folder/Tree.aspx"; } else { opener.parent.frmLeft.location.href = "/DeskPlusEIP/Address/Folder/Tree.aspx"; self.close(); }
    }
    else {
        alert(R_ProcessingError);
    }

}

/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {

    FavoriteFolder.SelectedNodeID = node.ID; //고유키

    FolderCategoryID = node.GetProperty("CategoryID");
    FolderID = node.GetProperty("CategoryID");
    FolderType = node.GetProperty("Type");

    if (FolderType == "FG" && FolderCategoryID != "0") {
        var etc = node.GetProperty("Etc");
        var selVal = etc.split("|");
        FolderCategoryID = selVal[0];
        FolderType = selVal[1];
    }

    if (FolderType != "FG") FolderID = 0;

    $("#folderName").val("");
        
    //즐겨찾기 그룹만 가능함
    if (FolderType == "FG") {
        if (FolderID > 0) {
            $("#folderName").val(node.Text);
        }
    }
}


/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}