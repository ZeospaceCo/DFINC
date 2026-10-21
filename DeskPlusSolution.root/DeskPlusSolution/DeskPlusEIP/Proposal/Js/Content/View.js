View = {}

/**************************************************
초기화
***************************************************/
$(function () {

    $(document).ready(function () {

        //목록
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            View.List();
        });

        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();

            ParamArray = {
                'startYears': StartYears,
                'startMonths': StartMonths,
                'endYears': EndYears,
                'endMonths': EndMonths,
                'judgeGubun': JudgeGubun,
                'listCount': ListCount,
                'currentPage': CurrentPage,
                'searchField': SearchField,
                'searchValue': SearchValue,
                'order': Order,
                'proposalNo': ProposalNo,
                'menuID': MenuID
            };

            CallEncrypt(CallGetParam());

            location = "Mod.aspx?p=" + GetEncryptValue;
        });

        // 제안취소
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            View.Del();
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        // 진행사항
        View.StatusList();

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
작성목적 : 목록으로 가기 
***************************************************/
View.List = function () {

    var listPage = "../Content/List.aspx";

    if (MenuID == "MyList")
    {
        listPage = "../Content/MyList.aspx";
    }
    else if (MenuID == "FirstProcess") {
        listPage = "../Content/Admin/FirstProcess.aspx";
    }

    ParamArray = {
        'judgeGubun': JudgeGubun,
        'currentPage': CurrentPage,
        'startYears': StartYears,
        'startMonths': StartMonths,
        'endYears': EndYears,
        'endMonths': EndMonths,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'listCount': ListCount,
        'menuID': MenuID
    };

    CallEncrypt(CallGetParam());
    location.href = listPage + "?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 제안취소
***************************************************/
View.Del = function () {

    if (window.confirm("제안을 삭제하겠습니까?")) {

        ParamArray = {
            'proposalNo': ProposalNo,
            'gubunCode': "W,P"
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessDel");
        var url = "/DeskPlusEIP/Proposal/Ws/Content/View.asmx/Del";

        AjaxService(url, data, obj);
    }
    
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 제안취소 성공
**********************************************************************************************/
View.SuccessDel = function (data, status) {
    if (data.d == true) {
        alert("제안이 취소되었습니다.");

        var listPage = "../Content/MyList.aspx";

        ParamArray = {
            'judgeGubun': JudgeGubun,
            'currentPage': CurrentPage,
            'startYears': StartYears,
            'startMonths': StartMonths,
            'endYears': EndYears,
            'endMonths': EndMonths,
            'searchField': SearchField,
            'searchValue': SearchValue,
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
작성목적 : 진행사항
***************************************************/
View.StatusList = function () {

    ParamArray = {
        'proposalNo': ProposalNo
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessStatusList");
    var url = "/DeskPlusEIP/Proposal/Ws/Content/View.asmx/GetStatusList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 진행사항 리스트 보여주기
**********************************************************************************************/
View.SuccessStatusList = function (data, status) {
    $('#phStatusList').setTemplateURL('/DeskPlusEIP/Proposal/Jt/Content/JTStatus.htm?seq=' + Guids());
    $('#phStatusList').processTemplate(data.d);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
View.FileDownload = function (filePath, fileName, fileGuid) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
    //WindowObjectReference = window.open(attachFilePath, "FileDownload", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");
}

/**************************************************
작성목적 : 사용자 정보
***************************************************/
View.AccountInfo = function (p_userID) {

    ParamArray = {
        'userID': p_userID
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Proposal/Common/UserInfo.aspx?p=" + GetEncryptValue;

    PopupWin(500, 370, url, "", "", true);
}