Common = {}

/**************************************************************
초기화
**************************************************************/
Common.Init = function () {
    Tree.ContentFrame = parent.frmContent;
    Tree.GoPage("");
}

Common.CheckOpinion = function () {
    //입력값 체크하기
    if ($.browser.msie) {
        $('.charsRemaining').text($('#tbOpinion').attr('maxlength'));
        $('#tbOpinion').bind('keyup', function () {
            $(this).css('border', '1px solid #d2d2d2');
            var $this = $(this);
            var len = $this.val().length;
            var maxlength = $this.attr('maxlength')

            $('.charsRemaining').text(maxlength - len);
            if (maxlength && len > maxlength) {
                $(this).css('border', '1px solid #f00');
                alert(R_Msg_ErrOpinion);
                $this.val($this.val().slice(0, maxlength));
                $('.charsRemaining').text("0");
                return;
            }
        });
    }
    else {
        Common.MaxLang();
    }
}

/**************************************************
작성목적 : 글자 제한
***************************************************/
Common.MaxLang = function () {

    var onEditCallback = function (remaining) {
        $('.charsRemaining').text(remaining);
        if (remaining > 0) { $(this).css('border', '1px solid #d2d2d2'); }
    }
    var onLimitCallback = function () {
        $(this).css('border', '1px solid #f00');
    }
    $('textarea[maxlength]').limitMaxlength({
        onEdit: onEditCallback,
        onLimit: onLimitCallback
    });
}

/**************************************************************
트리메뉴제어
**************************************************************/
Common.SetMenu = function (_index) {

    $(".accordion").accordion({
        heightStyle: "fill",
        autoHeight: false,

        //fillSpace: true, // CHANGED (previously fillSpace: true)
        animated: 'slide',
        active: _index
    });
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 조직도 선택 이벤트
***************************************************/
var unode = null;
function DeptUserTreeScript_onNodeSelect(node) {
    if (node.Value == "") return;

    // 팝업 clear처리
    if (Tree.ContentFrame.oContextMenuCtrl != null) Tree.ContentFrame.oContextMenuCtrl.clearItem();
    $("#zeoEditorDIV", Tree.ContentFrame.document).hide();

    Common.SetUserInfo(node.Value);

    unode = node;
    setTimeout("unode.Expand()", 300); //node.Expand();  //하위노드 확장
}
/**************************************************
작성자   : 
작성일자 : 
작성목적 : listbox 선택한 값 가져오기
***************************************************/
function LbxDeptUser_OnChange() {
    var tmpval = $("#" + LBDeptUser + " :selected").val();
    if (tmpval == "") return;

    // 팝업 clear처리
    if (Tree.ContentFrame.oContextMenuCtrl != null) Tree.ContentFrame.oContextMenuCtrl.clearItem();
    $("#zeoEditorDIV", Tree.ContentFrame.document).hide();

    Common.SetUserInfo(tmpval);
}
/**************************************************
작성자   : 
작성일자 : 
작성목적 : 결재자지정
***************************************************/
Common.SetUserInfo = function (_val) {
    if (_val == "") return;
    if (_val == undefined) return;

    var nowEl = Tree.ContentFrame.nowEl;
    //if (nowEl == null) { alert(R_Msg_ErrSelUnit); return; }
    if (nowEl == null) { return; }
    if (nowEl.id == "ZD0") { return; }
    if (!/^Z{1}[D|H|C|P|Q|A|R]{1}([0-9])?[0-9]*$/.test(nowEl.id)) {
        alert(R_Msg_ErrSelUnit); return;    // 모든 결재자의 경우 (결재칸선택)
    }

    if (nowEl.getAttribute("IsAsign")) { alert(R_Msg_ErrReCancel); return; }

    var tmpval = _val.split('|');

    // [0]type(G) | [1]deptID | [2]deptName | [3]parentDeptID | [4]parentDeptName | [5]titleID | [6]titleName | [7]dutyID | [8]dutyName | [9]loginID | [10]mail | [11]officephone | [12]fax         
    // [13]cpuserID | [14]cpuserName | [15]cpdeptID | [16]cpdeptName | [17]cptitleID | [18]cptitleName | [19]cpdutyID | [20]cpdutyName | [21]cploginID              : 수발자
    // [22]appuserID | [23]appuserName | [24]appdeptID  | [25]appdeptName | [26]apptitleID | [27]apptitleName | [28]appdutyID | [29]appdutyName | [30]apploginID    : 결재자   

    // [0]type(M) | [1]userID | [2]userName | [3]deptID       | [4]deptName       | [5]titleID | [6]titleName | [7]dutyID | [8]dutyName | [9]loginID | [10]mail | [11]phone       | [12]officePhone 
    // [13~30] 빈값

    // Common.SetApproval = function (_tagID, _userID, _userName, _deptID, _deptName, _titleID, _titleName, _dutyID, _dutyName, _isGroup, _subTree, _userList)
    if (tmpval[0] == "M") {
        //사용자선택
        Tree.ContentFrame.Common.SetApproval(nowEl.id, tmpval[1], tmpval[2], tmpval[3], tmpval[4], tmpval[5], tmpval[6], tmpval[7], tmpval[8], "M", "0", "");
    } else {
        //부서선택

        // 참조(ZC)
        if (/Z{1}[C][0-9]?[0-9]?$/.test(nowEl.id)) {
            //하위부서지정 popup
            Common.popUpGroupList(_val); return;
        }
        // 발신(ZD)/협조(ZH)/합의(ZA)/공람(ZR)
        if ((/Z{1}[D|H|A|R][0-9]?[0-9]?[0-9]?$/.test(nowEl.id))) {
            // 결재자
            Tree.ContentFrame.Common.SetApproval(nowEl.id, tmpval[22], tmpval[23], tmpval[24], tmpval[25], tmpval[26], tmpval[27], tmpval[28], tmpval[29], "G", "0", ""); return;
        }
        // 수신(ZQ/ZP)
        if ((/Z{1}[Q|P][0-9]?[0-9]?[0-9]?$/.test(nowEl.id))) {
            // 수발자
            Tree.ContentFrame.Common.SetApproval(nowEl.id, tmpval[13], tmpval[14], tmpval[15], tmpval[16], tmpval[17], tmpval[18], tmpval[19], tmpval[20], "G", "0", ""); return;
        }
    }
}
var oContextMenuCtrl = null;
Common.popUpGroupList = function (_val) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    var oItem = oContextMenuCtrl.addItem(R_DeptSet, Common.popUpGroup_onclick, "");     //.addItem = function(sTxt, eventHandler, sImgUrl, subItem)
    oItem.txt.setAttribute("arg", _val, 0);
    oContextMenuCtrl.show(window.event);
}
Common.popUpGroup_onclick = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");

        var nowEl = Tree.ContentFrame.nowEl;
        var isSubTree = (confirm(R_MSG_SubDept)) ? "1" : "0";
        var tmpval = arg.split('|');

        Tree.ContentFrame.Common.SetApproval(nowEl.id, 0, "", tmpval[1], tmpval[2], 0, "", 0, "", "G", isSubTree, "");
    }

    oContextMenuCtrl.hide();
}
/**************************************************
작성자   :
작성일자 :
작성목적 : 검색의 엔터키이벤트
***************************************************/
Common.KeyEvents = function () {
    if (event.keyCode == 13) {
        SearchDeptUserList(1);
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 조직도 검색
***************************************************/
function SearchDeptUserList(param) {

    var searchName = $("#UCDeptUserTreeScript1_tbDeptUser").val().securityText();
    $("#UCDeptUserTreeScript1_tbDeptUser").val(searchName);

    var params = searchName + "" + param + "" + CompanyID;
    UCDeptUserTreeScript1_CallBack1.Callback(params);
}

/**************************************************
작성목적 : 부서정보설정(기타정보설정's 겸직)
***************************************************/
Common.SetDeptInfo = function (_deptID) {
    var selIndex = 0;
    var options = $("#ddlAdditionDept")[0].options;
    $.each(options, function (i, o) {
        if (o.getAttribute("deptID") != null) {
            if (o.getAttribute("deptID") == _deptID) {
                selIndex = i;
            }
        }
    });
    $("#ddlAdditionDept")[0].selectedIndex = selIndex;
}

/**************************************************
작성목적 : 열람권한설정
***************************************************/
Common.SetGradeInfo = function (_val) { //alert(_val)
    //열람권한 처리
    $("tbody > tr > td, tbody > tr > th", "#gradeinfo").css("font-weight", "normal");
    if ($("td, th", $("#grd_" + _val)).length > 0) {
        $("td, th", $("#grd_" + _val)).css("font-weight", "bold");
    }
}