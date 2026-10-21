Tree = {};

Tree.Control = null;
Tree.InitNodeID = 0;
Tree.ContentFrame = null;

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //트리 초기화
        Tree.Init();

        //확인
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            Tree.Confirm();
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
작성목적 : 확인하기
***************************************************/
Tree.Confirm = function () {

    //return;
    if (SelCatID == "") {
        alert(R_MoveCategory);
        return;
    }

    if (HomeCategory == 1) {
        //parent.document.all.spanCateID.innerText = CategoryName;
        parent.document.all.hiddenCateID.value = SelCatID;

        ParamArray = {
            'homeCategory': "1",
            'homeCategoryID': SelCatID
        };

        CallEncrypt(CallGetParam());
        parent.location.href = "/DeskPlusEIP/Board/Content/AddN.aspx?p=" + GetEncryptValue;       

    }
    else {
        parent.GetSelBoardCategory(SelCatID);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }

}

/**************************************************
초기화
***************************************************/
Tree.Init = function () {
    Tree.ContentFrame = parent.frmContent;
    Tree.Control = UCBoardTreeScript_tvBoardTreeScript;
    Tree.InitNodeID = 0;
}

/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {
    Tree.SelectedNodeID = node.ID;

    if (node.ID > 0) {
        SelCatID = node.ID;
        CategoryName = node.Text;
    }
    else {
        SelCatID == "";
    }

    $("#folderName").val(node.Text);
}

/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}