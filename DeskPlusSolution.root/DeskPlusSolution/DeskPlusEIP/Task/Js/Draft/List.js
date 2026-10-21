List = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            //List.LoadData("");
            List.GoPageLoad();
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

        // 삭제하기
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            List.Del();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                List.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            List.Search();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            ListCount = $('#ddlListCount').val();
            List.ModListUser();
            List.Search();
        });

        //대분류 선택
        $('#ddlItem').change(function () {
            List.GetItemSubList();
        });

        List.Init();

        $(".clsDate").keydown(function (e) { return false; });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 초기화 하기
***************************************************/
List.Init = function () {

    List.LoadData("");
    __addEventListener(document, "click", List.click);

    $("#zeoModalContact").draggable({ disabled: false });
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 검색
***************************************************/
List.Search = function () {
    var response = List.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리
    //List.LoadData("search");
    List.GoPageLoad();
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 검색 필드 설정하기
***************************************************/
List.SettingSearch = function () {
    IsGubun = $("#ddlIsGubun").val();
    SearchField = $("#ddlSearchField").val();
    ItemID = $("#ddlItem").val(); // 대분류
    ItemSubID = $("#ddlItemSub").val(); // 소분류

    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    return true;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, ListCount);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadTaskData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadTaskSearchData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                List.LoadData("search");
            }
        }

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
    }

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        if (gubun == "search") {
            loadTaskSearchData();
        }
        else {
            loadTaskData();
        }
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/Task/Ws/Draft/List.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': ListCount,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,        
        'itemID': ItemID,
        'itemSubID': ItemSubID,
        'isGubun': IsGubun
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#phList').setTemplateURL('/DeskPlusEIP/Task/Jt/Draft/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {
    $("#r_title")[0].innerHTML = R_Title;
    $("#r_attach")[0].innerHTML = R_AttachName;
    $("#r_draftDate")[0].innerHTML = R_DraftDate;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoListData); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }    
    if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/Task/Ws/Draft/List.asmx/GetListCount";

    ParamArray = {
        'searchField': SearchField,
        'searchValue': SearchValue,
        'itemID': ItemID,
        'itemSubID': ItemSubID,
        'isGubun': IsGubun
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, ListCount);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 요청하기
**********************************************************************************************/
List.Add = function (p_draftID) {
    ParamArray = {        
        'currentPage': CurrentPage,
        'searchStatus': SearchStatus,
        'searchField': SearchField,
        'searchValue': SearchValue,                
        'draftID': p_draftID,        
        'itemID': ItemID,
        'itemSubID': ItemSubID,
        'isGubun': IsGubun        
    };

    CallEncrypt(CallGetParam());

    location = "AddN.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 소분류 가져오기 (웹서비스 호출)
***************************************************/
List.GetItemSubList = function () {
    var url = "/DeskPlusEIP/Task/Ws/Draft/List.asmx/GetItemSub";

    var selVal = $('#ddlItem').val();

    ParamArray = {
        'itemID': selVal
    };

    var obj = eval("List.SuccessGetItemSubList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 소분류 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessGetItemSubList = function (data, status) {

    $("#ddlItemSub option").remove();
    $("#ddlItemSub").append("<option value='0'>" + R_SubCategory + "</option>");

    for (var obj in data.d) {
        var nameBase = data.d[obj].NameBase;
        var vals = data.d[obj].ItemSubID;

        $("#ddlItemSub").append("<option value='" + vals + "'>" + nameBase + "</option>");
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 클릭 이벤트 분기
***************************************************/
List.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": List.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 첨부파일 정보가져오기
***************************************************/
List.GetAttachFileListMenu = function (p_draftID) {
    var url = "/DeskPlusEIP/Task/Ws/Draft/List.asmx/GetAttachFileListMenu";
    var data = "draftID=" + p_draftID;
    var obj = eval("List.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
List.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
List.ShowAttachContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var listID = el.getAttribute("ListID");

    List.GetAttachFileListMenu(listID);

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
                oItem = oContextMenuCtrl.addItem(R_AllDownLoad, List.TotalDownLoad, ImagePath + "/Icon/folders.gif");
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

            oItem = oContextMenuCtrl.addItem(attachFileName, List.DownLoad, "EXT");
            oItem.txt.setAttribute("arg", attachFilePath, 0);
        }

        //oContextMenuCtrl.show(e);
        var TmpX = new __event(e);
        oContextMenuCtrl.show(e, '', TmpX.pageX - 150, 0);
        
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 첨부파일 열기
***************************************************/
List.DownLoad = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        window.open(arg);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 첨부파일 전체 다운로드
***************************************************/
List.TotalDownLoad = function (_e) {
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
작성일자 : 2015-04-24
작성목적 : 메뉴별 사용자 페이징 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
List.ModListUser = function () {

    var url = "/DeskPlusEIP/Task/Ws/Draft/List.asmx/ModListUser";
    ParamArray = {
        'isType': "DR",
        'pageSize': $("#ddlListCount").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessModListUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 메뉴별 사용자 페이징 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessModListUser = function (data, status) {
    if (data.d == true) {
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 전체 체크박스
***************************************************/
List.Checked = function () {
    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 선택된 값 가져오기
***************************************************/
List.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 문서 삭제
***************************************************/
List.Del = function () {
    //키|구분자,키|구분자
    var selValue = List.CheckedValue();

    if (selValue != "") {
        if (window.confirm(R_ApplyDelete)) {
            //파라미터값 배열에 담기
            ParamArray = {
                'draftID': selValue
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Task/Ws/Draft/List.asmx/DelList";
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("List.SuccessDel");

            AjaxService(url, data, obj);
        }
    } else {
        alert(R_CheckDeleteList);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 성공시 , 삭제
**********************************************************************************************/
List.SuccessDel = function (data, status) {
    if (data.d == true) {
        alert(R_CompleteConfirm);
        List.Search();
    }
    else {
        alert(R_ErrorProcessing);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-18
작성목적 : 페이지이동 처리
**********************************************************************************************/
List.GoPageLoad = function () {

    ParamArray = {
        'currentPage': CurrentPage,
        'searchStatus': SearchStatus,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchDateValue': SearchDateValue,
        'itemID': ItemID,
        'itemSubID': ItemSubID,
        'isGubun': IsGubun
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}