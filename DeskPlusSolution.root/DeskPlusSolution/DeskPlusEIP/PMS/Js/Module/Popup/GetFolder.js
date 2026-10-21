GetFolder = {}

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            GetFolder.Confirm();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });
    });
});

/**********************************************************************************************
[폴더 저장]
**********************************************************************************************/
GetFolder.Confirm = function () {
    //    var of = parent.document.forms[0];
    var folderID = $("#hdnFolderID").val();
    var folderName = $("#hdnFolderName").val();

    if (folderID != "") {
        folderName = escape(folderName);
        parent.SelectedFolder(folderID, folderName);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        //window.close();
    }
    else {
        alert(R_Project_MSGFolder);
    }
}

/**********************************************************************************************
[폴더 선택]
**********************************************************************************************/
function FolderScript_OnNodeSelect(node) {
    $("#hdnFolderID").val("");
    $("#hdnFolderName").val("");

    if (node.ID != "-1") {
        $("#hdnFolderID").val(node.ID);
        $("#hdnFolderName").val(node.Text);
    }
    else {
        alert(R_Project_MSGFolder);
    }
}