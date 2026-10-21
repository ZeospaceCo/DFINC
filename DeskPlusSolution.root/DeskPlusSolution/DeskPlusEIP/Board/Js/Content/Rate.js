/// <reference path="../../../Html/Js/jQuery/jquery.js" />

Rate = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //평가 저장
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            Rate.Confirm();
        });

        //취소
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //평가 현황
        $('#CbtnRatingList').click(function (e) {
            e.preventDefault();

            ParamArray = {
                'categoryID': CategoryID,
                'listID': ListID
            };

            CallEncrypt(CallGetParam());
            location = "RateList.aspx?p=" + GetEncryptValue;
        });
    });
});

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 평가 저장
***************************************************/
Rate.Confirm = function () {
    var selRate = $('input[name=rating]:radio:checked').val();

    if (selRate == "") {
        alert(Rate_SelectVote);
        return;
    }

    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID,
        'rating': selRate,
        'contents': $("#txtComments").val()
    };

    var url = "/DeskPlusEIP/Board/Ws/Content/Rate.asmx/AddRating";
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Rate.SuccessConfirm");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 평가 성공시
**********************************************************************************************/
Rate.SuccessConfirm = function (data, status) {
    alert(Rate_Success);
    //    parent.disablePopup();

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}