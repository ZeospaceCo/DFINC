GetUserDept = {}

$(function () {
    $(document).ready(function () {

        /* 2017-02-10 제거
        //자동화완성 autocomplete - written by shimpark
        $("#DeptUserTreeScript1_tbDeptUser").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/DeskPlusEIP/Account/Ws/Common/GetUserDept.asmx/GetDeptUserSearch",
                    data: "{ 'companyID': '" + CompanyID + "','gubun': '','searchName': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        //Callback method when the precess is success
                        response($.map(data.d, function (item) {
                            return {
                                label: item.Name,
                                value: item.Values
                            }
                        }))
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Communication Error");
                    }
                });
            },
            focus: function (event, ui) {
                // prevent autocomplete from updating the textbox
                event.preventDefault();
                // manually update the textbox
                $(this).val(ui.item.value);
            },
            select: function (event, ui) {
                // prevent autocomplete from updating the textbox
                event.preventDefault();
                // manually update the textbox and hidden field
                $(this).val(ui.item.value);
                $("#DeptUserTreeScript1_tbDeptUser").val(ui.item.value);
            },
            minLength: 1
        }).autocomplete("widget").addClass("fixed-height");
        */

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            GetUserDept.Confirm();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //검색입력필드 엔터 이벤트 기술
        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (event) {
            GetUserDept.KeyEvents();
        });

        //트리검색(검색버튼)
        $('#CbtnDeptUserSearch').click(function (e) { e.preventDefault(); GetUserDept.SearchDeptUserList(1); });

        //트리검색(검색취소)
        $('#CbtnTreeView').click(function (e) { e.preventDefault(); GetUserDept.SearchDeptUserList(2); });

    });
});

/**************************************************
작성자   : withsuki
작성목적 : 조직도 검색
***************************************************/
GetUserDept.SearchDeptUserList = function(param) {
    var searchName = $("#DeptUserTreeScript1_tbDeptUser").val().securityText();
    $("#DeptUserTreeScript1_tbDeptUser").val(searchName);

    var params = searchName + "" + param + "" + CompanyID;
    DeptUserTreeScript1_CallBack1.Callback(params);
}

/**************************************************
작성자   : withsuki
작성목적 : 엔터키이벤트
***************************************************/
GetUserDept.KeyEvents = function () {
    if (event.keyCode == 13) {
        GetUserDept.SearchDeptUserList(1);
    }
}

/**************************************************
작성자   : withsuki
작성목적 : 조직도 선택시
***************************************************/
function DeptUserTreeScript_onNodeSelect(node) {

    var selIDs = $("#tvSelectedID").val();
    var selValues = $("#tvSelectedValue").val();
    var selID = node.ID;
    var selValue = node.Value;

    if (node.Checked) {
        selIDs = selIDs.replace(selID + "|", "");
        selIDs += selID + "|";

        selValues = selValues.replace(selValue + ";", "");
        selValues += selValue + ";";
    } else {
        selIDs = selIDs.replace(selID + "|", "");
        selValues = selValues.replace(selValue + ";", "");
    }

    $("#tvSelectedID").val(selIDs);
    $("#tvSelectedValue").val(selValues);
    // type(G) | deptID | deptName | parentDeptID | titleID | titleName | loginID | mail | Phone | officephone | DutyName
    // type(M) | userID | userName | parentDeptID | titleID | titleName | loginID | mail | Phone | officephone | DutyName
}

/**************************************************
작성자   : withsuki
작성목적 : listbox 선택한 값 가져오기
***************************************************/
function LbxDeptUser_OnChange() {

    $("#tvSelectedID").empty();
    $("#tvSelectedValue").empty();

    var selIDs = "", selValues = "";

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
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 확인 
***************************************************/
GetUserDept.Confirm = function () {

    var selectedID = $("#tvSelectedID").val().trimEnds(';');
    var selectedValue = $("#tvSelectedValue").val().trimEnds(';');

    if (selectedID == "" || selectedValue == "") { alert(R_Msg_Sel); return; }

    if (window.confirm(R_Msg_Add)) {

        if (Func != "") {
            if (eval("opener." + Func)) {
                eval("opener." + Func)(selectedValue); self.close();
            } else {
                alert(R_Msg_Admin);
            }
        }
    }
}