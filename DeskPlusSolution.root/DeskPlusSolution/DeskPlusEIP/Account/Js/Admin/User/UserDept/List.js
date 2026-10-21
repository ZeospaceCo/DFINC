/// <reference path="../../../../Html/Js/jQuery/jquery.js" />


List = {}


var AuthArray = []; //db 에서 부서할당 값을 부서할당배열에 담는다.
var CompareArray = []; //넘어온 부서할당값 배열에 할당하기(G$1$영업부,영업부)
var DelArray = []; //삭제할 값 배열에 담기.

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //검색입력필드 엔터 이벤트 기술
        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (event) {
            List.KeyEvents();
        });

        //db 에서 부서할당 값을 부서할당배열에 담는다.        
        var AuthArrayListStr = AuthArrayList.split(';');
        for (i = 0; i < AuthArrayListStr.length; i++) {

            if (AuthArrayListStr[i].split("$")[1] != undefined) {

                var keyid = AuthArrayListStr[i].split("$")[0]; //deptid
                var keyName = AuthArrayListStr[i].split("$")[1];//deptname
                List.AddAuthArray(keyid, keyName);
            }
        }

        //저장
        $('#cbtnSubmit').click(function (e) {
            e.preventDefault();
            List.Process();
        });

        //부서 추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.AddAuth();
        });

        //기본정보
        $('#cbtnUser').click(function (e) {
            e.preventDefault();
            List.GoList("user");
        });

        //확장정보
        $('#cbtnExtend').click(function (e) {
            e.preventDefault();
            List.GoList("extend");
        });

        //부서정보
        $('#cbtnDept').click(function (e) {
            e.preventDefault();
            List.GoList("dept");
        });

        //리스트 정보
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            List.GoList("list");
        });
    });
});

/**************************************************
작성자   : shimpark
작성목적 : 이동하기
***************************************************/
List.GoList = function (gubun) {

    var goList = "";

    switch (gubun) {
        case "user":
            goList = "/DeskPlusEIP/Account/Admin/User/Mod.aspx";
            break;
        case "extend":
            goList = "/DeskPlusEIP/Account/Admin/User/Extend/FamilyInfo.aspx";
            break;
        case "dept":
            goList = "/DeskPlusEIP/Account/Admin/User/UserDept/List.aspx";
            break;
        case "list":
            goList = "/DeskPlusEIP/Account/Admin/User/" + ListPage;
            break;
    }

    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchGubun': SearchGubun,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'listPage': ListPage
    };

    CallEncrypt(CallGetParam());
    var url = goList + "?p=" + GetEncryptValue;
    location = url;
}


/**************************************************
작성자   : shimpark
작성목적 : 부서할당추가 버튼
***************************************************/
List.AddAuth = function () {

    //deptid
    var selectedID = $("#tvSelectedID").val().trimEnds('|');
    //deptID | deptName | parentDeptID | deptOrder
    var selectedValue = $("#tvSelectedValue").val().trimEnds('$')

    if (selectedID != "" && selectedValue != "") {

        CompareArray = []; //넘어온 부서할당값

        var selStr = selectedValue.split('$');

        //deptID | deptName | parentDeptID | deptOrder
        for (i = 0; i < selStr.length; i++) {
            var selDeptID = selStr[i].split('|')[0];
            var selName = selStr[i].split('|')[1];

            //1. 넘어온 부서할당값 배열에 할당하기(G$1$영업부,영업부)
            List.AddCompareArray(selDeptID, selName);
        }

        //2. 넘어온 부서할당값(CompareArray[]) 배열과 부서할당값(AuthArray[]) 비교하기
        $.each(CompareArray, function (i, v) {
            List.CompareArrayVal(v.id, v.name);
        });

    }
    else {
        alert(R_SettingAuthority);
    }
}


/**************************************************
작성자   : shimpark
작성목적 : 넘어온 부서할당값과 부서할당 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
List.CompareArrayVal = function (compID, compName) {
    
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
            //중복되지 않으면 부서할당값 할당하기
            List.AddAuthRow(compID, compName);
        }
    }
    else {
        response = 0; //다른 값일 경우
        List.AddAuthRow(compID, compName);
    }

    return response;
}


/**************************************************
작성자   : shimpark
작성목적 : 추가 table row 생성하고 부서할당값 AuthArray[] 배열에 할당하기
***************************************************/
List.AddAuthRow = function (keyID, name) {
    var rows = $("#contentTable tr");
    RowMax = RowMax + 1;

    var selStr = keyID.split('$');    

    $("#contentTable").append(
		"<tr id='boardAuth" + RowMax + "'>"
			+ "<td class='center'>"
			+ name
			+ "</td>"
            + "<td class='center'>"
			+ "<select name='ddlTitle' id='ddlTitle" + RowMax + "' class='searchselect form-control'>"
			+ TitleTable
			+ "</select>"
            + "</td>"
            + "<td class='center'>"
			+ "<select name='ddlDuty' id='ddlDuty" + RowMax + "' class='searchselect form-control'>"
			+ DutyTable
			+ "</select>"
            + "</td>"
            + "<td class='center'>"
            + "<select name='ddlBaseDept' id='ddlBaseDept" + RowMax + "' class='searchselect form-control'>"
			+ "<option value='0'>" + R_Both + "</option><option value='1' selected='selected'>" + R_Basic + "</option>"
			+ "</select>"
            + "</td>"
            + "<td class='center'><a class='btn' onclick=\"List.DelRow(" + RowMax + ",'" + keyID + "','" + name + "')\">" + R_Admin_CompanyManager_btnDel + "</a></td>"
			//+ "<td class='center'><a onclick=\"List.DelRow(" + RowMax + ",'" + keyID + "','" + name + "')\"><img src='" + ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif' /></a></td>"
		+ "</tr>"
	);

    List.AddAuthArray(keyID, name)
}


/**************************************************
작성자   : shimpark
작성목적 : 부서할당값 table 특정 row 삭제하기
***************************************************/
List.DelRow = function (index, keyID, keyName) {

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
작성자   : shimpark
작성목적 : 부서할당 배열값 저장소; 비교 대상자 배열값
***************************************************/
List.AddAuthArray = function (keyID, keyName) {    
    AuthArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : shimpark
작성목적 : 조직도 넘어온 값 저장소, 차후 비교값
***************************************************/
List.AddCompareArray = function(keyID, keyName){
    CompareArray.push({
        'id': keyID,
        'name': keyName
    });
}



/**************************************************
작성자   : 심재운
작성일자 : 2011-01-26
작성목적 : 처리하기
***************************************************/
List.Process = function () {

    var authID = ""; //deptid
    var authVal = ""; //선택한 직급,직책 가져오기

    if (AuthArray.length > 0) {
        for (var i = 0; i < AuthArray.length; i++) {
            authID += AuthArray[i].id + ",";
        }
    }

    var j = 0;
    var i = 1;
    $("#contentTable select option:selected").each(function () {
        authVal += $(this).val();

        if (i % 3 == 0) {
            authVal += "$";
            if ($(this).val() == "1") {
                j += 1; //겸직개수
            }
        }
        else {
            authVal += ",";
        }
        i += 1;
    });

    //겸직 설정하지 않았을 경우
    if (j == 0) {
        alert(R_SettingBothDept);
        return;
    }
    //겸직 기본을 2번 이상 넣었을 경우
    if (j > 1) {
        alert(R_SettingBaseDeptOnlyone);
        return;
    }

    List.Add(authID.trimEnds(","), authVal.trimEnds("$"));


}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 사용자의 부서 저장하기
***************************************************/
List.Add = function (deptList, titleDutyList) {
    
    var url = "/DeskPlusEIP/Account/Ws/Admin/User/UserDept/List.asmx/AddUserDept";

    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID,
        'deptList': deptList,
        'titleDutyList': titleDutyList
    };

    var obj = eval("List.SuccessAdd");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시  로그인 체크
**********************************************************************************************/
List.SuccessAdd = function (data, status) {
    if (data.d == true) {
        alert(R_Admin_UserManager_MSG_SUCCESS);

        ParamArray = {
            'userID': UserID,
            'deptID': DeptID,
            'currentPage': CurrentPage,
            'recordPerPage': RecordPerPage,
            'searchField': SearchField,
            'searchValue': SearchValue,
            'searchTerm': SearchTerm,
            'searchTermValue': SearchTermValue,
            'order': Order,
            'listPage': ListPage
        };

        CallEncrypt(CallGetParam());
        location.href = "../Mod.aspx?p=" + GetEncryptValue;
    }
    else {
        alert(R_ProcessingError);
    }
}






/**************************************************
작성자   : 심재운
작성목적 : 조직도 선택시
***************************************************/
function DeptNodeSelect_onNodeSelect(node) {   

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

    $("#tvSelectedID").val(selIDs); //deptid
    
    //deptID | deptName | parentDeptID | deptOrder
    $("#tvSelectedValue").val(selValues);
    
}