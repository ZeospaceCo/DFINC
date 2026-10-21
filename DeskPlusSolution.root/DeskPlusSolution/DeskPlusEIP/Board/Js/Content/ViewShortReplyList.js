/// <reference path="../../../Html/Js/jQuery/jquery.js" />

View = {}

$(function () {
    $(document).ready(function () {

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

        if (Option_Reple != "False") {
            View.GetShortReplyList();
            View.Resource();
        }
        
        $("#zeoModalContact").draggable({ disabled: false });

    });
});

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
View.Resource = function () {

    //if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

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
작성목적 : 목록으로 가기
***************************************************/
View.List = function () {
    //window.open("about:blank", "_self").close();
    if (parent.disablePopup) { window.open("about:blank", "_self").close(); return; }
    else if (parent.parent.disablePopup) { parent.PopupClose(); return; }
    else { parent.close(); }
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
    var url = "/DeskPlusEIP/Board/Ws/Content/View.asmx/GetShortReplyList";

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

    if ($('#cbtnShortReply').length == 0) {
        if ($(".replyGroupAdd") != undefined) { $(".replyGroupAdd").css("display", "none"); }
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
        var url = "/DeskPlusEIP/Board/Ws/Content/View.asmx/DelShortReply";
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
        tmpVal += "<a onclick='View.GroupReplyAdd(" + shortReplyID + ")'><img style='vertical-align: bottom' title='' src='/DeskPlusEIP/Design/C/Img/Board/save.png' /></a>";
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
