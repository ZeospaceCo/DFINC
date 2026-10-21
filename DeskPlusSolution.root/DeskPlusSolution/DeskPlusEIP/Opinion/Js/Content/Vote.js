Vote = {}

var ArrIsReq = [];      //필수질문배열
var ArrSelItem = [];    //선택한질문값배열

$(function () {
    $(document).ready(function () {

        //이벤트

        // [인쇄]
        $('#cbtnPrint').click(function (e) { e.preventDefault(); Vote.PrintElem({ leaveOpen: false, printMode: 'popup' }); });

        // [수정]
        $('#cbtnVote').click(function (e) { e.preventDefault(); Vote.VoteSubmit(); });

        // [취소]
        $('#cbtnCancel').click(function (e) { e.preventDefault(); Vote.Cancel(); });

        // [짧은답글추가]
        $('#cbtnReplyAdd').click(function (e) { e.preventDefault(); JtReplyAdd(0); });

        if (QuesID > 0) {
            setTimeout(function () {
                Vote.GetAttachFileList();   //첨부파일리스트
                Vote.GetQuesUnitList();     //질문내용리스트
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
Vote.PrintElem = function (options) {
    $('#prtArea').printElement(options);
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [투표]
***************************************************/
Vote.VoteSubmit = function () {
    // 설문선택값
    ArrSelItem = [];
    var arr = $('[name^="objitem_"]');
    $.each(arr, function (i) {
        Vote.GetItemVal(arr[i]);
    });

    // 필수항목체크
    if (ArrSelItem.length < 1) {
        alert(R_Msg_ErrOpt); return;
    }
    for (var i = 0; i < ArrIsReq.length; i++) {
        if (!Vote.GetIsReq(ArrIsReq[i]["unitid"])) {
            alert(R_Msg_ErrReq); return;
        }
    }

    //tbHiddenVals
    $.each(ArrSelItem, function (i) {
        $("#tbHiddenVals").val(
            $("#tbHiddenVals").val() 
            +ArrSelItem[i]["unitid"] + "^" + ArrSelItem[i]["itemid"] + "^" + ArrSelItem[i]["itemcont"] + "^" + ArrSelItem[i]["point"] + "^" + ArrSelItem[i]["utype"] 
            + ";"
        )
    });

    __doPostBack('lbtnSubmit', "ADD");
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 설문항목 선택값 가져오기(ArrSelItem)
***************************************************/
Vote.GetItemVal = function (_obj) {
    var arrID = _obj.id;
    var rtnval = "";

    if (_obj.nodeName.toUpperCase() == "INPUT") {
        if ($(_obj).is(":checked")) {
            var tmpCont = ($("#txt" + arrID).length > 0) ? $("#txt" + arrID).val() : "";
            Vote.AddArrSelItem(arrID.split("_")[1], arrID.split("_")[2], tmpCont, 0, $(_obj).attr("utype"));
        }
    } else {
        if ($("#" + _obj.id).val() != "") {
            var itemID = (arrID.split("_").length < 3) ? 0 : arrID.split("_")[2];
            Vote.AddArrSelItem(arrID.split("_")[1], itemID, $("#" + _obj.id).val(), $("#" + _obj.id).val(), $(_obj).attr("utype"));
        }
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 설문 선택값 배열객체 입력 (ArrSelItem)
***************************************************/
Vote.AddArrSelItem = function (unitid, itemid, itemcont, point, utype) {
    ArrSelItem.push({ 'unitid': unitid, 'itemid': itemid, 'itemcont': itemcont, 'point': point, 'utype': utype });
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 필수항목 값 배열객체 입력 (ArrIsReq)
***************************************************/
Vote.AddArrIsReq = function (unitid) {
    ArrIsReq.push({ 'unitid': unitid });
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 설문 필수항목체크
***************************************************/
Vote.GetIsReq = function (_id) {
    for (var i = 0; i < ArrSelItem.length; i++) {
        if (ArrSelItem[i]["unitid"] == _id) { return true;  }
    }
    return false;
}
/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [목록/취소]
***************************************************/
Vote.Cancel = function () {

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
        , 'listtype': ListType
        , 'quesID': QuesID
    };

    var bUrl = $.url(PageBeforeUrl);    //  bUrl.attr('path') / bUrl.attr('query')
    CallEncrypt(CallGetParam());

    //parent.location.href = bUrl.attr('path') + "?p=" + GetEncryptValue;
    location.href = bUrl.attr('path') + "?p=" + GetEncryptValue;
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 첨부파일리스트
***************************************************/
Vote.GetAttachFileList = function () {
    ParamArray = { 'quesID': QuesID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Vote.SuccessAttachFile");
    var url = "/DeskPlusEIP/Opinion/Ws/Content/Vote.asmx/GetAttachList";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 첨부파일리스트 보여주기
***************************************************/
Vote.SuccessAttachFile = function (data, status) {
    $('#phFileList').setTemplateURL('/DeskPlusEIP/Opinion/Jt/Admin/Content/JtAttachListView.htm?seq=' + Guids());
    $('#phFileList').processTemplate(data.d);
    setTimeout(function () { Vote.Resource('ATTACH'); }, 100);
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
작성목적 : 질문내용가져오기
***************************************************/
Vote.GetQuesUnitList = function () {
    ParamArray = { 'quesID': QuesID };
    CallEncrypt(CallGetParam());        //배열값을 파라미터형태로 반환

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Vote.ScsUnitList");
    var url = "/DeskPlusEIP/Opinion/Ws/Content/Vote.asmx/GetQuesUnitEtc";
    AjaxService(url, data, obj);
}
Vote.ScsUnitList = function (data, status) {
    $.each(data.d, function (i) {
        Vote.AddObj(i, data.d[i]);
    });

    //$("#txtArea").val($('#divUnitCont').html()); debug
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 질문항목생성
***************************************************/
var unitCnt = 1;
Vote.AddObj = function (i, arr) {

    var unitID = arr["UnitID"];
    var unitItemID = arr["UnitItemID"];

    var ulObj = document.getElementById("ul_" + unitID);
    var titObj = document.getElementById("tit_" + unitID);

    // 그룹객체
    if (ulObj == null) {
        var ulObj = Vote.CreateObj("ul", "ul_" + unitID, "", "", "", "");      //<ul id="ul_14"></ul>
        if (ulObj != null) {
            $('#divUnitCont').append(ulObj);
        }
    }

    // 타이틀
    if (titObj == null) {
        var liObj = Vote.CreateObj("li", "", "", "opn_unittit", "", "");

        // 타이틀(질문) 제목 : <span id="tit_14"> 1. 단일선택-질문타이틀</span>
        titObj = Vote.CreateObj("span", "tit_" + unitID, "", "", unitCnt + ". " + arr["Title"], "");
        unitCnt++;
        liObj.insertAdjacentHTML("beforeEnd", "&nbsp;"); liObj.appendChild(titObj);

        // 타이틀(질문) 필수여부 : <span id="req_14" class="opn_req">[필수]</span>
        if (arr["IsReq"] == "1") {
            Vote.AddArrIsReq(unitID);   //필수항목id입력
            var reqObj = Vote.CreateObj("span", "", "", "opn_req", "[" + R_TitReq + "]", "");
            liObj.insertAdjacentHTML("beforeEnd", "&nbsp;"); liObj.appendChild(reqObj);
        }
        // 타이틀(질문) 단일가점(C) : <select><option></option></select>
        if (arr["Type"] == "C" && parseInt(arr["Point"]) > 0) {
            var selObj = Vote.CreateObj("select", "pnt_" + unitID, "objitem_" + unitID, "", "", arr["Type"]);
            selObj.add(Vote.CreateObjOpt("---", ""));
            for (i = 0; i < parseInt(arr["Point"]); i++) {
                selObj.add(Vote.CreateObjOpt(i + 1, i + 1));
            }
            liObj.insertAdjacentHTML("beforeEnd", "&nbsp;"); liObj.appendChild(selObj);
        }
        // 타이틀(질문) 주관식(E)
        if (arr["Type"] == "E") {
            var areaObj = Vote.CreateObj("textarea", "txtarea_" + unitID, "objitem_" + unitID, "", "", arr["Type"]);
            liObj.insertAdjacentHTML("beforeEnd", "<br>"); liObj.appendChild(areaObj);
        }
        ulObj.appendChild(liObj);       //ul Tag에 li Tag 추가
    }

    // 질문항목
    var itemID = "item_" + unitID + "_" + unitItemID;
    var itemObj = document.getElementById(itemID);
    if (itemObj == null) {
        var tmpclass = (arr["IsHorizon"] == "0") ? "opn_unititem" : "opn_unititemHor";
        itemObj = Vote.CreateObj("li", "", "", tmpclass, "", ""); //<li class="opn_unititem"></li>

        //텍스트
        var itemTxt = Vote.CreateObj("span", "", "", "", arr["ItemCont"], "");

        // 단일선택(A)
        if (arr["Type"] == "A") {
            var typeAobj = Vote.CreateObj("input", itemID, "objitem_" + unitID, "", "", arr["Type"]);
            typeAobj.setAttribute("type", "radio");
            itemObj.insertAdjacentHTML("beforeEnd", "&nbsp;"); itemObj.appendChild(typeAobj);
        }
        // 다중선택(B)
        if (arr["Type"] == "B") {
            var typeBobj = Vote.CreateObj("input", itemID, "objitem_" + unitID, "", "", arr["Type"]);
            typeBobj.setAttribute("type", "checkbox");
            itemObj.insertAdjacentHTML("beforeEnd", "&nbsp;"); itemObj.appendChild(typeBobj);
        }

        itemObj.appendChild(itemTxt);   //텍스트추가

        // 주관식
        if (arr["ItemType"] == "1") {
            var itemTxtObj = Vote.CreateObj("input", "txt" + itemID, "txt" + itemID, "", "", "");
            itemTxtObj.setAttribute("type", "text");
            itemObj.insertAdjacentHTML("beforeEnd", "&nbsp;"); itemObj.appendChild(itemTxtObj);
        }

        // 다중가점(D)
        if (arr["Type"] == "D") {
            var typeDobj = Vote.CreateObj("select", itemID, "objitem_" + unitID, "", "", arr["Type"]);
            typeDobj.add(Vote.CreateObjOpt("---", ""));
            for (i = 0; i < parseInt(arr["Point"]); i++) {
                typeDobj.add(Vote.CreateObjOpt(i + 1, i + 1));
            }
            itemObj.insertAdjacentHTML("beforeEnd", "&nbsp;"); itemObj.appendChild(typeDobj);
        }
        // 이미지
        if (arr["ItemImgUrl"] != "") {
            var imgObj = Vote.CreateObj("img", "", "", "", "", "");
            imgObj.setAttribute("src", arr["ItemImgUrl"]);
            imgObj.setAttribute("title", "");   //R_TitBig
            imgObj.setAttribute("style", "max-width:500px;");
            //imgObj.setAttribute("onclick", "");
            itemObj.insertAdjacentHTML("beforeEnd", "&nbsp;"); itemObj.appendChild(imgObj);
        }
        ulObj.appendChild(itemObj);
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 질문항목생성
***************************************************/
Vote.CreateObj = function (_tagName, _id, _name, _class, _text, _utype) {
    if (_tagName == "") return null;
    var tmpObj = document.createElement(_tagName);
    tmpObj.id = (_id != "") ? _id : "tmp_" + Guids() + Guids();
    if (_name != "") { tmpObj.setAttribute("name", _name); }
    if (_class != "") { tmpObj.setAttribute("class", _class); }
    if (_text != "") { tmpObj.innerHTML = _text; }
    if (_utype != "") { tmpObj.setAttribute("utype", _utype); }

    var rtnObj = ($("#" + tmpObj.id).get() == "") ? tmpObj : null;
    return rtnObj;
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 질문항목생성 (option)
***************************************************/
Vote.CreateObjOpt = function (_text, _val) {
    var optObj = document.createElement("option");
    optObj.text = _text;
    optObj.value = _val;
    return optObj;
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
    var obj = eval("Vote.ScsReplyList");
    var url = "/DeskPlusEIP/Opinion/Ws/Content/Result.asmx/GetReplyList";
    AjaxService(url, data, obj);
}
Vote.ScsReplyList = function (data, status) {
    $('#phReply').setTemplateURL('/DeskPlusEIP/Opinion/Jt/Content/JtReplyList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phReply').processTemplate(data.d);

    setTimeout(function () { Vote.Resource('REPLY'); }, 100);
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
작성목적 : 다국어처리
***************************************************/
Vote.Resource = function (sec) {

//    switch (sec.toUpperCase()) {
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

//    }
    if ($(".ModMsg") != undefined) { $(".ModMsg").text(R_Modify); }
    if ($(".DelMsg") != undefined) { $(".DelMsg").text(R_Del); }
}
/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 팝업 투표후, 포탈새로고침
***************************************************/
Vote.Close = function () {
    if (opener.Opinion_Reload) {
        opener.Opinion_Reload();
    } 
    self.close();
}