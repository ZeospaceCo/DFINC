GetFavList = {}

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //[닫기]
        $('#cbtnClose').click(function (e) { e.preventDefault(); GetFavList.Close(); });

        GetFavList.Init();
    });
});

/**************************************************
작성자   : 
작성일자 :2012-05-09
작성목적 : 초기화 하기
***************************************************/
var targetObj = null;
GetFavList.Init = function () {

    targetObj = (window.dialogArguments) ? window.dialogArguments : parent;
    GetFavList.GetPageList();
}

/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-09
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
GetFavList.GetPageList = function () {

    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Common/GetFavList.asmx/GetFlowList";

    //파라미터값 배열에 담기
    ParamArray = { 'p': 'p' };

    var obj = eval("GetFavList.SuccessFlowList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
GetFavList.SuccessFlowList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/FormFlow/Common/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    GetFavList.Resource();
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
GetFavList.Resource = function () {

    $("#r_number")[0].innerHTML = R_Number;
    $("#r_subject")[0].innerHTML = R_Subject;
    $("#r_memo")[0].innerHTML = R_Memo;
    $("#r_addDate")[0].innerHTML = R_AddDate;

    if ($("#r_noData")[0]) {
        $("#r_noData")[0].innerHTML = R_NoData;
    }
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : [닫기]
**********************************************************************************************/
GetFavList.Close = function () {
    if (window.dialogArguments) {
        window.returnValue = false; self.close();
    } else {
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
GetFavList.AddLine = function (_favSeqID) {

    if (_favSeqID < 1) return;

    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Common/GetFavList.asmx/GetFlowSeqLine";

    //파라미터값 배열에 담기
    ParamArray = { 'favSeqID': _favSeqID };

    var obj = eval("GetFavList.SuccessAddLine");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
    alert("개인결재선을 지정하였습니다.");
}
var cntZD = 0, cntZH = 0, cntZA = 0;
var cntZP = 0, cntZQ = 0, cntZC = 0;
GetFavList.AddItemObj = function (_tag, _obj) {

    _tag = _tag.toUpperCase();
    switch (_tag) {
        case "ZD":
            dicZD.add(cntZD, _obj);
            cntZD++;
            break;
        case "ZH":
            dicZH.add(cntZH, _obj);
            cntZH++;
            break;
        case "ZA":
            dicZA.add(cntZA, _obj);
            cntZA++;
            break;
        case "ZP":
            dicZP.add(cntZP, _obj);
            cntZP++;
            break;
        case "ZQ":
            dicZQ.add(cntZQ, _obj);
            cntZQ++;
            break;
        case "ZC":
            dicZC.add(cntZC, _obj);
            cntZC++;
            break;
    }
}
GetFavList.SuccessAddLine = function (data, status) {

    if (data != null) {

        var rtnval = {};
        $.each(data.d, function (i, o) {
            rtnval = {
                'tag': data.d[i].ApprovalTag
                , 'seq': data.d[i].Sequence
                , 'userID': data.d[i].UserID
                , 'name': data.d[i].NameBase
                , 'deptID': data.d[i].DeptID
                , 'deptName': data.d[i].DeptBase
                , 'titleID': data.d[i].TitleID
                , 'titleName': data.d[i].TitleBase
                , 'dutyID': data.d[i].DutyID
                , 'dutyName': data.d[i].DutyBase
                , 'isDept': data.d[i].IsDept
                , 'isSub': data.d[i].IsSub
            };

            GetFavList.AddItemObj(data.d[i].ApprovalTag, rtnval);
        });
    }

    GetFavList.SetSeqLine();
}
// 결재라인지정
GetFavList.SetSeqLine = function () {

    var items = null, subkey = null, subitem = null;
    if (dicZD.size() > 0 || dicZH.size() > 0 || dicZA.size() > 0) {

        items = targetObj.formSeqDic.items;
        for (var o in items) {

            /*** 고정결재 ***/

            // 고정결재칸(ZD1,ZD2,ZD3 ...)
            if (o.indexOf("ZD") == 0 && dicZD.size() > 0 && o != "ZD0") {
                subkey = cntZD - dicZD.size(); subitem = dicZD.items[subkey];
                GetFavList.SetObj(o, subitem);
                dicZD.remove(subkey);
            }
            // 고정결재칸(ZH1,ZH2,ZH3 ...)
            if (o.indexOf("ZH") == 0 && dicZH.size() > 0) {
                subkey = cntZH - dicZH.size(); subitem = dicZH.items[subkey];
                GetFavList.SetObj(o, subitem);
                dicZH.remove(subkey);
            }
            // 고정결재칸(ZA1,ZA2,ZA3 ...)
            if (o.indexOf("ZA") == 0 && dicZA.size() > 0) {
                subkey = cntZA - dicZA.size(); subitem = dicZA.items[subkey];
                GetFavList.SetObj(o, subitem);
                dicZA.remove(subkey);
            }

            /*** 유동결재 ***/
            var i = 0, addcnt = 0;

            // 유동결재망(ZD)
            if (o == "FLOWZD" && dicZD.size() > 0) {

                addcnt = (dicZD.size() - $("#FLOWZD", targetObj.ifrmObj).children().length) + 1;
                targetObj.Common.SetFlowInfo("FLOWZD:" + addcnt);

                items = dicZD.items;
                i = 1;
                for (var o in items) {
                    GetFavList.SetObj(items[o].tag + i, items[o]); i++;
                }
            }
            // 유동결재망(ZH)
            if (o == "FLOWZH" && dicZH.size() > 0) {

                addcnt = dicZH.size() - $("#FLOWZH", targetObj.ifrmObj).children().length;
                targetObj.Common.SetFlowInfo("FLOWZH:" + addcnt);

                items = dicZH.items;
                i = 1;
                for (var o in items) {
                    GetFavList.SetObj(items[o].tag + i, items[o]); i++;
                }
            }
            // 유동결재망(ZA)
            if (o == "FLOWZA" && dicZA.size() > 0) {

                addcnt = dicZA.size() - $("#FLOWZA", targetObj.ifrmObj).children().length;
                targetObj.Common.SetFlowInfo("FLOWZA:" + addcnt);

                items = dicZA.items;
                i = 1;
                for (var o in items) {
                    GetFavList.SetObj(items[o].tag + i, items[o]); i++;
                }
            }
        }
    }
    //수신(ZP)
    if (dicZP.size() > 0 && targetObj.formSeqDic.contains("ZP")) {
        items = dicZP.items;
        for (var o in items) {
            GetFavList.SetObj(items[o].tag, items[o]);
        }
    }
    //수신(ZQ)
    if (dicZQ.size() > 0 && targetObj.formSeqDic.contains("ZQ")) {
        items = dicZQ.items;

        for (var o in items) {
            GetFavList.SetObj(items[o].tag, items[o]);
        }
    }
    //고정수신(ZQ01)
    if (dicZQ.size() > 0 && targetObj.formSeqDic.contains("ZQ01_ZD1")) {
        items = dicZQ.items;

        for (var o in items) {
            GetFavList.SetObj(items[o].tag + "01", items[o]);
            break;
        }
    }
    //참조(ZC)
    if (dicZC.size() > 0 && targetObj.formSeqDic.contains("ZC")) {
        items = dicZC.items;
        for (var o in items) {
            GetFavList.SetObj(items[o].tag, items[o]);
        }
    }

    GetFavList.Close();
}

// 결재라인지정 (실제, 태그에 결재자 지정)
GetFavList.SetObj = function (_nowEl, _item) { //alert(_nowEl + "-" + _item.userID + "-" + _item.name + "-" + _item.deptID + "-" + _item.deptName + "-" + _item.titleID + "-" + _item.titleName + "-" + _item.dutyID + "-" + _item.dutyName + "-" + _item.isDept + "-" + _item.isSub)
    //_item.tag
    targetObj.Common.SetApproval(_nowEl, _item.userID, _item.name, _item.deptID, _item.deptName, _item.titleID, _item.titleName, _item.dutyID, _item.dutyName, _item.isDept, _item.isSub, "");
}
