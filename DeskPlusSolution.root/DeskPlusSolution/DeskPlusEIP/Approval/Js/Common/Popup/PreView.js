Preview = {}

$(function () {

    $(document).ready(function () {

        $('#cbtnPrint').click(function (e) {
            e.preventDefault();

            var divToPrint = document.getElementById('ifrmForm');

            //스타일정보+내용
            var html = divToPrint.contentWindow.document.documentElement.outerHTML;
            newWin = window.open();
            newWin.document.write(html);

            //alert(navigator.userAgent)
            if (navigator.userAgent.indexOf("Chrome") > 0 || navigator.userAgent.indexOf("Safari") > 0) {
                setTimeout("newWin.focus(); newWin.print();", 600);     //for, Chrome & safari
            } else {
                newWin.location.reload();                               //for, exploror & firefox
                newWin.focus();
                newWin.print();
            }

            setTimeout("newWin.close();", 1000);        
        });

        //[닫기]
        $('#Close').click(function (e) { if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); } });

        Preview.Init();
    });
});

Preview.Init = function () {

    targetObj = (window.dialogArguments) ? window.dialogArguments : parent;
    //alert($("#htaPrintArea", targetObj.document).length)

    //스타일정보+내용
    var divToPrint = targetObj.document.getElementById("ifrmForm");
    var html = divToPrint.contentWindow.document.documentElement.outerHTML;
    var editorObj = "";
    if ($("#EDITOR", targetObj.ifrmForm.document).length > 0) {
        editorObj = $("#EDITOR", targetObj.ifrmForm.document).html();
    }

    html = html.replace(editorObj, "");
    html = html.replace("Func_ApprovalEdit.css", "Func_ApprovalView.css");

    ifrmForm.document.open();
    ifrmForm.document.write(html);
    ifrmForm.document.close();

    ifrmObj = ifrmForm.document;
    setTimeout("Preview.InitForm();", 100);
}

Preview.InitForm = function () {

    // 문서내용(에디터) + 에디터이미지 처리(activeX인경우만, 이미지 업로드)
    Preview.SetEditorTags(NonActiveX);

    //  빈 결재란 처리
    Preview.SetDefaultTags();

    // Input Tag 처리
    Preview.SetReadOnly();
}

/**************************************************
에디터 처리 (+ 이미지 업로드 포함)
***************************************************/
Preview.SetEditorTags = function (_nonActiveX) {

    if ($("#EDITOR", ifrmForm.document).length < 1) return;

    var objHtml = "";

    // 문서내용 (에디터+이미지)
    if (_nonActiveX == "T") {

        var objEditor = targetObj.ifrmObj.getElementById("ifrmTFEditor").contentWindow.xfe;
        objHtml = objEditor.getHtmlValue();
    }
    else {
        var tmptwe = $("#twe", targetObj.ifrmObj.ifrmTFEditor.document);
        if (tmptwe == null) return;

        objHtml = tmptwe[0].BodyValue;
    }

    $("#EDITOR", ifrmObj).html(objHtml);
}

/**************************************************
빈결재란 및 텍스트노드(다중) 처리
***************************************************/
Preview.SetDefaultTags = function () {
    var zTags = $("span", ifrmObj);

    var arrSel = $(".ZDSel, .ZPSel, .ZQSel, .ZHSel, .ZCSel, .ZRSel, .ZASel", ifrmObj);
    $.each(arrSel, function (i, o) {
        o.className = o.className.replace("Sel", "");
    });

    // 불필요한 텍스트 노드 삭제 (ZH/ZC/ZP/ZQ/FLOWZD/FLOWZH/FLOWZA)
    var objRemove = Array(); var k = 0;
    var objArr = $("#ZH,#ZC,#ZP,#ZQ,#FLOWZD,#FLOWZH,#FLOWZA", ifrmObj);
    $.each(objArr, function (i, o) {
        $.each(o.childNodes, function (c, d) {
            if (d.nodeType == 3) { objRemove[k] = d; k++; }
        });
    });
    $.each(objRemove, function (i, o) {
        $(o, ifrmForm).remove();   //o.removeNode(true);
    });
    // 빈결재란 처리 (고정결재망)
    $.each(zTags, function (i, o) {
        if (o.id != "") {
            if (/Z{1}[D|H|C|P|Q|A][0-9]?[0-9]?[0-9]?$/.test(o.id)) {
                if (!$("#" + o.id, ifrmObj).attr("IsAsign") && $("#" + o.id, ifrmObj).children('span').length == 0) {
                    o.innerHTML = (/Z{1}[D|H|R|A][0-9]$/.test(o.id)) ? "/" : "";
                }
            }
        }
    });
}
/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : Input Tag 처리
***************************************************/
Preview.SetReadOnly = function () {
    var arrInfo = $(".formfieldinput", ifrmObj);
    $.each(arrInfo, function (i, o) {
        if (o.tagName.toUpperCase() == "INPUT") { o.setAttribute("readonly", "true"); }
    });
}
