Home = {}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적 : 초기화
**********************************************************************************************/

$(function () {

    $(document).ready(function () {

        //검색입력필드 엔터 이벤트 기술
        $("#cbtnSearch").click(function (e) { e.preventDefault(); Home.Search(); });
        //검색입력필드 엔터 이벤트 기술
        $("#searchValue").keydown(function (e) { if (e.keyCode == 13) { e.preventDefault(); Home.Search(); } });

        // 공용폴더-최신 업데이트 이전목록
        $('#PublicFileR').click(function (e) {
            e.preventDefault();
            PublicPageNo = parseInt(PublicPageNo) + 1;
            Home.GetGoPublicList();
        });

        // 공용폴더-최신 업데이트 다음목록
        $('#PublicFileL').click(function (e) {
            e.preventDefault();
            PublicPageNo = parseInt(PublicPageNo) - 1;
            Home.GetGoPublicList();
        });

        // 전달받은파일-최신 업데이트 이전목록
        $('#SendFileR').click(function (e) {
            e.preventDefault();
            SendPageNo = parseInt(SendPageNo) + 1;
            Home.GetGoSendList();
        });

        // 전달받은파일-최신 업데이트 다음목록
        $('#SendFileL').click(function (e) {
            e.preventDefault();
            SendPageNo = parseInt(SendPageNo) - 1;
            Home.GetGoSendList();
        });

        // 공유받은폴더-최신 업데이트 이전목록
        $('#ShareFolderR').click(function (e) {
            e.preventDefault();
            SharePageNo = parseInt(SharePageNo) + 1;
            Home.GetGoShareList();
        });

        // 공유받은폴더-최신 업데이트 다음목록
        $('#ShareFolderL').click(function (e) {
            e.preventDefault();
            SharePageNo = parseInt(SharePageNo) - 1;
            Home.GetGoShareList();
        });       

        //검색어입력
        $("#searchValue").attr("placeholder", R_PlaceholderMenu);

        __addEventListener(document, "click", Home.click);

        Home.Init();

    });
});

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적 : 초기화 하기
**********************************************************************************************/
Home.Init = function () {
    Home.GetPublicList();
    Home.GetSendList();
    Home.GetShareList();
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적 : 공용폴더 (웹서비스 호출)
**********************************************************************************************/
Home.GetPublicList = function () {

    var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetNewListPublic";
    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 6,
        'pageSizePageNo': PublicPageNo
    };

    var obj = eval("Home.SuccessPublicList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적 : 공용폴더 (웹서비스 값 받기)
**********************************************************************************************/
Home.SuccessPublicList = function (data, status) {
    $('#phPublicList').setTemplateURL('/DeskPlusEIP/WebHard/Jt/Content/JTListNewPublic.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phPublicList').processTemplate(data.d);
    setTimeout("Home.PublicResource();", 200);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Home.PublicResource = function () {

    if ($("#r_fileName")[0] == undefined) {
        setTimeout("Home.PublicResource();", 200);
    }
    else {
        $("#r_folders")[0].innerHTML = R_Folders;
        $("#r_fileName")[0].innerHTML = R_FileName;
        $("#r_download")[0].innerHTML = R_Download;
        $("#r_fileLength")[0].innerHTML = R_FileLength;
        $("#r_fileType")[0].innerHTML = R_FileType;
        $("#r_regUser")[0].innerHTML = R_RegUser;
        $("#r_writeDate")[0].innerHTML = R_RegDate;
        
        if ($("#NoListData")[0] != undefined) {
            $("#NoListData")[0].innerHTML = R_NoContent;
        }
    }

    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적 : 전달받은 파일 (웹서비스 호출)
**********************************************************************************************/
Home.GetSendList = function () {

    var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetNewListSend";
    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 6,
        'pageSizePageNo': SendPageNo
    };

    var obj = eval("Home.SuccessSendList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적 : 전달받은 파일 (웹서비스 값 받기)
**********************************************************************************************/
Home.SuccessSendList = function (data, status) {
    $('#phSendFile').setTemplateURL('/DeskPlusEIP/WebHard/Jt/Content/JTListNewSend.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phSendFile').processTemplate(data.d);
    setTimeout("Home.SendResource();", 200);
}
/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Home.SendResource = function () {

    if ($("#r_sfileName")[0] == undefined) {
        setTimeout("Home.SendResource();", 200);
    }
    else {
        $("#r_ssendUser")[0].innerHTML = R_SendUser;
        $("#r_sfileName")[0].innerHTML = R_FileName;
        $("#r_sdownload")[0].innerHTML = R_Download;
        $("#r_sfileLength")[0].innerHTML = R_FileLength;
        $("#r_sfileType")[0].innerHTML = R_FileType;
        $("#r_sregUser")[0].innerHTML = R_RegUser;
        $("#r_swriteDate")[0].innerHTML = R_RegDate;        
        
        if ($("#sNoListData")[0] != undefined) {
            $("#sNoListData")[0].innerHTML = R_NoContent;
        }
    }

    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적 : 공유받은폴더 (웹서비스 호출)
**********************************************************************************************/
Home.GetShareList = function () {

    var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetNewListShare";
    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 6,
        'pageSizePageNo': SharePageNo
    };

    var obj = eval("Home.SuccessShareList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적 : 공유받은폴더 (웹서비스 값 받기)
**********************************************************************************************/
Home.SuccessShareList = function (data, status) {
    $('#phShareFolder').setTemplateURL('/DeskPlusEIP/WebHard/Jt/Content/JTListNewShare.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phShareFolder').processTemplate(data.d);
    setTimeout("Home.ShareResource();", 200);
}
/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Home.ShareResource = function () {

    if ($("#r_ssfileName")[0] == undefined) {
        setTimeout("Home.ShareResource();", 200);
    }
    else {
        $("#r_ssfolders")[0].innerHTML = R_ShareUser + "(" + R_Folders + ")";
        $("#r_ssfileName")[0].innerHTML = R_FileName;
        $("#r_ssdownload")[0].innerHTML = R_Download;
        $("#r_ssfileLength")[0].innerHTML = R_FileLength;
        $("#r_ssfileType")[0].innerHTML = R_FileType;
        $("#r_ssshareUser")[0].innerHTML = R_RegUser;
        $("#r_sswriteDate")[0].innerHTML = R_RegDate;

        if ($("#ssNoListData")[0] != undefined) {
            $("#ssNoListData")[0].innerHTML = R_NoContent;
        }
    }

    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
}

/******************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적: 검색
****************************************/
Home.Search = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("searchValue", R_LengthOver, true);
    if (!validator.validate()) return;

    var searchVal = $("#searchValue").val();

    ParamArray = {
        'searchValue': searchVal
    };
    CallEncrypt(CallGetParam());

    location = "Search.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적 : 페이지이동 처리
**********************************************************************************************/
Home.GoPageLoad = function () {

    ParamArray = {
        'publicPageNo': PublicPageNo,
        'sendPageNo': SendPageNo,
        'sharePageNo': SharePageNo
    };

    CallEncrypt(CallGetParam());
    location = "Home.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적 : 공용폴더 (웹서비스 호출)
**********************************************************************************************/
Home.GetGoPublicList = function () {

    if (PublicPageNo < 0) {
        alert(R_FirstList);
        PublicPageNo = 0;
        return;
    }

    var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetNewListPublic";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 6,
        'pageSizePageNo': PublicPageNo
    };

    var obj = eval("Home.SuccessGoPublicList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적 : 공용폴더 (웹서비스 값 받기)
**********************************************************************************************/
Home.SuccessGoPublicList = function (data, status) {

    if (data.d.length > 0) {
        Home.GoPageLoad();
    }
    else {
        if (PublicPageNo > 0) {
            alert(R_LastList);
            PublicPageNo = parseInt(PublicPageNo) - 1;
        }
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적 : 전달받은 파일 (웹서비스 호출)
**********************************************************************************************/
Home.GetGoSendList = function () {

    if (SendPageNo < 0) {
        alert(R_FirstList);
        SendPageNo = 0;
        return;
    }

    var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetNewListSend";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 6,
        'pageSizePageNo': SendPageNo
    };

    var obj = eval("Home.SuccessGoSendList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적 : 전달받은 파일 (웹서비스 값 받기)
**********************************************************************************************/
Home.SuccessGoSendList = function (data, status) {

    if (data.d.length > 0) {
        Home.GoPageLoad();
    }
    else {
        if (SendPageNo > 0) {
            alert(R_LastList);
            SendPageNo = parseInt(SendPageNo) - 1;
        }
    }
}
/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적 : 공유받은폴더 (웹서비스 호출)
**********************************************************************************************/
Home.GetGoShareList = function () {

    if (SharePageNo < 0) {
        alert(R_FirstList);
        SharePageNo = 0;
        return;
    }

    var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetNewListShare";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 6,
        'pageSizePageNo': SharePageNo
    };

    var obj = eval("Home.SuccessGoShareList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-05
작성목적 : 공유받은폴더 (웹서비스 값 받기)
**********************************************************************************************/
Home.SuccessGoShareList = function (data, status) {

    if (data.d.length > 0) {
        Home.GoPageLoad();
    }
    else {
        if (SharePageNo > 0) {
            alert(R_LastList);
            SharePageNo = parseInt(SharePageNo) - 1;
        }
    }
}
/**************************************************
작성자   : 이지원
작성일자 : 2017-07-06
작성목적 : 클릭 이벤트 분기
***************************************************/
Home.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": Home.DownLoad(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

/**************************************************
작성자   : 이지원
작성일자 : 2017-07-06
작성목적 : 첨부파일 열기
***************************************************/
Home.DownLoad = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var attachFilePath = el.getAttribute("FilePath");

    /*
    var attachFileName = $("#fileName_" + el.id).text().trimText();
    CallEncrypt("filePath=" + attachFilePath + "&fileName=" + attachFileName);
    attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + GetEncryptValue;
    */

    attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + attachFilePath;

    window.open(attachFilePath);
}

