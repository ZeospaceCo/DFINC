List = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-11-25
작성목적 : 데이터 초기화 
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


        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            List.SavePageSize();
            List.Search();
        });

        //날짜검색
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

        //복원
        $('#cbtnRestore').click(function (e) { e.preventDefault(); List.Restore(); });

        //삭제
        $('#cbtnDel').click(function (e) { e.preventDefault(); List.Del(); });

        List.Init();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $(".clsDate").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2013-11-25
작성목적 : 한달 전 날짜 값 구하기
***************************************************/
function getBeforeMonth() {
    var today = new Date();
    var beforeMonth = new Date(today.getFullYear(), today.getMonth() - 1, today.getDate());
    return beforeMonth;
}


/**************************************************
작성자   : famo23
작성일자 : 2013-11-25
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
   

    List.LoadData("");
    __addEventListener(document, "click", List.click);

    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : famo23
작성일자 : 2013-11-25
작성목적 : 등록일자 활성화 여부
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
작성일자 : 2013-11-25
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {    

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadDisuseData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadDisuseSearchData = function () {
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
            loadDisuseSearchData();
        }
        else {
            loadDisuseData();
        }

        var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
        $("#pageInfo").text(CurrentPage + " / " + pageCount);

    }, 100);
}



/**************************************************
작성자   : famo23
작성일자 : 2013-11-25
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
작성일자 : 2013-11-25
작성목적 : 검색
***************************************************/
List.Search = function () {

    var response = List.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  
    //List.LoadData("search");
    List.GoPageLoad();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-11-25
작성목적 : 리스트 호출
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Disuse/List.asmx/GetPageList";

    //파라미터값 배열에 담기    
    ParamArray = {
        'totalItemCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'formID': FormID
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-11-25
작성목적 : 성공시 리스트 표시
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#phList').setTemplateURL('/DeskPlusEIP/Approval/Jt/Disuse/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-11-25
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {

    $("#r_docid")[0].innerHTML = R_DocID;
    $("#r_docnumber")[0].innerHTML = R_DocNumber;
    $("#r_number3")[0].innerHTML = R_Number3;
    $("#r_subject")[0].innerHTML = R_Subject;
    $("#r_attachName")[0].innerHTML = R_AttachName;
    $("#r_number1")[0].innerHTML = R_Number1;
    $("#r_name")[0].innerHTML = R_Name;
    $("#r_writetime")[0].innerHTML = R_Writetime;
    $("#r_completetime")[0].innerHTML = R_Completetime;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-11-25
작성목적 : 웹서비스 카운트  호출
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Disuse/List.asmx/GetListCount";
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'formID': FormID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-11-25
작성목적 : 성공시 , 결재 카운트 이벤트
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-11-25
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


    List.LoadData("search");
}

/**************************************************
작성자   : famo23
작성일자 : 2013-11-25
작성목적 : 결재 보기
***************************************************/
List.View = function (p_docID) {

    ParamArray = {
        'documentID': p_docID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'formID': FormID
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;
}



/**************************************************
작성자   : famo23
작성일자 : 2013-11-25
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

List.GetAttachFileListMenu = function (p_childDocID) {
    var url = "/DeskPlusEIP/Approval/Ws/Disuse/List.asmx/GetAttachFileListMenu";    
    var data = "childDocID=" + p_childDocID;
    var obj = eval("List.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-11-25
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
List.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}


/**************************************************
작성자   : famo23
작성일자 : 2013-11-25
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
List.ShowAttachContextMenu = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var childDocID = el.getAttribute("childDocID");

    List.GetAttachFileListMenu(childDocID);

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
        oContextMenuCtrl.show(e);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2013-11-25
작성목적 : 첨부파일 열기
***************************************************/
List.DownLoad = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        //$("#ifrmAttachFile")[0].src = arg;     
        window.open(arg);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2013-11-25
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


/**************************************************
작성자   : famo23
작성일자 : 2013-11-25
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Disuse_Doc",
        'pageSize': $("#ddlListCount").val(),
        'types': 'Admin'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessSavePageSize");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2013-11-25
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-19
작성목적 : 페이지이동 처리
**********************************************************************************************/
List.GoPageLoad = function () {

    ParamArray = {
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'formID': FormID
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 선택된 값 가져오기
***************************************************/
List.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**********************************************************************************************
문서복원
**********************************************************************************************/
List.Restore = function () {

    var selValue = List.CheckedValue();
    if (selValue != "") {
        if (window.confirm(R_RestoreDoc)) {
            var url = "/DeskPlusEIP/Approval/Ws/Disuse/View.asmx/RestoreDocList";
            ParamArray = {
                'docID': selValue
            };

            CallEncrypt(CallGetParam());
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("List.SuccessRestore");

            AjaxService(url, data, obj);
        }
    }
    else {
        alert("복원할 문서를 선택해 주세요.");
    }
}
List.SuccessRestore = function (data, status) {

    if (data.d == true) {
        alert(R_MSG_SUCCESS);
        var response = List.SettingSearch();
        if (response == false) return;

        CurrentPage = 1; //1페이지로 이동처리  
        List.LoadData("search");
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 선택한 값 삭제하기
***************************************************/
List.Del = function () {

    var selValue = List.CheckedValue();
    if (selValue != "") {

        if (window.confirm(R_MSG_Del2)) {
            var url = "/DeskPlusEIP/Approval/Ws/Disuse/View.asmx/DelDocList";

            ParamArray = {
                'docID': selValue
            };

            CallEncrypt(CallGetParam());
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("List.SuccessDel");

            AjaxService(url, data, obj);
        }

    } else {
        alert(R_MSG_Del3);
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 문서 삭제
**********************************************************************************************/
List.SuccessDel = function (data, status) {

    if (data.d == true) {
        alert(R_MSG_Del4);
        var response = List.SettingSearch();
        if (response == false) return;

        CurrentPage = 1; //1페이지로 이동처리  
        List.LoadData("search");
    }
}