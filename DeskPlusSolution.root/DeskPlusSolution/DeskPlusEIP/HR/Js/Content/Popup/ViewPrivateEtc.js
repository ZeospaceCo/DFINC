ViewPrivateEtc = {}

$(function () {

    $(document).ready(function () {
        //닫기        
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        // 휴가
        if (bHoliAll == "1") {
            $('#trHoli').show();
        }

        // 연장
        if (bOver == "1") {
            $('#trOver').show();
        }

        // 지각
        if (bLate == "1") {
            $('#trLate').show();
        }

        // 조퇴
        if (bEarly == "1") {
            $('#trEarly').show();
        }
    });
});

/**************************************************
작성자   : kkzz52
작성일자 : 2019-03-07
작성목적 : 휴가원, 연장근무 문서보기
***************************************************/
ViewPrivateEtc.DocView = function (docID) {

    ParamArray = {
        'documentID': docID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;

    window.open(url, "View" + docID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}