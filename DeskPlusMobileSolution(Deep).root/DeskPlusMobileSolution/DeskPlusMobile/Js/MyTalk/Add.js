Add = {}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    if (UploadCheck == "Y") {
        jQuery.each($("#btPCAddFile"), function () { this.style.display = ""; });
    }

    $("#btPCAddFile").click(function () {
        $("#upload").click();
    });

    /*
    if (MobileType == "Android") {
    //안드로이드앱에서 접속시에는 앱업로드기능 호출
    jQuery.each($("#btPCAddFile"), function () {
    this.style.display = "";
    });

    $('#btAddFile').click(function (e) {
    Add.addFile();
    });
    }
    else {
    //댓글을 쓰는 경우에는 파일추가 이미지 안 보이게 처리
    if (UploadCheck == "Y") {
    jQuery.each($("#btPCAddFile"), function () {
    this.style.display = "";
    });
    }
    }
    */


    $('#form1').ajaxForm({
        dataType: 'text',
        beforeSerialize: function () {
            //$('#data').attr('value', '야호');
        },
        beforeSubmit: function () {
            $('#fileList').html('uploading...');
        },

        success: function (data) {
            Add.getSpoolList();

            // data = data.replace(/[<][^>]*[>]/gi, '');
            // var jData = JSON.parse(data);
            //$('#fileList').html('Success - ' + jData.result);
        },
        error: function (data) {
            NoticeShow("!", R_ProcessingError);
            LoadingShow(false);
        }

    });


    if (GroupID > 0) {
        $('#listTitle').html("<i class='fas fa-comments dropbtn '></i>" + R_Comment);
        jQuery.each($("#upload"), function () { this.style.display = "none"; });
    }
    else {
        $('#listTitle').html("<i class='fas fa-comments dropbtn '></i>" + R_WallPost);
    }

    $('#btCancel').click(function (e) {
        if (GroupID > 0) {
            Add.viewList();
        }
        else {
            Add.goBefore();
        }
    });

    $('#btAdd').click(function (e) {
        Add.add();
    });

    $('#before').click(function (e) {
        Add.goBefore();
    });

    $('#listTitle').click(function (e) {
        if (GroupID > 0) {
            Add.viewList();
        }
        else {
            Add.goBefore();
        }
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    //입력값 체크하기
    Add.MaxLang();

    if (FileCheck == "Y") {
        Add.getSpoolList();
    }

});

/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 글자 제한
***************************************************/
Add.MaxLang = function () {

    var onEditCallback = function (remaining) {

        $('.charsRemaining').text(remaining);

        if (remaining > 0) {
            $(this).css('border', '1px solid #d2d2d2');
        }
    }

    var onLimitCallback = function () {
        $(this).css('border', '1px solid #f00');
    }

    $('textarea[maxlength]').limitMaxlength({
        onEdit: onEditCallback,
        onLimit: onLimitCallback
    });
}


/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 목록 페이지 이동
***************************************************/
Add.goBefore = function () {

    ParamArray = {
        'PageNo': PageNo
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 취소 버튼 클릭시
***************************************************/
Add.viewList = function () {

    ParamArray = {
        'GroupID': GroupID,
        'PageNo': PageNo
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 댓글 등록 (웹서비스 호출)
***************************************************/
Add.add = function () {

    var tmpContent = $("#memo").val();

    if (jQuery.trim(tmpContent) == "") {
        NoticeShow("!", R_InputContent);
        return;
    }

    var IsSecurity = "1";
    if ($("#cbOpen").is(':checked')) {
        IsSecurity = "0";
    }

    var fileYN = "N";
    if (jQuery.trim($('#fileList').text()) != "") {
        fileYN = "Y";
    }

    LoadingShow(true);

    var url = "/DeskPlusMobile/MyTalk/Ws/SetMyTalk.asmx/AddMyTalk";

    //파라미터값 배열에 담기
    ParamArray = {
        'Content': encodeURIComponent(tmpContent),
        'IsSecurity': IsSecurity,
        'RUserID': RUserID,
        'GroupID': GroupID,
        'FileYN': fileYN,
        'SwfUploadGUID': SwfUploadGUID
    };

    var obj = eval("Add.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 댓글 등록 (웹서비스 값 받기)
***************************************************/
Add.succeeded = function (data, status) {

    if (data.d) {
        Add.goBefore();

        LoadingShow(false);
    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-03
작성목적 : 파일 선택
***************************************************/
function fileChange(obj) {

    NoticeHide();

    var tmpRtn = "";

    if (jQuery.trim($('#upload').val()) != "") {
        var file = obj.files;

        if (file) {
            for (var i = 0; i < file.length; i++) {

                if (UpFileSize < file[i].size) {
                    tmpRtn = "N";
                    var tmpMsg = bytesToSize(UpFileSize);
                    NoticeShow("!", tmpMsg + R_MaxFile);
                    return;
                }

                var fileExt = file[i].name.substr(file[i].name.lastIndexOf("."));
                fileExt = fileExt.toLowerCase();

                //.bmp,.tif,.jpeg,.jpg,.gif,.png,.pjpeg,.pjpg
                if (fileExt.indexOf("bmp") > 0 || fileExt.indexOf("tif") > 0 || fileExt.indexOf("jpeg") > 0 || fileExt.indexOf("jpg") > 0 || fileExt.indexOf("gif") > 0 || fileExt.indexOf("png") > 0 || fileExt.indexOf("pjpeg") > 0 || fileExt.indexOf("pjpg") > 0) {
                    tmpRtn = "Y";
                }
                else {
                    tmpRtn = "N";
                    NoticeShow("!", fileExt + R_CheckFileType);
                    return;
                }
            }

            jQuery.each($("#divFile"), function () { this.style.display = ""; });
            //$('#fileList').html(R_SelCntFile + " : " + file.length + "  <a href='#' onclick=\"Add.fileDel()\"><span>[" + R_Del + "]</span></a>");

            if (tmpRtn == "Y") {
                LoadingShow(true);
                $("#form1").submit();
            }
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-04
작성목적 : 파일삭제
***************************************************/
Add.fileDel = function () {
    $('#fileList').text("");
    $('#upload').val("");
    $('#upload').replaceWith($('#upload').clone(true));
    jQuery.each($("#divFile"), function () { this.style.display = "none"; });
}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-17
작성목적 : Spool 폴더 파일 리스트 가져오기 (웹서비스 호출)
***************************************************/
Add.getSpoolList = function () {

    var url = "/DeskPlusMobile/MyTalk/Ws/GetMyTalk.asmx/GetSpoolList";

    //파라미터값 배열에 담기
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    var obj = eval("Add.succeededSpool");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-17
작성목적 : Spool 폴더 파일 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
Add.succeededSpool = function (data, status) {

    $('#fileList').empty();

    if (data != null) {

        $.each(data.d, function (i, o) {
            $('#fileList').append(data.d[i].FilePath + " " + data.d[i].FileName + "  <img src='/DeskPlusMobile/Content/ko/Base/Img/ico_tree_listDel.gif' class='icon_Talk' onclick=\"javascript:Add.DelFile('" + data.d[i].FileName + "')\" /> <br/>");
        });

        if (data.d.length == 0) {
            jQuery.each($("#divFile"), function () { this.style.display = "none"; });
        }
        else {
            jQuery.each($("#divFile"), function () { this.style.display = ""; });
        }
    }

    if (FileCheck == "N") {
        $('#upload').val("");
        $('#upload').replaceWith($('#upload').clone(true));
    }

    LoadingShow(false);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-17
작성목적 : Spool 폴더 파일삭제
***************************************************/
Add.DelFile = function (fileName) {
    var url = "/DeskPlusMobile/MyTalk/Ws/SetMyTalk.asmx/DelSpoolFileList";

    //파라미터값 배열에 담기
    ParamArray = {
        'fileName': fileName,
        'swfUploadGUID': SwfUploadGUID
    };

    var obj = eval("Add.getSpoolList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}



/**************************************************
작성자   : famo23
작성일자 : 2015-11-10
작성목적 : 파일등록을 하기 위한 글임시저장 (웹서비스 호출)
***************************************************/
Add.addFile = function () {

    var dbCnt = 0;

    var tmpContent = $("#memo").val();
    if (jQuery.trim(tmpContent) != "") {
        dbCnt = 1;
    }

    var IsSecurity = "1";
    if ($("#cbOpen").is(':checked')) {
        IsSecurity = "0";
    }
    else {
        dbCnt = 1;
    }

    LoadingShow(true);

    if (dbCnt == "1") {

        var url = "/DeskPlusMobile/MyTalk/Ws/SetMyTalk.asmx/AddDraft";

        ParamArray = {
            'Content': encodeURIComponent(tmpContent),
            'IsSecurity': IsSecurity,
            'RUserID': RUserID,
            'GroupID': GroupID
        };

        var obj = eval("Add.succeededAddDraft");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxAsyncService(url, data, obj);
    }
    else {

        ParamArray = {
            'PageNo': PageNo,
            'RUserID': UserID,
            'GroupID': GroupID,
            'SwfUploadGUID': SwfUploadGUID,
            'FileCheck': 'Y'
        };

        CallEncrypt(CallGetParam());
        var beforeUrl = HttpType + MobileDOMAIN + "/DeskPlusMobile/MyTalk/Add.aspx?p=" + GetEncryptValue;

        document.location.href = beforeUrl + "&action=GetPicture&target=MyTalk&beforePath=" + escape(beforeUrl) + "&cID=" + CompanyID + "&uID=" + UserID + "&sID=" + SwfUploadGUID;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-11-10
작성목적 : 사진 올리기
***************************************************/
Add.succeededAddDraft = function (data, status) {

    if (data.d != "") {

        ParamArray = {
            'PageNo': PageNo,
            'RUserID': UserID,
            'GroupID': GroupID,
            'SwfUploadGUID': SwfUploadGUID,
            'FileCheck': 'Y',
            'DraftListID': data.d
        };

        CallEncrypt(CallGetParam());
        var beforeUrl = HttpType + MobileDOMAIN + "/DeskPlusMobile/MyTalk/Add.aspx?p=" + GetEncryptValue;

        document.location.href = beforeUrl + "&action=GetPicture&target=MyTalk&beforePath=" + escape(beforeUrl) + "&cID=" + CompanyID + "&uID=" + UserID + "&sID=" + SwfUploadGUID;

    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}


function goBeforePath(arg) {
    document.location.href = arg;
}

function bytesToSize(bytes) {
    var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    if (bytes == 0) return 'n/a';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    if (i == 0) return bytes + ' ' + sizes[i];
    return (bytes / Math.pow(1024, i)).toFixed(1) + ' ' + sizes[i];
}
