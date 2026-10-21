List = {}

/**************************************************
작성자   : famo23
작성일자 : 2012-04-16
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    //업로드 권한이 없는 폴더는 파일추가 이미지 안 보이게 처리
    if (UploadCheck == "Y") {
        if ($('#btAddFile').length > 0) {

            $('#btAddFile').hide();
            $('#btPCAddFile').show();

            $("#btPCAddFile").click(function () {
                $("#upload").click();
            });
        }
    }
    else {
        $('#btAddFile').hide();
        $('#btPCAddFile').hide();
    }
    /*
    if (MobileType == "Android") {
    //안드로이드앱에서 접속시에는 앱업로드기능 호출
    $('#btAddFile').click(function (e) {
    List.addFile();
    });
    }
    else {
    //업로드 권한이 없는 폴더는 파일추가 이미지 안 보이게 처리
    if (UploadCheck == "Y") {
    if ($('#btAddFile').length > 0) {

    $('#btAddFile').hide();
    $('#btPCAddFile').show();

    $("#btPCAddFile").click(function () {
    $("#upload").click();
    });
    }
    }
    }
    */

    // 문서 리스트 가져오기
    setTimeout("List.getList();", 400);

    $('#btAll').click(function (e) {
        NoticeHide();
        List.checkAll();
    });

    $('#ddlOrder').change(function () {
        List.changeList();
    });

    $('#btDel').click(function (e) {
        List.del();
    });

    $('#btSend').click(function (e) {
        List.send();
    });

    $('#pvNum').click(function (e) {
        e.preventDefault();
        List.pvNum();
    });

    $('#nxNum').click(function (e) {
        e.preventDefault();
        List.nxNum();
    });


    $('#listTitle').click(function (e) {
        List.ParentList();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    if (window.history && window.history.pushState) {
        window.history.replaceState(null, null, '');
        $(window).on('popstate', function (event) {
            LoadingShow(false);
        });
    }

});



/**************************************************
작성자   : famo23
작성일자 : 2012-04-16
작성목적 : 파일 리스트 가져오기 (웹서비스 호출)
***************************************************/
List.getList = function () {
    var url = "/DeskPlusMobile/WebHard/Ws/GetWebHard.asmx/GetList";

    //파라미터값 배열에 담기
    ParamArray = {
        'FolderID': FolderID,
        'PageNo': PageNo,
        'PageSize': PageSize,
        'Order': $("#ddlOrder").val(),
        'ShareGubun': ShareGubun,
        'PublicGubun': PublicGubun
    };

    var obj = eval("List.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-04-16
작성목적 : 문서 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
List.succeeded = function (data, status) {
    if (AttachFileID.length > 0) {
        List.SetDSFile(AttachFileID);
    }

    $('#phWebHard').setTemplateURL('/DeskPlusMobile/WebHard/Jt/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phWebHard').processTemplate(data.d);

    setTimeout(function () {
        List.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-04-16
작성목적 : 다국어 처리
**********************************************************************************************/
List.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-04-16
작성목적 : 전체선택 / 전체해제
***************************************************/
List.checkAll = function () {

    if ($("#allcheck").text() == R_SelAll) {
        if ($("input:checkbox").length > 0) {
            $("#allcheck").text(R_SelNoAll);

            for (var i = 0; i < $("input:checkbox").length; i++) {
                $("input:checkbox")[i].checked = true;
            }
        }
    }
    else {
        if ($("input:checkbox").length > 0) {
            $("#allcheck").text(R_SelAll);

            for (var i = 0; i < $("input:checkbox").length; i++) {
                $("input:checkbox")[i].checked = false;
            }
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-04-16
작성목적 : 더보기 선택시 
***************************************************/
List.changeList = function () {

    ParamArray = {
        'FolderID': FolderID,
        'Order': $("#ddlOrder").val(),
        'ShareGubun': ShareGubun,
        'PublicGubun': PublicGubun
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;

}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-26
작성목적 : 파일 삭제하기 권한확인
***************************************************/
List.del = function () {

    var tmpValue = "";
    for (var i = 0; i < $("input:checkbox").length; i++) {
        if ($("input:checkbox")[i].checked == true) {
            tmpValue += $("input:checkbox")[i].value + ",";
        }
    }

    if (tmpValue == "") {
        NoticeShow("!", R_DelNoFile);
        return;
    }

    var url = "/DeskPlusMobile/WebHard/Ws/GetWebHard.asmx/GetChk";

    ParamArray = {
        'tmpValue': tmpValue
    };

    var obj = eval("List.SuccessCheckDel");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-04-17
작성목적 : 파일 삭제하기  (웹서비스 호출)
***************************************************/
List.SuccessCheckDel = function (data, status) {

    var CheckCount = data.d;

    if (CheckCount == "0") {

        var tmpValue = "";

        for (var i = 0; i < $("input:checkbox").length; i++) {
            if ($("input:checkbox")[i].checked == true) {
                tmpValue += $("input:checkbox")[i].value + ",";
            }
        }

        action_popup.confirm(R_DelList, function (res) {
            if (res) {
                LoadingShow(true);

                var url = "/DeskPlusMobile/WebHard/Ws/SetWebHard.asmx/UpdTrash";

                //파라미터값 배열에 담기
                ParamArray = {
                    'DeleteYN': 'Y',
                    'FileID': tmpValue
                };

                var obj = eval("List.succeededDel");
                CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
                data = "{p:'" + GetEncryptValue + "'}";
                AjaxAsyncService(url, data, obj);
            }
        })
    }
    else {
        action_popup.alert(R_DeleteAuthority);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-04-17
작성목적 : 파일 삭제하기 (웹서비스 값 받기)
***************************************************/
List.succeededDel = function (data, status) {
    LoadingShow(false);

    ParamArray = {
        'FolderID': FolderID,
        'Order': $("#ddlOrder").val(),
        'ShareGubun': ShareGubun,
        'PublicGubun': PublicGubun
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2012-04-17
작성목적 : 파일 선택시 & 폴더 선택시
***************************************************/
List.gubun = function (tmpfolderID, fileID, path, fileName) {
    if (fileID == "0") {

        var tmpOrder = $("#ddlOrder option").index($("#ddlOrder option:selected"));

        ParamArray = {
            'FolderID': tmpfolderID,
            'Order': tmpOrder,
            'PublicGubun': PublicGubun
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;

    }
    else {
        List.getDS(fileID, path, fileName);
        //        if (DownCheck == 'Y') {
        //            List.goAttach(path, fileName);
        //        }
        //        else {
        //            alert(R_DownloadAuthority);
        //        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-04-17
작성목적 : 첨부파일 읽기 (웹서비스 호출)
***************************************************/
List.goAttach = function (path, fileName) {

    var url = "/DeskPlusMobile/Common/Ws/GetCommon.asmx/CopyAttach";

    //파라미터값 배열에 담기
    ParamArray = {
        'Path': path,
        'FileName': encodeURIComponent(fileName)
    };

    var obj = eval("List.succeededGoAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}

/**************************************************
작성자   : famo23
작성일자 : 2012-04-17
작성목적 : 첨부파일 읽기 (웹서비스 값 받기)
***************************************************/
List.succeededGoAttach = function (data, status) {
    if (data.d == "!NULL") {
        NoticeShow("!", R_ProcessingError);
        return;
    }
    else if (data.d == "FAIL") {
        NoticeShow("!", R_NoDownload);
        return;
    }
    else {
        var tmpAttachName = data.d;
        var lastidx = tmpAttachName.lastIndexOf(".");
        var tmpFiletype = tmpAttachName.substring(lastidx + 1, tmpAttachName.length);

        if (tmpFiletype.toLowerCase() != "mht") {
            if (MobileType == "ios") {

                ParamArray = {
                    'companyID': CompanyID,
                    'userID': UserID,
                    'attachName': encodeURIComponent(tmpAttachName)
                };

                CallEncrypt(CallGetParam());

                var url = HttpType + MobileDOMAIN + "/DeskPlusMobile/Common/AttachUrl.aspx?p=" + GetEncryptValue;
                //window.webkit.messageHandlers.open_safari.postMessage(url);
                openSafari(url);
            }
            else {
                location.href = "/DeskPlusMobile/FileData/" + CompanyID + "/users/" + UserID + "/Spool/Mobile/" + encodeURIComponent(tmpAttachName);
            }
        }
        else {
            ParamArray = {
                'AttachName': encodeURIComponent(tmpAttachName)
            };

            CallEncrypt(CallGetParam());

            location.href = "/DeskPlusMobile/Common/AttachView.aspx?p=" + GetEncryptValue;
        }
    }
}

/**************************************************
작성자   : 이중호
작성일자 : 2018-04-13
작성목적 : 문서 닥스트리밍 첨부파일 저장하기 (웹서비스 호출)
***************************************************/
List.SetDSFile = function (fileID) {
    var url = "/DeskPlusMobile/WebHard/Ws/SetWebHard.asmx/SetDSFile";

    //파라미터값 배열에 담기
    ParamArray = {
        'FileID': fileID
    };
    var obj = eval("List.succeededSetDSFile");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, null);
}

/**************************************************
작성자   : 이중호
작성일자 : 2018-04-13
작성목적 : 문서 첨부파일 저장오기 (웹서비스 값 받기)
***************************************************/
List.succeededSetDSFile = function (data, status) {
    AttachFileID = "";
}

/**************************************************
작성자   : 이중호
작성일자 : 2018-04-13
작성목적 : 문서 닥스트리밍 첨부파일 가져오기 (웹서비스 호출)
***************************************************/
List.getDS = function (fileID, path, fileName) {
    var url = "/DeskPlusMobile/WebHard/Ws/GetWebHard.asmx/GetDSFile"

    //파라미터값 배열에 담기
    ParamArray = {
        'FileID': fileID
        , 'Path': path
        , 'FileName': fileName
        , 'TYPE': "WHD_FILE"
    };
    var obj = eval("List.succeededDS");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : 이중호
작성일자 : 2018-04-13
작성목적 : 문서 스트리밍
***************************************************/
List.succeededDS = function (data, status) {
    var attObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    if (attObj.dsDS.length > 0) {

        var url = "http://sp.deskplus.co.kr:9979/docviewer.melita";

        var form = document.createElement("form");
        var parm = new Array();
        var input = new Array();

        form.action = url;
        //    form.target = "_blank";
        form.method = "post";

        parm.push(['FILE_ID', attObj.dsDS[0].FileID]);
        parm.push(['client', 'mobile_web']);

        for (var i = 0; i < parm.length; i++) {
            input[i] = document.createElement("input");
            input[i].setAttribute("type", "hidden");
            input[i].setAttribute('name', parm[i][0]);
            input[i].setAttribute("value", parm[i][1]);
            form.appendChild(input[i]);
        }

        document.body.appendChild(form);
        form.submit();
    } else {
        if (DownCheck == 'Y') {
            List.goAttach(ParamArray.Path, ParamArray.FileName);
        }
        else {
            action_popup.alert(R_DownloadAuthority);
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-04-17
작성목적 : 상위 폴더로 가기
***************************************************/
List.ParentList = function () {
    if (ShareGubun == "N") {

        if (ParentFolderID != 0 && ParentFolderID != -6) {

            var tmpOrder = $("#ddlOrder option").index($("#ddlOrder option:selected"));

            ParamArray = {
                'FolderID': ParentFolderID,
                'Order': tmpOrder,
                'PublicGubun': PublicGubun
            };

            CallEncrypt(CallGetParam());

            location.href = "List.aspx?p=" + GetEncryptValue;
        }
        else {
            location.href = "Folder.aspx";
        }
    }
    else {
        location.href = "Share.aspx";
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-04-18
작성목적 : 메일 쓰기
***************************************************/
List.send = function () {

    var tmpValue = "";
    for (var i = 0; i < $("input:checkbox").length; i++) {
        if ($("input:checkbox")[i].checked == true) {
            tmpValue += $("input:checkbox")[i].value + ",";
        }
    }

    if (tmpValue == "") {
        NoticeShow("!", R_SendNoFile);
        return;
    }

    ParamArray = {
        'FolderID': FolderID,
        'PageNo': PageNo,
        'Order': $("#ddlOrder").val(),
        'ShareGubun': ShareGubun,
        'FileID': tmpValue,
        'PublicGubun': PublicGubun
    };

    CallEncrypt(CallGetParam());

    location.href = "Send.aspx?p=" + GetEncryptValue;

}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-03
작성목적 : 파일 선택
***************************************************/
function fileChange(obj) {

    NoticeHide();

    if (ShareGubun != "Y") {

        var fileSizePerUser = "0";

        if (PrivateQuota == "0") {
            fileSizePerUser = "0";
        }
        else {
            if (PublicGubun == "Y") {
                fileSizePerUser = ((PrivateQuota / 1024) * 1048576) - UseCount;
                FileLength = FileLength * 1024;
            }
            else {
                fileSizePerUser = (PrivateQuota * 1048576) - UseCount;
            }
        }

        if (fileSizePerUser < 0) {
            NoticeShow("!", R_UploadCount);
            return;
        }
        else {
            var tmpRtn = "Y";
            var FileTotalSize = 0;
            var file = obj.files;

            if (file) {
                for (var i = 0; i < file.length; i++) {

                    if (file[i].size > UpFileSize) {
                        tmpRtn = "N";
                        var tmpMsg = bytesToSize(UpFileSize);
                        NoticeShow("!", tmpMsg + R_MaxFile);
                        return;
                    }

                    if (FileLength > 0) {
                        if (file[i].size > FileLength) {
                            tmpRtn = "N";
                            NoticeShow("!", R_MaxFile);
                            return;
                        }
                    }

                    var fileExt = file[i].name.substr(file[i].name.lastIndexOf("."));
                    fileExt = fileExt.toLowerCase();

                    $.each(BlockExt.split(','), function (idx) {
                        if (fileExt == ("." + BlockExt.split(',')[idx])) {
                            tmpRtn = "N";
                            NoticeShow("!", BlockExt + R_CheckFileType);
                            return;
                        }
                    });

                    FileTotalSize += file[i].size;
                }

                if (tmpRtn == "Y") {
                    if (fileSizePerUser == "0") {
                        LoadingShow(true);
                        $("#form1").submit();
                    }
                    else {
                        if (fileSizePerUser > FileTotalSize) {
                            LoadingShow(true);
                            $("#form1").submit();
                        }
                        else {
                            NoticeShow("!", R_UploadCount);
                            return;
                        }
                    }
                }
            }
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-20
작성목적 : 사진 올리기
***************************************************/
List.addFile = function () {

    if (ShareGubun != "Y") {

        var fileSizePerUser = "0";

        if (PrivateQuota == "0") {
            fileSizePerUser = "0";
        }
        else {
            if (PublicGubun == "Y") {
                fileSizePerUser = ((PrivateQuota / 1024) * 1048576) - UseCount;
                FileLength = FileLength * 1024;
            }
            else {
                fileSizePerUser = (PrivateQuota * 1048576) - UseCount;
            }
        }

        if (fileSizePerUser < 0) {
            NoticeShow("!", R_UploadCount);
            return;
        }
        else {
            document.location.href = location.href + "&action=GetPicture&target=WebHard&beforePath=" + escape(location.href) + "&cID=" + CompanyID + "&uID=" + UserID + "&fID=" + FolderID;
        }
    }
}



/**************************************************
작성자   : famo23
작성일자 : 2015-10-27
작성목적 : 이전 페이지 이동
***************************************************/
List.pvNum = function () {

    if (parseInt(PageNo) > 1) {
        PageNo = parseInt(PageNo) - 1;

        ParamArray = {
            'FolderID': FolderID,
            'PageNo': PageNo,
            'Order': $("#ddlOrder").val(),
            'ShareGubun': ShareGubun,
            'PublicGubun': PublicGubun
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-27
작성목적 : 다음 페이지 이동
***************************************************/
List.nxNum = function () {

    var tmpTotalNum = $("#TotalNum").html();

    if (parseInt(PageNo) < parseInt(tmpTotalNum)) {
        PageNo = parseInt(PageNo) + 1;

        ParamArray = {
            'FolderID': FolderID,
            'PageNo': PageNo,
            'Order': $("#ddlOrder").val(),
            'ShareGubun': ShareGubun,
            'PublicGubun': PublicGubun
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
    }
}

function goBeforePath(arg) {
    location.href = arg;
}


function bytesToSize(bytes) {
    var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    if (bytes == 0) return 'n/a';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    if (i == 0) return bytes + ' ' + sizes[i];
    return (bytes / Math.pow(1024, i)).toFixed(1) + ' ' + sizes[i];
}
