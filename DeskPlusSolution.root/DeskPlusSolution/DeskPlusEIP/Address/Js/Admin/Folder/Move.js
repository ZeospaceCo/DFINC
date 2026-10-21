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

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CategoryID,
        'toCategoryID': FolderID
    };
    

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Address/Ws/Admin/Folder/Move.asmx/MoveCommonCategory";
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Move.SuccessMove");

    AjaxService(url, data, obj);
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
//        parent.Add.GoTree();

        if (parent.disablePopup) { parent.disablePopup(); parent.Add.GoTree(); } else { opener.Add.GoTree(); self.close(); }
    }
    else {
        alert(R_AfterProcessingError);
    }
}


/**************************************************
추가, 수정 후 트리 새로고침
***************************************************/
Move.GoTree = function () {
    parent.frmFolderLeft.location.href = "/DeskPlusEIP/Address/Admin/Folder/Tree.aspx";
}

/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {

    Move.SelectedNodeID = node.ID; //고유키

    FolderID = node.GetProperty("CategoryID");
    $("#folderName").val(node.Text);
}


/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}


