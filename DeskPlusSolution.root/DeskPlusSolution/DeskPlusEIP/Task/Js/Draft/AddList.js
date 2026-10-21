AddList = {}

/**************************************************
초기화
***************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            AddList.LoadData("");
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

        //확인
        $('#cbtnSubmit').click(function (e) {
            e.preventDefault();
            AddList.Confirm();
        });

        //닫기
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            AddList.Cancel();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                AddList.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            AddList.Search();
        });

        //대분류 선택
        $('#ddlItem').change(function () {
            AddList.GetItemSubList();
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        AddList.LoadData("");
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2015-05-14
작성목적 : 소분류 가져오기 (웹서비스 호출)
***************************************************/
AddList.GetItemSubList = function () {
    var url = "/DeskPlusEIP/Task/Ws/Draft/AddList.asmx/GetItemSub";

    var selVal = $('#ddlItem').val();

    ParamArray = {
        'itemID': selVal
    };

    var obj = eval("AddList.SuccessGetItemSubList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-05-14
작성목적 : 소분류 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
AddList.SuccessGetItemSubList = function (data, status) {

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
작성일자 : 2015-04-27
작성목적 : 검색
***************************************************/
AddList.Search = function () {
    var response = AddList.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리
    AddList.LoadData("search");
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 검색 필드 설정하기
***************************************************/
AddList.SettingSearch = function () {
    SearchField = $("#ddlSearchField").val();
    ItemID = $("#ddlItem").val(); // 대분류
    ItemSubID = $("#ddlItemSub").val(); // 소분류

    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    return true;
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-27
작성목적 : 리스트 초기화
***************************************************/
AddList.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, ListCount);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadData = function () {
        AddList.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadSearchData = function () {
        AddList.GetCount(); //검색 총 카운트 가져오기
        AddList.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                AddList.LoadData("search");
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

    setTimeout(function () {
        if (gubun == "search") {
            loadSearchData();
        }
        else {
            loadData();
        }
    }, 100);
}



/**********************************************************************************************
작성자 : famo23 
작성일자 : 2015-04-27
작성목적 : 업체 리스트 가져오기
**********************************************************************************************/
AddList.GetPageList = function () {

    var url = "/DeskPlusEIP/Task/Ws/Draft/AddList.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': ListCount,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,        
        'itemID': ItemID,
        'itemSubID': ItemSubID
    };

    var obj = eval("AddList.ListSuccess");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자 : famo23 
작성일자 : 2015-04-27
작성목적 : 리스트 가져오기 성공시, 완료
**********************************************************************************************/
AddList.ListSuccess = function (data, status) {

    $('#phList').setTemplateURL('/DeskPlusEIP/Task/Jt/Draft/JTAddList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);

    setTimeout(function () {
        AddList.Resource();
    }, 100);
}

/**********************************************************************************************
작성자 : famo23 
작성일자 : 2015-04-27
작성목적 : 메뉴 다국어 처리
**********************************************************************************************/
AddList.Resource = function () {

    $("#r_title")[0].innerHTML = R_Title;
    $("#r_draftDate")[0].innerHTML = R_DraftDate;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoListData); }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-27
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
AddList.GetCount = function () {

    var url = "/DeskPlusEIP/Task/Ws/Draft/AddList.asmx/GetListCount";

    ParamArray = {
        'searchField': SearchField,
        'searchValue': SearchValue,
        'itemID': ItemID,
        'itemSubID': ItemSubID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AddList.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-27
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
AddList.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, ListCount);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-27
작성목적 : 확인
***************************************************/
AddList.Confirm = function () {

    var hdDraftID = $("#hdnDraftID").val();

    if (hdDraftID != "") {

        // 부모창이 문서 작성 페이지에서 벗어났을경우 익스플로러 기본 오류메세지 숨긴 후 창닫기
        try {
            if (window.opener && window.opener.open && !window.opener.closed) {
                opener.Add.GetDraft(hdDraftID);
            }
        }
        catch (e) { }

        self.close();
    }
    else {
        alert(R_NoSelectDraft);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-27
작성목적 : 취소
***************************************************/
AddList.Cancel = function () {

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**************************************************
작성자 : famo23 
작성일자 : 2015-04-27
작성목적 : 선택한 임시저장 리스트
***************************************************/
AddList.Choice = function (_draftID, _name) {

    $("#hdnDraftID").val(_draftID);
    $("#ctbName").val(_name);    
}

/**************************************************
작성자 : famo23 
작성일자 : 2014-05-14
작성목적 : 선택한 대분류
***************************************************/
AddList.ChoiceClose = function (_draftID, _name) {

    $("#hdnDraftID").val(_draftID);
    $("#ctbName").val(_name);    

    AddList.Confirm();
}
