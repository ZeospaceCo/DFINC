GetUserDept = {}

var BeforeArray = []; //db 에서 권한 값을 권한배열에 담는다.
var CompareArray = []; //넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
var DelArray = []; //삭제할 값 배열에 담기.

/**************************************************/
$(function () {
    $(document).ready(function () {

        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (event) {
            GetUserDept.KeyEvents();
        });

        //추가
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            GetUserDept.Add();
        });

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

        //트리검색(검색버튼)
        $('#CbtnDeptUserSearch').click(function (e) {
            e.preventDefault();
            SearchDeptUserList(1);
        });

        //트리검색(검색취소)
        $('#CbtnTreeView').click(function (e) {
            e.preventDefault();

            $("#tvSelectedID").val('');
            $("#tvSelectedValue").val('');

            SearchDeptUserList(2);
        });
    });
});

/*************************************************/
GetUserDept.KeyEvents = function () {
    if (event.keyCode == 13) {
        SearchDeptUserList(1);
    }
}

/***************************************************/
function SearchDeptUserList(param) {
    var searchName = $("#DeptUserTreeScript1_tbDeptUser").val().securityText();
    $("#DeptUserTreeScript1_tbDeptUser").val(searchName);

    var params = searchName + "" + param + "" + CompanyID;
    DeptUserTreeScript1_CallBack1.Callback(params);
}

/***************************************************/
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
}

/*****************************************************/
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

        selIDs = selIDs.replace(selID + "|", "");
        selIDs += selID + "|";
        selValues = selValues.replace(selValue + ";", "");
        selValues += selValue + ";";
        
    });

    $("#tvSelectedID").val(selIDs);
    $("#tvSelectedValue").val(selValues);

}

/***************************************************/
GetUserDept.DelAuthRow = function (index, keyID) {
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

/*************************************************/
GetUserDept.AddBeforeArray = function (keyID, keyName) {
    BeforeArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************/
GetUserDept.AddCompareArray = function (keyID, keyName) {
    CompareArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************/
GetUserDept.CompareArrayVal = function (compID, compName) {
    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (BeforeArray.length > 0) {
        for (var i = 0; i < BeforeArray.length; i++) {

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
            GetUserDept.AddAuthRow(compID, compName);
        }
    }
    else {
        response = 0; //다른 값일 경우
        GetUserDept.AddAuthRow(compID, compName);
    }

    return response;
}

/****************************************************/
GetUserDept.AddAuthRow = function (keyID, name) {
    var rows = $("#contentTable tr");
    RowMax = RowMax + 1;

    $("#contentTable").append(
		"<tr id='taskSend" + RowMax + "'>"
			+ "<td class='center'>"
			+ name.split("$")[1]
			+ "</td>"
			+ "<td class='center'><a class=btn onclick=GetUserDept.DelAuthRow(" + RowMax + ",'" + keyID + "')>삭제</a></td>"
		+ "</tr>"
	);

    GetUserDept.AddBeforeArray(keyID, name)
}

/**************************************************/
GetUserDept.Add = function () {
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
                if (selName.indexOf('/') > 1) {
                    selName = selName.split('/')[0]
                }

                selDeptID = selStr[i].split('|')[1];
            }

            //1. 넘어온 권한값 배열에 할당하기(G$1,영업부)
            GetUserDept.AddCompareArray(selGubun + "$" + selOwnerID, selDeptID + "$" + selName);
        }

        //2. 넘어온 권한값(CompareArray[]) 배열과 이미 들어온 값(BeforeArray[]) 비교하기
        $.each(CompareArray, function (i, v) {
            GetUserDept.CompareArrayVal(v.id, v.name);
        });
    }
    else {
         alert(R_SelectAdd);
    }
}

/**************************************************/
GetUserDept.Confirm = function () {
    if (BeforeArray.length > 0) {
        var selectedValue = "";

        for (var i = 0; i < BeforeArray.length; i++) {
            selectedValue += BeforeArray[i].id + "$" + BeforeArray[i].name + ";";
        }

        selectedValue = selectedValue.trimEnds(';');

        if (parent.disablePopup) {
            if (eval("parent." + CallBackEvent)(selectedValue) === false) return;
            parent.disablePopup();
        } else {
            if (eval("opener." + CallBackEvent)(selectedValue) === false) return;
            self.close();
        }
    }
    else {
         alert(R_SelectAdd);
    }
}
