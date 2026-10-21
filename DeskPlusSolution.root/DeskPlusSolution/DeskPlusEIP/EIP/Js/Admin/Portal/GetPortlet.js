
GetPortlet = {}

$(function () {
    $(document).ready(function () {

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            GetPortlet.Confirm();
            parent.Main.ModalHide();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        GetPortlet.Resource();
    });
});

/**************************************************
작성자:
작성일:2011-11-14
작성목적 : 
***************************************************/
GetPortlet.Confirm = function () {


}


GetPortlet.Resource = function () {

    if ($(".r_notice").text() != undefined) {
        $(".r_notice").text(R_Notice);
    }

    if ($(".r_common").text() != undefined) {
        $(".r_common").text(R_Common);
    }
}
