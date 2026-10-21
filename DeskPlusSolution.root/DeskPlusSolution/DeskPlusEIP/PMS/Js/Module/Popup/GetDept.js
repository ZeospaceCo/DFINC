GetDept = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnSubmit').click(function (e) {
            e.preventDefault();
            GetDept.Confirm();
        });

        //닫기
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            //GetDept.Cancel();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //검색입력필드 엔터 이벤트 기술
        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (event) {
            GetDept.KeyEvents();
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
    });
});

/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 트리 선택 이벤트
***************************************************/
function DeptUserTreeScript_onNodeSelect(node) {
    // 체크된 리스트 세팅
    SetCheckedList(node);

    if (document.getElementById("tvSelectedValue").value != "") {
        document.getElementById("tbReceiveMan").value = ""; //이름값
        document.getElementById("hdnUserID").value = ""; //아이디값
        document.getElementById("tvTypes").value = "";  // type값

        var userName = "";
        var userID = "";
        var types = "";

        var arrSelectedValue = (document.getElementById("tvSelectedValue").value).split(";");

        for (var i = 0; i < arrSelectedValue.length; i++) {
            var arrSubValue = arrSelectedValue[i].split("|");

            if (arrSubValue[0] == "M" || arrSubValue[0] == "G") {
                userID += arrSubValue[1] + ",";
                userName += arrSubValue[2] + ",";
                types += arrSubValue[0] + ",";
            }
        }

        document.getElementById("tbReceiveMan").value = userName;
        document.getElementById("hdnUserID").value = userID;
        document.getElementById("tvTypes").value = types;
    }
    else {
        document.getElementById("tbReceiveMan").value = "";
        document.getElementById("hdnUserID").value = "";
        document.getElementById("tvTypes").value = "";
    }
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

        selIDs = selIDs.replace(selID + "|", "");
        selIDs += selID + "|";

        var selValue = colSplit[1]; //nodeValue
        selValues = selValues.replace(selValue + ";", "");
        selValues += selValue + ";";
    });

    // type(G) | deptID | deptName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName
    // type(M) | userID | userName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName

    $("#tvSelectedID").val(selIDs);
    $("#tvSelectedValue").val(selValues);

    $("#tbReceiveMan").val("");
    $("#hdnUserID").val("");
    $("#tvTypes").val("");

    //alert($("#tvSelectedValue").val());

    if ($("#tvSelectedID").val() != "") {
        $("#tbReceiveMan").val("")
        $("#hdnUserID").val("")

        var arrSubValue = "";
        var userName = "";
        var userID = "";
        var types = "";

        //alert($("#tvSelectedValue").val());
        var iSelValue = $("#tvSelectedValue").val();
        var arrSelectedValue = iSelValue.split(";");

        for (var i = 0; i < arrSelectedValue.length; i++) {
            arrSubValue = arrSelectedValue[i].split("|");
            var nameStr = "";

            if (arrSubValue[2] != "" && arrSubValue[2] != undefined) {
                if (arrSubValue[2].indexOf("/") != -1) {
                    nameStr = arrSubValue[2].split("/");
                    nameStr = nameStr[0];
                }
                else {
                    nameStr = arrSubValue[2];
                }
            }

            //1:부서; 2:사용자
            if (Gubun == "1" && arrSubValue[0] == "G") {
                userName += nameStr + ",";
                userID += arrSubValue[1] + ",";
                types += arrSubValue[0] + ",";
            }
            else if (Gubun == "2" && arrSubValue[0] == "M") {
                userName += nameStr + ",";
                userID += arrSubValue[1] + ",";
                types += arrSubValue[0] + ",";
            }
        }

        $("#tbReceiveMan").val(userName);
        $("#hdnUserID").val(userID);
        $("#tvTypes").val(types);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색의 엔터키이벤트
***************************************************/
GetDept.KeyEvents = function () {
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
GetDept.Confirm = function () {
    var userName = document.getElementById("tbReceiveMan").value;
    var userID = document.getElementById("hdnUserID").value;
    var types = document.getElementById("tvTypes").value;  // type값
    var response = userID + "‡" + userName + "‡" + types;

    if (parent.GetDept) {
        parent.GetDept(escape(response));
    }

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }

    //window.returnValue = escape(response);
    //window.close();
}