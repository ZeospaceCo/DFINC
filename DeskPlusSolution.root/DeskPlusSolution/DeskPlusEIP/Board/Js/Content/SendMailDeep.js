SendMailDeep = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        SendMailDeep.SendMail();


    });
});

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 편지쓰기
***************************************************/
SendMailDeep.SendMail = function () {

    var objPopupMail = window.open("", "WriteMail", "width=1100px,height=700px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");

    document.getElementById("frmMail").action = URL_GateUrl;
    document.getElementById("frmMail").submit();
}