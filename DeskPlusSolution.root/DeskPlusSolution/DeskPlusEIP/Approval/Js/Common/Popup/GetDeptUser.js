GetDeptUser = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //확인
        $('#cbtnSubmit').click(function (e) {
            e.preventDefault();
            GetDeptUser.Confirm();
        });

        //닫기
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            GetDeptUser.Cancel();
        });

        //트리검색(검색버튼)
        $('#CbtnDeptUserSearch').click(function (e) {
            e.preventDefault();
            SearchDeptUserList(1);
        });

        //트리검색(검색취소)
        $('#CbtnTreeView').click(function (e) {
            e.preventDefault();
            SearchDeptUserList(2);
        });

        //검색입력필드 엔터 이벤트 기술
        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (event) {
            GetDeptUser.KeyEvents();
        });
    });
});


/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 트리 선택 이벤트
***************************************************/
var unode = null;
function DeptUserTreeScript_onNodeSelect(node) {
    // 체크된 리스트 세팅
    SetCheckedList(node);

    if (document.getElementById("tvSelectedValue").value != "") {
        document.getElementById("tbReceiveMan").value = ""; //이름값
        document.getElementById("hdnUserID").value = ""; //아이디값
        document.getElementById("hdnDeptID").value = "";  // type값
        document.getElementById("tvTypes").value = "";  // type값

        var userName = "";
        var userID = "";
        var deptID = "";
        var types = "";

        var arrSelectedValue = (document.getElementById("tvSelectedValue").value).split("$");

        for (var i = 0; i < arrSelectedValue.length; i++) {
            var arrSubValue = arrSelectedValue[i].split("|");

            if (arrSubValue[0] == "M" || arrSubValue[0] == "G") {
                userID += arrSubValue[1] + ",";
                userName += arrSubValue[2] + ",";
                types += arrSubValue[0] + ",";
                deptID += arrSubValue[3] + ",";
            }
        }

        document.getElementById("tbReceiveMan").value = userName;
        document.getElementById("hdnUserID").value = userID;
        document.getElementById("hdnDeptID").value = deptID;
        document.getElementById("tvTypes").value = types;
    }
    else {
        document.getElementById("tbReceiveMan").value = "";
        document.getElementById("hdnUserID").value = "";
        document.getElementById("hdnDeptID").value = "";
        document.getElementById("tvTypes").value = "";
    }

    unode = node;
    setTimeout("unode.Expand()", 300); //node.Expand();  //하위노드 확장
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 트리 체크한 값 가져오기
***************************************************/
function SetCheckedList(node) {
    var selIDs = document.getElementById("tvSelectedID").value; 	// ID값만 넣기
    var selValues = document.getElementById("tvSelectedValue").value;  // Value 값 넣기

    var selID = node.ID;
    var selValue = node.Value;

    if (IsCheckBox == "0") {
        // 단일선택
        selIDs = selID;
        selValues = selValue;
    } else {
        // 다중선택
        if (node.Checked) {
            selIDs = selIDs.replace(selID + "|", "");
            selIDs += selID + "|";

            selValues = selValues.replace(selValue + "$", "");
            selValues += selValue + "$";
        } else {
            selIDs = selIDs.replace(selID + "|", "");
            selValues = selValues.replace(selValue + "$", "");
        }
    }

    document.getElementById("tvSelectedID").value = selIDs;

    // type(G) | deptID | deptName | parentDeptID | parentDeptName | titleID | titleName | loginID | orders | mail | officephone | fax
    // type(M) | userID | userName | deptID | deptName | titleID | titleName | loginID | orders | mail | phone | officePhone
    document.getElementById("tvSelectedValue").value = selValues;
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : listbox 선택한 값 가져오기
***************************************************/
function LbxDeptUser_OnChange() {

    $("#tvSelectedID").empty();
    $("#tvSelectedValue").empty();

    var selIDs = "";
    var selValues = "";

    $("#" + LBDeptUser + " :selected").each(function (i, selected) {
        var rtnValue = $(selected).val();
        var rtnText = $(selected).text();

        var colSplit = rtnValue.split("^"); //nodeID
        var rowSplit = colSplit[1].split("$"); //nodeValue
        var selID = colSplit[0]; //nodeID

        //사용자만 추가한다.
        if (rowSplit[0].split("|")[0] == "M") {

            selIDs = selIDs.replace(selID + "|", "");
            selIDs += selID + "|";

            var selValue = colSplit[1]; //nodeValue
            selValues = selValues.replace(selValue + "$", "");
            selValues += selValue + "$";
        }
    });

    // type(G) | deptID | deptName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName
    // type(M) | userID | userName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName

    $("#tvSelectedID").val(selIDs);
    $("#tvSelectedValue").val(selValues);

    $("#tbReceiveMan").val("");
    $("#hdnUserID").val("");
    $("#hdnDeptID").val("");
    $("#tvTypes").val("");


    if ($("#tvSelectedID").val() != "") {

        $("#tbReceiveMan").val("")
        $("#hdnUserID").val("")
        $("#hdnDeptID").val("")

        var arrSubValue = "";
        var userName = "";
        var userID = "";
        var types = "";
        var deptID = "";

        var iSelValue = $("#tvSelectedValue").val();
        var arrSelectedValue = iSelValue.split("$");

        for (var i = 0; i < arrSelectedValue.length; i++) {
            arrSubValue = arrSelectedValue[i].split("|");

            if (arrSubValue[0] == "M" || arrSubValue[0] == "G") {
                var nameStr = "";
                if (arrSubValue[2].indexOf("/") > 0) {
                    nameStr = arrSubValue[2].split("/");
                    nameStr = nameStr[0];
                }
                else {
                    nameStr = arrSubValue[2];
                }

                userName += nameStr + ",";
                userID += arrSubValue[1] + ",";
                types += arrSubValue[0] + ",";
                deptID += arrSubValue[3] + ",";
            }
        }

        $("#tbReceiveMan").val(userName);
        $("#hdnUserID").val(userID);
        $("#hdnDeptID").val(deptID);
        $("#tvTypes").val(types);

    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색의 엔터키이벤트
***************************************************/
GetDeptUser.KeyEvents = function () {
    if (event.keyCode == 13) {
        SearchDeptUserList(1);
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 조직도 검색
***************************************************/
function SearchDeptUserList(param) {
    var searchName = $("#DeptUserTreeScript1_tbDeptUser").val().securityText();
    $("#DeptUserTreeScript1_tbDeptUser").val(searchName);

    var params = searchName + "" + param + "" + CompanyID;
    DeptUserTreeScript1_CallBack1.Callback(params);
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 확인
***************************************************/
GetDeptUser.Confirm = function () {

    var userName = document.getElementById("tbReceiveMan").value;
    var userID = document.getElementById("hdnUserID").value;
    var deptID = document.getElementById("hdnDeptID").value;
    var types = document.getElementById("tvTypes").value;  // type값	

    if (userID == "" || userID == "0") {
        alert(R_PleaseSelectPerson);
        return;
    }

    var response = userID + "" + userName + "" + deptID + "" + types + "" + document.getElementById("tvSelectedValue").value;
    if (Callback != "") {
        var obj = eval("parent." + Callback);
        if (obj) {
            obj(escape(response));
        }
    }
    else {
        parent.GetUserDept(escape(response));
    }

    parent.disablePopup();
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 취소
***************************************************/
GetDeptUser.Cancel = function () {

    if (parent.Add != undefined) {
        if (parent.Add.AfterLine) parent.Add.AfterLine();
    }

    parent.disablePopup();
}

