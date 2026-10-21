Add = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //수정
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            Add.Process();
        });

        //취소
        $('#CbtnCancel').click(function (e) {
            e.preventDefault();
            Add.Cancel();
        });

        if (NonActiveX == "T") {
            xfe.setHtmlValue($("#tbHiddenMime_contents").val());
            $('#ActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "Add.aspx?" + ClientQueryString;
                }
            });
        } else {
            $('#NonActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "AddN.aspx?" + ClientQueryString;
                }
            });
        }
    });
});

/**************************************************
작성자   : shimpark
작성목적 : 추가 및 수정
***************************************************/

Add.Process = function () {
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbName", R_TemplateName, true);
    if (!validator.validate()) return;

    if (NonActiveX == "T") {
        $("#tbHiddenHTML_contents").val(xfe.getHtmlValue());
        $("#tbHiddenMime_contents").val("");
        $("#tbHiddenText_contents").val(xfe.getTextValue());
    } else {
        if (!EditorSecurity(form1.twe)) return;

        var strHtml = form1.twe.HtmlValue;
        if (strHtml.indexOf(".js") > -1 || strHtml.indexOf('=\"xss:expression') > -1) {
            strHtml = strHtml.replace(".js\"", "\"");
            strHtml = strHtml.replace('=\"xss:expression', "");

            form1.twe.HtmlValue = strHtml;

            alert(R_DoubtStcipt);
        }

        $("#tbHiddenHTML_contents").val(form1.twe.HtmlValue);
        $("#tbHiddenMime_contents").val(form1.twe.MimeEnValue);
        $("#tbHiddenText_contents").val(form1.twe.TextValue);
    }

    var contents = $("#tbHiddenText_contents").val();
    var contentHtml = $("#tbHiddenHTML_contents").val();
    if (contentHtml.indexOf("<IMG") > -1) {
        contents = "<img"; //dummy (if you save only image in editor)
    }

    if (contents == "") {
        alert(R_Add_InputContent);
        return;
    }

    //2번 이상 클릭 불가
    $("#CbtnConfirm").attr('onclick', '').unbind('click');

    __doPostBack("LBTNProcess", "ADD");
}

/**************************************************
작성자   : shimpark
작성목적 : 취소
***************************************************/
Add.Cancel = function () {
    window.opener.List.GetPageSubList(ItemID);
    window.opener.List.Resource();
    self.close();
}