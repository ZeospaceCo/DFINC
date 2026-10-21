List = {}
var imagePath = "";
/**********************************************************************************************
작성자   : freeman
작성일자 : 2012-06-26
작성목적 : 초기화 
**********************************************************************************************/
$(function () {
    $(document).ready(function () {
        //인장파일 추가 
        $('#Add').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        //인장 삭제 
        $('#Del').click(function (e) {
            e.preventDefault();
            List.Del();
        });

        //사인하기
        $('#Sign').click(function (e) {
            e.preventDefault();
            List.Sign();
        });
    });
});


/**********************************************************************************************
작성자   : 
작성일자 : 2012-06-20
작성목적 : 인장 파일 추가
**********************************************************************************************/
List.Add = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'fileCount': '1',
        'moduleName': 'ACCOUNT',
        'callBackEvent': "List.GetAttachFileList"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")

}


/**********************************************************************************************
작성자   : 
작성일자 : 2012-06-20
작성목적 : 사진 이미지 넣기
**********************************************************************************************/
List.GetAttachFileList = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessAttachFile");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 
작성일자 : 2012-06-20
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
List.SuccessAttachFile = function (data, status) {
    var fileName = "";
    for (var count in data.d) {
        imagePath = data.d[count].FilePath;
        fileName = data.d[count].FileName;
    }

    if (imagePath != "" && fileName != "") {

        //*********** Seal DB Field 업데이트
        ParamArray = {
            'swfUploadGUID': SwfUploadGUID,
            'fileName': fileName
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("List.SuccessDBSave");
        var url = "/DeskPlusEIP/Approval/Ws/Preference/Signature/List.asmx/Save";
        
        AjaxService(url, data, obj);
    }
}


/**********************************************************************************************
작성자   : freeman
작성일자 : 2012-06-26
작성목적 : 인장파일 이미지 로드
**********************************************************************************************/
List.SuccessDBSave = function (data, status) {
    var image = document.getElementById("SignImage");
    var tmpImage = new Image(200, 200);
    tmpImage.src = imagePath;

    tmpImage.onload = function () {
        image.src = tmpImage.src;
    }
}


/**********************************************************************************************
작성자   : freeman
작성일자 : 2012-06-26
작성목적 : 인장파일 삭제
**********************************************************************************************/
List.Del = function () {

}


/**********************************************************************************************
작성자   : freeman
작성일자 : 2012-06-20
작성목적 : 사인하기
**********************************************************************************************/
List.Sign = function () {
    var url = "/DeskPlusEIP/Approval/Common/Popup/GetSign.aspx";
    PopupWin(300, 290, url, "-500", "", true);
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2012-06-20
작성목적 : 사인 리로드
**********************************************************************************************/
function SignConfirm(path) {
    
    CallEncrypt(path);
    
    var image = document.getElementById("SignImage");
    var tmpImage = new Image(200, 200);
    tmpImage.src = "GetImage.ashx?p=" + GetEncryptValue;
    
    tmpImage.onload = function () {
        image.src = tmpImage.src;
    }
}