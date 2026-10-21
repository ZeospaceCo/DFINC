
AddComplete = { submitting: false, fileCount: -1, uploadWindow: null };

$(function () {
    $('#cbtnClose').click(function (e) {
        e.preventDefault();
        AddComplete.Close();
    });

    $('#cbtnSwfFileAdd').click(function (e) {
        e.preventDefault();
        AddComplete.AttachFile();
    });

    if ($('#cbtnSwfFileAdd').length > 0) AddComplete.GetSpoolFileList();
});

AddComplete.Close = function () {
    AddComplete.CloseUpload();
    if (parent.disablePopup) parent.disablePopup();
    else window.close();
}

AddComplete.CloseUpload = function () {
    if (AddComplete.uploadWindow && !AddComplete.uploadWindow.closed) AddComplete.uploadWindow.close();
}

AddComplete.AttachFile = function () {
    if (AddComplete.submitting) return;

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': '*.*',
        'fileTypeDesc': 'All Files (*.*)',
        'fileCount': '0',
        'moduleName': 'EDUCATION',
        'callBackEvent': 'AddComplete.GetSpoolFileList'
    };
    CallEncrypt(CallGetParam());

    AddComplete.uploadWindow = PopupWinOpen('/DeskPlusEIP/Upload/JSUpload/Main.aspx',
        'p=' + encodeURIComponent(GetEncryptValue), 'DPEducationCertificateUpload_' + SwfUploadGUID,
        '680', '510', 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

AddComplete.GetSpoolFileList = function () {
    AddComplete.fileCount = -1;
    ParamArray = { 'swfUploadGUID': SwfUploadGUID };
    CallEncrypt(CallGetParam());

    AjaxService('/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList',
        "{p:'" + GetEncryptValue + "'}", AddComplete.SuccessSpoolFileList);
}

AddComplete.SuccessSpoolFileList = function (data) {
    var files = data && data.d;
    if (!files || typeof files.length != 'number') return;

    AddComplete.fileCount = files.length;
    var list = $('<ul class="table_typeT ul_typeT"></ul>');
    $.each(files, function (index, file) {
        var row = $('<li></li>');
        row.append($('<span></span>').text(file.FileName));
        row.append($('<span class="mount certificate-file-size"></span>').text(file.FileLengthKB));
        var remove = $('<a href="#" class="certificate-file-delete" title="삭제" aria-label="수료증 파일 삭제"><i class="fas fa-times-circle"></i></a>');
        remove.click(function (e) {
            e.preventDefault();
            AddComplete.DeleteFile(file.FileName);
        });
        row.append(remove);
        list.append(row);
    });
    if (files.length == 0) list.append($('<li></li>').text('등록된 수료증 파일이 없습니다.'));
    $('#placeholder_file').empty().append(list);
}

AddComplete.DeleteFile = function (fileName) {
    if (AddComplete.submitting || !confirm('수료증 파일을 삭제하시겠습니까?')) return;

    AddComplete.fileCount = -1;
    ParamArray = { 'fileName': fileName, 'swfUploadGUID': SwfUploadGUID };
    CallEncrypt(CallGetParam());
    AjaxService('/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList',
        "{p:'" + GetEncryptValue + "'}", AddComplete.SuccessSpoolFileList);
}

AddComplete.Validate = function () {
    if (AddComplete.submitting) return false;
    if (AddComplete.uploadWindow && !AddComplete.uploadWindow.closed) {
        alert('파일 업로드 창에서 업로드를 마친 뒤 적용 버튼을 눌러 주세요.');
        AddComplete.uploadWindow.focus();
        return false;
    }

    AddComplete.GetSpoolFileList();
    if (AddComplete.fileCount < 0) {
        alert('수료증 파일 목록을 확인하지 못했습니다. 다시 시도해 주세요.');
        return false;
    }
    if (AddComplete.fileCount < 1) {
        alert('수료증 파일을 1개 이상 등록해 주세요.');
        return false;
    }
    if (!confirm('수료증을 등록하고 이 교육을 완료 처리하시겠습니까?')) return false;

    AddComplete.submitting = true;
    $('#btnComplete').attr('aria-disabled', 'true').val('등록 중...');
    return true;
}

