Print = {}

$(function () {
    $(document).ready(function () {



        $('#cbtnPrint').click(function (e) {
            e.preventDefault();

            //IE 이면 ScriptX 로 프린트설정
            if ($.browser.msie) {
                // for, IE
                //setTimeout("Print.PrintStart();", 1000);  윈도우 업데이트 버그에 따른 임시처리 (ifrm)

                newWin = window.open();
                newWin.document.write(window.frames["ifrmForm"].document.documentElement.outerHTML);
                setTimeout("newWin.location.reload();newWin.focus(); newWin.print();", 600);     //for, Chrome & safari
                setTimeout("newWin.close();", 1000);

            } else {

                //for, Chrome & safari
                newWin = window.open("", "_blank");
                newWin.document.write(window.frames["ifrmForm"].document.documentElement.outerHTML);
                //newWin.document.close();
                //newWin.print();

                setTimeout("newWin.focus(); newWin.print();", 600);     //for, Chrome & safari
                setTimeout("newWin.close();", 1000);
            }
        });

        //[닫기]
        $('#Close').click(function (e) { if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); } });


        // 결재의견 체크
        $('#chkOpinion').click(function (e) {
            var obj = $("#OPINION", ifrmObj);
            if ($("#chkOpinion").is(":checked")) {
                $(obj).hide();
            } else {
                $(obj).show();
            }
        });

        // 첨부파일 체크
        $('#chkEtcs').click(function (e) {
            var obj = $("#PRINT", ifrmObj);
            if ($("#chkEtcs").is(":checked")) {
                $(obj).show();
            } else {
                $(obj).hide();
            }
        });

        Print.GetAttachFileList();  // 첨부파일 가져오기
        Print.GetBaseDocList();     // 근거문서 가져오기

        Print.Init();
    });
});

Print.Init = function () {

    // 내용로딩
    targetObj = (window.dialogArguments) ? window.dialogArguments : parent;
    printarea = targetObj.document.getElementById("ifrmForm");

    var html = printarea.contentWindow.document.documentElement.outerHTML;
    ifrmForm.document.open();
    ifrmForm.document.write(html);
    ifrmForm.document.close();

    ifrmObj = ifrmForm.document;

    //프린트영역 태그 유무확인
    if ($("#PRINT", ifrmObj).length > 0) {
        $(".isprint").show();
        $("#PRINT", ifrmObj).hide();
    } else {
        $(".isprint").hide();
    }

    //결재의견 태그 유무확인
    if ($("#OPINION", ifrmObj).length > 0) {
        $(".isopinion").show();
    } else {
        $(".isopinion").hide();
    }

    $('head', ifrmForm.document).append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
}

//인쇄-IE
Print.PrintStart = function () {

    var objFrame = window.frames["ifrmForm"];
    var rtn = PrintX(factory, objFrame);
    if (rtn) {
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }  // 인쇄후,창닫기
    }
}

//Component Option
PrintX = function (factory, obj) {

    factory.printing.header = "";           //머릿말 설정
    factory.printing.footer = "";           //꼬릿말 설정
    factory.printing.portrait = true;       //출력방향 설정: false-가로, true-세로
    factory.printing.leftMargin = 15.0;     //왼쪽 여백 설정
    factory.printing.topMargin = 10.0;      //위쪽 여백 설정
    factory.printing.rightMargin = 15.0;    //오른쪽 여백 설정
    factory.printing.bottomMargin = 10.0;   //아래쪽 여백 설정
    //factory.printing.printBackground = true;// 백그라운드까지 출력
    rtn = factory.printing.Print(true, obj); //출력하기

    return rtn;
}


// 첨부파일  리스트
Print.GetAttachFileList = function () {

    ParamArray = { 'childDocID': ChildDocID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Print.SuccessAttachFile");
    var url = "/DeskPlusEIP/Approval/Ws/Common/Popup/ViewTot.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}
var filehtml = "", basehtml = "";
Print.SuccessAttachFile = function (data, status) {

    if (data == null) return;

    if (data.d.length > 0) {
        filehtml += "<table class='tabprt'><tr><th style='width:80px;'>" + AttachName + " (" + data.d.length + ")</th><td>";
        $.each(data.d, function (i, o) {
            filehtml += "<span style='padding-left:5px;'>" + o.FileName + " (" + o.FileLengthKB + ")</span>, ";
        });
        filehtml += "</td></tr></table>";
    }
    setTimeout("Print.SetInnerHTML('file')", 600);
}

// 근거문서 리스트
Print.GetBaseDocList = function () {

    ParamArray = { 'childDocID': ChildDocID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Print.SuccessBaseDoc");
    var url = "/DeskPlusEIP/Approval/Ws/Common/Popup/ViewTot.asmx/GetBaseDocList";

    AjaxService(url, data, obj);
}
Print.SuccessBaseDoc = function (data, status) {

    if (data == null) return;

    if (data.d.length > 0) {
        basehtml += "<table class='tabprt'><tr><th style='width:80px;'>" + BaseDoc + " (" + data.d.length + ")</th><td>";
        $.each(data.d, function (i, o) {
            basehtml += "<span style='padding-left:5px;'>" + o.Subject + "</span>, ";
        });
        basehtml += "</td></tr></table>";
    }
    setTimeout("Print.SetInnerHTML('base')", 600);
}

Print.SetInnerHTML = function (_o) {

    if (_o == "file") {
        $("#PRINT", ifrmObj).append(filehtml);
    }
    if (_o == "base") {
        $("#PRINT", ifrmObj).append(basehtml);
    }
}