FavorUserUpdown = {};

/**************************************************
초기화
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

            var url = "/DeskPlusEIP/Coop/Ws/Admin/Category/FavorUserUpdown.asmx/ModOrder";
            var obj = eval("FavorUserUpdown.SuccessModOrder");
            CallEncrypt(CallGetParam());
            data = "{p:'" + GetEncryptValue + "'}";

            AjaxService(url, data, obj);
        });
    });
});

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 주소록 카테고리 순서 성공시 페이지 표시
**********************************************************************************************/
FavorUserUpdown.SuccessModOrder = function (data, status) {
    if (data.d == true) {
        alert(R_ChangeOrders);

        if (parent.disablePopup) { parent.disablePopup(); parent.List.GetPageList(); parent.List.Resource(); } else { opener.List.GetPageList(); parent.List.Resource(); self.close(); }
    }
}