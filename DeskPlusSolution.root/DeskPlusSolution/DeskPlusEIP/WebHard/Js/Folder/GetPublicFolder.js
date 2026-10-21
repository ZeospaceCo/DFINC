GetPublicFolder = {}
GetPublicFolder.ContentFrame = parent.parent.frmContent;
GetPublicFolder.TreeFrame = parent.parent.frmLeft;

/**************************************************
작성자   : famo23
작성일자 : 2011-11-08
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            GetPublicFolder.Confirm();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        GetPublicFolder.Control = tvFolderTree;
        RealFolderID = GetPublicFolder.Control.SelectedNode.ID;
        UID = UserID;

        $("#folderName").val(PublicFolder);
    });
});

/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}

/**************************************************
작성자 : famo23
작성일자 : 2011-11-08
작성목적 : onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {
    RealFolderID = node.ID;
    UID = node.GetProperty("UserID");

    $("#folderName").val("");
    if (FolderID != 0) {
        var tmpTitle = node.Text;

        tmpTitle = tmpTitle.split("&amp;").join("&");
        tmpTitle = tmpTitle.split("&quot;").join("\"");
        tmpTitle = tmpTitle.split("&#35;").join("#");
        tmpTitle = tmpTitle.split("&#39;").join("'");
        tmpTitle = tmpTitle.split("&#40;").join("(");
        tmpTitle = tmpTitle.split("&#41;").join(")");
        tmpTitle = tmpTitle.split("&lt;").join("<");
        tmpTitle = tmpTitle.split("&gt;").join(">");

        $("#folderName").val(tmpTitle);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-11-08
작성목적 : 확인
***************************************************/
GetPublicFolder.Confirm = function () {
    if (Gubun == "MOVE") {
        //이동
        if (RealFolderID == FolderID) {
            alert(R_CompareFolder);
            return;
        }
    }

    ParamArray = {
        'folderID': FolderID,
        'realFolderID': RealFolderID,
        'fileList': FileList,
        'uID': UID,
        'gubun': Gubun,
        'listType': ListType,
        'order': Order,
        'types': Types

    };

    CallEncrypt(CallGetParam());
    location.href = "CopyMove.aspx?p=" + GetEncryptValue;
}