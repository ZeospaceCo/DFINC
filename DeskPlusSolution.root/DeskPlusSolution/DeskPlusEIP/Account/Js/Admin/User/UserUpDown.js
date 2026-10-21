ModTabOrder = {};


/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); parent.List.Init(""); } else { opener.List.Init(""); self.close(); }
        });


        $("#ulWidgets").sortable();

        $("#cbtnConfirm").click(function (e) {

            e.preventDefault();
            var items = $("#ulWidgets li");
            var sortCatList = "";

            for (var i = 0; i < items.length; i++) {
                sortCatList += items[i].id;
                sortCatList += ",";
            }

            //alert(sortCatList);

            ParamArray = {                
                'sortList': sortCatList
            };


            var url = "/DeskPlusEIP/Account/Ws/Admin/User/UserUpDown.asmx/ModOrder";
            var obj = eval("ModTabOrder.SuccessModOrder");
            CallEncrypt(CallGetParam());
            data = "{p:'" + GetEncryptValue + "'}";

            AjaxService(url, data, obj);
        });

    });
});


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 
**********************************************************************************************/
ModTabOrder.SuccessModOrder = function (data, status) {

    if (data.d == true) {
        alert(R_ChangeCategoryOrder);

        parent.List.Init("");
        parent.disablePopup();
    }
}
