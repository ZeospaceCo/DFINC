List = {}
List.TreeFrame = parent.frmLeft;

/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-13
작성목적 : 초기화 
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            List.LoadData("");
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
        $('#CbtnSearch').click(function (e) {
            e.preventDefault();
            List.Search();
        });

        //복원
        $('#CbtnRestore').click(function (e) {
            e.preventDefault();
            List.Restore();
        });

        //삭제
        $('#CbtnDel').click(function (e) {
            e.preventDefault();
            List.Del();
        });

        //전체삭제
        $('#CbtnWDel').click(function (e) {
            e.preventDefault();
            List.WDel();
        });

        //다운로드
        $('#CbtnDownload').click(function (e) {
            e.preventDefault();
            List.FileDownload();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            List.ModFolderUser("1");
            List.GoPageListType("1");
        });

        //리스트형 저장
        $('#listType1').click(function () {
            List.ModFolderUser("1");
            List.GoPageListType("1");
        });

        //Icon 형 저장
        $('#listType2').click(function () {
            List.ModFolderUser("2");
            List.GoPageListType("2");
        });

        //Preview 형 저장
        $('#listType3').click(function () {
            List.ModFolderUser("3");
            List.GoPageListType("3");
        });

        $("#startEndDate").hide();

        $(".clsDate").keydown(function (e) { return false; });

        List.LoadData("");

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        __addEventListener(document, "click", List.click);

        $("#zeoModalContact").draggable({ disabled: false });
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2011-12-13
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
작성일자 : 2011-12-13
작성목적 : 검색
***************************************************/
List.Search = function () {
    var response = List.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리     

    List.LoadData("search");
}

/**************************************************
작성자   : famo23
작성일자 : 2011-12-13
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadWebHardData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadWebHardSearchData = function () {
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
            loadWebHardSearchData();
        }
        else {
            loadWebHardData();
        }
    }, 100);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-13
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/WebHard/Ws/Trash/List.asmx/GetTrashPageList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'uID': UID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'order': Order,
        'types': Types
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-13
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#phTrashList').setTemplateURL('/DeskPlusEIP/WebHard/Jt/Trash/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phTrashList').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-13
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {
    $("#r_folderName")[0].innerHTML = R_FolderName;
    $("#r_fileName")[0].innerHTML = R_FileName;
    $("#r_download")[0].innerHTML = R_Download;
    $("#r_fileLength")[0].innerHTML = R_FileLength;
    $("#r_fileType")[0].innerHTML = R_FileType;
    $("#r_regUser")[0].innerHTML = R_RegUser;
    $("#r_writeDate")[0].innerHTML = R_RegDate;

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

    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-13
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/WebHard/Ws/Trash/List.asmx/GetTrashListCount";
    ParamArray = {
        'uID': UID,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'types': Types
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-13
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-13
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
작성일자 : 2011-12-13
작성목적 : 클릭 이벤트 분기
***************************************************/
List.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": List.DownLoad(e); break;
    }
}

var oContextMenuCtrl = null;
/**************************************************
작성자   : famo23
작성일자 : 2011-12-13
작성목적 : 첨부파일 열기
***************************************************/
List.DownLoad = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var attachFilePath = el.getAttribute("FilePath");
    attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + attachFilePath;

    window.open(attachFilePath);

}

/**************************************************
작성자   : famo23
작성일자 : 2011-12-13
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
작성일자 : 2011-12-13
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
작성일자 : 2011-12-14
작성목적 : 복원
***************************************************/
List.Restore = function () {

    var selValue = List.CheckedValue();
    var tmpValue = "";

    if (selValue != "") {
        if (confirm(R_RestoreFile)) {

            for (var i = 0; i < selValue.length; i++) {
                tmpValue += selValue[i] + ",";
            }

            ParamArray = {
                'deleteYN': 'N',
                'tmpValue': tmpValue
            };

            CallEncrypt(CallGetParam());

            if (Types == "B") {
                var data = "{p:'" + GetEncryptValue + "'}";
                var obj = eval("List.SuccessRestore");
                var url = "/DeskPlusEIP/WebHard/Ws/Trash/List.asmx/UpdPublicTrash";

                AjaxService(url, data, obj);
            }
            else {
                var data = "{p:'" + GetEncryptValue + "'}";
                var obj = eval("List.SuccessDel");
                var url = "/DeskPlusEIP/WebHard/Ws/Trash/List.asmx/UpdTrash";

                AjaxService(url, data, obj);
            }            
        }

    } else {
        alert(R_SelectRestore);
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-13
작성목적 : 휴지통 복원 체크
**********************************************************************************************/
List.SuccessRestore = function (data, status) {
    var rtn = data.d

    if (rtn == "1") {
        var folderID = "";

        if (Types == "T") {
            folderID = "-4";
        }
        else {
            folderID = "-6";
        }

        if (Types == "U" || Types == "L" || Types == "B") {
            var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetPublicUseCount";

            ParamArray = {
                'folderID': folderID,
                'types': Types
            };

            CallEncrypt(CallGetParam());
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("List.SuccessLoadPublic");

            AjaxService(url, data, obj);
        }
        else {
            var obj = eval("List.SuccessLoad");
            var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetUseCount";

            AjaxService(url, "", obj);
        }
    }
    else if (rtn == "2") {
        alert(R_PublicFolderCount);
    }
    else {
        alert(R_ProcessingError);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-14
작성목적 : 파일 완전 삭제
**********************************************************************************************/
List.Del = function () {

    var selValue = List.CheckedValue();
    var tmpValue = "";

    if (selValue != "") {
        if (confirm(R_DelFile)) {

            for (var i = 0; i < selValue.length; i++) {
                tmpValue += selValue[i] + ",";
            }

            ParamArray = {
                'fileList': tmpValue,
                'order': Order
            };

            CallEncrypt(CallGetParam());

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("List.SuccessDel");
            var url = "/DeskPlusEIP/WebHard/Ws/Trash/List.asmx/DelFile";

            AjaxService(url, data, obj);
        }

    } else {
        alert(R_SelectDel);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-13
작성목적 : 파일 삭제 (웹서비스 값 받기) -> 총 사용 용량 가져오기
**********************************************************************************************/
List.SuccessDel = function (data, status) {

    if (data.d) {

        var folderID = "";

        if (Types == "T") {
            folderID = "-4";
        }
        else {
            folderID = "-6";
        }

        if (Types == "U" || Types == "L" || Types == "B") {
            var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetPublicUseCount";

            ParamArray = {
                'folderID': folderID,
                'types': Types
            };

            CallEncrypt(CallGetParam());
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("List.SuccessLoadPublic");

            AjaxService(url, data, obj);
        }
        else {
            var obj = eval("List.SuccessLoad");
            var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetUseCount";

            AjaxService(url, "", obj);
        }
    }
    else {
        alert(R_ProcessingError);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-13
작성목적 : 파일 업로드 및 삭제 한 후에 페이지 로드
**********************************************************************************************/
List.SuccessLoad = function (data, status) {

    alert(R_CompleteConfirm);

    // 트리의 그래픽 바 , 퍼센트 보여주기
    var tmpUseCount = data.d;
    var tmpPercentCount = "0%";

    if (tmpUseCount == "0") {
        tmpPercentCount = "0";
    }
    else if (PrivateQuota == "0") {
        tmpPercentCount = "0";
    }
    else {
        tmpPercentCount = ((tmpUseCount / (PrivateQuota * 1048576)) * 100);

        if ((0 < tmpPercentCount) && (tmpPercentCount < 1)) {
            tmpPercentCount = "0.1";
        }
        else {
            tmpPercentCount = tmpPercentCount.toFixed(1);
        }
    }

    if (tmpPercentCount > 100) {
        tmpPercentCount = 100;
    }

    // Format 맞추어서 사용량 보여 주기 
    if (tmpUseCount < 1024) {
        if (tmpUseCount == 0) {
            tmpUseCount = "0 B";
        }
        else {
            tmpUseCount = tmpUseCount.toFixed(1) + "B";
        }
    }
    else if (tmpUseCount < 1024 * 1024) {
        tmpUseCount = (tmpUseCount / 1024).toFixed(1) + "KB";
    }
    else if (tmpUseCount < 1024 * 1024 * 1024) {
        tmpUseCount = (tmpUseCount / (1024 * 1024)).toFixed(1) + "MB";
    }
    else {
        tmpUseCount = (tmpUseCount / (1024 * 1024 * 1024)).toFixed(1) + "GB";
    }

    List.TreeFrame.$("#userTotal").text(tmpUseCount);
    List.TreeFrame.$("#percent").text(tmpPercentCount + "%");
    List.TreeFrame.$("#graphBar").width(tmpPercentCount + "%");

    // 페이지 리스트 호출 
    List.Search();
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-01
작성목적 : 폴더용량 확인
**********************************************************************************************/
List.SuccessLoadPublic = function (data, status) {

    alert(R_CompleteConfirm);
 
    var folderData = data.d;
    var PUseCount = 0;
    var PPrivateQuota = 0;
    var PDisPlayQuota = 0;

    if (Types == "B") {
        PUseCount = folderData.split("|")[1];
        PPrivateQuota = 0;
    }
    else {
        PUseCount = folderData.split("|")[1];
        PPrivateQuota = folderData.split("|")[0];
    }

    if (PPrivateQuota == "0" || PPrivateQuota == "") {
        PDisPlayQuota = "∞";
    }
    else {
        //용량에 맞춰서 보여주기
        if (PPrivateQuota < 1024) {
            PDisPlayQuota = (parseFloat(PPrivateQuota)).toFixed(1) + "KB";
        }
        else if (PPrivateQuota < 1024 * 1024) {
            PDisPlayQuota = (parseFloat(PPrivateQuota) / (1024)).toFixed(1) + "MB";
        }
        else {
            PDisPlayQuota = (parseFloat(PPrivateQuota) / (1024 * 1024)).toFixed(1) + "GB";
        }
    }

    var tmpPercentCount = "0%";

    if (PUseCount == "0") {
        tmpPercentCount = "0";
    }
    else if (PPrivateQuota == "0") {
        tmpPercentCount = "0";
    }
    else {
        tmpPercentCount = ((PUseCount / (PPrivateQuota * 1024)) * 100);

        if ((0 < tmpPercentCount) && (tmpPercentCount < 1)) {
            tmpPercentCount = "0.1";
        }
        else {
            tmpPercentCount = parseFloat(tmpPercentCount).toFixed(1);
        }
    }

    if (tmpPercentCount > 100) {
        tmpPercentCount = 100;
    }

    var tmpUseCount = PUseCount;

    // Format 맞추어서 사용량 보여 주기 
    if (tmpUseCount < 1024) {
        if (tmpUseCount == 0) {
            tmpUseCount = "0 B";
        }
        else {
            tmpUseCount = parseFloat(tmpUseCount).toFixed(1) + "B";
        }
    }
    else if (tmpUseCount < 1024 * 1024) {
        tmpUseCount = (parseFloat(tmpUseCount) / 1024).toFixed(1) + "KB";
    }
    else if (tmpUseCount < 1024 * 1024 * 1024) {
        tmpUseCount = (parseFloat(tmpUseCount) / (1024 * 1024)).toFixed(1) + "MB";
    }
    else {
        tmpUseCount = (parseFloat(tmpUseCount) / (1024 * 1024 * 1024)).toFixed(1) + "GB";
    }    

    List.TreeFrame.$("#userTotal").text(tmpUseCount);
    List.TreeFrame.$("#userQuota").text(PDisPlayQuota);
    List.TreeFrame.$("#graphBar").width(tmpPercentCount + "%");
    List.TreeFrame.$("#percent").text(tmpPercentCount + "%");

    // 페이지 리스트 호출 
    List.Search();
}
/**************************************************
작성자   : famo23
작성일자 : 2011-12-13
작성목적 : 파일 다운로드
***************************************************/
List.FileDownload = function () {

    var selValue = List.CheckedValue();
    var tmpValue = "";

    if (selValue != "") {

        for (var i = 0; i < selValue.length; i++) {
            tmpValue += selValue[i] + ",";
        }

        ParamArray = {
            'fileList': tmpValue,
            'order': Order
        };

        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/WebHard/Content/Download.aspx";
        var parm = "p=" + GetEncryptValue;
        PopupWinOpen(url, parm, "openWin", "600", "400")

    } else {
        alert(R_SelectDownload);
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2011-12-13
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

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-09
작성목적 : 리스트 타입선택시 이동하기
**********************************************************************************************/
List.GoPageListType = function (gubun) {

    var iPath = "List.apsx";

    switch (gubun) {
        case "1":
            iPath = "List.aspx";
            break;
        case "2":
            iPath = "ListIcon.aspx";
            break;
        case "3":
            iPath = "ListPreview.aspx";
            break;
        default:
            iPath = "List.aspx";
            break;
    }

    var response = List.SettingSearch();
    if (response == false) return;


    ParamArray = {
        'currentPage': 1,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'types': Types
    };

    CallEncrypt(CallGetParam());

    //이동하기    
    location = iPath + "?p=" + GetEncryptValue;
}



/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-11
작성목적 : 폴더별 사용자 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
List.ModFolderUser = function (gubun) {

    var folderID = "";

    if (Types == "T") {
        folderID = "-4";
    }
    else {
        folderID = "-6";
    }

    var url = "/DeskPlusEIP/WebHard/Ws/Trash/List.asmx/ModFolderUser";
    ParamArray = {
        'folderID': folderID,
        'listType': gubun,
        'pageSize': $("#ddlListCount").val(),
        'types': Types
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessModFolderUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-11
작성목적 : 폴더별 사용자 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessModFolderUser = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-04-09
작성목적 : 파일 전체 삭제
**********************************************************************************************/
List.WDel = function () {

    if (confirm(R_WDelFile)) {

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("List.SuccessWDel");
        var url = "/DeskPlusEIP/WebHard/Ws/Trash/List.asmx/WDelFile";

        AjaxService(url, data, obj);
    }

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-04-09
작성목적 : 파일 전체 삭제 (웹서비스 값 받기) -> 총 사용 용량 가져오기
**********************************************************************************************/
List.SuccessWDel = function (data, status) {
    if (data.d) {

        var folderID = "";

        if (Types == "T") {
            folderID = "-4";
        }
        else {
            folderID = "-6";
        }

        if (Types == "U" || Types == "L" || Types == "B") {
            var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetPublicUseCount";

            ParamArray = {
                'folderID': folderID,
                'types': Types
            };

            CallEncrypt(CallGetParam());
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("List.SuccessLoadPublic");

            AjaxService(url, data, obj);
        }
        else {
            var obj = eval("List.SuccessLoad");
            var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetUseCount";

            AjaxService(url, "", obj);
        }
    }
    else {
        alert(R_ProcessingError);
    }
}
