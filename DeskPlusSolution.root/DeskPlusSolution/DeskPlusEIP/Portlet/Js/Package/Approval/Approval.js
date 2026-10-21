
$(document).ready(function () {

});


/**************************************************
작성자: freeman
작성일:2012-02-07
작성목적 : 포틀릿 Popup (33버전)
***************************************************/
function Approval_Popup(documentID, approvalState, approvalTag, sequences, pageType) {
    var PagePath = "/DeskPlusApp";
    /*
    //재결
    if (approvalTag.indexOf("ZR") >= 0) {
        url = PagePath + "/Approval/FormEdit/FormEditZR.aspx";
        url += "?dID=" + documentID;
        url += "&dTargetUserID=" + UserID;
        url += "&dTag=" + approvalTag;
        url += "&dState=" + approvalState;
        url += "&sequence=" + sequences;
        url += "&approvalTag=" + approvalTag;
        url += "&IsPopup=1";
        url += "&ranNum=Y";
        url += "&cpDeptID=" + cpDeptID;
        url += "&cpDeptName=" + escape(cpDeptName);
    }
    else if (approvalTag.indexOf("ZP") >= 0) {
        //수신문서
        url = PagePath + "/Approval/FormEdit/FormEditZP.aspx";
        url += "?dID=" + documentID;
        url += "&sequence=" + sequences;
        url += "&dNum2=" + dNum2;
        url += "&accountTag=" + accountTag;
        url += "&IsPopup=1";
        url += "&isApproveNetYN=N";
        url += "&ranNum=Y";
        url += "&cpDeptID=" + cpDeptID;
        url += "&cpDeptName=" + escape(cpDeptName);
    }
    else {
        if (accountTag.indexOf("ZR") >= 0) {
            url = PagePath + "/Approval/Approve/ApproveZR.aspx";
            url += "?dID=" + documentID;
            url += "&cID=1";
            url += "&dTargetUserID=" + UserID;
            url += "&dTag=" + approvalTag;
            url += "&dState=" + approvalState;
            url += "&sequence=" + sequences;
            url += "&ranNum=Y";
            url += "&accountTag=" + accountTag;
        }
        else if (accountTag.indexOf("ZP") >= 0) {
            url = PagePath + "/Approval/Approve/Approve.aspx";
            url += "?dID=" + documentID;
            url += "&cID=1";
            url += "&dTargetUserID=" + UserID;
            url += "&dNum2=" + dNum2;
            url += "&dTag=" + approvalTag;
            url += "&dState=" + approvalState;
            url += "&sequence=" + sequences;
            url += "&ranNum=Y";
            url += "&accountTag=" + accountTag;
        }
        else {
            if (pageType == "A") {
                //결재대상문서
                url = PagePath + "/Approval/Approve/Approve.aspx";
            } else {
                //상신한 문서
                url = PagePath + "/Approval/View/ViewDocument.aspx";
            }
            url += "?dID=" + documentID;
            url += "&dTargetUserID=" + UserID;
            url += "&dTag=" + approvalTag;
            url += "&dState=" + approvalState;
            url += "&sequence=" + sequences;
            url += "&ranNum=Y";
        }       
    }
    */

    if (pageType == "A") {
        //결재대상문서
        url = PagePath + "/Approval/Approve/Approve.aspx";
    } else {
        //상신한 문서
        url = PagePath + "/Approval/View/ViewDocument.aspx";
    }
    url += "?dID=" + documentID;
    url += "&dTargetUserID=" + UserID;
    url += "&dTag=" + approvalTag;
    url += "&dState=" + approvalState;
    url += "&sequence=" + sequences;
    url += "&ranNum=Y";


    var title = "approvalWin";
    //var width = "1024";
    //var height = "750";
    //var left = Math.ceil( (window.screen.availWidth  - width) / 2 );
    //var top = Math.ceil( (window.screen.availHeight - height) / 2 );

    //alert(url);
    //window.open(url, title, "toolbar=no,location=no,status=no,menubar=no,scrollbars=auto,resizable=yes, width=1024,height=768,resizable=yes,channelmode=yes");
    PopupWinOpen(url, '', title, 1024, 700)
}