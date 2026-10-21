List = {}

var oContextMenuCtrl = null;
/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-09
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            //List.LoadData("");
            List.GoPageLoad();
        }
    }

    $(document).ready(function () {
        RecordPerPage = $("#ddlListCount").val();

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

        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            List.RegDate();
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

        //추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            List.Del();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            List.ModCategoryUser();
        });

        //순서변경
        $('#CbtnMoveList').click(function (e) {
            e.preventDefault();            
            List.MoveList();
        });

        //하위폴더 순서 변경
        $('#CbtnMoveSubFolder').click(function (e) {
            e.preventDefault();
            List.MoveSubFolder();
        });

        //상위 폴더 변경
        $('#CbtnParentFolder').click(function (e) {
            e.preventDefault();
            List.SetParentFolder();
        });

        List.Init();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $(".clsDate").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2012-02-22
작성목적 : 초기화 하기
***************************************************/
List.Init = function () {
    $("#startEndDate").hide(); //기간

    if (SearchTerm == "period") {
        $("#startEndDate").show(); //기간
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);

        if (SearchTermValue != "") {
            $("#startDate").val(SearchTermValue.split(",")[0]);
            $("#endDate").val(SearchTermValue.split(",")[1]);
        }
    }

    if (CategoryID == 0) {
        $("#CbtnMoveList").css("display", "none");
    }


    List.LoadData("");
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-09
작성목적 : 달력 보이기
***************************************************/
List.RegDate = function () {
    //기간 선택
    var sField = $("#ddlSearchTerm").val();
    if (sField == "period") {
        $("#startEndDate").show(); //기간
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);
    } else {
        $("#startEndDate").hide(); //기간 이외
    }

    $("#startDate").val("");
    $("#endDate").val("");
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-09
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
작성일자 : 2012-02-09
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadRMData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadRMSearchData = function () {
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

        RecordPerPage = $("#ddlListCount").val();

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

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        if (gubun == "search") {
            loadRMSearchData();
        }
        else {
            loadRMData();
        }
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-09
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/RM/Ws/Admin/Content/List.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CategoryID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'order': Order
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-09
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#phList').setTemplateURL('/DeskPlusEIP/RM/Jt/Admin/Content/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-09
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {
    $("#r_title")[0].innerHTML = R_ResourcesTitle;
    $("#r_qty")[0].innerHTML = R_Qty;
    $("#r_writeDate")[0].innerHTML = R_WriteDate;
    $("#r_writeUserName")[0].innerHTML = R_WriteUserName;
    //$("#r_ListOrder")[0].innerHTML = R_ListOrder; //"순서";

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }

    var tmpOrder = Order.replace(" asc", "").replace(" desc", "");
    var colValue = tmpOrder + " asc";

    if (Order == colValue) {
        $("#a_" + tmpOrder).addClass("asc");
    }
    else {
        $("#a_" + tmpOrder).addClass("desc");
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-09
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/RM/Ws/Admin/Content/List.asmx/GetListCount";
    ParamArray = {
        'categoryID': CategoryID,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-09
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-09
작성목적 : 정렬
**********************************************************************************************/
List.Sort = function (col) {
    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;

    //List.LoadData("search");
    List.GoPageLoad();
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-09
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
작성일자 : 2012-02-09
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
작성일자 : 2012-02-09
작성목적 : 검색 필드 설정하기
***************************************************/
List.SettingSearch = function () {
    SearchTerm = $("#ddlSearchTerm").val();
    SearchField = $("#ddlSearchField").val();

    if (SearchTerm == "period") {
        var getStartDate = $("#startDate").val();
        var getEndDate = $("#endDate").val();

        if (getStartDate == "" || getEndDate == "") {
            alert(R_ApplyRegDate);
            return false;
        }
        else {
            var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

            if (dtResponse == false) {
                alert(R_StartDateEndDate);
                return false;
            }
        }

        SearchTermValue = getStartDate + "," + getEndDate;
    }

    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    return true;
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-09
작성목적 : 리스트 삭제 및 추가에 따른 리스트 재로딩
***************************************************/
List.ListLoad = function () {
    document.all("ddlSearchTerm").selectedIndex = 0;
    document.all("ddlSearchField").selectedIndex = 0;

    $("#ctbSearch").val("");
    $("#startDate").val("");
    $("#endDate").val("");
    $("#startEndDate").hide(); //기간 이외

    CurrentPage = 1; //1페이지로 이동처리

    List.LoadData("search");
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-07
작성목적 : 자원 추가
**********************************************************************************************/
List.Add = function () {
    ParamArray = {
        'categoryID': CategoryID
    };

    CallEncrypt(CallGetParam());
    location.href = "Add.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-09
작성목적 : 자원 삭제
**********************************************************************************************/
List.Del = function () {
    var selValue = List.CheckedValue();
    var tmpValue = "";

    if (selValue != "") {
        if (confirm(R_DelList)) {
            for (var i = 0; i < selValue.length; i++) {
                tmpValue += selValue[i] + ",";
            }

            ParamArray = {
                'activeUse': '0',
                'tmpValue': tmpValue
            };

            CallEncrypt(CallGetParam());

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("List.SuccessDel");
            var url = "/DeskPlusEIP/RM/Ws/Admin/Content/List.asmx/Upd";

            AjaxService(url, data, obj);
        }
    } else {
        alert(R_SelectDel);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-09
작성목적 : 파일 삭제 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessDel = function (data, status) {
    if (data.d) {
        // 페이지 리스트 호출
        List.ListLoad();
    }
    else {
        alert(R_ProcessingError);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-10
작성목적 : 폴더별 사용자 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
List.ModCategoryUser = function (gubun) {
    var url = "/DeskPlusEIP/RM/Ws/Admin/Content/List.asmx/ModCategoryUser";
    ParamArray = {
        'categoryID': CategoryID,
        'pageSize': $("#ddlListCount").val(),
        'types': "L"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessModCategoryUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-10
작성목적 : 폴더별 사용자 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessModCategoryUser = function (data, status) {
    if (data.d) {
        RecordPerPage = $("#ddlListCount").val();
        List.Search();
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-10
작성목적 : 자원 보기
**********************************************************************************************/
List.View = function (listID) {
    ParamArray = {
        'categoryID': CategoryID,
        'listID': listID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-19
작성목적 : 페이지이동 처리
**********************************************************************************************/
List.GoPageLoad = function () {

    ParamArray = {
        'categoryID': CategoryID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'recordPerPage': RecordPerPage,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성목적 : 자원순서 변경하기
***************************************************/
List.MoveList = function () {
    ParamArray = {
        'categoryID': CategoryID
    };
    
    CallEncrypt(CallGetParam());
    var url = "ListUpDown.aspx?p=" + GetEncryptValue;
    PopupWin(500, 382, url, "", "", true);

}

/**************************************************
작성자   : shimpark
작성목적 : 하위폴더순서 변경하기
***************************************************/
List.MoveSubFolder = function () {
    ParamArray = {
        'categoryID': CategoryID        
    };

    CallEncrypt(CallGetParam());
    var url = "SubFolderUpdown.aspx?p=" + GetEncryptValue;

    PopupWin(400, 400, url, "-500", "", true);
}


/**************************************************
하위폴더 순서 변경 후 트리 새로고침
***************************************************/
List.GoTree = function () {
    //파라미터값 배열에 담기
    ParamArray = {
        'accActive': 0        
    };
    CallEncrypt(CallGetParam());
    var goUrl = "/DeskPlusEIP/RM/Admin/Folder/Tree.aspx?p=" + GetEncryptValue;

    if (parent.frmLeft) parent.frmLeft.location.href = goUrl;
}
