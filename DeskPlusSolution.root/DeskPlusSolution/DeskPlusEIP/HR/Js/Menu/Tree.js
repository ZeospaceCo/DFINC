var Tree = {};

Tree.Control = null;
Tree.InitNodeID = 0;
Tree.ContentFrame = null;

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

        //근태정보수정
        $('#ListPrivateMod').click(function (e) {
            var url = "/DeskPlusEIP/HR/Content/ListPrivateMod.aspx";
            ParamArray = { 'folderStatusID': "PrivateMod" };

            CallEncrypt(CallGetParam());
            parent.frmContent.location.href = url + "?p=" + GetEncryptValue;
        });

        //겸직부서관리
        $('#ListUserDept').click(function (e) {
            var url = "/DeskPlusEIP/HR/Content/ListUserDept.aspx";
            parent.frmContent.location.href = url;
        });

        //휴가정보
        $('#HoliListUser').click(function (e) {
            var url = "/DeskPlusEIP/ETC/Holiday/ListUser.aspx";
            parent.frmContent.location.href = url;
        });

        //야근/특근내역
        $('#WorkListUser').click(function (e) {
            var url = "/DeskPlusEIP/ETC/Work/ListUser.aspx";
            parent.frmContent.location.href = url;
        });

        Tree.Init();
    });
});

/**************************************************************
초기화
**************************************************************/
Tree.Init = function () {

    Tree.ContentFrame = parent.frmContent;
    Tree.SelectedNodeID = FolderStatusID;

    if (DefaultMenuID == "ListUserDept") {
        parent.frmContent.location = "/DeskPlusEIP/HR/Content/ListUserDept.aspx";
    }
}

/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {

    var id = node.ID;
    var targetPage = "";
    Tree.SelectedNodeID = node.ID;

    switch (id) {
        case "Attendance": //근태 현황 함
            targetPage = "../Content/ListPrivate.aspx";
            ParamArray = { 'folderStatusID': "Private" };
            break;

        case "Private": //개인 근태 현황표
            targetPage = "../Content/ListPrivate.aspx";
            ParamArray = { 'folderStatusID': "Private" };
            break;

        case "Dept": //부서 근태 현황표
            targetPage = "../Content/ListDept.aspx";
            ParamArray = { 'folderStatusID': "Dept" };
            break;

        case "Company": //회사 근태 현황표
            targetPage = "../Content/ListCompany.aspx";
            ParamArray = { 'folderStatusID': "Company" };
            break;

        case "PrivateMod": //근태 내역 관리
            targetPage = "../Content/ListPrivateMod.aspx";
            ParamArray = { 'folderStatusID': "PrivateMod" };
            break;

        default:
            break;
    }
    if (targetPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = targetPage + "?p=" + GetEncryptValue;
    }
}


/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}
