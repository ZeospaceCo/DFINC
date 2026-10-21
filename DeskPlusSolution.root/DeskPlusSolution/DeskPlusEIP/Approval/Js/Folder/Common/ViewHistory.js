ViewHistory = {}

$(function () {

    $(document).ready(function () {

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();

            ParamArray = { 'childDocID': ChildDocID };
            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Approval/Common/Popup/Print.aspx?p=" + GetEncryptValue;

            if ($.browser.msie) {
                WindowObjectReference = PopupModalWinOpen(url, "800", "640")
            } else {
                PopupWin("800", "640", url, "", "", true);
            }
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            self.close();
        });

        ViewHistory.GetContentHtmlList();

    });
});

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-08
작성목적 : 문서 호출
**********************************************************************************************/
ViewHistory.GetContentHtmlList = function () {

    ifrmForm.document.open();
    ifrmForm.document.write($("#htaFormHTML").val());
    ifrmForm.document.close();

    // 문서 수정불가 처리.
    ViewHistory.SetInActive($('#ifrmForm').get());
}

// 수정불가 처리
ViewHistory.SetInActive = function (_targetFrm) {  //alert(_targetFrm);
    if (_targetFrm == null) return;

    try {
        // xscript처리.
        var delObj = $(_targetFrm).contents().find('xscript');
        $.each(delObj, function (i, o) {
            o.innerHTML = "";
        });
    } catch (e) { }

    // input,textarea 제어 (readonly)
    $(_targetFrm).contents().find('input,textarea').attr('readonly', "true");
    $(_targetFrm).contents().find('input').attr('contenteditable', "false");
    $(_targetFrm).contents().find('div').attr('contenteditable', "false");
    // 그외객체제어 (disable) - input(button,image,submit,reset),select,checkbox, ...
    $(_targetFrm).contents().find('input:checkbox,input:button,input:image,input:submit,input:reset').attr('disabled', "true");
    $(_targetFrm).contents().find('select').attr('disabled', "true");
    // 그외객체제어 (disable-선택적) - input(radio)
    var actObj = $("input:radio", _targetFrm);
    $.each(actObj, function (i, o) {
        var subobj = _targetFrm.getElementsByName(o.name);
        for (var i = 0; i < subobj.length; i++) {
            if (!subobj[i].checked) {
                subobj[i].setAttribute("disabled", "true");
            }
        }
    });

    $(_targetFrm).contents().find('input:radio').attr('disabled', "true");
}