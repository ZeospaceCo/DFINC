GetFolder = {}


$(function () {
    $(document).ready(function () {

        //[확인]
        $('#CbtnConfirm').click(function (e) { e.preventDefault(); GetFolder.Confirm(); });

        //[닫기]
        $('#CbtnClose').click(function (e) { e.preventDefault(); GetFolder.Close(); });
    });
});

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 닫기
***************************************************/
GetFolder.Close = function () {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 확인 
***************************************************/
GetFolder.Confirm = function () {

    if (FolderID == 0) { alert(R_Msg_SelFolder); return; }

    if (parent.$("#tbFolderID") && parent.$("#ctbFolderName")) {
        parent.$("#tbFolderID").val(FolderID);
        parent.$("#ctbFolderName").val(FolderName);
    }

    GetFolder.Close();
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 폴더선택
***************************************************/
FolderTree_onNodeSelect = function (node) {//alert(node.ID + "," + node.Text + "," + typeInfo + "," + isSys);
    FolderID = node.ID;
    FolderName = node.Text;
}

