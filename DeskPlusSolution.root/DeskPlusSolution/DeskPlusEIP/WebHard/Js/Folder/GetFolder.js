GetFolder = {}
GetFolder.ContentFrame = parent.parent.frmContent;
GetFolder.TreeFrame = parent.parent.frmLeft;

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
            GetFolder.Confirm();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        GetFolder.Control = tvFolderTree;
        RealFolderID = GetFolder.Control.SelectedNode.ID;
        UID = UserID;

        $("#folderName").val(R_PrivateFolderName);
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
GetFolder.Confirm = function () {
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
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "CopyMove.aspx?p=" + GetEncryptValue;

    /*
    $("#zeoModalBG").css({ "width": "100%", "height": "100%" });
    $("#zeoModalBG").css({ "opacity": "0.7" });
    $("#zeoModalBG").show();

    if (Gubun == "MOVE") {
    //이동

    if (RealFolderID == FolderID) {
    $("#zeoModalBG").hide();
    alert(R_CompareFolder);
    return;
    }

    ParamArray = {
    'folderID': FolderID,
    'realFolderID': RealFolderID,
    'tmpValue': TmpValue,
    'uID': UID
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("GetFolder.SuccessMoveCopy");
    var url = "/DeskPlusEIP/WebHard/Ws/Folder/GetFolder.asmx/MoveFile";

    AjaxService(url, data, obj);
    }
    else if (Gubun == "COPY") {
    //복사

    ParamArray = {
    'folderID': FolderID,
    'realFolderID': RealFolderID,
    'tmpValue': TmpValue,
    'uID': UID
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("GetFolder.SuccessMoveCopy");
    var url = "/DeskPlusEIP/WebHard/Ws/Folder/GetFolder.asmx/CopyFile";

    AjaxService(url, data, obj);
    }
    */
}