View = {}

/*************************************************************************************************/
View.cancellingComplete = false;

$(function () {
    $(document).ready(function () {

        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            View.GoList();
        });

        $('#cbtnCancelComplete').click(function (e) {
            e.preventDefault();
            View.CancelComplete();
        });

        if (HasEducation) View.GetAttachFileList();
    });
});

View.CancelComplete = function () {
    if (!HasEducation || !IsComplete || View.IsBusy()) return;
    if (!View.CheckCancellationPolicy()) return;
    if (View.CheckUploadWindow()) return;
    if (!confirm('완료를 취소하시겠습니까?')) return;

    View.cancellingComplete = true;
    View.SetBusyButtons();
    try {
        ParamArray = { 'listID': ListID };
        CallEncrypt(CallGetParam());
        $.ajax({
            type: "POST",
            url: "/DeskPlusEIP/Education/Ws/Content/View.asmx/CancelComplete",
            data: JSON.stringify({ p: GetEncryptValue }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: View.SuccessCancelComplete,
            error: View.ErrorCancelComplete
        });
    } catch (error) {
        View.ErrorCancelComplete();
    }
}

View.SuccessCancelComplete = function (data, status) {
    var result = data && data.d;
    if (!result || typeof result.Cancelled != 'boolean') {
        View.ErrorCancelComplete();
        return;
    }
    try {
        if (!result.Cancelled) alert('완료 취소할 수 없는 상태입니다. 완료·관리자 확인 상태를 다시 확인해 주세요.');
        window.location.reload();
    } finally {
        View.FinishCancelComplete();
    }
}

View.ErrorCancelComplete = function () {
    try {
        alert('완료 취소 처리에 실패했습니다. 다시 시도해 주세요.');
    } finally {
        View.FinishCancelComplete();
    }
}

View.FinishCancelComplete = function () {
    View.cancellingComplete = false;
    View.SetBusyButtons();
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

    if (View.IsBusy() || View.CheckUploadWindow()) return;

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

