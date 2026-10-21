var Item = {};

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        
        // 확인
        $('#cbtnAdd').click(function () {
            Item.Add();
        });

        // 다시작성
        $('#cbtnDel').click(function () {
            Item.Del();
        });
    });
});

/**************************************************
작성목적 : 평가항목 저장
***************************************************/
Item.Add = function () {
    var item = "";
    var score = "";
    var unit = "";
    var explain = "";

    for (i = 1; i <= 10; i++) {

        item = $("#CTBName" + i).val();
        score = $("#CTBScore" + i).val();
        unit = $("#CTBUnit" + i).val();
        explain = $("#CTBExplain" + i).val();

        if (unit.length > 0 || explain.length > 0) {
            if (item == "") {
                alert("최소 하나 이상의 평가항목 및 배점 입력이 필요합니다.");
                $("#CTBName" + i).focus();
                return;
            }
            else if (score == "") {
                alert("최소 하나 이상의 평가항목 및 배점 입력이 필요합니다.");
                $("#CTBScore" + i).focus();
                return;
            }
        }
        else if (item.length > 0) {
            if (score == "") {
                alert("최소 하나 이상의 평가항목 및 배점 입력이 필요합니다.");
                $("#CTBScore" + i).focus();
                return;
            }
        }
        else if (score.length > 0) {
            if (item == "") {
                alert("최소 하나 이상의 평가항목 및 배점 입력이 필요합니다.");
                $("#CTBName" + i).focus();
                return;
            }
        }

    }
    __doPostBack('lbtnSave', 'Add');
}

/**************************************************
작성목적 : 평가항목 다시작성
***************************************************/
Item.Del = function () {
    __doPostBack('lbtnSave', 'Del');
}

/**************************************************
작성목적 : 숫자 + ,만 입력
***************************************************/
Item.NumberCheck = function (arg) {
    var code = window.event.keyCode;

    // 숫자만 입력
    if ((code > 34 && code < 41) || (code > 47 && code < 58) || (code > 95 && code < 106) || code == 8 || code == 9 || code == 13 || code == 46) {
        window.event.returnValue = true;
        return;
    }
    window.event.returnValue = false;

}