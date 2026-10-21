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
    if (SelCatID == "") {
        alert(R_MoveCategory);
        return;
    }

    var url = "/DeskPlusEIP/Board/Ws/Admin/Content/Move.asmx/MoveList";

    ParamArray = {
        'toCateogryID': SelCatID,
        'listID': ListID
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
    Move.Control = UCBoardTreeScript_tvBoardTreeScript;
    Move.InitNodeID = 0;
}

/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {
    $("#folderName").val("");
    SelCatID = "";
    OptionUseView = node.GetProperty("OptionUseView");

    if (OptionUseView == "1") {
        return;
    }

    Move.SelectedNodeID = node.ID;

    $("#folderName").val("");
    if (node.ID > 0) {
        SelCatID = node.ID;
        $("#folderName").val(node.Text);
    }
    else {
        SelCatID = "";
    }
}

/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}