
/// <reference path="../../../Html/Js/jQuery/jquery.js" />

View = {}

$(function () {

    $(document).ready(function () {

        // 일지확인
        $('#cbtnComplate').click(function (e) {
            e.preventDefault();
            View.Complate("3");
        });

        // 일지확인취소
        $('#cbtnComplateCancel').click(function (e) {
            e.preventDefault();
            View.Complate("2");
        });

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            View.Del();
        });

        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            View.Mod();
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        //목록
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            var pathname = window.location.pathname;
            if (pathname.toUpperCase().indexOf("VIEWPOPUP.ASPX") != -1)
                self.close();
            else
                View.List();
        });

        //댓글 추가
        $('#cbtnShortReply').click(function (e) {
            e.preventDefault();
            View.AddShortReply();
        });

        //이미지 보기
        $('#cbtnShowImage').click(function (e) {
            e.preventDefault();
            View.ShowImage();
        });
        View.Init();
        View.GetShortReplyList();
        View.GetAttachFileList();
        View.Resource();
        View.ReLoadTree();

        $("#zeoModalContact").draggable({ disabled: false });

        //큰 이미지 줄이기
        $(".shrink-me").resize({ maxWidth: 800 });
    });
});

/**************************************************
좌측 메뉴 reload
***************************************************/
View.ReLoadTree = function () {

    ParamArray = {
        'targetPage': ListType + "," + ListType,
        'reLoad': '1'
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Report/Category/Tree.aspx?p=" + GetEncryptValue;
    if (parent.frmLeft != null) parent.frmLeft.location = url;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 인쇄
***************************************************/
View.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 일지확인, 취소 버튼 세팅(2:요청, 3: 확인)
***************************************************/
View.Init = function () {
    if (FlagCheck == "3") {
        $("#cbtnComplate").hide();
        $("#cbtnComplateCancel").show();
    }
    else {
        $("#cbtnComplate").show();
        $("#cbtnComplateCancel").hide();
    }

    $("input[name='rbkind']").each(function (i) {
        $(this).attr('disabled', 'disabled');
    });
}
/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
View.Resource = function () {
//    $("#j_fileName")[0].innerHTML = R_FileName;
//    $("#j_fileType")[0].innerHTML = R_FileType;
//    $("#j_fileLen")[0].innerHTML = R_FileLen;
//    $("#j_download")[0].innerHTML = R_FileDown;

//    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
//    /*
//    if (Option_Reple != "False") {
//    $("#j_Name")[0].innerHTML = R_Name;
//    $("#j_Content")[0].innerHTML = R_Contents;
//    $("#j_RegDate")[0].innerHTML = R_RegDate;
//    }*/
//    $("#j_Name").text(R_Name);
//    $("#j_Content").text(R_Contents);
//    $("#j_RegDate").text(R_RegDate);

//    if ($(".r_mod") != undefined) {
//        $(".r_mod").text(R_Modify);
//    }

//    if ($(".r_del") != undefined) {
//        $(".r_del").text(R_Del);
//    }

    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
    if ($(".ModMsg") != undefined) { $(".ModMsg").text(R_Modify); }
    if ($(".DelMsg") != undefined) { $(".DelMsg").text(R_Del); }

}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 첨부파일
***************************************************/
View.AttachFile = function () {

}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 목록으로 가기 
***************************************************/
View.List = function () {

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
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = ListType + ".aspx?p=" + GetEncryptValue;

}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 삭제
***************************************************/
View.Del = function () {
    if (window.confirm(R_ApplyDelete)) {
        __doPostBack("LBTNProcess", "DEL");
    }
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 수정
***************************************************/
View.Mod = function () {

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
        'listID': ListID
    };

    CallEncrypt(CallGetParam());

    location.href = "Mod.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 조회현황
***************************************************/
View.ReadList = function () {

    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var url = "/DeskPlusEIP/Report/Content/ReadList.aspx?p=" + GetEncryptValue;

    PopupWin(500, 400, url, "", "", true);
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 댓글
***************************************************/
View.AddShortReply = function () {

    ParamArray = {
        'listID': ListID,
        'gubun': 'View'
    };

    CallEncrypt(CallGetParam())

    var url = "ShortReply.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "-220", false);
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 댓글  리스트 호출
**********************************************************************************************/
View.GetShortReplyList = function () {
    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessShortReplyList");
    var url = "/DeskPlusEIP/Report/Ws/Content/View.asmx/GetShortReplyList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 댓글 리스트 성공시 페이지 표시
**********************************************************************************************/
View.SuccessShortReplyList = function (data, status) {

    $('#placeholder').setTemplateURL('/DeskPlusEIP/Report/Jt/Content/JTReple.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
FileDownload = function (filePath, fileName, fileGuid) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
    //WindowObjectReference = window.open(attachFilePath, "FileDownload", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 파일  리스트 호출
**********************************************************************************************/
View.GetAttachFileList = function () {

    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessAttachFile");
    var url = "/DeskPlusEIP/Report/Ws/Content/View.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
View.SuccessAttachFile = function (data, status) {

    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Report/Jt/Content/JTAttachFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-22
작성목적 : 댓글 수정화면 보여주기
**********************************************************************************************/
ModShortReply = function (shortReplyID, listID) {

    ParamArray = {
        'shortReplyID': shortReplyID,
        'listID': listID,
        'gubun': 'View'
    };

    CallEncrypt(CallGetParam())
    var url = "ShortReplyMod.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "", true);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-22
작성목적 : 댓글 삭제
**********************************************************************************************/
DelReple = function (shortReplyID) {

    if (confirm(R_ReallyApplyDelete)) {

        ParamArray = {
            'listID': ListID,
            'ShortReplyID': shortReplyID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessDelShortReply");
        var url = "/DeskPlusEIP/Report/Ws/Content/View.asmx/DelShortReply";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 댓글 삭제 성공
**********************************************************************************************/
View.SuccessDelShortReply = function (data, status) {
    if (data.d) {
        View.GetShortReplyList();
        View.Resource();
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 댓글 삭제 성공
**********************************************************************************************/
View.ShowImage = function () {
    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Report/Content/GallerySubList.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "ShowImage1", "800", "600")
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 일지확인, 취소
**********************************************************************************************/
View.Complate = function (_flag) {
    if (_flag == "3") {
        FlagCheck = "2";
        $("#cbtnComplate").hide();
        $("#cbtnComplateCancel").show();
    }
    else {
        FlagCheck = "3";
        $("#cbtnComplate").show();
        $("#cbtnComplateCancel").hide();
    }

    ParamArray = {
        'listID': ListID,
        'flag': _flag
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessModComplate");
    var url = "/DeskPlusEIP/Report/Ws/Content/View.asmx/FlagComplate";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 댓글 삭제 성공
**********************************************************************************************/
View.SuccessModComplate = function (data, status) {
    if (data.d) {
        if (FlagCheck == "3")
            alert(R_WorkConfirmCancel);
        else
            alert(R_WorkConfirmOK);
    }
}