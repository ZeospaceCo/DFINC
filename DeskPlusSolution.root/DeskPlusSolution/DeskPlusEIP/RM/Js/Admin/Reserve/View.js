View = {}

/**************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            View.Del();
        });

        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            View.Mod();
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        //목록
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            View.GoList();
        });

        //댓글 추가
        $('#cbtnShortReply').click(function (e) {
            e.preventDefault();
            View.AddShortReply();
        });

        //첨부파일
        View.GetFileList();

        //댓글
        View.GetShortReplyList();

        //공유자
        View.GetShareUserList();

        //거부사유
        if (Usable == "2") {
            $("#dvReject").attr("style", "display");
            $("#dvRejectList").attr("style", "display");  
            View.GetRejectList();
        }

        // 구분값(이미지) 선택금지
        $("[name=RblPattern]:not(:checked)").attr('disabled', 'disabled');

    });
});

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 첨부 파일 리스트 (웹서비스 호출)
**********************************************************************************************/
View.GetFileList = function () {
    ParamArray = {
        'reservationID': ParentReservationID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessFileList");
    var url = "/DeskPlusEIP/RM/Ws/Admin/Reserve/View.asmx/GetFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 첨부 파일 리스트 (웹서비스 값 받기)
**********************************************************************************************/
View.SuccessFileList = function (data, status) {
    $('#phFile').setTemplateURL('/DeskPlusEIP/RM/Jt/Content/JTFile.htm?seq=' + Guids());
    $('#phFile').processTemplate(data.d);

    /*
    setTimeout(function () {
        View.FileResource();
    }, 100);
    */
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 리스트 메뉴 다국어 처리 - 사용X
**********************************************************************************************/
View.FileResource = function () {
    $("#j_fileName")[0].innerHTML = R_FileName;
    $("#j_fileType")[0].innerHTML = R_FileType;
    $("#j_fileLength")[0].innerHTML = R_FileLength;
    $("#j_download")[0].innerHTML = R_Download;

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
View.FileDownload = function (filePath) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
    //WindowObjectReference = window.open(attachFilePath, "FileDownload", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");
}

/**************************************************************
작성자 : famo23
작성일자 : 2017-03-08
작성목적 : 예약 자원 삭제
**************************************************************/
View.Del = function () {
    if (confirm(R_DelResources)) {
        if (ReservationID != ParentReservationID) {
            var url = "/DeskPlusEIP/RM/Content/DelRepeat.aspx";
            PopupWin(400, 225, url, "50", "", true);
        }
        else {
            View.GetRepeat();
        }
    }
}

/**************************************************************
작성자 : famo23
작성일자 : 2017-03-09
작성목적 : 자원 수정
**************************************************************/
View.Mod = function () {
    ParamArray = {
        'reservationID': ReservationID,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': TagGubun
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusEIP/RM/Admin/Reserve/ModN.aspx?p=" + GetEncryptValue;
}

/**************************************************************
작성자 : famo23
작성일자 : 2017-03-08
작성목적 : 목록
**************************************************************/
View.GoList = function () {

    if (TagGubun == "usable0") {
        ParamArray = {
            'currentPage': CurrentPage,
            'searchField': SearchField,
            'searchValue': SearchValue
        };

        CallEncrypt(CallGetParam());

        location.href = "/DeskPlusEIP/RM/Admin/Reserve/Usable0List.aspx?p=" + GetEncryptValue;
    }
    else if (TagGubun == "usable1") {
        ParamArray = {
            'currentPage': CurrentPage,
            'searchField': SearchField,
            'searchValue': SearchValue
        };

        CallEncrypt(CallGetParam());

        location.href = "/DeskPlusEIP/RM/Admin/Reserve/Usable1List.aspx?p=" + GetEncryptValue;
    }
    else if (TagGubun == "usable2") {
        ParamArray = {
            'currentPage': CurrentPage,
            'searchField': SearchField,
            'searchValue': SearchValue
        };

        CallEncrypt(CallGetParam());

        location.href = "/DeskPlusEIP/RM/Admin/Reserve/Usable2List.aspx?p=" + GetEncryptValue;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 반복 일정 삭제
**********************************************************************************************/
function RepeatDel(_arg) {
    $("#hdRepeatSel").val(_arg);

    __doPostBack('lbtnConfirm', '');
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 반복일정인지 체크 (웹서비스 호출)
**********************************************************************************************/
View.GetRepeat = function () {
    ParamArray = {
        'reservationID': ReservationID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessGetRepeat");
    var url = "/DeskPlusEIP/RM/Ws/Admin/Reserve/View.asmx/GetRepeat";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 반복일정인지 체크 (웹서비스 값 받기)
**********************************************************************************************/
View.SuccessGetRepeat = function (data, status) {
    if (data.d) {
        var url = "/DeskPlusEIP/RM/Content/DelRepeat.aspx";
        PopupWin(400, 225, url, "50", "", true);
    }
    else {
        __doPostBack('lbtnConfirm', '');
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 인쇄
***************************************************/
View.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 댓글  리스트 호출
**********************************************************************************************/
View.GetShortReplyList = function () {
    ParamArray = {
        'reservationID': ReservationID
    };

    CallEncrypt(CallGetParam())

    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessShortReplyList");
    var url = "/DeskPlusEIP/RM/Ws/Admin/Reserve/View.asmx/GetShortReplyList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 댓글 리스트 성공시 페이지 표시
**********************************************************************************************/
View.SuccessShortReplyList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/RM/Jt/Admin/Reserve/JTReply.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    setTimeout(function () {
        View.ReplyResource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 다국어 처리
**********************************************************************************************/
View.ReplyResource = function () {

    /*
    $("#j_Name")[0].innerHTML = R_Name;
    $("#j_Content")[0].innerHTML = R_Contents;
    $("#j_RegDate")[0].innerHTML = R_WriteDate;    

    if ($(".NoReplyListData") != undefined) { $(".NoReplyListData").text(R_NoContent); }
    */
 
    if ($(".ModMsg") != undefined) { $(".ModMsg").text(R_Mod); }
    if ($(".DelMsg") != undefined) { $(".DelMsg").text(R_Del); }

    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}

/**************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 댓글
***************************************************/
View.AddShortReply = function () {
    ParamArray = {
        'reservationID': ReservationID,
        'parentReservationID': ParentReservationID
    };

    CallEncrypt(CallGetParam())

    var url = "/DeskPlusEIP/RM/Content/ShortReply.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "-220", false);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 댓글 수정
**********************************************************************************************/
View.ModShortReply = function (shortReplyID) {
    ParamArray = {
        'shortReplyID': shortReplyID,
        'reservationID': ReservationID,
        'parentReservationID': ParentReservationID
    };

    CallEncrypt(CallGetParam())
    var url = "/DeskPlusEIP/RM/Content/ShortReplyMod.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "", true);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 댓글 삭제
**********************************************************************************************/
View.DelReple = function (shortReplyID) {
    if (confirm(R_ApplyDelete)) {
        ParamArray = {
            'shortReplyID': shortReplyID,
            'reservationID': ReservationID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessDelShortReply");
        var url = "/DeskPlusEIP/RM/Ws/Admin/Reserve/View.asmx/DelShortReply";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 댓글 삭제 성공
**********************************************************************************************/
View.SuccessDelShortReply = function (data, status) {
    if (data.d) {
        View.GetShortReplyList();
    }
}



/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 공유자  리스트 호출
**********************************************************************************************/
View.GetShareUserList = function () {
    ParamArray = {
        'reservationID': ParentReservationID
    };

    CallEncrypt(CallGetParam())

    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessShareUserList");
    var url = "/DeskPlusEIP/RM/Ws/Admin/Reserve/View.asmx/GetShareUserList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 공유자 리스트 성공시 페이지 표시
**********************************************************************************************/
View.SuccessShareUserList = function (data, status) {

    if (data.d.length > 0) {
        for (var i = 0; i < data.d.length; i++) {
            var vUserInfoLink = "";
            if (data.d[i].Kind == "M") {
                vUserInfoLink = "<span>(" + R_AttendeeType2 + ")</span><a onclick='AccountInfo(" + data.d[i].UserID + ");return false;'>" + data.d[i].NameBase + "</a>";
            }
            else {
                vUserInfoLink = "<span>(" + R_AttendeeType1 + ") " + data.d[i].NameBase + "</span>";
            }

            $("#phShareList").append(
		          "<li>"
			    + vUserInfoLink
                + "</li>"
	        );

            /*
            $("#contentTable_Share").append(
                "<tr>"
                + "<td class='center'>"
                + vUserInfoLink
                + "</td>"
                + "<td class='center'>"
                + ((data.d[i].Kind == "M") ? R_AttendeeType2 : R_AttendeeType1)
                + "</td>"
                + "</tr>"
            );
            */
        }
    }

    /*
    else {
        $("#contentTable_Share").append(
            "<tr>"
            + "<td colspan='2' class='center'>"
            + R_NoContent
            + "</td>"
            + "</tr>"
        );
    }
    */
}



/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 거부사유 리스트 호출
**********************************************************************************************/
View.GetRejectList = function () {
    ParamArray = {
        'reservationID': ReservationID
    };

    CallEncrypt(CallGetParam())

    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessRejectList");
    var url = "/DeskPlusEIP/RM/Ws/Admin/Reserve/View.asmx/GetRejectList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 거부사유 리스트 성공시 페이지 표시
**********************************************************************************************/
View.SuccessRejectList = function (data, status) {
    $('#phReject').setTemplateURL('/DeskPlusEIP/RM/Jt/Content/JTReject.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phReject').processTemplate(data.d);

    /*
    setTimeout(function () {
        View.RejectResource();
    }, 100);
    */
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 다국어 처리 - 사용X
**********************************************************************************************/
View.RejectResource = function () {

    $("#r_Name")[0].innerHTML = R_Name;
    $("#r_Content")[0].innerHTML = R_Contents;
    $("#r_RegDate")[0].innerHTML = R_WriteDate;

    if ($(".NoRejectListData") != undefined) { $(".NoRejectListData").text(R_NoContent); }
}