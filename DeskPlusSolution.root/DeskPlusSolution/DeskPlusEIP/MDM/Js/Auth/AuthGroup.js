AuthGroup = {}


var AuthArray = []; //db 에서 권한 값을 권한배열에 담는다.
var CompareArray = []; //넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
var DelArray = []; //삭제할 값 배열에 담기.

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //검색입력필드 엔터 이벤트 기술
        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (event) {
            AuthGroup.KeyEvents();
        });

        //db 에서 권한 값을 권한배열에 담는다.        
        var AuthArrayListStr = AuthArrayList.split(';');
        for (i = 0; i < AuthArrayListStr.length; i++) {
            if (AuthArrayListStr[i].split("$")[1] != undefined) {
                var keyid = AuthArrayListStr[i].split("$")[0] + "$" + AuthArrayListStr[i].split("$")[1];
                var keyName = AuthArrayListStr[i].split("$")[2];
                AuthGroup.AddAuthArray(keyid, keyName);
            }
        }

        //저장
        $('#CbtnSave').click(function (e) {
            e.preventDefault();
            AuthGroup.Save();
        });

        //추가
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            AuthGroup.AddAuth();
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
작성일자 : 2015-10-21
작성목적 : 권한추가 버튼
***************************************************/
AuthGroup.AddAuth = function () {

    var selectedID = $("#tvSelectedID").val().trimEnds('|');
    var selectedValue = $("#tvSelectedValue").val().trimEnds('$');

    if (selectedID != "" && selectedValue != "") {

        CompareArray = []; //넘어온 권한값

        var selStr = selectedValue.split('$');

        for (i = 0; i < selStr.length; i++) {
            var selGubun = selStr[i].split('|')[0];
            var selOwnerID = selStr[i].split('|')[1];

            var selName = selStr[i].split('|')[2]; //리스트박스에서 선택한 이름은 부서,직급 모두 붙어있으므로 잘라줘야 함    
            if (selName.indexOf('/') > 1) {
                selName = selName.split('/')[0]
            }

            //1. 넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
            AuthGroup.AddCompareArray(selGubun + "$" + selOwnerID, selName);
        }

        //2. 넘어온 권한값(CompareArray[]) 배열과 권한값(AuthArray[]) 비교하기
        $.each(CompareArray, function (i, v) {
            AuthGroup.CompareArrayVal(v.id, v.name);
        });

    }
    else {
        alert(R_CheckUserGroup);
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
AuthGroup.CompareArrayVal = function (compID, compName) {

    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (AuthArray.length > 0) {
        for (var i = 0; i < AuthArray.length; i++) {
        
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
            AuthGroup.AddAuthRow(compID, compName);
        }
    }
    else {
        response = 0; //다른 값일 경우
        AuthGroup.AddAuthRow(compID, compName);
    }

    return response;
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 추가 table row 생성하고 권한값 AuthArray[] 배열에 할당하기
***************************************************/
AuthGroup.AddAuthRow = function (keyID, name) {
    var rows = $("#contentTable tr");
    RowMax = RowMax + 1;

    var selStr = keyID.split('$');

    $("#contentTable").append(
		"<tr id='authGroup" + RowMax + "'>"
			+ "<td class='center'>"
			+ unescape(name)
			+ "</td>"
            + "<td class='center'><a class=btn onclick=\"AuthGroup.DelAuthRow(" + RowMax + ",'" + keyID + "','" + name + "')\">" + R_Del + "</a></td>"
			//+ "<td class='center'><a onclick=\"AuthGroup.DelAuthRow(" + RowMax + ",'" + keyID + "','" + name + "')\"><img src='" + ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif' /></a></td>"
		+ "</tr>"
	);

    AuthGroup.AddAuthArray(keyID, name)
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 권한값 table 특정 row 삭제하기
***************************************************/
AuthGroup.DelAuthRow = function (index, keyID, keyName) {
    var row = $("#authGroup" + index);

    for (var i = 0; i < AuthArray.length; i++) {

        if (AuthArray[i].id == keyID) {

            AuthArray.splice(i, 1);

            row.fadeOut("slow", function () {
                row.remove();
            });

            break;
        }
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 권한 배열값 저장소; 비교 대상자 배열값
***************************************************/
AuthGroup.AddAuthArray = function (keyID, keyName) {

    AuthArray.push({
        'id': keyID,
        'name': escape(keyName)
    });
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 조직도 넘어온 값 저장소, 차후 비교값
***************************************************/
AuthGroup.AddCompareArray = function (keyID, keyName) {
    CompareArray.push({
        'id': keyID,
        'name': escape(keyName)
    });
}



/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 엔터키이벤트
***************************************************/
AuthGroup.KeyEvents = function () {
    if (event.keyCode == 13) {
        SearchDeptUserList(1);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
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
작성일자 : 2015-10-21
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

        selValues = selValues.replace(selValue + "$", "");
        selValues += selValue + "$";
    } else {
        selIDs = selIDs.replace(selID + "|", "");
        selValues = selValues.replace(selValue + "$", "");
    }

    $("#tvSelectedID").val(selIDs);

    // type(G) | deptID | deptName | parentDeptID | titleID | titleName | loginID | mail | Phone | officephone | DutyName
    // type(M) | userID | userName | parentDeptID | titleID | titleName | loginID | mail | Phone | officephone | DutyName
    $("#tvSelectedValue").val(selValues);

}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
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
        selValues = selValues.replace(selValue + "$", "");
        selValues += selValue + "$";
    });

    // type(G) | deptID | deptName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName
    // type(M) | userID | userName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName

    $("#tvSelectedID").val(selIDs);
    $("#tvSelectedValue").val(selValues);
}



/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 저장하기
***************************************************/
AuthGroup.Save = function () {

    var selectedValue = "";

    for (var i = 0; i < AuthArray.length; i++) {

        selectedValue += AuthArray[i].id + ",";
    }


    $("#tbAuthValue").val(selectedValue.trimEnds(","));

    __doPostBack("LBTNProcess", "Register");
}

