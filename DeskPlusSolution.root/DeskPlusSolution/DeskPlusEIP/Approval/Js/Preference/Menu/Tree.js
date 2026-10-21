var Tree = {};

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            active: AccordionActive
            //event: "mouseover",
        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () { $("#accordion").accordion("resize"); }
        });

        //자주지정하는결재자
        $('#FavoriteApp').click(function () {
            Tree.GoListPage('F');
        });

        //대결자관리
        $('#ProxyApp').click(function () {
            Tree.GoListPage('P');
        });

        //인장,사인관리
        $('#SignApp').click(function () {
            Tree.GoListPage('S');
        });

        //개인결재망관리
        $('#UserFlowApp').click(function () {
            Tree.GoListPage('U');
        });

        //자주사용하는양식관리
        $('#FavoriteForm').click(function () {
            Tree.GoListPage('FF');
        });

        //환경설정
        $('#Setting').click(function () {
            Tree.GoListPage('SS');
        });
    });
});


/**************************************************************
 리스트 페이지 호출하기
**************************************************************/
Tree.GoListPage = function (menuID) {

    var listPage = "../Favorite/List.aspx";
    switch (menuID) {
        case "F": //자주지정하는 결재자
            listPage = "../Favorite/List.aspx";
            ParamArray = { 'menuID': "F" };

            break;
        case "P": //대결자 관리
            listPage = "../Proxy/List.aspx";
            ParamArray = { 'menuID': "P" };
            break;
        case "S": //사인,인장관리
            listPage = "../Signature/List.aspx";
            ParamArray = { 'menuID': "S" };
            break;
        case "U": //개인 결재망 관리
            listPage = "../Flow/List.aspx";
            ParamArray = { 'menuID': "U" };
            break;
        case "FF": //자주사용하는양식관리
            listPage = "../FavoriteForm/List.aspx";
            ParamArray = { 'menuID': "FF" };
            break;
        case "SS": //환경설정
            listPage = "../Setting/List.aspx";
            ParamArray = { 'menuID': "SS" };
            break;
        default:
            listPage = "../Favorite/List.aspx";
            ParamArray = { 'menuID': "F" };
            break;
    }
    CallEncrypt(CallGetParam());
    parent.frmContent.location = listPage + "?p=" + GetEncryptValue;
}
