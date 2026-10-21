EditorNonActiveX = {}

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //EditorNonActiveX.Init();
        setTimeout("EditorNonActiveX.Init();", 1000);
    });
});

EditorNonActiveX.Init = function () {
    if ($("#htaEditorHTML", parent.parent.document).val() != "") {
        xfe.setHtmlValue(TemplateHeader + $("#htaEditorHTML", parent.parent.document).val() + TemplateFooter);
    } else {
        xfe.setHtmlValue(TemplateHeader + TemplateFooter);
    }
}