
AddAuth = {}

var AuthArray = []; //db 에서 권한 값을 권한배열에 담는다.
var CompareArray = []; //넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
var DelArray = []; //삭제할 값 배열에 담기.

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //db 에서 권한 값을 권한배열에 담는다.        
        var AuthArrayListStr = AuthArrayList.split(';');
        for (i = 0; i < AuthArrayListStr.length; i++) {
            if (AuthArrayListStr[i].split("$")[1] != undefined) {
                var keyid = AuthArrayListStr[i].split("$")[0] + "$" + AuthArrayListStr[i].split("$")[1];
                var keyName = AuthArrayListStr[i].split("$")[2];
                AddAuth.AddAuthArray(keyid, keyName);
            }
        }

        //권한추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            AddAuth.Add();
        });

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            AddAuth.Process();
        });

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

    });
});


/**************************************************
작성자   : famo23
작성목적 : 권한추가 버튼
***************************************************/
AddAuth.Add = function () {

    var selectedID = $("#tvSelectedID").val().trimEnds('|');
    //deptID | deptName | parentDeptID | deptOrder
    var selectedValue = $("#tvSelectedValue").val().trimEnds('$');

    if (selectedID != "" && selectedValue != "") {

        CompareArray = []; //넘어온 권한값

        var selStr = selectedValue.split('$');
        for (i = 0; i < selStr.length; i++) {
            var selOwnerID = selStr[i].split('|')[0];
            var selName = selStr[i].split('|')[1];

            //1. 넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
            AddAuth.AddCompareArray(selOwnerID, selName);
        }

        //2. 넘어온 권한값(CompareArray[]) 배열과 권한값(AuthArray[]) 비교하기
        $.each(CompareArray, function (i, v) {
            AddAuth.CompareArrayVal(v.id, v.name);
        });

    }
    else {
        alert(R_SettingAuthority);
    }
}


/**************************************************
작성자   : famo23
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
AddAuth.CompareArrayVal = function (compID, compName) {

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
            AddAuth.AddAuthRow(compID, compName);
        }
    }
    else {
        response = 0; //다른 값일 경우
        AddAuth.AddAuthRow(compID, compName);
    }

    return response;
}


/**************************************************
작성자   : famo23
작성목적 : 추가 table row 생성하고 권한값 AuthArray[] 배열에 할당하기
***************************************************/
AddAuth.AddAuthRow = function (keyID, name) {
    var rows = $("#contentTable tr");
    RowMax = RowMax + 1;

    $("#contentTable").append(
		"<tr id='boardAuth" + RowMax + "'>"
			+ "<td class='center'>"
			+ name
			+ "</td>"
            + "<td class='center'>"
			+ "<select name='ddlListType' id='ddlListType" + RowMax + "' class='searchselect form-control'>"
			+ "	<option selected='selected' value='" + keyID + "'>" + R_Apply + "</option>"
			+ "</select></td>"
            + "<td class='center'><a class=\"btn\" onclick=\"AddAuth.DelAuthRow(" + RowMax + ",'" + keyID + "','" + name + "')\">" + R_Del + "</a></td>"
			//+ "<td class='center'><a onclick=\"AddAuth.DelAuthRow(" + RowMax + ",'" + keyID + "','" + name + "')\"><img src='" + ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif' /></a></td>"
		+ "</tr>"
	);

    AddAuth.AddAuthArray(keyID, name)
}


/**************************************************
작성자   : famo23
작성목적 : 권한값 table 특정 row 삭제하기
***************************************************/
AddAuth.DelAuthRow = function (index, keyID, keyName) {
    var row = $("#boardAuth" + index);

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
작성목적 : 권한 배열값 저장소; 비교 대상자 배열값
***************************************************/
AddAuth.AddAuthArray = function (keyID, keyName) {

    AuthArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : famo23
작성목적 : 조직도 넘어온 값 저장소, 차후 비교값
***************************************************/
AddAuth.AddCompareArray = function (keyID, keyName) {
    CompareArray.push({
        'id': keyID,
        'name': keyName
    });
}


/**************************************************
작성자   : famo23
작성일자 : 2014-01-17
작성목적 : 처리하기
***************************************************/
AddAuth.Process = function () {

    //권한값 추출해 오기
    var authVal = "";
    $("#contentTable select option:selected").each(function () {
        authVal += $(this).val() + ",";
    });

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'menuID': MenuID,
        'roleList': authVal.trimEnds(",")
    };

    var url = "/DeskPlusEIP/MDM/Ws/MenuAuth/AddAuth.asmx/AddMenuRole";
    var obj = eval("AddAuth.SuccessAddAuth");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-01-17
작성목적 : 저장 결과값 반환
**********************************************************************************************/
AddAuth.SuccessAddAuth = function (data, status) {
    if (data.d == true) {
        parent.Add.MenuRoleList();

        if (parent.disablePopup) {
            parent.disablePopup();
        } else { self.close(); }
    }
    else {
        alert(R_ProcessingError);
    }
}



/**************************************************
작성자   : famo23
작성일자 : 2014-01-17
작성목적 : 조직도 선택시
***************************************************/
function RoleNodeSelect_onNodeSelect(node) {

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

