Tree = {};
Tree.ContentFrame = null;

$(function () {
    $(document).ready(function () {

        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            overTop: 0,
            active: 0
        });

        Tree.ContentFrame = parent.frmContent;

        //임시 게시물 추가
        $('#register').click(function (e) {
            e.preventDefault();

            if (Tree.ContentFrame) {
                Tree.ContentFrame.location.href = "../Content/AddN.aspx";
            }
        });

        //Tree.Init();

    });
});



/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 홈화면 이동
***************************************************/
Tree.Init = function () {
    var listPage = "./Content.aspx";

    if (listPage != "" && Tree.ContentFrame) {
        Tree.ContentFrame.location.href = listPage;
    }
}


/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {   

    Tree.SelectedNodeID = node.ID;

    CatID = node.GetProperty("CategoryID");
    FavoriteID = node.GetProperty("FavoriteID");

    //게시판 폴더 및 임시게시판일 경우
    if (CatID > 0 || CatID == -1) {
        OptionUseDept = node.GetProperty("OptionUseDept");
        MyAuthority = node.GetProperty("MyAuthority");
        OptionListType = node.GetProperty("OptionListType");
        OptionUseView = node.GetProperty("OptionUseView");
        Types = node.GetProperty("Types"); //즐겨찾기,카테고리 구분자

        GetCategoryUser(CatID, "BL"); //사용자설정 리스트형태

        GoListPage();

        //즐겨찾기 폴더내부에 일반폴더 존재할 경우, 삭제 가능토록 하기
        if (FavoriteID != "0") {
            $("#imgDel").show();
        }
    }
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 사용자별 카테고리 정보 가져오기(BL : 게시판 TL : 임시저장 IL : 중요표시)
**********************************************************************************************/
function GetCategoryUser(p_categoryID, p_types) {
    var url = "/DeskPlusEIP/Board/Ws/Category/Tree.asmx/GetCategoryUser";

    ParamArray = {
        'catID': p_categoryID,
        'types': p_types
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("SuccessCatUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 카테고리별 사용자 정보 저장하기
**********************************************************************************************/
function SuccessCatUser(data, status) {
    var catUser = data.d;
    if (data.d != "") {
        ListTypeUser = catUser.split(":")[0]; //리스트형식
    }
}



/**************************************************************
리스트 페이지 호출하기
**************************************************************/
function GoListPage() {
    var listPage = "../Content/List.aspx";

    if (CatID > 0) {
        //전체용 리스트 선택시
        if (OptionListType == "0") {
            switch (ListTypeUser) {
                case "1":
                    listPage = "../Content/List.aspx";
                    break;
                case "2":
                    listPage = "../Content/BlogList.aspx";
                    break;
                case "3":
                    listPage = "../Content/GalleryList.aspx";
                    break;
                default: listPage = "../Content/List.aspx";
                    break;
            }
        } else {
            switch (OptionListType) {
                case "1":
                    listPage = "../Content/List.aspx";
                    break;
                case "2":
                    listPage = "../Content/BlogList.aspx";
                    break;
                case "3":
                    listPage = "../Content/GalleryList.aspx";
                    break;
                default: listPage = "../Content/List.aspx";
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
                    'dcparm': Dcparm,
                    'menuCatID': MenuCatID,
                    'catID': CatID,
                    'favoriteID': FavoriteID,
                    'types': Types,
                    'kind': "HOME"
                };
            }
        }
        else if (OptionUseDept == "1") //부서별 게시판일 경우, 권한 체크 하지 않음(false 라도 상관없음)
        {
            ParamArray = {
                'dcparm': Dcparm,
                'menuCatID': MenuCatID,
                'catID': CatID,
                'favoriteID': FavoriteID,
                'types': Types,
                'kind': "HOME"
            };
        }
    }
    else if (CatID == "-1") //임시저장 누를 경우
    {
        Tree.GoTempPage();
        return;
    }
    else {
        ParamArray = {
            'dcparm': Dcparm,
            'menuCatID': "0",
            'catID': "0",
            'types': Types,
            'kind': "HOME"
        };
    }

    //    alert("menuCatID : " + MenuCatID);
    //    alert("catID : " + CatID);

    if (listPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location = listPage + "?p=" + GetEncryptValue;
    }
}



/**************************************************************
작성자 :
작성일자 : 2011-10-17
작성목적 : 임시저장 페이지 이동하기
**************************************************************/
Tree.GoTempPage = function () {
    GetCategoryUser("0", "TL");

    var listPage = "";

    switch (ListTypeUser) {
        case "1":
            listPage = "../Temp/List.aspx";
            break;
        case "2":
            listPage = "../Temp/BlogList.aspx";
            break;
        case "3":
            listPage = "../Temp/GalleryList.aspx";
            break;
        default: listPage = "../Temp/List.aspx";
            break;
    }

    ParamArray = {
        'dcparm': Dcparm,
        'menuCatID': "0",
        'catID': "0"
    };

    if (listPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location = listPage + "?p=" + GetEncryptValue;
    }
}


/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}