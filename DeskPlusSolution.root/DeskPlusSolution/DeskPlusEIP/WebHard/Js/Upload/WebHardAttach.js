WebHardAttach = {}

var oContextMenuCtrl = null;

/**************************************************
작성자 : famo23
작성일자 : 2012-01-25
작성목적 : 초기화 
***************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            WebHardAttach.LoadData("");
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


        //파일 업로드
        $('#CbtnConfirm').click(function (e) {
            if (ModuleName != "WEBHARD") {
                e.preventDefault();
                WebHardAttach.Upload();
            }
            else {
                alert(R_NoWebHardUpload);
                return;
            }
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });


        //dp upload
        $('#DPUpload').click(function () {

            var iPageUrl = $.url(PageUrl);
            var iQuery = iPageUrl.attr('query');
            location = "/DeskPlusEIP/Upload/DPUpload/Main.aspx?" + iQuery;

        });

        //RIA upload
        $('#RIAUpload').click(function () {

            var iPageUrl = $.url(PageUrl);
            var iQuery = iPageUrl.attr('query');

            location = "/DeskPlusEIP/Upload/JSUpload/Main.aspx?" + iQuery;
        });

        WebHardAttach.LoadData("");

    });
});

/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}

/**************************************************
작성자 : famo23
작성일자 : 2012-01-26
작성목적 : 초기화
***************************************************/
WebHardAttach.LoadData = function (gubun) {

    WebHardAttach.Control = tvFolderTree;

    if (WebHardAttach.Control != null) {
        FolderID = WebHardAttach.Control.SelectedNode.ID;
    }

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadWebHardData = function () {
        WebHardAttach.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadWebHardSearchData = function () {
        WebHardAttach.GetCount(); //검색 총 카운트 가져오기
        WebHardAttach.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                WebHardAttach.LoadData("search");
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
        if (gubun == "search") {
            loadWebHardSearchData();
        }
        else {
            loadWebHardData();
        }
    }, 100);

}

/**************************************************
작성자 : famo23
작성일자 : 2011-10-17
작성목적 : onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {

    FolderID = node.ID;
    UID = node.GetProperty("UserID");
    CurrentPage = 1; //1페이지로 이동처리     

    WebHardAttach.LoadData("search");

}

/**************************************************
작성자   : famo23
작성일자 : 2012-01-26
작성목적 : 전체 체크박스
***************************************************/
WebHardAttach.Checked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-01-26
작성목적 : 선택된 값 가져오기
***************************************************/
WebHardAttach.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-26
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
WebHardAttach.GetPageList = function () {

    var url = "/DeskPlusEIP/WebHard/Ws/Upload/WebHardAttach.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'folderID': FolderID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'order': Order,
        'fileType': FileType
    };

    var obj = eval("WebHardAttach.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-26
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
WebHardAttach.SuccessPageList = function (data, status) {
    $('#phWebAttach').setTemplateURL('/DeskPlusEIP/WebHard/Jt/Upload/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phWebAttach').processTemplate(data.d);

    WebHardAttach.Resource();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-26
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
WebHardAttach.Resource = function () {
    $("#r_fileName")[0].innerHTML = R_FileName;
    $("#r_fileLength")[0].innerHTML = R_FileLength;
    //$("#r_fileType")[0].innerHTML = R_FileType;
    //$("#r_writeDate")[0].innerHTML = R_RegDate;

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
작성일자 : 2012-01-26
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
WebHardAttach.GetCount = function () {
    var url = "/DeskPlusEIP/WebHard/Ws/Upload/WebHardAttach.asmx/GetListCount";
    ParamArray = {
        'folderID': FolderID,
        'fileType': FileType
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("WebHardAttach.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-26
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
WebHardAttach.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-26
작성목적 : 정렬
**********************************************************************************************/
WebHardAttach.Sort = function (col, arg) {

    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;

    WebHardAttach.LoadData("search");
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-27
작성목적 : Spool 폴더에 업로드 하기 
**********************************************************************************************/
WebHardAttach.Upload = function () {

    var selValue = WebHardAttach.CheckedValue();
    var tmpValue = "";

    if (selValue != "") {

        for (var i = 0; i < selValue.length; i++) {
            var attFileName = $("#fileName_" + selValue[i]).text();
            tmpValue += $("#" + selValue[i]).text() + ":" + attFileName + "|";
        }

        ParamArray = {
            'folderID': FolderID,
            'tmpValue': tmpValue,
            'uID': UID,
            'guid': GUID,
            'callBackEvent': callBackEvent
        };

        CallEncrypt(CallGetParam());

        location.href = "Attach.aspx?p=" + GetEncryptValue;

    }
    else {
        alert(R_SelectAttach);
    }
}

