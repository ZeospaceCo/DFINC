Menu = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        var accordionIdx = 0;
        if (IsTotBoard == "1") {
            $(".IsTotBoardManager").show()
            accordionIdx = 2;
        }
        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            overTop: 55,
            active: accordionIdx
            //event: "mouseover",
        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () {
                $("#accordion").accordion("resize");
            }
        });

        //템플릿설정
        $('#tempManage').click(function () {
            parent.frmContent.location.href = "/DeskPlusEIP/Board/Admin/Template/List.aspx";
        });

        //템플릿설정
        $('#tempManageSub').click(function () {
            parent.frmContent.location.href = "/DeskPlusEIP/Board/Admin/Template/List.aspx";
        });

        //환경설정
        $('#configManage').click(function () {
            parent.frmContent.location.href = "/DeskPlusEIP/Board/Admin/Config/Default.aspx";
        });

        //환경설정
        $('#configManageSub').click(function () {
            parent.frmContent.location.href = "/DeskPlusEIP/Board/Admin/Config/Default.aspx";
        });

        //게시물조회
        $('#readCount').click(function () {
            parent.frmContent.location.href = "/DeskPlusEIP/Board/Admin/Statistic/ReadCount.aspx";
        });

        //게시물등록
        $('#writeCount').click(function () {
            parent.frmContent.location.href = "/DeskPlusEIP/Board/Admin/Statistic/WriteCount.aspx";
        });

        //통계
        $('#chart').click(function () {
            parent.frmContent.location.href = "/DeskPlusEIP/Board/Admin/Statistic/LineChart.aspx";
        });

        //통계
        $('#chartSub').click(function () {
            parent.frmContent.location.href = "/DeskPlusEIP/Board/Admin/Statistic/LineChart.aspx";
        });

        //추가
        $('#imgAdd').click(function () {
            Menu.FolderAdd();
        });

        //삭제
        $('#imgDel').click(function () {
            Menu.FolderDel();
        });

        //새로고침
        $('#imgReload').click(function () {
            location = location.href;
        });

        //통합관리자에게 통합게시판관리화면을 제공합니다. shimpark
        if (IsTotAccount == "0") $(".IsTotBoardManager").hide();

        Menu.Init();
    });
});

/**************************************************************
작성자   : shimpark
작성일자 : 2008-08-11
작성목적 : 폴더 초기화
**************************************************************/
Menu.Init = function () {
    $("#imgDel").hide();
    $("#tbHdnValue").val("");

    if (CatID != "0") {
        $("#imgAdd").show();
        $("#imgDel").show();
    }

    /*
    //파라미터값 배열에 담기
    ParamArray = {
        'menuCatID': MenuCatID,
        'catID': CatID
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    var pageList = (IsTotBoard == "1") ? "ModTot.aspx" : "Mod.aspx";

    parent.frmContent.location.href = "/DeskPlusEIP/Board/Admin/Category/" + pageList + "?p=" + GetEncryptValue;
    */
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 폴더 수정 페이지 이동
***************************************************/
function onNodeSelect(node) {
    IsTotBoard = "0";

    $("#imgAdd").show();
    $("#imgDel").show();

    OptionUseDept = node.GetProperty("OptionUseDept");
    CatID = node.ID;

    //파라미터값 배열에 담기
    ParamArray = {
        'menuCatID': MenuCatID,
        'catID': CatID
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    parent.frmContent.location.href = "/DeskPlusEIP/Board/Admin/Category/Mod.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 통합게시판 폴더 선택시
***************************************************/
function onNodeTotSelect(node) {
    IsTotBoard = "1";

    $("#imgAdd").show();
    $("#imgDel").show();

    OptionUseDept = node.GetProperty("OptionUseDept");
    CatID = node.ID;

    //파라미터값 배열에 담기
    ParamArray = {
        'menuCatID': MenuCatID,
        'catID': CatID
    };
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    parent.frmContent.location.href = "/DeskPlusEIP/Board/Admin/Category/ModTot.aspx?p=" + GetEncryptValue;
}

/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 폴더 추가
***************************************************/
Menu.FolderAdd = function () {
    //파라미터값 배열에 담기
    ParamArray = {
        'menuCatID': MenuCatID,
        'pCatID': CatID,
        'optionUseDept': OptionUseDept
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    var pageList = (IsTotBoard == "1") ? "ModTot.aspx" : "Mod.aspx";

    parent.frmContent.location.href = "/DeskPlusEIP/Board/Admin/Category/" + pageList + "?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 폴더 삭제
***************************************************/
Menu.FolderDel = function () {
    Menu.FolderListCount();

    if (FolderListCount > 0) {
        alert(R_Description1 + FolderListCount + R_Description2);
        return;
    }

    $("#tbHdnValue").val(CatID);
    $("#tbHdnTotValue").val(IsTotBoard);
    
    if (window.confirm(R_DeleteFolder)) {
        __doPostBack("LBTNProcess", "Del");
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 특정 폴더의 리스트 개수 가져오기
***************************************************/
Menu.FolderListCount = function () {
    ParamArray = {
        'categoryID': CatID
    };

    var url = "/DeskPlusEIP/Board/Ws/Admin/Menu/Menu.asmx/GetCategoryListCount";
    var obj = eval("Menu.SuccessFolderListCount");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 댓글 add 성공시 페이지 표시
**********************************************************************************************/
Menu.SuccessFolderListCount = function (data, status) {
    FolderListCount = data.d;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 리스트 페이지 호출
***************************************************/
function onNodeSelectList(node) {
    $("#imgAdd").hide();
    $("#imgDel").hide();

    if (node.ID > 0) {
        listPage = "/DeskplusEIP/Board/Admin/Content/List.aspx";
        //파라미터값 배열에 담기
        ParamArray = {
            'menuCatID': MenuCatID,
            'catID': node.ID
        };

        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        parent.frmContent.location.href = listPage + "?p=" + GetEncryptValue;
    }
}

/**************************************************************
onCallbackCompleteList
**************************************************************/
function onCallbackCompleteList(node) {
}