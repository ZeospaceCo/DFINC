JudgeView = {}

/**************************************************
초기화
***************************************************/
$(function () {

    $(document).ready(function () {

        // 확인
        $('#cbtnSave').click(function (e) {
            e.preventDefault();
            JudgeView.Save();
        });

        // 다시평가
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            JudgeView.Mod();
        });

        // 평가자 변경
        $('#cbtnChange').click(function (e) {
            e.preventDefault();
            JudgeView.Change();
        });

        // 취소
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            JudgeView.List();
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            JudgeView.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        JudgeView.Init();
    });
});

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 인쇄
***************************************************/
JudgeView.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 평가표 총합  
***************************************************/
JudgeView.Init = function () {

    JudgeView.ChangeScore();
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 평가 점수 선택(변경) 시 총합 계산
***************************************************/
JudgeView.ChangeScore = function () {
    Sum = 0;
    Score = "";

    if (CodeName.length > 0) {
        var item = CodeName.split('^');

        for (var i = 0; i < Count; i++) {
            Sum += parseFloat($("#ddl" + item[i]).val());
            Score += $("#ddl" + item[i]).val() + "^";
        }
    }

    $('#tbSum').attr("disabled", false);
    $("#tbSum").val(Sum);
    $('#tbSum').attr("disabled", true);

}
/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 확인
***************************************************/
JudgeView.Save = function () {

    if (window.confirm("심사하신 내용은 수정이 불가능 합니다.\n심사를 완료하시겠습니까?")) {
        var url = "/DeskPlusEIP/Proposal/Ws/Content/Judge/JudgeView.asmx/Add";

        // 심사관 의견
        var opinion = $("#textarea").val();
        
        ParamArray = {
            'proposalNo': ProposalNo,
            'itemNo': ItemNo,
            'codeName': CodeName,
            'score': Score,
            'sum': Sum,
            'count': Count,
            'opinion': opinion
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("JudgeView.SuccessSave");

        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 확인(웹서비스)
***************************************************/
JudgeView.SuccessSave = function (data, status) {
    if (data.d == true) {
        alert("완료하였습니다.");

        var listPage = "JudgeList.aspx";

        ParamArray = {
            'judgeAuth': JudgeAuth,
            'currentPage': CurrentPage,
            'order': Order,
            'listCount': ListCount,
            'menuID': MenuID
        };

        CallEncrypt(CallGetParam());
        
        location.href = listPage + "?p=" + GetEncryptValue;
    }
    else {
        alert("처리시 문제가 발생하였습니다.\n 관리자에게 문의하세요.");
    }
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 다시평가
***************************************************/
JudgeView.Mod = function () {

    __doPostBack('lbtnMod', 'Del');
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 평가자 변경
***************************************************/
JudgeView.Change = function () {

    ParamArray = {
        'judgeUserList': JudgeList,
        'menuID': "Judge"
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Proposal/Common/GetJudgeUserList.aspx?p=" + GetEncryptValue;
    PopupWin(600, 480, url, "-500", "", true);
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 목록으로 가기 
***************************************************/
JudgeView.List = function () {

    var listPage = "JudgeList.aspx";

    ParamArray = {
        'currentPage': CurrentPage,
        'order': Order,
        'listCount': ListCount,
        'menuID': MenuID,
        'judgeAuth': JudgeAuth,
    };

    CallEncrypt(CallGetParam());
    location.href = listPage + "?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
JudgeView.FileDownload = function (filePath, fileName, fileGuid) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
    //WindowObjectReference = window.open(attachFilePath, "FileDownload", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");
}

/**************************************************
작성목적 : 사용자 정보
***************************************************/
JudgeView.AccountInfo = function (p_userID) {

    ParamArray = {
        'userID': p_userID
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Proposal/Common/UserInfo.aspx?p=" + GetEncryptValue;

    PopupWin(500, 370, url, "", "", true);
}

/**************************************************
작성자   : 
작성일자 : shine
작성목적 : 1차 심사자 추가값 가져오기
***************************************************/
function GetDept(rtnVal) {

    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
        var response = unescape(rtnVal);
        var responseStr = response.split("‡")[0];

        setTimeout(function () {
            JudgeView.ModJudgeUser(responseStr);    
        }, 500);   
    }
}

/**************************************************
작성자   : 
작성일자 : shine
작성목적 : 1차 심사자 변경
***************************************************/
JudgeView.ModJudgeUser = function (_userList) {

    ParamArray = {
        'proposalNo': ProposalNo,
        'userList': _userList
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("JudgeView.SuccessJudgeUserList");
    var url = "/DeskPlusEIP/Proposal/Ws/Content/Judge/JudgeView.asmx/ModJudgeUserList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 1차 심사 변경(웹서비스)
**********************************************************************************************/
JudgeView.SuccessJudgeUserList = function (data, status) {
    if (data.d == true) {
        alert("심사자 변경이 완료되었습니다.");

        var listPage = "JudgeList.aspx";

        ParamArray = {
            'currentPage': CurrentPage,
            'order': Order,
            'listCount': ListCount,
            'menuID': MenuID,
            'judgeAuth': JudgeAuth
        };
        
        CallEncrypt(CallGetParam());
        location.href = listPage + "?p=" + GetEncryptValue;
    }
    else {
        alert("처리시 문제가 발생하였습니다. 관리자에게 문의하세요.");
    }
}