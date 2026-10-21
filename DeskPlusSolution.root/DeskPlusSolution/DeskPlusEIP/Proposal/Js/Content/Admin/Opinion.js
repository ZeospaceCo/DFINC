Opinion = {}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 
**********************************************************************************************/
$(function () {

    $(document).ready(function () {

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

    });
});