Search = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-15
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            Search.GoPageLoad();
        }
    }

    $(document).ready(function () {
        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            ListCount: ListCount,
            currentPage: CurrentPage,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                Search.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            Search.Search();
        });


        //페이지 카운트
        $('#ddlListCount').change(function () {
            ListCount = $('#ddlListCount').val();
            Search.ModListUser();
        });

        __addEventListener(document, "click", Search.click);
        Search.Init();

    });
});

/**************************************************
작성자   : famo23
작성일자 : 2016-03-15
작성목적 : 초기
***************************************************/
Search.Init = function () {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, ListCount);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
    Search.ReLoadTree();
    Search.GetPageList(); //검색 리스트 가져오기
}

/**************************************************
작성자   : famo23
작성일자 : 2016-01-04
작성목적 : 좌측 메뉴 reload
***************************************************/
Search.ReLoadTree = function () {

    if (ReLoad == "1") {
        ParamArray = {
            'cabinetID': "root",
            'reLoad': ReLoad
        };
        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Task/Menu/Tree.aspx?p=" + GetEncryptValue;
        parent.frmLeft.location = url;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2016-03-15
작성목적 : 검색
***************************************************/
Search.Search = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("searchValue", R_InputSearch, true);
    if (!validator.validate()) return;

    SearchValue = $("#searchValue").val().securityText();

    CurrentPage = 1; //1페이지로 이동처리
    Search.GoPageLoad();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-15
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Search.GetPageList = function () {
    var url = "/DeskPlusEIP/Task/Ws/Home/Search.asmx/GetPageList";

    if (AdminYN == "Y") {
        url = "/DeskPlusEIP/Task/Ws/Home/Search.asmx/GetAllPageList";
    }

    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': ListCount,
        'pageSizePageNo': CurrentPage,
        'searchValue': SearchValue
    };

    var obj = eval("Search.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-15
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Search.SuccessPageList = function (data, status) {
    $('#phList').setTemplateURL('/DeskPlusEIP/Task/Jt/Home/JTSearch.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);
    $('#loading').html("");

    Search.Resource();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-15
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Search.Resource = function () {

    $("#r_title")[0].innerHTML = R_Title;
    $("#r_attach")[0].innerHTML = R_AttachFile;    
    $("#r_status")[0].innerHTML = R_State;
    $("#r_requestdept")[0].innerHTML = R_RequestDept;
    $("#r_requester")[0].innerHTML = R_Requester;
    $("#r_requsetdate")[0].innerHTML = R_RequestDate;
    $("#r_enddate")[0].innerHTML = R_EndDate;
    $("#r_todept")[0].innerHTML = R_ToDeptName;
    $("#r_toname")[0].innerHTML = R_Receiver;

    if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoListData;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-15
작성목적 : 보기 이동
**********************************************************************************************/
Search.View = function (p_listID, p_groupID) {

    ParamArray = {
        'listID': p_listID,
        'groupID': p_groupID,
        'isPage': "Search",
        'currentPage': CurrentPage,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());

    location = "ViewN.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2016-03-15
작성목적 : 클릭 이벤트 분기
***************************************************/
Search.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");

    switch (action) {
        case "attach": Search.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

/**************************************************
작성자   : famo23
작성일자 : 2016-03-15
작성목적 : 첨부파일 정보가져오기
***************************************************/
Search.GetAttachFileListMenu = function (p_listID) {
    var url = "/DeskPlusEIP/Task/Ws/Home/Search.asmx/GetAttachFileListMenu";
    var data = "groupID=" + p_listID;
    var obj = eval("Search.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-15
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
Search.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}

/**************************************************
작성자   : famo23
작성일자 : 2016-03-15
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Search.ShowAttachContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var listID = el.getAttribute("ListID");

    Search.GetAttachFileListMenu(listID);

    if (AttachFileInfoList == "0") {
        alert(R_NoFileData);
        return;
    }
    else if (AttachFileInfoList == "1") {
        alert(R_ErrorAttachFile);
        return;
    }
    else {
        oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
        oContextMenuCtrl.init();

        var attachArr = AttachFileInfoList.split("|");
        var attachFile = "";
        var attachFileName = "";
        var attachFilePath = "";

        /*
        if ($.browser.msie) {
            if (attachArr.length > 1) {
                oItem = oContextMenuCtrl.addItem(R_AllDownLoad, Search.TotalDownLoad, ImagePath + "/Icon/folders.gif");
                oItem.txt.setAttribute("arg", AttachFileInfoList, 0);
                oContextMenuCtrl.addBlank();
            }
        }
        */

        for (i = 0; i < attachArr.length; i++) {
            attachFile = attachArr[i].split(":");
            attachFileName = attachFile[0];
            attachFilePath = attachFile[1];

            attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + attachFilePath;

            oItem = oContextMenuCtrl.addItem(attachFileName, Search.DownLoad, "EXT");
            oItem.txt.setAttribute("arg", attachFilePath, 0);
        }
        oContextMenuCtrl.show(e);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2016-03-15
작성목적 : 첨부파일 열기
***************************************************/
Search.DownLoad = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        window.open(arg);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2016-03-15
작성목적 : 첨부파일 전체 다운로드
***************************************************/
Search.TotalDownLoad = function (_e) {
    var evt = new __event(_e);

    if (evt.target) {
        var arg = evt.target.getAttribute("arg");

        $("#fileList").val(arg);

        document.getElementById("frmTotalDownLoad").action = "/DeskPlusEIP/Common/DownLoad/TotalDownLoad.aspx";
        PopupWinOpen("", "", "openWin", "600", "400")
        document.getElementById("frmTotalDownLoad").submit();
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-15
작성목적 : 메뉴별 사용자 페이징 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
Search.ModListUser = function () {

    var url = "/DeskPlusEIP/Task/Ws/Home/Search.asmx/ModListUser";
    ParamArray = {
        'isType': "SE",
        'pageSize': $("#ddlListCount").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Search.SuccessModListUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-15
작성목적 : 메뉴별 사용자 페이징 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
Search.SuccessModListUser = function (data, status) {
    if (data.d == true) {

        CurrentPage = 1; //1페이지로 이동처리
        Search.GoPageLoad();
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-15
작성목적 : 페이지이동 처리
**********************************************************************************************/
Search.GoPageLoad = function () {

    ParamArray = {
        'currentPage': CurrentPage,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    location = "Search.aspx?p=" + GetEncryptValue;
}


