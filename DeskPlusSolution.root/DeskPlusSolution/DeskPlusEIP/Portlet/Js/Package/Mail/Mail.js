
$(document).ready(function () {

});


/**************************************************
작성자: shimpark
작성일:2012-02-07
작성목적 : 메일 탭메인의 포틀릿 Popup
***************************************************/
function Mail_Popup(obj, mail_sn) {
    //var url = MailServerIP + "/" + ZettaBaseFolder + "/login.nvd";
    //url += "?user_account=" + Mail;
    //url += "&cmd=mail_detail_popup";
    //url += "&mail_sn=" + mail_sn;
    //url += "&mailbox_code=1100";

    //var title = "mailWin";

    //<%# MailServerIP %>/<%# ZettaBaseFolder%>/login.nvd?user_account=<%# Mail%>&cmd=mail_detail_popup&mail_sn=<%# DataBinder.Eval(Container.DataItem,"mail_sn").ToString()%>&mailbox_code=1100
    //window.open(url, title, "toolbar=no,location=no,status=no,menubar=no,scrollbars=auto,resizable=yes, width=1024,height=768,resizable=yes,channelmode=yes");
    //PopupWinOpen(url, '', title, 1024, 700)

    var width = "1024";
    var height = "768";
    var left = Math.ceil((window.screen.availWidth - width) / 2);
    var top = Math.ceil((window.screen.availHeight - height) / 2);
    window.open('', 'mailWin', "history=no,resizable=1,status=no,scrollbars=yes,menuebar=no,width=" + width + ",height=" + height + ",top=" + top + ",left=" + left);
    //document.location.reload();
    $(obj).find('font').attr('style', '');
}



/**************************************************
작성자: shimpark
작성일:2012-02-07
작성목적 : 메일 탭메인의 승인 포틀릿 Popup
***************************************************/


function Mail_ConfirmPopup(p_Request_mail_sn, p_Confirm_gubun, p_Request_user_sn, p_Confirm_user_sn, p_Confirm_sn) {

    var thisForm = document.frmMail;

    //user_account=deskplus@cpt.co.kr&cmd=confirm_view_portlet&mail_sn=617&confirm_gubun=0&confirm_sn=19&request_user_sn=2&confirm_user_sn=0												
    thisForm.user_account.value = Mail;
    thisForm.cmd.value = "confirm_view_portlet";
    thisForm.mail_sn.value = p_Request_mail_sn;
    thisForm.confirm_gubun.value = p_Confirm_gubun;
    thisForm.confirm_sn.value = p_Confirm_sn;
    thisForm.request_user_sn.value = p_Request_user_sn;
    thisForm.confirm_user_sn.value = p_Confirm_user_sn;
    var winObj = window.open('about:blank', 'MailWin', 'status=no,scrollbars=yes,menuebar=no,resizable=yes, top=50, left=50,width=920, height=700');
    winObj.focus();

    thisForm.method = "post";
    thisForm.target = "MailWin";
    thisForm.action = MailHttp + "/zmail/login.nvd";
    thisForm.submit();
} 												

