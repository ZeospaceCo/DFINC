var Menu = {};

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

    });
});

// 메뉴 Accordion 초기화
$(function () {
    /*
    $(".accordion").accordion({
        fillSpace: true,
        animated: 'slide',
        active: AccordionActive,
        overTop: 55
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
        resize: function () {
            $("#accordion").accordion("resize");
        }
    });
});



/**************************************************************
리스트 페이지 호출하기
**************************************************************/
Menu.GoListPage = function (menuID) {
    var listPage = "";

    if (menuID == "") { menuID = "Approval"; }

    switch (menuID) {
        case "Approval": //전자결재
            listPage = "/DeskPlusEIP/Approval/Folder/Impo/List.aspx";
            break;

        case "Board": //게시판
            Menu.GetBoard("0", "IL");

            switch (ListTypeB) {
                case "1":
                    listPage = "/DeskPlusEIP/Board/Content/ListImpo.aspx";
                    break;
                case "2":
                    listPage = "/DeskPlusEIP/Board/Content/BlogListImpo.aspx";
                    break;
                case "3":
                    listPage = "/DeskPlusEIP/Board/Content/GalleryListImpo.aspx";
                    break;
                default:
                    listPage = "/DeskPlusEIP/Board/Content/ListImpo.aspx";
                    break;
            }
            break;

        case "Task": //업무협업
            listPage = "/DeskPlusEIP/Task/ImpoUser/List.aspx";
            break;

        case "WebHard": //웹폴더

            Menu.GetWebHard("0", "I");

            switch (ListTypeW) {
                case "1":
                    listPage = "/DeskPlusEIP/WebHard/Content/ListImpo.aspx";
                    break;
                case "2":
                    listPage = "/DeskPlusEIP/WebHard/Content/ListImpoIcon.aspx";
                    break;
                case "3":
                    listPage = "/DeskPlusEIP/WebHard/Content/ListImpoPreview.aspx";
                    break;
                default:
                    listPage = "/DeskPlusEIP/WebHard/Content/ListImpo.aspx";
                    break;
            }
            break;

        case "Address": //주소록

            Menu.GetAddress("0", "IG");

            var listPage = "/DeskPlusEIP/Address/Content/ListImpo.aspx";
            if (ListTypeA == "2") {
                listPage = "/DeskPlusEIP/Address/Content/ListBlogImpo.aspx";
            }

            break;

        case "Opinion": //설문조사
            listPage = "/DeskPlusEIP/Opinion/Content/FavList.aspx";
            break;      
                   
        default:
            break;
    }

    if (listPage != "") {
        parent.frmContent.location.href = listPage;
    }
}

/**************************************************
작성자 : famo23
작성일자 : 2017-03-16
작성목적 : 웹폴더 onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {

    var realFolderID = node.ID;
    var favoriteFolderID = node.GetProperty("FavoriteFolderID");
    var parentFavoriteID = node.GetProperty("ParentFavoriteID");
    var folderType = node.GetProperty("Type");

    if (folderType == "FC") {
        return;
    }
    else {
        if (favoriteFolderID > 0) {

            Menu.GetWebHard(favoriteFolderID, folderType);
            ParamArray = {
                'folderID': favoriteFolderID,
                'parentFavoriteID': parentFavoriteID,
                'favoriteID': realFolderID,
                'types': folderType,
                'pGubun': 'Y'
            };
        }
        else {
            Menu.GetWebHard(realFolderID, folderType);

            ParamArray = {
                'folderID': realFolderID,
                'types': folderType,
                'pGubun': 'Y'
            };
        }
    }

    var listPage = "/DeskPlusEIP/WebHard/Content/List.aspx";

    switch (ListTypeW) {
        case "1":
            listPage = "/DeskPlusEIP/WebHard/Content/List.aspx";
            break;
        case "2":
            listPage = "/DeskPlusEIP/WebHard/Content/ListIcon.aspx";
            break;
        case "3":
            listPage = "/DeskPlusEIP/WebHard/Content/ListPreview.aspx";
            break;
        default: 
            listPage = "/DeskPlusEIP/WebHard/Content/List.aspx";
            break;
    }
    

    if (listPage != "") {
        CallEncrypt(CallGetParam());
        parent.frmContent.location.href = listPage + "?p=" + GetEncryptValue;
    }

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-16
작성목적 : 웹폴더 사용자별 폴더 정보 가져오기 (웹서비스 호출)
**********************************************************************************************/
Menu.GetWebHard = function (p_folderID, p_types) {
    var url = "/DeskPlusEIP/WebHard/Ws/Folder/Tree.asmx/GetFolderUser";

    if (p_types == "M") {
        p_types = "P";
    }

    ParamArray = {
        'folderID': p_folderID,
        'types': p_types
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Menu.SuccessGetWebHard");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-16
작성목적 : 웹폴더 사용자별 폴더 정보 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Menu.SuccessGetWebHard = function (data, status) {
    var folderUser = data.d;

    if (data.d != "") {
        ListTypeW = folderUser.split(":")[0]; //리스트형식
    }
}


/**************************************************************
작성자   : famo23
작성일자 : 2017-03-16
작성목적 : 주소록 사용자별 카테고리 정보 가져오기
**************************************************************/
Menu.GetAddress = function (_categoryID, _types) {
    var url = "/DeskPlusEIP/Address/Ws/Folder/Tree.asmx/GetCategoryUser";

    ParamArray = {
        'categoryID': _categoryID,
        'types': _types
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Menu.SuccessGetAddress");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-16
작성목적 : 주소록 사용자별 카테고리 정보 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Menu.SuccessGetAddress = function (data, status) {
    var catUser = data.d;
    if (data.d != "") {
        ListTypeA = catUser.split(":")[0]; //리스트형식
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-16
작성목적 : 게시판 사용자별 카테고리 정보 가져오기
**********************************************************************************************/
Menu.GetBoard = function  (p_categoryID, p_types) {
    var url = "/DeskPlusEIP/Board/Ws/Category/Tree.asmx/GetCategoryUser";

    ParamArray = {
        'catID': p_categoryID,
        'types': p_types
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Menu.SuccessGetBoard");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-16
작성목적 : 게시판 사용자별 카테고리 정보 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Menu.SuccessGetBoard = function (data, status) {
    var catUser = data.d;
    if (data.d != "") {
        ListTypeB = catUser.split(":")[0]; //리스트형식
    }
}


/**************************************************
작성자 : famo23
작성일자 : 2017-03-20
작성목적 : 게시판 onBoardNodeSelect 트리 노드 선택
***************************************************/
function onBoardNodeSelect(node) {

    var CatID = node.GetProperty("CategoryID");

    //게시판 폴더
    if (CatID > 0) {
        var OptionUseDept = node.GetProperty("OptionUseDept");
        var MyAuthority = node.GetProperty("MyAuthority");
        var OptionListType = node.GetProperty("OptionListType");
        var OptionUseView = node.GetProperty("OptionUseView");
        var Types = node.GetProperty("Types"); //즐겨찾기,카테고리 구분자

        Menu.GetBoard(CatID, "BL"); //사용자설정 리스트형태

        var listPage = "/DeskPlusEIP/Board/Content/List.aspx";

        //전체용 리스트 선택시
        if (OptionListType == "0") {
            switch (ListTypeB) {
                case "1":
                    listPage = "/DeskPlusEIP/Board/Content/List.aspx";
                    break;
                case "2":
                    listPage = "/DeskPlusEIP/Board/Content/BlogList.aspx";
                    break;
                case "3":
                    listPage = "/DeskPlusEIP/Board/Content/GalleryList.aspx";
                    break;
                default: listPage = "/DeskPlusEIP/Board/Content/List.aspx";
                    break;
            }
        } else {
            switch (OptionListType) {
                case "1":
                    listPage = "/DeskPlusEIP/Board/Content/List.aspx";
                    break;
                case "2":
                    listPage = "/DeskPlusEIP/Board/Content/BlogList.aspx";
                    break;
                case "3":
                    listPage = "/DeskPlusEIP/Board/Content/GalleryList.aspx";
                    break;
                default: listPage = "/DeskPlusEIP/Board/Content/List.aspx";
                    break;
            }
        }

        //일반 게시판 경우, 권한 체크 하기
        if (OptionUseDept == "0") {
            var isAuthority = 0; //읽기권한이 없음

            if (MyAuthority != "") {
                var authStr = MyAuthority.split('/');

                //1:읽기, 2:쓰기, 3: 읽기+꼬리말쓰기, 4:읽기+쓰기+꼬리말쓰기
                if (authStr[0] == "true" && (authStr[1] == "1" || authStr[1] == "2" || authStr[1] == "3" || authStr[1] == "4")) {
                    isAuthority = 1; // 권한이 있으며, 읽기,쓰기 둘다 될 경우
                }
            }

            //폴더가 아니고, 권한이 없을 경우
            if (isAuthority != 1 && OptionUseView == "0") {
                alert(R_NoAuthority);
                return;
            }
            else // 권한이 있으며(폴더포함), 읽기,쓰기 둘다 될 경우
            {
                ParamArray = {
                    //'dcparm': Dcparm,
                    //'menuCatID': MenuCatID,
                    'catID': CatID,
                    //'favoriteID': FavoriteID,
                    'types': Types,
                    'pGubun': 'Y',
                    'kind': "HOME"
                };
            }
        }
        else if (OptionUseDept == "1") //부서별 게시판일 경우, 권한 체크 하지 않음(false 라도 상관없음)
        {
            ParamArray = {
                //'dcparm': Dcparm,
                //'menuCatID': MenuCatID,
                'catID': CatID,
                //'favoriteID': FavoriteID,
                'types': Types,
                'pGubun': 'Y',
                'kind': "HOME"
            };
        }

        if (listPage != "") {
            CallEncrypt(CallGetParam());
            parent.frmContent.location.href = listPage + "?p=" + GetEncryptValue;
        }
    }
}

