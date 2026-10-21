
AddPortletUser = {}

$(function () {
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            AddPortletUser.LoadData("");
        }
    }

    $(document).ready(function () {

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            recordPerPage: RecordPerPage,
            currentPage: CurrentPage,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });

        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            //event: "mouseover",
            active: AccordionActive,
            overTop: 50
        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () {
                $("#accordion").accordion("resize");
            }
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            AddPortletUser.Close();
            //if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            AddPortletUser.ModPageSize();
        });

        //그룹 추가 페이지 호출
        $('#folderAdd').click(function (e) {
            e.preventDefault();
            $(".actionDiv").hide();
            $("#divfolderAdd").show();

            $("#ltlHeader_folderAdd")[0].innerHTML = "";
            $("#ctbAddName").val("");

            Gubun = "add";
            AddPortletUser.GetCategoryName();
        });

        //그룹 수정 페이지 호출
        $('#folderModify').click(function (e) {
            e.preventDefault();
            if (CatID == "0") { alert(R_CheckModGroup); return; }

            $(".actionDiv").hide();
            $("#divfolderMod").show();

            $("#ltlHeader_folderMod")[0].innerHTML = "";
            $("#ctbModName").val("");

            Gubun = "mod";
            AddPortletUser.GetCategoryName();
        });

        //RSS 추가 페이지 호출
        $('#cbtnAddPortlet').click(function (e) {
            if (CatID == "0") { alert(R_CheckAddRSSGroup); return; }
            e.preventDefault();
            $(".actionDiv").hide();
            $("#divAddRSS").show();

            $("#ltlHeader_AddRSS")[0].innerHTML = "";
            $("#ctbAddRSSName").val("");
            $("#ctbAddRSSUrl").val("");
            Gubun = "rss";
            AddPortletUser.GetCategoryName();
        });

        //그룹 새로 추가
        $('#cbtnAddName').click(function (e) {
            e.preventDefault();
            AddPortletUser.AddCategory();
        });

        //그룹 수정하기
        $('#cbtnModName').click(function (e) {
            e.preventDefault();
            AddPortletUser.ModCategory();
        });

        //그룹 삭제
        $('#folderDelete').click(function (e) {
            e.preventDefault();
            if (CatID == "0") { alert(R_CheckDeleteGroup); return; }
            AddPortletUser.DelCategory();
        });

        //RSS 추가
        $('#cbtnAddRSSName').click(function (e) {
            e.preventDefault();
            if (CatID == "0") { alert(R_CheckAddRSSGroup); return; }
            AddPortletUser.AddPortlet();
        });

        $(".actionDiv").hide();
        $("#divList").show();

        $('#folderDelete').hide();
        $('#folderModify').hide();

        AddPortletUser.LoadData("");
    });
});



/**************************************************
    onNodeSelect 트리 노드 선택
***************************************************/
function PrivatePortletTree_onNodeSelect(node) {
    $(".actionDiv").hide();
    $("#divList").show();
    CatID = node.ID;
    CategoryName = node.Text;

    CurrentPage = 1;     //1페이지로 이동처리  
    AddPortletUser.LoadData("search");

    if (CatID != "0") {
        $('#folderDelete').show();
        $('#folderModify').show();
    }
    else {
        $('#folderDelete').hide();
        $('#folderModify').hide();
    }
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
AddPortletUser.LoadData = function (p_gubun) {
    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        AddPortletUser.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        AddPortletUser.GetCount(); //검색 총 카운트 가져오기
        AddPortletUser.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                AddPortletUser.LoadData("search");
            }
        }
        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            recordPerPage: RecordPerPage,
            currentPage: CurrentPage,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });
    }

    setTimeout(function () {
        if (p_gubun == "search") {
            loadBoardSearchData();
        }
        else {
            loadBoardData();
        }
    }, 100);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 포틀릿 카운트  호출
**********************************************************************************************/
AddPortletUser.GetCount = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Portal/AddPortletUser.asmx/GetListCount";
    
    ParamArray = {
        'categoryID': CatID,
        'searchKey': '',
        'searchValue': '',
        'tabID': 0
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AddPortletUser.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
AddPortletUser.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 포틀릿 리스트 호출
**********************************************************************************************/
AddPortletUser.GetPageList = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Portal/AddPortletUser.asmx/GetList";
    
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'categoryID': CatID,
        'searchKey': '',
        'searchValue': '',
        'order': '',
        'tabID': 0
    };
    
    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AddPortletUser.SuccessPageList");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-02-14
작성목적 : 성공시 리스트 표시
**********************************************************************************************/
AddPortletUser.SuccessPageList = function (data, status) {
    $('#phList').setTemplateURL('/DeskPlusEIP/EIP/Jt/Portal/JTAddPortletUser.htm?seq=' + Guids(), null, { filter_data: false });

    $('#phList').processTemplate(data.d);

    AddPortletUser.Resource();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-02-14
작성목적 : 다국어
**********************************************************************************************/
AddPortletUser.Resource = function () {
    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }

    $('.DirectAdd').text(R_DirectAdd);

    $(".Del").text(R_DeletePortlet);
}

/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}



/**************************************************
작성자:
작성일:2011-11-14
작성목적 : 닫기
***************************************************/
AddPortletUser.Close = function () {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}


/**************************************************
작성자   : shimpark
작성일자 :2011-06-15
작성목적 : 페이지 사이즈 설정
***************************************************/
AddPortletUser.ModPageSize = function () {
    RecordPerPage = $('#ddlListCount').val();
    CurrentPage = 1; //1페이지로 이동처리  
    AddPortletUser.LoadData("search");
}




/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 그룹 추가하기
**********************************************************************************************/
AddPortletUser.AddCategory = function () {    

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbAddName", R_InputGroupName, true);
    if (!validator.validate()) return;

    var url = "/DeskPlusEIP/EIP/Ws/Portal/AddPortletUser.asmx/AddPortletCategory";

    ParamArray = {
        'categoryID': CatID,
        'name': $("#ctbAddName").val().replace(/&/g, '＆')
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AddPortletUser.SuccessAddCategory");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 그룹 추가하기
**********************************************************************************************/
AddPortletUser.SuccessAddCategory = function (data, status) {
    if (data.d == true) {
        location = "/DeskPlusEIP/EIP/Portal/AddPortletUser.aspx";
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 그룹 수정하기
**********************************************************************************************/
AddPortletUser.ModCategory = function () {

    if (CatID == "0") { alert(R_CheckModGroup); return; }

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbModName", R_InputGroupName, true);
    if (!validator.validate()) return;

    var url = "/DeskPlusEIP/EIP/Ws/Portal/AddPortletUser.asmx/ModPortletCategory";

    ParamArray = {
        'categoryID': CatID,
        'name': $("#ctbModName").val().replace(/&/g, '＆')
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AddPortletUser.SuccessModCategory");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 그룹 수정하기
**********************************************************************************************/
AddPortletUser.SuccessModCategory = function (data, status) {
    if (data.d == true) {
        location = "/DeskPlusEIP/EIP/Portal/AddPortletUser.aspx";
    }
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 그룹 수정하기
**********************************************************************************************/
AddPortletUser.DelCategory = function () {

    if (CatID == "0") { alert(R_CheckDeleteGroup); return; }

    if (window.confirm(R_ConfirmDeleteTabAddPortlet)) {

        var url = "/DeskPlusEIP/EIP/Ws/Portal/AddPortletUser.asmx/DelPortletCategory";

        ParamArray = {
            'categoryID': CatID
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("AddPortletUser.SuccessDelCategory");

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 그룹 삭제하기
**********************************************************************************************/
AddPortletUser.SuccessDelCategory = function (data, status) {
    if (data.d == true) {
        location = "/DeskPlusEIP/EIP/Portal/AddPortletUser.aspx";
    }
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 그룹 추가 및 수정페이지의 데이터 가져오기
**********************************************************************************************/
AddPortletUser.GetCategoryName = function () {

    var url = "/DeskPlusEIP/EIP/Ws/Portal/AddPortletUser.asmx/GetCategoryName";
   
    ParamArray = {
        'categoryID': CatID,
        'gubun': Gubun
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AddPortletUser.SuccessGetCategoryName");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 그룹 추가 및 수정페이지의 데이터 가져오기
**********************************************************************************************/
AddPortletUser.SuccessGetCategoryName = function (data, status) {
    var response = data.d;
    
    if (Gubun == "add") {
        $("#ltlHeader_folderAdd")[0].innerHTML = response.split("#")[0];
        $("#ctbAddName").val(response.split("#")[1]);
    } else if (Gubun == "mod") {
        $("#ltlHeader_folderMod")[0].innerHTML = response.split("#")[0];
        $("#ctbModName").val(response.split("#")[1]);
    } else if (Gubun == "rss") {
        $("#ltlHeader_AddRSS")[0].innerHTML = response.split("#")[1];
    }
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 개인 포틀릿 추가하기
**********************************************************************************************/
AddPortletUser.AddPortlet = function () {

    if (CatID == "0") { alert(R_CheckGroup); return; }

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbAddRSSName", R_InputPortletName, true);
    validator.checkRequired("ctbAddRSSUrl", R_InputRSSURL, true);
    if (!validator.validate()) return;

    var url = "/DeskPlusEIP/EIP/Ws/Portal/AddPortletUser.asmx/AddPortlet";

    ParamArray = {
        'categoryID': CatID,
        'name': $("#ctbAddRSSName").val().replace(/&/g, '＆'),
        'dataUrl': $("#ctbAddRSSUrl").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AddPortletUser.SuccessAddPortlet");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 개인 포틀릿 추가하기
**********************************************************************************************/
AddPortletUser.SuccessAddPortlet = function (data, status) {
    if (data.d == true) {
        $(".actionDiv").hide();
        $("#divList").show();

        CurrentPage = 1;     //1페이지로 이동처리  
        AddPortletUser.LoadData("search");
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 포틀릿 삭제하기
**********************************************************************************************/
AddPortletUser.DelPortlet = function (p_categoryID, p_portletID) {

    if (p_categoryID == "0") { alert(R_CheckGroupAfterDeletePortlet); return; }

    if (window.confirm(R_ConfirmDeleteTabAddPortlet)) {

        var url = "/DeskPlusEIP/EIP/Ws/Portal/AddPortletUser.asmx/DelPortlet";

        ParamArray = {
            'categoryID': p_categoryID,
            'portletID': p_portletID
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("AddPortletUser.SuccessDelPortlet");

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 포틀릿 삭제하기
**********************************************************************************************/
AddPortletUser.SuccessDelPortlet = function (data, status) {
    if (data.d == true) {
        $(".actionDiv").hide();
        $("#divList").show();

        CurrentPage = 1;     //1페이지로 이동처리  
        AddPortletUser.LoadData("search");
    }
}
