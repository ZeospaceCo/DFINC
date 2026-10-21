FavoriteOrder = {};


/**************************************************
작성자   : famo23
작성일자 : 2011-12-27
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });


        $("#ulWidgets").sortable();

        $("#cbtnConfirm").click(function (e) {
            e.preventDefault();
            FavoriteOrder.Confirm();
          
        });
    });
});


/**************************************************
작성자   : famo23
작성일자 : 2011-12-27
작성목적 : 하위 순서변경 (웹서비스 호출)
***************************************************/
FavoriteOrder.Confirm = function () {

    var items = $("#ulWidgets li");
    var sortFolList = "";

    if (items.length == 0) {
        alert(R_MoveNoSubFolder);
        return;
    }

    for (var i = 0; i < items.length; i++) {
        sortFolList += items[i].id;
        sortFolList += ",";
    }


    ParamArray = {
        'sortFolList': sortFolList
    };

    var url = "/DeskPlusEIP/WebHard/Ws/Content/FavoriteOrder.asmx/ModOrderFavorite";
    var obj = eval("FavoriteOrder.SuccessModOrder");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-27
작성목적 : 하위 순서변경 (웹서비스 값 받기)
**********************************************************************************************/
FavoriteOrder.SuccessModOrder = function (data, status) {
    if (data.d) {
        alert(R_OrderFolder);

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
