Common = {}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 근거문서 보기
***************************************************/
Common.View = function (_baseDocumnetID) {
    ParamArray = {
        'documentID': _baseDocumnetID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Folder/Common/ViewBaseDoc.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DocViewer" + _baseDocumnetID, "1200", "800", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
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
    $('#tbAddAppID').val(BaseDocIDs);

    Common.DelRow(p_documentID);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 근거문서 삭제시 ROW 제거
***************************************************/
Common.DelRow = function (p_documentID) {
    var row = $("#" + p_documentID);
    BaseDocCnt = BaseDocCnt - 1;
    row.remove();

    if (BaseDocCnt == 0) {
        $("#contentTable").append(
             "<li id='noContent'></li>"
    	);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
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
    $('#tbAddAppID').val(BaseDocIDs);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
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
    $('#tbAddAppID').val(BaseDocIDs);
}

/******************************************************************************
############# 작성목적 : 업무협업 근거문서 순서변경(위로) ####################
*******************************************************************************/

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 업무협업 근거문서  문서 보기
***************************************************/
Common.TaskViewer = function (p_listID) {
    ParamArray = {
        'listID': p_listID,
        'groupID': p_listID,
        'isType': 'BA'
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Task/Common/ViewPopup.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "TaskView_" + Guids(), "1024", "700", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 업무협업 근거문서 순서변경(위로)
***************************************************/
Common.TaskUp = function (p_listID) {
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
        BaseTasks = BaseTasks.replace(selectRowVal, '');
        BaseTasks = BaseTasks.replace(upRowVal, selectRowVal + upRowVal);

        //히든박스에 문서번호 넣기 - 개발용
        $('#tbAddTaskID').val(BaseTasks);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 업무협업 근거문서  순서변경(아래로)
***************************************************/
Common.TaskDown = function (p_listID) {
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

        BaseTasks = BaseTasks.replace(selectRowVal, '');
        BaseTasks = BaseTasks.replace(downRowVal, downRowVal + selectRowVal);

        //히든박스에 문서번호 넣기 - 개발용
        $('#tbAddTaskID').val(BaseTasks);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-14
작성목적 : 업무협업 근거문서 문서 삭제
***************************************************/
Common.TaskDel = function (p_listID) {
    BaseTasks = BaseTasks.split("|").join("");  // replaceAll
    BaseTasks = BaseTasks.trimEnds(';');
    var ListBaseIDsStr = BaseTasks.split(';');
    var newListIDs = "";

    //팝업 파라미터에 삭제한 나머지 문서번호 저장
    for (var i = 0; i < ListBaseIDsStr.length; i++) {
        if (ListBaseIDsStr[i] != p_listID) {
            newListIDs = newListIDs + "|" + ListBaseIDsStr[i] + ";";
        }
    }

    BaseTasks = newListIDs;

    //히든박스에 문서번호 넣기 - 개발용
    $('#tbAddTaskID').val(BaseTasks);

    Common.TaskDelRow(p_listID);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 업무협업 근거문서  삭제시 ROW 제거
***************************************************/
Common.TaskDelRow = function (p_listID) {
    var row = $("#" + p_listID);

    BaseTaskCnt = BaseTaskCnt - 1;

    row.remove();

    if (BaseTaskCnt == 0) {
        $("#contentTaskTable").append(
             "<tr id='noTaskContent'>"
              + "<td colspan='4' class='center'>" + R_NoContent + "</td>"
           + "</tr>"
    	);
    }
}


/**************************************************
작성자 : maniaysw
작성일자 : 2015-05-11
작성목적 : 업무협업 상태표시 팝업띄우기
***************************************************/
Common.StatusInfo = function () {
    var url = "/DeskPlusEIP/Task/Common/StatusInfo.aspx";
    
    PopupWin(300, 350, url, "-320", "0", false);
}


/**************************************************
작성자   : famo23
작성일자 : 2015-05-11
작성목적 : 변경이력 보기
***************************************************/
Common.History = function (_gubun, p_listID, p_listID) {
    ParamArray = {
        'gubun': _gubun,
        'listID': p_listID,
        'groupID': p_listID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Task/Common/ViewHistory.aspx?p=" + GetEncryptValue;    
    PopupWin(700, 500, url, "-500", "", true);
}

