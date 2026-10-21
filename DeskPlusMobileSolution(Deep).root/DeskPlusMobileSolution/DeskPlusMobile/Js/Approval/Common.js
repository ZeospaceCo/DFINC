Common = {}

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        Common.Init();
    });
});

/**************************************************
작성목적 : 초기화
***************************************************/
Common.Init = function () {
    if (tagDic != null) {
        tagDic.add("INFO0", TagDic_INFO0);
        tagDic.add("INFO1", TagDic_INFO1);
        tagDic.add("INFO2", TagDic_INFO2);
        tagDic.add("INFO3", TagDic_INFO3);
        tagDic.add("INFO4", TagDic_INFO4);
        tagDic.add("INFO5", TagDic_INFO5);
        tagDic.add("INFO6", TagDic_INFO6);
        tagDic.add("INFO7", TagDic_INFO7);
        tagDic.add("INFO8", TagDic_INFO8);
        tagDic.add("INFO9", TagDic_INFO9);
        tagDic.add("INFO10", TagDic_INFO10);
        tagDic.add("INFO11", TagDic_INFO11);
        tagDic.add("INFO12", TagDic_INFO12);
        tagDic.add("INFO13", TagDic_INFO13);
        tagDic.add("INFO14", TagDic_INFO14);
        tagDic.add("INFO15", TagDic_INFO15);

        tagDic.add("ZD0", TagDic_ZD0);
        tagDic.add("ZD1", TagDic_ZD + "1");
        tagDic.add("ZD2", TagDic_ZD + "2");
        tagDic.add("ZD3", TagDic_ZD + "3");
        tagDic.add("ZD4", TagDic_ZD + "4");
        tagDic.add("ZD5", TagDic_ZD + "5");
        tagDic.add("ZD6", TagDic_ZD + "6");
        tagDic.add("ZD7", TagDic_ZD + "7");
        tagDic.add("ZD8", TagDic_ZD + "8");
        tagDic.add("ZD9", TagDic_ZD + "9");
        tagDic.add("ZD10", TagDic_ZD + "10");
        tagDic.add("ZD11", TagDic_ZD + "11");
        tagDic.add("ZD12", TagDic_ZD + "12");
        tagDic.add("ZD13", TagDic_ZD + "13");
        tagDic.add("ZDL", TagDic_ZD);
        tagDic.add("ZD", TagDic_ZD);

        tagDic.add("ZP", TagDic_ZP);
        tagDic.add("ZP01", TagDic_ZP + TagDic_ZD0);
        tagDic.add("ZP01_ZD1", TagDic_ZP + "1");
        tagDic.add("ZP01_ZD2", TagDic_ZP + "2");
        tagDic.add("ZP01_ZD3", TagDic_ZP + "3");
        tagDic.add("ZP01_ZD4", TagDic_ZP + "4");
        tagDic.add("ZP01_ZD5", TagDic_ZP + "5");
        tagDic.add("ZP01_ZD6", TagDic_ZP + "6");
        tagDic.add("ZP01_ZD7", TagDic_ZP + "7");

        tagDic.add("ZQ", TagDic_ZP);
        tagDic.add("ZQ01", TagDic_ZP + TagDic_ZD0);
        tagDic.add("ZQ01_ZD1", TagDic_ZP + "1");
        tagDic.add("ZQ01_ZD2", TagDic_ZP + "2");
        tagDic.add("ZQ01_ZD3", TagDic_ZP + "3");
        tagDic.add("ZQ01_ZD4", TagDic_ZP + "4");
        tagDic.add("ZQ01_ZD5", TagDic_ZP + "5");
        tagDic.add("ZQ01_ZD6", TagDic_ZP + "6");
        tagDic.add("ZQ01_ZD7", TagDic_ZP + "7");
        tagDic.add("ZQ02", TagDic_ZP + TagDic_ZD0);
        tagDic.add("ZQ02_ZD1", TagDic_ZP + "1");
        tagDic.add("ZQ02_ZD2", TagDic_ZP + "2");
        tagDic.add("ZQ02_ZD3", TagDic_ZP + "3");
        tagDic.add("ZQ02_ZD4", TagDic_ZP + "4");
        tagDic.add("ZQ02_ZD5", TagDic_ZP + "5");
        tagDic.add("ZQ02_ZD6", TagDic_ZP + "6");
        tagDic.add("ZQ02_ZD7", TagDic_ZP + "7");

        tagDic.add("ZH1", TagDic_ZH + "1");
        tagDic.add("ZH2", TagDic_ZH + "2");
        tagDic.add("ZH3", TagDic_ZH + "3");
        tagDic.add("ZH4", TagDic_ZH + "4");
        tagDic.add("ZH5", TagDic_ZH + "5");
        tagDic.add("ZH6", TagDic_ZH + "6");
        tagDic.add("ZH7", TagDic_ZH + "7");
        tagDic.add("ZH8", TagDic_ZH + "8");
        tagDic.add("ZH9", TagDic_ZH + "9");
        tagDic.add("ZH10", TagDic_ZH + "10");
        tagDic.add("ZH11", TagDic_ZH + "11");
        tagDic.add("ZH12", TagDic_ZH + "12");
        tagDic.add("ZH13", TagDic_ZH + "13");
        tagDic.add("ZH", TagDic_ZH);

        tagDic.add("ZC", TagDic_ZC);

        tagDic.add("ZR1", TagDic_ZR + "1");
        tagDic.add("ZR2", TagDic_ZR + "2");
        tagDic.add("ZR3", TagDic_ZR + "3");
        tagDic.add("ZR4", TagDic_ZR + "4");
        tagDic.add("ZR5", TagDic_ZR + "5");
        tagDic.add("ZR6", TagDic_ZR + "6");
        tagDic.add("ZR7", TagDic_ZR + "7");
        tagDic.add("ZR8", TagDic_ZR + "8");
        tagDic.add("ZR9", TagDic_ZR + "9");
        tagDic.add("ZR10", TagDic_ZR + "10");

        tagDic.add("ZA1", TagDic_ZA + "1");
        tagDic.add("ZA2", TagDic_ZA + "2");
        tagDic.add("ZA3", TagDic_ZA + "3");
        tagDic.add("ZA4", TagDic_ZA + "4");
        tagDic.add("ZA5", TagDic_ZA + "5");
        tagDic.add("ZA6", TagDic_ZA + "6");
        tagDic.add("ZA7", TagDic_ZA + "7");
        tagDic.add("ZA8", TagDic_ZA + "8");
        tagDic.add("ZA9", TagDic_ZA + "9");
        tagDic.add("ZA10", TagDic_ZA + "10");
        tagDic.add("ZA11", TagDic_ZA + "11");
        tagDic.add("ZA12", TagDic_ZA + "12");
        tagDic.add("ZA13", TagDic_ZA + "13");
        tagDic.add("ZA14", TagDic_ZA + "14");
        tagDic.add("ZA15", TagDic_ZA + "15");
        tagDic.add("ZA16", TagDic_ZA + "16");
        tagDic.add("ZA17", TagDic_ZA + "17");
        tagDic.add("ZA18", TagDic_ZA + "18");
        tagDic.add("ZA19", TagDic_ZA + "19");
        tagDic.add("ZA20", TagDic_ZA + "20");
        tagDic.add("ZA21", TagDic_ZA + "21");
        tagDic.add("ZA", TagDic_ZA);

        tagDic.add("NO", TagDic_NO);
        tagDic.add("DATE", TagDic_DATE);
        tagDic.add("LIFE", TagDic_LIFE);
        tagDic.add("SUBJ", TagDic_SUBJ);
        tagDic.add("BODY", TagDic_BODY);
        tagDic.add("BODY_etc", TagDic_BODY_etc);
        tagDic.add("EDITOR", TagDic_EDITOR);
        tagDic.add("COMPANY", TagDic_COMPANY);
        tagDic.add("UPNM", TagDic_UPNM);
        tagDic.add("EMPNUM", TagDic_EMPNUM);
        tagDic.add("UPDPT", TagDic_UPDPT);
        tagDic.add("UPJC", TagDic_UPJC);
        tagDic.add("OPINION", TagDic_OPINION);
        tagDic.add("OPINIONVIEW", TagDic_OPINIONVIEW);
        tagDic.add("FLOWZD", TagDic_FLOWZD);
        tagDic.add("FLOWZH", TagDic_FLOWZH);
        tagDic.add("FLOWZA", TagDic_FLOWZA);
        tagDic.add("FLOWZR", TagDic_FLOWZR);
        tagDic.add("ENFORCEDEPTNAME", TagDic_ENFORCEDEPTNAME);
        tagDic.add("ENFORCENAME", TagDic_ENFORCENAME);
        tagDic.add("ENFORCERECEIVECARBON", TagDic_ENFORCERECEIVECARBON);
        tagDic.add("PRINT", TagDic_Print);
    }
}

/**************************************************
ifrmForm - 양식클릭
***************************************************/
Common.ifrmForm_click = function (_event, _zqEdit) {

    if (_event != null && _zqEdit != undefined) {

        // 팝업 clear처리
        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
        $("#zeoEditorDIV").hide();

        //////////if (parent.frmLeft.oContextMenuCtrl != null) parent.frmLeft.oContextMenuCtrl.clearItem();

        preEl = nowEl;
        nowEl = _event.srcElement ? _event.srcElement : _event.target;
        clientX = _event.x; //클릭 좌표값
        clientY = _event.y;

        if (nowEl.getAttribute("isapp") != undefined) { //이미 결재된 결재칸의 경우, 팝업처리불가.
            return;
        }

        // 이전 항목이 결재자였으면, 클래스명 변경.
        if (preEl) {
            if (preEl.id.indexOf("Z") == 0 || preEl.id.indexOf("FLOW") == 0) { preEl.className = preEl.className.replace("Sel", ""); }
        }

        // 항목이 결재자였다면, 클래스 변경 (Sel) 
        // exp : 수신담당자 문서수정의 경우, 예외처리.
        var chkSec = (!_zqEdit) ? /^Z{1}[D|H|C|P|Q|A|R]{1}([0-9])?[0-9]*$/.test(nowEl.id) : /^Z{1}[C|P|Q]{1}([0-9])?[0-9]*$/.test(nowEl.id);
        if (chkSec) // 모든 결재자의 경우
        {
            if (nowEl.id == ApprovalTag) { return; }    // 결재자지정 시, ZD0 & 본인차례의 순서는 지정(수정)불가. if (nowEl.id == "ZD0") { return; }

            nowEl.className = nowEl.className + "Sel";
            if (nowEl.getAttribute("IsAsign") == "true") {
                // 결재선이 지정된 경우 - '지정취소'
                Common.popUpCancel(_event);
            }
            else {
                // 결재선이 지정안된 경우 - '자주 지정하는 결재자' 메뉴 팝업
                Common.popUpFavList(_event);
            }
        }
        // 항목이 유동결재망이라면, 클래스 변경 및 유동결재망버튼 처리
        if (nowEl.id.toUpperCase().indexOf("FLOW") == 0 && !_zqEdit) {
            nowEl.className = nowEl.className + "Sel";
            Common.popUpFlow(_event);
        }

        // 문서번호 필드의 경우, 팝업
        if (nowEl.id.toUpperCase() == "LIFE" && !_zqEdit) {
            Common.popUpLife(_event);
        }

        // 커스텀파일의 경우, 해당 파일의 onclick이벤트 호출.
        if (IsLegacy == "1" && ifrmProcessing.docClickEvent) {
            ifrmProcessing.docClickEvent(_event);
        }
    }
}
/**************************************************
ifrmForm - 양식keydown
***************************************************/
Common.ifrmForm_keydown = function (_event, _isZQ) {
    // 커스텀 스크립트 파일이 있다면 키보드 클릭시 커스텀 스크립트 내에 docClickEvent()를 실행
    if (IsLegacy == "1" && ifrmProcessing.docKeyDownEvent) {
        ifrmProcessing.docKeyDownEvent(_event);
    }
}
/**************************************************
ifrmForm - 양식keyup
***************************************************/
Common.ifrmForm_keyup = function (_event, _isZQ) {
    // 커스텀 스크립트 파일이 있다면 키보드 클릭시 커스텀 스크립트 내에 docClickEvent()를 실행
    if (IsLegacy == "1" && ifrmProcessing.docKeyUpEvent) {
        ifrmProcessing.docKeyUpEvent(_event);
    }
}
/**************************************************
ifrmForm - 양식onchage
***************************************************/
Common.ifrmForm_change = function (_event, _isZQ) {
    // 커스텀 스크립트 파일이 있다면 키보드 클릭시 커스텀 스크립트 내에 docChangeEvent()를 실행
    if (IsLegacy == "1" && ifrmProcessing.docChangeEvent) {
        ifrmProcessing.docChangeEvent(_event);
    }
}

/**************************************************
결재자지정
***************************************************/
Common.SetApproval = function (_tagID, _userID, _userName, _deptID, _deptName, _titleID, _titleName, _dutyID, _dutyName, _isGroup, _subTree, _userList) {

    //alert(_tagID + "-" + _userID + "-" + _userName + "-" + _deptID + "-" + _deptName + "-" + _titleID + "-" + _titleName + "-" + _dutyID + "-" + _dutyName + "-" + _isGroup + "-" + _subTree + "-" + _userList);

    // 발신,협조,합의합의,참조,수신(ZP),수신(ZQ)
    if (_tagID == undefined || _userID == undefined || _userName == undefined || _deptID == undefined || _deptName == undefined || _titleName == undefined || _dutyName == undefined) return;
    if (_tagID == "" || _deptID == "0" || _deptName == "0") { return; }     //alert(R_Msg_AlertUserInfo); 
    //if ($("#" + _tagID, ifrmObj).length == 0) return;                     //20150102 by.withsuki (태그가없는경우,예외처리)

    if (/Z{1}[H|Q|P|C][0-9]?[0-9]?[0-9]?$/.test(_tagID)) {
        if ($("#" + _tagID, ifrmObj).length == 0) {
            if ($("#" + _tagID.substring(0, 2), ifrmObj).length == 0) return;
        }
    }
    else {
        if ($("#" + _tagID, ifrmObj).length == 0) return;
    }

    // 본문 수정할때 ZA 단계가 지난 이후에 ZA 추가할 수 없게 체크
    if ((_tagID.substring(0, 2) == "ZA") && (ZACount > 0)) {
        action_popup.alert(R_ZACheck);
        return;
    }

    // 태그에 따른 필수값체크 (G or M)
    if (/Z{1}[Q|P|C][0-9]?[0-9]?[0-9]?$/.test(_tagID)) {

        //수신(ZQ),참조,수신(ZP)
        if (_deptID == "") { action_popup.alert(Msg_ErrorUserInfo); return; }

        // 하위부서포함인데, 하위부서리스트가 없는경우.
        //if (_subTree == "1" && _userList == "") { action_popup.alert(Msg_ErrorUserNon); return; }

        // 중복지정체크
        var isDup = false;
        var arrtmp = $("#" + _tagID, ifrmObj).children().get();
        $.each(arrtmp, function (i, o) {
            if ($("#" + o.id, ifrmObj).attr("deptID") == _deptID && (/Z{1}[Q|P][0-9]?[0-9]?[0-9]?$/.test(_tagID))) { isDup = true; return; }
            if ($("#" + o.id, ifrmObj).attr("userID") == _userID && (/Z{1}[C][0-9]?[0-9]?[0-9]?$/.test(_tagID)) && $("#" + o.id, ifrmObj).attr("isGroup") == "M") { isDup = true; return; }
            if ($("#" + o.id, ifrmObj).attr("deptID") == _deptID && (/Z{1}[C][0-9]?[0-9]?[0-9]?$/.test(_tagID)) && $("#" + o.id, ifrmObj).attr("isGroup") == "G") { isDup = true; return; }
        });
        if (isDup) { action_popup.alert(Msg_ErrorUserBefore); return; }

    } else {
        //발신,협조,합의합의
        if (_userID == "" || _userName == "" || _deptID == "" || _deptName == "") {
            action_popup.alert(Msg_ErrorUserInfo); return;
        }
    }


    // 결재자 "지정취소" 하면 원래 있던 값으로 되돌리기 위해 어트리뷰트로 저장
    //$("#" + _tagID, ifrmObj).attr("DefaultText", tagDic.get(_tagID));

    if (tagDic.get(_tagID) != null) {
        $("#" + _tagID, ifrmObj).attr("DefaultText", tagDic.get(_tagID));
    }
    else {
        $("#" + _tagID, ifrmObj).attr("DefaultText", tagDic.get(_tagID.substr(0, 2)));
    }

    var newID = "", newText = "";
    var className = _tagID.substring(0, 2);

    if (/Z{1}[H|C|P|Q]$/.test(_tagID)) {
        // 다중결재태그인 경우 (협조/참조/수신/수신)
        var cnt = 1;
        if ($("#" + _tagID, ifrmObj).children().length > 0) {
            cnt = parseInt($("#" + _tagID, ifrmObj).children(':last').attr("idx")) + 1;    // 일련번호생성(다중결재란) 1, 2, 3...: newId - "ZP" + "O" + "1" ex) ZP01, ZC01, ZQ01, ZH01
        }

        newID = className + "0" + cnt;

        // 그룹이면, 부서정보 / 사용자면 부서&사용자정보
        newText = (_isGroup == "M") ? _deptName + "/" + _titleName + "/" + _userName : _deptName;
        newText += (_subTree == "1") ? R_TitSub : "";
        newText += ",";

        // 고정태그의 경우는 이미 존재. 다중태그의 경우만 생성.
        if ($("#" + newID, ifrmObj).get() == "") {
            var itemHTML = "<span class='" + className + "' id='" + newID + "' idx='" + cnt + "'></span>";
            nowEl = $("#" + className, ifrmObj)[0];
            if (nowEl != undefined) {
                nowEl.insertAdjacentHTML("beforeEnd", itemHTML); // <span>aaa<span newid>bbb></span></span>
            }
        }
    }
    else {
        if (/Z{1}[H|Q|P|C][0-9]?[0-9]?[0-9]?$/.test(_tagID)) {
            newID = _tagID;

            if (_tagID.substring(2, 3) == "0") {

                // 그룹이면, 부서정보 / 사용자면 부서&사용자정보
                newText = (_isGroup == "M") ? _deptName + "/" + _titleName + "/" + _userName : _deptName;
                newText += (_subTree == "1") ? R_TitSub : "";
                newText += ",";

                var cnt = _tagID.replace(_tagID.substring(0, 3), "");

                // 고정태그의 경우는 이미 존재. 다중태그의 경우만 생성.
                if ($("#" + newID, ifrmObj).get() == "") {
                    var itemHTML = "<span class='" + className + "' id='" + newID + "' idx='" + cnt + "'></span>";
                    nowEl = $("#" + className, ifrmObj)[0];
                    if (nowEl != undefined) {
                        nowEl.insertAdjacentHTML("beforeEnd", itemHTML); // <span>aaa<span newid>bbb></span></span>
                    }
                }
            }
            else {
                // 고정결재태그인 경우                
                newText = _userName;
            }
        }
        else {
            // 고정결재태그인 경우
            newID = _tagID;
            newText = _userName;
        }
    }

    var tmpKey = Common.GetKey(newID);
    var tmpSeq = Common.GetSeqInfo(tmpKey, newID, _userID, _userName, _deptID, _deptName, _titleID, _titleName, _dutyID, _dutyName, "S", _isGroup, _subTree);

    Common.AddKey(newID, tmpKey, tmpSeq);                   // 결재라인 객체에 값 할당. (각 그룹별로 - 발신/수신/참조)

    // 결재자지정
    $("#" + newID, ifrmObj).html(newText);
    $("#" + newID, ifrmObj).attr("IsAsign", "true");
    $("#" + newID, ifrmObj).attr("userID", _userID);
    $("#" + newID, ifrmObj).attr("deptID", _deptID);
    $("#" + newID, ifrmObj).attr("deptName", _deptName);
    $("#" + newID, ifrmObj).attr("titleID", _titleID);
    $("#" + newID, ifrmObj).attr("titleName", _titleName);
    $("#" + newID, ifrmObj).attr("dutyID", _dutyID);
    $("#" + newID, ifrmObj).attr("dutyName", _dutyName);
    $("#" + newID, ifrmObj).attr("isGroup", _isGroup);
    $("#" + newID, ifrmObj).attr("subTree", _subTree);
    $("#" + newID, ifrmObj).attr("key", tmpKey);            // 결재순서변경 시, key값이 변경될 수 있으므로, 지정취소를 대비해, 이전 키 값을 가지고 있음.
}
/**************************************************
결재라인 객체의 키값 가져오기 (appSeqDic/appSeqDicZQ/appSeqDicZC 키값 생성)
기본 키 값은 양식의 결재라인에 지정된 키를 기준으로 생성. 단, 결재순서 변경 시, 해당 값 변경됨.
***************************************************/
Common.GetKey = function (_objID) {                         //alert(_objID + ":" + _objID.substring(0, 2));

    var tmpKey = _objID;
    if ($("#" + _objID, ifrmObj).attr("isFlow") != null) {  // FLOWZA, FLOWZD, FLOWZH, FLOWZR
        tmpKey = "FLOW" + _objID.substring(0, 2);
    }

    if ($("#" + _objID, ifrmObj).attr("idx") != null) {  // ZH01, ZC01, ZP01, ZQ01
        tmpKey = _objID.substring(0, 2);
    }

    if ($("#" + _objID, ifrmObj).attr("btag") != undefined) {   //ZQ01_ZD1, ZQ01_ZD2 : 수신고정태그처리 2016-01-26
        tmpKey = $("#" + _objID, ifrmObj).attr("btag") + "_" + _objID;
    }

    if (formSeqDic != null) {
        tmpKey = (formSeqDic.contains(tmpKey)) ? formSeqDic.get(tmpKey) + "_" + _objID : "0_" + _objID;     // 1_ZD0 , 0_ZP01, 3_ZA1
    }

    return tmpKey;
}

Common.GetSeqInfo = function (key, _approvalTag, _userID, _userName, _deptID, _deptName, _titleID, _titleName, _dutyID, _dutyName, _approvalType, _isGroup, _subTree) {
    var rtnval = { 'key': key                   // key (지정취소 시, 해당 값 사용) 
                    , 'approvalTag': _approvalTag   // "ZD0"
                    , 'userID': _userID             // "59"
                    , 'name': _userName             // "신현숙"
                    , 'deptID': _deptID             // "38"
                    , 'deptName': _deptName         // "솔루션개발팀"
                    , 'titleID': _titleID           // "15"
                    , 'titleName': _titleName       // "과장"
                    , 'dutyID': _dutyID             // "41"
                    , 'dutyName': _dutyName         // "파트장"
                    , 'approvalType': _approvalType // "S"
                    , 'isGroup': _isGroup           // "M"
                    , 'subTree': _subTree           // "0"
    };
    return rtnval;
}
/**************************************************
결재라인 객체에서 추가
***************************************************/
Common.AddKey = function (_tagID, _key, _val) {               // 결재라인 객체에 값 할당. (각 그룹별로 - 발신/수신/참조)

    switch (_tagID.substring(0, 2).toUpperCase()) {
        case "ZP":
            appSeqDicZQ.add(_key, _val);                // 수신결재라인
            break;
        case "ZQ":
            appSeqDicZQ.add(_key, _val);                // 수신결재라인
            break;
        case "ZC":
            appSeqDicZC.add(_key, _val);                // 참조결재라인
            break;
        case "ZR":
            appSeqDicZR.add(_key, _val);                // 공람결재라인
            break;
        default:
            appSeqDic.add(_key, _val);                  // 발신결재라인
            //appSeqKeyPush.push(_key);                   // 선택한 순서로 결재진행 표시 ByFreeman 20131023
            break;
    }
}
/**************************************************
결재라인 객체에서 삭제
***************************************************/
Common.DelKey = function (_tagID) {               // 결재라인 객체에 값 삭제. (각 그룹별로 - 발신/수신/참조)

    var tmpKey = null;
    switch (_tagID.substring(0, 2).toUpperCase()) {
        case "ZP":
            tmpKey = Common.GetKey(_tagID);             // 수신결재라인
            appSeqDicZQ.remove(tmpKey);
            break;
        case "ZQ":
            tmpKey = Common.GetKey(_tagID);             // 수신결재라인
            appSeqDicZQ.remove(tmpKey);
            break;
        case "ZC":
            tmpKey = Common.GetKey(_tagID);             // 참조결재라인
            appSeqDicZC.remove(tmpKey);
            break;
        case "ZR":
            tmpKey = Common.GetKey(_tagID);             // 공람결재라인
            appSeqDicZR.remove(tmpKey);
            break;
        default:
            if (appSeqKeyHistoryCnt > 0) {                              //결재순서변경 이후, 지정취소시, 호출 (결재순서변경 페이지에서 해당값 할당)
                var beforeKey = $("#" + nowEl.id, ifrmObj).attr("key");
                tmpKey = appSeqKeyHistory[beforeKey];
            } else {
                tmpKey = Common.GetKey(nowEl.id);
            }
            appSeqDic.remove(tmpKey);

            break;
    }
}

/**************************************************
지정취소 - 팝업
***************************************************/
var oContextMenuCtrl = null;
Common.popUpCancel = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl(e.clientX, e.clientY);
    oContextMenuCtrl.init();

    var oItem = oContextMenuCtrl.addItem(Tit_Cancel, Common.popupCancel_onclick, "");     //.addItem = function(sTxt, eventHandler, sImgUrl, subItem)
    oContextMenuCtrl.show(e, null, e.clientX * TmpSize, (e.clientY * TmpSize) + DHeight);
}
//지정취소
Common.popupCancel_onclick = function () {
    Common.DelSeq();
    oContextMenuCtrl.hide();
    $("#zeoEditorDIV").hide();
}
Common.DelSeq = function () {
    Common.DelKey(nowEl.id);       // 결재라인 객체에서 삭제

    if (nowEl.getAttribute("idx")) {
        // 다중태그인 경우, 객체삭제.
        $(nowEl, ifrmForm).remove(); nowEl = null;
    } else {
        // 고정태그인 경우, 객체초기화.
        nowEl.removeAttribute("IsAsign");
        nowEl.removeAttribute("userID");
        nowEl.removeAttribute("deptID");
        nowEl.removeAttribute("deptName");
        nowEl.removeAttribute("titleID");
        nowEl.removeAttribute("titleName");
        nowEl.removeAttribute("dutyID");
        nowEl.removeAttribute("dutyName");
        nowEl.removeAttribute("isGroup");
        nowEl.removeAttribute("subTree");
        if (nowEl.getAttribute("isapp") != undefined) { nowEl.removeAttribute("isapp"); }
        if (nowEl.getAttribute("btag") != undefined) { nowEl.removeAttribute("btag"); }
        if (nowEl.getAttribute("DefaultText")) { nowEl.innerHTML = nowEl.getAttribute("DefaultText"); }
    }
}

/**************************************************
문서번호 - 팝업 (보존년한 1,3,5,영구)
***************************************************/
Common.popUpLife = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl(e.clientX, e.clientY);
    oContextMenuCtrl.init();

    var oItem = oContextMenuCtrl.addItem(Tit_Life1, Common.popupLife_onclick, "");     //.addItem = function(sTxt, eventHandler, sImgUrl, subItem)
    oItem.txt.setAttribute("arg", Tit_Life1, 0);
    oItem = oContextMenuCtrl.addItem(Tit_Life3, Common.popupLife_onclick, "");
    oItem.txt.setAttribute("arg", Tit_Life3, 0);
    oItem = oContextMenuCtrl.addItem(Tit_Life5, Common.popupLife_onclick, "");
    oItem.txt.setAttribute("arg", Tit_Life5, 0);
    oItem = oContextMenuCtrl.addItem(Tit_LifeYear, Common.popupLife_onclick, "");
    oItem.txt.setAttribute("arg", Tit_LifeYear, 0);

    oContextMenuCtrl.show(e, null, e.clientX * TmpSize, (e.clientY * TmpSize) + DHeight);
}
//문서번호
Common.popupLife_onclick = function (e) {
    var evt = new __event(e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        $("#LIFE", ifrmObj).val(arg);
    }
    oContextMenuCtrl.hide();
    $("#zeoEditorDIV").hide();
}
/**************************************************
자주지정하는결재자 - 팝업          :   'userID': data.d[i].ApprovalUserID , 'name': data.d[i].ApprovalUserName , 'deptID': data.d[i].ApprovalUserDeptID , 'deptName': data.d[i].ApprovalUserDeptName , 'titleID': data.d[i].ApprovalUserTitleID , 'titleName': data.d[i].ApprovalUserTitleName , 'dutyID': data.d[i].ApprovalUserDutyID , 'dutyName': data.d[i].ApprovalUserDutyName
***************************************************/
Common.popUpFavList = function (e) {
    if (favDic != null) {

        /*
        if ($("#EDITOR")) {
        $("#zeoEditorDIV").css("width", "300px");
        $("#zeoEditorDIV").css("height", "150px");
        $("#zeoEditorDIV").css("top", e.clientY + 50 + "px");
        $("#zeoEditorDIV").css("left", e.clientX + "px");

        $("#zeoEditorIfrm").css("width", "300px");
        $("#zeoEditorIfrm").css("height", "150px");
        $("#zeoEditorIfrm").css("top", e.clientY + 50 + "px");
        $("#zeoEditorIfrm").css("left", e.clientX + "px");

        $("#zeoEditorDIV").show();
        }
        */

        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

        oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
        oContextMenuCtrl.init();

        var itxt = "";
        var items = favDic.items;

        for (var o in items) {

            // 참조(ZC)
            if (/Z{1}[C][0-9]?[0-9]?$/.test(nowEl.id)) {

                //참조일때는 부서+사용자 모두
                itxt = items[o].deptName + " " + items[o].titleName + " (" + items[o].dutyName + ") " + items[o].name;

                //부서의 경우, 예외처리
                if (items[o].isDept == "G") { itxt = (items[o].isSub == "0") ? items[o].deptName : items[o].deptName + R_TitSub; }

                var oItem = oContextMenuCtrl.addItem(itxt, Common.popUpFavList_onclick, "");
                oItem.txt.setAttribute("deptID", items[o].deptID, 0); oItem.txt.setAttribute("deptName", items[o].deptName, 0);
                oItem.txt.setAttribute("titleID", items[o].titleID, 0); oItem.txt.setAttribute("titleName", items[o].titleName, 0);
                oItem.txt.setAttribute("dutyID", items[o].dutyID, 0); oItem.txt.setAttribute("dutyName", items[o].dutyName, 0);
                oItem.txt.setAttribute("userID", items[o].userID, 0); oItem.txt.setAttribute("name", items[o].name, 0);
                oItem.txt.setAttribute("isDept", items[o].isDept, 0); oItem.txt.setAttribute("isSub", items[o].isSub, 0);

            } else {
                //참조아닐때 사용자만
                if (items[o].isDept == "M") {
                    itxt = items[o].deptName + " " + items[o].titleName + " (" + items[o].dutyName + ") " + items[o].name;

                    var oItem = oContextMenuCtrl.addItem(itxt, Common.popUpFavList_onclick, "");
                    oItem.txt.setAttribute("deptID", items[o].deptID, 0); oItem.txt.setAttribute("deptName", items[o].deptName, 0);
                    oItem.txt.setAttribute("titleID", items[o].titleID, 0); oItem.txt.setAttribute("titleName", items[o].titleName, 0);
                    oItem.txt.setAttribute("dutyID", items[o].dutyID, 0); oItem.txt.setAttribute("dutyName", items[o].dutyName, 0);
                    oItem.txt.setAttribute("userID", items[o].userID, 0); oItem.txt.setAttribute("name", items[o].name, 0);
                    oItem.txt.setAttribute("isDept", items[o].isDept, 0); oItem.txt.setAttribute("isSub", items[o].isSub, 0);
                }
            }
        }

        // 20140618. 자주지정하는결재자리스트가 많은 경우, 에디터뒤로 숨는 현상->스크롤처리.
        // 사이트별로, 양식포맷마다, height값 변경될수 있음.
        if (items.length > 0) {
            $(oContextMenuCtrl.oEl).attr("style", "height:100px;overflow:auto;");
            oContextMenuCtrl.show(e, null, e.clientX * TmpSize, (e.clientY * TmpSize) + DHeight);
        }
        //        }
    }
}

//자주지정하는결재자 (Non-IE용 팝업)
Common.popUpFavList_onclick = function (e) {
    var evt = new __event(e);
    if (evt.target) {
        var deptID = evt.target.getAttribute("deptID"); var deptName = evt.target.getAttribute("deptName");
        var titleID = evt.target.getAttribute("titleID"); var titleName = evt.target.getAttribute("titleName");
        var dutyID = evt.target.getAttribute("dutyID"); var dutyName = evt.target.getAttribute("dutyName");
        var userID = evt.target.getAttribute("userID"); var name = evt.target.getAttribute("name");
        var isDept = evt.target.getAttribute("isDept"); var isSub = evt.target.getAttribute("isSub");

        Common.SetApproval(nowEl.id, userID, name, deptID, deptName, titleID, titleName, dutyID, dutyName, isDept, isSub, "");
    }
    oContextMenuCtrl.hide();
    $("#zeoEditorDIV").hide();
}

/**************************************************
유동결재망 - 팝업
***************************************************/
Common.popUpFlow = function (e) {

    // 유동결재망(ZA)에서 기결재 체크.
    if (ApprovalTag != "ZD0") {     //ApprovalTag (Add+Mod=ZD0 / ModContent=ApprovalTag
        //  유동결재망ZA이고, ZA결재가 모두 이루어진 후 (= ZA결재가 현재 결재보다 이전순서인 경우)
        if (nowEl.id == "FLOWZA") {
            var flowCnt = $('td[type=body]', nowEl).length;
            var imgCnt = $('td:has(span img)', nowEl).length;

            if (flowCnt == 0 && imgCnt == 0) {

            }
            else {
                if (flowCnt == imgCnt) return;
            }
        }
    }

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl(e.clientX, e.clientY);
    oContextMenuCtrl.init();

    var oItem = oContextMenuCtrl.addItem(Tit_FlowAdd, Common.popUpFlow_onclick, "");     //.addItem = function(sTxt, eventHandler, sImgUrl, subItem)
    oItem.txt.setAttribute("arg", "ADD", 0);
    var oItem = oContextMenuCtrl.addItem(Tit_FlowDel, Common.popUpFlow_onclick, "");     //.addItem = function(sTxt, eventHandler, sImgUrl, subItem)
    oItem.txt.setAttribute("arg", "DEL", 0);

    oContextMenuCtrl.show(e, null, e.clientX * TmpSize, (e.clientY * TmpSize) + DHeight);
}
// ---------------------------유동결재망
Common.popUpFlow_onclick = function (e) {

    var evt = new __event(e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        if (arg == "ADD") {
            CommonFlow.Add(1, nowEl);
        } else {
            CommonFlow.Remove(nowEl);
        }
    }
    oContextMenuCtrl.hide();
    $("#zeoEditorDIV").hide();
}
/**************************************************
유동결재망 - 유동결재망 사용여부 및 max지정값 가져오기
***************************************************/
Common.GetFlowInfo = function () {
    var rtnval = "";
    if ($("#FLOWZD", ifrmObj).length > 0) {
        rtnval += "FLOWZD:" + CommonFlow.countZD + ",";
    }
    if ($("#FLOWZH", ifrmObj).length > 0) {
        rtnval += "FLOWZH:" + (CommonFlow.countZH - 1) + ",";     // why) ZH,ZA,ZR은 '1'부터 시작
    }
    if ($("#FLOWZA", ifrmObj).length > 0) {
        rtnval += "FLOWZA:" + (CommonFlow.countZA - 1) + ",";
    }
    if ($("#FLOWZR", ifrmObj).length > 0) {
        rtnval += "FLOWZR:" + (CommonFlow.countZR - 1) + ",";
    }
    return rtnval;
}

//유동결재망초기화 (상신&수정후재상신&본문내용수정&임시저장문서상신)
Common.SetDefault = function () {
    var flowObj = $("#FLOWZD,#FLOWZA,#FLOWZH,#FLOWZR", ifrmObj);
    if (flowObj.length > 0) {
        $.each(flowObj, function (i, o) {
            if ($("#" + o.id + " table", ifrmObj).length == 0) {
                //o.innerHTML = "";     //태그초기화
                o.innerHTML = "&nbsp;"; //유동결재망 높이
            }
        });

        CommonFlow.Init(null, null);            //유동결재 (결재망변경 시, null대신 변경결재망html을 설정함.= 추후, 옵션작업으로 하면 좋을듯.)
    }
}
//유동결재망 기존정보를 통한 초기화 (상신&수정후재상신)
Common.SetFlowInfo = function (_isUseFlow) {
    if (_isUseFlow == "") return;

    var arrObj = _isUseFlow.split(",");
    var cnt = 0;
    var tmpObj = null;

    $.each(arrObj, function (i, o) {       //Common.SetFlowInfo("FLOWZD:1");     
        tmpObj = $("#" + o.split(":")[0], ifrmObj)[0];
        cnt = parseInt(o.split(":")[1]);
        if (cnt != 0) {
            CommonFlow.Add(cnt, tmpObj);
        }
    });
}
//유동결재망 기존정보를 통한 초기화 (본문내용수정&임시저장문서상신)
Common.SetFlowInfoModCotent = function (_isUseFlow) {
    if (_isUseFlow == "") return;

    CommonFlow.Init(null, null);
    var arrObj = _isUseFlow.split(",");
    $.each(arrObj, function (i, o) {     //alert(i + ":" + o)
        switch (o.split(":")[0].toUpperCase()) {
            case "FLOWZD":
                CommonFlow.countZD = parseInt(o.split(":")[1]); break;
            case "FLOWZH":
                CommonFlow.countZH = parseInt(o.split(":")[1]) + 1; break;
            case "FLOWZA":
                CommonFlow.countZA = parseInt(o.split(":")[1]) + 1; break;
            case "FLOWZR":
                CommonFlow.countZR = parseInt(o.split(":")[1]) + 1; break;
        }
    });
}
/**************************************************
연동모듈체크
***************************************************/
Common.GetLegacy = function (_path) {
    if (_path == "") return;

    //alert(ifrmForm.document.readyState)
    _path += (_path.indexOf("?") < 0) ? "?" : "&";
    _path += "seq=" + Guids();
    _path += "&legacyKey=" + LegacyKey + "&parametc=" + ParamEtc;

    ifrmProcessing.document.location.href = _path;
}

/**************************************************
결재라인 자동정렬
***************************************************/
Common.GetSeqXML = function (_objArr, _objHash, _objHashZR) {

    var rtnxml = "<ITEMS>";

    // 발신 (ZD,ZA,ZH)
    if (_objArr.length > 0 && _objHash.size() > 0) {
        $.each(_objArr, function (i, o) {
            var obj = _objHash.get(o);

            var deptName = obj.deptName;
            deptName = deptName.replace(/&/g, '＆');
            deptName = deptName.replace(/</g, '〈');
            deptName = deptName.replace(/>/g, '〉');

            rtnxml += "<ITEM CLASS='" + obj.approvalTag.substr(0, 2) + "'>";
            rtnxml += "<APPROVALTAG>" + obj.approvalTag + "</APPROVALTAG>";
            rtnxml += "<USERID>" + obj.userID + "</USERID>";
            rtnxml += "<NAME>" + obj.name + "</NAME>";
            rtnxml += "<DEPTID>" + obj.deptID + "</DEPTID>";
            rtnxml += "<DEPTNAME>" + deptName + "</DEPTNAME>";
            rtnxml += "<TITLEID>" + obj.titleID + "</TITLEID>";
            rtnxml += "<TITLENAME>" + obj.titleName + "</TITLENAME>";
            rtnxml += "<DUTYID>" + obj.titleID + "</DUTYID>";
            rtnxml += "<DUTYNAME>" + obj.titleName + "</DUTYNAME>";
            rtnxml += "<APPROVALTYPE>" + obj.approvalType + "</APPROVALTYPE>";
            rtnxml += "<ISGROUP>" + obj.isGroup + "</ISGROUP>";
            rtnxml += "<SUBTREE>" + obj.subTree + "</SUBTREE>";
            rtnxml += "</ITEM>";
        });
    }
    // 공람 (ZR)
    if (_objHashZR.size() > 0) {

        var arrTagKey = Common.GetAppSeqQueue(_objHashZR);
        $.each(arrTagKey, function (i, o) {
            var obj = _objHashZR.get(o);

            var deptName = obj.deptName;
            deptName = deptName.replace(/&/g, '＆');
            deptName = deptName.replace(/</g, '〈');
            deptName = deptName.replace(/>/g, '〉');

            rtnxml += "<ITEM CLASS='" + obj.approvalTag.substr(0, 2) + "'>";
            rtnxml += "<APPROVALTAG>" + obj.approvalTag + "</APPROVALTAG>";
            rtnxml += "<USERID>" + obj.userID + "</USERID>";
            rtnxml += "<NAME>" + obj.name + "</NAME>";
            rtnxml += "<DEPTID>" + obj.deptID + "</DEPTID>";
            rtnxml += "<DEPTNAME>" + deptName + "</DEPTNAME>";
            rtnxml += "<TITLEID>" + obj.titleID + "</TITLEID>";
            rtnxml += "<TITLENAME>" + obj.titleName + "</TITLENAME>";
            rtnxml += "<DUTYID>" + obj.titleID + "</DUTYID>";
            rtnxml += "<DUTYNAME>" + obj.titleName + "</DUTYNAME>";
            rtnxml += "<APPROVALTYPE>" + obj.approvalType + "</APPROVALTYPE>";
            rtnxml += "<ISGROUP>" + obj.isGroup + "</ISGROUP>";
            rtnxml += "<SUBTREE>" + obj.subTree + "</SUBTREE>";
            rtnxml += "</ITEM>";
        });
    }

    rtnxml += "</ITEMS>";
    return rtnxml;
}
Common.GetSeqXMLZQZC = function (_objHash) {

    var rtnxml = "<ITEMS>";
    if (_objHash.size() > 0) {

        var arrTagKey = Common.GetAppSeqQueue(_objHash);

        $.each(arrTagKey, function (i, o) {
            var obj = _objHash.get(o);

            var deptName = obj.deptName;
            deptName = deptName.replace(/&/g, '＆');
            deptName = deptName.replace(/</g, '〈');
            deptName = deptName.replace(/>/g, '〉');

            rtnxml += "<ITEM CLASS='" + obj.approvalTag.substr(0, 2) + "'>";
            rtnxml += "<APPROVALTAG>" + obj.approvalTag + "</APPROVALTAG>";
            rtnxml += "<USERID>" + obj.userID + "</USERID>";
            rtnxml += "<NAME>" + obj.name + "</NAME>";
            rtnxml += "<DEPTID>" + obj.deptID + "</DEPTID>";
            rtnxml += "<DEPTNAME>" + deptName + "</DEPTNAME>";
            rtnxml += "<TITLEID>" + obj.titleID + "</TITLEID>";
            rtnxml += "<TITLENAME>" + obj.titleName + "</TITLENAME>";
            rtnxml += "<DUTYID>" + obj.titleID + "</DUTYID>";
            rtnxml += "<DUTYNAME>" + obj.titleName + "</DUTYNAME>";
            rtnxml += "<APPROVALTYPE>" + obj.approvalType + "</APPROVALTYPE>";
            rtnxml += "<ISGROUP>" + obj.isGroup + "</ISGROUP>";
            rtnxml += "<SUBTREE>" + obj.subTree + "</SUBTREE>";
            rtnxml += "</ITEM>";
        });
    }
    rtnxml += "</ITEMS>";
    return rtnxml;
}
/**************************************************
결재선 정렬
***************************************************/
Common.GetAppSeqQueueNoSort = function (_objHash) {

    var rtnval = new Array();
    var i = 0;
    for (var o in _objHash.items) {
        rtnval[i] = o; i++;
    }
    //rtnval.sort(CompareForSort);
    return rtnval;
}

Common.GetAppSeqQueue = function (_objHash) {

    var rtnval = new Array();
    var i = 0;
    for (var o in _objHash.items) {
        rtnval[i] = o; i++;
    }
    rtnval.sort(CompareForSort);
    return rtnval;
}
this.CompareForSort = function (param1, param2) {
    var first = parseInt(param1);
    var second = parseInt(param2);

    if (first == second) return 0;
    if (first < second)
        return -1;
    else
        return 1;
}
/**************************************************
빈결재란 및 텍스트노드(다중) 처리
***************************************************/
Common.SetDefaultTags = function () {
    var zTags = $("span", ifrmObj);

    var arrSel = $(".ZDSel, .ZPSel, .ZQSel, .ZHSel, .ZCSel, .ZRSel, .ZASel", ifrmObj);
    $.each(arrSel, function (i, o) {
        o.className = o.className.replace("Sel", "");
    });

    // 불필요한 텍스트 노드 삭제 (ZH/ZC/ZP/ZQ/FLOWZD/FLOWZH/FLOWZA)
    var objRemove = Array(); var k = 0;
    var objArr = $("#ZH,#ZC,#ZP,#ZQ,#FLOWZD,#FLOWZH,#FLOWZA,#FLOWZR", ifrmObj);
    $.each(objArr, function (i, o) {
        $.each(o.childNodes, function (c, d) {
            if (d.nodeType == 3) { objRemove[k] = d; k++; }
        });
    });
    $.each(objRemove, function (i, o) {
        $(o, ifrmForm).remove();   //o.removeNode(true);
    });
    // 빈결재란 처리 (고정결재망)
    $.each(zTags, function (i, o) {
        if (o.id != "") {
            if (/Z{1}[D|H|C|P|Q|A|R][0-9]?[0-9]?[0-9]?$/.test(o.id)) {
                if (!$("#" + o.id, ifrmObj).attr("IsAsign") && $("#" + o.id, ifrmObj).children('span').length == 0) {
                    o.innerHTML = (/Z{1}[D|H|R|A][0-9]$/.test(o.id)) ? "/" : "";
                }
            }
        }
    });
}
/**************************************************
에디터 처리 (+ 에디터 스타일 체크)
***************************************************/
Common.CheckStyle = function (_nonActiveX) {

    var rtnval = true;

    if ($("#EDITOR", ifrmObj).length < 1) return rtnval;

    // 문서내용 (에디터+이미지)
    if (_nonActiveX != "T") {

        var tmptwe = $("#twe", ifrmObj.ifrmTFEditor.document);
        if (tmptwe == null) return rtnval;

        var dom = tmptwe[0].GetDOM();
        if (dom) {
            if (dom.styleSheets.length > 0 && $(".WordSection1", dom).length > 0) {
                /*  office에서 사용하는 스타일 제거. (why.특정PC에서 결재에러발생)
                첫번째 파라미터 : 제거할 태그이름 
                두번째 파라미터 : 태그를 제거할 때 하위 태그도 삭제 여부
                세번째 파라미터 : css 스타일 제거 여부
                네번째 파라미터 : 비어있는 태그 제거 여부
                다섯번째 파라미터 : p태그를 br태그로 변환 여부
                여섯번째 파라미터 : Meta태그의 제거 여부
                일곱번째 파라미터 : MS Office 설정 및 스타일 제거 여부
                */
                tmptwe[0].TagDiet("", false, false, false, false, true, true);
                rtnval = false;
            }
        }
    }
    return rtnval;
}

/**************************************************
에디터 처리 (+ 이미지 업로드 포함)
***************************************************/
Common.SetEditorTags = function () {

    if ($("#EDITOR", ifrmObj).length < 1) return;

    var objEditor = ifrmObj.getElementById("ifrmTFEditor").contentWindow.xfe;
    $("#htaEditorHTML").val(objEditor.getHtmlValue());

    $("#ifrmTFEditor", ifrmObj).replaceWith("<ZEOEDITOR></ZEOEDITOR>");
}

/**************************************************
양식관련 기본정보 처리 (회사명COMPANY,작성일DATE,상신부서UPDPT,상신자직급UPJC,상신자명UPNM,상신자사번EMPNUM)
***************************************************/
Common.SetDefaultInfo = function () {

    // 서버시간 체크
    Common.GetServerTime();

    if ($("#COMPANY", ifrmObj).length > 0) {
        $("#COMPANY", ifrmObj).val(CompanyName);
    }
    if ($("#DATE", ifrmObj).length > 0) {
        $("#DATE", ifrmObj).val(todayObj);          //Common.SetStamp 에서 이미 할당됨.
    }
    if ($("#UPDPT", ifrmObj).length > 0) {
        $("#UPDPT", ifrmObj).val(DeptName);
    }
    if ($("#UPJC", ifrmObj).length > 0) {
        $("#UPJC", ifrmObj).val(TitleName);
    }
    if ($("#UPNM", ifrmObj).length > 0) {
        $("#UPNM", ifrmObj).val(UserName);
    }
    if ($("#EMPNUM ", ifrmObj).length > 0) {
        $("#EMPNUM ", ifrmObj).val(EmployeeNumber)
    }
    if ($("#NO", ifrmObj).length > 0) {
        $("#NO", ifrmObj).val("")
    }

    // 문서보존년한 기본값 : 1년
    if ($("#LIFE", ifrmObj).length > 0) {
        if ($("#LIFE", ifrmObj).val() == "") {
            $("#LIFE", ifrmObj).val(Tit_Life1);
        }
    }
}

/**************************************************
승인 인장처리
***************************************************/
var todayObj = "", timeObj = "", MonthDayObj = "", timeExObj = "";
Common.SetStamp = function (_tagID, _sealpath, _userName, _deptName, _titleName, _dutyName, _proxyUserID) {

    if (nowEl) {
        // 편집항목이 결재자였다면 클래스명 변경
        if (nowEl.id.indexOf("Z") == 0 || nowEl.id.indexOf("FLOW") == 0) { nowEl.className = nowEl.className.replace("Sel", ""); }
    }

    // 승인자 인장처리
    if ($("#" + _tagID, ifrmObj).length > 0) {
        var imgObj = "<img src='" + _sealpath + "' style='width:50px;height:50px'>";
        $("#" + _tagID, ifrmObj).html(imgObj);

        $("#" + _tagID, ifrmObj).attr("isapp", "true");
    }

    // 대결자 체크
    var msgProxy = "";
    if (_proxyUserID != "0") {
        msgProxy = R_PROXY;
    } else {
        msgProxy = "";
    }

    // 서버시간 체크
    Common.GetServerTime();

    // 결재란처리 (0~15)
    var brTag = " ";    //  <br>
    if ($("#" + _tagID + "_INFO0", ifrmObj).length > 0) {    //결재일
        $("#" + _tagID + "_INFO0", ifrmObj).html(todayObj);
    }
    if ($("#" + _tagID + "_INFO1", ifrmObj).length > 0) {    //직급명 + 이름
        $("#" + _tagID + "_INFO1", ifrmObj).html(msgProxy + _titleName + brTag + _userName);
    }
    if ($("#" + _tagID + "_INFO2", ifrmObj).length > 0) {    //직급명 + 이름 + 결재일
        $("#" + _tagID + "_INFO2", ifrmObj).html(msgProxy + _titleName + brTag + _userName + brTag + todayObj);
    }
    if ($("#" + _tagID + "_INFO3", ifrmObj).length > 0) {    //이름
        $("#" + _tagID + "_INFO3", ifrmObj).html(msgProxy + _userName);
    }
    if ($("#" + _tagID + "_INFO4", ifrmObj).length > 0) {    //직급명
        $("#" + _tagID + "_INFO4", ifrmObj).html(msgProxy + _titleName);
    }
    if ($("#" + _tagID + "_INFO5", ifrmObj).length > 0) {    //부서명
        $("#" + _tagID + "_INFO5", ifrmObj).html(msgProxy + _deptName);
    }
    if ($("#" + _tagID + "_INFO6", ifrmObj).length > 0) {    //이름 + 결재일
        $("#" + _tagID + "_INFO6", ifrmObj).html(msgProxy + _userName + brTag + todayObj);
    }
    if ($("#" + _tagID + "_INFO7", ifrmObj).length > 0) {    //부서명 + 직급명 + 이름 + 결재일
        $("#" + _tagID + "_INFO7", ifrmObj).html(msgProxy + _deptName + brTag + _titleName + brTag + _userName + brTag + todayObj);
    }
    if ($("#" + _tagID + "_INFO8", ifrmObj).length > 0) {    //결재일,시간까지
        $("#" + _tagID + "_INFO8", ifrmObj).html(timeObj);
    }
    if ($("#" + _tagID + "_INFO9", ifrmObj).length > 0) {    //직책명
        $("#" + _tagID + "_INFO9", ifrmObj).html(msgProxy + _dutyName);
    }
    if ($("#" + _tagID + "_INFO10", ifrmObj).length > 0) {    //직책명 + 이름
        $("#" + _tagID + "_INFO10", ifrmObj).html(msgProxy + _dutyName + brTag + _userName);
    }
    if ($("#" + _tagID + "_INFO11", ifrmObj).length > 0) {    //직책명 + 이름+ 결재일
        $("#" + _tagID + "_INFO11", ifrmObj).html(msgProxy + _dutyName + brTag + _userName + brTag + todayObj);
    }
    if ($("#" + _tagID + "_INFO12", ifrmObj).length > 0) {    //부서명 + 직책명 + 이름 + 결재일
        $("#" + _tagID + "_INFO12", ifrmObj).html(msgProxy + _deptName + brTag + _dutyName + brTag + _userName + brTag + todayObj);
    }
    if ($("#" + _tagID + "_INFO13", ifrmObj).length > 0) {    //이름 + 결재일 + 시간까지.
        $("#" + _tagID + "_INFO13", ifrmObj).html(msgProxy + _userName + brTag + timeObj);
    }
    if ($("#" + _tagID + "_INFO14", ifrmObj).length > 0) {    //결재일(월일)
        $("#" + _tagID + "_INFO14", ifrmObj).html(msgProxy + MonthDayObj);
    }
    if ($("#" + _tagID + "_INFO15", ifrmObj).length > 0) {    //시간
        $("#" + _tagID + "_INFO15", ifrmObj).html(msgProxy + timeExObj);
    }
}

/**************************************************
자주지정하는결재자 가져오기
***************************************************/
Common.GetFavoriteApp = function () {
    var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/GetFavoriteApp";
    ParamArray = { 'p': 'p' };
    var obj = eval("Common.SuccessFavoriteApp");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Common.SuccessFavoriteApp = function (data, status) {
    if (data != null) {
        var rtnval = {};
        $.each(data.d, function (i, o) {
            rtnval = {
                'userID': data.d[i].ApprovalUserID
                , 'name': data.d[i].ApprovalUserName
                , 'deptID': data.d[i].ApprovalUserDeptID
                , 'deptName': data.d[i].ApprovalUserDeptName
                , 'titleID': data.d[i].ApprovalUserTitleID
                , 'titleName': data.d[i].ApprovalUserTitleName
                , 'dutyID': data.d[i].ApprovalUserDutyID
                , 'dutyName': data.d[i].ApprovalUserDutyName
                , 'isDept': data.d[i].IsDept
                , 'isSub': data.d[i].IsSub
                , 'baseDept': data.d[i].BaseDept
                , 'baseDeptStr': data.d[i].BaseDeptStr
            };
            favDic.add(i, rtnval);  //favDic.add(data.d[i].FavoriteApprovalID, rtnval);
        });
    }
}

/**************************************************
양식결재라인 가져오기
***************************************************/
Common.GetFormSeq = function (_formID) {
    if (_formID == 0) return;

    var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/GetFormSeq";
    ParamArray = { 'formID': _formID };
    var obj = eval("Common.SuccessFormSeq");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Common.SuccessFormSeq = function (data, status) {

    if (data != null) {
        $.each(data.d, function (i, o) {
            formSeqDic.add(data.d[i].ApprovalTag, data.d[i].Sequence);      // (ZD0,1)  (ZD1,2)  (ZC,3)  (ZP,4)
        });
    }

}
/**************************************************
서버시간 가져오기
***************************************************/
Common.GetServerTime = function () {
    var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/GetServerTime";
    ParamArray = { 'p': 'p' };
    var obj = eval("Common.SuccessServerTime");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Common.SuccessServerTime = function (data, status) {
    if (data != null) {
        $.each(data.d, function (i, o) {
            switch (i) {
                case 0: todayObj = o; break;      //getToDayByServer
                case 1: timeObj = o; break;       //getTimeByServer
                case 2: MonthDayObj = o; break;   //getMonthDayByServer
                case 3: timeExObj = o; break;     //getTimeExByServer
            }
        });
    }
}
/**************************************************
작성(수정)로딩 시, 예외처리 (에디터로딩)
***************************************************/
Common.ExceptObj = function (_o) {
    if (_o == null) return;

    // 에디터처리    $("#EDITOR", ifrmObj)
    if (_o.id.toUpperCase() == "EDITOR") {
        var editorUrl = "/DeskPlusMobile/Approval/EditorNonActiveX.aspx";

        var oHtml = "<iframe id='ifrmTFEditor' name='ifrmTFEditor' src='" + editorUrl + "' frameborder='0' width='100%' height='100%' marginwidth='0' marginheight='0' scrolling='no'></IFRAME>";
        _o.innerHTML = oHtml;
    }
}


/**************************************************
겸직정보 수정 시, 관련정보(변수) 변경.
***************************************************/
Common.ModDeptInfo = function () {

    var deptVal = $("#ddlAdditionDept").val();     //38|솔루션개발팀|15|과장|41|파트장
    DeptID = deptVal.split("|")[0];
    DeptName = deptVal.split("|")[1];
    TitleID = deptVal.split("|")[2];
    TitleName = deptVal.split("|")[3];
    DutyID = deptVal.split("|")[4];
    DutyName = deptVal.split("|")[5];
    RootDeptID = deptVal.split("|")[6];
    RootDeptName = deptVal.split("|")[7];

    Common.SetDefaultInfo();    // 양식에 변경된 정보 설정.

    // 상신자지정.
    Common.SetApprovalZD("ZD0", UserID, UserName, DeptID, DeptName, TitleID, TitleName, DutyID, DutyName, "M", "0", "");

}


/**************************************************
첨언처리(결재의견)
***************************************************/
Common.SetOpinion = function (_userName, _titleName, _dutyName, _deptName, _txt) { //alert(_userName + "-" + _titleName + "-" + _dutyName + "-" + _deptName + "-" + _txt)

    if (_txt == "" || timeObj == "") return;

    var objTit = _userName + "/" + _titleName + "/" + _deptName;

    var objHtml = "<div class='app_opi'>";
    objHtml += "<span class='tit'>[" + objTit + "]</span>";
    objHtml += "<span class='date'>- " + timeObj + "</span>";
    objHtml += "<div class='cont'>" + _txt + "</div>";
    objHtml += "</div>";

    if ($("#OPINION", ifrmObj).length > 0) {    //alert($("#OPINION", ifrmObj).length)
        $("#OPINION", ifrmObj)[0].insertAdjacentHTML("beforeEnd", objHtml);
    }

    if ($("#OPINIONVIEW", ifrmObj).length > 0) {
        $("#OPINIONVIEW", ifrmObj).html(TagDic_OPINIONVIEW);        //결재의견붙임 태그처리.
    }
}


/**************************************************
Input Tag 처리
***************************************************/
Common.SetReadOnly = function () {
    var arrInfo = $(".formfieldinput", ifrmObj);
    $.each(arrInfo, function (i, o) {
        if (o.tagName.toUpperCase() == "INPUT") { o.setAttribute("readonly", "true"); }
    });
}

/**************************************************
특정태그처리 (for, chrome,ff,safari,ie문서모드9) : 
***************************************************/
Common.UpdateDOM = function () {    //alert(ifrmObj);

    if (ifrmObj == null) return;

    // 특정태그처리 (for, chrome,ff,safari, ie의 문서모드가 IE9표준인경우)
    var actObj = $("input,textarea,checkbox,select,radio", ifrmObj);
    $.each(actObj, function (i, o) {
        Common.ProcDOM(o);
    });

}
Common.ProcDOM = function (obj) {     // alert(typeof obj) //alert(obj.type.toLowerCase())
    // if the obj ID string has been passed in, get the obj object
    if (typeof obj == "string") { obj = $(obj, ifrmObj); }

    switch (obj.type.toLowerCase()) {
        case "select-one":
            for (var i = 0; i < obj.options.length; i++) {
                if (i == obj.selectedIndex) {
                    obj.options[obj.selectedIndex].setAttribute("selected", "selected");
                }
                else {
                    obj.options[i].removeAttribute("selected");
                }
            }
            break;
        case "select-multiple":
            for (var i = 0; i < obj.options.length; i++) {
                if (obj.options[i].selected) {
                    obj.options[i].setAttribute("selected", "selected");
                }
                else {
                    obj.options[i].removeAttribute("selected");
                }
            }
            break;
        case "text":
            obj.setAttribute("value", obj.value);
            break;
        case "textarea":
            obj.setAttribute("value", obj.value);
            obj.innerHTML = obj.value;
            break;
        case "checkbox":
            if (obj.checked) {
                obj.setAttribute("checked", "checked");
            } else {
                obj.removeAttribute("checked");
            }
            break;
        case "radio":
            var radioNames = ifrmObj.getElementsByName(obj.name);
            for (var i = 0; i < radioNames.length; i++) {
                if (radioNames[i].checked) {
                    radioNames[i].setAttribute("checked", "checked");
                } else {
                    radioNames[i].removeAttribute("checked");
                }
            }
            break;
        default:
            break;
    }
}
// 수정불가 처리
Common.SetInActive = function (_targetFrm) {  //alert(_targetFrm);
    if (_targetFrm == null) return;

    try {
        // xscript처리.
        var delObj = $(_targetFrm).contents().find('xscript');
        $.each(delObj, function (i, o) {
            o.innerHTML = "";
        });
    } catch (e) { }

    // input,textarea 제어 (readonly)
    $(_targetFrm).contents().find('input,textarea').attr('readonly', "true");
    $(_targetFrm).contents().find('input').attr('contenteditable', "false");
    $(_targetFrm).contents().find('div').attr('contenteditable', "false");
    // 그외객체제어 (disable) - input(button,image,submit,reset),select,checkbox, ...
    $(_targetFrm).contents().find('input:checkbox,input:button,input:image,input:submit,input:reset').attr('disabled', "true");
    $(_targetFrm).contents().find('select').attr('disabled', "true");

    // 그외객체제어 (disable-선택적) - input(radio)
    var actObj = $("input:radio", _targetFrm);
    $.each(actObj, function (i, o) {
        var subobj = _targetFrm.getElementsByName(o.name);
        for (var i = 0; i < subobj.length; i++) {
            if (!subobj[i].checked) {
                subobj[i].setAttribute("disabled", "true");
            }
        }
    });

    $(_targetFrm).contents().find('input:radio').attr('disabled', "true");
}

/**************************************************
결재자지정 (상신자 재지정)
***************************************************/
Common.SetApprovalZD = function (_tagID, _userID, _userName, _deptID, _deptName, _titleID, _titleName, _dutyID, _dutyName, _isGroup, _subTree, _userList) {

    // 발신,협조,합의합의,참조,수신(ZP),수신(ZQ)
    if (_tagID == undefined || _userID == undefined || _userName == undefined || _deptID == undefined || _deptName == undefined || _titleName == undefined || _dutyName == undefined) return;
    if (_tagID == "" || _deptID == "0" || _deptName == "0") { return; }     //alert(R_Msg_AlertUserInfo); 
    if ($("#" + _tagID, ifrmObj) == null) return;

    var tmpKey = Common.GetKey(_tagID);
    var tmpSeq = Common.GetSeqInfo(tmpKey, _tagID, _userID, _userName, _deptID, _deptName, _titleID, _titleName, _dutyID, _dutyName, "S", _isGroup, _subTree);

    appSeqDic.remove(tmpKey);
    appSeqDic.add(tmpKey, tmpSeq);                  // 발신결재라인

    // 결재자지정
    $("#" + _tagID, ifrmObj).html(_userName);
    $("#" + _tagID, ifrmObj).attr("IsAsign", "true");
    $("#" + _tagID, ifrmObj).attr("userID", _userID);
    $("#" + _tagID, ifrmObj).attr("deptID", _deptID);
    $("#" + _tagID, ifrmObj).attr("deptName", _deptName);
    $("#" + _tagID, ifrmObj).attr("titleID", _titleID);
    $("#" + _tagID, ifrmObj).attr("titleName", _titleName);
    $("#" + _tagID, ifrmObj).attr("dutyID", _dutyID);
    $("#" + _tagID, ifrmObj).attr("dutyName", _dutyName);
    $("#" + _tagID, ifrmObj).attr("isGroup", _isGroup);
    $("#" + _tagID, ifrmObj).attr("subTree", _subTree);
    $("#" + _tagID, ifrmObj).attr("key", tmpKey);            // 결재순서변경 시, key값이 변경될 수 있으므로, 지정취소를 대비해, 이전 키 값을 가지고 있음.
}

Common.popUpGroupList = function (_val) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    var oItem = oContextMenuCtrl.addItem(R_DeptSet, Common.popUpGroup_onclick, "");     //.addItem = function(sTxt, eventHandler, sImgUrl, subItem)
    oItem.txt.setAttribute("arg", _val, 0);
    //oContextMenuCtrl.show(window.event);
    var wevent = window.event;
    oContextMenuCtrl.show(wevent, null, wevent.clientX * TmpSize, 270);
}

Common.popUpGroup_onclick = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");

        //var isSubTree = (confirm(R_MSG_SubDept)) ? "1" : "0";
        var isSubTree = "0";

        action_popup.confirm(R_MSG_SubDept, function (res) {
            if (res) {
                isSubTree = "1";
            }
        })

        var tmpval = arg.split('|');

        Common.SetApproval(nowEl.id, 0, "", tmpval[1], tmpval[2], 0, "", 0, "", "G", isSubTree, "");
    }

    oContextMenuCtrl.hide();
    $("#zeoEditorDIV").hide();
}