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

        //연차관리
        $('#HoliAdmin').click(function () {
            Tree.GoListPage('HoliAdmin');
        });

        //휴가내역관리
        $('#HoliAdminManage').click(function () {
            Tree.GoListPage('HoliAdminManage');
        });

        //야/특근 내역관리
        $('#OverTimeAdmin').click(function () {
            Tree.GoListPage('OverTimeAdmin');
        });

        //근무 유형관리
        $('#WorkAdmin').click(function () {
            Tree.GoListPage('WorkAdmin');
        });

        //조회권한관리
        $('#RoleAdmin').click(function () {
            Tree.GoListPage('RoleAdmin');
        });

        //수정권한관리
        $('#AttendanceRoleAdmin').click(function () {
            Tree.GoListPage('AttendanceRoleAdmin');
        });

    });
});


/**************************************************************
리스트 페이지 호출하기
**************************************************************/
Tree.GoListPage = function (menuID) {

    var listPage = "/DeskPlusEIP/ETC/Holiday/ListAdmin.aspx";
    switch (menuID) {
        case "HoliAdmin": //연차관리
            listPage = "/DeskPlusEIP/ETC/Holiday/ListAdmin.aspx";
            ParamArray = { 'menuID': "HoliAdmin" };
            break;
        case "HoliAdminManage": //휴가내역관리
            listPage = "/DeskPlusEIP/ETC/Holiday/AdminManage.aspx";
            ParamArray = { 'menuID': "HoliAdminManage" };
            break;
        case "OverTimeAdmin": //야/특근 내역관리
            listPage = "/DeskPlusEIP/ETC/Work/AdminManage.aspx";
            ParamArray = { 'menuID': "OverTimeAdmin" };
            break;
        case "WorkAdmin": //근무 유형관리
            listPage = "../Content/WorkAdmin.aspx";
            ParamArray = { 'menuID': "WorkAdmin" };
            break;
        case "RoleAdmin": //조회권한관리
            listPage = "../Content/RoleAdmin.aspx";
            ParamArray = { 'menuID': "RoleAdmin" };
            break;
        case "AttendanceRoleAdmin": //수정권한관리
            listPage = "../Content/AttendanceRoleAdmin.aspx";
            ParamArray = { 'menuID': "AttendanceRoleAdmin" };
            break;
        default:
            listPage = "/DeskPlusEIP/ETC/Holiday/ListAdmin.aspx";
            ParamArray = { 'menuID': "HoliAdmin" };
            break;
    }
    CallEncrypt(CallGetParam());
    parent.frmContent.location = listPage + "?p=" + GetEncryptValue;
}
