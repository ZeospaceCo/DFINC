View = {}

/**************************************************
초기화
***************************************************/
$(function () {

    $(document).ready(function () {

        //목록
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            View.List();
        });

        //심사완료
        $('#cbtnComplete').click(function (e) {
            e.preventDefault();
            View.SaveGrade();
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });

    });
});

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 인쇄
***************************************************/
View.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 등급 저장
***************************************************/
View.SaveGrade = function () {
    var grade = $("#ctbGrade").val();
    
    ParamArray = {
        'grade': grade,
        'proposalNo': ProposalNo
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessSaveGrade");
    var url = "/DeskPlusEIP/Proposal/Ws/Content/Admin/ResultView.asmx/ModGrade";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 등급 저장
**********************************************************************************************/
View.SuccessSaveGrade = function (data, status) {
    if (data.d == true) {
        alert("심사완료되었습니다.");
    }
    else {
        alert("처리시 문제가 발생하였습니다.\n관리자에게 문의하세요.");
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 심사의견 보기
**********************************************************************************************/
View.Opinion = function (p_proposalNo, p_userID) {
    
    ParamArray = {
        'proposalNo': p_proposalNo,
        'judegUserID': p_userID
    };

    CallEncrypt(CallGetParam());

    var pageList = "Opinion.aspx?p=" + GetEncryptValue;
    PopupWin(450, 196, pageList, "-500", "JudgeOpinion", true);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 목록
**********************************************************************************************/
View.List = function () {
    var pageList = "FirstProcess.aspx";

    ParamArray = {
        'currentPage': CurrentPage,
        'order': Order,
        'listCount': ListCount,
        'menuID': MenuID
    };

    CallEncrypt(CallGetParam());
    location.href = pageList + "?p=" + GetEncryptValue;
}