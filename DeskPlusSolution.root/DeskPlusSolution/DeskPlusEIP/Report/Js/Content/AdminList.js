AdminList = {}

/**************************************************
초기화
***************************************************/
$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            //AdminList.LoadData("");
            AdminList.GoPageLoad();
        }
    }

    $(document).ready(function () {
        $("#ddlListCount").val(RecordPerPage);

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

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            AdminList.Search()
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                AdminList.Search();
            }
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            AdminList.ModSetting();
        });

        //사용자 및 부서 보이기,감추기
        $('#ddlFolder').change(function () {
            $("#sendInfo").html("");
            $("#receiveInfo").html("");

            TargetUserID = "0"; //변경전 사용자 아이디
            TargetDeptID = "0"; //변경전 부서 아이디

            AfterUserID = "0"; //변경후 사용자 아이디
            AfterDeptID = "0"; //변경후 부서 아이디

            AdminList.ShowHidFolder();
        });

        //[변경 전 부서] 리스트 가져오기
        $('#cbtnBeforeDept').click(function (e) {
            e.preventDefault();

            BeforeAfter = "before";

            ParamArray = { 'callback': 'AdminList.SetDept', 'isCheckBox': '0' };
            CallEncrypt(CallGetParam());

            var url = "/DeskPlusEIP/Approval/Common/Popup/GetDept.aspx?p=" + GetEncryptValue;
            PopupWin(400, 517, url, "-500", "", true);
        });

        //[변경 전 삭제 부서] 리스트 가져오기
        $('#cbtnBeforeDeptGarbage').click(function (e) {
            e.preventDefault();

            $("#ctbSearch").val("");
            BeforeAfter = "before";

            ParamArray = { 'callback': 'AdminList.SelDelDept' };
            CallEncrypt(CallGetParam());

            var url = "/DeskPlusEIP/Report/Common/DeptBackup.aspx?p=" + GetEncryptValue;
            PopupWin(400, 350, url, "-500", "", true);
        });

        //[변경 후 부서] 가져오기
        $('#cbtnAfterDept').click(function (e) {
            e.preventDefault();

            BeforeAfter = "after";

            ParamArray = { 'callback': 'AdminList.SetDept', 'isCheckBox': '0' };
            CallEncrypt(CallGetParam());

            var url = "/DeskPlusEIP/Approval/Common/Popup/GetDept.aspx?p=" + GetEncryptValue;
            PopupWin(400, 517, url, "-500", "", true);
        });

        //[변경 전, 사용자] 리스트 가져오기
        $('#cbtnBeforeUser').click(function (e) {
            e.preventDefault();

            $("#ctbSearch").val("");
            BeforeAfter = "before";

            ParamArray = { 'gubun': '2', 'callback': 'AdminList.SetUser', 'isCheckBox': '0' };
            CallEncrypt(CallGetParam());

            var url = "/DeskPlusEIP/Approval/Common/Popup/GetDeptUser.aspx?p=" + GetEncryptValue;
            PopupWin(400, 517, url, "-500", "", true);
        });

        //[변경 후, 사용자] 리스트 가져오기
        $('#cbtnAfterUser').click(function (e) {
            e.preventDefault();

            $("#ctbSearch").val("");
            BeforeAfter = "after";

            ParamArray = { 'gubun': '2', 'callback': 'AdminList.SetUser', 'isCheckBox': '0' };
            CallEncrypt(CallGetParam());

            var url = "/DeskPlusEIP/Approval/Common/Popup/GetDeptUser.aspx?p=" + GetEncryptValue;
            PopupWin(400, 517, url, "-500", "", true);
        });

        // 선택이관 - 부서 및 사용자
        $('#cbtnTransfer').click(function (e) {
            e.preventDefault();

            var lists = AdminList.CheckedValue();
            var folderVal = $("#ddlFolder").val();
            if (lists == "") { alert(R_SelectMoveList); return; }

            var obj = eval("AdminList.SuccessLink");

            if (folderVal == "Dept") {
                //부서

                if (AfterDeptID == "0") { alert(R_SelectDeptAfterDept); return; }

                var url = "/DeskPlusEIP/Report/Ws/Content/AdminList.asmx/MoveDeptSelList";
                //alert(TotalItemCount);
                //파라미터값 배열에 담기

                ParamArray = {
                    'listID': lists,
                    'afterDeptID': AfterDeptID
                };

                CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
                data = "{p:'" + GetEncryptValue + "'}";
                AjaxService(url, data, obj);
            } else if (folderVal == "User") {
                //사용자
                if (AfterUserID == "0") { alert(R_SelectUserAfterUser); return; }

                var url = "/DeskPlusEIP/Report/Ws/Content/AdminList.asmx/MoveUserSelList";
                //alert(TotalItemCount);
                //파라미터값 배열에 담기

                ParamArray = {
                    'listID': lists,
                    'afterUserID': AfterUserID
                };

                CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
                data = "{p:'" + GetEncryptValue + "'}";
                AjaxService(url, data, obj);
            }
        });

        // 전체이관 - 부서,사용자
        $('#cbtnTransferAll').click(function (e) {
            e.preventDefault();

            var lists = AdminList.CheckedValue();
            var folderVal = $("#ddlFolder").val();

            var obj = eval("AdminList.SuccessLink");

            if (folderVal == "Dept") {
                //부서
                if (TargetDeptID == "0") { alert(R_SelectDeptBeforeDept); return; }
                if (AfterDeptID == "0") { alert(R_SelectDeptAfterDept); return; }

                var url = "/DeskPlusEIP/Report/Ws/Content/AdminList.asmx/MoveDeptAllList";

                ParamArray = {
                    'beforeDeptID': TargetDeptID,
                    'afterDeptID': AfterDeptID
                };

                CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
                data = "{p:'" + GetEncryptValue + "'}";
                AjaxService(url, data, obj);
            } else if (folderVal == "User") {
                //사용자
                if (TargetUserID == "0") { alert(R_SelectUserBeforeUser); return; }
                if (AfterUserID == "0") { alert(R_SelectUserAfterUser); return; }

                var url = "/DeskPlusEIP/Report/Ws/Content/AdminList.asmx/MoveUserAllList";

                ParamArray = {
                    'beforeUserID': TargetUserID,
                    'afterUserID': AfterUserID
                };

                CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
                data = "{p:'" + GetEncryptValue + "'}";
                AjaxService(url, data, obj);
            }
        });

        // 선택복사 - 부서 및 사용자
        $('#cbtnCopy').click(function (e) {
            e.preventDefault();

            var lists = AdminList.CheckedValue();
            var folderVal = $("#ddlFolder").val();

            if (lists == "") { alert(R_SelectCopyList); return; }

            var obj = eval("AdminList.SuccessLink");

            if (folderVal == "Dept") {
                //부서

                if (AfterDeptID == "0") { alert(R_SelectDeptAfterDept); return; }

                var url = "/DeskPlusEIP/Report/Ws/Content/AdminList.asmx/CopyDeptSelList";
                //alert(TotalItemCount);
                //파라미터값 배열에 담기

                ParamArray = {
                    'listID': lists,
                    'afterDeptID': AfterDeptID
                };

                CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
                data = "{p:'" + GetEncryptValue + "'}";
                AjaxService(url, data, obj);
            } else if (folderVal == "User") {
                //사용자
                if (AfterUserID == "0") { alert(R_SelectUserAfterUser); return; }

                var url = "/DeskPlusEIP/Report/Ws/Content/AdminList.asmx/CopyUserSelList";
                //alert(TotalItemCount);
                //파라미터값 배열에 담기

                ParamArray = {
                    'listID': lists,
                    'afterUserID': AfterUserID
                };

                CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
                data = "{p:'" + GetEncryptValue + "'}";
                AjaxService(url, data, obj);
            }
        });

        // 전체복사 - 부서,사용자
        $('#cbtnCopyAll').click(function (e) {
            e.preventDefault();

            var lists = AdminList.CheckedValue();
            var folderVal = $("#ddlFolder").val();

            var obj = eval("AdminList.SuccessLink");

            if (folderVal == "Dept") {
                //부서
                if (TargetDeptID == "0") { alert(R_SelectDeptBeforeDept); return; }
                if (AfterDeptID == "0") { alert(R_SelectDeptAfterDept); return; }

                var url = "/DeskPlusEIP/Report/Ws/Content/AdminList.asmx/CopyDeptAllList";

                ParamArray = {
                    'beforeDeptID': TargetDeptID,
                    'afterDeptID': AfterDeptID
                };

                CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
                data = "{p:'" + GetEncryptValue + "'}";
                AjaxService(url, data, obj);
            } else if (folderVal == "User") {
                //사용자
                if (TargetUserID == "0") { alert(R_SelectUserBeforeUser); return; }
                if (AfterUserID == "0") { alert(R_SelectUserAfterUser); return; }

                var url = "/DeskPlusEIP/Report/Ws/Content/AdminList.asmx/CopyUserAllList";

                ParamArray = {
                    'beforeUserID': TargetUserID,
                    'afterUserID': AfterUserID
                };

                CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
                data = "{p:'" + GetEncryptValue + "'}";
                AjaxService(url, data, obj);
            }
        });

        // 삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();

            var lists = AdminList.CheckedValue();
            if (lists == "") { alert(R_SelectDeleteList); return; }

            if (window.confirm(R_ApplyDelete)) {
                var url = "/DeskPlusEIP/Report/Ws/Content/AdminList.asmx/Del";

                ParamArray = {
                    'listID': lists
                };

                var obj = eval("AdminList.SuccessLink");
                CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
                data = "{p:'" + GetEncryptValue + "'}";
                AjaxService(url, data, obj);
            }
        });

        AdminList.ShowHidFolder();

        //        cbtnBeforeDept;
        //        cbtnBeforeDeptGarbage;
        //        cbtnAfterDept;

        //        cbtnBeforeUser;
        //        cbtnAfterUser;

        //        cbtnTransfer;
        //        cbtnTransferAll;
        //        cbtnCopy;
        //        cbtnCopyAll;

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        AdminList.Init();
    });
});

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 이관 처리
**********************************************************************************************/
AdminList.SuccessLink = function (data, status) {
    if (data.d == true) {
        alert(R_Processing);
        AdminList.Search();
    }
    else {
        alert(R_ProcessingError);
    }
}

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 초기화 하기
***************************************************/
AdminList.Init = function () {
    AdminList.LoadData("");
    __addEventListener(document, "click", AdminList.click);

    $("#zeoModalContact").draggable({ disabled: false });
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-05-08
작성목적 : 이관 버튼 보이기 및 감추기
**********************************************************************************************/
AdminList.ShowHidFolder = function () {
    $(".dept").hide();
    $(".user").hide();

    var res = $("#ddlFolder").val();

    if (res == "Dept") {
        $(".dept").show();
    }

    if (res == "User") {
        $(".user").show();
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-05-08
작성목적 : 변경전 사용자 리스트 가져오기
**********************************************************************************************/
AdminList.SetUser = function (_val) {
    var val = unescape(_val).split("");    // userID + "" + userName + "" + deptID + "" + types;
    if (val[3].indexOf('M') < 0) {
        alert(R_SelectUser); return;
    }

    //변경전 경우,
    if (BeforeAfter == "before") {
        TargetUserID = val[0].replace(",", "");
        $("#sendInfo").html(val[1].replace(",", ""));

        AdminList.Search();
    }
    else if (BeforeAfter == "after") { //변경 후
        AfterUserID = val[0].replace(",", "");
        $("#receiveInfo").html(val[1].replace(",", ""));
    }
}

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 선택한 부서 리스트 가져오기
***************************************************/
AdminList.SetDept = function (_ids, _values) { //alert(unescape(_ids) + "," + unescape(_values));
    var deptID = unescape(_ids);        //  deptID
    var deptName = unescape(_values).split("|")[1];    // deptID | deptName | parentDeptID | parentDeptName

    //변경전 경우,
    if (BeforeAfter == "before") {
        TargetDeptID = deptID;
        $("#sendInfo").html(deptName);

        AdminList.Search();
    }
    else if (BeforeAfter == "after") { //변경 후
        AfterDeptID = deptID;
        $("#receiveInfo").html(deptName);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-05-08
작성목적 : 변경전 삭제부서 기술하기
**********************************************************************************************/
AdminList.SelDelDept = function (p_deptID, p_deptName) {
    p_deptID = unescape(p_deptID);
    p_deptName = unescape(p_deptName);

    TargetDeptID = p_deptID;
    $("#sendInfo").html(p_deptName);

    AdminList.Search();
}

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 페이지 카운트
***************************************************/
AdminList.ModSetting = function () {
    RecordPerPage = $("#ddlListCount").val();
    AdminList.Search();
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
AdminList.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        AdminList.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        AdminList.GetCount(); //검색 총 카운트 가져오기
        AdminList.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                AdminList.LoadData("search");
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
작성목적 : 전체 체크박스
***************************************************/
AdminList.Checked = function () {
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
AdminList.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
AdminList.Search = function () {
    CurrentPage = "1";
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);
    //AdminList.LoadData("search");
    AdminList.GoPageLoad();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
AdminList.GetPageList = function () {
    var url = "/DeskPlusEIP/Report/Ws/Content/AdminList.asmx/GetPageList";
    //alert(TotalItemCount);
    //파라미터값 배열에 담기

    ParamArray = {
        'targetUserID': TargetUserID,
        'targetDeptID': TargetDeptID,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'order': Order
    };

    var obj = eval("AdminList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
AdminList.SuccessPageList = function (data, status) {
    $('#placeholderAdminList').setTemplateURL('/DeskPlusEIP/Report/Jt/Content/JTAdminList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholderAdminList').processTemplate(data.d);
    $('#loading').html("");
    AdminList.Resource();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
AdminList.GetCount = function () {
    var url = "/DeskPlusEIP/Report/Ws/Content/AdminList.asmx/GetListCount";
    //auth 관리자페이지 : 1; 일반인페이지 : 0

    ParamArray = {
        'searchField': SearchField,
        'searchValue': SearchValue,
        'targetUserID': TargetUserID,
        'targetDeptID': TargetDeptID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AdminList.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
AdminList.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
AdminList.Resource = function () {
    $("#r_term")[0].innerHTML = R_Report;
    $("#r_write")[0].innerHTML = R_Writer;
    $("#r_confirm")[0].innerHTML = R_Confirm;
    $("#r_close")[0].innerHTML = R_CloseYN;
    $("#r_attach")[0].innerHTML = R_Attach;
    $("#r_short")[0].innerHTML = R_Short;
    //$("#r_linked")[0].innerHTML = R_Linked;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    if ($(".Write") != undefined) { $(".Write").text(R_Writer); }
    if ($(".Save") != undefined) { $(".Save").text(R_Save); }
    if ($(".Request") != undefined) { $(".Request").text(R_Request); }
    if ($(".ConfirmCheck") != undefined) { $(".ConfirmCheck").text(R_ConfirmCheck); }
    if ($(".IsClose") != undefined) { $(".IsClose").text(R_IsClose); }
    if ($(".IsOpen") != undefined) { $(".IsOpen").text(R_IsOpen); }
    if ($(".Inform") != undefined) { $(".Inform").text(R_Inform); }

    if ($(".moveuser") != undefined) { $(".moveuser").text(R_MoveUser); }
    if ($(".copyuser") != undefined) { $(".copyuser").text(R_CopyUser); }
    if ($(".movedept") != undefined) { $(".movedept").text(R_MoveDept); }
    if ($(".copydept") != undefined) { $(".copydept").text(R_CopyDept); }

    //if ($(".IconShort") != undefined) { $(".IconShort").attr('src', ImagePath + '/Icon/icon_comment.gif'); }
    if ($(".IconPopUp") != undefined) { $(".IconPopUp").attr('src', ImagePath + '/Icon/ico_popup_silver.gif'); }
    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 게시물 보기
***************************************************/
AdminList.View = function (p_projectID, p_menuID, p_boardID) {
    ParamArray = {
        'projectID': p_projectID,
        'menuID': p_menuID,
        'boardID': p_boardID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'recordPerPage': RecordPerPage
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
AdminList.ShowAttachContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var listID = el.getAttribute("ListID");

    AdminList.GetAttachFileListMenu(listID);

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
                oItem = oContextMenuCtrl.addItem(R_AllDownLoad, AdminList.TotalDownLoad, ImagePath + "/Icon/folders.gif");
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

            oItem = oContextMenuCtrl.addItem(attachFileName, AdminList.DownLoad, "EXT");
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
AdminList.DownLoad = function (_e) {
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
AdminList.TotalDownLoad = function (_e) {
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
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
AdminList.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": AdminList.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

AdminList.GetAttachFileListMenu = function (p_listID) {
    var url = "/DeskPlusEIP/Report/Ws/Content/AdminList.asmx/GetAttachFileListMenu";
    //var data = "{communityID :" + categoryID + ",listID:" + listID + "}";
    var data = "listID=" + p_listID;
    var obj = eval("AdminList.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
AdminList.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 업무보고 보기, 작성하기
**********************************************************************************************/
AdminList.View = function (p_ListID, p_StartDateYMD, p_EndDateYMD, p_Flag) {
    ParamArray = {
        'years': Years,
        'months': Months,
        'targetUserID': TargetUserID,
        'targetDeptID ': TargetDeptID,
        'weeks': Weeks,
        'flag': Flag,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'listType': ListType,
        'order': Order,
        'startDate': p_StartDateYMD,
        'endDate': p_EndDateYMD,
        'listID': p_ListID
    };

    CallEncrypt(CallGetParam())

    //미작성글 (0:미작성;1:저장;2:요청;3:완료)
    if (p_Flag == "0") {
        //작성페이지 이동
        location = "Register.aspx?p=" + GetEncryptValue;
    }
    else {
        location = "View.aspx?p=" + GetEncryptValue;
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 의견보기
**********************************************************************************************/
AdminList.ShortReplyList = function (p_listID) {
    ParamArray = {
        'listID': p_listID
    };

    CallEncrypt(CallGetParam())

//    var url = "ShortReplyList.aspx?p=" + GetEncryptValue;

//    PopupWin(600, 480, url, "", "", true);
    var url = "/DeskPlusEIP/Report/Content/ShortReplyList.aspx?p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, "", "ShortReplyList", "600", "690", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-22
작성목적 : 페이지이동 처리
**********************************************************************************************/
AdminList.GoPageLoad = function () {

    ParamArray = {
        'years': Years,
        'months': Months,
        'targetUserID': TargetUserID,
        'targetDeptID ': TargetDeptID,
        'weeks': Weeks,
        'flag': Flag,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'listType': ListType,
        'order': Order,
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
    };

    CallEncrypt(CallGetParam());
    location = "AdminList.aspx?p=" + GetEncryptValue;
}


