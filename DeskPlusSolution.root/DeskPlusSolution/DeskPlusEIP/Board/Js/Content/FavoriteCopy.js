FavoriteCopy = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            FavoriteCopy.Confirm();
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
FavoriteCopy.Confirm = function () {
    if (Types == "BC" || Types == "") {
        alert(R_SelectFavoriteFolder);
        return;
    }

    var url = "/DeskPlusEIP/Board/Ws/Content/FavoriteCopy.asmx/AddFavoriteCategory";

    ParamArray = {
        'parentFavoriteID': FavoriteID,
        'categoryID': CatID
    };

    var obj = eval("FavoriteCopy.SuccessConfirm");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**************************************************
즐겨찾기 폴더에 추가
***************************************************/
FavoriteCopy.SuccessConfirm = function (data, status) {
    if (data.d) {
        alert(R_Complete);

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
    Types = node.GetProperty("Types"); //즐겨찾기,카테고리 구분자

    FavoriteCopy.SelectedNodeID = node.ID; //고유키
    $("#folderName").val("");

    if (Types == "BC") {
        FavoriteID = 0; //즐겨찾기 루트는 -2 임.
        alert(R_SelectFavoriteFolder);
        return;
    }
    else {
        FavoriteID = node.GetProperty("FavoriteID");
        $("#folderName").val(node.Text);
    }
}

/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}