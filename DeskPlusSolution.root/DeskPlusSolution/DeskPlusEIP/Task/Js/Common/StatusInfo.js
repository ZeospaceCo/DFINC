StatusInfo = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-05-12
작성목적 : 초기화
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

