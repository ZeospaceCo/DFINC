Content = {};

$(function () {

    $(document).ready(function () {

        Gubun = 2;
       
        //본인게시물 클릭
        $("#myPage").click(function (e) {
            StartTab = 1;

            $("#chartUser").hide();
            $("#allBoardList").hide();
            $("#maxYearUser").hide();
            $("#maxYearShort").hide();
            $("#newMyBoardList").show();

            $("#allPage").removeClass('selected');
            $("#myPage").addClass('selected');            
            Content.ModUserTab();
           
        });

        //전체게시물 클릭
        $("#allPage").click(function (e) {
            StartTab = 0;

            $("#chartUser").show();
            $("#allBoardList").show();
            $("#maxYearUser").show();
            $("#maxYearShort").show();
            $("#newMyBoardList").hide();

            $("#myPage").removeClass('selected');
            $("#allPage").addClass('selected');            
            Content.ModUserTab();
          
        });

        //검색입력필드 엔터 이벤트 기술
        $("#cbtnSearch").click(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                Content.Search();
            }
            Content.Search();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#searchValue").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                Content.Search();
            }
        });

        //최신글
        $("#RecentWrite").click(function (e) {
            ParamArray = {
                'homeCategory': 'RecentWrite'
            };
            CallEncrypt(CallGetParam());
            var param = "p=" + GetEncryptValue;
            //location.href = "MoreList.aspx?homeCategory=RecentWrite";
            location.href = "MoreList.aspx?" + param;
        });

        //최다댓글
        $("#MaxShort").click(function (e) {
            ParamArray = {
                'homeCategory': 'MaxShort'
            };
            CallEncrypt(CallGetParam());
            var param = "p=" + GetEncryptValue;
            //location.href = "MoreList.aspx?homeCategory=RecentWrite";
            location.href = "MoreList.aspx?" + param;
        });

        //최다조회
        $("#MaxRead").click(function (e) {
            ParamArray = {
                'homeCategory': 'MaxRead'
            };
            CallEncrypt(CallGetParam());
            var param = "p=" + GetEncryptValue;
            //location.href = "MoreList.aspx?homeCategory=RecentWrite";
            location.href = "MoreList.aspx?" + param;
        });

        //평가우수
        $("#MaxRate").click(function (e) {
            ParamArray = {
                'homeCategory': 'MaxRate'
            };
            CallEncrypt(CallGetParam());
            var param = "p=" + GetEncryptValue;
            //location.href = "MoreList.aspx?homeCategory=RecentWrite";
            location.href = "MoreList.aspx?" + param;
        });

        //등록한 게시글
        $("#MyWrite").click(function (e) {
            ParamArray = {
                'homeCategory': 'MyWrite'
            };
            CallEncrypt(CallGetParam());
            var param = "p=" + GetEncryptValue;
            //location.href = "MoreList.aspx?homeCategory=RecentWrite";
            location.href = "MoreList.aspx?" + param;
        });

        //댓글 단 게시글
        $("#MyListByShort").click(function (e) {
            ParamArray = {
                'homeCategory': 'MyListByShort'
            };
            CallEncrypt(CallGetParam());
            var param = "p=" + GetEncryptValue;
            //location.href = "MoreList.aspx?homeCategory=RecentWrite";
            location.href = "MoreList.aspx?" + param;
        });

        //등록한 댓글
        $("#MyShort").click(function (e) {
            ParamArray = {
                'homeCategory': 'MyShort'
            };
            CallEncrypt(CallGetParam());
            var param = "p=" + GetEncryptValue;
            //location.href = "MoreList.aspx?homeCategory=RecentWrite";
            location.href = "MoreList.aspx?" + param;
        });

        //중요표시
        $("#MyImpo").click(function (e) {
            ParamArray = {
                'homeCategory': 'MyImpo'
            };
            CallEncrypt(CallGetParam());
            var param = "p=" + GetEncryptValue;
            //location.href = "MoreList.aspx?homeCategory=RecentWrite";
            location.href = "MoreList.aspx?" + param;
        });

        //본인게시물 탭
        /* 등록한 게시글 */
        $("#myWriteL").click(function (e) {
            MyWritePageNo = parseInt(MyWritePageNo) - 1;
            Content.MyWrite5();
        });
        $("#myWriteR").click(function (e) {
            MyWritePageNo = parseInt(MyWritePageNo) + 1;
            Content.MyWrite5();
        });

        /* 댓글 단 게시글*/
        $("#myListByShortL").click(function (e) {
            MyListByShortPageNo = parseInt(MyListByShortPageNo) - 1;
            Content.MyListByShort5();
        });
        $("#myListByShortR").click(function (e) {
            MyListByShortPageNo = parseInt(MyListByShortPageNo) + 1;
            Content.MyListByShort5();
        });


        /* 등록한 댓글*/
        $("#myShortL").click(function (e) {
            MyShortPageNo = parseInt(MyShortPageNo) - 1;
            Content.MyShort5();
        });
        $("#myShortR").click(function (e) {
            MyShortPageNo = parseInt(MyShortPageNo) + 1;
            Content.MyShort5();
        });


        $("#myImpoL").click(function (e) {
            MyImpoPageNo = parseInt(MyImpoPageNo) - 1;
            Content.MyImpo5();
        });
        $("#myImpoR").click(function (e) {
            MyImpoPageNo = parseInt(MyImpoPageNo) + 1;
            Content.MyImpo5();
        });

        Content.GetMyWriteList('MyWrite'); //등록한게시글
        Content.GetMyListByShortList('MyListByShort'); //댓글 단 게시글
        Content.GetMyShortList('MyShort'); //등록한 댓글
        Content.GetMyImpoList('MyImpo'); //중요표시

        //관심대상자추가
        $('#interested').click(function () {
            Content.AddnInterested();
        });

        //검색어입력
        $("#searchValue").attr("placeholder", R_PlaceholderMenu);
    });
});

/**************************************************
작성자   : 윤석원
작성일자 : 2012-01-27
작성목적 : 검색  (일반검색 : 제목/첨부파일명만 검색 /  검색엔진 : 상황에 따라)
***************************************************/
Content.Search = function () {
    
    var validator = new Validator($("#form1"));
    //validator.checkRequired("searchValue", "검색어를 입력해 주세요", true);
    validator.checkRequired("searchValue", R_LengthOver, true);
    if (!validator.validate()) return;

    var searchVal = $("#searchValue").val();

    ParamArray = {
    //'moduleName': 'BOARD',
    'searchValue': searchVal
    };

    CallEncrypt(CallGetParam());
    var param = "p=" + GetEncryptValue;
    /*
    //DB 검색
    //parent.frmMain.location.href = "/DeskPlusEIP/EIP/Search/Default.aspx?" + param;
    //검색엔진
    parent.location = "/DeskPlusEIP/Search/Default.aspx?" + param;
    */

    location.href = "/DeskPlusEIP/Board/Home/Search.aspx?" + param;
}

/**************************************************
작성자   : 윤석원
작성일자 : 2011-04-11
작성목적 : 새창으로 게시물 보기
***************************************************/
Content.PopupView = function (p_categoryID, p_listID) {
    ParamArray = {
        'catID': p_categoryID,
        'categoryID': p_categoryID,
        'listID': p_listID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Board/Content/ViewPopup.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "ShowImage", "1200", "800", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

/**********************************************************************************************
작성자   : 윤석원
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
Content.GetPageList = function (p_MenuTab) {
    MenuTab = p_MenuTab;

    Content.GetSelMenu(); //메뉴 tab selected

    var url = "/DeskPlusEIP/Board/Ws/Home/List.asmx/GetPageList";

    ParamArray = {
        'menuTab': MenuTab,
        'pageSize': 5
    };

    var obj = eval("Content.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 윤석원
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
Content.SuccessPageList = function (data, status) {
    var templates = Content.GetMenuTemplate();

    if (Gubun == "1") {
        $('#placeholderMain').setTemplateURL('/DeskPlusEIP/Board/Jt/Home/' + templates + '?seq=' + Guids(), null, { filter_data: false });
        $('#placeholderMain').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (윤석원)
    } else if (Gubun == "2") {
        $('#placeholderMy').setTemplateURL('/DeskPlusEIP/Board/Jt/Home/' + templates + '?seq=' + Guids(), null, { filter_data: false });
        $('#placeholderMy').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (윤석원)
    }
}



/**********************************************************************************************
작성자   : 윤석원
작성일자 : 2011-04-14
작성목적 : 등록한 게시글 호출
**********************************************************************************************/
Content.GetMyWriteList = function (p_MenuTab) {
    //alert(1);
    MenuTab = p_MenuTab;

    var url = "/DeskPlusEIP/Board/Ws/Home/List.asmx/GetPageList";

    ParamArray = {
        'menuTab': MenuTab,
        'pageSize': 5
    };

    var obj = eval("Content.SuccessMyWriteList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 윤석원
작성일자 : 2011-04-14
작성목적 : 등록한 게시글 호출 성공시 게시판 리스트 표시
**********************************************************************************************/
Content.SuccessMyWriteList = function (data, status) {
    var templates = Content.GetMenuTemplate();    
    if (Gubun == "1") {
        $('#placeholderMain').setTemplateURL('/DeskPlusEIP/Board/Jt/Home/' + templates + '?seq=' + Guids(), null, { filter_data: false });
        $('#placeholderMain').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (윤석원)
    } else if (Gubun == "2") {
        $('#phMyWriteList').setTemplateURL('/DeskPlusEIP/Board/Jt/Home/' + templates + '?seq=' + Guids(), null, { filter_data: false });
        $('#phMyWriteList').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (윤석원)
    }
}





/**********************************************************************************************
작성자   : 윤석원
작성일자 : 2011-04-14
작성목적 : 댓글 단 게시글 호출
**********************************************************************************************/
Content.GetMyListByShortList = function (p_MenuTab) {
    //alert(1);
    MenuTab = p_MenuTab;

    var url = "/DeskPlusEIP/Board/Ws/Home/List.asmx/GetPageList";

    ParamArray = {
        'menuTab': MenuTab,
        'pageSize': 5
    };

    var obj = eval("Content.SuccessMyListByShortList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 윤석원
작성일자 : 2011-04-14
작성목적 : 댓글 단 게시글 호출 성공시 게시판 리스트 표시
**********************************************************************************************/
Content.SuccessMyListByShortList = function (data, status) {
    var templates = Content.GetMenuTemplate();

    if (Gubun == "1") {
        $('#placeholderMain').setTemplateURL('/DeskPlusEIP/Board/Jt/Home/' + templates + '?seq=' + Guids(), null, { filter_data: false });
        $('#placeholderMain').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (윤석원)
    } else if (Gubun == "2") {
        $('#phMyListByShortList').setTemplateURL('/DeskPlusEIP/Board/Jt/Home/' + templates + '?seq=' + Guids(), null, { filter_data: false });
        $('#phMyListByShortList').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (윤석원)
    }
}




/**********************************************************************************************
작성자   : 윤석원
작성일자 : 2011-04-14
작성목적 : 등록한댓글 호출
**********************************************************************************************/
Content.GetMyShortList = function (p_MenuTab) {
    //alert(1);
    MenuTab = p_MenuTab;

    var url = "/DeskPlusEIP/Board/Ws/Home/List.asmx/GetPageList";

    ParamArray = {
        'menuTab': MenuTab,
        'pageSize': 5
    };

    var obj = eval("Content.SuccessMyShortList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 윤석원
작성일자 : 2011-04-14
작성목적 : 등록한 댓글 호출 성공시 게시판 리스트 표시
**********************************************************************************************/
Content.SuccessMyShortList = function (data, status) {
    var templates = Content.GetMenuTemplate();

    if (Gubun == "1") {
        $('#placeholderMain').setTemplateURL('/DeskPlusEIP/Board/Jt/Home/' + templates + '?seq=' + Guids(), null, { filter_data: false });
        $('#placeholderMain').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (윤석원)
    } else if (Gubun == "2") {
        $('#phMyShortList').setTemplateURL('/DeskPlusEIP/Board/Jt/Home/' + templates + '?seq=' + Guids(), null, { filter_data: false });
        $('#phMyShortList').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (윤석원)
    }
}



/**********************************************************************************************
작성자   : 윤석원
작성일자 : 2011-04-14
작성목적 : 중요표시 호출
**********************************************************************************************/
Content.GetMyImpoList = function (p_MenuTab) {
    //alert(1);
    MenuTab = p_MenuTab;

    var url = "/DeskPlusEIP/Board/Ws/Home/List.asmx/GetPageList";

    ParamArray = {
        'menuTab': MenuTab,
        'pageSize': 5
    };

    var obj = eval("Content.SuccessMyImpoList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 윤석원
작성일자 : 2011-04-14
작성목적 : 중요표시 호출 성공시 게시판 리스트 표시
**********************************************************************************************/
Content.SuccessMyImpoList = function (data, status) {
    var templates = Content.GetMenuTemplate();

    if (Gubun == "1") {
        $('#placeholderMain').setTemplateURL('/DeskPlusEIP/Board/Jt/Home/' + templates + '?seq=' + Guids(), null, { filter_data: false });
        $('#placeholderMain').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (윤석원)
    } else if (Gubun == "2") {
        $('#phMyImpoList').setTemplateURL('/DeskPlusEIP/Board/Jt/Home/' + templates + '?seq=' + Guids(), null, { filter_data: false });
        $('#phMyImpoList').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (윤석원)
    }
}



/**********************************************************************************************
작성자   : 윤석원
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
Content.GetMenuTemplate = function () {
    var templateName = "";
    switch (MenuTab) {
        case "RecentWrite":
            templateName = "JTRecentWrite.htm";
            break;
        case "MaxShort":
            templateName = "JTMaxShort.htm";
            break;
        case "MaxRead":
            templateName = "JTMaxReadCount.htm";
            break;
        case "MaxRate":
            templateName = "JTRate.htm";
            break;

        case "MyWrite":
            templateName = "JTMyWrite.htm";
            break;
        case "MyListByShort":
            templateName = "JTMyListByShort.htm";
            break;
        case "MyShort":
            templateName = "JTMyShort.htm";
            break;
        case "MyImpo":
            templateName = "JTMyImpo.htm";
            break;
    }

    return templateName;
}

/**********************************************************************************************
작성자   : 윤석원
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
Content.GetSelMenu = function () {
    switch (MenuTab) {
        case "RecentWrite":
            Gubun = "1";
            $("#MainMenu li").removeClass("selected");
            $("#MainMenu li").eq(0).addClass("selected");
            break;
        case "MaxShort":
            Gubun = "1";
            $("#MainMenu li").removeClass("selected");
            $("#MainMenu li").eq(1).addClass("selected");
            break;
        case "MaxRead":
            Gubun = "1";
            $("#MainMenu li").removeClass("selected");
            $("#MainMenu li").eq(2).addClass("selected");
            break;
        case "MaxRate":
            Gubun = "1";
            $("#MainMenu li").removeClass("selected");
            $("#MainMenu li").eq(3).addClass("selected");
            break;

        case "MyWrite":
            Gubun = "2";
            $("#MyMenu li").removeClass("selected");
            $("#MyMenu li").eq(0).addClass("selected");
            break;
        case "MyListByShort":
            Gubun = "2";
            $("#MyMenu li").removeClass("selected");
            $("#MyMenu li").eq(1).addClass("selected");
            break;
        case "MyShort":
            Gubun = "2";
            $("#MyMenu li").removeClass("selected");
            $("#MyMenu li").eq(2).addClass("selected");
            break;
        case "MyImpo":
            Gubun = "2";
            $("#MyMenu li").removeClass("selected");
            $("#MyMenu li").eq(3).addClass("selected");
            break;

    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 본인 게시글 탭 본인 등록한 게시글 
**********************************************************************************************/
Content.MyWrite5 = function () {

    if (MyWritePageNo < 1) {
        alert(R_FirstList);
        MyWritePageNo = 1;
        return;
    }
    var url = "/DeskPlusEIP/Board/Ws/Home/MoreList.asmx/GetPageList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'homeCategory': 'MyWrite',
        'totalCount': '0',
        'pageSize': 5,
        'pageSizePageNo': MyWritePageNo,
        'searchTerm': '',
        'searchTermValue': '',
        'searchGroup': '',
        'searchField': '',
        'searchValue': '',
        'order': '',
        'optionUseDept': '',
        'optionUseView': '',
        'auth': "0"
    };

    var obj = eval("Content.MyWriteList5");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 본인 게시글 탭 본인 등록한 게시글 성공시 게시판 리스트 표시
**********************************************************************************************/
Content.MyWriteList5 = function (data, status) {

    if ((data.d.length != 2) || (MyWritePageNo == 1)) {
        $('#phMyWriteList').setTemplateURL('/DeskPlusEIP/Board/Jt/Home/JTMyWrite.htm?seq=' + Guids(), null, { filter_data: false });
        $('#phMyWriteList').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (shimpark)
    }
    else {
        if (data.d.length == 2) {
            alert(R_LastList);
            MyWritePageNo = MyWritePageNo - 1;
            return;
        }

    }

}




/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 본인 게시글 탭 댓글 단 게시글
**********************************************************************************************/
Content.MyListByShort5 = function () {

    if (MyListByShortPageNo < 1) {
        alert(R_FirstList);
        MyListByShortPageNo = 1;
        return;
    }
    var url = "/DeskPlusEIP/Board/Ws/Home/MoreList.asmx/GetPageList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'homeCategory': 'MyListByShort',
        'totalCount': '0',
        'pageSize': 5,
        'pageSizePageNo': MyListByShortPageNo,
        'searchTerm': '',
        'searchTermValue': '',
        'searchGroup': '',
        'searchField': '',
        'searchValue': '',
        'order': '',
        'optionUseDept': '',
        'optionUseView': '',
        'auth': "0"
    };

    var obj = eval("Content.MyListByShortList5");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 본인 게시글 탭 댓글 단 게시글 성공시 게시판 리스트 표시
**********************************************************************************************/
Content.MyListByShortList5 = function (data, status) {

    if ((data.d.length != 2) || (MyListByShortPageNo == 1)) {
        $('#phMyListByShortList').setTemplateURL('/DeskPlusEIP/Board/Jt/Home/JTMyListByShort.htm?seq=' + Guids(), null, { filter_data: false });
        $('#phMyListByShortList').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (shimpark)
    }
    else {
        if (data.d.length == 2) {
            alert(R_LastList);
            MyListByShortPageNo = MyListByShortPageNo - 1;
            return;
        }

    }

}





/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 본인 게시물 탭 등록한 댓글 게시글
**********************************************************************************************/
Content.MyShort5 = function () {
    
    if (MyShortPageNo < 1) {
        alert(R_FirstList);
        MyShortPageNo = 1;
        return;
    }
    var url = "/DeskPlusEIP/Board/Ws/Home/MoreList.asmx/GetPageList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'homeCategory': 'MyShort',
        'totalCount': '0',
        'pageSize': 5,
        'pageSizePageNo': MyShortPageNo,
        'searchTerm': '',
        'searchTermValue': '',
        'searchGroup': '',
        'searchField': '',
        'searchValue': '',
        'order': '',
        'optionUseDept': '',
        'optionUseView': '',
        'auth': "0"
    };

    var obj = eval("Content.MyShortList5");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 본인 게시글 탭 댓글 단 게시글 성공시 게시판 리스트 표시
**********************************************************************************************/
Content.MyShortList5 = function (data, status) {
    
    if ((data.d.length != 2) || (MyShortPageNo == 1)) {
        $('#phMyShortList').setTemplateURL('/DeskPlusEIP/Board/Jt/Home/JTMyShort.htm?seq=' + Guids(), null, { filter_data: false });
        $('#phMyShortList').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (shimpark)
    }
    else {
        if (data.d.length == 2) {
            alert(R_LastList);
            MyShortPageNo = MyShortPageNo - 1;
            return;
        }

    }

}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 본인 게시물 탭 중요표시
**********************************************************************************************/
Content.MyImpo5 = function () {

    if (MyImpoPageNo < 1) {
        alert(R_FirstList);
        MyImpoPageNo = 1;
        return;
    }
    var url = "/DeskPlusEIP/Board/Ws/Home/MoreList.asmx/GetPageList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'homeCategory': 'MyImpo',
        'totalCount': '0',
        'pageSize': 5,
        'pageSizePageNo': MyImpoPageNo,
        'searchTerm': '',
        'searchTermValue': '',
        'searchGroup': '',
        'searchField': '',
        'searchValue': '',
        'order': '',
        'optionUseDept': '',
        'optionUseView': '',
        'auth': "0"
    };

    var obj = eval("Content.MyImpoList5");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 본인 게시글 탭 중요표시 리스트 표시
**********************************************************************************************/
Content.MyImpoList5 = function (data, status) {

    if ((data.d.length != 2) || (MyImpoPageNo == 1)) {
        $('#phMyImpoList').setTemplateURL('/DeskPlusEIP/Board/Jt/Home/JTMyImpo.htm?seq=' + Guids(), null, { filter_data: false });
        $('#phMyImpoList').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (shimpark)
    }
    else {
        if (data.d.length == 2) {
            alert(R_LastList);
            MyImpoPageNo = MyImpoPageNo - 1;
            return;
        }

    }

}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 탭 선택에 따라 웹 서비스 호출
**********************************************************************************************/
Content.ModUserTab = function () {

    var url = "/DeskPlusEIP/Board/Ws/Home/List.asmx/ModUserTab";
    ParamArray = {
        'startTabID': StartTab
    };

    var obj = eval("Content.SuccessModUserTab");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

Content.SuccessModUserTab = function (data, status) {

};

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-23
작성목적 : 관심대상자 추가
***************************************************/
Content.AddnInterested = function () {
    ParamArray = {
        'gubun': "2",
        'kind': "HOME"
    };

    CallEncrypt(CallGetParam());

    var url = "";

    url = "/DeskPlusEIP/Board/Common/GetInterestedUser.aspx?p=" + GetEncryptValue;

    PopupWin(800, 470, url, "-500", "", true);
}