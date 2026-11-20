var View = {};

/***********************************************************************************************/
View.cancellingComplete = false;

$(function () {
    $('#cbtnClose').click(function (e) {
        e.preventDefault();
        if (View.IsBusy()) {
            alert('처리 중입니다. 완료 후 닫아 주세요.');
            return;
        }
        if (View.CheckUploadWindow()) return;
        window.close();
    });

    $('#cbtnCancelComplete').click(function (e) {
        e.preventDefault();
        View.CancelComplete();
    });

    if (HasEducation) View.GetAttachFileList();
});

/***********************************************************************************************
완료 취소
***********************************************************************************************/
View.CancelComplete = function () {
    if (!HasEducation || !IsComplete || View.IsBusy()) return;
    if (!View.CheckCancellationPolicy()) return;
    if (View.CheckUploadWindow() || !confirm('완료를 취소하시겠습니까? 등록된 첨부파일은 유지됩니다.')) return;

    View.cancellingComplete = true;
    try {
        View.SetBusyButtons();
        ParamArray = { 'listID': ListID };
        CallEncrypt(CallGetParam());
        $.ajax({
            type: 'POST',
            url: '/DeskPlusEIP/Education/Ws/Content/View.asmx/CancelComplete',
            data: JSON.stringify({ p: GetEncryptValue }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: View.SuccessCancelComplete,
            error: View.ErrorCancelComplete
        });
    } catch (error) {
        View.ErrorCancelComplete();
    }
};

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
};

View.ErrorCancelComplete = function () {
    try {
        alert('완료 취소 처리에 실패했습니다. 다시 시도해 주세요.');
    } finally {
        View.FinishCancelComplete();
    }
};

View.FinishCancelComplete = function () {
    View.cancellingComplete = false;
    View.SetBusyButtons();
};

/***********************************************************************************************
 공지첨부 조회 및 다운로드
***********************************************************************************************/
View.GetAttachFileList = function () {
    ParamArray = { 'listID': ListID };
    CallEncrypt(CallGetParam());
    var data = JSON.stringify({ p: GetEncryptValue });
    var url = '/DeskPlusEIP/Education/Ws/Admin/View.asmx/GetAttachFileList';
    AjaxService(url, data, View.SuccessAttachFile);
};

View.SuccessAttachFile = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Education/Jt/Admin/JTAttachFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);
};

View.FileDownload = function (filePath, fileName, fileGuid) {
    window.open('/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=' + filePath);
};
