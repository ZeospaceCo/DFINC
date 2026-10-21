Send = {}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/
$(function () {

    $(document).ready(function () {

        //직인미리보기
        $('#CbtnPreviewStamp').click(function (e) { e.preventDefault(); Send.PreviewStamp(); });
        //공문발송내역
        $('#CbtnHistory').click(function (e) { e.preventDefault(); Send.History(); });
        //메일발송
        $('#CbtnMail').click(function (e) { e.preventDefault(); Send.Mail(); });
        //출력(직인)
        $('#CbtnPrintStamp').click(function (e) { e.preventDefault(); Send.Print(true); });
        //출력(직인없음)
        $('#CbtnPrint').click(function (e) { e.preventDefault(); Send.Print(false); });
        //취소
        $('#CbtnCancel').click(function (e) { e.preventDefault(); Send.Cancel(); });

        Send.Init();
    });
});

Send.Init = function () {

    //발송형태가 메일(0)인경우, 메일버튼이 보여짐
    var viewing = (SendType == "0") ? "display:" : "display:none";
    $(".mail").attr("style", viewing);

    //직인사용여부가 온라인(0)인 경우, 직인미리보기&출력(직인) 사용
    //직인등록이 되어있는경우, 직인사용
    viewing = (StampType == "0" && StampPath != "") ? "display:" : "display:none";
    $(".stamp").attr("style", viewing);

    if (FormPath == "") return;

    // 양식로드
    ifrmForm.document.location.href = FormPath;
    ifrmObj = ifrmForm.document;

    // 공문내용처리
    setTimeout("Common.InitEnforce();", 100);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 공문발송내역
***************************************************/
Send.History = function () {

    ParamArray = { 'enforceID': EnforceID };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Approval/Folder/Enforce/History.aspx?p=" + GetEncryptValue;
    PopupWin(450, 310, url, "-500", "", true);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 직인미리보기
***************************************************/
var isview = false;
Send.PreviewStamp = function () {
    isview = (isview) ? false : true;
    Common.SetStamp(isview);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 메일발송
***************************************************/
Send.Mail = function () {

    if (confirm(R_EnforceMsgSendMail)) {

        Send.SendHistory(true); // History처리
        
        // 문서내용(Html)처리
        $("#htaFormHTML").val("<HTML>" + $("HTML", ifrmObj).html() + "</HTML>");
        __doPostBack('lbtnSubmit', "MAIL");
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 출력(직인/직인없음)
***************************************************/
Send.Print = function (_isstamp) {

    isview = _isstamp;
    Common.SetStamp(isview);

    var divToPrint = document.getElementById('ifrmForm');

    //스타일정보+내용
    var html = divToPrint.contentWindow.document.documentElement.outerHTML;
    newWin = window.open();
    newWin.document.write(html);

    //alert(navigator.userAgent)
    if (navigator.userAgent.indexOf("Chrome") > 0 || navigator.userAgent.indexOf("Safari") > 0) {
        setTimeout("newWin.focus(); newWin.print();", 600);     //for, Chrome & safari
    } else {
        newWin.location.reload();                               //for, exploror & firefox
        newWin.focus();
        newWin.print();
    }

    setTimeout("newWin.close();", 1000);

    Send.SendHistory(false); // History처리
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 발송내역처리
***************************************************/
Send.SendHistory = function (_isMail) {

    if (_isMail == null)  { return; }

    var sendType = (_isMail) ? "0" : "1";
    var stampType = (isview) ? "0" : "1";       //온라인직인(0)/오프라인직인(1)
    var docType = (_isMail) ? "0" : "1";        //문서형태 - 온라인(0)/오프라인(1)

    var url = "/DeskPlusEIP/Approval/Ws/Folder/Enforce/Send.asmx/AddHistory";
    ParamArray = { 
        'enforceID': EnforceID
        , 'sendType': sendType
        , 'stampType': stampType
        , 'docType': docType
    };

    var obj = eval("Send.SendHistorySuccess");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Send.SendHistorySuccess = function (data, status) {
    //alert(data.d);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 취소
***************************************************/
Send.Cancel = function () {
    ParamArray = {
        'currentPage': CurrentPage
        , 'recordPerPage': RecordPerPage
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'searchTerm': SearchTerm
        , 'searchTermValue': SearchTermValue
        , 'order': Order
        , 'targetPage': TargetPage
        , 'cabinetID': CabinetID
    };
    CallEncrypt(CallGetParam());
    location.href = TargetPage + "?p=" + GetEncryptValue;
}
