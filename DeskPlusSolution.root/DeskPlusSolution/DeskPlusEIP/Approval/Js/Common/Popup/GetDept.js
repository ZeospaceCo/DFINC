GetDept = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //[확인]
        $('#cbtnSubmit').click(function (e) { e.preventDefault(); GetDept.Confirm(); });

        //[닫기]
        $('#cbtnCancel').click(function (e) { e.preventDefault(); GetDept.Close(); });

        //하위폴더적용 사용유무
        if (IsSub == "0") { $("#divsub").hide(); }
    });
});


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 트리 선택 이벤트
***************************************************/
var unode = null;
DeptNodeSelect_onNodeSelect = function (node) {

    SetCheckedList(node);       // 체크된 리스트 세팅

    if ($("#tvSelectedValue").val() != "") {

        $("#selData").text("");
        var selname = "";

        var arrSelVal = $("#tvSelectedValue").val().split(";");
        for (var i = 0; i < arrSelVal.length; i++) {
            var arrObj = arrSelVal[i].split("|");
            selname += (arrObj.length > 2) ? arrObj[1] + "," : "";
        }
        $("#selData").text(selname);
    }
    else {
        $("#selData").text("");
    }

    unode = node;
    setTimeout("unode.Expand()", 300); //node.Expand();  //하위노드 확장
}


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 트리 체크한 값 가져오기
***************************************************/
SetCheckedList = function (node) {

    var selID = node.ID;
    var selValue = node.Value;

    if (IsCheckBox == "1") {    //체크박스를 사용하는 경우.

        var selIDs = $("#tvSelectedID").val(); 	        // ID 값만 넣기
        var selValues = $("#tvSelectedValue").val();    // Value 값 넣기

        if (node.Checked) {
            selIDs = selIDs.replace(selID + "|", "");
            selIDs += selID + "|";

            selValues = selValues.replace(selValue + ";", "");
            selValues += selValue + ";";
        }
        else {
            selIDs = selIDs.replace(selID + "|", "");
            selValues = selValues.replace(selValue + ";", "");
        }

    } else {

        selIDs = selID;
        selValues = selValue;
    }

    $("#tvSelectedID").val(selIDs);
    $("#tvSelectedValue").val(selValues);           // deptID | deptName | parentDeptID | parentDeptName
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [확인]
***************************************************/
GetDept.Confirm = function () {

    var selIDs = $("#tvSelectedID").val();          // deptID | deptID | deptID | deptID |
    var selValues = $("#tvSelectedValue").val();

    var isSub = "0"; ;
    if ($("#cbOptionSubFolder").is(":checked")) {
        isSub = "1";
    }
    else {
        isSub = "0";
    }

    if (selIDs == "") { alert(R_Msg_Dept); return; }

    if (Callback != "") {
        var obj = eval("parent." + Callback);
        if (obj) {
            obj(escape(selIDs), escape(selValues), isSub);
        }
    }
    else {
        parent.GetDept(escape(selIDs), escape(selValues), isSub);
    }

    GetDept.Close();
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [취소]
***************************************************/
GetDept.Close = function () {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

