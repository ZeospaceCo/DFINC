MoveCopy = {};


/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            MoveCopy.Confirm();
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
MoveCopy.Confirm = function () {

    if (FolderID == 0) {
        alert(R_SelectFolder);
    }
    else {

        if (Gubun == "COPY") {
            MoveCopy.Copy();
        }
        else if (Gubun == "MOVE") {
            MoveCopy.Move();
        }
    }

}

/**************************************************
복사
***************************************************/
MoveCopy.Copy = function () {

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID,
        'types': Types,
        'toCategoryID': FolderID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Address/Ws/Content/MoveCopy.asmx/CopyPrivate";
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("MoveCopy.SuccessCopy");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , google 주소록 전체 가져오기
**********************************************************************************************/
MoveCopy.SuccessCopy = function (data, status) {
    if (data.d == true) {
        if (Gubun == "MOVE") {
            alert(R_CompleteSelectMoveAddress);
        }
        else if (Gubun == "COPY") {
            alert(R_CompleteSelectCopyAddress);
        }

        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else {
        alert(R_AfterProcessingError);
    }
}


/**************************************************
복사
***************************************************/
MoveCopy.Move = function () {
    

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID,
        'toCategoryID': FolderID
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Address/Ws/Content/MoveCopy.asmx/MovePrivate";
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("MoveCopy.SuccessMove");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , google 주소록 전체 가져오기
**********************************************************************************************/
MoveCopy.SuccessMove = function (data, status) {
    if (data.d == true) {
        alert(R_CompleteSelectMoveAddress);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else {
        alert(R_AfterProcessingError);
    }
}

/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {

    MoveCopy.SelectedNodeID = node.ID; //고유키

    FolderID = node.GetProperty("CategoryID");
    $("#folderName").val("");
    if(FolderID != 0)
    {
        $("#folderName").val(node.Text);
    }
}


/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}