View = {}

$(function () {
    $(document).ready(function () {

        //수정
        $('#CbtnEdit').click(function (e) {
            e.preventDefault();
            View.Edit();
        });
        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();


            if ($("#hdRepeatChk").val() == "") {
                View.Delete();
            }
            else {
                var url = "/DeskPlusEIP/Pim/Schedule/ConfirmRepeat.aspx";
                PopupWin(400, 270, url, "-500", "-10", false);
            }

        });
        //취소
        $('#CbtnCancel').click(function (e) {
            e.preventDefault();

            View.Cancel();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();

            View.Close();
        });
        

        //인쇄
        $('#CbtnPrint').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        //댓글 추가
        $('#cbtnShortReply').click(function (e) {
            e.preventDefault();
            View.AddShortReply();
        });


        // 첨부파일
        View.GetAttachFileList();
        // 댓글
        View.GetShortReplyList();
        View.Resource();

        if (parent.FrameHeightControl != null) {
            var yHeight = 900;
            parent.FrameHeightControl(yHeight);
        }
    });
});

/******************
인쇄버튼
******************/

View.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}


/******************
취소버튼
******************/
View.Cancel = function () {

    if (PageScope == "ListDay.aspx" || PageScope == "ListMonth.aspx" || PageScope == "ListWeek.aspx") {
        ParamArray = {
            'PageScope': PageScope,
            'Date': paraDate,
            'Opentype': paraOpentype
        }
        CallEncrypt(CallGetParam());
        location.href = "/DeskPlusEIP/Pim/Schedule/ScheduleMain.aspx?p=" + GetEncryptValue;
        return;
    }



    if (PageUrl == "") {        
        PageUrl = "/DeskplusEip/Pim/Schedule/ScheduleMain.aspx";
        ParamArray = {
            'Page': '',
            'Date': ''
            , 'PageScope': PageScope
        };
    }
    else if (PageUrl == "ListTotal.aspx") {
        PageUrl = "/DeskplusEip/Pim/Schedule/" + PageUrl;
        ParamArray = {
            'Distance': s_Distance,
            'inquryPerson': s_inquryPerson,
            'DatePeriod': s_DatePeriod,
            'SearchKind': s_SearchKind,
            'SerachValue': s_SerachValue,
            'StartDate': s_StartDate,
            'EndDate': s_EndDate,
            'PageSize': s_PageSize,
            'CurrentPage': s_CurrentPage
            , 'PageScope': PageScope
        };
    }
    else if (PageUrl == "AllSchedule.aspx") {
        ParamArray = {
            'UserList': s_UserId,
            'StartDate': s_StartDate,
            'EndDate': s_EndDate,
            'title': s_SearchKind,
            'name': s_SerachValue
        };

    }
    else {
        if (PageUrl == "ListSearch.aspx") {
            PageUrl = "/DeskplusEip/Pim/Schedule/" + PageUrl;
            ParamArray = {
                'Distance': s_Distance,
                'inquryPerson': s_inquryPerson,
                'DatePeriod': s_DatePeriod,
                'SearchKind': s_SearchKind,
                'SerachValue': s_SerachValue,
                'StartDate': s_StartDate,
                'EndDate': s_EndDate,
                'PageSize': s_PageSize,
                'CurrentPage': s_CurrentPage
                , 'PageScope': PageScope
            };
        }
        else {
            
            PageUrl = "/DeskplusEip/Pim/Schedule/" + PageUrl;
            ParamArray = {
                'Page': '',
                'Date': ''
                , 'PageScope': PageScope
            };
        }
    }
    CallEncrypt(CallGetParam());

    location.href = PageUrl + "?p=" + GetEncryptValue;
}

/******************
수정버튼
******************/
View.Edit = function () {

    if (PageUrl == "") {

        PageUrl = "ScheduleMain.aspx";
    }

    if (PageUrl == "ListSearch.aspx") {
        ParamArray = {
            'Distance': s_Distance,
            'inquryPerson': s_inquryPerson,
            'DatePeriod': s_DatePeriod,
            'SearchKind': s_SearchKind,
            'SerachValue': s_SerachValue,
            'StartDate': s_StartDate,
            'EndDate': s_EndDate,
            'PageSize': s_PageSize,
            'CurrentPage': s_CurrentPage,
            'Page': PageUrl,
            'ScheduleID': ScheduleID,
            'DisplayID': DisplayID
        };
    }
    else {
        
        ParamArray = {
            'ScheduleID': ScheduleID,
            'DisplayID': DisplayID,
            'PageScope': PageScope,
            'Date': paraDate,
            'Opentype': paraOpentype
        };
    }
    CallEncrypt(CallGetParam());

    location.href = "ModN.aspx?p=" + GetEncryptValue;
    return;

    //이코드는 뭐하는 코드지??
    ParamArray = {
        'ScheduleID': ScheduleID,
        'Page': PageUrl,
        'Date': Dateinfo
    };

    CallEncrypt(CallGetParam());
    location.href = "ModN.aspx?p=" + GetEncryptValue;

}

/******************
삭제버튼
******************/
View.Delete = function () {
    if (confirm(R_ReallyApplyDelete)) {
        


        ParamArray = {
            'DisplayID': DisplayID,
            'CompanyID': CompanyID,
            'SEL': $("#hdRepeatSel").val()
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessDel");
        var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/DelScheduleDis";

        AjaxService(url, data, obj);
    }
}

View.SuccessDel = function (data, status) {

    View.Cancel();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
View.FileDownload = function (filePath, fileName, fileGuid) {

    ParamArray = {
        'filePath': filePath + fileGuid,
        'fileName': fileName
    };

    CallEncrypt(CallGetParam());

    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + GetEncryptValue;
    window.open(attachFilePath);
    //WindowObjectReference = window.open(attachFilePath, "FileDownload", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 파일  리스트 호출
**********************************************************************************************/
View.GetAttachFileList = function () {

    ParamArray = {
        'ScheduleID': ScheduleID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessAttachFile");
    
    var url = "/DeskPlusEIP/Pim/Ws/Common/AttachFile.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
View.SuccessAttachFile = function (data, status) {

    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Pim/Jt/Anniversary/JTAttachFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
View.Resource = function () {

    if ($(".NoAuthListData") != undefined) { $(".NoAuthListData").text(R_SecurityMsg); }

    if ($(".ModMsg") != undefined) { $(".ModMsg").text(R_Modify); }
    if ($(".DelMsg") != undefined) { $(".DelMsg").text(R_Del); }

    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 댓글
***************************************************/
View.AddShortReply = function () {

    ParamArray = {
        'scheduleID': ScheduleID,
        'displayID': DisplayID
    };

    CallEncrypt(CallGetParam())

    //var url = "ShortReplyList.aspx?p=" + GetEncryptValue;
    var url = "ShortReply.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "-220", false);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-22
작성목적 : 댓글 수정화면 보여주기
**********************************************************************************************/
View.ModShortReply = function (scheduleID, replyID) {

    ParamArray = {
        'scheduleID': scheduleID,
        'replyID': replyID
    };

    CallEncrypt(CallGetParam())
    var url = "ShortReplyMod.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "", true);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-22
작성목적 : 댓글 삭제
**********************************************************************************************/
View.DelReple = function (replyID) {

    if (confirm(R_ReallyApplyDelete)) {

        ParamArray = {
            'scheduleID': ScheduleID,
            'replyID': replyID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessDelShortReply");
        var url = "/DeskPlusEIP/Pim/Ws/Schedule/View.asmx/DelShortReply";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 댓글 삭제 성공
**********************************************************************************************/
View.SuccessDelShortReply = function (data, status) {
    if (data.d) {
        View.GetShortReplyList();
        View.Resource();
    }
    else
        alert(R_MsgError);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 댓글  리스트 호출
**********************************************************************************************/
View.GetShortReplyList = function () {
    ParamArray = {
        'scheduleID': ScheduleID
    };

    CallEncrypt(CallGetParam())

    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessShortReplyList");
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/View.asmx/GetShortReplyList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 댓글 리스트 성공시 페이지 표시
**********************************************************************************************/
View.SuccessShortReplyList = function (data, status) {
    $('#placeholder_shortReply').setTemplateURL('/DeskPlusEIP/Pim/Jt/Schedule/JTReple.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder_shortReply').processTemplate(data.d);
}


/**************************************************
작성자   : famo23
작성일자 : 2012-12-14
작성목적 : 목록으로 가기 
***************************************************/
View.Close = function () {

    self.close();
    return;
}