Add = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Add.Confirm();
        });


        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });


        $('#ctbTabName').focus();
    });
});


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 확인
**********************************************************************************************/
Add.Confirm = function () {

}
