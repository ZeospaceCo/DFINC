Menu = {}
Menu.ContentFrame = parent.frmContent;
Menu.TreeFrame = parent.frmLeft;


/**************************************************
작성자 : famo23
작성일자 : 2012-05-12
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            overTop: 55,
            active: AccordionActive
            //event: "mouseover",
        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () { $("#accordion").accordion("resize"); }
        });

        // 가입 신청 관리
        $('#MAdd').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('MAdd');
        });

        // 회원 조회
        $('#MMod').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('MMod');
        });

        // 우수회원 조회
        $('#MGood').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('MGood');
        });

        // 커뮤니티 정보 수정
        $('#GInfo').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('GInfo');
        });

        // 커뮤니티 해체
        $('#GDel').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('GDel');
        });

        // 시샵변경
        $('#GMod').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('GMod');
        });

        // 포인트 관리
        $('#GPoint').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('GPoint');
        });

        // 회원등급명 관리
        $('#GGrade').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('GGrade');
        });

        // 메뉴관리
        $('#FMenu').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('FMenu');
        });

        // 대문관리
        $('#FMain').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('FMain');
        });

        // 시작화면 관리
        $('#FPortal').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('FPortal');
        });

        // 스킨관리
        $('#FSkin').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('FSkin');
        });

        // 홈이동
        $('#HomeGo').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('GHome');
        });

        Menu.Init();

    });
});

/**************************************************************
작성자 : famo23
작성일자 : 2012-05-12
작성목적 : 초기화
**************************************************************/
Menu.Init = function () {
    Menu.GoListPage('MAdd');
}


/**************************************************************
작성자 : famo23
작성일자 : 2012-05-12
작성목적 : 페이지 호출하기
**************************************************************/
Menu.GoListPage = function (menuID) {

    var listPage = "../Member/ListAdd.aspx";

    if (menuID == "") {
        menuID = "MAdd";
    }


    switch (menuID) {
        case "MAdd": //가입신청관리
            listPage = "../Member/ListAdd.aspx";

            ParamArray = {
                'communityID': CommunityID
            };

            break;
        case "MMod": //회원 관리
            listPage = "../Member/ListMod.aspx";

            ParamArray = {
                'communityID': CommunityID
            };
            break;
        case "MGood": //우수회원조회
            listPage = "../Member/ListGood.aspx";

            ParamArray = {
                'communityID': CommunityID
            };
            break;
        case "GInfo": //커뮤니티 정보수정
            listPage = "../General/Info.aspx";

            ParamArray = {
                'communityID': CommunityID
            };
            break;
        case "GDel": //커뮤니티 해지신청
            listPage = "../General/Del.aspx";

            ParamArray = {
                'communityID': CommunityID
            };
            break;
        case "GMod": //시샵변경
            listPage = "../General/Mod.aspx";

            ParamArray = {
                'communityID': CommunityID,
                'adminCheck': AdminCheck
            };
            break;
        case "GPoint": //포인트관리
            listPage = "../General/Point.aspx";

            ParamArray = {
                'communityID': CommunityID
            };
            break;
        case "GGrade": //회원분류관리
            listPage = "../General/Grade.aspx";

            ParamArray = {
                'communityID': CommunityID
            };
            break;
        case "FMenu": //메뉴변경 및 권한설정
            listPage = "../Function/Menu/Add.aspx";

            ParamArray = {
                'communityID': CommunityID
            };
            break;
        case "FMain": //대문관리
            ParamArray = {
                'communityID': CommunityID
            };

            CallEncrypt(CallGetParam());
            Menu.ContentFrame.location.href = "../Function/Main/AddN.aspx?p=" + GetEncryptValue;
            return;           

            break;
        case "FPortal": //시작화면
            listPage = "../Function/Portal/Add.aspx";

            ParamArray = {
                'communityID': CommunityID
            };
            break;
        case "FSkin": //스킨관리
            listPage = "../Function/Skin/Title.aspx";

            ParamArray = {
                'communityID': CommunityID
            };
            break;
        case "GHome": // 홈이동
            listPage = "/DeskPlusEIP/Community/Site/Default.aspx";

            ParamArray = {
                'communityID': CommunityID,
                'grade': '2',
                'adminCheck': AdminCheck
            };
            CallEncrypt(CallGetParam());
            parent.location.href = listPage + "?p=" + GetEncryptValue;
            break;
        default:
            break;
    }

    if (listPage != "" && Menu.ContentFrame) {
        CallEncrypt(CallGetParam());
        Menu.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}
