View = {}

/*************************************************************************************************/
$(function () {
    $(document).ready(function () {

        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            View.GoList();
        });

        $('#cbtnComplete').click(function (e) {
            e.preventDefault();
            View.OpenComplete();
        });

        if (HasEducation) View.GetAttachFileList();
    });
});

View.OpenComplete = function () {
    if (!HasEducation) return;

    ParamArray = { 'listID': ListID };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Education/Content/AddComplete.aspx?p=" + encodeURIComponent(GetEncryptValue);
    PopupWin(640, 430, url, "", "", true);
}

View.CompleteSaved = function () {
    if (window.disablePopup) disablePopup();
    window.location.reload();
}


/***********************************************************************************************/
View.GetAttachFileList = function () {
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
        'currentPage': CurrentPage,
        'searchItem': SearchItem,
        'searchStatus': SearchStatus,
        'searchComplete': SearchComplete,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchDateField': SearchDateField,
        'searchDateValue': SearchDateValue
    };

    CallEncrypt(CallGetParam());

    location = "List.aspx?p=" + GetEncryptValue;    
}

