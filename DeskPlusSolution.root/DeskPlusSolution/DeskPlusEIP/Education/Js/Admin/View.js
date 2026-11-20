View = { deleting: false, sendingAlarm: false }

/*************************************************************************************************/
View.cancellingComplete = false;
View.modifying = false;
View.savingConfirmation = false;
View.confirmationState = null;
View.bulkAlarmDelayed = null;
View.sendingMail = false;
View.bulkMailDelayed = null;

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

        $('#cbtnTargetList').click(function (e) {
            e.preventDefault();
            View.OpenTargetList();
        });

        $('#cbtnSendAlarm').click(function (e) {
            e.preventDefault();
            View.SendAlarm();
        });

        $('#cbtnSendAllAlarm').click(function (e) {
            e.preventDefault();
            View.SendAllAlarm();
        });

        $('#cbtnSendDelayedAlarm').click(function (e) {
            e.preventDefault();
            View.SendDelayedAlarm();
        });

        $('#cbtnSendIncompleteMail').click(function (e) {
            e.preventDefault();
            View.SendIncompleteMail();
        });

        $('#cbtnSendDelayedMail').click(function (e) {
            e.preventDefault();
            View.SendDelayedMail();
        });

        $('#cbtnDelete').click(function (e) {
            e.preventDefault();
            View.DeleteList();
        });

        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            View.OpenModify();
        });

        View.InitConfirmation();
        if (HasEducation) View.GetAttachFileList();
    });
});

View.IsConfirmationState = function (state) {
    return !!(state && HasEducation && typeof TargetUserID != 'undefined'
        && View.ConfirmationID(state.ListID) == View.ConfirmationID(ListID)
        && View.ConfirmationID(state.UserID) == View.ConfirmationID(TargetUserID)
        && View.ConfirmationID(state.ListID) > 0 && View.ConfirmationID(state.UserID) > 0
        && typeof state.IsConfirmed == 'boolean'
        && /^[0-9]+$/.test(String(state.ConfirmUserID))
        && Number(state.ConfirmUserID) <= 2147483647
        && typeof state.ConfirmUserName == 'string' && typeof state.ConfirmDateStr == 'string');
}

View.ConfirmationID = function (value) {
    return /^[0-9]+$/.test(String(value)) && Number(value) <= 2147483647 ? Number(value) : -1;
}

View.RenderConfirmation = function (state) {
    IsAdminConfirmed = state.IsConfirmed;
    $('#chkAdminConfirm').prop('checked', state.IsConfirmed);
    $('#eduConfirmUserName').text(state.ConfirmUserName);
    $('#eduConfirmDate').text(state.ConfirmDateStr);
    View.ApplyCompletionPolicy();
}

View.InitConfirmation = function () {
    var checkbox = $('#chkAdminConfirm');
    if (!checkbox.length) return;
    if (typeof AdminConfirmation == 'undefined' || !View.IsConfirmationState(AdminConfirmation)) {
        View.confirmationUnknown = true;
        View.ApplyCompletionPolicy();
        checkbox.prop('disabled', true);
        $('#eduConfirmSaveStatus').text('관리자 확인 정보를 불러오지 못했습니다. 화면을 새로고침해 주세요.');
        return;
    }

    View.confirmationUnknown = false;
    View.confirmationState = AdminConfirmation;
    View.RenderConfirmation(View.confirmationState);
    checkbox.prop('disabled', false).change(function () { View.SaveConfirmation(); });
}

View.SaveConfirmation = function () {
    if (!View.IsConfirmationState(View.confirmationState)) return;
    if (View.savingConfirmation || View.IsBusy() || View.CheckUploadWindow()) {
        View.RenderConfirmation(View.confirmationState);
        return;
    }

    var confirmed = $('#chkAdminConfirm').prop('checked') === true;
    if (confirmed === View.confirmationState.IsConfirmed) return;

    View.savingConfirmation = true;
    $('#chkAdminConfirm').prop('disabled', true);
    $('#eduConfirmSaveStatus').text('저장 중입니다.');
    View.SetBusyButtons();
    try {
        ParamArray = { 'listID': View.ConfirmationID(ListID), 'userID': View.ConfirmationID(TargetUserID), 'confirmYN': confirmed ? 'Y' : 'N' };
        CallEncrypt(CallGetParam());
        $.ajax({
            type: 'POST',
            url: '/DeskPlusEIP/Education/Ws/Admin/List.asmx/SetConfirmation',
            data: JSON.stringify({ p: GetEncryptValue }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            timeout: 30000,
            success: View.SuccessSaveConfirmation,
            error: View.ErrorSaveConfirmation
        });
    } catch (error) {
        View.ErrorSaveConfirmation();
    }
}

View.SuccessSaveConfirmation = function (data) {
    if (!View.savingConfirmation) return;
    var state = data && data.d;
    if (!View.IsConfirmationState(state)) {
        View.ErrorSaveConfirmation();
        return;
    }
    var wasConfirmed = View.confirmationState && View.confirmationState.IsConfirmed;
    View.confirmationUnknown = false;
    View.confirmationState = state;
    View.RenderConfirmation(state);
    View.FinishSaveConfirmation();
    $('#eduConfirmSaveStatus').text('');
    // 최초 조회 때 서버에서 숨긴 버튼은 확인 해제 후 새 상태로 다시 렌더링합니다.
    if (wasConfirmed === true && state.IsConfirmed === false) window.location.reload();
}

View.ErrorSaveConfirmation = function () {
    if (!View.savingConfirmation) return;
    View.RenderConfirmation(View.confirmationState);
    View.confirmationUnknown = true;
    View.ApplyCompletionPolicy();
    View.FinishSaveConfirmation();
    var message = '관리자 확인 저장 결과를 확인하지 못했습니다. 화면을 새로고침한 뒤 다시 시도해 주세요.';
    $('#eduConfirmSaveStatus').text(message);
    alert(message);
}

View.FinishSaveConfirmation = function () {
    View.savingConfirmation = false;
    $('#chkAdminConfirm').prop('disabled', false);
    View.SetBusyButtons();
}

View.OpenModify = function () {
    if (!HasEducation || !ModifyUrl || View.IsBusy() || View.CheckUploadWindow()) return;

    View.modifying = true;
    View.SetBusyButtons();
    try {
        ParamArray = { 'listID': ListID };
        CallEncrypt(CallGetParam());
        $.ajax({
            type: 'POST',
            url: '/DeskPlusEIP/Education/Ws/Admin/View.asmx/CanModifyEducation',
            data: JSON.stringify({ p: GetEncryptValue }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: View.SuccessOpenModify,
            error: View.ErrorOpenModify
        });
    } catch (error) {
        View.ErrorOpenModify();
    }
}

View.SuccessOpenModify = function (data, status) {
    View.FinishOpenModify();
    if (data && data.d === 1) {
        window.location.href = ModifyUrl;
    } else {
        alert('수정할 내역을 확인할 수 없습니다. 현재 화면을 새로고침해 주세요.');
    }
}

View.ErrorOpenModify = function () {
    View.FinishOpenModify();
    alert('수정 가능 여부를 확인하지 못했습니다. 다시 시도해 주세요.');
}

View.FinishOpenModify = function () {
    View.modifying = false;
    View.SetBusyButtons();
}

View.OpenTargetList = function () {
    if (!HasEducation) return;

    ParamArray = { 'listID': ListID };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Education/Admin/TargetList.aspx?p=" + encodeURIComponent(GetEncryptValue);
    PopupWin(760, 430, url, "", "", true);
}

View.SendAlarm = function () {
    var userID = Number(TargetUserID);
    if (!HasEducation || IsComplete || View.IsBusy() || !/^[0-9]+$/.test(String(TargetUserID))
        || userID <= 0 || userID > 2147483647) return;
    if (View.CheckUploadWindow() || !confirm('현재 보고 있는 사용자에게 완료 요청 알람을 발송하시겠습니까?')) return;

    View.sendingAlarm = true;
    try {
        View.SetBusyButtons();
        ParamArray = { 'listID': ListID, 'userID': userID };
        CallEncrypt(CallGetParam());

        $.ajax({
            type: "POST",
            url: "/DeskPlusEIP/Education/Ws/Admin/View.asmx/SendAlarm",
            data: JSON.stringify({ p: GetEncryptValue }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: View.SuccessSendAlarm,
            error: View.ErrorSendAlarm
        });
    } catch (error) {
        View.ErrorSendAlarm();
    }
}

View.SuccessSendAlarm = function (data, status) {
    try {
        if (data && data.d === true) alert('메신저 알람 발송을 요청했습니다.');
        else alert('발송 요청을 처리하지 못했습니다. 사용자의 완료 여부를 확인해 주세요.');
    } finally {
        View.FinishSendAlarm();
    }
}

View.SendAllAlarm = function () {
    View.SendBulkAlarm(false);
}

View.SendDelayedAlarm = function () {
    View.SendBulkAlarm(true);
}

View.SendBulkAlarm = function (delayed) {
    if (!HasEducation || typeof TargetUserID == 'undefined' || View.ConfirmationID(TargetUserID) != 0
        || View.IsBusy() || View.CheckUploadWindow()) return;
    var message = delayed
        ? '지연 대상자에게 완료 요청 알람을 발송하시겠습니까?'
        : '미완료 대상자에게 완료 요청 알람을 발송하시겠습니까?';
    if (!confirm(message)) return;

    View.sendingAlarm = true;
    View.bulkAlarmDelayed = delayed === true;
    try {
        View.SetBusyButtons();
        ParamArray = { 'listID': ListID };
        CallEncrypt(CallGetParam());

        $.ajax({
            type: "POST",
            url: "/DeskPlusEIP/Education/Ws/Admin/View.asmx/" + (delayed ? "SendDelayedAlarm" : "SendAllAlarm"),
            data: JSON.stringify({ p: GetEncryptValue }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: View.SuccessSendAllAlarm,
            error: View.ErrorSendAlarm
        });
    } catch (error) {
        View.ErrorSendAlarm();
    }
}

View.SuccessSendAllAlarm = function (data, status) {
    try {
        var result = data && data.d;
        var recipientCount = View.AlarmCount(result && result.RecipientCount);
        var successCount = View.AlarmCount(result && result.SuccessCount);
        var failureCount = View.AlarmCount(result && result.FailureCount);
        if (recipientCount < 0 || successCount < 0 || failureCount < 0 || successCount + failureCount != recipientCount) {
            alert('발송 결과를 확인할 수 없습니다. 즉시 재발송하지 말고 수신 여부를 확인해 주세요.');
        } else if (recipientCount == 0) {
            alert(View.bulkAlarmDelayed
                ? '알람을 발송할 지연 상태의 대상자가 없습니다.'
                : '알람을 발송할 진행 상태의 미완료 대상자가 없습니다.');
        } else if (failureCount == 0) {
            alert(successCount + '명에게 메신저 알람 발송을 요청했습니다.');
        } else {
            alert(successCount + '명 요청 성공, ' + failureCount + '명 실패. 이미 요청된 사용자에게 중복 발송되지 않도록 전체 재발송은 주의해 주세요.');
        }
    } finally {
        View.FinishSendAlarm();
    }
}

View.AlarmCount = function (value) {
    if (!/^[0-9]+$/.test(String(value))) return -1;
    var count = Number(value);
    return count <= 2147483647 ? count : -1;
}

View.ErrorSendAlarm = function () {
    try {
        alert('발송 결과를 확인할 수 없습니다. 즉시 재발송하지 말고 수신 여부를 확인해 주세요.');
    } finally {
        View.FinishSendAlarm();
    }
}

View.FinishSendAlarm = function () {
    View.sendingAlarm = false;
    View.bulkAlarmDelayed = null;
    View.SetBusyButtons();
}

View.SendIncompleteMail = function () {
    View.SendBulkMail(false);
}

View.SendDelayedMail = function () {
    View.SendBulkMail(true);
}

View.SendBulkMail = function (delayed) {
    if (!HasEducation || View.ConfirmationID(ListID) <= 0 || typeof TargetUserID == 'undefined'
        || View.ConfirmationID(TargetUserID) != 0 || View.sendingMail || View.IsBusy()
        || View.CheckUploadWindow()) return;
    var message = delayed
        ? '지연 대상자에게 완료 요청 메일을 발송하시겠습니까?'
        : '미완료 대상자에게 완료 요청 메일을 발송하시겠습니까?';
    if (!confirm(message)) return;

    View.sendingMail = true;
    View.bulkMailDelayed = delayed === true;
    try {
        View.SetBusyButtons();
        ParamArray = { 'listID': View.ConfirmationID(ListID) };
        CallEncrypt(CallGetParam());
        $.ajax({
            type: 'POST',
            url: '/DeskPlusEIP/Education/Ws/Admin/View.asmx/' + (delayed ? 'SendDelayedMail' : 'SendIncompleteMail'),
            data: JSON.stringify({ p: GetEncryptValue }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            timeout: 30000,
            success: View.SuccessSendBulkMail,
            error: View.ErrorSendBulkMail
        });
    } catch (error) {
        View.ErrorSendBulkMail();
    }
}

View.SuccessSendBulkMail = function (data) {
    if (!View.sendingMail) return;
    try {
        var result = data && data.d;
        var recipientCount = View.AlarmCount(result && result.RecipientCount);
        var successCount = View.AlarmCount(result && result.SuccessCount);
        var failureCount = View.AlarmCount(result && result.FailureCount);
        var validCounts = recipientCount >= 0 && successCount >= 0 && failureCount >= 0;
        if (validCounts && result.Outcome === 'Sent' && recipientCount > 0
            && successCount === recipientCount && failureCount === 0) {
            alert(successCount + '명에게 메일을 발송했습니다.');
        } else if (validCounts && result.Outcome === 'RolledBack' && successCount === 0
            && failureCount === recipientCount) {
            alert('메일 발송에 실패했습니다. 다시 시도해 주시고, 문제가 지속되면 관리자에게 문의해 주세요.');
        } else if (validCounts && result.Outcome === 'NoRecipients'
            && recipientCount === 0 && successCount === 0 && failureCount === 0) {
            alert(View.bulkMailDelayed
                ? '메일을 발송할 지연 상태의 대상자가 없습니다.'
                : '메일을 발송할 진행 상태의 미완료 대상자가 없습니다.');
        } else {
            alert('메일 발송 결과를 확인할 수 없습니다. 중복 발송될 수 있으니 다시 시도하지 말고 관리자에게 문의해 주세요.');
        }
    } finally {
        View.FinishSendBulkMail();
    }
}

View.ErrorSendBulkMail = function () {
    if (!View.sendingMail) return;
    try {
        alert('메일 발송 결과를 확인할 수 없습니다. 중복 발송될 수 있으니 다시 시도하지 말고 관리자에게 문의해 주세요.');
    } finally {
        View.FinishSendBulkMail();
    }
}

View.FinishSendBulkMail = function () {
    View.sendingMail = false;
    View.bulkMailDelayed = null;
    View.SetBusyButtons();
}

View.CancelComplete = function () {
    var userID = Number(TargetUserID);
    if (!HasEducation || !IsComplete || View.IsBusy() || !/^[0-9]+$/.test(String(TargetUserID))
        || userID <= 0 || userID > 2147483647) return;
    if (!View.CheckCancellationPolicy()) return;
    if (View.CheckUploadWindow() || !confirm('완료를 취소하시겠습니까?')) return;

    View.cancellingComplete = true;
    try {
        View.SetBusyButtons();
        ParamArray = { 'listID': ListID, 'userID': userID };
        CallEncrypt(CallGetParam());
        $.ajax({
            type: "POST",
            url: "/DeskPlusEIP/Education/Ws/Admin/View.asmx/CancelComplete",
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

View.DeleteList = function () {
    if (!HasEducation || View.IsBusy() || View.CheckUploadWindow()) return;

    View.deleting = true;
    try {
        View.SetBusyButtons();
        ParamArray = { 'listID': ListID };
        CallEncrypt(CallGetParam());

        $.ajax({
            type: "POST",
            url: "/DeskPlusEIP/Education/Ws/Admin/View.asmx/CanDeleteEducation",
            data: JSON.stringify({ p: GetEncryptValue }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: View.SuccessCanDeleteEducation,
            error: View.ErrorCanDeleteEducation
        });
    } catch (error) {
        View.ErrorCanDeleteEducation();
    }
}

View.SuccessCanDeleteEducation = function (data, status) {
    if (!View.deleting) return;
    if (!data || data.d !== 1) {
        try {
            if (data && data.d === 2) alert('완료자가 있는 내역은 삭제할 수 없습니다.');
            else alert('삭제할 내역을 확인할 수 없습니다. 현재 화면을 새로고침해 주세요.');
        } finally {
            View.FinishDelList();
        }
        return;
    }

    try {
        if (!confirm('해당 내역을 삭제하시겠습니까?')) {
            View.FinishDelList();
            return;
        }
        ParamArray = { 'listID': ListID };
        CallEncrypt(CallGetParam());

        $.ajax({
            type: "POST",
            url: "/DeskPlusEIP/Education/Ws/Admin/View.asmx/DelList",
            data: JSON.stringify({ p: GetEncryptValue }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: View.SuccessDelList,
            error: View.ErrorDelList
        });
    } catch (error) {
        View.ErrorDelList();
    }
}

View.ErrorCanDeleteEducation = function () {
    try {
        alert('삭제 가능 여부를 확인하지 못했습니다. 다시 시도해 주세요.');
    } finally {
        View.FinishDelList();
    }
}

View.SuccessDelList = function (data, status) {
    if (data && data.d === true) {
        View.FinishDelList();
        View.GoList(true);
        return;
    }
    View.ErrorDelList();
}

View.ErrorDelList = function (xhr) {
    try {
        if (xhr && xhr.status === 409) alert('완료자가 있는 내역은 삭제할 수 없습니다.');
        else alert('삭제 처리에 실패했습니다. 다시 시도해 주세요.');
    } finally {
        View.FinishDelList();
    }
}

View.FinishDelList = function () {
    View.deleting = false;
    View.SetBusyButtons();
}


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
View.GoList = function (afterDelete) {

    if (View.IsBusy() || View.CheckUploadWindow()) return;

    if (afterDelete === true && typeof ReturnAfterDeleteUrl == 'string' && ReturnAfterDeleteUrl) {
        window.location.href = ReturnAfterDeleteUrl;
        return;
    }

    if (ReturnListUrl) {
        window.location.href = ReturnListUrl;
        return;
    }

    ParamArray = {
        'searchTarget': SearchTarget,
        'searchItem': SearchItem,
        'searchDateValue': SearchDateValue
    };

    CallEncrypt(CallGetParam());

    location = "DashBoard.aspx?p=" + GetEncryptValue;    
}

