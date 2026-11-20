// 보기 화면의 첨부파일 업로드와 바로 완료 처리를 공통으로 사용합니다.
// 각 화면의 View.js 다음에 로드해야 합니다.
View.completing = false;
View.savingAttachments = false;
View.deletingFile = false;
View.uploadWindow = null;
View.spoolRequestSerial = 0;
View.confirmationUnknown = false;

$(function () {
    $('#cbtnComplete').click(function (e) {
        e.preventDefault();
        View.OpenComplete();
    });

    $('#cbtnSwfFileAdd').click(function (e) {
        e.preventDefault();
        View.AttachFile();
    });

    $('.certificate-saved-file-delete').click(function (e) {
        e.preventDefault();
        View.DeleteSavedFile($(this).attr('data-file-guid'));
    });

    View.ApplyCompletionPolicy();
    if (View.HasCompletionContext() && $('#cbtnSwfFileAdd').length > 0) View.GetSpoolFileList();
});

View.HasCompletionContext = function () {
    if (!HasEducation || typeof SwfUploadGUID != 'string'
        || !/^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i.test(SwfUploadGUID)
        || SwfUploadGUID == '00000000-0000-0000-0000-000000000000') return false;
    if (typeof TargetUserID != 'undefined'
        && (!/^[0-9]+$/.test(String(TargetUserID)) || Number(TargetUserID) <= 0
            || Number(TargetUserID) > 2147483647)) return false;
    return true;
}

View.HasCompletionPolicy = function () {
    return typeof IsComplete == 'boolean' && typeof IsAdminConfirmed == 'boolean'
        && !View.confirmationUnknown;
}

View.CanRegisterAttachments = function () {
    return HasEducation && View.HasCompletionPolicy() && !IsComplete && !IsAdminConfirmed;
}

View.CanComplete = function () {
    return HasEducation && View.HasCompletionPolicy() && !IsComplete && !IsAdminConfirmed;
}

View.CanDeleteAttachments = function () {
    return HasEducation && View.HasCompletionPolicy() && !IsComplete && !IsAdminConfirmed;
}

View.CanCancelComplete = function () {
    return HasEducation && View.HasCompletionPolicy() && IsComplete && !IsAdminConfirmed;
}

View.CheckAttachmentPolicy = function () {
    if (View.CanRegisterAttachments()) return true;
    if (typeof IsAdminConfirmed == 'boolean' && IsAdminConfirmed)
        alert('관리자 확인이 완료된 내역은 첨부파일을 등록할 수 없습니다.');
    else if (typeof IsComplete == 'boolean' && IsComplete)
        alert('완료된 내역은 첨부파일을 등록할 수 없습니다.');
    else
        alert('상태를 확인할 수 없습니다. 화면을 새로고침해 주세요.');
    return false;
}

View.CheckCompletionPolicy = function () {
    if (View.CanComplete()) return true;
    if (typeof IsAdminConfirmed == 'boolean' && IsAdminConfirmed)
        alert('관리자 확인이 완료된 내역은 완료 처리할 수 없습니다.');
    else if (typeof IsComplete == 'boolean' && IsComplete)
        alert('이미 완료한 내역입니다.');
    else
        alert('상태를 확인할 수 없습니다. 화면을 새로고침해 주세요.');
    return false;
}

View.CheckAttachmentDeletionPolicy = function () {
    if (View.CanDeleteAttachments()) return true;
    if (typeof IsAdminConfirmed == 'boolean' && IsAdminConfirmed)
        alert('관리자 확인이 완료된 내역의 첨부파일은 삭제할 수 없습니다.');
    else if (typeof IsComplete == 'boolean' && IsComplete)
        alert('완료된 내역의 첨부파일은 삭제할 수 없습니다.');
    else
        alert('상태를 확인할 수 없습니다. 화면을 새로고침해 주세요.');
    return false;
}

View.CheckCancellationPolicy = function () {
    if (View.CanCancelComplete()) return true;
    if (typeof IsAdminConfirmed == 'boolean' && IsAdminConfirmed)
        alert('관리자 확인이 완료된 내역은 완료 취소할 수 없습니다.');
    else
        alert('상태를 확인할 수 없습니다. 화면을 새로고침해 주세요.');
    return false;
}

View.ApplyCompletionPolicy = function () {
    $('#cbtnComplete').toggle(!!View.CanComplete());
    $('#cbtnCancelComplete').toggle(!!View.CanCancelComplete());
    $('#cbtnSwfFileAdd').toggle(!!View.CanRegisterAttachments());
    $('.certificate-saved-file-delete, .certificate-file-delete').toggle(!!View.CanDeleteAttachments());
}

View.OpenComplete = function () {
    if (!View.HasCompletionContext() || View.IsBusy()) return;
    if (!View.CheckCompletionPolicy()) return;
    if (IsWaitingForCompletion) {
        alert('아직 대기 상태이므로 완료할 수 없습니다.');
        return;
    }
    if (View.CheckUploadWindow()) return;
    View.completing = true;
    View.SetBusyButtons();
    var certificateRequired = typeof CertificateRequired == 'undefined' || CertificateRequired !== false;
    if (certificateRequired) {
        var savedCount = View.GetSavedCertificateCount();
        if (savedCount == null) {
            alert('등록된 첨부파일 목록을 확인하지 못했습니다. 화면을 새로 고친 뒤 다시 시도해 주세요.');
            View.FinishComplete();
            return;
        }
        if (savedCount == 0) {
            alert('첨부파일을 1개 이상 등록해 주세요.');
            View.FinishComplete();
            return;
        }
    }
    try {
        if (typeof CompletePostBackTarget != 'string' || !CompletePostBackTarget
            || typeof __doPostBack != 'function') throw new Error('Completion postback is unavailable.');
        __doPostBack(CompletePostBackTarget, '');
    } catch (error) {
        alert('완료 처리에 실패했습니다. 다시 시도해 주세요.');
        View.FinishComplete();
    }
}

View.GetSavedCertificateCount = function () {
    if (typeof SavedCertificateCount != 'number' || !isFinite(SavedCertificateCount)
        || SavedCertificateCount < 0 || Math.floor(SavedCertificateCount) != SavedCertificateCount) return null;
    return SavedCertificateCount;
}

View.FinishComplete = function () {
    View.completing = false;
    View.SetBusyButtons();
}

View.IsBusy = function () {
    return !!(View.completing || View.savingAttachments || View.cancellingComplete || View.deletingFile
        || View.deleting || View.modifying || View.sendingAlarm || View.sendingMail || View.savingConfirmation);
}

View.SetBusyButtons = function () {
    var buttons = $('#cbtnComplete, #cbtnCancelComplete, #cbtnSwfFileAdd, .certificate-file-delete, .certificate-saved-file-delete, #cbtnDelete, #cbtnMod, #cbtnSendAlarm, #cbtnSendAllAlarm, #cbtnSendDelayedAlarm, #cbtnSendIncompleteMail, #cbtnSendDelayedMail, #cbtnCancel, #cbtnClose');
    if (View.IsBusy()) buttons.attr('aria-disabled', 'true');
    else buttons.removeAttr('aria-disabled');
    if (typeof View.IsConfirmationState == 'function')
        $('#chkAdminConfirm').prop('disabled', View.IsBusy() || !View.IsConfirmationState(View.confirmationState));
}

View.CheckUploadWindow = function () {
    if (!View.uploadWindow || View.uploadWindow.closed) return false;
    alert('파일 업로드 창에서 업로드를 마친 뒤 적용 버튼을 눌러 주세요.');
    View.uploadWindow.focus();
    return true;
}

View.AttachFile = function () {
    if (!View.HasCompletionContext() || View.IsBusy()) return;
    if (!View.CheckAttachmentPolicy()) return;
    if (View.CheckUploadWindow()) return;

    try {
        ParamArray = {
            'swfUploadGUID': SwfUploadGUID,
            'fileType': '*.*',
            'fileTypeDesc': 'All Files (*.*)',
            'fileCount': '0',
            'moduleName': 'EDUCATION',
            'callBackEvent': 'View.SaveUploadedFiles'
        };
        CallEncrypt(CallGetParam());
        View.uploadWindow = PopupWinOpen('/DeskPlusEIP/Upload/JSUpload/Main.aspx',
            'p=' + encodeURIComponent(GetEncryptValue), 'DPEducationCertificateUpload_' + SwfUploadGUID,
            '680', '510', 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
    } catch (error) {
        alert('파일 업로드 창을 열지 못했습니다. 팝업 차단 여부를 확인해 주세요.');
    }
}

// 업로더의 적용 콜백: Spool 파일을 DB에 등록하고 완료 상태는 변경하지 않습니다.
View.SaveUploadedFiles = function () {
    if (!View.HasCompletionContext() || View.IsBusy()) return;
    if (!View.CheckAttachmentPolicy()) return;

    View.savingAttachments = true;
    View.SetBusyButtons();
    View.RequestSpoolFileList(function (files) {
        if (files.length == 0) {
            View.FinishSaveAttachments();
            return;
        }
        if (!View.CheckAttachmentPolicy()) {
            View.FinishSaveAttachments();
            return;
        }
        try {
            if (typeof SaveAttachmentsPostBackTarget != 'string' || !SaveAttachmentsPostBackTarget
                || typeof __doPostBack != 'function') throw new Error('Attachment postback is unavailable.');
            __doPostBack(SaveAttachmentsPostBackTarget, '');
        } catch (error) {
            alert('첨부파일을 등록하지 못했습니다. 파일 업로드 창에서 적용 버튼을 다시 눌러 주세요.');
            View.FinishSaveAttachments();
        }
    }, function () {
        alert('첨부파일 목록을 확인하지 못했습니다. 파일 업로드 창에서 적용 버튼을 다시 눌러 주세요.');
        View.FinishSaveAttachments();
    });
}

View.FinishSaveAttachments = function () {
    View.savingAttachments = false;
    View.SetBusyButtons();
}

View.GetSpoolFileList = function () {
    if (!View.HasCompletionContext() || View.IsBusy() || !View.CanRegisterAttachments()) return;
    View.RequestSpoolFileList(function () { }, function () {
        alert('첨부파일 목록을 확인하지 못했습니다. 다시 시도해 주세요.');
    });
}

View.RequestSpoolFileList = function (success, error) {
    if (!View.HasCompletionContext()) { error(); return; }
    var requestSerial = ++View.spoolRequestSerial;
    View.SendSpoolRequest('/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList',
        { 'swfUploadGUID': SwfUploadGUID }, function (data) {
            if (requestSerial != View.spoolRequestSerial) return;
            var files = View.GetSpoolFiles(data);
            if (!files) { error(); return; }
            try {
                View.RenderSpoolFiles(files);
            } catch (renderError) {
                error();
                return;
            }
            success(files);
        }, function () {
            if (requestSerial == View.spoolRequestSerial) error();
        });
}

View.SendSpoolRequest = function (url, parameters, success, error) {
    if (!View.HasCompletionContext()) { error(); return; }
    try {
        ParamArray = parameters;
        CallEncrypt(CallGetParam());
        $.ajax({
            type: 'POST',
            url: url,
            data: JSON.stringify({ p: GetEncryptValue }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: success,
            error: error
        });
    } catch (requestError) {
        error();
    }
}

View.GetSpoolFiles = function (data) {
    var files = data && data.d;
    if (Object.prototype.toString.call(files) != '[object Array]') return null;
    for (var i = 0; i < files.length; i++) {
        if (!files[i] || !View.IsSpoolFileName(files[i].FileName)) return null;
    }
    return files;
}

View.IsSpoolFileName = function (fileName) {
    return typeof fileName == 'string' && fileName.length > 0
        && fileName != '.' && fileName != '..' && !/[\\\/\u0000]/.test(fileName);
}

View.RenderSpoolFiles = function (files) {
    var placeholder = $('#placeholder_certificate_pending').empty();
    if (files.length == 0) {
        View.SetBusyButtons();
        return;
    }
    var list = $('<ul class="table_typeT ul_typeT"></ul>');
    $.each(files, function (index, file) {
        var row = $('<li></li>');
        row.append($('<span></span>').text(file.FileName));
        row.append($('<span class="mount certificate-file-size"></span>').text(file.FileLengthKB));
        var remove = $('<a href="#" class="certificate-file-delete" title="삭제" aria-label="첨부파일 삭제"><i class="fas fa-times-circle"></i></a>');
        remove.click(function (e) {
            e.preventDefault();
            View.DeleteFile(file.FileName);
        });
        row.append(remove);
        list.append(row);
    });
    placeholder.append(list);
    View.ApplyCompletionPolicy();
    View.SetBusyButtons();
}

View.DeleteFile = function (fileName) {
    if (!View.HasCompletionContext() || View.IsBusy() || !View.IsSpoolFileName(fileName)) return;
    if (!View.CheckAttachmentDeletionPolicy()) return;
    if (View.CheckUploadWindow() || !confirm('첨부파일을 삭제하시겠습니까?')) return;

    View.deletingFile = true;
    View.SetBusyButtons();
    var requestSerial = ++View.spoolRequestSerial;
    View.SendSpoolRequest('/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList',
        { 'fileName': fileName, 'swfUploadGUID': SwfUploadGUID }, function (data) {
            if (requestSerial != View.spoolRequestSerial) return;
            var files = View.GetSpoolFiles(data);
            if (!files) {
                View.ErrorDeleteFile();
                return;
            }
            View.RenderSpoolFiles(files);
            View.FinishDeleteFile();
        }, View.ErrorDeleteFile);
}

// 미완료 내역의 등록 파일을 삭제하며, 서버 처리 후 새 목록을 표시합니다.
View.DeleteSavedFile = function (fileGuid) {
    if (!View.HasCompletionContext() || View.IsBusy()
        || !View.IsSpoolFileName(fileGuid) || fileGuid.length > 200) return;
    if (!View.CheckAttachmentDeletionPolicy()) return;
    if (typeof DeleteAttachmentFieldID != 'string' || !DeleteAttachmentFieldID
        || typeof DeleteAttachmentPostBackTarget != 'string' || !DeleteAttachmentPostBackTarget
        || typeof __doPostBack != 'function') return;
    var field = document.getElementById(DeleteAttachmentFieldID);
    if (!field || field.type != 'hidden') return;
    if (View.CheckUploadWindow() || !confirm('첨부파일을 삭제하시겠습니까?')) return;

    View.deletingFile = true;
    View.SetBusyButtons();
    ++View.spoolRequestSerial;
    try {
        $(field).val(fileGuid);
        __doPostBack(DeleteAttachmentPostBackTarget, '');
    } catch (error) {
        View.ErrorDeleteFile();
    }
}

View.ErrorDeleteFile = function () {
    try {
        alert('첨부파일을 삭제하지 못했습니다. 다시 시도해 주세요.');
    } finally {
        View.FinishDeleteFile();
    }
}

View.FinishDeleteFile = function () {
    View.deletingFile = false;
    View.SetBusyButtons();
}
