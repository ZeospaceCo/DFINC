QuesView = {}

$(function () {
    $(document).ready(function () {

        //이벤트

        // [인쇄]
        $('#cbtnPrint').click(function (e) { e.preventDefault(); QuesView.PrintElem({ leaveOpen: false, printMode: 'popup' }); });

        // [복사본(설문생성)]
        $('#cbtnCopy').click(function (e) { e.preventDefault(); QuesView.AddCopy(); });

        // [수정]
        $('#cbtnEdit').click(function (e) { e.preventDefault(); QuesView.Edit(); });

        // [취소]
        $('#cbtnCancel').click(function (e) { e.preventDefault(); QuesView.Cancel(); });

        if (QuesID > 0) {
            setTimeout(function () {
                QuesView.GetAttachFileList();    //첨부파일리스트
                QuesView.GetQuesUnitList();     //질문내용리스트
            }, 200);
        }
    });
});

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [인쇄]
***************************************************/
QuesView.PrintElem = function (options) {
    $('#prtArea').printElement(options);
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [복사본(설문생성)]
***************************************************/
QuesView.AddCopy = function () {
    alert("QuesView.AddCopy()");
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [수정]
***************************************************/
QuesView.Edit = function () {
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
    CallEncrypt(CallGetParam());
    
    location.href = "QuesModN.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [목록/취소]
***************************************************/
QuesView.Cancel = function () {
    ParamArray = {
        'menuID': MenuID
        , 'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
        , 'quesID':QuesID
    };
    CallEncrypt(CallGetParam());
    location.href = "QuesList.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 첨부파일리스트
***************************************************/
QuesView.GetAttachFileList = function () {
    ParamArray = { 'quesID': QuesID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("QuesView.SuccessAttachFile");
    var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/QuesView.asmx/GetAttachList";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 첨부파일리스트 보여주기
***************************************************/
QuesView.SuccessAttachFile = function (data, status) {
    $('#phFileList').setTemplateURL('/DeskPlusEIP/Opinion/Jt/Admin/Content/JtAttachListView.htm?seq=' + Guids());
    $('#phFileList').processTemplate(data.d);
    setTimeout(function () { QuesView.Resource('ATTACH'); }, 100);
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
QuesView.GetQuesUnitList = function () {
    ParamArray = { 'quesID': QuesID };
    CallEncrypt(CallGetParam());        //배열값을 파라미터형태로 반환

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("QuesView.ScsUnitList");
    var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/QuesView.asmx/GetQuesUnitEtc";
    AjaxService(url, data, obj);
}
QuesView.ScsUnitList = function (data, status) {
    $.each(data.d, function (i) {
        QuesView.AddObj(i, data.d[i]);
    });
}
/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 질문항목생성
***************************************************/
var unitCnt = 1;
QuesView.AddObj = function (i, arr) {

    var unitID = arr["UnitID"];
    var unitItemID = arr["UnitItemID"];

    var ulObj = document.getElementById("ul_" + unitID);
    var titObj = document.getElementById("tit_" + unitID);

    // 그룹객체
    if (ulObj == null) {
        var ulObj = QuesView.CreateObj("ul", "ul_" + unitID, "", "", "", "");      //<ul id="ul_14"></ul>
        if (ulObj != null) {
            $('#divUnitCont').append(ulObj);
        }
    }

    // 타이틀
    if (titObj == null) {
        var liObj = QuesView.CreateObj("li", "", "", "opn_unittit", "", "");

        // 타이틀(질문) 제목 : <span id="tit_14"> 1. 단일선택-질문타이틀</span>
        titObj = QuesView.CreateObj("span", "tit_" + unitID, "", "", unitCnt + ". " + arr["Title"], "");
        unitCnt++;
        liObj.insertAdjacentHTML("beforeEnd", "&nbsp;"); liObj.appendChild(titObj);

        // 타이틀(질문) 필수여부 : <span id="req_14" class="opn_req">[필수]</span>
        if (arr["IsReq"] == "1") {
            //QuesView.AddArrIsReq(unitID);   //필수항목id입력
            var reqObj = QuesView.CreateObj("span", "", "", "opn_req", "[" + R_TitReq + "]", "");
            liObj.insertAdjacentHTML("beforeEnd", "&nbsp;"); liObj.appendChild(reqObj);
        }
        // 타이틀(질문) 단일가점(C) : <select><option></option></select>
        if (arr["Type"] == "C" && parseInt(arr["Point"]) > 0) {
            var selObj = QuesView.CreateObj("select", "pnt_" + unitID, "objitem_" + unitID, "", "", arr["Type"]);
            selObj.add(QuesView.CreateObjOpt("---", ""));
            for (i = 0; i < parseInt(arr["Point"]); i++) {
                selObj.add(QuesView.CreateObjOpt(i + 1, i + 1));
            }
            liObj.insertAdjacentHTML("beforeEnd", "&nbsp;"); liObj.appendChild(selObj);
        }
        // 타이틀(질문) 주관식(E)
        if (arr["Type"] == "E") {
            var areaObj = QuesView.CreateObj("textarea", "txtarea_" + unitID, "objitem_" + unitID, "", "", arr["Type"]);
            liObj.insertAdjacentHTML("beforeEnd", "<br>"); liObj.appendChild(areaObj);
        }
        ulObj.appendChild(liObj);       //ul Tag에 li Tag 추가
    }

    // 질문항목
    var itemID = "item_" + unitID + "_" + unitItemID;
    var itemObj = document.getElementById(itemID);
    if (itemObj == null) {
        var tmpclass = (arr["IsHorizon"] == "0") ? "opn_unititem" : "opn_unititemHor";
        itemObj = QuesView.CreateObj("li", "", "", tmpclass, "", ""); //<li class="opn_unititem"></li>

        //텍스트
        var itemTxt = QuesView.CreateObj("span", "", "", "", arr["ItemCont"], "");

        // 단일선택(A)
        if (arr["Type"] == "A") {
            var typeAobj = QuesView.CreateObj("input", itemID, "objitem_" + unitID, "", "", arr["Type"]);
            typeAobj.setAttribute("type", "radio");
            itemObj.insertAdjacentHTML("beforeEnd", "&nbsp;"); itemObj.appendChild(typeAobj);
        }
        // 다중선택(B)
        if (arr["Type"] == "B") {
            var typeBobj = QuesView.CreateObj("input", itemID, "objitem_" + unitID, "", "", arr["Type"]);
            typeBobj.setAttribute("type", "checkbox");
            itemObj.insertAdjacentHTML("beforeEnd", "&nbsp;"); itemObj.appendChild(typeBobj);
        }

        itemObj.appendChild(itemTxt);   //텍스트추가

        // 주관식
        if (arr["ItemType"] == "1") {
            var itemTxtObj = QuesView.CreateObj("input", "txt" + itemID, "txt" + itemID, "", "", "");
            itemTxtObj.setAttribute("type", "text");
            itemObj.insertAdjacentHTML("beforeEnd", "&nbsp;"); itemObj.appendChild(itemTxtObj);
        }

        // 다중가점(D)
        if (arr["Type"] == "D") {
            var typeDobj = QuesView.CreateObj("select", itemID, "objitem_" + unitID, "", "", arr["Type"]);
            typeDobj.add(QuesView.CreateObjOpt("---", ""));
            for (i = 0; i < parseInt(arr["Point"]); i++) {
                typeDobj.add(QuesView.CreateObjOpt(i + 1, i + 1));
            }
            itemObj.insertAdjacentHTML("beforeEnd", "&nbsp;"); itemObj.appendChild(typeDobj);
        }
        // 이미지
        if (arr["ItemImgUrl"] != "") {
            var imgObj = QuesView.CreateObj("img", "", "", "", "", "");
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
QuesView.CreateObj = function (_tagName, _id, _name, _class, _text, _utype) {
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
QuesView.CreateObjOpt = function (_text, _val) {
    var optObj = document.createElement("option");
    optObj.text = _text;
    optObj.value = _val;
    return optObj;
}
/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 다국어처리
***************************************************/
QuesView.Resource = function (sec) {

    switch (sec.toUpperCase()) {
        case "ATTACH":  //첨부파일리스트
            $("#jtAttach_fileName").text(R_FileName);
            $("#jtAttach_fileType").text(R_FileType);
            $("#jtAttach_fileLen").text(R_FileLen);
            $("#jtAttach_filedelete").text(R_FileDown);
            $(".jtNoData").text(R_JTNoData);
            break;
        case "":
            break;
    }
}

