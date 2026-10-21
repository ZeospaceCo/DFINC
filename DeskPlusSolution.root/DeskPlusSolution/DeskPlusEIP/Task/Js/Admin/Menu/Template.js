Template = {}

$(function () {
    $(document).ready(function () {

        //수정
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            Template.Process();
        });

        //삭제
        $('#CbtnDel').click(function (e) {
            e.preventDefault();
            Template.Del();
        });

        //취소
        $('#CbtnCancel').click(function (e) {
            e.preventDefault();
            Template.Cancel();
        });

        xfe.setHtmlValue($("#tbHiddenMime_contents").val());
        /*
        if (NonActiveX == "T") {

            xfe.setHtmlValue($("#tbHiddenMime_contents").val());
            var agent = navigator.userAgent.toLowerCase();
            if ((navigator.appName == 'Netscape' && navigator.userAgent.search('Trident') != -1) || (agent.indexOf("msie") != -1)) {

            }
            else {
                $("#ActiveX").hide();
            }
            $('#ActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "Template.aspx?" + ClientQueryString;
                }
            });
        } else {
           
            $('#NonActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "TemplateN.aspx?" + ClientQueryString;
                }
            });
        }
        */

    });
});

//저장버튼 클릭시
Template.Process = function () {
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

Template.Del = function () {
    if (window.confirm(R_ConfirmDeleteList)) {
        //2번 이상 클릭 불가
        $("#CbtnDel").attr('onclick', '').unbind('click');
        __doPostBack("LBTNProcess", "DEL");
    }
}

//닫기 버튼클릭시 
Template.Cancel = function () {
    window.opener.List.GetPageSubList(ItemID);
    window.opener.List.Resource();
    self.close();
}