Menu = {}
var CallFirstSubMenu = false;
var DirectSubMenu = "";
var SubMenuPostion = 0;
var BeforeMenu = "";
/**************************************************
작성자   : shimpark
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        if (DirectCallMenuID == "") {
            //if (MenuID == "0") $(".menu_gnb a:first").click();

            $(".mmenu a:first").html("<em>" + $(".mmenu a:first").html() + "</em>");

            BeforeMenu = "#" + $(".mmenu a:first").attr("id");
        } else {
            Menu.DirectCall();
        }

        //확인
        $('#MyInfo').click(function (e) {
            e.preventDefault();
            Menu.MyUserInfo();
        });
        //상단나의정보
        $('#pro').click(function (e) {
            e.preventDefault();
            Menu.MyUserInfo();
        });


        $("#btnSearch").click(function (e) {
            e.preventDefault();
            Menu.Search();
        });

        $("#btnSearchDetail").click(function (e) {
            e.preventDefault();
            Menu.SearchDetail();
        });
        //Search버튼클릭
        $("#search-glass").click(function (e) {
            e.preventDefault();
            Menu.SearchDetail();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                Menu.Search();
            }
        });

        $("#ctbSearch").focus(function (e) {
            e.preventDefault();
            $("#ctbSearch").val("");
        });

        $("#ContextMenuUserDept").contextMenu({ menu: 'ContextSubMenuUserDept', leftButton: true }, function (action, el, pos) {

            //var pID = el[0].id.replace('ptConfig_', '');
            //BirthDay_ContextMenuWork(pID, action, el, pos);
        });

        //        $(document).mousemove(function (e) {
        //            MousePostionX = parseInt(e.pageX);
        //        });


        //스킨설정
        $("#SetSkin").click(function (e) {
            e.preventDefault();
            ParamArray = {
                'tabID': 0
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/EIP/Portal/GetSkin.aspx?p=" + GetEncryptValue;
            //PopupWin(600, 290, url, "", "", true);

            var WindowObjectReference = PopupWinOpen(url, "", "SETSKIN", "600", "280");
        });
        //상단스킨설정
        $("#sett").click(function (e) {
            e.preventDefault();
            ParamArray = {
                'tabID': 0
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/EIP/Portal/GetSkin.aspx?p=" + GetEncryptValue;
            //PopupWin(600, 290, url, "", "", true);

            var WindowObjectReference = PopupWinOpen(url, "", "SETSKIN", "600", "280");
        });

        //로그아웃
        $("#lgot").click(function (e) {
            e.preventDefault();
            location.href = "/DeskPlusEIP/Site/Auth/LogOut.aspx";
        });

        //admin 사용자 아닐경우 스타일시트 변경
        if ($('#lnkAdmin').length == 0) {
            $("#shtmn").removeClass("shtmn").addClass("shtmn2");
        }

        $("#button").toggle(
            function () {                
                $("#dropmenu").animate({ left: 0 }, 500);
                $("#effect").animate({ 'width': '100%' }, 500);
                Menu.TodayInfo();

                setTimeout('$("#button").css("background-image", "url(" + ImagePath + "/Package/idmoreon.png)");', 100);
            },
            function () {
                $('#ContextSubMenuUserDept').fadeOut(1);
                $("#dropmenu").animate({ left: -650 }, 500);
                $("#effect").animate({ 'width': '0px' }, 500);

                setTimeout('$("#button").css("background-image", "url(" + ImagePath + "/Package/idmore.png)");', 100);
            }
        );
    });
});


/**************************************************
작성자   : freeman
작성일자 : 2011-07-14
작성목적 : 메뉴 Today
***************************************************/
Menu.TodayInfo = function () {
    var url = "/DeskPlusEIP/Portlet/Ws/Package/MyInfo.asmx/GetMyInfo";
    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
    };

    var obj = eval("Menu.SuccessTodayInfo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-07-14
작성목적 : 
**********************************************************************************************/
Menu.SuccessTodayInfo = function (data, status) {
    $("#plTodayInfo").setTemplateURL('/DeskPlusEIP/EIP/Jt/Package/JTTodayInfo.htm?seq=' + Guids(), null, { filter_data: false });
    $("#plTodayInfo").processTemplate(data.d);

    Menu.TodayInfoResource();
}


/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-04-14
작성목적 : 다국어 
**********************************************************************************************/
Menu.TodayInfoResource = function () {


}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-04-14
작성목적 : 다국어 
**********************************************************************************************/
function MyInfo_ShortCut(id) {

    switch (id) {
        case "myInfo_NewMail":
            parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webmail";
            break;
        case "myInfo_TodayS":
            Menu.GoParam("webpims");
            parent.frmMain.location.href = "/DeskPlusEIP/Pim/Default.aspx";
            break;
        case "myInfo_TodayT":
            Menu.GoParam("webpims");
            parent.frmMain.location.href = "/DeskPlusEIP/Pim/Default.aspx";
            break;
        case "myInfo_Task":
            parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=TASK";
            break;
        case "myInfo_AppDoc":
            parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=approvalStatus";
            break;
        case "myInfo_AppApp":
            parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=approvalStatus";
            break;
        case "myInfo_AppDept":
            parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=approvalStatus";
            break;
        case "myInfo_AppCom":
            parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=approvalStatus";
            break;
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : UI를 위한 테스트 코드이므로 별도 개발 필요
***************************************************/
Menu.MyUserInfo = function () {
    var url = "/DeskPlusEIP/Account/MyInfo/Mod.aspx";
    var WindowObjectReference = PopupWinOpen(url, "", "MYINFO", "1000", "643");
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : UI를 위한 테스트 코드이므로 별도 개발 필요
***************************************************/
Menu.CallTopMenu = function (url, menuID, target) {
    MenuID = menuID;
    $('#plSubMenu').html("");
    var mnuID = "#mn_" + MenuID;
    //$(".menu_gnb a").removeAttr("class");      //Class 삭제
    //$(mnuID).attr("class", "menu_gnb_sel");  //부모 Class


    $(BeforeMenu).html($(BeforeMenu).text());
    BeforeMenu = mnuID;
    $(mnuID).html("<em>" + $(mnuID).html() + "</em>");

    if ($(mnuID).position() != null) {
        SubMenuPostion = $(mnuID).position().left;  //왼쪽 공간
    } else {
        SubMenuPostion = "250";
    }

    //$("#subMenu").attr("style", "position:absolute;top:38px;left:" + parseInt(SubMenuPostion) + "px;");

    if (url.trimText() != "") {
        Menu.OpenUrl(url, target, '');
        CallFirstSubMenu = false;
    } else {
        CallFirstSubMenu = true;
    }

    Menu.SubMenuList();
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 윈도우 창 
***************************************************/
Menu.OpenUrl = function (url, target, menuID) {

    if (TargetParam != "") {
        if (url.indexOf("?") > 0) {
            url += "&" + TargetParam;
            //TargetParam = "";
        } else {
            url += "?" + TargetParam;
            //TargetParam = "";
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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 하위메뉴 이동하기
***************************************************/
Menu.CallSubMenu = function (url, menuID, target) {

    //선택한 메뉴에 대한 Class 처리
    var mnuID = "#smn_" + menuID;
    $(".menu_sub a").removeAttr("class");      //서브 Class 삭제
    $(mnuID).attr("class", "menu_sub_sel");  //서브 Class

    Menu.OpenUrl(url, target, mnuID);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 서브 메뉴 리스트 호출
**********************************************************************************************/
Menu.SubMenuList = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Menu/Menu.asmx/GetSubMenuList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'menuID': MenuID
    };

    var obj = eval("Menu.SuccessSubMenuList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 리스트 표시
**********************************************************************************************/
Menu.SuccessSubMenuList = function (data, status) {
    $('#plSubMenu').setTemplateURL('/DeskPlusEIP/EIP/Jt/Menu/JTSubMenu.htm?seq=' + Guids(), null, { filter_data: false });
    $('#plSubMenu').processTemplate(data.d);

    //$(".menu_sub").attr("style", "padding-left:" + parseInt(SubMenuPostion) + "px");
    //Top Menu에서 Link가 존재 하지 않아서 첫번째 SubMenu 호출함
    if (CallFirstSubMenu) {
        $("#subMenu a:first").click();
    }

    //alert(parseInt(SubMenuPostion) + " : " +  parseInt($('#plSubMenu').width()));
    var subMenuWidth = parseInt(SubMenuPostion) + parseInt($('#plSubMenu').width());
    //alert(subMenuWidth + " : " + $(window).width());
    //메뉴를 오른쪽 끝으로 붙히기
    if (subMenuWidth > $(window).width()) {
        var subMenuNewPosition = parseInt($(window).width()) - parseInt($('#plSubMenu').width()) - 100;
        //alert(parseInt(subMenuNewPosition));
        $("#subMenu").attr("style", "position:absolute;top:38px;left:" + parseInt(subMenuNewPosition) + "px;");
    } else {
        $("#subMenu").attr("style", "position:absolute;top:38px;left:" + parseInt(SubMenuPostion) + "px;");
    }

    //DirectCall 일 경우 서브 메뉴 선택 표시 ByFreeman 20120116
    if (DirectSubMenu != "") {
        $(DirectSubMenu).click();
        DirectSubMenu = "";
        return;
    }

    //SubMenuPostion = $(mnuID).position().right;

}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
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
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 닫고, 포탈 페이지 리로드
***************************************************/
Menu.ReLoad = function () {
    top.location = "/DeskPlusEIP/";
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-01-16
작성목적 : 직접 호출
***************************************************/
Menu.DirectCall = function () {
    if (DirectCallMenuID == "") return;

    var dArr = DirectCallMenuID.split("|");
    var topMenuID = "#mn_" + dArr[0]; //메인메뉴 ID
    if (dArr[1] != "") DirectSubMenu = "#smn_" + dArr[1];  //서버 메뉴 ID

    $(topMenuID).click(); //메인 메뉴 Click으로 서브메뉴 가져오기
}



/**************************************************
작성자   : 이현수
작성일자 : 2012-01-27
작성목적 : 검색  (일반검색 : 제목/첨부파일명만 검색 /  검색엔진 : 상황에 따라)
***************************************************/
Menu.Search = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbSearch", R_InputSearchValue, true);
    if (!validator.validate()) return;

    var searchVal = $("#ctbSearch").val();

    ParamArray = {
        'searchValue': searchVal
    };

    CallEncrypt(CallGetParam());
    var param = "p=" + GetEncryptValue;

    //DB 검색
    parent.frmMain.location.href = "/DeskPlusEIP/EIP/Search/Default.aspx?" + param;
    //검색엔진
    //parent.frmMain.location.href = "/DeskPlusEIP/Search/Default.aspx?" + param;
}


/**************************************************
작성자   : 이현수
작성일자 : 2012-01-27
작성목적 : 상세 검색 페이지로 이동
***************************************************/
Menu.SearchDetail = function () {
    //DB 검색
    parent.frmMain.location.href = "/DeskPlusEIP/EIP/Search/Default.aspx"
    //검색엔진
    //parent.frmMain.location.href = "/DeskPlusEIP/Search/Default.aspx"
}

/**************************************************
작성자   : 심재운
작성일자 : 2012-01-27
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

    //    $.each(ParamArray, function (key, value) {
    //        alert(key + ': ' + value);
    //    });

    //    return;

    var obj = eval("Menu.SuccessSetBaseDept");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

    Menu.CallSLO(companyID, userID, loginID);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시  로그인 체크
**********************************************************************************************/
Menu.SuccessSetBaseDept = function (data, status) {
    if (data.d == true) {

    }
    else {
        alert(R_ProcessingError);
    }
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2012-03-20
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
작성자   : freeman
작성일자 : 2012-03-20
작성목적 : 성공시 메뉴 선택 표시
**********************************************************************************************/
Menu.SuccessGoParam = function (data, status) {
    for (var count in data.d) {
        var menuID = data.d[count].MenuID;
        var parentMenuID = data.d[count].ParentMenuID

        if (parentMenuID > 0) { //서브메뉴 호출
            var mnuID = "#mn_" + parentMenuID;
            //            $(".menu_gnb a").removeAttr("class");      //Class 삭제
            //            $(mnuID).attr("class", "menu_gnb_sel");  //부모 Class

            //            var posLeft = $(mnuID).position().left;  //왼쪽 공간 띄우기


            $(BeforeMenu).html($(BeforeMenu).text());
            BeforeMenu = mnuID;
            $(mnuID).html("<em>" + $(mnuID).html() + "</em>");

            //SubMenuPostion = $(mnuID).position().left;  //왼쪽 공간

            //$("#subMenu").attr("style", "position:absolute;top:38px;left:" + parseInt(SubMenuPostion) + "px;");

            //alert(parseInt(SubMenuPostion) + " : " +  parseInt($('#plSubMenu').width()));
            var subMenuWidth = parseInt(SubMenuPostion) + parseInt($('#plSubMenu').width());
            //alert(subMenuWidth + " : " + $(window).width());
            //메뉴를 오른쪽 끝으로 붙히기
            if (subMenuWidth > $(window).width()) {
                var subMenuNewPosition = parseInt($(window).width()) - parseInt($('#plSubMenu').width()) - 100;
                //alert(parseInt(subMenuNewPosition));
                $("#subMenu").attr("style", "position:absolute;top:38px;left:" + parseInt(subMenuNewPosition) + "px;");
            } else {
                $("#subMenu").attr("style", "position:absolute;top:38px;left:" + parseInt(SubMenuPostion) + "px;");
            }


            var url = "/DeskPlusEIP/EIP/Ws/Menu/Menu.asmx/GetSubMenuList";

            ParamArray = {
                'menuID': parentMenuID
            };

            CallEncrypt(CallGetParam());
            var prarmData = "{p:'" + GetEncryptValue + "'}";

            jQuery.ajax({
                type: "POST",  //GET 
                url: url,
                async: true,
                data: prarmData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data != null) {
                        $('#plSubMenu').setTemplateURL('/DeskPlusEIP/EIP/Jt/Menu/JTSubMenu.htm?seq=' + Guids(), null, { filter_data: false });
                        $('#plSubMenu').processTemplate(data.d); //서브메뉴 바인딩

                        //                        $(".menu_sub").attr("style", "padding-left:" + parseInt(posLeft) + "px");

                        //선택한 메뉴에 대한 Class 처리
                        //                        var mnuID = "#smn_" + menuID;
                        //                        $(".menu_sub a").removeAttr("class");      //서브 Class 삭제
                        //                        $(mnuID).attr("class", "menu_sub_sel");  //서브 Class

                    } else {

                    }
                },
                error: function () {

                }
            });

        } else { //TopMenu일 경우
            var mnuID = "#mn_" + menuID;
            $(BeforeMenu).html($(BeforeMenu).text());
            BeforeMenu = mnuID;
            $(mnuID).html("<em>" + $(mnuID).html() + "</em>");
            //            $(".menu_gnb a").removeAttr("class");      //Class 삭제
            //            $(mnuID).attr("class", "menu_gnb_sel");  //부모 Class
            //$('#plSubMenu').html(""); //이전 서브 메뉴 삭제

        }

        break; //DirectCall이 중복되더라도 맨처음 한건에 대해서만 처리
    }
}