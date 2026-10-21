Move = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Move.Confirm();
        });

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });
    });
});

/**************************************************
확인
***************************************************/
Move.Confirm = function () {
    Move.Move();
}

/**************************************************
이동
***************************************************/
Move.Move = function () {
    if (CatID == FolderID) {
        alert(R_NoMove);
        return;
    }
    else {
        //파라미터값 배열에 담기
        ParamArray = {
            'categoryID': CatID,
            'toCategoryID': FolderID
        };

        CallEncrypt(CallGetParam());
        var url = "/DeskPlusEIP/Board/Ws/Admin/Category/Move.asmx/MoveCategory";
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Move.SuccessMove");

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , google 주소록 전체 가져오기
**********************************************************************************************/
Move.SuccessMove = function (data, status) {
    if (data.d == true) {
        alert(R_CompleteFolderMoved);
        //        parent.disablePopup();
        //        parent.Mod.GoTree();

        if (parent.disablePopup) { parent.disablePopup(); parent.Mod.GoTree(); } else { opener.Mod.GoTree(); self.close(); }
    }
    else {
        alert(R_AfterProcessingError);
    }
}

/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {
    Move.SelectedNodeID = node.ID; //고유키

    FolderID = node.ID;
    $("#folderName").val(node.Text);
}

/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}