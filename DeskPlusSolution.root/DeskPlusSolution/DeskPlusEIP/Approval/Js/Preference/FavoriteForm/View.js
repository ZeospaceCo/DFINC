View = {}

$(function () {

    $(document).ready(function () {

        //[닫기]
        $('#Close').click(function (e) { if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); } });

        View.Init();
    });
});

View.Init = function () {

    // 양식로드
    if ($("#htaFormHTML").val() == "") {
        var msg = "<div style='font-size:12px;font-weight:bold;color:#FF7E00;padding:20px 0px 0px 0px;text-align:center;'>"
        msg += R_Msg_AlarmError;
        msg += "</div>";
        $("#htaFormHTML").val(msg);
    }

    ifrmForm.document.open();
    ifrmForm.document.write($("#htaFormHTML").val());
    ifrmForm.document.close();

    ifrmObj = ifrmForm.document;
    setTimeout("View.InitForm();", 100);
}

View.InitForm = function () {

    // 결재망 초기화 (span & div) = 고정(ZD1,ZA1,ZH1 ..) + 유동(FLOWZD,FLOWZH,FLOWZA)
    var arrStamp = $(".ZD, .ZP, .ZQ, .ZH, .ZC, .ZR, .ZA", ifrmObj);
    $.each(arrStamp, function (i, o) {
        o.innerHTML = "&nbsp";
    });

    // 문서정보 태그 초기화 (input/div)
    var arrInfo = $(".formfieldinput", ifrmObj);
    $.each(arrInfo, function (i, o) {
        if (o.tagName.toUpperCase() == "INPUT") { o.value = "";}
        if (o.tagName.toUpperCase() == "DIV") { o.innerHTML = ""; }
    });
}