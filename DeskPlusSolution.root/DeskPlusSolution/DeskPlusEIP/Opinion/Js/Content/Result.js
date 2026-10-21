Result = {}

$(function () {
    $(document).ready(function () {

        //이벤트

        // [인쇄]
        $('#cbtnPrint').click(function (e) { e.preventDefault(); Result.PrintElem({ leaveOpen: false, printMode: 'popup' }); });

        // [게시]
        $('#cbtnBoard').click(function (e) { e.preventDefault(); Result.AddBoard(); });

        // [엑셀리포트]
        $('#cbtnExcel').click(function (e) { e.preventDefault(); Result.Excel(); });

        // [투표현황(관리자)]
        $('#cbtnVoteUser').click(function (e) { e.preventDefault(); Result.VoteUser(); });

        // [취소]
        $('#cbtnCancel').click(function (e) { e.preventDefault(); Result.Cancel(); });

        // [짧은답글추가]
        $('#cbtnReplyAdd').click(function (e) { e.preventDefault(); JtReplyAdd(0); });

        if (QuesID > 0) {
            setTimeout(function () {
                Result.GetQuesUnitList();   //질문내용리스트(설문결과)
                Result.GetAttachFileList(); //첨부파일리스트
                GetReplyList();             //짧은답글리스트
            }, 200);
        }

        //짧은답글 사용여부
        if ($("#cbIsReply").attr("checked")) {
            $("#divReply").show();
        } else {
            $("#divReply").hide();
        }

    });
});

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [인쇄]
***************************************************/
Result.PrintElem = function (options) {
    $('#prtArea').printElement(options);
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [복사본(설문생성)]
***************************************************/
Result.AddBoard = function () {
    alert("게시기능-추후개발");
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [엑셀리포트]
***************************************************/
Result.Excel = function () {
    alert("엑셀리포트");
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [투표현황(관리자)]
***************************************************/
Result.VoteUser = function () {
    ParamArray = { 'quesID': QuesID };
    CallEncrypt(CallGetParam());        //배열값을 파라미터형태로 반환

    var url = "/DeskPlusEIP/Opinion/Folder/VoteList.aspx?p=" + GetEncryptValue;
    PopupWin(500, 325, url, "", "", true);
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [목록/취소]
***************************************************/
Result.Cancel = function () {

    //팝업창일 경우,
    if (Popup == "1") { self.close(); return; }

    ParamArray = {
        'menuID': MenuID
        , 'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
        , 'quesID': QuesID
    };

    var bUrl = $.url(PageBeforeUrl);    //  bUrl.attr('path') / bUrl.attr('query')
    CallEncrypt(CallGetParam());
    location.href = bUrl.attr('path') + "?p=" + GetEncryptValue;
    //parent.location.href = bUrl.attr('path') + "?p=" + GetEncryptValue;
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 첨부파일리스트
***************************************************/
Result.GetAttachFileList = function () {
    ParamArray = { 'quesID': QuesID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Result.SuccessAttachFile");
    var url = "/DeskPlusEIP/Opinion/Ws/Content/Result.asmx/GetAttachList";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 첨부파일리스트 보여주기
***************************************************/
Result.SuccessAttachFile = function (data, status) {
    $('#phFileList').setTemplateURL('/DeskPlusEIP/Opinion/Jt/Content/JtAttachListView.htm?seq=' + Guids());
    $('#phFileList').processTemplate(data.d);
    setTimeout(function () { Result.Resource('ATTACH'); }, 100);
}

/**********************************************************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
FileDownload = function (filePath, fileName, fileGuid) {
    ParamArray = {
        'filePath': filePath + fileGuid,
        'fileName': fileName
    };
    CallEncrypt(CallGetParam());
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + GetEncryptValue;
    WindowObjectReference = window.open(attachFilePath, "FileDownload", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 짧은답글
***************************************************/
GetReplyList = function () {
    ParamArray = { 'quesID': QuesID };
    CallEncrypt(CallGetParam());        //배열값을 파라미터형태로 반환

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Result.ScsReplyList");
    var url = "/DeskPlusEIP/Opinion/Ws/Content/Result.asmx/GetReplyList";
    AjaxService(url, data, obj);
}
Result.ScsReplyList = function (data, status) {
    $('#phReply').setTemplateURL('/DeskPlusEIP/Opinion/Jt/Content/JtReplyList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phReply').processTemplate(data.d);

    setTimeout(function () { Result.Resource('REPLY'); }, 100);
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 짧은답글 추가/수정
***************************************************/
JtReplyAdd = function (_id) {
    ParamArray = { 'quesID': QuesID, 'replyID': _id };
    CallEncrypt(CallGetParam());        //배열값을 파라미터형태로 반환

    var url = "/DeskPlusEIP/Opinion/Folder/ShortReply.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "", "", true);
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 짧은답글 삭제
***************************************************/
JtReplyDel = function (_id) {
    if (confirm(R_Delete)) {
        ParamArray = { 'quesID': QuesID, 'replyID': _id };
        CallEncrypt(CallGetParam());        //배열값을 파라미터형태로 반환
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("GetReplyList");
        var url = "/DeskPlusEIP/Opinion/Ws/Content/Result.asmx/DelReply";
        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 설문결과
***************************************************/
Result.GetQuesUnitList = function () {
    var quesID = ($("#cbIsOpen").attr("checked")) ? QuesID : 0; //결과공개인 경우만, 내용공개
    if (IsAdmin == "1") {
        quesID = QuesID; //관리자인 경우 공개
    }
    ParamArray = { 'quesID': quesID };
    CallEncrypt(CallGetParam());        //배열값을 파라미터형태로 반환

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Result.ScsQUList");
    var url = "/DeskPlusEIP/Opinion/Ws/Content/Result.asmx/GetQuesUnitList";
    AjaxService(url, data, obj);
}
Result.ScsQUList = function (data, status) {
    $("#divUnitCont").setTemplateURL('/DeskPlusEIP/Opinion/Jt/Content/JTResult.htm?seq=' + Guids(), null, { filter_data: false });
    $("#divUnitCont").processTemplate(data.d);

    setTimeout(function () { Result.Resource('RESULT'); }, 100);
}
/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 질문별 상세보기
***************************************************/
JtDetailShow = function (_unitID, _type, _point) {

    $("#divUnit_" + _unitID).html("");  //초기화
    $("#divUnit_" + _unitID).show();
    $("#show_" + _unitID).hide(); $("#hide_" + _unitID).show();

    ParamArray = { 'quesID': QuesID, 'unitID': _unitID };
    var obj = eval("Result.ScsListType");
    var url = "/DeskPlusEIP/Opinion/Ws/Content/Result.asmx/GetQUResultList";

    switch (_type.toUpperCase()) {
        case "C":
            ParamArray = { 'quesID': QuesID, 'unitID': _unitID, 'point': _point };
            url = "/DeskPlusEIP/Opinion/Ws/Content/Result.asmx/GetResultListTypeC";
            obj = eval("Result.ScsListTypeC");
            break;
        case "D":
            obj = eval("Result.ScsListTypeD");
            break;
        case "E":
            obj = eval("Result.ScsListTypeE");
            url = "/DeskPlusEIP/Opinion/Ws/Content/Result.asmx/GetResultListTypeE";
            break;
    }
    CallEncrypt(CallGetParam());        //배열값을 파라미터형태로 반환
    var data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 질문별 결과닫기
***************************************************/
JtDetailHide = function (_unitID) {
    $("#divUnit_" + _unitID).hide(); $("#spTot_" + _unitID).text("");
    $("#show_" + _unitID).show(); $("#hide_" + _unitID).hide();
}

Result.ViewVoteList = function (_quesID, _unitID, _unitItemID) {
    //alert(_quesID + "," + _unitID + "," + _unitItemID);
    ParamArray = {
        'quesID': _quesID,
        'unitID': _unitID,
        'unitItemID': _unitItemID
    };
    CallEncrypt(CallGetParam());        //배열값을 파라미터형태로 반환

    var url = "/DeskPlusEIP/Opinion/Admin/Content/VoteList.aspx?p=" + GetEncryptValue;
    PopupWin(550, 325, url, "", "", true);
}

Result.ViewVoteListC = function (_quesID, _unitID, _point) {
    //alert(_quesID + "," + _unitID + "," + _unitItemID);
    ParamArray = {
        'quesID': _quesID,
        'unitID': _unitID,
        'unitItemCont': _point,
        'type': 'C'
    };
    CallEncrypt(CallGetParam());        //배열값을 파라미터형태로 반환

    var url = "/DeskPlusEIP/Opinion/Admin/Content/VoteList.aspx?p=" + GetEncryptValue;
    PopupWin(550, 325, url, "", "", true);
}

Result.ViewVoteListD = function (_quesID, _unitID, _unitItemID, _point) {
    //alert(_quesID + "," + _unitID + "," + _unitItemID);
    ParamArray = {
        'quesID': _quesID,
        'unitID': _unitID,
        'unitItemID': _unitItemID,
        'unitItemCont': _point,
        'type': 'D'
    };
    CallEncrypt(CallGetParam());        //배열값을 파라미터형태로 반환

    var url = "/DeskPlusEIP/Opinion/Admin/Content/VoteList.aspx?p=" + GetEncryptValue;
    PopupWin(550, 325, url, "", "", true);
}

Result.ViewVoteListE = function (_quesID, _unitID, _unitItemCont) {
    //alert(_quesID + "," + _unitID + "," + _unitItemID);
    ParamArray = {
        'quesID': _quesID,
        'unitID': _unitID,
        'unitItemCont': _unitItemCont,
        'type': 'E'
    };
    CallEncrypt(CallGetParam());        //배열값을 파라미터형태로 반환

    var url = "/DeskPlusEIP/Opinion/Admin/Content/VoteList.aspx?p=" + GetEncryptValue;
    PopupWin(550, 325, url, "", "", true);
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : Chart그리기(Type : A, B, C)
***************************************************/
Result.ScsListType = function (data, status) {
    $.each(data.d, function (i) {
        var tmpobj = $("#divUnit_" + data.d[i]["UnitID"]);
        var tit = data.d[i]["RowNum"] + ". " + data.d[i]["Title"];
        var val = Result.GetProb(data.d[i]["VoteCnt"], data.d[i]["VoteTotCnt"]);
        var txt = "";
        if ($('#cbIsSecret').is(':checked')) { //익명사용
            txt = val + "% " + "(" + R_Tit_Count + ":" + data.d[i]["VoteCnt"] + R_Tit_Person + ")";
        }
        else {
            if (IsAdmin == "1") { //관리자인 경우 공개
                txt = "<span style=\"cursor: pointer;\" onclick=\"Result.ViewVoteList('" + data.d[i]["QuesID"] + "', '" + data.d[i]["UnitID"] + "', '" + data.d[i]["UnitItemID"] + "')\">" + val + "% " + "(" + R_Tit_Count + ":" + data.d[i]["VoteCnt"] + R_Tit_Person + ")</span>";
            }
            else {
                if ($('#cbIsVoteUser').is(':checked') == false) {
                    txt = val + "% " + "(" + R_Tit_Count + ":" + data.d[i]["VoteCnt"] + R_Tit_Person + ")";
                }
                else {
                    txt = "<span style=\"cursor: pointer;\" onclick=\"Result.ViewVoteList('" + data.d[i]["QuesID"] + "', '" + data.d[i]["UnitID"] + "', '" + data.d[i]["UnitItemID"] + "')\">" + val + "% " + "(" + R_Tit_Count + ":" + data.d[i]["VoteCnt"] + R_Tit_Person + ")</span>";
                }
            }
        }
        var isOdd = ((parseInt(data.d[i]["RowNum"]) % 2) == 0) ? false : true;
        var imgUrl = data.d[i]["ItemImgUrl"];
        $("#spTot_" + data.d[i]["UnitID"]).text("(" + R_Tit_CountTot + " : " + data.d[i]["VoteTotCnt"] + R_Tit_Person + " )");   //총참여자수
        Result.SetChart(tmpobj, tit, val, txt, isOdd, imgUrl);
    });
}
/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : Chart그리기(Type : A, B, C)
***************************************************/
Result.ScsListTypeC = function (data, status) {
    $.each(data.d, function (i) {
        var tmpobj = $("#divUnit_" + data.d[i]["UnitID"]);
        var tit = data.d[i]["RowNum"] + ". " + data.d[i]["Title"];
        var val = Result.GetProb(data.d[i]["VoteCnt"], data.d[i]["VoteTotCnt"]);
        var txt = "";
        if ($('#cbIsSecret').is(':checked')) { //익명사용
            txt = val + "% " + "(" + R_Tit_Count + ":" + data.d[i]["VoteCnt"] + R_Tit_Person + ")";
        }
        else {
            if (IsAdmin == "1") { //관리자인 경우 공개
                txt = "<span style=\"cursor: pointer;\" onclick=\"Result.ViewVoteListC('" + data.d[i]["QuesID"] + "', '" + data.d[i]["UnitID"] + "', '" + data.d[i]["RowNum"] + "')\">" + val + "% " + "(" + R_Tit_Count + ":" + data.d[i]["VoteCnt"] + R_Tit_Person + ")</span>";
            }
            else {
                if ($('#cbIsVoteUser').is(':checked') == false) {
                    txt = val + "% " + "(" + R_Tit_Count + ":" + data.d[i]["VoteCnt"] + R_Tit_Person + ")";
                }
                else {
                    txt = "<span style=\"cursor: pointer;\" onclick=\"Result.ViewVoteListC('" + data.d[i]["QuesID"] + "', '" + data.d[i]["UnitID"] + "', '" + data.d[i]["RowNum"] + "')\">" + val + "% " + "(" + R_Tit_Count + ":" + data.d[i]["VoteCnt"] + R_Tit_Person + ")</span>";
                }
            }
        }
        var isOdd = ((parseInt(data.d[i]["RowNum"]) % 2) == 0) ? false : true;
        var imgUrl = data.d[i]["ItemImgUrl"];
        $("#spTot_" + data.d[i]["UnitID"]).text("(" + R_Tit_CountTot + " : " + data.d[i]["VoteTotCnt"] + R_Tit_Person + ")");   //총참여자수
        Result.SetChart(tmpobj, tit, val, txt, isOdd, imgUrl);
    });
}
/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : Chart그리기(Type : D)
***************************************************/
Result.ScsListTypeD = function (data, status) {
    $.each(data.d, function (i) {
        // 질문내용 바인딩
        var tmpobj = $("#divUnit_" + data.d[i]["UnitID"]);
        var tit = data.d[i]["RowNum"] + ". " + data.d[i]["Title"];
        var objchartH = Result.CreateObj("div", "chartH", "", "");      //<div class="chartH">
        objchartH.appendChild(Result.CreateObj("span", "tit", tit, "")); //<span class="tit">1.항목제목
        objchartH.appendChild(Result.CreateObj("div", "gap", "", ""));  //<div class="gap">
        objchartH.appendChild(Result.CreateObj("div", "", "", "divUnitItem_" + data.d[i]["UnitItemID"]));   //<div id="divUnitItem_">
        tmpobj.append(objchartH);
        $("#spTot_" + data.d[i]["UnitID"]).text("(" + R_Tit_CountTot + " : " + data.d[i]["VoteTotCnt"] + R_Tit_Person + " )");   //총참여자수

        Result.ListTypeDSub(data.d[i]["UnitID"], data.d[i]["Point"], data.d[i]["UnitItemID"]);
    });
}
Result.ListTypeDSub = function (_unitID, _point, _unitItemID) {
    //질문별, 가점정보 가져오기
    ParamArray = { 'quesID': QuesID, 'unitID': _unitID, 'point': _point, 'unititemID': _unitItemID };
    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Result.ScsListTypeDSub");
    var url = "/DeskPlusEIP/Opinion/Ws/Content/Result.asmx/GetResultListTypeD";
    AjaxService(url, data, obj);
}
Result.ScsListTypeDSub = function (data, status) {
    $.each(data.d, function (i) {
        var tmpobj = $("#divUnitItem_" + data.d[i]["UnitItemID"]);
        var tit = data.d[i]["RowNum"] + ". " + data.d[i]["Title"];
        var val = Result.GetProb(data.d[i]["VoteCnt"], data.d[i]["VoteTotCnt"]);
        var txt = "";
        if ($('#cbIsSecret').is(':checked')) { //익명사용
            txt = val + "% " + "(" + R_Tit_Count + ": " + data.d[i]["VoteCnt"] + R_Tit_Person + ")";
        }
        else {
            if (IsAdmin == "1") { //관리자인 경우 공개
                txt = "<span style=\"cursor: pointer;\" onclick=\"Result.ViewVoteListD('" + data.d[i]["QuesID"] + "', '" + data.d[i]["UnitID"] + "', '" + data.d[i]["UnitItemID"] + "', '" + data.d[i]["RowNum"] + "')\">" + val + "% " + "(" + R_Tit_Count + ": " + data.d[i]["VoteCnt"] + R_Tit_Person + ")</span>";
            }
            else {
                if ($('#cbIsVoteUser').is(':checked') == false) {
                    txt = val + "% " + "(" + R_Tit_Count + ": " + data.d[i]["VoteCnt"] + R_Tit_Person + ")";
                }
                else {
                    txt = "<span style=\"cursor: pointer;\" onclick=\"Result.ViewVoteListD('" + data.d[i]["QuesID"] + "', '" + data.d[i]["UnitID"] + "', '" + data.d[i]["UnitItemID"] + "', '" + data.d[i]["RowNum"] + "')\">" + val + "% " + "(" + R_Tit_Count + ": " + data.d[i]["VoteCnt"] + R_Tit_Person + ")</span>";
                }
            }
        }
        var isOdd = ((parseInt(data.d[i]["RowNum"]) % 2) == 0) ? false : true;
        var imgUrl = data.d[i]["ItemImgUrl"];
        Result.SetChart(tmpobj, tit, val, txt, isOdd, imgUrl);
    });
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : Chart그리기(Type : E)
***************************************************/
Result.ScsListTypeE = function (data, status) {
    $.each(data.d, function (i) {
        var tmpobj = $("#divUnit_" + data.d[i]["UnitID"]);
        var tit = data.d[i]["RowNum"] + ". " + data.d[i]["UnitItemCont"];
        var txt = "";
        if ($('#cbIsSecret').is(':checked')) { //익명사용
            txt = "(" + R_Tit_Count + ": 1" + R_Tit_Person + ")";
        }
        else {
            if (IsAdmin == "1") { //관리자인 경우 공개
                txt = "<span style=\"cursor: pointer;\" onclick=\"Result.ViewVoteListE('" + data.d[i]["QuesID"] + "', '" + data.d[i]["UnitID"] + "', '" + data.d[i]["UnitItemCont"] + "')\">" + "(" + R_Tit_Count + ": 1" + R_Tit_Person + ")</span>";
            }
            else {
                if ($('#cbIsVoteUser').is(':checked') == false) {
                    txt = "(" + R_Tit_Count + ": 1" + R_Tit_Person + ")";
                }
                else {
                    txt = "<span style=\"cursor: pointer;\" onclick=\"Result.ViewVoteListE('" + data.d[i]["QuesID"] + "', '" + data.d[i]["UnitID"] + "', '" + data.d[i]["UnitItemCont"] + "')\">" + "(" + R_Tit_Count + ": 1" + R_Tit_Person + ")</span>";
                }
            }
        }
        var obj = Result.CreateObj("div", "chartH", tit, "");

        tmpobj.append(obj);
        obj.appendChild(Result.CreateObj("span", "txt", txt, ""));
        tmpobj.append(Result.CreateObj("br", "", "", ""));

        $("#spTot_" + data.d[i]["UnitID"]).text("(" + R_Tit_CountTot + " : " + data.d[i]["VoteTotCnt"] + R_Tit_Person + ")");   //총참여자수
    });
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 확률정보(%처리)
***************************************************/
Result.GetProb = function (_cnt, _tot) {
    if (_tot == 0) return 0;
    _cnt = parseInt(_cnt); _tot = parseInt(_tot);
    return parseInt(parseFloat(_cnt / _tot) * 100);
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 객체생성
***************************************************/
Result.CreateObj = function (_tagName, _class, _text, _id) {
    if (_tagName == "") return null;

    var tmpObj = document.createElement(_tagName);
    if (_class != "") { tmpObj.setAttribute("class", _class); }
    if (_text != "") { tmpObj.innerHTML = _text; }
    if (_id != "") { tmpObj.id = _id; }
    //bywithsuki수정중.
    return tmpObj;
}


/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : chart그리기
***************************************************/
Result.SetChart = function (_tmpobj, _tit, _val, _txt, _isOdd, _imgUrl) {
    if (_val == "") _val = 0; if (_tmpobj == null) return;

    var cssline = (_isOdd) ? "line" : "linegreen";   //짝수, 홀수 라인스타일변경.(line/linered/linegreen)

    var objchartH = Result.CreateObj("div", "chartH", "", "");      //<div class="chartH">
    var objtit = Result.CreateObj("span", "tit", _tit, "");         //<span class="tit">1.항목제목
    if (_imgUrl != "") {

        //        var objimg = Result.CreateObj("span", "rst_img", R_Titmsg, "");
        //        objimg.setAttribute("onclick", "javascript:Result.ImgItem('" + _imgUrl + "')"); //이미지 다운로드
        //        objtit.appendChild(objimg);
        //이미지 미리보기 (modify by shimpark)
        var imgObj = Result.CreateObj("img", "", "", "", "", "");
        imgObj.setAttribute("src", _imgUrl);
        imgObj.setAttribute("title", "");   //R_TitBig
        imgObj.setAttribute("style", "max-width:500px;");
        objtit.appendChild(imgObj);

    }
    var objgrp = Result.CreateObj("div", "grp", "", "");            //<div class="grp">
    var objline = Result.CreateObj("div", cssline, "", "");         //<div class="line"></div> 
    objline.setAttribute("style", "width:" + _val + "%");       //<table class="bg"><tr><td></td><td></td><td></td><td></td></tr></table>
    var objbgtab = Result.CreateObj("table", "bg", "", "");
    var objbgtr = Result.CreateObj("tr", "", "", "");
    for (var i = 0; i < 4; i++) { objbgtr.appendChild(Result.CreateObj("td", "", "", "")); }
    objbgtab.appendChild(objbgtr);
    objgrp.appendChild(objline); objgrp.appendChild(objbgtab);
    var objtxt = Result.CreateObj("span", "txt", _txt, "");         //<span class="txt">3 % (참여자수:2 명)</span>

    objchartH.appendChild(objtit); objchartH.appendChild(objgrp); objchartH.appendChild(objtxt);
    _tmpobj.append(objchartH);
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 설문결과 목록별 이미지 다운로드.
***************************************************/
Result.ImgItem = function (_path) {
    ParamArray = {
        'filePath': _path,        //파일명포함전체경로.
        'fileName': _path
    };
    CallEncrypt(CallGetParam());
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + GetEncryptValue;
    WindowObjectReference = window.open(attachFilePath, "FileDownload", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");
}
/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 다국어처리
***************************************************/
Result.Resource = function (sec) {

    switch (sec.toUpperCase()) {
        //        case "ATTACH":  //첨부파일리스트 
        //            $("#jtAttach_fileName").text(R_FileName); 
        //            $("#jtAttach_fileType").text(R_FileType); 
        //            $("#jtAttach_fileLen").text(R_FileLen); 
        //            $("#jtAttach_filedelete").text(R_FileDown); 
        //            $(".jtNoData").text(R_JTNoData); 
        //            break; 
        //        case "REPLY": 
        //            $("#jtRL_F1").text(R_JTF13); 
        //            $("#jtRL_F2").text(R_JTF14); 
        //            $("#jtRL_F3").text(R_JTF15); 
        //            $("#jtRL_F4").text(R_JTF16); 
        //            $(".jtRL_F5").text(R_JTF20); 
        //            $(".jtRL_F6").text(R_JTF21); 
        //            $(".jtNoData").text(R_JTNoData); 
        //            break; 
        case "RESULT":
            $("#jtR_F1").text(R_JTF17);
            $(".jtR_F2").text(R_JTF18);
            $(".jtR_F3").text(R_JTF19);
            break;

    }
    if ($(".ModMsg") != undefined) { $(".ModMsg").text(R_Modify); }
    if ($(".DelMsg") != undefined) { $(".DelMsg").text(R_Del); }
}