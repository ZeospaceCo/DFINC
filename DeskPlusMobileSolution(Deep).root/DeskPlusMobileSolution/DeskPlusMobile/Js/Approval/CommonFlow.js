/// CommonFlow.CustomRegion ################################################################################################
/// Developer : 
/// date      : 
/// desc      : 유동결재망을 관리하는 클래스

CommonFlow = new function () {

    this.countZD = 0;               // 현재 추가된 결재칸 수(발신)
    this.countZH = 0;               // 현재 추가된 결재칸 수(협조)
    this.countZA = 0;               // 현재 추가된 결재칸 수(병렬)
    this.countZR = 0;               // 현재 추가된 결재칸 수(재결)

    this.contentHTML = "<table width=\"90\" height=\"100\" bgcolor=\"#000000\" cellspacing=\"1\" style=\"float:left;margin-right:1px;margin-bottom:1px;\" >";
    this.contentHTML += "<tr><td style=\"padding-bottom:2px\" bgcolor=\"#FFFFFF\" height=\"20\" align=\"center\" type=\"header\"><span>&nbsp;</span></td></tr>";
    this.contentHTML += "<tr><td style=\"padding-bottom:2px\" bgcolor=\"#FFFFFF\" height=\"60\" align=\"center\" type=\"body\"><span isFlow='true'>결재</span></td></tr>";
    this.contentHTML += "<tr><td style=\"padding-bottom:2px\" bgcolor=\"#FFFFFF\" height=\"20\" align=\"center\" type=\"footer\"><span>&nbsp;</span></td></tr>";
    this.contentHTML += "</table>";

    this.regionColor = "#eeeeee";

    //   count값 가져오기
    this.getCount = function (_type) {
        switch (_type) {
            case "ZD": return this.countZD;
            case "ZH": return this.countZH;
            case "ZA": return this.countZA;
            case "ZR": return this.countZR;
        }
    }

    //   count값 설정
    this.operateCount = function (_type, _operator, _val) {
        if (_operator == "plus") {
            switch (_type) {
                case "ZD": this.countZD = this.countZD + _val; return this.countZD;
                case "ZH": this.countZH = this.countZH + _val; return this.countZH;
                case "ZA": this.countZA = this.countZA + _val; return this.countZA;
                case "ZR": this.countZR = this.countZR + _val; return this.countZR;
            }
        }
        else if (_operator == "minus") {
            switch (_type) {
                case "ZD": this.countZD = this.countZD - _val; return this.countZD;
                case "ZH": this.countZH = this.countZH - _val; return this.countZH;
                case "ZA": this.countZA = this.countZA - _val; return this.countZA;
                case "ZR": this.countZR = this.countZR - _val; return this.countZR;
            }
        }
    }

    /// summary =====================================================
    /// 초기화
    /// sHtml  : optional    string   디버그 모드
    /// cntArr : optional    array   [ZD][2]
    ///                              [ZH][1]
    this.Init = function (_sHtml, _cntArr) {

        if (_sHtml && _sHtml != "") { this.contentHTML = _sHtml; };
        /*
        if (_cntArr) {
        for (var i = 0; i < _cntArr.length; i++) {
        if (_cntArr[i][1] > 0)
        this.operateCount(_cntArr[i][0], "plus", _cntArr[i][1]);
        }
        }*/
        this.operateCount("ZD", "plus", 0);   // ZD0 부터 시작
        this.operateCount("ZH", "plus", 1);   // ZH1 부터 시작
        this.operateCount("ZA", "plus", 1);   // ZA1 부터 시작
        this.operateCount("ZR", "plus", 1);   // ZR1 부터 시작
    };


    /// summary =====================================================
    /// 결재칸 추가
    /// count : optional    int      추가할 칸 수
    /// id    : optional    string   아이디 (Default:'ZD')
    /// _return : 추가된 Item의 Array로 반환
    this.Add = function (_count, _nowEl) {          //alert(_count +"-"+ _nowEl)
        if (_nowEl == null || _nowEl.id == "") return false;    // 유효한id가 아니면 false
        if (_nowEl.id.indexOf("FLOW") < 0) return;

        if (!_count) { _count = 1; };

        var oItemArr = new Array();
        var parentEl = _nowEl;
        var tagID = _nowEl.id.replace("FLOW", "");     // itemID 가져오기 (ZD, ZH, ZA, ZR)
        var oEl;

        for (var i = 0; i < _count; i++) {
            parentEl.insertAdjacentHTML("beforeEnd", this.contentHTML);

            oEl = parentEl.lastChild;
            itemCnt = this.getCount(tagID); //alert(itemCnt)

            oItemArr[i] = this.GetItemInfoByElement(oEl);
            oItemArr[i].oHeader.id = tagID + itemCnt.toString() + "_INFO1";
            oItemArr[i].oHeader.className = tagID;
            oItemArr[i].oBody.id = tagID + itemCnt.toString();              //alert(oItemArr[i].oBody.id);
            oItemArr[i].oBody.className = tagID;
            oItemArr[i].oFooter.id = tagID + itemCnt.toString() + "_INFO0";
            oItemArr[i].oFooter.className = tagID;

            this.operateCount(tagID, "plus", 1);
        };

        return oItemArr;
    };


    /// summary =====================================================
    /// 결재칸 삭제
    /// _return : 성공시 삭제된 element 반환, 실패시 false 반환
    this.Remove = function (_nowEl) {
        if (_nowEl == null || _nowEl.id == "") return false; //  유효한 id가 아니면 false
        if (_nowEl.id.indexOf("FLOW") < 0) return;

        var parentEl = _nowEl;
        var oEl = parentEl.lastChild;

        if (oEl == null) return false;      //삭제 할 객체가 없을때
        if (oEl.firstChild == null) return false;

        var subItemID = this.GetItemInfoByElement(oEl).oBody.id;
        if (subItemID == "ZD0") return;

        if ($("#" + subItemID, ifrmObj).attr("IsAsign")) {
            action_popup.alert(Msg_FlowErrorSet);
            return;
        }

        if (oEl) {
            // itemID 가져오기
            var itemID = _nowEl.id.replace("FLOW", "");     // itemID 가져오기 (ZD, ZH, ZA, ZR)
            $(oEl, ifrmForm).remove();          //o = oEl.removeNode(true);

            if (itemID) this.operateCount(itemID, "minus", 1);
            return;
        }

        return false;
    };

    /// summary =====================================================
    /// 결재칸이 들어 있는 Item 반환
    /// oEl : required   element  결재칸 객체
    this.GetItemInfoByElement = function (oEl) {

        if (!oEl) { return null; }

        oContentTB = oEl;
        if (oContentTB) {
            var oHeader = null; var oBody = null; var oFooter = null;
            var oItem = { oHeader: null, oBody: null, oFooter: null };

            var oTR = oContentTB.firstChild.children;

            for (var i = 0; i < oTR.length; i++) {
                oCells = oTR[i].children;
                for (var j = 0; j < oCells.length; j++) { //alert($(oCells[j]).attr("type"))
                    if ($(oCells[j]).attr("type") == "header") {
                        oItem.oHeader = oCells[j].firstChild;
                    }
                    else if ($(oCells[j]).attr("type") == "body") {
                        oItem.oBody = oCells[j].firstChild;
                    }
                    else if ($(oCells[j]).attr("type") == "footer") {
                        oItem.oFooter = oCells[j].firstChild;
                    }
                }
            }
            return oItem;
        }
        return null;
    };
    this.GetItemInfoByID = function (sID) {
        var oEl = this.GetITemElementByID(sID);
        return this.GetItemInfoByElement(oEl);
    };
    this.GetItemInfoByIndex = function (nIndex) {
        var oEl = this.GetItemElementByIndex(nIndex);
        return this.GetItemInfoByElement(oEl);
    };

    this.GetItemIDByEl = function (_nowEl) {
        var arrParentObj = this.setFlowRegion(_nowEl);
        var parentEl = arrParentObj[0];

        oEl = parentEl.lastChild;
        if (oEl == null) return false;      //삭제 할 객체가 없을때
        return subItemID = this.GetItemInfoByElement(oEl).oBody.id;
    };


    /// summary =====================================================
    /// 결재칸이 들어 있는 TD Element 반환
    /// nIndex : required   int      인덱스값 (0-index)
    /// sID    : required   string   아이디
    this.GetItemElementByIndex = function (nIndex) {
        return this.parentEl.children[nIndex];
    };
    this.GetITemElementByID = function (sID) {
        var oTBs = this.parentEl.children;
        for (var i = 0; i < oTBs.length; i++) {
            oTRs = oTBs[i].firstChild.children;
            for (var j = 0; j < oTRs.length; j++) {
                oTDs = oTRs[j].children;
                for (var k = 0; k < oTDs.length; k++) {
                    if (oTDs[k].id == sID) {
                        return oTBs[i];
                    }
                }
            }
        }
        return null;
    };
};
///############################################################################################## CommonFlow.CustomRegion ##
