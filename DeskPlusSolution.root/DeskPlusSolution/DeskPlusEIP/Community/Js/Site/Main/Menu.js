Menu = {}
Menu.ContentFrame = parent.frmContent;
Menu.TreeFrame = parent.frmLeft;

/**************************************************
작성자 : famo23
작성일자 : 2012-05-10
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        // 커뮤니티 바로가기
        $('#ddlMyCommunity').change(function () {
            Menu.AddVisitPoint();
        });

        // 홈
        $('#aContent').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('0', 'Content');
        });

        // 회원검색
        $('#aSearch1').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('0', 'Search');
        });

        $('#aSearch2').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('0', 'Search');
        });

        // 커뮤니티 홈
        $('#aMain1').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('0', 'Main');
        });

        $('#aMain2').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('0', 'Main');
        });

        $('#aMain3').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('0', 'Main');
        });

        // 가입하기 
        $('#aJoin').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('0', 'Join');
        });

        // 관리자 메뉴 
        $('#aAdmin').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('0', 'Admin');
        });

        // 바로가기 
        $('.aClipboard').click(function (e) {
            $('#tbGoUrl').val($('#ShortCutURL').text());
            $('#tbGoUrl').select(); //복사할 텍스트를 선택
            document.execCommand("copy") //클립보드 복사 실행
            alert(R_ClipBoard);
        });


        Menu.Init();
    });
});

/**************************************************************
작성자 : famo23
작성일자 : 2012-05-10
작성목적 : 초기화
**************************************************************/
Menu.Init = function () {
    Menu.GoListPage('0', 'Content');

    if (Grade == "10") {
        $("#dvUser").css("display", "none");
        $("#dvJoin").css("display", "");
        $("#dvAdmin").css("display", "none");
    }
    else if (Grade == "2") {
        $("#dvUser").css("display", "none");
        $("#dvJoin").css("display", "none");
        $("#dvAdmin").css("display", "");
    }
    else {
        $("#dvUser").css("display", "");
        $("#dvJoin").css("display", "none");
        $("#dvAdmin").css("display", "none");
    }

    /*
    $('.aClipboard').zclip({
        path: '../../../Html/Js/JClip/ZeroClipboard.swf',
        copy: $('#ShortCutURL').text(),
        afterCopy: function () {
            alert(R_ClipBoard);
        }
    });
    */
}

/**************************************************************
작성자 : famo23
작성일자 : 2012-05-10
작성목적 : 페이지 호출
**************************************************************/
Menu.GoListPage = function (menuID, type) {

    var listPage = "Content.aspx";

    if (type == "") {
        type = "Content";
    }


    switch (type) {
        case "Content": //메인
            listPage = "Content.aspx";

            ParamArray = {
                'communityID': CommunityID,
                'grade': Grade
            };

            break;
        case "Board": //공지사항/게시판/자료실 형식 
            listPage = "../Apps/Board/List.aspx";

            ParamArray = {
                'communityID': CommunityID,
                'menuID': menuID,
                'grade': Grade,
                'listType': type
            };
            break;
        case "Album": //앨범 형식             
            listPage = "../Apps/Board/ListGallery.aspx";

            ParamArray = {
                'communityID': CommunityID,
                'menuID': menuID,
                'grade': Grade,
                'listType': type
            };
            break;
        case "Schedule": //일정관리
            listPage = "../Apps/Schedule/List.aspx";

            ParamArray = {
                'communityID': CommunityID,
                'menuID': menuID,
                'grade': Grade
            };
            break;
        case "Search": //회원정보 검색            
            listPage = "./Search/List.aspx";

            ParamArray = {
                'communityID': CommunityID,
                'grade': Grade
            };
            break;
        case "Join": // 가입하기
            listPage = "Join.aspx";

            ParamArray = {
                'communityID': CommunityID,
                'grade': Grade
            };
            break;
        case "Main": // 커뮤니티 홈
            listPage = "../../Default.aspx";
            CallEncrypt(CallGetParam());
            parent.location.href = listPage + "?p=" + GetEncryptValue;
            break;
        case "Admin": //관리자
            listPage = "../Admin/Default.aspx";

            ParamArray = {
                'communityID': CommunityID,
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

/**************************************************************
작성자 : famo23
작성일자 : 2015-07-28
작성목적 : 커뮤니티 바로가기
**************************************************************/
Menu.AddVisitPoint = function () {

    var tmpVal = $("#ddlMyCommunity").val();
    var arrVal = tmpVal.split('^');

    var url = "/DeskPlusEIP/Community/Ws/Site/Main/Menu.asmx/AddVisitPoint";
    ParamArray = {
        'communityID': arrVal[0],
        'type': '6',
        'menuID': '0'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Menu.SuccessAddVisitPoint");

    AjaxService(url, data, obj);

}


/**************************************************************
작성자 : famo23
작성일자 : 2012-05-10
작성목적 : 커뮤니티 바로가기
**************************************************************/
Menu.SuccessAddVisitPoint = function (data, status) {

    if (data.d) {

        var tmpVal = $("#ddlMyCommunity").val();
        var arrVal = tmpVal.split('^');

        var tmpGrade = arrVal[1];
        if (AdminCheck == "Y") {
            tmpGrade = "2";
        }

        ParamArray = {
            'communityID': arrVal[0],
            'grade': tmpGrade,
            'adminCheck': AdminCheck
        };

        CallEncrypt(CallGetParam());
        parent.location.href = "/DeskPlusEIP/Community/Site/Default.aspx?p=" + GetEncryptValue;

    }
    else {
        alert(R_ProcessingError);
        return;
    }
}

/**************************************************************
작성자 : famo23
작성일자 : 2012-06-20
작성목적 : 커뮤니티 권한 
**************************************************************/
Menu.NoAuth = function () {

    alert(R_NoReadAuth);
    return;
}
