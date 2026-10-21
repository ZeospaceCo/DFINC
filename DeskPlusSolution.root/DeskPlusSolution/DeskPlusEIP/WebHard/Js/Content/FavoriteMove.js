FavoriteMove = {};

/**************************************************
작성자   : famo23
작성일자 : 2011-12-27
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //적용
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            FavoriteMove.Confirm();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        $("#folderName").val(R_FavoriteFolder);
        Types = "FC";
        TmpFavoriteID = -5;
        TmpParentFavoriteID = 0;
    });
});


/**************************************************
작성자   : famo23
작성일자 : 2011-12-27
작성목적 : 적용 (웹서비스 호출)
***************************************************/
FavoriteMove.Confirm = function () {

    if (Types != "FC" || Types == "") {
        alert(R_SelectFavoriteFolderMove);
        return;
    }

    if (TmpFavoriteID == FavoriteID) {
        alert(R_MoveSameFavorite);
        return;
    }
    
    if (TmpFavoriteID == ParentFavoriteID) {
        alert(R_MoveLocationFavorite);
        return;
    }

    if (ChildCnt > 0) {
        alert(R_MoveSubFavorite);
        return;
    }

    var url = "/DeskPlusEIP/WebHard/Ws/Content/FavoriteMove.asmx/MoveFavorite";

    ParamArray = {
        'favoriteID': FavoriteID,
        'parentFavoriteID': TmpFavoriteID
    };

    var obj = eval("FavoriteMove.SuccessConfirm");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);

}

/**************************************************
작성자   : famo23
작성일자 : 2011-12-27
작성목적 : 즐겨찾기 추가 (웹서비스 값 받기)
***************************************************/
FavoriteMove.SuccessConfirm = function (data, status) {
    if (data.d) {
        alert(R_CompleteConfirm);

        ParamArray = {
            'accActive': 3
        };

        CallEncrypt(CallGetParam());
        var goUrl = "/DeskPlusEIP/WebHard/Folder/Tree.aspx?p=" + GetEncryptValue;

        if (parent.disablePopup) { parent.parent.frmLeft.location.href = goUrl; parent.disablePopup(); } else { opener.parent.frmLeft.location.href = goUrl; self.close(); }
    }
    else {
        alert(R_ProcessingError);
    }

}

/**************************************************
작성자   : famo23
작성일자 : 2011-12-27
작성목적 : onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {

    Types = node.GetProperty("Type");

    $("#folderName").val("");

    if (Types == "FC") {
        TmpFavoriteID = node.ID;

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
    else {
        TmpFavoriteID = 0;
        alert(R_SelectFavoriteFolder);
    }
}


/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}