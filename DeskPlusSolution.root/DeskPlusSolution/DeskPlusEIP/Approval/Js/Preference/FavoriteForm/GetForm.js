GetForm = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //[확인]
        $('#cbtnSubmit').click(function (e) { e.preventDefault(); GetForm.Confirm(); });

        //[닫기]
        $('#cbtnCancel').click(function (e) { e.preventDefault(); GetForm.Close(); });
    });
});


/**************************************************
작성자   : 
작성일자 : 
작성목적 : onNodeSelect 양식트리 노드 선택
ex)   node.ID : C-G-folderid
node.Attributes["ID"] : folderid
node.Attributes["PID"] : parentfolderid
node.Attributes["Section"] : C
node.Attributes["Type"] : G
***************************************************/
var tnode = null;
function onFormNodeSelect(node) {   //    alert(node.ID + "," + node.Text + "," + node.GetProterty(""));

    SetCheckedList(node);       // 체크된 리스트 세팅

    if ($("#tvSelectedValue").val() != "") {

        $("#selData").text("");
        var selname = "";

        var arrSelVal = $("#tvSelectedValue").val().split(";");
        for (var i = 0; i < arrSelVal.length; i++) {
            selname += (arrSelVal[i] != "") ? arrSelVal[i] + "," : "";
        }
        $("#selData").text(selname);
    }
    else {
        $("#selData").text("");
    }

    tnode = node;
    setTimeout("tnode.Expand()", 300); //node.Expand();  //하위노드 확장
}


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 트리 체크한 값 가져오기
***************************************************/
SetCheckedList = function (node) {

    var selID = "F" + node.ID;
    var selValue = node.Text;

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

    $("#tvSelectedID").val(selIDs);
    $("#tvSelectedValue").val(selValues);           // deptID | deptName | parentDeptID | parentDeptName
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [확인]
***************************************************/
GetForm.Confirm = function () {

    var selIDs = $("#tvSelectedID").val();          // deptID | deptID | deptID | deptID |
    var selValues = $("#tvSelectedValue").val();

    if (selIDs == "") { alert(R_Msg_ErrSelForm); return; }

    if (Callback != "") {
        var obj = eval("parent." + Callback);
        if (obj) {
            obj(escape(selIDs), escape(selValues));
        }
    }

    GetForm.Close();
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [취소]
***************************************************/
GetForm.Close = function () {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}
