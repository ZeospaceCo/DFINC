GetBaseDoc = {}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-19
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $('#btConfirm').click(function (e) {
        e.preventDefault();
        GetBaseDoc.Confirm();
    });

    $('#btClose').click(function (e) {
        e.preventDefault();
        GetBaseDoc.Close();
    });

    $("input:radio[name=rdFolder]").click(function () {
        GetBaseDoc.LoadDisplay();
    });

    $('#pvNum').click(function (e) {
        e.preventDefault();
        GetBaseDoc.pvNum();
    });

    $('#nxNum').click(function (e) {
        e.preventDefault();
        GetBaseDoc.nxNum();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    // 문서 리스트 가져오기
    GetBaseDoc.LoadDisplay();

});


/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-07-19
작성목적 : [닫기]
**********************************************************************************************/
GetBaseDoc.Close = function () {

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}


/**************************************************
작성자   : famo23
작성일자 : 2019-07-22
작성목적 : 문서함 선택  
***************************************************/
GetBaseDoc.LoadDisplay = function () {

    PageNo = 1;
    $('#phApproval').empty();

    FolderName = $(":input:radio[name=rdFolder]:checked").val();

    GetBaseDoc.getCount();

}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-22
작성목적 : 문서 리스트 갯수 가져오기 (웹서비스 호출)
***************************************************/
GetBaseDoc.getCount = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/GetBaseCount";

    //파라미터값 배열에 담기
    ParamArray = {
        'FolderName': FolderName,
        'PageNo': PageNo
    };

    var obj = eval("GetBaseDoc.succeededCount");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-22
작성목적 : 문서 리스트 갯수 가져오기 (웹서비스 값 받기)
***************************************************/
GetBaseDoc.succeededCount = function (data, status) {

    var tmpVal = data.d;
    var arrVal = tmpVal.split("^");

    PageNo = arrVal[1];

    $('#TotalNum').text(arrVal[0]);
    $('#PageNum').text(arrVal[1]);

    GetBaseDoc.getList();
}


/**************************************************
작성자   : famo23
작성일자 : 2019-07-22
작성목적 : 문서 리스트 가져오기 (웹서비스 호출)
***************************************************/
GetBaseDoc.getList = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/GetBaseList";

    //파라미터값 배열에 담기
    ParamArray = {
        'FolderName': FolderName,
        'PageNo': PageNo,
        'PageSize': PageSize
    };

    var obj = eval("GetBaseDoc.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-22
작성목적 : 문서 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
GetBaseDoc.succeeded = function (data, status) {

    $('#phApproval').setTemplateURL('/DeskPlusMobile/Approval/Jt/JTBaseList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phApproval').processTemplate(data.d);

    setTimeout(function () {
        GetBaseDoc.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-07-22
작성목적 : 다국어 처리
**********************************************************************************************/
GetBaseDoc.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-22
작성목적 : 이전 페이지 이동
***************************************************/
GetBaseDoc.pvNum = function () {

    if (parseInt(PageNo) > 1) {
        PageNo = parseInt(PageNo) - 1;

        $('#PageNum').text(PageNo);
        $('#phApproval').empty();

        GetBaseDoc.getList();
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2019-07-22
작성목적 : 다음 페이지 이동
***************************************************/
GetBaseDoc.nxNum = function () {

    var tmpTotalNum = $("#TotalNum").html();

    if (parseInt(PageNo) < parseInt(tmpTotalNum)) {
        PageNo = parseInt(PageNo) + 1;

        $('#PageNum').text(PageNo);
        $('#phApproval').empty();

        GetBaseDoc.getList();
    }
}



/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-07-23
작성목적 : 문서추가
**********************************************************************************************/
GetBaseDoc.Add = function (p_documentID, p_docNumber, p_subject) {

    if (parent) {
        if (parent.DocumentID != undefined) {
            if (parent.DocumentID == p_documentID) {
                parent.action_popup.alert(R_Msg_ErrBaseSameDoc); //동일한 문서는 추가할 수 없습니다.
                return;
            }
        }

        if (parent.ParentDocumentID != undefined) {
            if (parent.ParentDocumentID == p_documentID) {
                parent.action_popup.alert(R_Msg_ErrBaseSameDoc); //동일한 문서는 추가할 수 없습니다.
                return;
            }
        }

        if (parent.GroupID != undefined) {
            if (parent.GroupID == p_documentID) {
                parent.action_popup.alert(R_Msg_ErrBaseSameDoc); //동일한 문서는 추가할 수 없습니다.
                return;
            }
        }
    }

    // 부모창에 추가된 근거문서가 있으면 체크
    if (BaseDocIDs != "") {

        var trimDocumentIDsP = BaseDocIDs.split("|").join("");  // replaceAll
        trimDocumentIDsP = trimDocumentIDsP.trimEnds(';'); // 부모창
        var selStrParent = trimDocumentIDsP.split(';'); // 부모창

        // 부모창에 이미 추가된 문서인지 체크
        for (i = 0; i < selStrParent.length; i++) {
            if (p_documentID + "^" + p_docNumber == selStrParent[i]) {
                parent.action_popup.alert(R_Msg_ErrBaseDoc);
                return;
            }
        }
    }

    var trimDocumentIDs = BaseDocPopUpIDs.split("|").join("");  // replaceAll
    trimDocumentIDs = trimDocumentIDs.trimEnds(';'); // 팝업창
    var selStrPopUp = trimDocumentIDs.split(';'); //팝업창    

    // 팝업창에서 이미 선택된 문서인지 체크
    for (i = 0; i < selStrPopUp.length; i++) {
        if (p_documentID + "^" + p_docNumber == selStrPopUp[i]) {
            parent.action_popup.alert(R_Msg_ErrBaseDocList); return;
        }
    }

    //팝업용 파라미터에 추가한 문서번호 저장
    BaseDocPopUpIDs = BaseDocPopUpIDs + '|' + p_documentID + '^' + p_docNumber + ';'

    //히든박스에 문서번호 넣기 - 개발용
    $('#tbAddDocID').val(BaseDocPopUpIDs);

    GetBaseDoc.AddRow(p_documentID, p_docNumber, p_subject);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-23
작성목적 : 추가시 ROW 그리기
***************************************************/
GetBaseDoc.AddRow = function (p_documentID, p_docNumber, p_subject) {

    var folderName = "";
    RowMax = RowMax + 1;

    switch (FolderName) {
        case "Dept":
            folderName = R_PublicBase;
            break;
        case "Private":
            folderName = R_PrivateBase;
            break;
        case "Approval":
            folderName = R_ApprovalBase;
            break;
        case "Arbitrary":
            folderName = R_ArbitraryBase;
            break;
        case "Carboncopy":
            folderName = R_CarboncopyBase;
            break;
        case "Forward":
            folderName = R_ForwardBase;
            break;
        case "Report":
            folderName = R_Report;
            break;
    }

    p_subject = p_subject.replace("zeo~zeo", "'");

    var contentHtml = "<ul class='mli7' id='" + p_documentID + "'><li class='contentblock'><span class='base'>"
             + "<img src='/DeskPlusMobile/Content/ko/Base/Img/ico_tree_listDel.gif'onclick=\"BaseDel('" + p_documentID + "', '" + p_docNumber + "')\" style='cursor:pointer' />&nbsp;&nbsp;</span>"
             + "<div class='conBox'><span class='LNoready'>[" + folderName + "] " + TextReverseToXss(p_subject) + "</span><br />"
             + "<span class='sch_user'>" + p_docNumber + "</span></div></li></ul>";

    $("#baseList").append(contentHtml);
}

function TextReverseToXss(arg) {

    arg = arg.replace(/&/gi, "&amp;");
    arg = arg.replace(/"/gi, "&quot;");
    arg = arg.replace(/#/gi, "&#35;");
    arg = arg.replace(/'/gi, "&#39;");
    arg = arg.replace("(", "&#40;");
    arg = arg.replace(")", "&#41;");
    arg = arg.replace(/</gi, "&lt;");
    arg = arg.replace(/>/gi, "&gt;");
    arg = arg.replace(/>/gi, "&gt;");

    return arg;
}


/**************************************************
작성자   : famo23
작성일자 : 2019-07-23
작성목적 : 문서 삭제
***************************************************/
BaseDel = function (p_documentID, p_docNumber) {

    BaseDocPopUpIDs = BaseDocPopUpIDs.split("|").join("");  // replaceAll
    BaseDocPopUpIDs = BaseDocPopUpIDs.trimEnds(';');
    var BaseDocIDsStr = BaseDocPopUpIDs.split(';');
    var newDocIDs = "";

    //팝업 파라미터에 삭제한 나머지 문서번호 저장
    for (var i = 0; i < BaseDocIDsStr.length; i++) {
        if (BaseDocIDsStr[i] != p_documentID + "^" + p_docNumber) {
            newDocIDs = newDocIDs + "|" + BaseDocIDsStr[i] + ";";
        }
    }

    BaseDocPopUpIDs = newDocIDs;

    //히든박스에 문서번호 넣기 - 개발용
    $('#tbAddDocID').val(BaseDocPopUpIDs);

    GetBaseDoc.DelRow(p_documentID);
}


/**************************************************
작성자   : famo23
작성일자 : 2019-07-23
작성목적 : 삭제시 ROW 제거
***************************************************/
GetBaseDoc.DelRow = function (p_documentID) {

    var row = $("#" + p_documentID);

    RowMax = RowMax - 1;

    row.remove();
}


/**************************************************
작성자   : famo23
작성일자 : 2019-07-23
작성목적 : 저장
***************************************************/
GetBaseDoc.Confirm = function () {

    var rowCnt = RowMax;
    var BaseDocsPopUp = $("#tbAddDocID").val(); //팝업창 추가된 문서번호들 
    var BaseDocsPopUpIDs = BaseDocsPopUpIDs;
    var baseDocHTML = $("#baseList").html();

    if (jQuery.trim(baseDocHTML) != "") {
        //baseDocHTML = baseDocHTML.replace(/GetBaseDoc./gi, CallBackName);

        //부모창 변수에 선택된 팝업창 변수값 저장
        BaseDocIDs = BaseDocIDs + BaseDocPopUpIDs;
        parent.BaseDocIDs = BaseDocIDs;

        //부모창 카운트 변수에 팝업창 카운트 변수값 더하기
        var ParentRows = parent.BaseDocCnt + rowCnt;
        parent.BaseDocCnt = ParentRows;

        //부모창 히든박스에 문서번호 넣기 - 개발용
        $(parent.document).find('#tbAddDocID').val(BaseDocIDs);

        //근거문서 타이틀
        $(parent.document).find("#uBase").css('display', '');

        // ROW 그리기
        $(parent.document).find("#baseList").append(baseDocHTML);

        parent.$('html, body').scrollTop($(parent.document).height());

        GetBaseDoc.Close();
    }
    else {
        parent.action_popup.alert(R_NoSelList);
    }
}