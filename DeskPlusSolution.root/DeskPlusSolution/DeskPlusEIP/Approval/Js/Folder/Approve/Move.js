Move = {};

Move.Control = null;
Move.InitNodeID = 0;
Move.ContentFrame = null;


/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //트리 초기화
        Move.Init();

        //확인
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            Move.Confirm();
        });

        //닫기
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

    });
});


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 추가
***************************************************/
Move.Confirm = function () {
    if (SelFolderID == "") {
        alert(R_MoveCategory);
        return;
    }

    var url = "/DeskPlusEIP/Approval/Ws/Folder/Approve/Move.asmx/MoveList";

    ParamArray = {
        'toFolderID': SelFolderID,
        'appDocID': AppDocID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Move.SuccessConfirm");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-18
작성목적 : 추가 성공시 
**********************************************************************************************/
Move.SuccessConfirm = function (data, status) {
    var response = data.d;
    if (response) {
        alert(R_Processing);
        if (parent.disablePopup) { parent.disablePopup(); parent.List.Init(); } else { self.close(); }
    }
}


/**************************************************
초기화
***************************************************/
Move.Init = function () {
    Move.ContentFrame = parent.frmContent;
    Move.Control = UCAppFolderSplitTreeScript1_tvFolder;
    Move.InitNodeID = 0;
}

/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {
    Move.SelectedNodeID = node.ID;
    
    if (node.ID != "Private_Folder") {
        SelFolderID = node.ID;
        $("#folderName").val(node.Text);
    }
    else {
        SelFolderID == "";
        $("#folderName").val("");
    }
}


/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}