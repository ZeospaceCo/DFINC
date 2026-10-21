var GetDocFlow = {}

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        GetDocFlow.Init();
    });
});

/**************************************************************
초기화
**************************************************************/
GetDocFlow.Init = function () {
    GetDocFlow.LoadDoc(DocumentID, true);
}

/**************************************************************
정보가져오기(문서,결재)
**************************************************************/
GetDocFlow.LoadDoc = function (_documentID, _isInit) {

    // 전역변수 초기화
    isApproveA = false; befApprovalType = ""; befApprovalTag = "";

    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/GetDocFlow.asmx/GetDocFlowList";
    ParamArray = {
        'documentID': _documentID,
        'isComplete': IsComplete
    };

    var obj = (_isInit) ? eval("GetDocFlow.SuccessInit") : eval("GetDocFlow.SuccessSub");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************************
초기로딩(발신)
**************************************************************/
GetDocFlow.SuccessInit = function (data, status) {

    if (data.d != null) {        //alert(data.d[0].SequenceList.length);

        var tmphtml = "<table cellpadding=0 cellspacing=0><tbody><tr>";
        //문서정보
        tmphtml += GetDocFlow.GetDocCont(data.d[0]);
        //발신결재(ZD/ZH/ZA)
        tmphtml += GetDocFlow.GetSeqInfo(data.d[0].SequenceList, data.d[0].ApprovalState);
        //수신결재(ZP/ZQ)
        tmphtml += GetDocFlow.GetCPInfo(data.d[0].CounterPartUserList);
        //참조결재(ZC)
        tmphtml += GetDocFlow.GetCCInfo(data.d[0].CarbonCopyUserList);

        tmphtml += "</tr></tbody></table>";

        $("#phFlow").html(tmphtml);

        // 수신문서(ZQ)가 있는 경우, 처리
        if (data.d[0].DocumentID != data.d[0].LastDocID) {
            var subhtml = "<img src='" + BaseImgPath + "/Icon/ico_item_minus.gif' alt='' /> <a href=\"javascript:GetDocFlow.GetSubDoc('" + data.d[0].LastDocID + "','" + data.d[0].UpPathID + "')\">" + R_ViewZPZQ + "</a>";
            $("#phFlowSubTit").html(subhtml);
        }
        else {
            // 수신문서(ZP)가 있는 경우, 처리
            if (IsZP != "0") {
                var subhtml = "<img src='" + BaseImgPath + "/Icon/ico_item_minus.gif' alt='' /> <a href=\"javascript:GetDocFlow.GetSubDocZP('" + data.d[0].DocumentID + "')\">" + R_ViewZPZQ + "</a>";
                $("#phFlowSubTit").html(subhtml);
            }
        }
    }
}

/**************************************************************
수신로딩(수신항목가져오기)
**************************************************************/
GetDocFlow.GetSubDoc = function (_lastDocID, _uppathID) {    //alert(_lastDocID + ":" + _uppathID);

    if ($("#phFlowSub")[0].style.display == "") {
        $("#phFlowSub")[0].style.display = "none";
        $(".apH_flow")[0].style.overflow = "auto";
        return;
    } else {
        $("#phFlowSub")[0].style.display = "";
        $("#phFlowSub").html("");
    }

    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/GetDocFlow.asmx/GetUpPath";
    ParamArray = {
        'uppathID': _uppathID,
        'lastDocID': _lastDocID,
        'isComplete': IsComplete
    };

    var obj = eval("GetDocFlow.SuccessSubDoc");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
/**************************************************************
수신로딩(수신항목가져오기)
**************************************************************/
GetDocFlow.GetSubDocZP = function (_documentID) {

    if ($("#phFlowSub")[0].style.display == "") {
        $("#phFlowSub")[0].style.display = "none";
        $(".apH_flow")[0].style.overflow = "auto";
        return;
    } else {
        $("#phFlowSub")[0].style.display = "";
        $("#phFlowSub").html("");
    }

    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/GetDocFlow.asmx/GetDocZPList";
    ParamArray = {
        'documentID': _documentID
    };

    var obj = eval("GetDocFlow.SuccessSubDoc");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
GetDocFlow.SuccessSubDoc = function (data, status) {
    if (data.d != null) {        //alert(data.d);
        var arrID = data.d.split(",");

        $.each(arrID, function (i, o) {      //alert(i + ":" + o);      
            GetDocFlow.LoadDoc(o, false);
        });
    }
}
//수신문서로딩
GetDocFlow.SuccessSub = function (data, status) {
    
    if (data.d != null) {        //alert(data.d[0].SequenceList.length);

        var tmphtml = "<div class='sub' style='border:1px solid red;padding-left:10px;'><table><tr>";
        //문서정보
        tmphtml += GetDocFlow.GetDocCont(data.d[0]);
        //발신결재(ZD/ZH/ZA)
        tmphtml += GetDocFlow.GetSeqInfo(data.d[0].SequenceList, data.d[0].ApprovalState);
        //참조결재(ZC)
        tmphtml += GetDocFlow.GetCCInfo(data.d[0].CarbonCopyUserList);

        tmphtml += "</tr></table></div>";

        $(tmphtml).appendTo($('.sub')[0]);
        $(".apH_flow")[0].style.overflow = "scroll";
    }
}


/**************************************************************
공통-문서
**************************************************************/
//문서내용-기본정보
GetDocFlow.GetDocCont = function (_obj) {
    if (_obj == null) return;

    var cls = GetDocFlow.GetDocStat(_obj.ApprovalState);
    var tmptitle = _obj.Number3;
    tmptitle += (parseInt(_obj.ReplyCnt) > 0) ? " <span class='app_point'>[" + _obj.ReplyCnt + "]</span>" : "";

    var tmphtml = "";
    tmphtml += "<td>";
    tmphtml += "<div class='doc'>";
    tmphtml += "<dl class='" + cls + "'>";          // ex) class='s1'
    tmphtml += "<dt>" + R_FormName + " :</dt><dd title='" + _obj.Number3 + "'>" + tmptitle + "</dd>";   // 양식
    tmphtml += "<dt>" + R_Subject + " :</dt><dd title='" + _obj.Subject + "'>" + _obj.Subject + "</dd>";    // 제목
    tmphtml += "<dt>" + R_WriteDate + " :</dt><dd>" + _obj.WriteTimeStr + "</dd>";                          //상신일 2012-04-05 16:03
    tmphtml += "</dl>";
    tmphtml += "<dl class='" + cls + "_S'>";        // ex) class='s1_S'
    tmphtml += "<dd class='s_txt'>" + GetDocFlow.GetDocStatStr(_obj.ApprovalState) + "</dd>";               // 결재상태정보
    tmphtml += "</dl>";
    tmphtml += "</div>";
    tmphtml += "</td>";

    return tmphtml;
}
//문서상태 : 클래스 정보 리턴
GetDocFlow.GetDocStat = function (_stat) { //alert(_val);
    if (_stat == "") return;

    var rtnval = "";
    switch (_stat.toUpperCase()) {
        case "H":
            rtnval = "s1"; break;
        case "W":
            rtnval = "s2"; break;
        case "I":
            rtnval = "s3"; break;
        case "C":
            rtnval = "s4"; break;
        case "R":
            rtnval = "s5"; break;
        case "D":
            rtnval = "s1"; break;
        default:
            rtnval = "s3"; break;
    }
    return rtnval;
}
GetDocFlow.GetDocStatStr = function (_stat) { //alert(_val);
    if (_stat == "") return;

    var rtnval = "";
    switch (_stat.toUpperCase()) {
        case "H":
            rtnval = R_InfoH; break;    //상신보류중
        case "W":
            rtnval = R_InfoW; break;    //대기중
        case "I":
            rtnval = R_InfoI; break;    //진행중
        case "C":
            rtnval = R_InfoC; break;    //완료
        case "R":
            rtnval = R_InfoR; break;    //반려
        case "D":
            rtnval = R_InfoD; break;    //보류중
        default:
            rtnval = R_InfoI; break;
    }
    return rtnval;
}

/**************************************************************
공통-결재라인
**************************************************************/

//결재태그정보
GetDocFlow.GetSeqTag = function (_obj) {
    var rtnval = "";

    if (_obj.ApprovalTag.toUpperCase() == "ZD0") {
        rtnval = R_Report;  //기안
    } else {
        switch (_obj.ApprovalTag.substring(0, 2).toUpperCase()) {
            case "ZD":
                rtnval = R_ZD; break;
            case "ZH":
                rtnval = R_ZH; break;
            case "ZA":
                rtnval = R_ZA; break;
            case "ZP":
                rtnval = R_Receive; break;
            case "ZQ":
                rtnval = R_Receive; break;
            case "ZC":
                rtnval = R_Carboncopy; break;
            case "ZR":
                rtnval = R_ZR; break;
            default:
                rtnval = ""; break;
        }
    }
    return rtnval;
}


/**************************************************************
공통-발신결재(ZD/ZH/ZA)
**************************************************************/

//발신결재(ZD/ZH/ZA) + 화살표
GetDocFlow.GetSeqInfo = function (_obj, _approvalState) {
    if (_obj == null) return;

    var tmphtml = "";
    $.each(_obj, function (i, o) {        //alert("|"+o.ApprovalType+"|");  alert(o.ApprovalTag +":"+ o.Proxy);
        if (i > 0) {
            tmphtml += GetDocFlow.GetArrow(o);
        }
        tmphtml += GetDocFlow.GetSeqUnit(o, _approvalState);        // 병렬합의처리필요
    });
    return tmphtml;
}
//화살표
var befApprovalType = "", befApprovalTag = "";
GetDocFlow.GetArrow = function (_obj) {    //alert(":" + _obj.ApprovalType + ":"+_obj.ApprovalTag); alert(_obj.ApprovalTag + ":" + _obj.Proxy);
    if (_obj == null) return;

    var tmphtml = "";
    if ((befApprovalTag.indexOf("ZA") < 0 || _obj.ApprovalTag.indexOf("ZA") < 0) && _obj.Proxy != "Y") {
        tmphtml += "<td>";
        tmphtml += "<img src='" + BaseImgPath + "/Approval/" + GetDocFlow.GetArrowStat(befApprovalType) + "' alt='' class='arr'/>";
        tmphtml += "</td>";
    }

    befApprovalTag = _obj.ApprovalTag;
    befApprovalType = _obj.ApprovalType;

    return tmphtml;
}
//화살표(결재상태)
GetDocFlow.GetArrowStat = function (_val) {

    /*
    var rtnval = "";
    switch (_val.toUpperCase()) {
    case "S":
    rtnval = "app_ico_arrowN.gif"; break;       //결재(진행)
    case "A":
    rtnval = "app_ico_arrowE.gif"; break;       //전결
    case "R":
    rtnval = "app_ico_arrowR.gif"; break;       //반려
    default:
    rtnval = "app_ico_arrowN.gif"; break;
    }
    return rtnval;
    */
    return "aparow.gif";
}
//발신결재단위
GetDocFlow.GetSeqUnit = function (_obj, _approvalState) {
    if (_obj == null) return;

    var tmphtml = "";
    var tmpcss = _obj.MyTurn;
    tmpcss += (_obj.ApprovalType == 'R') ? " s_re" : "";
    tmpcss += (_obj.MyTurn == 'Y' && _approvalState == "R") ? " s_retar" : "";

    if (_obj.Proxy == "Y") {
        tmphtml += "<td style='padding-right:0px;'>";   //패딩값조절????????????????????

        tmphtml += "<div class='step'>";
        tmphtml += "<dl class='" + tmpcss + "'>";
        tmphtml += "<dt title='" + _obj.DeptBase + "'>" + _obj.DeptBase + "</dt>";
        tmphtml += "<dt title='" + _obj.TitleBase + " " + _obj.NameBase + "'>[" + GetDocFlow.GetSeqTag(_obj) + "]" + _obj.TitleBase + " <a onclick='AccountInfoWin(" + _obj.UserID + ");return false;'>" + _obj.NameBase + "</a></dt>"; //[기안]
        tmphtml += "<dt class='date'>&nbsp;</dt>";
        tmphtml += "</dl>";
        tmphtml += GetDocFlow.GetUnitSub(_obj, _approvalState);    //하단텍스트
        tmphtml += "</div>";
        tmphtml += "</td>";

        var msgAppType = "[";
        msgAppType += (_obj.ApprovalType.toUpperCase() == "L") ? R_LaterOnTit : R_ProxyTit;
        msgAppType += "]";

        tmphtml += "<td>";
        tmphtml += "<div class='step '>";
        tmphtml += "<dl class='" + tmpcss + "'>";
        tmphtml += "<dt title='" + _obj.P_DeptBase + "'>" + _obj.P_DeptBase + "</dt>";
        tmphtml += "<dt title='" + _obj.P_TitleBase + " " + _obj.ProxyNameBase + "'>" + msgAppType + _obj.P_TitleBase + " <a onclick='AccountInfoWin(" + _obj.ProxyUserID + ");return false;'>" + _obj.ProxyNameBase + "</a></dt>"; //[기안]
        tmphtml += "<dt class='date'>" + _obj.ApprovalDateStr + "</dt>";
        tmphtml += "</dl>";
        tmphtml += "</div>";
        tmphtml += "</td>";
    } else {

        tmphtml += "<td>";
        tmphtml += "<div class='step'>";
        tmphtml += "<dl class='" + tmpcss + "' >";
        tmphtml += "<dt title='" + _obj.DeptBase + "'>" + _obj.DeptBase + "</dt>";
        tmphtml += "<dt title='" + _obj.TitleBase + " " + _obj.NameBase + "'>[" + GetDocFlow.GetSeqTag(_obj) + "]" + _obj.TitleBase + " <a onclick='AccountInfoWin(" + _obj.UserID + ");return false;'>" + _obj.NameBase + "</a></dt>"; //[기안]
        tmphtml += "<dt class='date'>" + _obj.ApprovalDateStr + "</dt>";
        tmphtml += "</dl>";
        tmphtml += GetDocFlow.GetUnitSub(_obj, _approvalState);    //하단텍스트
        tmphtml += "</div>";
        tmphtml += "</td>";
    }

    return tmphtml;
}
//발신결재상태(아이콘)
var isApproveA = false;
GetDocFlow.GetUnitSub = function (_obj, _approvalState) {

    /*
    .apH_flow .step .s_tar   {border:1px solid #a5acb0; border-top:0; color:#FFF; text-align:center; background:#a5acb0;}	검토중-하단txt
    .apH_flow .step .s_ccT{border:1px solid #2688ca; border-top:0; color:#FFF; text-align:center; background:#2688ca;}		참조-하단txt
    .apH_flow .step .s_toT{border:1px solid #9e9080; border-top:0; color:#FFF; text-align:center; background:#9e9080;}		수신-하단txt
    .apH_flow .step .s_reT{border:1px solid #b24486; border-top:0; color:#FFF; text-align:center; background:#b24486;}		반려-하단txt
    .apH_flow .step .s_retarT{border:1px solid #b24486; border-top:0; color:#FFF; text-align:center; background:#ec5a8f;}	반려대상-하단txt

    <dl class="s_tar">
    <dd class="s_txt">검토중</dd>
    </dl> 
    */

    var rtnval = "";

    if (_obj.MyTurn == "Y" && _approvalState != "R") {
        rtnval += "<dl class='s_tar'>";
        rtnval += "<dd class='s_txt'>" + R_InfoY + "</dd>";     //검토중
        rtnval += "</dl> ";
    }
    if (_obj.MyTurn == "Y" && _approvalState == "R") {
        rtnval += "<dl class='s_retarT'>";
        rtnval += "<dd class='s_txt'>" + R_InfoT + "</dd>";     //반려대상
        rtnval += "</dl> ";
    }
    if (_obj.MyTurn != "Y" && _obj.ApprovalType == "R" && _approvalState == "R") {
        rtnval += "<dl class='s_reT'>";
        rtnval += "<dd class='s_txt'>" + R_InfoTR + "</dd>";    //반려
        rtnval += "</dl> ";
    }

    return rtnval;
}


/**************************************************************
공통-수신결재(ZP/ZQ)
**************************************************************/
//수신결재(ZP/ZQ)
GetDocFlow.GetCPInfo = function (_obj) {
    if (_obj == null) return;

    var tmphtml = (_obj.length > 0) ? "<td><img src='" + BaseImgPath + "/Approval/gbl.gif' alt='' class='arr' /></td>" : "";
    $.each(_obj, function (i, o) {        //alert("|"+o.ApprovalType+"|");
        if (o.ApprovalTag.substring(0, 2) == "ZQ" && i > 0) {
            tmphtml += GetDocFlow.GetArrow(o);      //화살표
        }
        tmphtml += GetDocFlow.GetSeqUnitCP(o);
    });
    return tmphtml;
}
//수신결재단위 (그룹지정시, 체크필요)
GetDocFlow.GetSeqUnitCP = function (_obj) {
    if (_obj == null) return;

    var tmpcss = (_obj.ApprovalType == 'R') ? "s_re" : "s_to";
    var tmptxt = (_obj.ApprovalType == 'R') ? R_InfoZR : R_InfoZ;   //수신반려:수신

    var tmphtml = "";
    tmphtml += "<td title='" + R_Receive + "'>"; // 수신
    tmphtml += "<div class='step'>";
    tmphtml += "<dl class='" + tmpcss + "'>";
    tmphtml += "<dt title='" + _obj.DeptBase + "'>" + _obj.DeptBase + "</dt>";
    tmphtml += "<dt title='" + _obj.TitleBase + " " + _obj.NameBase + "'>[" + GetDocFlow.GetSeqTag(_obj) + "]" + _obj.TitleBase + " <a onclick='AccountInfoWin(" + _obj.UserID + ");return false;'>" + _obj.NameBase + "</a></dt>";
    tmphtml += "<dt class='date'>" + _obj.ApprovalDateStr + "</dt>";        //조회여부는 알수없음 = 처리안됨(_obj.ViewNameBase)
    tmphtml += "</dl>";
    tmphtml += "<dl class='" + tmpcss + "T'>";
    tmphtml += "<dd class='s_txt'>" + tmptxt + "</dd>";     //하단텍스트
    tmphtml += "</dl>";
    tmphtml += "</div>";
    tmphtml += "</td>";

    return tmphtml;
}

/**************************************************************
공통-참조결재(ZC)
**************************************************************/
//참조결재(ZC)
GetDocFlow.GetCCInfo = function (_obj) {
    if (_obj == null) return;

    var tmphtml = (_obj.length > 0) ? "<td><img src='" + BaseImgPath + "/Approval/gbl.gif' alt='' class='arr' /></td>" : "";
    $.each(_obj, function (i, o) {        //alert("|"+o.ApprovalTag+"|");
        tmphtml += GetDocFlow.GetSeqUnitCC(o);
    });
    return tmphtml;
}
//참조결재단위 (그룹지정시, 체크필요)
GetDocFlow.GetSeqUnitCC = function (_obj) {
    if (_obj == null) return;

    //_obj.DeptBase = (_obj.DeptBase == "") ? "&nbsp;" : _obj.DeptBase;

    var tmpTitSub = "";
    if (_obj.IsSubTree == "1") {
        tmpTitSub = R_TitSub;
    }

    _obj.DeptBase = (_obj.IsGroup == "G" && _obj.DeptBase == "") ? _obj.NameBase : _obj.DeptBase;       //부서지정 시, 빈값으로 넘어옴

    var tmphtml = "";
    tmphtml += "<td title='" + R_Carboncopy + "'>";
    tmphtml += "<div class='step'>";
    tmphtml += "<dl class='s_cc'>";
    tmphtml += "<dt title='" + _obj.DeptBase + "'>" + _obj.DeptBase + "</dt>";
    tmphtml += "<dt title='" + _obj.TitleBase + " " + _obj.NameBase + "'>[" + GetDocFlow.GetSeqTag(_obj) + "]" + _obj.TitleBase + " " + _obj.NameBase + tmpTitSub + "</dt>"; // [참조]
    tmphtml += "<dt class='date'>" + _obj.ViewingDateStr + "</dt>";
    tmphtml += "</dl>";
    tmphtml += "<dl class='s_ccT'>";
    tmphtml += "<dd class='s_txt'>" + R_InfoCR + "</dd>";     //하단텍스트:참조
    tmphtml += "</dl>";
    tmphtml += "</div>";
    tmphtml += "</td>";

    return tmphtml;
}
