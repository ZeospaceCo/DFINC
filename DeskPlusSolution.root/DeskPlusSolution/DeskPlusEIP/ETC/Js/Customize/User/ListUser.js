var ListUser = {};

$(function () {


});


// 결재문서 보기
ListUser.AppPopupView = function (p_documentID) {

    ParamArray = {
        'documentID': p_documentID
        , 'srh': ''
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "SHOWDOC", "1024", "700", true)
}

ListUser.ExChecked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".exCbxID").attr("checked", true);
    }
    else {
        $(".exCbxID").attr("checked", false);
    }
}
// 바우처 단위 체크
ListUser.VoucherCheck = function (_checkID, _voucherNo, _seq) {
    if ($("#" + _checkID).is(":checked")) {
        // 동일 바우처 넘버에 대한 체크

        $.each($(".exCbxID"), function (i, o) {
            if ($("#VoucherNo" + o.value).text() == _voucherNo) {
                $("#Excheckbox" + o.value).attr("checked", true);
            }
        });

        // VoucherNum 전역변수를 세팅해준다. 
        //DocumentNo = _voucherNo;

    }
    else {
        $.each($(".exCbxID"), function (i, o) {
            if ($("#VoucherNo" + o.value).text() == _voucherNo) {
                $("#Excheckbox" + o.value).attr("checked", false);
            }
        });
        //DocumentNo = ""; // clear
    }
}