RegisterLanguage = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            RegisterLanguage.Confirm();
        });

        //취소
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        if (UseName.indexOf("base") == -1) {
            $("#baseInput").hide();
        }

        if (UseName.indexOf("ko") == -1) {
            $("#koInput").hide();
        }
        
        if (UseName.indexOf("en") == -1) {
            $("#enInput").hide();
        }

        if (UseName.indexOf("ja") == -1) {
            $("#jaInput").hide();
        }

        if (UseName.indexOf("fr") == -1) {
            $("#frInput").hide();
        }

        if (UseName.indexOf("zh-cn") == -1) {
            $("#zhcnInput").hide();
        }

        if (UseName.indexOf("zh-tw") == -1) {
            $("#zhtwInput").hide();
        }

        $("#nameBase").val(NameBase);
        $("#nameKo").val(NameKo);
        $("#nameEn").val(NameEn);
        $("#nameJa").val(NameJa);
        $("#nameFr").val(NameFr);
        $("#nameZhcn").val(NameZhcn);
        $("#nameZhtw").val(NameZhtw);
    });
});


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 우편번호 호출페이로 보내기
***************************************************/
RegisterLanguage.Confirm = function () {

    var name_base = $("#nameBase").val().replace(/,/g, '');
    var name_ko = $("#nameKo").val().replace(/,/g, '');
    var name_en = $("#nameEn").val().replace(/,/g, '');
    var name_ja = $("#nameJa").val().replace(/,/g, '');
    var name_fr = $("#nameFr").val().replace(/,/g, '');
    var name_zhcn = $("#nameZhcn").val().replace(/,/g, '');
    var name_zhtw = $("#nameZhtw").val().replace(/,/g, '');

    name_base = encodeURIComponent(name_base);
    name_ko = encodeURIComponent(name_ko);
    name_en = encodeURIComponent(name_en);
    name_ja = encodeURIComponent(name_ja);
    name_fr = encodeURIComponent(name_fr);
    name_zhcn = encodeURIComponent(name_zhcn);
    name_zhtw = encodeURIComponent(name_zhtw);    

    var obj = eval("opener." + CallBackPage);
    obj(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw);
    self.close();
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 닫기
***************************************************/
RegisterLanguage.Close = function () {
    self.close();
}