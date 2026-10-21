Menu = {}
var CallFirstSubMenu = false;
var DirectSubMenu = "";
Menu.IsDirectCallOpening = false;
/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //메뉴오픈
        Menu.DirectCall();

        //오늘의 카운트 (1분간격)
        Menu.TodayInfo();

        setInterval(function () {
            Menu.TodayInfo();
        }, 1000 * 1 * 60);

        $('#mnav > li >  a').mouseover(function (e) {
            $(this).parent().addClass('over').siblings().removeClass('over');
            var index = $(this).parent('li').index();
            //console.log(index);
            $.cookie('whatover', index);
            $('.dropdown-menu').removeClass('show');
            DropSetting = "N";
        });

        //사용자정보
        $('#pro').click(function (e) {
            e.preventDefault();
            Menu.MyUserInfo();
        });

        //Search버튼클릭
        $("#search-glass").click(function (e) {
            e.preventDefault();
            Menu.SearchDetail();
        });

        $("#ContextMenuUserDept").contextMenu({ menu: 'ContextSubMenuUserDept', leftButton: true }, function (action, el, pos) {

        });

        //스킨설정
        $("#sett").click(function (e) {
            e.preventDefault();
            ParamArray = {
                'tabID': 0
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/EIP/Portal/GetSkin.aspx?p=" + GetEncryptValue;
            var WindowObjectReference = PopupWinOpen(url, "", "SETSKIN", "600", "280");
        });

        //로그아웃
        $("#lgot").click(function (e) {
            e.preventDefault();
            location.href = "/DeskPlusEIP/Site/Auth/LogOut.aspx";
        });

        //환경설정클릭
        $("#navbarDropdownMenuLink").click(function (e) {
            e.preventDefault();

            if (DropSetting == "N") {
                $('.dropdown-menu').addClass('show');
                DropSetting = "Y";
            }
            else {
                $('.dropdown-menu').removeClass('show');
                DropSetting = "N";
            }
        });

        //Home CI Reload
        $("#HomeReload").click(function (e) {
            e.preventDefault();
            parent.frmTop.location.href = "/DeskPlusEIP/EIP/Package/NMenu.aspx?d=homeFa";
            parent.frmMain.location.href = "../Package/NMain.aspx";
        });

        $('#HomeReload').mouseover(function (e) {
            $("#mn_" + HomeID).parent().addClass('over').siblings().removeClass('over');
            var index = $(this).parent('li').index();
            //console.log(index);
            $.cookie('whatover', index);
            $('.dropdown-menu').removeClass('show');
            DropSetting = "N";
        });

    });
});


/**********************************************************************************************
작성목적 : 다국어 
**********************************************************************************************/
function MyInfo_ShortCut(id) {

    switch (id) {
        case "myInfo_NewMail":
            parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webmail";
            break;
        case "myInfo_TodayS":
            parent.frmMain.location.href = "/DeskPlusEIP/Pim/Default.aspx?dcparm=webpims";
            break;
        case "myInfo_Task":
            parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=TASK";
            break;
        case "myInfo_AppApp":
            parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=approvalHome";
            break;
    }
}

/**************************************************
작성목적 : 사용자정보
***************************************************/
Menu.MyUserInfo = function () {
    var url = "/DeskPlusEIP/Account/MyInfo/Mod.aspx";
    var WindowObjectReference = PopupWinOpen(url, "", "MYINFO", "1000", "700");    
}

/**************************************************
작성목적 : 직접 호출
***************************************************/
Menu.DirectCall = function () {
    if (DirectCallMenuID == "") return;

    var dArr = DirectCallMenuID.split("|");
    var topMenuID = "#mn_" + dArr[0]; //메인메뉴 ID
    if (dArr[1] != "") DirectSubMenu = "#smn_" + dArr[1];  //서버 메뉴 ID    

    Menu.IsDirectCallOpening = dArr[1] == "";
    $(topMenuID).click(); //메인 메뉴 Click으로 서브메뉴 가져오기
    Menu.ClearApprovalStatusDirectCall();
    Menu.IsDirectCallOpening = false;

    $(topMenuID).parent().addClass('over').siblings().removeClass('over');
    var index = $(topMenuID).parent('li').index();
    $.cookie('whatover', index);
    $('.dropdown-menu').removeClass('show');
    DropSetting = "N";
}

Menu.ClearApprovalStatusDirectCall = function () {
    var directCall = (DirectCall || "").toLowerCase();
    if (directCall != "approvalstatus" && directCall != "approvalcomplete") return;
    if (window.parent == window || !parent.history || !parent.history.replaceState) return;

    parent.history.replaceState(null, parent.document.title, parent.location.pathname);
}

/**************************************************
작성목적 : top 메뉴 클릭
***************************************************/
Menu.CallTopMenu = function (url, menuID, target) {
    MenuID = menuID;

    if (url.trimText() != "" && DirectSubMenu == "") {
        Menu.OpenUrl(url, target, '');
        CallFirstSubMenu = false;
    } else {
        CallFirstSubMenu = true;
    }

    if (CallFirstSubMenu) {
        $("#umenu_" + MenuID + " > li:eq(1) > a").click();
    }
    
    if (DirectSubMenu != "") {
        Menu.IsDirectCallOpening = true;
        $(DirectSubMenu).click();
        Menu.IsDirectCallOpening = false;
        DirectSubMenu = "";
        return;
    }
}

/**************************************************
작성목적 : 윈도우 창 
***************************************************/
Menu.OpenUrl = function (url, target, menuID) {

    if (DirectCall == "homeFa") {
        DirectCall = "";
        return;
    }

    if (DirectCall == "FirstMenu") {
        DirectCall = "";
        if (parent.frmMain.location.href.indexOf("/NMain.aspx") > 0) {
            return;
        }
    }

    if (Menu.IsDirectCallOpening && ApprovalStatusNewMenuDirectCall) {
        var queryIndex = url.indexOf("?");
        url = "/DeskPlusEIP/Approval/Status/DefaultAll.aspx" + (queryIndex >= 0 ? url.substring(queryIndex) : "");
    }

    if (TargetParam != "") {
        if (url.indexOf("?") > 0) {
            url += "&" + TargetParam;
        } else {
            url += "?" + TargetParam;
        }

        if (!CallFirstSubMenu) {
            // TopMenu에서 오픈되었을 때 
            TargetParam = "";
        }
        else {
            // SubMenu에서 DirectCall일때
            if (menuID == DirectSubMenu) {
                TargetParam = "";
            }
        }
    }

    switch (target) {
        case "1":
            //하단프레임
            parent.frmMain.location.href = url;
            break;
        case "2":
            //새창
            WindowObjectReference = PopupWinOpen(url, "", "OpenUrl", "1000", "618", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");

            break;
        case "3":
            //전체
            parent.location.href = url;
            break;
        default:
            //하단프레임
            parent.frmMain.document.location.href = url;
            break;
    }
}

/**************************************************
작성목적 : 하위메뉴 이동하기
***************************************************/
Menu.CallSubMenu = function (url, menuID, target) {

    var mnuID = "#dsmn_" + menuID;

    if (url.trimText() != "") {
        Menu.OpenUrl(url, target, mnuID);
        CallFirstSubMenu = false;
    } else {
        CallFirstSubMenu = true;
    }

    if (CallFirstSubMenu) {
        $("#umenu_" + menuID + " > li:eq(1) > a").click();
    }

}

/**************************************************
작성목적 : 겸직 수정
***************************************************/
Menu.SetBaseDept = function (companyID, userID, deptID, titleID, dutyID, loginID) {

    var url = "/DeskPlusEIP/EIP/Ws/Menu/Menu.asmx/ModUserBaseDept";

    ParamArray = {
        'companyID': companyID,
        'userID': userID,
        'deptID': deptID,
        'titleID': titleID,
        'dutyID': dutyID
    };

    var obj = eval("Menu.SuccessSetBaseDept");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

    Menu.CallSLO(companyID, userID, loginID);
}

/**********************************************************************************************
작성목적 : 성공시  로그인 체크
**********************************************************************************************/
Menu.SuccessSetBaseDept = function (data, status) {
    if (data.d == true) {

    }
    else {
        alert(R_ProcessingError);
    }
}

/**************************************************
작성목적 : 닫고, 포탈 페이지 리로드
***************************************************/
Menu.CallSLO = function (companyID, userID, loginID) {

    var url = "/DeskPlusEIP/Account/MyInfo/SLO.aspx";

    ParamArray = {
        'companyID': companyID,
        'userID': userID,
        'loginID': loginID
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    url = url + "?p=" + GetEncryptValue;

    $("#ifrmSLO").attr("src", url);
}



/**************************************************
작성목적 : 상세 검색 페이지로 이동
***************************************************/
Menu.SearchDetail = function () {
    //DB 검색
    parent.frmMain.location.href = "/DeskPlusEIP/EIP/Search/Default.aspx"
    //검색엔진
    //parent.frmMain.location.href = "/DeskPlusEIP/Search/Default.aspx"
}

/**************************************************
작성목적 : 메뉴 Today
***************************************************/
Menu.TodayInfo = function () {
    var url = "/DeskPlusEIP/Portlet/Ws/Package/MyInfo.asmx/GetMyInfo";
    ParamArray = {
    };

    var obj = eval("Menu.SuccessTodayInfo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성목적 : 메뉴 Today
**********************************************************************************************/
Menu.SuccessTodayInfo = function (data, status) {

    var obj = data.d;
    $.each(obj, function (index, objInfo) {
        $("#MCnt1").text(Number(objInfo.NewMailStr));
        $("#MCnt2").text(Number(objInfo.TodayPimSchStr));
        $("#MCnt3").text(Number(objInfo.TaskStr));
        $("#MCnt4").text(Number(objInfo.ApprovalStr));

        if (Number(objInfo.NewMailStr) > 0) {
            $("#MCnt1").addClass("badge number badge-light");
            $("#MCnt1").text(Number(objInfo.NewMailStr));
        }
        else {
            $("#MCnt1").removeClass();
            $("#MCnt1").text('');
        }

        if (Number(objInfo.TodayPimSchStr) > 0) {
            $("#MCnt2").addClass("badge number badge-light");
            $("#MCnt2").text(Number(objInfo.TodayPimSchStr));
        }
        else {
            $("#MCnt2").removeClass();
            $("#MCnt2").text('');
        }

        if (Number(objInfo.TaskStr) > 0) {
            $("#MCnt3").addClass("badge number badge-light");
            $("#MCnt3").text(Number(objInfo.TaskStr));
        }
        else {
            $("#MCnt3").removeClass();
            $("#MCnt3").text('');
        }

        if (Number(objInfo.ApprovalStr) > 0) {
            $("#MCnt4").addClass("badge number badge-light");
            $("#MCnt4").text(Number(objInfo.ApprovalStr));
        }
        else {
            $("#MCnt4").removeClass();
            $("#MCnt4").text('');
        }
    });

}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 닫고, 포탈 페이지 리로드
***************************************************/
Menu.ReLoad = function () {
    top.location = "/DeskPlusEIP/";
}

/**********************************************************************************************
작성목적 : dcparam 으로 메뉴 정보 가져오기
**********************************************************************************************/
Menu.GoParam = function (dcparam) {
    var url = "/DeskPlusEIP/EIP/Ws/Menu/Menu.asmx/GetMenuByDirectCall";

    ParamArray = {
        'companyID': CompanyID,
        'dcparam': dcparam
    };

    var obj = eval("Menu.SuccessGoParam");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성목적 : 성공시 메뉴 선택 표시
**********************************************************************************************/
Menu.SuccessGoParam = function (data, status) {
    for (var count in data.d) {
        var menuID = data.d[count].MenuID;
        var parentMenuID = data.d[count].ParentMenuID;

        if (parentMenuID > 0) {

            var topMenuID = "#mn_" + parentMenuID; //메인메뉴 ID

            $(topMenuID).parent().addClass('over').siblings().removeClass('over');
            var index = $(topMenuID).parent('li').index();
            $.cookie('whatover', index);
            $('.dropdown-menu').removeClass('show');
            DropSetting = "N";

        }
        else {
            //TopMenu일 경우
            var topMenuID = "#mn_" + menuID; //메인메뉴 ID

            $(topMenuID).parent().addClass('over').siblings().removeClass('over');
            var index = $(topMenuID).parent('li').index();
            $.cookie('whatover', index);
            $('.dropdown-menu').removeClass('show');
            DropSetting = "N";
        }

        break; //DirectCall이 중복되더라도 맨처음 한건에 대해서만 처리
    }
}

/**************************************************
작성목적 : 즐겨찾기 삭제
***************************************************/
Menu.DelFaMenu = function (menuID) {

    if (window.confirm(R_ConfirmDeleteMenu)) {

        TmpMenu = "#lifa_" + menuID;

        var url = "/DeskPlusEIP/EIP/Ws/Menu/FaMenu.asmx/DelFaMenu";

        ParamArray = {
            'companyID': CompanyID,
            'userID': UserID,
            'menuID': menuID
        };

        var obj = eval("Menu.SuccessDelFaMenu");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성목적 : 성공시  즐겨찾기 체크
**********************************************************************************************/
Menu.SuccessDelFaMenu = function (data, status) {
    if (data.d == true) {
        $(TmpMenu).remove();
        TmpMenu = "";
    }
    else {
        alert(R_ProcessingError);
    }
}

/**************************************************
작성목적 : 즐겨찾기 설정
***************************************************/
Menu.SettingFaMenu = function () {

    var url = "/DeskPlusEIP/EIP/Menu/FaMenu.aspx";
    var WindowObjectReference = PopupWinOpen(url, "", "FaMenu", "500", "400");

}

/**************************************************
작성목적 : 페이지 리로드
***************************************************/
function MenuReLoad() {
    parent.frmTop.location.href = "/DeskPlusEIP/EIP/Package/NMenu.aspx?d=homeFa";
}
