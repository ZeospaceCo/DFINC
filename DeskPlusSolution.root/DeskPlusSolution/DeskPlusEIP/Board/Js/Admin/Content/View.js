View = {}

$(function () {
    $(document).ready(function () {
        //추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            View.Add();
        });

        //답변
        $('#cbtnReply').click(function (e) {
            e.preventDefault();
            View.Reply();
        });

        //조회
        $('#cbtnRead').click(function (e) {
            e.preventDefault();
            View.ReadList();
        });

        //평가
        $('#cbtnRate').click(function (e) {
            e.preventDefault();
            View.Rate();
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

        //메일발송
        $('#cbtnMail').click(function (e) {
            e.preventDefault();

            ParamArray = {
                'categoryID': CategoryID,
                'listID': ListID
            };

            CallEncrypt(CallGetParam());

            //DeepMail UI팝업
            $("#ifrmMail")[0].src = "/DeskPlusEIP/Board/Content/SendMailDeep.aspx?p=" + GetEncryptValue;
            //var url = "/DeskPlusEIP/Board/Content/SendMail.aspx?p=" + GetEncryptValue;
            //PopupWin(800, 500, url, "-500", "", true);
        });

        // [PDF일괄다운로드]
        $('#cbtnPDF').click(function (e) {
            e.preventDefault();
            View.PDFDownLoad();
        });

        if (Option_Reple != "False") {
            View.GetShortReplyList();
        }

        View.GetAttachFileList();
        View.GetLinkURLList();

        View.Resource();
        $("#zeoModalContact").draggable({ disabled: false });

        //큰 이미지 줄이기
        $(".shrink-me").resize({ maxWidth: 800 });
    });
});

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 인쇄
***************************************************/
View.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
View.Resource = function () {
  

    if (Option_Reple != "False") {
        //$("#j_Name")[0].innerHTML = R_Name;
        //$("#j_Content")[0].innerHTML = R_Contents;
        //$("#j_RegDate")[0].innerHTML = R_RegDate;

        if ($(".AddMsg") != undefined) { $(".AddMsg").text(R_AddShortReply); }
        if ($(".ModMsg") != undefined) { $(".ModMsg").text(R_Modify); }
        if ($(".DelMsg") != undefined) { $(".DelMsg").text(R_Del); }
        if ($(".delReply") != undefined) { $(".delReply").text("[" + R_DelReply + "]"); }
    }

    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
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
    //팝업창일 경우,
    if (Popup == "1") {
        //window.open("about:blank", "_self").close();
        if (parent.disablePopup) { window.open("about:blank", "_self").close(); return; } else { parent.close(); }
        return;
    }

    ParamArray = {
        'menuCatID': MenuCatID,
        'catID': CatID,
        'categoryID': CategoryID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchGroup': SearchGroup,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = GoPage + "?p=" + GetEncryptValue;
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
        'menuCatID': MenuCatID,
        'catID': CatID,
        'categoryID': CategoryID,
        'listID': ListID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchGroup': SearchGroup,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "ModN.aspx?p=" + GetEncryptValue;

}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 추가
***************************************************/
View.Add = function () {
    ParamArray = {
        'menuCatID': MenuCatID,
        'catID': CatID,
        'categoryID': CategoryID
    };

    CallEncrypt(CallGetParam());
    location.href = "AddN.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 답변
***************************************************/
View.Reply = function () {
    ParamArray = {
        'menuCatID': MenuCatID,
        'catID': CatID,
        'categoryID': CategoryID,
        'listID': ListID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchGroup': SearchGroup,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "ReplyN.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 조회현황
***************************************************/
View.ReadList = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var url = "/DeskPlusEIP/Board/Content/ReadList.aspx?p=" + GetEncryptValue;

    PopupWin(500, 400, url, "", "", true);
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 댓글
***************************************************/
View.AddShortReply = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var url = "ShortReply.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "-220", false);
}

/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 평가
***************************************************/
View.Rate = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID
    };

    CallEncrypt(CallGetParam())
    var url = "/DeskPlusEIP/Board/Content/Rate.aspx?p=" + GetEncryptValue;

    PopupWin(600, 450, url, "", "", true);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 댓글  리스트 호출
**********************************************************************************************/
View.GetShortReplyList = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID,
        'optionShortReplyGood': OptionShortReplyGood,
        'optionShortReplyTop': OptionShortReplyTop
    };

    CallEncrypt(CallGetParam())

    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessShortReplyList");
    var url = "/DeskPlusEIP/Board/Ws/Admin/Content/View.asmx/GetShortReplyList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 댓글 리스트 성공시 페이지 표시
**********************************************************************************************/
View.SuccessShortReplyList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Board/Jt/Content/JTReple.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    // 댓글 추천/비추천 기능 사용 안할 경우
    if (OptionShortReplyGood == "0") {
        if ($(".goodtd") != undefined) { $(".goodtd").css("display", "none"); }
    }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
View.FileDownload = function (filePath, fileName, fileGuid) {
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
        'categoryID': CategoryID,
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessAttachFile");
    var url = "/DeskPlusEIP/Board/Ws/Admin/Content/View.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
View.SuccessAttachFile = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Board/Jt/Content/JTAttachFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    if (data.d.length == "0") {
        $("#solbatch").attr("title", "");
        $("#pdfbatch").css("display", "none");
    }

}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-22
작성목적 : 댓글 수정화면 보여주기
**********************************************************************************************/
View.ModShortReply = function (shortReplyID) {
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID,
        'shortReplyID': shortReplyID
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
View.DelReple = function (shortReplyID, groupID) {
    if (confirm(R_ReallyApplyDelete)) {
        ParamArray = {
            'categoryID': CategoryID,
            'listID': ListID,
            'ShortReplyID': shortReplyID,
            'groupID': groupID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessDelShortReply");
        var url = "/DeskPlusEIP/Board/Ws/Admin/Content/View.asmx/DelShortReply";
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
작성목적 : 이미지 보여주기
**********************************************************************************************/
View.ShowImage = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID
    };

    CallEncrypt(CallGetParam());

    //    var goPage = "/DeskPlusEIP/Board/Content/GallerySubList.aspx?p=" + GetEncryptValue;
    //    WindowObjectReference = window.open(goPage, "ShowImage1", "width=800px,height=600px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");

    var url = "/DeskPlusEIP/Board/Content/GallerySubList.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "ShowImage1", "800", "600")
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-08
작성목적 : PDF생성유무 체크
**********************************************************************************************/
View.PDFDownLoad = function () {

    if (ListID != "") {

        ParamArray = {
            'module': 'BOARD'
            , 'listID': ListID
        };
        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessPDFDownLoad");
        var url = "/DeskPlusEIP/Common/Ws/PDF/PDF.asmx/GetPDFCreateCheck";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-08
작성목적 : PDF다운로드
**********************************************************************************************/
View.SuccessPDFDownLoad = function (data, status) {
    var vStatus = data.d;

    if (vStatus == "Y") {
        //pdf파일이 존재할 경우
        //var iscreate = (confirm("새로 생성하시겠습니까?")) ? "1" : "0";
        ParamArray = {
            'module': 'BOARD'
            , 'listID': ListID
        };
        CallEncrypt(CallGetParam());
        $("#ifrm")[0].src = "/DeskPlusEIP/Common/PDF/PDFDownLoad.aspx?p=" + GetEncryptValue;

    }
    else {
        //pdf파일이 존재하지 않을경우 pdf파일 생성후 다운로드
        ParamArray = {
            'module': 'BOARD'
            , 'listID': ListID
            , 'iscreate': '1'
            , 'catID': CategoryID
        };
        CallEncrypt(CallGetParam());

        $("#ifrm")[0].src = "/DeskPlusEIP/Common/PDF/PDFCreate.aspx?p=" + GetEncryptValue;
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-03-20
작성목적 : 링크URL 리스트 호출
**********************************************************************************************/
View.GetLinkURLList = function () {

    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessLinkURLList");
    var url = "/DeskPlusEIP/Board/Ws/Content/Mod.asmx/GetLinkURLList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-03-20
작성목적 : 링크URL 리스트 보여주기
**********************************************************************************************/
View.SuccessLinkURLList = function (data, status) {

    if (data.d.length > 0) {

        for (var i = 0; i < data.d.length; i++) {

            $("#placeholder_URLfileList").append(
                  "<li>"
                + "<a href=\"#\" onclick=\"javascript:View.LinkURL('" + data.d[i].URL + "')\">" + data.d[i].URLName + " ( " + data.d[i].URL + " ) </a>"
                + "<span class='mount'>URL</span>"
                + "</li>"
             );
        }
    }
    else {
        $("#liLinkURLList").hide();
    }

    /*
    if (data.d.length > 0) {
        var row = $("#NoListData");
        row.remove();
    }

    for (var i = 0; i < data.d.length; i++) {
        $("#contentTable").append(
		    "<tr>"
			    + "<td>"
			    + "<a href=\"#\" onclick=\"javascript:View.LinkURL('" + data.d[i].URL + "')\">" + data.d[i].URLName + " ( " + data.d[i].URL + " ) </a>"
			    + "</td>"
                + "<td id='tdLinkURLName" + data.d[i].LinkURLID + "' style='Display:none'>"
			    + data.d[i].URLName
			    + "</td>"
                + "<td id='tdLinkURL" + data.d[i].LinkURLID + "' style='Display:none'>"
			    + data.d[i].URL
			    + "</td>"
			    + "<td class='center'>"
			    + "URL"
			    + "</td>"
			    + "<td class='center'>"
			    + "</td>"
			    + "<td class='center'><a href=\"#\" onclick=\"javascript:View.LinkURL('" + data.d[i].URL + "')\" style='display:inline-block;'><span class=\"btn_file_dw\" title=\"DownLoad\"></span></a></td>"
		    + "</tr>"
	        );
    }
    */
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-03-20
작성목적 : 링크URL 링크처리
**********************************************************************************************/
View.LinkURL = function (linkUrl) {

    if (linkUrl.indexOf("http://") == -1) {
        linkUrl = "http://" + linkUrl;
    }

    window.open(linkUrl);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-09-11
작성목적 : 댓글의 댓글 입력창
***************************************************/
View.GroupAdd = function (shortReplyID) {

    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID,
        'shortReplyID': shortReplyID
    };

    CallEncrypt(CallGetParam())

    var url = "ShortReply.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "-220", false);

    /*
    var replyChk = $("#ReplyCheck_" + shortReplyID)[0].innerHTML;

    if (replyChk == "") {
        var tmpVal = "<div class='layout_subbox' style='padding: 0px;'>";
        tmpVal += "<table style='padding: 15px 0px 0px 0px;'>";
        tmpVal += "<tr>";
        tmpVal += "<td style='padding: 0; border: 0px;'>";
        tmpVal += "<span style='font-size: 11px; font-family: \"NanumGothic\", \"Malgun Gothic\"'>" + R_ReplyAdd + "</span>&nbsp;(<span id='charsRemaining_" + shortReplyID + "' style='font-family: \"NanumGothic\", \"Malgun Gothic\"'></span>)";
        tmpVal += "</td>";
        tmpVal += "<td style='padding: 0 0 0 13px; border: 0px; vertical-align: bottom'>";
        //tmpVal += "<ul class='btn_small' style='margin-top: 0; margin-bottom: 0; width: 100%'><li style='width: 100%'><a onclick='View.GroupReplyAdd(" + shortReplyID + ")'><span>" + R_Save + "</span></a></li></ul>";
        tmpVal += "<a onclick='View.GroupReplyAdd(" + shortReplyID + ")'><img title='' src='/DeskPlusEIP/Design/C/Img/Board/save.png' /></a>";
        tmpVal += "</td>";
        tmpVal += "</tr>";
        tmpVal += "</table>";
        tmpVal += "<textarea class='textarea' id='textarea_" + shortReplyID + "' style='border: 1px solid rgb(210, 210, 210); border-image: none; width: 95%; height: 30px;' maxlength='200'></textarea>";
        tmpVal += "</div>";

        $("#AddReply_" + shortReplyID)[0].innerHTML = tmpVal;
        $("#ReplyCheck_" + shortReplyID)[0].innerHTML = "1";

        //입력값 체크하기
        if (navigator.appName == 'Microsoft Internet Explorer') {
            $('#charsRemaining_' + shortReplyID).text($('#textarea_' + shortReplyID).attr('maxlength'));
            $('#textarea_' + shortReplyID).bind('keyup', function () {
                $(this).css('border', '1px solid #d2d2d2');
                var $this = $(this);
                var len = $this.val().length;
                var maxlength = $this.attr('maxlength')

                $('#charsRemaining_' + shortReplyID).text(maxlength - len);
                if (maxlength && len > maxlength) {
                    $(this).css('border', '1px solid #f00');
                    alert(maxlength + " limited");
                    $this.val($this.val().slice(0, maxlength));
                    $('#charsRemaining_' + shortReplyID).text("0");
                    return;
                }
            });
        }
        else {
            var onEditCallback = function (remaining) {
                $('#charsRemaining_' + shortReplyID).text(remaining);

                if (remaining > 0) {
                    $(this).css('border', '1px solid #d2d2d2');
                }
            }

            var onLimitCallback = function () {
                $(this).css('border', '1px solid #f00');
            }

            $('#textarea_' + shortReplyID + '[maxlength]').limitMaxlength({
                onEdit: onEditCallback,
                onLimit: onLimitCallback
            });
        }
    }
    else {
        $("#AddReply_" + shortReplyID)[0].innerHTML = "";
        $("#ReplyCheck_" + shortReplyID)[0].innerHTML = "";
    }
    */
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-09-11
작성목적 : 댓글의 댓글 추가
***************************************************/
View.GroupReplyAdd = function (shortReplyID) {

    var validator = new Validator($("#form1"));
    validator.checkRequired("textarea_" + shortReplyID, R_Add_ReplyInputContent, true);
    if (!validator.validate()) return;

    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID,
        'contents': escape($("#textarea_" + shortReplyID).val()),
        'shortReplyID': shortReplyID
    };

    var url = "/DeskPlusEIP/Board/Ws/Content/ShortReply.asmx/AddShortReplyReplys";
    var obj = eval("View.SuccessGroupReplyAdd");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2018-09-11
작성목적 : 댓글 add 성공시 페이지 표시
**********************************************************************************************/
View.SuccessGroupReplyAdd = function (data, status) {
    View.GetShortReplyList(); View.Resource();
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-09-14
작성목적 : 댓글 추천/비추천 참여 여부
***************************************************/
View.GoodCount = function (_shortReplyID, _good, _userID) {

    if (UserID == _userID) {
        alert(R_MyComment); // 자신의 댓글은 추천/비추천할 수 없습니다.
        return;
    }

    $("#hdShortReplyID").val(_shortReplyID);
    $("#hdGood").val(_good);

    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID,
        'shortReplyID': _shortReplyID
    };

    var url = "/DeskPlusEIP/Board/Ws/Content/ShortReplyGood.asmx/GetGoodCount";
    var obj = eval("View.SuccessGoodCount");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2018-09-14
작성목적 : 댓글 추천/비추천 참여 여부 성공시 
**********************************************************************************************/
View.SuccessGoodCount = function (data, status) {
    if (data.d == 0) {
        if ($("#hdGood").val() == "1") {
            View.GoodReple($("#hdShortReplyID").val());
        }
        else if ($("#hdGood").val() == "0") {
            View.BadReple($("#hdShortReplyID").val());
        }
    }
    else {
        alert(R_AlreadyComment); // 이미 참여한 댓글입니다.
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-09-14
작성목적 : 댓글 추천/비추천
***************************************************/
View.GoodReple = function (shortReplyID) {

    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID,
        'shortReplyID': shortReplyID,
        'good': '1',
        'bad': '0'
    };

    var url = "/DeskPlusEIP/Board/Ws/Content/ShortReplyGood.asmx/AddShortReplyGood";
    var obj = eval("View.SuccessGoodReple");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2018-09-14
작성목적 : 댓글 추천/비추천 성공시 페이지 표시
**********************************************************************************************/
View.SuccessGoodReple = function (data, status) {
    alert(R_Recommend); // 추천하였습니다.
    View.GetShortReplyList(); View.Resource();
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-09-14
작성목적 : 댓글 추천/비추천
***************************************************/
View.BadReple = function (shortReplyID) {

    if (confirm(R_Decommendation)) { // 비추천을 하시겠습니까?
        ParamArray = {
            'categoryID': CategoryID,
            'listID': ListID,
            'shortReplyID': shortReplyID,
            'good': '0',
            'bad': '1'
        };

        var url = "/DeskPlusEIP/Board/Ws/Content/ShortReplyGood.asmx/AddShortReplyGood";
        var obj = eval("View.SuccessBadReple");
        CallEncrypt(CallGetParam());
        data = "{p:'" + GetEncryptValue + "'}";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2018-09-14
작성목적 : 댓글 추천/비추천 성공시 페이지 표시
**********************************************************************************************/
View.SuccessBadReple = function (data, status) {
    View.GetShortReplyList(); View.Resource();
}
