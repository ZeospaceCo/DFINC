Memo = {}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-23
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $('#before').click(function (e) {
        Memo.goBefore();
    });

    $('#listTitle').click(function (e) {
        Memo.goBefore();
    });

});

/**************************************************
작성자   : famo23
작성일자 : 2015-10-23
작성목적 : 취소 버튼 클릭시
***************************************************/
Memo.goBefore = function () {

    ParamArray = {
        'BPage': BPage,
        'PageNo': PageNo,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    if (BPage == "Forward") {
        location.href = "DocList.aspx?p=" + GetEncryptValue;
    }
    else {
        location.href = "CirDocList.aspx?p=" + GetEncryptValue;
    }
}

