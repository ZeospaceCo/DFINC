List = {}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
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

        //관리자 페이지 이동
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            List.Admin();
        });

        //이동
        $('#cbtnMove').click(function (e) {
            e.preventDefault();
            List.Move();
        });

        //전달
        $('#cbtnForward').click(function (e) {
            e.preventDefault();
            List.Forward();
        });

        //이관하기
        $('#cbtnPassed').click(function (e) {
            e.preventDefault();
            List.Passed();
        });

        //회람
        $('#cbtnCir').click(function (e) {
            e.preventDefault();
            List.Cir();
        });

        //타부서문서열람
        $("#cbtnOtherDept").click(function (e) {
            e.preventDefault();

            var url = "/DeskPlusEIP/Approval/Folder/OtherDept/Default.aspx";
            PopupWinOpen(url, "", "DiffDeptOpen", "1300", "620")
        });

        //초기 메뉴로 설정
        $('#cbtnDefault').click(function (e) {
            e.preventDefault();
            List.SetDefault();
        });

        List.Init();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $(".clsDate").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
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
작성자   : shimpark
작성일자 : 2011-04-11
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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
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
            loadBoardSearchData();
        }
        else {
            loadBoardData();
        }
    }, 100);
}



/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
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
작성자   : shimpark
작성일자 : 2011-04-11
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
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Dept/ListAll.asmx/GetPageList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'totalItemCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'formID': FormID,
        'yyyy': YYYY,
        'yyyymm': YYYYMM,
        'targetDeptID': TargetDeptID
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Folder/Dept/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

List.Resource = function () {

    //$("#r_number")[0].innerHTML = R_Number;
    $("#r_docnumber")[0].innerHTML = R_DocNumber;
    $("#r_subject")[0].innerHTML = R_Subject;
    $("#r_attachName")[0].innerHTML = R_AttachName;
    $("#r_name")[0].innerHTML = R_Name;
    $("#r_writetime")[0].innerHTML = R_WriteTime;
    $("#r_completetime")[0].innerHTML = R_CompleteDate;
    $("#r_senddeptname")[0].innerHTML = R_SendDept;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
    if ($(".IconSecurity") != undefined) { $(".IconSecurity").attr('src', ImagePath + '/Icon/Security.gif'); }
    if ($(".IconEmergency") != undefined) { $(".IconEmergency").attr('src', ImagePath + '/Icon/Emergency.gif'); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }

}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Dept/ListAll.asmx/GetListCount";
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'formID': FormID,
        'yyyy': YYYY,
        'yyyymm': YYYYMM,
        'targetDeptID': TargetDeptID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}




/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 결재 보기
***************************************************/
List.View = function (p_docID, groupID) {

    ParamArray = {
        'documentID': p_docID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'targetPage': TargetPage,
        'cabinetID': CabinetID,
        'types': Types,
        'formID': FormID,
        'yyyy': YYYY,
        'yyyymm': YYYYMM,
        'headerName': HeaderName,
        'folderStatusID': FolderStatusID
    };

    CallEncrypt(CallGetParam());

    //location.href = "/DeskPlusEIP/Approval/Folder/Common/View.aspx?p=" + GetEncryptValue;

    if (groupID < 800000) {
        url = "/DeskPlusEIP/Approval/Common/Popup33/ViewTot.aspx?p=" + GetEncryptValue;
        if ($.browser.msie) {
            PopupWinOpen(url, "", "OpenUrl", "1000", "745", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
        }
        else {
            PopupWinOpen(url, "", "OpenUrl", "1000", "700", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
        }
    }
    else {
        location.href = "/DeskPlusEIP/Approval/Folder/Common/View.aspx?p=" + GetEncryptValue;
    }
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



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 이동하기
**********************************************************************************************/
List.Move = function () {

    //키|구분자,키|구분자
    var selValue = List.CheckedValue();

    if (selValue != "") {

        ParamArray = {
            'docID': selValue
        };

        CallEncrypt(CallGetParam())

        var url = "Move.aspx?p=" + GetEncryptValue;

        PopupWin(400, 400, url, "-500", "", true);

    } else {
        alert(R_MSG_Move2);
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
작성자   : shimpark
작성일자 : 2012-05-08
작성목적 : 전달하기
**********************************************************************************************/
List.Forward = function () {

    //키|구분자,키|구분자
    var selValue = List.CheckedValue();

    if (selValue != "") {

        ParamArray = {
            'docID': selValue
        };

        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Approval/Folder/Dept/ForwardMemo.aspx?p=" + GetEncryptValue;
        PopupWin(800, 500, url, "-500", "", true);
    }
    else {
        alert(R_MSG_Forward);
    }

}




/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-05-08
작성목적 : 이관하기
**********************************************************************************************/
List.Passed = function () {

    //키|구분자,키|구분자
    var selValue = List.CheckedValue();

    if (selValue != "") {

        ParamArray = {
            'gubun': '2',
            'callback': 'GetPassedUser'
        };

        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Approval/Common/Popup/GetDeptUser.aspx?p=" + GetEncryptValue;
        PopupWin(400, 517, url, "-500", "", true);
    }
    else {
        alert(R_MSG_SelTransferDoc);
    }

}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-08
작성목적 : 이관 처리하기
**********************************************************************************************/
function GetPassedUser(response) {

    var rtnVal = response;

    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
        rtnVal = unescape(rtnVal);
        var responseStr = rtnVal.split("");

        var userList = responseStr[0];
        var docList = List.CheckedValue();

        var url = "/DeskPlusEIP/Approval/Ws/Folder/Dept/ListAll.asmx/AddPassed";

        ParamArray = {
            'docID': docList,
            'recvUser': userList
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("List.SuccessGetPassedUser");

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 이관 처리하기
**********************************************************************************************/
List.SuccessGetPassedUser = function (data, status) {
    if (data.d == true) {
        alert(R_MSG_SUCCESS);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 관리자 도구
***************************************************/
List.Admin = function () {
    var url = "/DeskPlusEIP/Approval/Admin/Default.aspx";
    parent.location.href = url;
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-05-08
작성목적 : 회람하기
**********************************************************************************************/
List.Cir = function () {

    //키|구분자,키|구분자
    var selValue = List.CheckedValue();

    if (selValue != "") {

        ParamArray = {
            'docID': selValue
        };

        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Approval/Folder/Dept/CirMemo.aspx?p=" + GetEncryptValue;
        PopupWin(800, 500, url, "-500", "", true);
    }
    else {
        alert(R_MSG_Cir);
    }

}





/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
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
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Dept/ListAll.asmx/GetAttachFileListMenu";
    //var data = "{categoryID :" + categoryID + ",listID:" + listID + "}";
    var data = "childDocID=" + p_childDocID;
    var obj = eval("List.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
List.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}


/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
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
작성자   : 이현수
작성일자 : 2008-08-29
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
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 첨부파일 전체 다운로드
***************************************************/
List.TotalDownLoad = function (_e) {
    var evt = new __event(_e);

    if (evt.target) {
        var arg = evt.target.getAttribute("arg");

        $("#fileList").val(arg);

        document.getElementById("frmTotalDownLoad").action = "/DeskPlusEIP/Common/DownLoad/TotalDownLoad.aspx";
        //$("#frmTotalDownLoad").action = "/DeskPlusEIP/Common/DownLoad/TotalDownLoad.aspx";

        PopupWinOpen("", "", "openWin", "600", "400")
        //window.open("", "openWin", "width=506px,height=388px,scrollbars=no,status=no,toolbar=no,resizable=0,location=no,menu=no");
        //$("#frmTotalDownLoad").submit();
        document.getElementById("frmTotalDownLoad").submit();

    }
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Dept_Doc",
        'pageSize': $("#ddlListCount").val(),
        'types': 'Complete'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessSavePageSize");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**************************************************
초기 메뉴로 설정(웹서비스 호출)
***************************************************/
List.SetDefault = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/SetDefaultAll.asmx/SetDefaultMenu";

    ParamArray = {
        'folderStatusID': FolderStatusID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessSetDefault");

    AjaxService(url, data, obj);
}
List.SuccessSetDefault = function (data, status) {
    alert(R_MSG_SUCCESS);
}

/**********************************************************************************************
중요표시 추가 및 삭제하기
**********************************************************************************************/
List.ModImpUser = function (_id, _arg) {
    if (_id == "") return;

    var url = "";

    if ($(_arg).attr("checked") == "checked") {
        $("." + _id).attr("checked", true);
        $("." + _id).next().addClass("on");
        //$(_arg).next().addClass("on");

        url = "/DeskPlusEIP/Approval/Ws/Folder/Impo/List.asmx/AddImpUser";
    }
    else {
        $("." + _id).attr("checked", false);
        $("." + _id).next().removeClass("on");
        //$(_arg).next().removeClass("on");

        url = "/DeskPlusEIP/Approval/Ws/Folder/Impo/List.asmx/DelImpUser";
    }

    ParamArray = { 'ids': _id.split("_")[1] };
    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessModImpUser");

    AjaxService(url, data, obj);
}
List.SuccessModImpUser = function (data, status) {
    if (data.d == true) {
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
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'cabinetID': CabinetID,
        'types': Types,
        'formID': FormID,
        'yyyy': YYYY,
        'yyyymm': YYYYMM,
        'headerName': HeaderName,
        'folderStatusID': FolderStatusID
    };

    CallEncrypt(CallGetParam());
    location = "ListAll.aspx?p=" + GetEncryptValue;
}
