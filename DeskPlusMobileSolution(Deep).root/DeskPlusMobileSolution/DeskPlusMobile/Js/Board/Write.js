Write = {}

/**************************************************
작성자   : famo23
작성일자 : 2015-06-24
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $("#btPCAddFile").click(function () {
        $("#upload").click();
    });

    /*
    if (MobileType == "Android") {
    //안드로이드앱에서 접속시에는 앱업로드기능 호출
    jQuery.each($("#btAddFile"), function () { this.style.display = ""; });

    $('#btAddFile').click(function (e) {
    Write.addFile();
    });
    } else {
    jQuery.each($("#btPCAddFile"), function () { this.style.display = ""; });

    $("#btPCAddFile").click(function () {
    $("#upload").click();
    });
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
            Write.getSpoolList();
        },
        error: function (data) {
            NoticeShow("!", R_ProcessingError);
            LoadingShow(false);
        }

    });


    NowMonth = (NowMonth) < 10 ? "0" + NowMonth : NowMonth;
    NowDay = (NowDay) < 10 ? "0" + NowDay : NowDay;
    CalDate = NowYear + "-" + NowMonth + "-" + NowDay;
    CalMonth = NowYear + "-" + NowMonth;

    $('#btCancel').click(function (e) {
        Write.goBefore();
    });

    $('#btAdd').click(function (e) {
        Write.add();
    });

    $('#listTitle').click(function (e) {
        Write.goBefore();
    });

    //공지사항 등록 선택시, 기간 보이기
    $("input[id=cbOptionTop]").click(function () {
        if (this.checked) {
            $("#topDay").show();
        }
        else {
            $("#topDay").hide();
        }
    });

    //팝업설정 선택시, 팝업기간 보이기
    $("input[id=cbPopUp]").click(function () {
        if (this.checked) {
            $("#date_start").show();
            $("#date_end").show();
        }
        else {
            $("#date_start").hide();
            $("#date_end").hide();
        }
    });

    $('#tbStartDate').click(function (e) {
        CalCheck = "1";
        Write.getCalendar();
    });

    $('#tbEndDate').click(function (e) {
        CalCheck = "2";
        Write.getCalendar();
    });

    $('#pvMonth').click(function (e) {
        Write.pvMonth();
    });

    $('#nxMonth').click(function (e) {
        Write.nxMonth();
    });

    if (FileCheck == "Y") {

        if (DraftListID > 0) {

            if (OptionTop == "1") {
                $("#topDay").show();
            }

            if (PopUp == "1") {
                $("#date_start").show();
                $("#date_end").show();
            }
        }

        Write.getSpoolList();
    }

});

/**************************************************
작성자   : famo23
작성일자 : 2015-06-24
작성목적 : 목록 페이지 이동
***************************************************/
Write.goBefore = function () {

    ParamArray = {
        'MainCID': MainCID,
        'PCatID': PCatID,
        'PageNo': PageNo,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2015-06-26
작성목적 : 글등록 (웹서비스 호출)
***************************************************/
Write.add = function () {

    var title = $("#tbTitle").val();

    if (jQuery.trim(title) == "") {
        NoticeShow("!", R_InputSubject);
        return;
    }

    var tmpContent = $("#tbcontent").val();
    tmpContent = tmpContent.split("\n").join("<br>");

    if (jQuery.trim(tmpContent) == "") {
        NoticeShow("!", R_InputCont);
        return;
    }

    var IsBold = "0";
    if ($("#cbxBold").is(':checked')) {
        IsBold = "1";
    }

    var IsOptionModify = "0";
    if ($("#cbOptionModify").is(':checked')) {
        IsOptionModify = "1";
    }

    var IsOptionTop = "0";
    var IsOptionTopDay = "0";
    if ($("#cbOptionTop").is(':checked')) {
        IsOptionTop = "1";
        IsOptionTopDay = $("#ddlDay").val();
    }

    var IsPopUp = "";
    var startDate = "";
    var endDate = "";
    if ($("#cbPopUp").is(':checked')) {
        IsPopUp = "1";

        startDate = $("#tbStartDate").val();
        endDate = $("#tbEndDate").val();

        if ((startDate == "") || (endDate == "")) {
            NoticeShow("!", R_InputDate);
            LoadingShow(false);
            return;
        }

        var dtResponse = CompareDT(startDate, endDate, "lessequal", PreLang);

        if (dtResponse == false) {
            NoticeShow("!", R_StartDateEndDate);
            LoadingShow(false);
            return;
        }
    }

    var IsOptionAnonymity = "0";

    if (OptionAnonymity == "1") {
        if ($("#cbOptionAnonymity").is(':checked')) {
            IsOptionAnonymity = "1";
        }
    }

    //외부다운로드 제한 체크 했는지 여부
    var IsOptionDownload = "0";
    if ($("#cbCheckDownLoad").is(':checked')) {
        IsOptionDownload = "1";
    }

    var fileYN = "N";
    var screenShot = "";
    if (jQuery.trim($('#fileList').text()) != "") {
        fileYN = "Y";
        if ($(":input:radio[name=boaImg]:checked").length == 1) {
            screenShot = $(":input:radio[name=boaImg]:checked").val();
        }
    }

    LoadingShow(true);

    var url = "/DeskPlusMobile/Board/Ws/SetBoard.asmx/AddContent";

    //파라미터값 배열에 담기
    ParamArray = {
        'CID': PCatID,
        'CatHeader': $("#ddlCatHeader").val(),
        'FontColor': $("#ddlFontColor").val(),
        'FontSize': $("#ddlFontSize").val(),
        'IsBold': IsBold,
        'IsOptionModify': IsOptionModify,
        'IsOptionTop': IsOptionTop,
        'IsOptionTopDay': IsOptionTopDay,
        'IsPopUp': IsPopUp,
        'IsOptionAnonymity': IsOptionAnonymity,
        'OptionAlarm': OptionAlarm,
        'CategoryName': CategoryName,
        'StartDate': startDate,
        'EndDate': endDate,
        'FileYN': fileYN,
        'ScreenShot': screenShot,
        'SwfUploadGUID': SwfUploadGUID,
        'Title': encodeURIComponent(title),
        'Content': encodeURIComponent(tmpContent),
        'OptionDownload': IsOptionDownload

    };

    var obj = eval("Write.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-06-26
작성목적 : 글등록 (웹서비스 값 받기)
***************************************************/
Write.succeeded = function (data, status) {

    if (data.d != "") {
        SearchValue = "";
        PageNo = 1;

        Write.viewList(data.d);

        LoadingShow(false);
    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-06-26
작성목적 : 글 등록 한 이후에 보기페이지
***************************************************/
Write.viewList = function (_id) {

    ParamArray = {
        'MainCID': MainCID,
        'PCatID': PCatID,
        'PageNo': PageNo,
        'CID': PCatID,
        'ID': _id,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2015-06-26
작성목적 : 달력 만들기 (웹서비스 호출)
***************************************************/
Write.getCalendar = function () {
    jQuery.each($("#boa_calendar"), function () { this.style.display = ""; });

    arrDate = CalMonth.split('-');
    var tmpTimeList = CalMonth + "-01";
    tmpTimeList = DateYM(tmpTimeList, PreLang);

    $("#timeList").empty();
    $("#timeList").html(tmpTimeList);

    var url = "/DeskPlusMobile/Board/Ws/GetBoard.asmx/GetBasicCalendar";

    //파라미터값 배열에 담기
    ParamArray = {
        'Year': arrDate[0],
        'Month': arrDate[1]
    };

    var obj = eval("Write.succeededCalendar");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-06-26
작성목적 : 달력 만들기 (웹서비스 값 받기)
***************************************************/
Write.succeededCalendar = function (data, status) {
    $('#phCal').setTemplateURL('/DeskPlusMobile/Board/Jt/JTCalendar.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phCal').processTemplate(data.d);

    $('*').removeClass("busy choice");
    $('#' + CalDate).attr("class", "busy choice");

    setTimeout(function () {
        Write.CalendarResource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-06-26
작성목적 : 다국어 처리
**********************************************************************************************/
Write.CalendarResource = function () {

    $("#JSun")[0].innerHTML = R_Sun;
    $("#JMon")[0].innerHTML = R_Mon;
    $("#JTue")[0].innerHTML = R_Tue;
    $("#JWed")[0].innerHTML = R_Wed;
    $("#JThu")[0].innerHTML = R_Thu;
    $("#JFri")[0].innerHTML = R_Fri;
    $("#JSat")[0].innerHTML = R_Sat;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-06-26
작성목적 : 날짜 이동
***************************************************/
Write.pvMonth = function () {
    addMonth(CalMonth, -1);
}

Write.nxMonth = function () {
    addMonth(CalMonth, 1);
}


function addMonth(calMonth, v_month) {
    var arrDate = calMonth.split('-');
    var yyyy = arrDate[0];
    var mm = eval(arrDate[1] + "-1");

    var addDate = new Date(yyyy, eval(mm + '+' + v_month), 1);
    yyyy = addDate.getFullYear();
    mm = (addDate.getMonth() + 1) < 10 ? "0" + (addDate.getMonth() + 1) : (addDate.getMonth() + 1);

    var tmpTimeList = yyyy + "-" + mm + "-01";
    tmpTimeList = DateYM(tmpTimeList, PreLang);

    $("#timeList").empty();
    $("#timeList").html(tmpTimeList);

    CalMonth = yyyy + "-" + mm;

    Write.getCalendar();
}


/**************************************************
작성자   : famo23
작성일자 : 2015-06-26
작성목적 : 달력 이동이벤트
***************************************************/
Write.setList = function (calDate) {

    var tmpCalDate = DateYMD(calDate, PreLang);
    var tmpTimeList = DateYM(calDate, PreLang);

    $("#timeList").empty();
    $("#timeList").html(tmpTimeList);

    if (CalCheck == "1") {
        $("#tbStartDate").val(tmpCalDate);
    }
    else {
        $("#tbEndDate").val(tmpCalDate);
    }

    jQuery.each($("#boa_calendar"), function () { this.style.display = "none"; });
}


/**************************************************
작성자   : famo23
작성일자 : 2015-06-29
작성목적 : 파일 선택
***************************************************/
function fileChange(obj) {

    NoticeHide();

    var tmpRtn = "Y";

    if (jQuery.trim($('#upload').val()) != "") {
        var file = obj.files;

        if (file) {
            for (var i = 0; i < file.length; i++) {

                if (file[i].size > UpFileSize) {
                    tmpRtn = "N";
                    var tmpMsg = bytesToSize(UpFileSize);
                    NoticeShow("!", tmpMsg + R_MaxFile);
                    return;
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
            }

            if (tmpRtn == "Y") {
                LoadingShow(true);
                $("#form1").submit();
            }
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-06-29
작성목적 : 파일삭제
***************************************************/
Write.fileDel = function () {
    $('#fileList').text("");
    $('#upload').val("");
    $('#upload').replaceWith($('#upload').clone(true));
    jQuery.each($("#divFile"), function () { this.style.display = "none"; });
}


/**************************************************
작성자   : famo23
작성일자 : 2015-06-29
작성목적 : Spool 폴더 파일 리스트 가져오기 (웹서비스 호출)
***************************************************/
Write.getSpoolList = function () {

    var url = "/DeskPlusMobile/Board/Ws/GetBoard.asmx/GetSpoolList";

    //파라미터값 배열에 담기
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    var obj = eval("Write.succeededSpool");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-06-29
작성목적 : Spool 폴더 파일 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
Write.succeededSpool = function (data, status) {

    var Cnt = 0;
    $('#fileList').empty();

    if (data != null) {

        $.each(data.d, function (i, o) {

            if (Cnt == 0) {
                if (data.d[i].ImageType == "1") {
                    Cnt += 1;
                    $('#fileList').append("<input type='radio' name='boaImg' value='" + data.d[i].FileName + "' checked/>" + data.d[i].FileName + "  <img src='/DeskPlusMobile/Content/ko/Base/Img/ico_tree_listDel.gif' class='icon_Talk' onclick=\"javascript:Write.DelFile('" + data.d[i].FileName + "')\" /> <br/>");
                }
                else {
                    $('#fileList').append(data.d[i].FileName + "  <img src='/DeskPlusMobile/Content/ko/Base/Img/ico_tree_listDel.gif' class='icon_Talk' onclick=\"javascript:Write.DelFile('" + data.d[i].FileName + "')\" /> <br/>");
                }
            }
            else {
                if (data.d[i].ImageType == "1") {
                    Cnt += 1;
                    $('#fileList').append("<input type='radio' name='boaImg' value='" + data.d[i].FileName + "' />" + data.d[i].FileName + "  <img src='/DeskPlusMobile/Content/ko/Base/Img/ico_tree_listDel.gif' class='icon_Talk' onclick=\"javascript:Write.DelFile('" + data.d[i].FileName + "')\" /> <br/>");
                }
                else {
                    $('#fileList').append(data.d[i].FileName + "  <img src='/DeskPlusMobile/Content/ko/Base/Img/ico_tree_listDel.gif' class='icon_Talk' onclick=\"javascript:Write.DelFile('" + data.d[i].FileName + "')\" /> <br/>");
                }
            }
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
작성일자 : 2015-06-29
작성목적 : Spool 폴더 파일삭제
***************************************************/
Write.DelFile = function (fileName) {
    var url = "/DeskPlusMobile/Board/Ws/SetBoard.asmx/DelSpoolFileList";

    //파라미터값 배열에 담기
    ParamArray = {
        'fileName': fileName,
        'swfUploadGUID': SwfUploadGUID
    };

    var obj = eval("Write.getSpoolList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-11-09
작성목적 : 파일등록을 하기 위한 글임시저장 (웹서비스 호출)
***************************************************/
Write.addFile = function () {

    var dbCnt = 0;

    if ($("#ddlCatHeader").val() != "0") {
        dbCnt = 1;
    }

    if ($("#ddlFontColor").val() != "") {
        dbCnt = 1;
    }

    if ($("#ddlFontSize").val() != "") {
        dbCnt = 1;
    }

    var IsBold = "0";
    if ($("#cbxBold").is(':checked')) {
        IsBold = "1";
        dbCnt = 1;
    }

    var IsOptionModify = "0";
    if ($("#cbOptionModify").is(':checked')) {
        IsOptionModify = "1";
        dbCnt = 1;
    }

    var IsOptionAnonymity = "0";
    if (OptionAnonymity == "1") {
        if ($("#cbOptionAnonymity").is(':checked')) {
            IsOptionAnonymity = "1";
            dbCnt = 1;
        }
    }

    var IsOptionTop = "0";
    var IsOptionTopDay = "0";
    if ($("#cbOptionTop").is(':checked')) {
        IsOptionTop = "1";
        IsOptionTopDay = $("#ddlDay").val();
        dbCnt = 1;
    }


    var IsPopUp = "";
    var startDate = "";
    var endDate = "";
    if ($("#cbPopUp").is(':checked')) {
        IsPopUp = "1";
        dbCnt = 1;

        startDate = $("#tbStartDate").val();
        endDate = $("#tbEndDate").val();
    }

    var title = $("#tbTitle").val();
    if (jQuery.trim(title) != "") {
        dbCnt = 1;
    }

    var tmpContent = $("#tbcontent").val();
    if (jQuery.trim(tmpContent) != "") {
        dbCnt = 1;
    }

    LoadingShow(true);

    if (dbCnt == "1") {

        var url = "/DeskPlusMobile/Board/Ws/SetBoard.asmx/AddDraft";

        //파라미터값 배열에 담기
        ParamArray = {
            'CID': PCatID,
            'CatHeader': $("#ddlCatHeader").val(),
            'FontColor': $("#ddlFontColor").val(),
            'FontSize': $("#ddlFontSize").val(),
            'IsBold': IsBold,
            'IsOptionModify': IsOptionModify,
            'IsOptionTop': IsOptionTop,
            'IsOptionTopDay': IsOptionTopDay,
            'IsPopUp': IsPopUp,
            'IsOptionAnonymity': IsOptionAnonymity,
            'StartDate': startDate,
            'EndDate': endDate,
            'Title': encodeURIComponent(title),
            'Content': encodeURIComponent(tmpContent)
        };

        var obj = eval("Write.succeededAddDraft");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxAsyncService(url, data, obj);
    }
    else {

        ParamArray = {
            'MainCID': MainCID,
            'PCatID': PCatID,
            'PageNo': PageNo,
            'Sv': SearchValue.securityText(),
            'SwfUploadGUID': SwfUploadGUID,
            'FileCheck': 'Y'
        };

        CallEncrypt(CallGetParam());
        var beforeUrl = HttpType + MobileDOMAIN + "/DeskPlusMobile/Board/Write.aspx?p=" + GetEncryptValue;

        document.location.href = beforeUrl + "&action=GetPicture&target=Board&beforePath=" + escape(beforeUrl) + "&cID=" + CompanyID + "&uID=" + UserID + "&sID=" + SwfUploadGUID;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-11-09
작성목적 : 사진 올리기
***************************************************/
Write.succeededAddDraft = function (data, status) {

    if (data.d != "") {

        ParamArray = {
            'MainCID': MainCID,
            'PCatID': PCatID,
            'PageNo': PageNo,
            'Sv': SearchValue.securityText(),
            'SwfUploadGUID': SwfUploadGUID,
            'FileCheck': 'Y',
            'DraftListID': data.d
        };

        CallEncrypt(CallGetParam());
        var beforeUrl = HttpType + MobileDOMAIN + "/DeskPlusMobile/Board/Write.aspx?p=" + GetEncryptValue;

        document.location.href = beforeUrl + "&action=GetPicture&target=Board&beforePath=" + escape(beforeUrl) + "&cID=" + CompanyID + "&uID=" + UserID + "&sID=" + SwfUploadGUID;

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
