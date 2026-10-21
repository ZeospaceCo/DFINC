ForwardUser = {}

var BeforeArray = []; //db 에서 권한 값을 권한배열에 담는다.
var CompareArray = []; //넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
var DelArray = []; //삭제할 값 배열에 담기.

/**************************************************
작성자   : famo23
작성일자 : 2015-04-21
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //검색입력필드 엔터 이벤트 기술
        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (event) {
            ForwardUser.KeyEvents();
        });

        //추가
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            ForwardUser.Add();
        });

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            ForwardUser.Confirm();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
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
작성자   : famo23
작성일자 : 2015-04-21
작성목적 : 엔터키이벤트
***************************************************/
ForwardUser.KeyEvents = function () {
    if (event.keyCode == 13) {
        SearchDeptUserList(1);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-21
작성목적 : 조직도 검색
***************************************************/
function SearchDeptUserList(param) {
    var searchName = $("#DeptUserTreeScript1_tbDeptUser").val().securityText();
    $("#DeptUserTreeScript1_tbDeptUser").val(searchName);

    var params = searchName + "" + param + "" + CompanyID;
    DeptUserTreeScript1_CallBack1.Callback(params);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-21
작성목적 : 조직도 선택시
***************************************************/
function DeptUserTreeScript_onNodeSelect(node) {

    $("#tvSelectedID").empty();
    $("#tvSelectedValue").empty();

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
작성자   : famo23
작성일자 : 2015-04-21
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
        var selID = colSplit[0]; //nodeID
        var selValue = colSplit[1]; //nodeValue

        if ((Gubun == "2") && (selValue.split("|")[0] == "G")) {
            //alert(R_ForwardDept);
        }
        else {
            selIDs = selIDs.replace(selID + "|", "");
            selIDs += selID + "|";
            selValues = selValues.replace(selValue + ";", "");
            selValues += selValue + ";";
        }
    });

    // type(G) | deptID | deptName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName
    // type(M) | userID | userName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName

    $("#tvSelectedID").val(selIDs);
    $("#tvSelectedValue").val(selValues);

}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-21
작성목적 : 권한값 table 특정 row 삭제하기
***************************************************/
ForwardUser.DelAuthRow = function (index, keyID) {
    var row = $("#taskSend" + index);

    for (var i = 0; i < BeforeArray.length; i++) {
        if (BeforeArray[i].id == keyID) {
            BeforeArray.splice(i, 1);

            row.fadeOut("slow", function () {
                row.remove();
            });

            break;
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-21
작성목적 : 권한 배열값 저장소; 비교 대상자 배열값
***************************************************/
ForwardUser.AddBeforeArray = function (keyID, keyName) {
    BeforeArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-21
작성목적 : 조직도 넘어온 값 저장소, 차후 비교값
***************************************************/
ForwardUser.AddCompareArray = function (keyID, keyName) {
    CompareArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-21
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
ForwardUser.CompareArrayVal = function (compID, compName) {
    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (BeforeArray.length > 0) {
        for (var i = 0; i < BeforeArray.length; i++) {
            //alert("BeforeArray[i].id : [" + i + "]" + v.id + " | compArr.id : " + compArr.id);

            if (BeforeArray[i].id == compID) {
                response = 1; //동일한 값이 존재할 경우
                break;
            }
            else {
                response = 0; //다른 값일 경우
            }
        }

        if (response == 0) {
            //중복되지 않으면 권한값 할당하기
            ForwardUser.AddAuthRow(compID, compName);
        }
    }
    else {
        response = 0; //다른 값일 경우
        ForwardUser.AddAuthRow(compID, compName);
    }

    return response;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-21
작성목적 : 추가 table row 생성하고 권한값 BeforeArray[] 배열에 할당하기
***************************************************/
ForwardUser.AddAuthRow = function (keyID, name) {
    var rows = $("#contentTable tr");
    RowMax = RowMax + 1;

    $("#contentTable").append(
		"<tr id='taskSend" + RowMax + "'>"
			+ "<td class='center'>"
			+ name.split("$")[1]
			+ "</td>"
            //+ "<td class='center'><a onclick=ForwardUser.DelAuthRow(" + RowMax + ",'" + keyID + "')><img src='" + ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif' style='cursor:hand' /></a></td>"
            + "<td class='center'><a class='btn' onclick=ForwardUser.DelAuthRow(" + RowMax + ",'" + keyID + "')>" + R_Del + "</a></td>"
		+ "</tr>"
	);

    ForwardUser.AddBeforeArray(keyID, name)
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-21
작성목적 : 추가 버튼
***************************************************/
ForwardUser.Add = function () {
    var selectedID = $("#tvSelectedID").val().trimEnds('|');
    var selectedValue = $("#tvSelectedValue").val().trimEnds(';');

    if (selectedID != "" && selectedValue != "") {
        CompareArray = []; //넘어온 권한값

        //if (BeforeArray.length == 0) {
        var selStr = selectedValue.split(';');

        //if (selStr.length == 1) {
        for (i = 0; i < selStr.length; i++) {

            var selGubun = selStr[i].split('|')[0];
            var selOwnerID = selStr[i].split('|')[1];
            var selName = "";
            var selDeptID = "";
            if (selGubun == "M") {
                selName = selStr[i].split('|')[11] + "/" + selStr[i].split('|')[5] + "/" + selStr[i].split('|')[2].split('/')[0];
                selDeptID = selStr[i].split('|')[3];
            }
            else {
                selName = selStr[i].split('|')[2];
                selDeptID = selStr[i].split('|')[1];
            }

            //1. 넘어온 권한값 배열에 할당하기(1,영업부)
            ForwardUser.AddCompareArray(selOwnerID, selDeptID + "$" + selName);

        }

        //2. 넘어온 권한값(CompareArray[]) 배열과 이미 들어온 값(BeforeArray[]) 비교하기
        $.each(CompareArray, function (i, v) {
            ForwardUser.CompareArrayVal(v.id, v.name);
        });
    }
    else {
        alert(R_SelectAdd + "(" + R_ForwardDept + ")");        
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-21
작성목적 : 확인
***************************************************/
ForwardUser.Confirm = function () {
    if (BeforeArray.length > 0) {
        var selectedValue = "";

        for (var i = 0; i < BeforeArray.length; i++) {
            selectedValue += BeforeArray[i].id + ",";
        }

        selectedValue = selectedValue.trimEnds(',');

        ParamArray = {
            'listID': ListID,
            'userList': selectedValue
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ForwardUser.SuccessConfirm");
        var url = "/DeskPlusEIP/Task/Ws/Common/ForwardUser.asmx/AddForward";

        AjaxService(url, data, obj);
    }
    else {
        alert(R_SelectAdd + "(" + R_ForwardDept + ")");
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-21
작성목적 : 확인
**********************************************************************************************/
ForwardUser.SuccessConfirm = function (data, status) {
    if (data.d == true) {
        alert(R_CompleteConfirm);

        if (parent.disablePopup) {
            parent.disablePopup();
        } else {
            self.close();
        }
    }
    else {
        alert(R_ErrorProcessing);
    }
}