var Menu = {};
Menu.Control = null;

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        /*
        $(".accordion").accordion({
        fillSpace: true,
        animated: 'slide',
        active: AccordionActive
        //event: "mouseover",
        });
        */

        $(".accordion").accordion({
            //heightStyle: "fill",
            //autoHeight: false,
            fillSpace: true,
            animated: 'slide',
            active: AccordionActive
        });

        var h = $(".menusub:eq(" + AccordionActive + ")").height();
        $(".menusub:eq(" + AccordionActive + ")").attr("style", "display:;height:" + h + "px;");

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () { $("#accordion").accordion("resize"); }
        });

        //승인 요청
        $('#setting').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('listAdd');
        });

        //승인 요청
        $('#listAdd').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('listAdd');
        });

        //승인 거부
        $('#listReject').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('listReject');
        });

        //승인 사용자
        $('#listUse').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('listUse');
        });

        //사용불가 사용자(부서)
        $('#authGroup').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('authGroup');
        });

        //초기기본값설정
        $('#config').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('config');
        });

        //설정값변경
        $('#change').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('change');
        });

        //메인구성
        $('#portal').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('portal');
        });

        $('#portalS').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('portal');
        });

        //조직도 선택시 
        $('#accountDept').click(function (e) {
            e.preventDefault();

            //UCDeptTreeScript1_DeptTreeScript1.SelectNodeById(0);

            ParamArray = {
                'deptID': TmpDeptID

            };

            CallEncrypt(CallGetParam());

            Menu.ContentFrame.location.href = "../Dept/List.aspx?p=" + GetEncryptValue;
        });

        //접근권한 등록
        $('#auth').click(function (e) {
            e.preventDefault();

            Menu.Control = UCMDMTreeScript1_MDMMenuTreeScript1;
            MenuID = Menu.Control.SelectedNode.ID;

            ParamArray = {
                'menuID': MenuID
            };

            CallEncrypt(CallGetParam());

            Menu.ContentFrame.location.href = "../MenuAuth/Add.aspx?p=" + GetEncryptValue;
        });

        Menu.ContentFrame = parent.frmContent;

    });
});

/**************************************************************
리스트 페이지 호출하기
**************************************************************/
Menu.GoListPage = function (menuID) {

    var listPage = "../Auth/ListAdd.aspx";

    if (menuID == "") {
        menuID = "listAdd";
    }


    switch (menuID) {
        case "listAdd": //승인요청
            listPage = "../Auth/ListAdd.aspx";

            ParamArray = {
                'companyID': 1
            };

            break;
        case "listReject": //승인거부
            listPage = "../Auth/ListReject.aspx";

            ParamArray = {
                'companyID': 1
            };
            break;
        case "listUse": //승인사용자
            listPage = "../Auth/ListUse.aspx";

            ParamArray = {
                'companyID': 1
            };
            break;
        case "authGroup": //사용불가사용자(부서)
            listPage = "../Auth/AuthGroup.aspx";

            ParamArray = {
                'companyID': 1
            };
            break;
        case "config": //초기기본값설정
            listPage = "../Config/Default.aspx";

            ParamArray = {
                'companyID': 1
            };
            break;
        case "change": //설정값변경
            listPage = "../Config/Setting.aspx";

            ParamArray = {
                'companyID': 1
            };
            break;
        case "portal": //메인구성
            listPage = "../Portal/List.aspx";

            ParamArray = {
                'companyID': 1
            };
            break;
        default:
            break;
    }

    if (listPage != "" && Menu.ContentFrame) {
        CallEncrypt(CallGetParam());
        Menu.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2013-01-17
작성목적 : 부서 선택
***************************************************/
function DeptNodeSelect_onNodeSelect(node) {

    TmpDeptID = node.ID;

    //파라미터값 배열에 담기
    ParamArray = {
        'deptID': TmpDeptID

    };

    CallEncrypt(CallGetParam());

    Menu.ContentFrame.location.href = "../Dept/List.aspx?p=" + GetEncryptValue;
}

/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}

/**************************************************
작성자   : famo23
작성일자 : 2014-01-17
작성목적 : 메뉴 선택
***************************************************/
function MenuNodeSelect_onNodeSelect(node) {

    MenuID = node.ID;

    //파라미터값 배열에 담기
    ParamArray = {
        'menuID': MenuID
    };
    CallEncrypt(CallGetParam());

    Menu.ContentFrame.location.href = "../MenuAuth/Add.aspx?p=" + GetEncryptValue;

}

