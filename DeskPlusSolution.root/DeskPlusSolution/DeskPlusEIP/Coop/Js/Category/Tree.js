Tree = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () {
                $("#accordion").accordion("resize");
            }
        });

        Tree.Init();
    });
});

/**************************************************
초기화
***************************************************/
Tree.Init = function () {
    //1이면 우측 보기페이지에서 갱신처리한것
    if (ReLoad == "") {
        var listType = $("#hdnCabinetID").val();
        if (listType != "") {
            var listTypeStr = listType.split(",");
            Tree.GoList(listTypeStr[1], "0");
        }
    }
}

/**************************************************
페이지 이동하기
***************************************************/
Tree.GoList = function (targetPage, itemID) {
    var targetPage = "/DeskPlusEIP/Coop/" + targetPage;

    if (targetPage != "") {
        ParamArray = {
            'listType': targetPage,
            'itemID': itemID
        };
        CallEncrypt(CallGetParam());

        var url = targetPage + "?p=" + GetEncryptValue;
        parent.frmContent.location = url;
    }
}

/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {
    var targetPage = node.GetProperty("TargetPage");
    var itemID = node.GetProperty("ItemID");

    if (targetPage != "" && targetPage != undefined) {
        Tree.GoList(targetPage, itemID);
    }
}

/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}