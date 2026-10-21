FavoriteCopy = {};

/**************************************************
작성자   : famo23
작성일자 : 2011-12-21
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //적용
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            FavoriteCopy.Confirm();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        $("#folderName").val(R_FavoriteFolder);
        FavoriteTypes = "FC";
    });
});


/**************************************************
작성자   : famo23
작성일자 : 2011-12-21
작성목적 : 즐겨찾기 추가 (웹서비스 호출)
***************************************************/
FavoriteCopy.Confirm = function () {

    if (FavoriteTypes != "FC" || FavoriteTypes == "") {
        alert(R_SelectFavoriteFolder);
        return;
    }

    var url = "/DeskPlusEIP/WebHard/Ws/Content/FavoriteCopy.asmx/AddFavoriteFolder";

    ParamArray = {
        'parentFavoriteID': FavoriteID,
        'folderID': FolderID,
        'types': Types
    };

    var obj = eval("FavoriteCopy.SuccessConfirm");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-12-21
작성목적 : 즐겨찾기 추가 (웹서비스 값 받기)
***************************************************/
FavoriteCopy.SuccessConfirm = function (data, status) {
    if (data.d) {
        alert(R_CompleteConfirm);

        ParamArray = {
            'accActive': 4
        };

        CallEncrypt(CallGetParam());
        var goUrl = "/DeskPlusEIP/WebHard/Folder/Tree.aspx?p=" + GetEncryptValue;

        if (parent.disablePopup) { parent.parent.frmLeft.location.href = goUrl; parent.disablePopup(); } else { opener.parent.frmLeft.location.href = goUrl; self.close(); }
    }
    else {
        alert(R_Error);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-12-21
작성목적 : onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {

    FavoriteTypes = node.GetProperty("Type"); //즐겨찾기,폴더 구분자
    $("#folderName").val("");

    if (FavoriteTypes == "FC") {
        FavoriteID = node.ID;

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
        FavoriteID = 0;
        alert(R_SelectFavoriteFolder);
    }
}


/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}