MoveSubUpDown = {}

$(function () {
    $(document).ready(function () {
        $("#ulWidgets").sortable();

        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });


        $("#cbtnConfirm").click(function (e) {
            e.preventDefault();

            var itemSub = $("#ulWidgets li");

            var sortItemSubList = "";

            for (var i = 0; i < itemSub.length; i++) {
                sortItemSubList += itemSub[i].id;
                sortItemSubList += ",";
            }

            //alert(sortItemList);

            ParamArray = {
                'sortItemSubList': sortItemSubList
            };
            //alert(sortItemList);
            var url = "/DeskPlusEIP/Task/Ws/Admin/Menu/MoveSubUpdown.asmx/ItemSubOrder";
            var obj = eval("MoveSubUpDown.SuccessItemOrder");
            CallEncrypt(CallGetParam());
            data = "{p:'" + GetEncryptValue + "'}";

            AjaxService(url, data, obj);

        });

    });
});



/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-30
작성목적 : 소분류 순서변경
**********************************************************************************************/
MoveSubUpDown.SuccessItemOrder = function (data, status) {
    if (data.d == true) {
        
        alert(R_CompleteConfirm);

        if (parent.disablePopup) { parent.disablePopup(); parent.List.GetPageSubList(ItemID); parent.List.Resource(); } else { opener.List.GetPageSubList(ItemID); parent.List.Resource(); self.close(); }
    }
}
