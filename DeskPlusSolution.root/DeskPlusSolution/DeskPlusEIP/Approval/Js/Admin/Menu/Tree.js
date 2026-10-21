var Tree = {};

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

    });
});

/**************************************************************
 리스트 페이지 호출하기
**************************************************************/
Tree.GoListPage = function (menuID) {

    var listPage = "../FormGeneration/List.aspx";
    if (menuID == "") { menuID = "Form"; }

    switch (menuID.toUpperCase()) {
        case "FORM": //양식 관리
            listPage = "../FormGeneration/List.aspx";
            menuID = "Form";
            break;
        case "FOLDER": //폴더관리
            listPage = "../Folder/List.aspx";
            menuID = "Private";
            break;
        case "DOC": //결재문서관리
            listPage = "../Document/JList.aspx";
            menuID = "Doc";
            break;        
        case "CONTACTR": //문서 담당자(수발자) 관리
            listPage = "../DocContact/ListR.aspx";
            menuID = "ContactR";
            break;
        case "CONTACTS": //문서 담당자(결재자) 관리
            listPage = "../DocContact/ListS.aspx";
            menuID = "ContactS";
            break;
        case "PROXY": //대결자 관리
            listPage = "../Proxy/List.aspx";
            menuID = "Proxy";
            break;
        case "ENFORCESEAL": //시행문 직인관리
            listPage = "../EnforceSeal/List.aspx";
            menuID = "EnforceSeal";
            break;
        case "FOLDERAUTH": //문서함 권한관리
            listPage = "../FolderAuth/List.aspx";
            menuID = "FolderAuth";
            break;
        case "ACCESSGRADE": //열람등급 관리
            listPage = "../AccessGrade/List.aspx";
            menuID = "AccessGrade";
            break;
        case "CONFIG": //환경설정
            listPage = "../Config/List.aspx";
            menuID = "Config";
            break;
        case "TRANSFER": //문서이관
            listPage = "../Transfer/List.aspx";
            menuID = "Transfer";
            break;
        case "STATMONTH": //월별현황-양식/부서/개인
            listPage = "../Statistics/ListMonth.aspx";
            menuID = "StatMonth";
            break;
        case "STATTOP": //월별현황-양식/부서/개인
            listPage = "../Statistics/ListTop.aspx";
            menuID = "StatTop";
            break;

        case "TRANSFERDEPT": //문서이관(부서별)
            listPage = "../Transfer/ListDept.aspx";
            menuID = "TransferDept";
            break;
        case "TRANSFERUSER": //문서이관(사용자별)
            listPage = "../Transfer/ListUser.aspx";
            menuID = "TransferUser";
            break;
        default:
            break;
    }

    if (listPage != "") {
        ParamArray = { 'menuID': menuID };
        CallEncrypt(CallGetParam());
        parent.frmContent.location.href = listPage + "?p=" + GetEncryptValue;
    }
}
