ItemSubUserUpdown = {}

$(function () {
    $(document).ready(function () {
        $("#ulWidgets").sortable();

        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        $("#cbtnConfirm").click(function (e) {
            e.preventDefault();

            var itemSubUser = $("#ulWidgets li");

            var sortSubUserList = "";

            for (var i = 0; i < itemSubUser.length; i++) {
                sortSubUserList += itemSubUser[i].id;
                sortSubUserList += ",";
            }

            //alert(sortItemList);

            ParamArray = {
                'sortSubUserList': sortSubUserList
            };
            //alert(sortItemList);
            var url = "/DeskPlusEIP/Task/Ws/Admin/Menu/ItemSubUserUpdown.asmx/ItemSubUserOrder";
            var obj = eval("ItemSubUserUpdown.SuccessModOrder");
            CallEncrypt(CallGetParam());
            data = "{p:'" + GetEncryptValue + "'}";

            AjaxService(url, data, obj);
        });

    })
})


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2014-04-21
작성목적 : 순서 변경 성공시
**********************************************************************************************/
ItemSubUserUpdown.SuccessModOrder = function (data, status) {
    if (data.d == true) {
        alert(R_CompleteConfirm);

        if (parent.disablePopup) { parent.disablePopup(); parent.List.GetPageList(); parent.List.Resource(); } else { opener.List.GetPageList(); parent.List.Resource(); self.close(); }
    }
}