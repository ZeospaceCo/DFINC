View = {}

/*************************************************************************************************/
$(function () {
    $(document).ready(function () {

        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            View.GoList();
        });

        if (HasEducation) View.GetAttachFileList();
    });
});


/***********************************************************************************************/
View.GetAttachFileList = function () {
    if (!HasEducation) return;
    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessAttachFile");
    var url = "/DeskPlusEIP/Education/Ws/Admin/View.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}

/***********************************************************************************************/
View.SuccessAttachFile = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Education/Jt/Admin/JTAttachFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);
}

/*********************************************************************************************/
View.FileDownload = function (filePath, fileName, fileGuid) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
}

/********************************************************************************************/
View.GoList = function () {

    ParamArray = {
        'searchTarget': SearchTarget,
        'searchItem': SearchItem,
        'searchDateValue': SearchDateValue
    };

    CallEncrypt(CallGetParam());

    location = "DashBoard.aspx?p=" + GetEncryptValue;    
}

