FavoriteMove = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            FavoriteMove.Confirm();
        });

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        $('#folderName').attr("disabled", true);
    });
});

/**************************************************
추가
***************************************************/
FavoriteMove.Confirm = function () {
    if (ParentFavoriteID > 0) {
        var url = "/DeskPlusEIP/Board/Ws/Content/FavoriteMove.asmx/MoveFavorite";

        ParamArray = {
            'favoriteID': FavoriteID,
            'parentFavoriteID': ParentFavoriteID
        };

        var obj = eval("FavoriteMove.SuccessConfirm");
        CallEncrypt(CallGetParam());
        data = "{p:'" + GetEncryptValue + "'}";

        AjaxService(url, data, obj);
    }
    else {
        alert(R_CheckMoveFavoriteFolder);
    }
}

/**************************************************
즐겨찾기 폴더에 추가
***************************************************/
FavoriteMove.SuccessConfirm = function (data, status) {
    if (data.d) {
        alert(R_Complete);
        //        parent.disablePopup();

        ParamArray = {
            'dcparm': Dcparm,
            'menuCatID': MenuCatID,
            'catID': CatID,
            'accActive': 2
        };

        CallEncrypt(CallGetParam());
        var goUrl = "/DeskPlusEIP/Board/Category/Tree.aspx?p=" + GetEncryptValue;

        //        parent.parent.frmLeft.location.href = goUrl;

        if (parent.disablePopup) { parent.parent.frmLeft.location.href = goUrl; parent.disablePopup(); } else { opener.parent.frmLeft.location.href = goUrl; self.close(); }
    }
    else {
        alert(R_ProcessingError);
    }
}

/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {
    ParentFavoriteID = 0;

    var types = node.GetProperty("Types");
    $("#folderName").val("");

    if (types == "FC") {
        ParentFavoriteID = node.GetProperty("FavoriteID");
        if (FavoriteID == ParentFavoriteID) {
            alert(R_FolerMoveSameFolder);
            ParentFavoriteID = 0;
        }
        else {
            $("#folderName").val(node.Text);
        }
    }
    else {
        alert(R_SelectFavoriteFolder);
    }
}

/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}