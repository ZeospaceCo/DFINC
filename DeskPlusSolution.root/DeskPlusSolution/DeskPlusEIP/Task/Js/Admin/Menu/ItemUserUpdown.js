ItemUserUpdown = {}

$(function () {
    $(document).ready(function () { })
    $("#ulWidgets").sortable();

    $('#cbtnClose').click(function (e) {
        e.preventDefault();
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    });

    $("#cbtnConfirm").click(function (e) {
        e.preventDefault();

        var items = $("#ulWidgets li");

        var sortItemList = "";

        for (var i = 0; i < items.length; i++) {
            sortItemList += items[i].id;
            sortItemList += ",";
        }

        //alert(sortItemList);

        ParamArray = {
            'sortItemList': sortItemList
        };
        //alert(sortItemList);
        var url = "/DeskPlusEIP/Task/Ws/Admin/Menu/ItemUserUpdown.asmx/ItemUserOrder";
        var obj = eval("ItemUserUpdown.SuccessModOrder");
        CallEncrypt(CallGetParam());
        data = "{p:'" + GetEncryptValue + "'}";

        AjaxService(url, data, obj);
    });
});

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2014-04-21
작성목적 : 순서 변경 성공시
**********************************************************************************************/
ItemUserUpdown.SuccessModOrder = function (data, status) {
    if (data.d == true) {
        alert(R_CompleteConfirm);

        if (parent.disablePopup) { parent.disablePopup(); parent.List.GetPageList(); parent.List.Resource(); } else { opener.List.GetPageList(); parent.List.Resource(); self.close(); }
    }
}