AddReserveUser = {}

var ResourceUserArray = []; //db 에서 권한 값을 권한배열에 담는다.
var CompareArray = []; //넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
var DelArray = []; //삭제할 값 배열에 담기.

/**************************************************
작성자   : amk0601
작성일자 : 2015-02-13
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //검색입력필드 엔터 이벤트 기술
        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (event) {
            AddReserveUser.KeyEvents();
        });

        //추가
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            AddReserveUser.Add();
        });

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            AddReserveUser.Confirm();
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
작성자   : amk0601
작성일자 : 2015-02-13
작성목적 : 엔터키이벤트
***************************************************/
AddReserveUser.KeyEvents = function () {
    if (event.keyCode == 13) {
        SearchDeptUserList(1);
    }
}

/**************************************************
작성자   : amk0601
작성일자 : 2015-02-13
작성목적 : 조직도 검색
***************************************************/
function SearchDeptUserList(param) {
    var searchName = $("#DeptUserTreeScript1_tbDeptUser").val().securityText();
    $("#DeptUserTreeScript1_tbDeptUser").val(searchName);

    var params = searchName + "" + param + "" + CompanyID;
    DeptUserTreeScript1_CallBack1.Callback(params);
}

/**************************************************
작성자   : amk0601
작성일자 : 2015-02-13
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
}

/**************************************************
작성자   : amk0601
작성일자 : 2015-02-13
작성목적 : listbox 선택한 값 가져오기
***************************************************/
function LbxDeptUser_OnChange() {
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

    $("#tvSelectedID").val(selIDs);
    $("#tvSelectedValue").val(selValues);
}

/**************************************************
작성자   : amk0601
작성일자 : 2015-02-13
작성목적 : 권한값 table 특정 row 삭제하기
***************************************************/
AddReserveUser.DelAuthRow = function (index, keyID) {
    var row = $("#webHardSend" + index);

    for (var i = 0; i < ResourceUserArray.length; i++) {
        if (ResourceUserArray[i].id == keyID) {
            ResourceUserArray.splice(i, 1);

            row.remove();

            break;
        }
    }
}

/**************************************************
작성자   : amk0601
작성일자 : 2015-02-13
작성목적 : 권한 배열값 저장소; 비교 대상자 배열값
***************************************************/
AddReserveUser.AddResourceUserArray = function (keyID, keyName) {
    ResourceUserArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : amk0601
작성일자 : 2015-02-13
작성목적 : 조직도 넘어온 값 저장소, 차후 비교값
***************************************************/
AddReserveUser.AddCompareArray = function (keyID, keyName) {
    CompareArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : amk0601
작성일자 : 2015-02-13
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
AddReserveUser.CompareArrayVal = function (compID, compName) {
    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (ResourceUserArray.length > 0) {
        for (var i = 0; i < ResourceUserArray.length; i++) {
            if (ResourceUserArray[i].id == compID) {
                response = 1; //동일한 값이 존재할 경우
                break;
            }
            else {
                response = 0; //다른 값일 경우
            }
        }

        if (response == 0) {
            //중복되지 않으면 권한값 할당하기
            AddReserveUser.AddAuthRow(compID, compName);
        }
    }
    else {
        response = 0; //다른 값일 경우
        AddReserveUser.AddAuthRow(compID, compName);
    }

    return response;
}

/**************************************************
작성자   : amk0601
작성일자 : 2015-02-13
작성목적 : 추가 table row 생성하고 권한값 ResourceUserArray[] 배열에 할당하기
***************************************************/
AddReserveUser.AddAuthRow = function (keyID, name) {
    var rows = $("#contentTable tr");
    RowMax = RowMax + 1;

    $("#contentTable").append(
		"<tr id='webHardSend" + RowMax + "'>"
			+ "<td class='center'>"
			+ name
			+ "</td>"
            + "<td class='center'><a class=btn onclick=AddReserveUser.DelAuthRow(" + RowMax + ",'" + keyID + "')>" + R_Del + "</a></td>"
			//+ "<td class='center'><a onclick=AddReserveUser.DelAuthRow(" + RowMax + ",'" + keyID + "')><img src='" + ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif' style='cursor:hand' /></a></td>"
		+ "</tr>"
	);

    AddReserveUser.AddResourceUserArray(keyID, name)
}

/**************************************************
작성자   : amk0601
작성일자 : 2015-02-13
작성목적 : 추가 버튼
***************************************************/
AddReserveUser.Add = function () {
    var selectedID = $("#tvSelectedID").val().trimEnds('|');
    var selectedValue = $("#tvSelectedValue").val().trimEnds(';');

    if (selectedID != "" && selectedValue != "") {
        CompareArray = []; //넘어온 권한값

        var selStr = selectedValue.split(';');

        for (i = 0; i < selStr.length; i++) {
            
            var selGubun = selStr[i].split('|')[0];
            var selOwnerID = selStr[i].split('|')[1];
            var selName = selStr[i].split('|')[2].split('/')[0];

            AddReserveUser.AddCompareArray(selGubun + "$" + selOwnerID, selName);
        }

        //2. 넘어온 권한값(CompareArray[]) 배열과 이미 들어온 값(ResourceUserArray[]) 비교하기
        $.each(CompareArray, function (i, v) {
            AddReserveUser.CompareArrayVal(v.id, v.name);
        });
    }
    else {
        alert(R_SelectAddAtte);
    }
}

/**************************************************
작성자   : amk0601
작성일자 : 2015-02-13
작성목적 : 확인
***************************************************/
AddReserveUser.Confirm = function () {
    if (ResourceUserArray.length > 0) {
        var selectedValue = "";

        for (var i = 0; i < ResourceUserArray.length; i++) {
            selectedValue += ResourceUserArray[i].id + "|" + ResourceUserArray[i].name + ";";
        }

        selectedValue = selectedValue.trimEnds(';');

        if (parent.disablePopup) {
            eval("parent." + CallBackEvent)(selectedValue);
            parent.disablePopup();
        } else {
            eval("opener." + CallBackEvent)(selectedValue);
            self.close();
        }
    }
    else {
        alert(R_SelectAddAtte);
    }
}