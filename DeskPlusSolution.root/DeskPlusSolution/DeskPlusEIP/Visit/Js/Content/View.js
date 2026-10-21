View = {}

/**************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //방문
        $('#cbtnAccept').click(function (e) {
            e.preventDefault();
            View.ModVisitType(1);
        });

        //거부
        $('#cbtnUnaccept').click(function (e) {
            e.preventDefault();
            View.ModVisitType(2);
        });

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

        if (ShareCheck == "Y") {
            if (VisitType == "0") {
                $("#unaccept").css("display", "");
                $("#accept").css("display", "");
            }
            else if (VisitType == "1") {
                $("#unaccept").css("display", "");
                $("#accept").css("display", "none");
            }
            else if (VisitType == "2") {
                $("#unaccept").css("display", "none");
                $("#accept").css("display", "");
            }
        }

        //기본공유자 
        View.GetShareList();

        //공유자 
        View.GetOpenList();

        //예약자원 
        View.GetRmList();

        //첨부파일 
        View.GetFileList();

    });
});

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 기본공유자 리스트 (웹서비스 호출)
**********************************************************************************************/
View.GetShareList = function () {

    ParamArray = {
        'reserveID': ReserveGroupID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessShareList");
    var url = "/DeskPlusEIP/Visit/Ws/Content/View.asmx/GetShareList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 기본공유자 리스트 (웹서비스 값 받기)
**********************************************************************************************/
View.SuccessShareList = function (data, status) {

    $('#phShare').setTemplateURL('/DeskPlusEIP/Visit/Jt/Content/JTShare.htm?seq=' + Guids());
    $('#phShare').processTemplate(data.d);

    /*
    setTimeout(function () {
        View.ShareResource();
    }, 100);
    */
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 기본공유자 리스트 메뉴 다국어 처리 - 사용X
**********************************************************************************************/
View.ShareResource = function () {

    $("#r_sTitle")[0].innerHTML = R_Title;
    $("#r_sType")[0].innerHTML = R_Gubun;

    if ($("#ShareNoListData")[0] != undefined) {
        $("#ShareNoListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 공유자 리스트 (웹서비스 호출)
**********************************************************************************************/
View.GetOpenList = function () {

    ParamArray = {
        'reserveGroupID': ReserveGroupID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessOpenList");
    var url = "/DeskPlusEIP/Visit/Ws/Content/View.asmx/GetOpenList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 공유자 리스트 (웹서비스 값 받기)
**********************************************************************************************/
View.SuccessOpenList = function (data, status) {

    $('#phOpenList').setTemplateURL('/DeskPlusEIP/Visit/Jt/Content/JTOpenList.htm?seq=' + Guids());
    $('#phOpenList').processTemplate(data.d);

    /*
    setTimeout(function () {
        View.OpenListResource();
    }, 100);
    */
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 공유자 리스트 메뉴 다국어 처리 - 사용X
**********************************************************************************************/
View.OpenListResource = function () {

    $("#r_oTitle")[0].innerHTML = R_Title;
    $("#r_oType")[0].innerHTML = R_Gubun;

    if ($("#OpenNoListData")[0] != undefined) {
        $("#OpenNoListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 예약자원 리스트 (웹서비스 호출)
**********************************************************************************************/
View.GetRmList = function () {

    ParamArray = {
        'reserveID': ReserveID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessRmList");
    var url = "/DeskPlusEIP/Visit/Ws/Content/View.asmx/GetRmList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 예약자원 리스트 (웹서비스 값 받기)
**********************************************************************************************/
View.SuccessRmList = function (data, status) {

    $('#phRm').setTemplateURL('/DeskPlusEIP/Visit/Jt/Content/JTRmList.htm?seq=' + Guids());
    $('#phRm').processTemplate(data.d);

    /*
    setTimeout(function () {
        View.RmResource();
    }, 100);
    */
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 예약자원 리스트 메뉴 다국어 처리 - 사용X
**********************************************************************************************/
View.RmResource = function () {

    $("#r_resourcesTitle")[0].innerHTML = R_ResourcesTitle;
    $("#r_qty")[0].innerHTML = R_Qty;
    $("#r_resourcesUser")[0].innerHTML = R_ResourcesUser;

    if ($("#RmNoListData")[0] != undefined) {
        $("#RmNoListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 첨부 파일 리스트 (웹서비스 호출)
**********************************************************************************************/
View.GetFileList = function () {

    ParamArray = {
        'reserveGroupID': ReserveGroupID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessFileList");
    var url = "/DeskPlusEIP/Visit/Ws/Content/View.asmx/GetFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 첨부 파일 리스트 (웹서비스 값 받기)
**********************************************************************************************/
View.SuccessFileList = function (data, status) {

    $('#phFile').setTemplateURL('/DeskPlusEIP/Visit/Jt/Content/JTFile.htm?seq=' + Guids());
    $('#phFile').processTemplate(data.d);

    /*
    setTimeout(function () {
        View.FileResource();
    }, 100);
    */
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
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
작성일자 : 2014-02-12
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
View.FileDownload = function (filePath) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);    
}

/**************************************************************
작성자 : famo23
작성일자 : 2014-02-12
작성목적 : 방문 예약 삭제
**************************************************************/
View.Del = function () {
    if (confirm(R_ConfirmDel)) {

        if (ReserveID != ReserveGroupID) {
            var url = "/DeskPlusEIP/Visit/Content/DelRepeat.aspx";
            PopupWin(400, 225, url, "50", "", true);
        }
        else {
            View.GetRepeat();
        }
    }
}

/**************************************************************
작성자 : famo23
작성일자 : 2014-02-12
작성목적 : 방문 예약 수정
**************************************************************/
View.Mod = function () {

    ParamArray = {
        'reserveID': ReserveID,        
        'tmpDate': TmpDate,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': TagGubun
    };

    CallEncrypt(CallGetParam());

    location.href = "ModN.aspx?p=" + GetEncryptValue;

}

/**************************************************************
작성자 : famo23
작성일자 : 2014-02-12
작성목적 : 목록
**************************************************************/
View.GoList = function () {

    ParamArray = {
        'tmpDate': TmpDate,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': TagGubun
    };

    CallEncrypt(CallGetParam());

    location.href = "ListMain.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 반복 일정 삭제 
**********************************************************************************************/
function RepeatDel(_arg) {
    $("#hdRepeatSel").val(_arg);

    __doPostBack('lbtnConfirm', '');
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 반복일정인지 체크 (웹서비스 호출)
**********************************************************************************************/
View.GetRepeat = function () {

    ParamArray = {
        'reserveID': ReserveID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessGetRepeat");
    var url = "/DeskPlusEIP/Visit/Ws/Content/View.asmx/GetRepeat";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 반복일정인지 체크 (웹서비스 값 받기)
**********************************************************************************************/
View.SuccessGetRepeat = function (data, status) {

    if (data.d) {
        var url = "/DeskPlusEIP/Visit/Content/DelRepeat.aspx";
        PopupWin(400, 225, url, "50", "", true);
    }
    else {
        __doPostBack('lbtnConfirm', '');
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 인쇄
***************************************************/
View.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 방문승인, 방문 거부 (웹서비스 호출)
**********************************************************************************************/
View.ModVisitType = function (_arg) {

    VisitType = _arg;

    ParamArray = {
        'visitType': _arg,
        'reserveID': ReserveID,
        'preLang': PreLang
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessModVisitType");
    var url = "/DeskPlusEIP/Visit/Ws/Content/View.asmx/ModVisitType";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 방문승인, 방문 거부 (웹서비스 값 받기)
**********************************************************************************************/
View.SuccessModVisitType = function (data, status) {

    if (data.d) {

        if (ShareCheck == "Y") {
            if (VisitType == "0") {
                $("#unaccept").css("display", "");
                $("#accept").css("display", "");
            }
            else if (VisitType == "1") {
                $("#unaccept").css("display", "");
                $("#accept").css("display", "none");
            }
            else if (VisitType == "2") {
                $("#unaccept").css("display", "none");
                $("#accept").css("display", "");
            }
        }

        alert(R_CompleteConfirm);
    }
    else {
        alert(R_ProcessingError);
    }
}

