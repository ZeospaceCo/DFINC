/// <reference path="../../../../Html/Js/jQuery/jquery.js" />

Send = {}

var AuthArray = []; //현 권한
var CompareArray = []; //추가된 권한
var DelArray = []; //삭제할 권한

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        /* 2017-02-10 제거
        //자동화완성 autocomplete - written by shimpark
        $("#DeptUserTreeScript1_tbDeptUser").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/DeskPlusEIP/Account/Ws/Common/GetUserDept.asmx/GetDeptUserSearch",
                    data: "{ 'companyID': '" + CompanyID + "','gubun': 'M','searchName': '" + request.term + "' }",
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

        //검색입력필드 엔터 이벤트 기술
        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (event) {
            Send.KeyEvents();
        });

        //db 에서 권한 값을 권한배열에 담는다.
        AuthArray = [];
        var AuthArrayListStr = AuthArrayList.split(';');
        for (i = 0; i < AuthArrayListStr.length; i++) {
            AuthArray.push({
                'id': AuthArrayListStr[i].split("$")[0] + "$" + AuthArrayListStr[i].split("$")[1],
                'name': AuthArrayListStr[i].split("$")[2]
            });
        }

        //확인
        $('#CbtnSubmit').click(function (e) {
            e.preventDefault();
            Send.Confirm();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //추가
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            Send.Add();
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
작성자   : shine
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
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 엔터키이벤트
***************************************************/
Send.KeyEvents = function () {
    if (event.keyCode == 13) {
        SearchDeptUserList(1);
    }
}

/**************************************************
작성자   : shine
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

    // type(G) | deptID | deptName | parentDeptID | titleID | titleName | loginID | mail | Phone | officephone | DutyName
    // type(M) | userID | userName | parentDeptID | titleID | titleName | loginID | mail | Phone | officephone | DutyName
    $("#tvSelectedValue").val(selValues);
}

/**************************************************
작성자   : shine
작성목적 : 권한 배열값 저장소; 비교 대상자 배열값
***************************************************/
Send.AddAuthArray = function (keyID, keyName) {
    AuthArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : shine
작성목적 : 조직도 넘어온 값 저장소, 차후 비교값
***************************************************/
Send.AddCompareArray = function (keyID, keyName) {
    CompareArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : shine
작성목적 : 권한 배열값에서 특정값 삭제 : table row 삭제시 사용됨
***************************************************/
Send.removeByValue = function (val) {
    for (var i = 0; i < AuthArray.length; i++) {
        if (AuthArray[i].id == val.id) {
            AuthArray.splice(i, 1);
            break;
        }
    }
}

/**************************************************
작성자   : shine
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
}

/**************************************************
작성자   : shine
작성목적 : 사용자 추가 버튼
***************************************************/
Send.Add = function () {
    var selectedID = $("#tvSelectedID").val().trimEnds(';');
    var selectedValue = $("#tvSelectedValue").val().trimEnds(';');
    CompareArray = [];

    if (selectedID != "" && selectedValue != "") {
        var selStr = selectedValue.split(';');
        for (i = 0; i < selStr.length; i++) {
            var selOwnerID = selStr[i].split('|')[1];
            var selName = selStr[i].split('|')[2];

            //1. 넘어온 사용자 배열에 할당하기(1$영업부,영업부)
            Send.AddCompareArray(selOwnerID, selName);
        }

        //2. 넘어온 권한값(CompareArray[]) 배열과 권한값(AuthArray[]) 비교하기
        $.each(CompareArray, function (i, v) {
            Send.CompareArrayVal(v.id, v.name);
        });
    }
    else {
        alert(R_SelectSendAddressUser);
    }
}

/**************************************************
작성자   : shine
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
Send.CompareArrayVal = function (compID, compName) {
    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (AuthArray.length > 0) {
        for (var i = 0; i < AuthArray.length; i++) {
            //alert("AuthArray[i].id : [" + i + "]" + v.id + " | compArr.id : " + compArr.id);

            if (AuthArray[i].id == compID) {
                response = 1; //동일한 값이 존재할 경우
                break;
            }
            else {
                response = 0; //다른 값일 경우
            }
        }

        if (response == 0) {
            //중복되지 않으면 권한값 할당하기
            Send.AddAuthRow(compID, compName);
        }
    }
}

/**************************************************
작성자   : shine
작성목적 : 사용자 추가 table 생성하기
***************************************************/
Send.AddAuthRow = function (keyID, name) {
    var rows = $("#contentTable tr");
    RowMax = RowMax + 1;

    var selStr = keyID.split('$');

    $("#contentTable").append(
		"<tr id='addressName" + RowMax + "'>"
			+ "<td class='center'>"
			+ name
            + "</td>"
            + "<td class='center'><a class=btn onclick=Send.DelUserRow(" + RowMax + ",'" + keyID + "','" + name + "')>" + R_Del + "</a></td>"
			//+ "<td class='center'><a onclick=Send.DelUserRow(" + RowMax + ",'" + keyID + "','" + name + "')><img src='" + ImagePath + "/icon/ico_delete.gif' /></a></td>"
		+ "</tr>"
	);

    Send.AddAuthArray(keyID, name)
}

/**************************************************
작성자   : shine
작성목적 : 선택된 사용자 table 특정 row 삭제하기
***************************************************/
Send.DelUserRow = function (index, keyID, keyName) {
    var row = $("#addressName" + index);
    row.fadeOut("slow", function () {
        row.remove();
    });

    DelArray = [];

    DelArray.push({
        'id': keyID,
        'name': keyName
    });

    $.each(DelArray, function (i, v) {
        Send.removeByValue(v);
    });
}

/**************************************************
작성자   : shine
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
}

/**************************************************
작성자   : shine
작성일자 : 2011-01-26
작성목적 : 처리하기
***************************************************/
Send.Confirm = function () {
    var userList = "";
    $.each(AuthArray, function (i, v) {
        if (v.id != "" && v.id != "$undefined") {
            userList += v.id + ",";
        }
    });

    if (userList != "") userList = userList.trimEnds(",");

    if (userList == "") {
        alert(R_SelectSendAddressUser);
        return;
    }

    var url = url = "/DeskPlusEIP/Address/Ws/Content/Send.asmx/AddPrivateReceived";

    ParamArray = {
        'userID': userList,
        'listID': ListID
    };

    var obj = eval("Send.CompleteSend");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : shine
작성일자 : 2011-01-26
작성목적 : 주소록 전달 완료
***************************************************/
Send.CompleteSend = function (data, status) {
    if (data.d) {
        alert(R_CompleteAddressSend);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else {
        alert(AfterProcessingError);
    }
}