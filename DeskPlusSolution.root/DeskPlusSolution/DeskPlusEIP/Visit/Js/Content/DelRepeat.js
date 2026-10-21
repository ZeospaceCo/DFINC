DelRepeat = {}

/**************************************************
작성자   : famo23
작성일자 : 2014-02-18
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            DelRepeat.Confirm();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

    });
});

/*************************************************************************************
작성자   : famo23
작성일자 : 2014-02-18
작성목적 : 반복설정 저장
*************************************************************************************/
DelRepeat.Confirm = function () {

    var seleR = "";
    if ($("#rbRechk1").attr("checked")) seleR = "1"; //이번일정만
    if ($("#rbRechk2").attr("checked")) seleR = "2"; //향후모든일정
    if ($("#rbRechk3").attr("checked")) seleR = "3"; //모든일정

    parent.RepeatDel(seleR);

    self.close();
}
