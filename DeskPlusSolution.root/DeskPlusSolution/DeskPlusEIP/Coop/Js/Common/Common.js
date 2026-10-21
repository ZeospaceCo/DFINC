Common = {}

/**************************************************
작성자   : 이문호
작성일자 : 2011-04-11
작성목적 : 근거문서 보기
***************************************************/
Common.View = function (_baseDocumnetID) {
    ParamArray = {
        'documentID': _baseDocumnetID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Folder/Common/ViewBaseDoc.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DocViewer" + _baseDocumnetID, "1024", "700", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-06-20
작성목적 : 근거문서 삭제
***************************************************/
Common.Del = function (p_documentID, p_docNumber) {
    BaseDocIDs = BaseDocIDs.split("|").join("");  // replaceAll
    BaseDocIDs = BaseDocIDs.trimEnds(';');
    var BaseDocIDsStr = BaseDocIDs.split(';');
    var newDocIDs = "";

    // 작제된 문서번호를 제외한 나머지 문서번호 새로 저장
    for (var i = 0; i < BaseDocIDsStr.length; i++) {
        if (BaseDocIDsStr[i] != p_documentID + "^" + p_docNumber) {
            newDocIDs = newDocIDs + '|' + BaseDocIDsStr[i] + ";";
        }
    }

    BaseDocIDs = newDocIDs;

    //히든박스에 문서번호 넣기 - 개발용
    $('#tbAddDocID').val(BaseDocIDs);

    Common.DelRow(p_documentID);
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-06-20
작성목적 : 근거문서 삭제시 ROW 제거
***************************************************/
Common.DelRow = function (p_documentID) {
    var row = $("#" + p_documentID);
    BaseDocCnt = BaseDocCnt - 1;
    row.remove();

    if (BaseDocCnt == 0) {
        $("#contentTable").append(
             "<tr id='noContent'>"
              + "<td colspan='8' class='center'>" + R_NoContent + "</td>"
           + "</tr>"
    	);
    }
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-06-20
작성목적 : 근거문서 순서변경(위로)
***************************************************/
Common.Up = function (_documentID, p_docNumber) {
    var selectRow = $('#' + _documentID);
    var upRow = selectRow.prev();

    selectRow.insertBefore(upRow);

    // IE의 테이블 하단  딜레이현상이 나타나 삽입
    // 이유는 모르겠으나 upRow에 effect를 넣어주면 해당 현상이 사라짐
    upRow.stop();

    if (upRow.html() != null) {
        var selectRowVal = '|' + _documentID + '^' + p_docNumber + ';';
        var upRowVal = '|' + upRow.attr("id") + '^' + upRow.attr("name") + ';';

        //변수에 저장된 문서번호 순서 변경
        BaseDocIDs = BaseDocIDs.replace(selectRowVal, '');
        BaseDocIDs = BaseDocIDs.replace(upRowVal, selectRowVal + upRowVal);
    }

    //히든박스에 문서번호 넣기 - 개발용
    $('#tbAddDocID').val(BaseDocIDs);
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-06-20
작성목적 : 근거문서 순서변경(아래로)
***************************************************/
Common.Down = function (_documentID, p_docNumber) {
    var selectRow = $('#' + _documentID);
    var downRow = selectRow.next();

    selectRow.insertAfter(downRow);

    // IE의 테이블 하단  딜레이현상이 나타나 삽입
    // 이유는 모르겠으나 downRow에 effect를 넣어주면 해당 현상이 사라짐
    downRow.stop();

    if (downRow.html() != null) {
        //파라미터에 저장된 문서번호 순서 변경
        var selectRowVal = '|' + _documentID + '^' + p_docNumber + ';';
        var downRowVal = '|' + downRow.attr("id") + '^' + downRow.attr("name") + ';';

        BaseDocIDs = BaseDocIDs.replace(selectRowVal, '');
        BaseDocIDs = BaseDocIDs.replace(downRowVal, downRowVal + selectRowVal);
    }

    //히든박스에 문서번호 넣기 - 개발용
    $('#tbAddDocID').val(BaseDocIDs);
}

/******************************************************************************
############# 작성목적 : 업무협업 근거문서 순서변경(위로) ####################
*******************************************************************************/

/**************************************************
작성자   : shimpark
작성일자 : 2014-08-30
작성목적 : 업무협업 근거문서  문서 보기
***************************************************/
Common.CoopViewer = function (p_listID) {
    ParamArray = {
        'listID': p_listID,
        'groupID': p_listID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Coop/Content/Total/ViewPopup.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "CoopViewer" + p_listID, "1024", "700", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

/**************************************************
작성자   : shimpark
작성일자 : 2014-08-30
작성목적 : 업무협업 근거문서 순서변경(위로)
***************************************************/
Common.CoopUp = function (p_listID) {
    var selectRow = $('#' + p_listID);
    var upRow = selectRow.prev();

    selectRow.insertBefore(upRow);

    // IE의 테이블 하단  딜레이현상이 나타나 삽입
    // 이유는 모르겠으나 upRow에 effect를 넣어주면 해당 현상이 사라짐
    upRow.stop();

    if (upRow.html() != null) {
        var selectRowVal = '|' + p_listID + ';';
        var upRowVal = '|' + upRow.attr("id") + ';';

        //파라미터에 저장된 문서번호 순서 변경
        CoopBaseIDs = CoopBaseIDs.replace(selectRowVal, '');
        CoopBaseIDs = CoopBaseIDs.replace(upRowVal, selectRowVal + upRowVal);

        //히든박스에 문서번호 넣기 - 개발용
        $('#tbAddCoopID').val(CoopBaseIDs);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2014-08-30
작성목적 : 업무협업 근거문서  순서변경(아래로)
***************************************************/
Common.CoopDown = function (p_listID) {
    var selectRow = $('#' + p_listID);
    var downRow = selectRow.next();

    selectRow.insertAfter(downRow);

    // IE의 테이블 하단  딜레이현상이 나타나 삽입
    // 이유는 모르겠으나 downRow에 effect를 넣어주면 해당 현상이 사라짐
    downRow.stop();

    if (downRow.html() != null) {
        //파라미터에 저장된 문서번호 순서 변경
        var selectRowVal = '|' + p_listID + ';';
        var downRowVal = '|' + downRow.attr("id") + ';';

        CoopBaseIDs = CoopBaseIDs.replace(selectRowVal, '');
        CoopBaseIDs = CoopBaseIDs.replace(downRowVal, downRowVal + selectRowVal);

        //히든박스에 문서번호 넣기 - 개발용
        $('#tbAddCoopID').val(CoopBaseIDs);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2014-08-30
작성목적 : 업무협업 근거문서 문서 삭제
***************************************************/
Common.CoopDel = function (p_listID) {
    CoopBaseIDs = CoopBaseIDs.split("|").join("");  // replaceAll
    CoopBaseIDs = CoopBaseIDs.trimEnds(';');
    var ListBaseIDsStr = CoopBaseIDs.split(';');
    var newListIDs = "";

    //팝업 파라미터에 삭제한 나머지 문서번호 저장
    for (var i = 0; i < ListBaseIDsStr.length; i++) {
        if (ListBaseIDsStr[i] != p_listID) {
            newListIDs = newListIDs + "|" + ListBaseIDsStr[i] + ";";
        }
    }

    CoopBaseIDs = newListIDs;

    //히든박스에 문서번호 넣기 - 개발용
    $('#tbAddCoopID').val(CoopBaseIDs);

    Common.CoopDelRow(p_listID);
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-20
작성목적 : 업무협업 근거문서  삭제시 ROW 제거
***************************************************/
Common.CoopDelRow = function (p_listID) {
    var row = $("#" + p_listID);

    CoopBaseCnt = CoopBaseCnt - 1;

    row.remove();

    if (CoopBaseCnt == 0) {
        $("#contentCoopTable").append(
             "<tr id='noCoopContent'>"
              + "<td colspan='4' class='center'>" + R_NoContent + "</td>"
           + "</tr>"
    	);
    }
}