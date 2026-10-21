FavoriteOrder = {};

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
            var sortCatList = "";

            for (var i = 0; i < items.length; i++) {
                sortCatList += items[i].id;
                sortCatList += ",";
            }

            //alert(sortCatList);

            ParamArray = {
                'sortCatList': sortCatList
            };

            var url = "/DeskPlusEIP/Board/Ws/Content/FavoriteOrder.asmx/ModOrderFavorite";
            var obj = eval("FavoriteOrder.SuccessModOrder");
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
FavoriteOrder.SuccessModOrder = function (data, status) {
    if (data.d == true) {
        //alert("순서 변경되었습니다.");

        //        parent.disablePopup();
        //        parent.GoTreeFavor();

        if (parent.disablePopup) { parent.disablePopup(); parent.GoTreeFavor(); } else { opener.GoTreeFavor(); self.close(); }
    }
}