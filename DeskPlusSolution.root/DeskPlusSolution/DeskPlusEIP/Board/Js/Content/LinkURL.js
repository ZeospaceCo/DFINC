LinkURL = {}
LinkURL.Control = null;

/*******************************************************
작성자: famo23
작성일: 2012-02-15
작성목적: 초기화
********************************************************/
$(function () {
    $(document).ready(function () {
        // 자원 예약
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();

            LinkURL.Confirm();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            self.close();
        });
    });
});

/*******************************************************************************
작성자: 이지원
작성일: 2017-03-17
작성목적: URL링크 입력
********************************************************************************/
LinkURL.Confirm = function () {

    var tmpLinkURLName = $("#txtLinkURLName").val();
    //var tmpLinkURL = $("#txtLinkURL").text();
    var tmpLinkURL = $("#txtLinkURL").val();

    if (tmpLinkURLName.length < 1) {
        alert(R_URLNameMsg);
        return;
    }

    if (tmpLinkURL.length < 1) {
        alert(R_URLMsg);
        return;
    }

    eval("top.opener.SuccessLinkURL")(tmpLinkURLName, tmpLinkURL);
    self.close();
}