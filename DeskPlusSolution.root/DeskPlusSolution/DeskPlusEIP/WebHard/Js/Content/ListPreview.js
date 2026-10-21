ListPreview = {}
ListPreview.TreeFrame = parent.frmLeft;

/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-10-19
작성목적 : 초기화 
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            ListPreview.LoadData("");
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

        /*
        // 검색입력 초기화       
        $("#startDate").datepick();
        $("#endDate").datepick();

        if (SearchDate != "") {
        var dateStr = SearchDate.split("~");

        $("#startDate").val(dateStr[0])
        $("#endDate").val(dateStr[1])
        }

        if (SearchValue != "") {
        $("#ctbSearch").val(SearchValue);
        }
        */


        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            ListPreview.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListPreview.Search();
            }
        });

        //관리자
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            ListPreview.Admin();
        });

        //검색
        $('#CbtnSearch').click(function (e) {
            e.preventDefault();
            ListPreview.Search();
        });

        //전달
        $('#CbtnSend').click(function (e) {
            e.preventDefault();
            ListPreview.Send();
        });

        //공유
        $('#CbtnShare').click(function (e) {
            e.preventDefault();
            ListPreview.Share();
        });

        //이동        
        $('#CbtnMove').click(function (e) {
            e.preventDefault();
            ListPreview.Move();
        });

        //복사
        $('#CbtnCopy').click(function (e) {
            e.preventDefault();
            ListPreview.Copy();
        });

        //삭제
        $('#CbtnDel').click(function (e) {
            e.preventDefault();
            ListPreview.Del();
        });

        //완전삭제
        $('#CbtnAllDel').click(function (e) {
            e.preventDefault();
            ListPreview.AllDel();
        });

        //다운로드
        $('#CbtnDownload').click(function (e) {
            e.preventDefault();
            ListPreview.FileDownload();
        });

        //업로드
        $('#CbtnUpload').click(function (e) {
            e.preventDefault();
            ListPreview.FileUpload();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            ListPreview.ModFolderUser("3");
            ListPreview.GoPageListType("3");
        });

        //즐겨찾기 추가
        $('#cbtnFavor').click(function (e) {
            e.preventDefault();
            ListPreview.AddFavorite();
        });

        //즐겨찾기이동
        $('#cbtnFavorMove').click(function (e) {
            e.preventDefault();
            ListPreview.ModFavoriteMove();
        });

        //즐겨찾기하위순서변경
        $('#cbtnOrder').click(function (e) {
            e.preventDefault();
            ListPreview.ModFavoriteOrder();
        });

        //리스트형 저장
        $('#listType1').click(function () {
            ListPreview.ModFolderUser("1");
            ListPreview.GoPageListType("1");
        });

        //Icon 형 저장
        $('#listType2').click(function () {
            ListPreview.ModFolderUser("2");
            ListPreview.GoPageListType("2");
        });

        //Preview 형 저장
        $('#listType3').click(function () {
            ListPreview.ModFolderUser("3");
            ListPreview.GoPageListType("3");
        });

        $("#startEndDate").hide();

        $(".clsDate").keydown(function (e) { return false; });

        ListPreview.LoadData("");

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        __addEventListener(document, "click", ListPreview.click);

        $("#zeoModalContact").draggable({ disabled: false });
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2011-11-04
작성목적 : 달력 보이기 
***************************************************/
ListPreview.RegDate = function () {
    /*
    if ($("#ddlSearchTerm").val() == "3") {
    $("#startEndDate").show();
    $("#startDate").val("");
    $("#endDate").val("");
    }
    else {
    $("#startEndDate").hide();
    $("#startDate").val("");
    $("#endDate").val("");
    }
    */

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
작성일자 : 2011-10-19
작성목적 : 검색
***************************************************/
ListPreview.Search = function () {
    var response = ListPreview.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리     

    ListPreview.LoadData("search");
}

/**************************************************
작성자   : famo23
작성일자 : 2011-10-20
작성목적 : 리스트 초기화
***************************************************/
ListPreview.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadWebHardData = function () {
        ListPreview.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadWebHardSearchData = function () {
        ListPreview.GetCount(); //검색 총 카운트 가져오기
        ListPreview.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                ListPreview.LoadData("search");
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
작성일자 : 2011-10-20
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListPreview.GetPageList = function () {
    var url = "/DeskPlusEIP/WebHard/Ws/Content/ListPreview.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'folderID': FolderID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'order': Order,
        'downCheck': DownCheck
    };

    var obj = eval("ListPreview.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-10-20
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListPreview.SuccessPageList = function (data, status) {
    if (Types != "J") {
        $('#phListPre').setTemplateURL('/DeskPlusEIP/WebHard/Jt/Content/JTListPreview.htm?seq=' + Guids(), null, { filter_data: false });
    }
    else {
        $('#phListPre').setTemplateURL('/DeskPlusEIP/WebHard/Jt/Content/JTListPreviewSend.htm?seq=' + Guids(), null, { filter_data: false });
    }
    $('#phListPre').processTemplate(data.d);
    $('#loading').html("");

    ListPreview.Resource();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-10-20
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ListPreview.Resource = function () {
    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-10-20
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListPreview.GetCount = function () {
    var url = "/DeskPlusEIP/WebHard/Ws/Content/ListPreview.asmx/GetListCount";
    ParamArray = {
        'folderID': FolderID,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPreview.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-10-20
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListPreview.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-10-20
작성목적 : 정렬
**********************************************************************************************/
ListPreview.Sort = function (col) {

    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;


    ListPreview.LoadData("search");
}


/**************************************************
작성자   : famo23
작성일자 : 2011-10-20
작성목적 : 클릭 이벤트 분기
***************************************************/
ListPreview.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": ListPreview.DownLoad(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

/**************************************************
작성자   : famo23
작성일자 : 2011-10-20
작성목적 : 첨부파일 열기
***************************************************/
ListPreview.DownLoad = function (e) {

    if (DownCheck == "Y") {
        var evt = new __event(e);
        var el = evt.target;

        var attachFilePath = el.getAttribute("FilePath");
        var attachFileName = $("#fileName_" + el.id).text().trimText();

        CallEncrypt("filePath=" + attachFilePath + "&fileName=" + attachFileName);
        attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + GetEncryptValue;

        window.open(attachFilePath);
    }
    else {
        alert(R_DownloadAuthority);
    }


}

/**************************************************
작성자   : famo23
작성일자 : 2011-10-20
작성목적 : 전체 체크박스
***************************************************/
ListPreview.Checked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-10-20
작성목적 : 선택된 값 가져오기
***************************************************/
ListPreview.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}


/**************************************************
작성자   : famo23
작성일자 : 2011-11-08
작성목적 : 전달
***************************************************/
ListPreview.Send = function () {

    var selValue = ListPreview.CheckedValue();
    var tmpValue = "";

    if (selValue != "") {

        for (var i = 0; i < selValue.length; i++) {
            tmpValue += selValue[i] + ",";
        }

        ParamArray = {
            'folderID': FolderID,
            'fileList': tmpValue,
            'order': Order
        };

        CallEncrypt(CallGetParam());

        var url = "../Send/GetTarget.aspx?p=" + GetEncryptValue;
        PopupWin(700, 485, url, "50", "", true);


    } else {
        alert(R_SelectSend);
    }

}


/**************************************************
작성자   : famo23
작성일자 : 2011-10-27
작성목적 : 공유
***************************************************/
ListPreview.Share = function () {

    ParamArray = {
        'folderID': FolderID
    };

    CallEncrypt(CallGetParam());

    var url = "../Share/GetShare.aspx?p=" + GetEncryptValue;
    PopupWin(700, 485, url, "50", "", true);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-11-08
작성목적 : 이동
***************************************************/
ListPreview.Move = function () {

    var selValue = ListPreview.CheckedValue();
    var tmpValue = "";

    if (selValue != "") {
        for (var i = 0; i < selValue.length; i++) {
            tmpValue += selValue[i] + ",";
        }

        ParamArray = {
            'tmpValue': tmpValue
        };

        CallEncrypt(CallGetParam());

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ListPreview.SuccessMove");
        var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetChkTrash";

        AjaxService(url, data, obj);

    } else {
        alert(R_SelectMove);
    }
}

ListPreview.SuccessMove = function (data, status) {

    var CheckCount = data.d;

    if (CheckCount == "0" || PAdmin == "1") {

        var selValue = ListPreview.CheckedValue();
        var tmpValue = "";

        for (var i = 0; i < selValue.length; i++) {
            tmpValue += selValue[i] + ",";
        }

        ParamArray = {
            'folderID': FolderID,
            'fileList': tmpValue,
            'gubun': 'MOVE',
            'listType': '3',
            'order': Order,
            'padmin': PAdmin,
            'types': Types
        };

        CallEncrypt(CallGetParam());

        var url = "";

        url = "../Folder/GetPublicFolder.aspx?p=" + GetEncryptValue;
        /*
        if (Types == "U" || Types == "L") {
        url = "../Folder/GetPublicFolder.aspx?p=" + GetEncryptValue;
        }
        else {
        url = "../Folder/GetFolder.aspx?p=" + GetEncryptValue;
        }
        */

        PopupWin(400, 400, url, "50", "", true);
    }
    else {
        alert(R_MoveAuthority);
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2011-11-08
작성목적 : 복사
***************************************************/
ListPreview.Copy = function () {

    var selValue = ListPreview.CheckedValue();
    var tmpValue = "";

    if (selValue != "") {

        for (var i = 0; i < selValue.length; i++) {
            tmpValue += selValue[i] + ",";
        }

        ParamArray = {
            'folderID': FolderID,
            'fileList': tmpValue,
            'gubun': 'COPY',
            'listType': '3',
            'order': Order,
            'padmin': PAdmin,
            'types': Types
        };

        CallEncrypt(CallGetParam());

        var url = "";

        url = "../Folder/GetPublicFolder.aspx?p=" + GetEncryptValue;
        /*
        if (Types == "U" || Types == "L") {
        url = "../Folder/GetPublicFolder.aspx?p=" + GetEncryptValue;
        }
        else {
        url = "../Folder/GetFolder.aspx?p=" + GetEncryptValue;
        }
        */

        PopupWin(400, 400, url, "50", "", true);

    } else {
        alert(R_SelectCopy);
    }
}

/**************************************************
작성자   : freeman
작성일자 : 2011-10-26
작성목적 : 즐겨찾기 수정
***************************************************/
ListPreview.ChangeFav = function (_listId, _arg) {

    if ($(_arg).attr("checked") == "checked") $(_arg).next().addClass("on");
    else $(_arg).next().removeClass("on");

}

/**************************************************
작성자   : famo23
작성일자 : 2011-10-28
작성목적 : 파일 업로드
***************************************************/
ListPreview.FileUpload = function () {

    var fileSizePerUser = "0";

    if (Types != "S") {
        if (PrivateQuota == "0") {
            fileSizePerUser = "0";
        }
        else {
            if (Types == "U" || Types == "L") {
                fileSizePerUser = ((PrivateQuota / 1024) * 1048576) - UseCount;
            }
            else {
                fileSizePerUser = (PrivateQuota * 1048576) - UseCount;
            }
        }

        if (fileSizePerUser < 0) {
            if (Types == "U" || Types == "L") {
                alert(R_PublicFolderCount);
            }
            else {
                alert(R_UploadCount);
            }
            return;
        }

        ParamArray = {
            'swfUploadGUID': SwfUploadGUID,
            'fileType': FileType,
            'fileTypeDesc': FileTypeDesc,
            'moduleName': 'WEBHARD',
            'callBackEvent': "ListPreview.SpoolUpload",
            'fileSizePerUser': fileSizePerUser,
            'fileLength': FileLength
        };

        CallEncrypt(CallGetParam());

        var url = GetAttachPathURL();
        var param = "p=" + GetEncryptValue;
        WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")

    }
    else {
        ListPreview.GetShareQuota();
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-04
작성목적 : 공유폴더의 주인의 폴더용량 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListPreview.GetShareQuota = function () {

    ParamArray = {
        'uID': UID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPreview.SuccessShareQuota");
    var url = "/DeskPlusEIP/WebHard/Ws/Content/ListPreview.asmx/GetShareQuota";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-04
작성목적 : 공유폴더의 주인의 폴더용량 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListPreview.SuccessShareQuota = function (data, status) {

    var tmpfileSizePerUser = data.d;

    if (tmpfileSizePerUser < 0) {
        alert(R_UploadCount);
        return;
    }

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'WEBHARD',
        'callBackEvent': "ListPreview.SpoolUpload",
        'fileSizePerUser': tmpfileSizePerUser
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-10-28
작성목적 : Spool 폴더에서 웹하드로 이동 하기 
**********************************************************************************************/
ListPreview.SpoolUpload = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'folderID': FolderID,
        'uID': UID,
        'types': Types
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPreview.SuccessUpload");
    var url = "/DeskPlusEIP/WebHard/Ws/Content/ListPreview.asmx/SetSpoolUpload";

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-11-01
작성목적 : 파일 업로드 (웹서비스 값 받기) -> 총 사용 용량 가져오기
**********************************************************************************************/
ListPreview.SuccessUpload = function (data, status) {

    // 공유받은 폴더에 업로드 할 시에는 트리의 사용량 표시를 바꿀 필요없다.
    if (Types == "U" || Types == "L" || Types == "B") {
        var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetPublicUseCount";

        ParamArray = {
            'folderID': FolderID,
            'types': Types
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ListPreview.SuccessLoadPublic");

        AjaxService(url, data, obj);
    }
    else if (Types != "S") {

        var obj = eval("ListPreview.SuccessLoad");
        var url = "/DeskPlusEIP/WebHard/Ws/Content/ListPreview.asmx/GetUseCount";

        AjaxService(url, "", obj);
    }
    else {
        alert(R_CompleteConfirm);

        // 페이지 리스트 호출 
        ListPreview.ListLoad();
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-06-22
작성목적 : 휴지통 삭제 파일체크
**********************************************************************************************/
ListPreview.Del = function () {

    var selValue = ListPreview.CheckedValue();
    var tmpValue = "";

    if (selValue != "") {
        for (var i = 0; i < selValue.length; i++) {
            tmpValue += selValue[i] + ",";
        }

        ParamArray = {
            'tmpValue': tmpValue
        };

        CallEncrypt(CallGetParam());

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ListPreview.SuccessCheckDel");
        var url = "/DeskPlusEIP/WebHard/Ws/Content/ListPreview.asmx/GetChkTrash";

        AjaxService(url, data, obj);

    } else {
        alert(R_SelectDel);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-11-01
작성목적 : 파일 삭제 (웹서비스 값 받기)
**********************************************************************************************/
ListPreview.SuccessCheckDel = function (data, status) {

    var CheckCount = data.d;

    if (CheckCount == "0" || PAdmin == "1") {
        var selValue = ListPreview.CheckedValue();
        var tmpValue = "";

        if (selValue != "") {
            if (confirm(R_DelTrashFile)) {

                for (var i = 0; i < selValue.length; i++) {
                    tmpValue += selValue[i] + ",";
                }

                ParamArray = {
                    'deleteYN': 'Y',
                    'tmpValue': tmpValue
                };

                CallEncrypt(CallGetParam());

                var data = "{p:'" + GetEncryptValue + "'}";
                var obj = eval("ListPreview.SuccessDel");
                var url = "/DeskPlusEIP/WebHard/Ws/Content/ListPreview.asmx/UpdTrash";

                AjaxService(url, data, obj);
            }

        }
    }
    else {
        alert(R_DeleteAuthority);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-11-01
작성목적 : 파일 삭제 (웹서비스 값 받기) -> 총 사용 용량 가져오기
**********************************************************************************************/
ListPreview.SuccessDel = function (data, status) {
    if (data.d) {

        if (Types == "U" || Types == "L" || Types == "B") {
            var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetPublicUseCount";

            ParamArray = {
                'folderID': FolderID,
                'types': Types
            };

            CallEncrypt(CallGetParam());
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("ListPreview.SuccessLoadPublic");

            AjaxService(url, data, obj);
        }
        else if (Types == "S") {
            ListPreview.LoadShare();
        }
        else {
            var obj = eval("ListPreview.SuccessLoad");
            var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetUseCount";

            AjaxService(url, "", obj);
        }
    }
    else {
        alert(R_ProcessingError);
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-12
작성목적 : 공유받은폴더 선택시
**********************************************************************************************/
ListPreview.LoadShare = function () {
    $("#userTotal").text("∞");
    $("#userQuota").text("∞");
    $("#graphBar").width("0%");
    $("#percent").text("0%");
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-11-01
작성목적 : 파일 업로드 및 삭제 한 후에 페이지 로드
**********************************************************************************************/
ListPreview.SuccessLoad = function (data, status) {

    alert(R_CompleteConfirm);

    // 트리의 그래픽 바 , 퍼센트 보여주기
    UseCount = data.d;
    var tmpPercentCount = "0%";

    if (UseCount == "0") {
        tmpPercentCount = "0";
    }
    else if (PrivateQuota == "0") {
        tmpPercentCount = "0";
    }
    else {
        tmpPercentCount = ((UseCount / (PrivateQuota * 1048576)) * 100);

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

    var tmpUseCount = UseCount;
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

    ListPreview.TreeFrame.$("#userTotal").text(tmpUseCount);
    ListPreview.TreeFrame.$("#percent").text(tmpPercentCount + "%");
    ListPreview.TreeFrame.$("#graphBar").width(tmpPercentCount + "%");

    // 페이지 리스트 호출 
    ListPreview.ListLoad();
}

/**************************************************
작성자   : famo23
작성일자 : 2011-11-04
작성목적 : 파일 다운로드
***************************************************/
ListPreview.FileDownload = function () {

    var selValue = ListPreview.CheckedValue();
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
작성일자 : 2011-12-12
작성목적 : 검색 필드 설정하기
***************************************************/
ListPreview.SettingSearch = function () {

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
작성일자 : 2011-12-20
작성목적 : 리스트 삭제 및 추가에 따른 리스트 재로딩
***************************************************/
ListPreview.ListLoad = function () {
    document.all("ddlSearchTerm").selectedIndex = 0;
    document.all("ddlSearchField").selectedIndex = 0;

    $("#ctbSearch").val("");
    $("#startDate").val("");
    $("#endDate").val("");
    $("#startEndDate").hide(); //기간 이외

    CurrentPage = 1; //1페이지로 이동처리     

    ListPreview.LoadData("search");
}

/**************************************************
작성자   : famo23
작성일자 : 2011-12-21
작성목적 : 폴더즐겨찾기 추가
***************************************************/
ListPreview.AddFavorite = function () {

    ParamArray = {
        'folderID': FolderID,
        'types': Types
    };

    CallEncrypt(CallGetParam());
    var url = "FavoriteCopy.aspx?p=" + GetEncryptValue;

    PopupWin(400, 400, url, "", "", true);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-12-26
작성목적 : 즐겨찾기 이동 변경하기
***************************************************/
ListPreview.ModFavoriteMove = function () {

    ParamArray = {
        'favoriteID': FavoriteID,
        'parentFavoriteID': ParentFavoriteID,
        'childCnt': ChildCnt
    };

    CallEncrypt(CallGetParam());
    var url = "FavoriteMove.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "", "", true);

}

/**************************************************
작성자   : famo23
작성일자 : 2011-12-26
작성목적 : 즐겨찾기 순서 변경하기
***************************************************/
ListPreview.ModFavoriteOrder = function () {

    ParamArray = {
        'favoriteID': FavoriteID
    };

    CallEncrypt(CallGetParam());
    var url = "FavoriteOrder.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "", "", true);

}



/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-29
작성목적 : 중요표시 추가 및 삭제하기 (웹서비스 호출)
**********************************************************************************************/
ListPreview.ModImpUser = function (_fileID, _arg) {

    if (_fileID == "") return;

    //카테고리별 사용자 정보 저장하기
    var url = "/DeskPlusEIP/WebHard/Ws/Content/ListPreview.asmx/AddImpUser";

    if ($(_arg).attr("checked") == "checked") {
        $("#" + _fileID).attr("checked", true);
        $(_arg).next().addClass("on");
    }
    else {
        $("#" + _fileID).attr("checked", false);
        $(_arg).next().removeClass("on");
        url = "/DeskPlusEIP/WebHard/Ws/Content/ListPreview.asmx/DelImpUser";
    }

    // 개인폴더나 공유한 폴더나 똑같은 폴더라고 보이기 때문에 같은 처리를 위하여 Types 같게 처리
    var tmpTypes = Types;
    if (tmpTypes == "M") {
        tmpTypes = "P";
    }

    ParamArray = {
        'fileID': _fileID.split("_")[1],
        'folderID': FolderID,
        'types': tmpTypes
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPreview.SuccessModImpUser");

    AjaxService(url, data, obj);

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-29
작성목적 : 성공시, 중요표시 삭제 및 추가하기 (웹서비스 값 받기)
**********************************************************************************************/
ListPreview.SuccessModImpUser = function (data, status) {
    if (data.d == true) {
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-05
작성목적 : 리스트 타입선택시 이동하기
**********************************************************************************************/
ListPreview.GoPageListType = function (gubun) {

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

    var response = ListPreview.SettingSearch();
    if (response == false) return;


    ParamArray = {
        'folderID': FolderID,
        'parentFolderID': ParentFolderID,
        'currentPage': 1,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'types': Types,
        'favoriteID': FavoriteID,
        'parentFavoriteID': ParentFavoriteID,
        'childCnt': ChildCnt,
        'pGubun': PGubun,
        'padmin': PAdmin
    };

    CallEncrypt(CallGetParam());

    //이동하기    
    location = iPath + "?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2012-01-09
작성목적 : 파일명 클릭시에 첨부파일 열기
***************************************************/
ListPreview.TitleDownLoad = function (_fileID, _filePath) {

    if (DownCheck == "Y") {
        var attachFileName = $("#fileName_" + _fileID).text().trimText();

        CallEncrypt("filePath=" + _filePath + "&fileName=" + attachFileName);
        attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + GetEncryptValue;

        window.open(attachFilePath);
    }
    else {
        alert(R_DownloadAuthority);
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-11
작성목적 : 폴더별 사용자 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
ListPreview.ModFolderUser = function (gubun) {

    // 개인폴더나 공유한 폴더나 똑같은 폴더라고 보이기 때문에 같은 처리를 위하여 Types 같게 처리
    var tmpTypes = Types;
    if (tmpTypes == "M") {
        tmpTypes = "P";
    }

    var url = "/DeskPlusEIP/WebHard/Ws/Content/ListPreview.asmx/ModFolderUser";
    ParamArray = {
        'folderID': FolderID,
        'listType': gubun,
        'pageSize': $("#ddlListCount").val(),
        'types': tmpTypes
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPreview.SuccessModFolderUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-11
작성목적 : 폴더별 사용자 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
ListPreview.SuccessModFolderUser = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-06-22
작성목적 : 파일 완전 체크
**********************************************************************************************/
ListPreview.AllDel = function () {

    var selValue = ListPreview.CheckedValue();
    var tmpValue = "";

    if (selValue != "") {
        for (var i = 0; i < selValue.length; i++) {
            tmpValue += selValue[i] + ",";
        }

        ParamArray = {
            'tmpValue': tmpValue
        };

        CallEncrypt(CallGetParam());

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ListPreview.SuccessCheckAllDel");
        var url = "/DeskPlusEIP/WebHard/Ws/Content/ListPreview.asmx/GetChkTrash";

        AjaxService(url, data, obj);

    } else {
        alert(R_SelectDel);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-04-09
작성목적 : 파일 완전 삭제
**********************************************************************************************/
ListPreview.SuccessCheckAllDel = function (data, status) {

    var CheckCount = data.d;

    if (CheckCount == "0" || PAdmin == "1") {
        var selValue = ListPreview.CheckedValue();
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
                var obj = eval("ListPreview.SuccessAllDel");
                var url = "/DeskPlusEIP/WebHard/Ws/Content/ListPreview.asmx/DelFile";

                AjaxService(url, data, obj);
            }

        }
    }
    else {
        alert(R_DeleteAuthority);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-04-09
작성목적 : 파일 삭제 (웹서비스 값 받기) -> 총 사용 용량 가져오기
**********************************************************************************************/
ListPreview.SuccessAllDel = function (data, status) {
    if (data.d) {
        if (Types == "U" || Types == "L" || Types == "B") {
            var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetPublicUseCount";

            ParamArray = {
                'folderID': FolderID,
                'types': Types
            };

            CallEncrypt(CallGetParam());
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("ListPreview.SuccessLoadPublic");

            AjaxService(url, data, obj);
        }
        else if (Types == "S") {
            ListPreview.LoadShare();
        }
        else {
            var obj = eval("ListPreview.SuccessLoad");
            var url = "/DeskPlusEIP/WebHard/Ws/Content/ListPreview.asmx/GetUseCount";

            AjaxService(url, "", obj);
        }
    }
    else {
        alert(R_ProcessingError);
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-01
작성목적 : 폴더용량 확인
**********************************************************************************************/
ListPreview.SuccessLoadPublic = function (data, status) {
    var folderData = data.d;
    var PUseCount = 0;
    var PPrivateQuota = 0;
    var PDisPlayQuota = 0;

    if (Types == "B") {
        PUseCount = folderData.split("|")[1];
        PPrivateQuota = 0;

        UseCount = folderData.split("|")[1];
        PrivateQuota = 0;
    }
    else {
        PUseCount = folderData.split("|")[1];
        PPrivateQuota = folderData.split("|")[0];

        UseCount = folderData.split("|")[1];
        PrivateQuota = folderData.split("|")[0];
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

    ListPreview.TreeFrame.$("#userTotal").text(tmpUseCount);
    ListPreview.TreeFrame.$("#userQuota").text(PDisPlayQuota);
    ListPreview.TreeFrame.$("#graphBar").width(tmpPercentCount + "%");
    ListPreview.TreeFrame.$("#percent").text(tmpPercentCount + "%");

    // 페이지 리스트 호출 
    ListPreview.ListLoad();
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-03
작성목적 : 관리자 모드
**********************************************************************************************/
ListPreview.Admin = function () {

    ParamArray = {
        'accActive': 0
    };

    CallEncrypt(CallGetParam());

    var goUrl = "/DeskPlusEIP/WebHard/Folder/TreeAdmin.aspx?p=" + GetEncryptValue;
    if (ListPreview.TreeFrame) ListPreview.TreeFrame.location.href = goUrl;
}