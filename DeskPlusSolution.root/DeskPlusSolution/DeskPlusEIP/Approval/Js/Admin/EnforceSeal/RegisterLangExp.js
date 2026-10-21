RegisterLangExp = {}

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
            RegisterLangExp.Confirm();
        });

        //취소
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        /*if (UseName.indexOf("base") == -1) { $("#baseInput").hide(); }
        if (UseName.indexOf("ko") == -1) { $("#koInput").hide(); }
        if (UseName.indexOf("en") == -1) { $("#enInput").hide(); }
        if (UseName.indexOf("ja") == -1) { $("#jaInput").hide(); }
        if (UseName.indexOf("fr") == -1) { $("#frInput").hide(); }
        if (UseName.indexOf("zhcn") == -1) { $("#zhcnInput").hide(); }
        if (UseName.indexOf("zhtw") == -1) { $("#zhtwInput").hide(); }*/

        $("#expFirstBase").val(FirstBase);
        $("#expMidBase").val(MidBase);
        $("#expEndBase").val(EndBase);
        $("#expFirstKo").val(FirstKo);
        $("#expMidKo").val(MidKo);
        $("#expEndKo").val(EndKo);
        $("#expFirstEn").val(FirstEn);
        $("#expMidEn").val(MidEn);
        $("#expEndEn").val(EndEn);
        $("#expFirstJa").val(FirstJa);
        $("#expMidJa").val(MidJa);
        $("#expEndJa").val(EndJa);
        $("#expFirstFr").val(FirstFr);
        $("#expMidFr").val(MidFr);
        $("#expEndFr").val(EndFr);
        $("#expFirstZhcn").val(FirstZhcn);
        $("#expMidZhcn").val(MidZhcn);
        $("#expEndZhcn").val(EndZhcn);
        $("#expFirstZhtw").val(FirstZhtw);
        $("#expMidZhtw").val(MidZhtw);
        $("#expEndZhtw").val(EndZhtw);
    });
});

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 우편번호 호출페이로 보내기
***************************************************/
RegisterLangExp.Confirm = function () {

    var firstBase = encodeURIComponent($("#expFirstBase").val().replace(/,/g, ''));
    var midBase = encodeURIComponent($("#expMidBase").val().replace(/,/g, ''));
    var endBase = encodeURIComponent($("#expEndBase").val().replace(/,/g, ''));
    var firstKo = encodeURIComponent($("#expFirstKo").val().replace(/,/g, ''));
    var midKo = encodeURIComponent($("#expMidKo").val().replace(/,/g, ''));
    var endKo = encodeURIComponent($("#expEndKo").val().replace(/,/g, ''));
    var firstEn = encodeURIComponent($("#expFirstEn").val().replace(/,/g, ''));
    var midEn = encodeURIComponent($("#expMidEn").val().replace(/,/g, ''));
    var endEn = encodeURIComponent($("#expEndEn").val().replace(/,/g, ''));
    var firstJa = encodeURIComponent($("#expFirstJa").val().replace(/,/g, ''));
    var midJa = encodeURIComponent($("#expMidJa").val().replace(/,/g, ''));
    var endJa = encodeURIComponent($("#expEndJa").val().replace(/,/g, ''));
    var firstFr = encodeURIComponent($("#expFirstFr").val().replace(/,/g, ''));
    var midFr = encodeURIComponent($("#expMidFr").val().replace(/,/g, ''));
    var endFr = encodeURIComponent($("#expEndFr").val().replace(/,/g, ''));
    var firstZhcn = encodeURIComponent($("#expFirstZhcn").val().replace(/,/g, ''));
    var midZhcn = encodeURIComponent($("#expMidZhcn").val().replace(/,/g, ''));
    var endZhcn = encodeURIComponent($("#expEndZhcn").val().replace(/,/g, ''));
    var firstZhtw = encodeURIComponent($("#expFirstZhtw").val().replace(/,/g, ''));
    var midZhtw = encodeURIComponent($("#expMidZhtw").val().replace(/,/g, ''));
    var endZhtw = encodeURIComponent($("#expEndZhtw").val().replace(/,/g, ''));

    if (parent.disablePopup) {
        var obj = eval("parent." + CallBackPage);
        obj(firstBase, firstKo, firstEn, firstJa, firstFr, firstZhcn, firstZhtw, midBase, midKo, midEn, midJa, midFr, midZhcn, midZhtw, endBase, endKo, endEn, endJa, endFr, endZhcn, endZhtw);
        parent.disablePopup();
    } else {
        var obj = eval("opener." + CallBackPage);
        obj(firstBase, firstKo, firstEn, firstJa, firstFr, firstZhcn, firstZhtw, midBase, midKo, midEn, midJa, midFr, midZhcn, midZhtw, endBase, endKo, endEn, endJa, endFr, endZhcn, endZhtw);
        self.close();
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 닫기
***************************************************/
RegisterLangExp.Close = function () {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}