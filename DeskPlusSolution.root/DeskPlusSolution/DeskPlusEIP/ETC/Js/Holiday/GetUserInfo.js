GetUserInfo = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnSubmit').click(function (e) {
            e.preventDefault();
            GetUserInfo.Confirm();
        });

        //대행자지우기
        $('#cbtnClean').click(function (e) {
            e.preventDefault();
            GetUserInfo.Clean();
        });

        //닫기
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            GetUserInfo.Cancel();
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
            GetUserInfo.KeyEvents();
        });
    });
});


/**************************************************
작성목적 : 트리 선택 이벤트
***************************************************/
function DeptUserTreeScript_onNodeSelect(node) {
    // 체크된 리스트 세팅
    SetCheckedList(node);

    if (document.getElementById("tvSelectedValue").value != "") {
        document.getElementById("tbReceiveMan").value = ""; //이름값
        document.getElementById("hdnUserID").value = ""; //아이디값
        document.getElementById("hdnDeptID").value = ""; //부서아이디값
        document.getElementById("hdnDeptName").value = "";  // 부서이름

        var userName = "";
        var userID = "";
        var deptID = "";
        var deptName = "";

        var arrSubValue = document.getElementById("tvSelectedValue").value.split("|");
        
        if (arrSubValue[0] == "M" || arrSubValue[0] == "G") {
            userID = arrSubValue[1];
            deptID = arrSubValue[3];
            userName = arrSubValue[5] + " " + arrSubValue[2];
            deptName = arrSubValue[11];
        }        

        document.getElementById("tbReceiveMan").value = userName;
        document.getElementById("hdnUserID").value = userID;
        document.getElementById("hdnDeptID").value = deptID;
        document.getElementById("hdnDeptName").value = deptName;
    }
    else {
        document.getElementById("tbReceiveMan").value = "";
        document.getElementById("hdnUserID").value = "";
        document.getElementById("hdnDeptID").value = "";
        document.getElementById("hdnDeptName").value = "";
    }
}


/**************************************************
작성목적 : 트리 체크한 값 가져오기
***************************************************/
function SetCheckedList(node) {
    var selIDs = "";
    var selValues = "";

    var selID = node.ID;
    var selValue = node.Value;

    // 단일선택
    if (node.Checked) {
        selIDs = selID;
        selValues = selValue;
    }
    
    document.getElementById("tvSelectedID").value = selIDs;
    document.getElementById("tvSelectedValue").value = selValues;
}


/**************************************************
작성목적 : listbox 선택한 값 가져오기
***************************************************/
function LbxDeptUser_OnChange() {

    $("#tvSelectedID").empty();
    $("#tvSelectedValue").empty();

    var selIDs = "";
    var selValues = "";

    $("#" + LBDeptUser + " :selected").each(function (i, selected) {
        if (i == 0) {

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
        }
        else {
            alert(R_MsgTarget);
            return;
        }
    });


    $("#tvSelectedID").val(selIDs);
    $("#tvSelectedValue").val(selValues);

    $("#tbReceiveMan").val("");
    $("#hdnUserID").val("");
    $("#hdnDeptID").val("");
    $("#hdnDeptName").val("");

    if ($("#tvSelectedID").val() != "") {

        $("#tbReceiveMan").val("");
        $("#hdnUserID").val("");
        $("#hdnDeptID").val("");
        $("#hdnDeptName").val("");

        var arrSubValue = "";
        var userName = "";
        var userID = "";
        var deptID = "";
        var deptName = "";


        arrSubValue = $("#tvSelectedValue").val().split("|");

        if (arrSubValue[0] == "M" || arrSubValue[0] == "G") {
            var nameStr = "";
            if (arrSubValue[2].indexOf("/") > 0) {
                nameStr = arrSubValue[2].split("/");
                nameStr = nameStr[0].replace("(겸)","");
            }
            else {
                nameStr = arrSubValue[2];
            }

            userName = arrSubValue[5] + " " + nameStr;
            userID = arrSubValue[1];
            deptID = arrSubValue[3];
            deptName = arrSubValue[11];
        }
        

        $("#tbReceiveMan").val(userName);
        $("#hdnUserID").val(userID);
        $("#hdnDeptID").val(deptID);
        $("#hdnDeptName").val(deptName);

    }
}

/**************************************************
작성목적 : 검색의 엔터키이벤트
***************************************************/
GetUserInfo.KeyEvents = function () {
    if (event.keyCode == 13) {
        SearchDeptUserList(1);
    }
}

/**************************************************
작성목적 : 조직도 검색
***************************************************/
function SearchDeptUserList(param) {
    var searchName = $("#DeptUserTreeScript1_tbDeptUser").val().securityText();
    $("#DeptUserTreeScript1_tbDeptUser").val(searchName);

    var params = searchName + "" + param + "" + CompanyID;
    DeptUserTreeScript1_CallBack1.Callback(params);
}

/**************************************************
작성목적 : 확인
***************************************************/
GetUserInfo.Confirm = function () {

    var userName = document.getElementById("tbReceiveMan").value;
    var userID = document.getElementById("hdnUserID").value;
    var deptID = document.getElementById("hdnDeptID").value;
    var deptName = document.getElementById("hdnDeptName").value;

    if (userName != "") {

        top.opener.GetUserInfo(userID, userName, deptName, deptID);

        GetUserInfo.Cancel();
    }
    else {
        alert(R_MsgPerson);
    }
}

/**************************************************
작성목적 : 취소
***************************************************/
GetUserInfo.Cancel = function () {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}


/**************************************************
작성목적 : 문서양식안에 지정되어 있는 업무대행자 지우기
***************************************************/
GetUserInfo.Clean = function () {

    top.opener.CleanUserInfo();

    GetUserInfo.Cancel();
}

