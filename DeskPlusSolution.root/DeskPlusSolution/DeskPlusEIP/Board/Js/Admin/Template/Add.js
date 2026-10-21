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

        //삭제
        $('#CbtnDel').click(function (e) {
            e.preventDefault();
            Add.Del();
        });

        //취소
        $('#CbtnCancel').click(function (e) {
            e.preventDefault();
            Add.Cancel();
        });

        xfe.setHtmlValue($("#contents").val());
        /*
        //ByFreeman 20120106 X-Free Editor - 템플릿 추가
        if (NonActiveX == "T") {
            xfe.setHtmlValue($("#contents").val());
	        var agent = navigator.userAgent.toLowerCase();
	        if((navigator.appName == 'Netscape' && navigator.userAgent.search('Trident') != -1) || (agent.indexOf("msie") != -1)) {
     
            } 
	        else{
	          $("#ActiveX").hide();
	        }

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
        */

        if (TemplateID == "0") {
            $("#CbtnDel").hide();
        }
    });
});

/**************************************************
작성자   : shimpark
작성목적 : 추가 및 수정
***************************************************/

Add.Process = function () {
    //에디터 내용 히든박스에 저장
    //ByFreeman 20120106 NonActiveX Test
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

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbName", R_TemplateName, true);
    if (!validator.validate()) return;

    var contents = $("#tbHiddenText_contents").val();
    var contentHtml = $("#tbHiddenHTML_contents").val();
    if (contentHtml.indexOf("<IMG") > -1) {
        contents = "<img"; //dummy (if you save only image in editor)
    }

    if (contents == "") {
        alert(R_Add_InputContent);
        return;
    }

    var args = "ADD";
    if (TemplateID != "0") {
        args = "MOD";
    }

    __doPostBack("LBTNProcess", args);
}

/**************************************************
작성자   : shimpark
작성목적 : 취소
***************************************************/
Add.Cancel = function () {

    ParamArray = {
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성목적 : 삭제
***************************************************/
Add.Del = function () {
    if (TemplateID != "0") {
        if (window.confirm(R_ConfirmDeleteList)) {
            __doPostBack("LBTNProcess", "DEL");
        }
    }
}