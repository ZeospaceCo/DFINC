RegisterLangAddress = {}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/
$(function () {

    $(document).ready(function () {

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            RegisterLangAddress.Confirm();
        });

        //취소
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        $("#addressFirstBase").val(FirstBase);
        $("#addressEndBase").val(EndBase);
        $("#addressFirstKo").val(FirstKo);
        $("#addressEndKo").val(EndKo);
        $("#addressFirstEn").val(FirstEn);
        $("#addressEndEn").val(EndEn);
        $("#addressFirstJa").val(FirstJa);
        $("#addressEndJa").val(EndJa);
        $("#addressFirstFr").val(FirstFr);
        $("#addressEndFr").val(EndFr);
        $("#addressFirstZhcn").val(FirstZhcn);
        $("#addressEndZhcn").val(EndZhcn);
        $("#addressFirstZhtw").val(FirstZhtw);
        $("#addressEndZhtw").val(EndZhtw);
    });
});

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 우편번호 호출페이로 보내기
***************************************************/
RegisterLangAddress.Confirm = function () {

    var firstBase = encodeURIComponent($("#addressFirstBase").val().replace(/,/g, ''));
    var endBase = encodeURIComponent($("#addressEndBase").val().replace(/,/g, ''));
    var firstKo = encodeURIComponent($("#addressFirstKo").val().replace(/,/g, ''));
    var endKo = encodeURIComponent($("#addressEndKo").val().replace(/,/g, ''));
    var firstEn = encodeURIComponent($("#addressFirstEn").val().replace(/,/g, ''));
    var endEn = encodeURIComponent($("#addressEndEn").val().replace(/,/g, ''));
    var firstJa = encodeURIComponent($("#addressFirstJa").val().replace(/,/g, ''));
    var endJa = encodeURIComponent($("#addressEndJa").val().replace(/,/g, ''));
    var firstFr = encodeURIComponent($("#addressFirstFr").val().replace(/,/g, ''));
    var endFr = encodeURIComponent($("#addressEndFr").val().replace(/,/g, ''));
    var firstZhcn = encodeURIComponent($("#addressFirstZhcn").val().replace(/,/g, ''));
    var endZhcn = encodeURIComponent($("#addressEndZhcn").val().replace(/,/g, ''));
    var firstZhtw = encodeURIComponent($("#addressFirstZhtw").val().replace(/,/g, ''));
    var endZhtw = encodeURIComponent($("#addressEndZhtw").val().replace(/,/g, ''));

    if (parent.disablePopup) {
        var obj = eval("parent." + CallBackPage);
        obj(firstBase, firstKo, firstEn, firstJa, firstFr, firstZhcn, firstZhtw, endBase, endKo, endEn, endJa, endFr, endZhcn, endZhtw);
        parent.disablePopup();
    } else {
        var obj = eval("opener." + CallBackPage);
        obj(firstBase, firstKo, firstEn, firstJa, firstFr, firstZhcn, firstZhtw, endBase, endKo, endEn, endJa, endFr, endZhcn, endZhtw);
        self.close();
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 닫기
***************************************************/
RegisterLangAddress.Close = function () {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}