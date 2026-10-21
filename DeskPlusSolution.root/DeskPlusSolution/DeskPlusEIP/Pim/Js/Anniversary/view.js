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
            View.Delete();
        });
        //취소
        $('#CbtnCancel').click(function (e) {
            e.preventDefault();
            View.Cancel();
        });

        //인쇄
        $('#CbtnPrint').click(function (e) {
            e.preventDefault();

            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });


        View.GetAttachFileList();

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


    ParamArray = {        
        'Searchkey': Searchkey,
        'SearchValue': SearchValue,
        'CurrentPage': CurrentPage,
        'Starttime': Starttime,
        'Endtime': Endtime,
        'order': Order
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}

/******************
수정버튼
******************/
View.Edit = function () {

    ParamArray = {
        'ScheduleID': ScheduleID,
        'Searchkey': Searchkey,
        'SearchValue': SearchValue,
        'CurrentPage': CurrentPage,
        'Starttime': Starttime,
        'Endtime': Endtime,
        'order': Order
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
            'ScheduleID': ScheduleID,
            'CompanyID': CompanyID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessDel");
        
        var url = "/DeskPlusEIP/Pim/Ws/Anniversary/View.asmx/DelSchedule";
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
    
    var url = "/DeskPlusEIP/Pim/Ws/Anniversary/View.asmx/GetAttachFileList";
    
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


