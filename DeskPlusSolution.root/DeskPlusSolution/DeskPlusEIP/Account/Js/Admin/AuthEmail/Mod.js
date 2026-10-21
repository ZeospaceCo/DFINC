
Mod = {}

var AuthArray = []; //db 에서 부서할당 값을 부서할당배열에 담는다.
var CompareArray = []; //넘어온 부서할당값 배열에 할당하기(G$1$영업부,영업부)
var DelArray = []; //삭제할 값 배열에 담기.

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        var AuthArrayListStr = AuthArrayList.split(';');

        for (i = 0; i < AuthArrayListStr.length; i++) {
            if (AuthArrayListStr[i].split("$")[1] != undefined) {
                var keyid = AuthArrayListStr[i].split("$")[0] + "$" + AuthArrayListStr[i].split("$")[1];
                var keyName = AuthArrayListStr[i].split("$")[2];
                Mod.AddAuthArray(keyid, keyName);
            }
        }

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

        //저장
        $('#cbtnSubmit').click(function (e) {
            e.preventDefault();
            Mod.Process();
        });

        //추가
        $('#CbtnAdd').click(function (e) {            
            e.preventDefault();
            Mod.AddAuth();

        });

        //검색입력필드 엔터 이벤트 기술
        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                SearchDeptUserList(1);
            }
        });

    })
});

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
작성목적 : 조직도 선택시
***************************************************/
function DeptUserTreeScript_onNodeSelect(node) {

    var selIDs = $("#tvSelectedID").val();
    var selValues = $("#tvSelectedValue").val();
    var selID = node.ID;
    var selValue = node.Value;
    var selCompanyID = node.GetProperty("CompanyID"); //회사아이디
    //alert(selValue);


    if (selCompanyID != CompanyID) {
        alert(R_NoAddOtherCompany);
        return;
    }

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
작성자   : shimpark
작성목적 : 추가 table row 생성하고 권한값 AuthArray[] 배열에 할당하기
***************************************************/
Mod.AddAuthRow = function (keyID, name, title, email, dept) {
    var rows = $("#contentTable tr");
    RowMax = RowMax + 1;

    var selStr = keyID.split('$');

    $("#contentTable").append(
		"<tr id='UserAuth" + RowMax + "'>"
			+ "<td class='center'>"
			+ unescape(name) + " / " + unescape(dept) + " / " + unescape(title) + " / " + unescape(email)
			+ "</td>"
            + "<td class='center'><a class='btn' onclick=\"Mod.DelAuthRow(" + RowMax + ",'" + keyID + "','" + name + "')\">" + R_Admin_CompanyManager_btnDel + "</a></td>"
			//+ "<td class='center'><a onclick=\"Mod.DelAuthRow(" + RowMax + ",'" + keyID + "','" + name + "')\"><img src='" + ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif' /></a></td>"
		+ "</tr>"
	);

    Mod.AddAuthArray(keyID, name)
}


/**************************************************
작성자   : shimpark
작성목적 : 권한추가 버튼
***************************************************/
Mod.AddAuth = function () {
    //deptid
    var selectedID = $("#tvSelectedID").val().trimEnds('|');
    //deptID | deptName | parentDeptID | deptOrder
    var selectedValue = $("#tvSelectedValue").val().trimEnds('$');


    if (selectedID != "" && selectedValue != "") {
        CompareArray = []; //넘어온 권한값

        var selStr = selectedValue.split('$');

        for (i = 0; i < selStr.length; i++) {

            var selGubun = selStr[i].split('|')[0];
            var selOwnerID = selStr[i].split('|')[1];
            var selName = selStr[i].split('|')[2];
            if (selGubun == "G") { //부서지정불가
                alert(R_ManagerNotDept);
                return;
            }
            if (selName.indexOf('/') > 2) { //검색 리스트에서 선택하면 값이 이상하게 나옴
                selName = selName.split('/')[0];
            }
            var selTitle = selStr[i].split('|')[5];
            var selEmail = selStr[i].split('|')[7];
            var selDept = selStr[i].split('|')[11];

            //1. 넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
            Mod.AddCompareArray(selGubun + "$" + selOwnerID, selName, selTitle, selEmail, selDept);
        }

        //2. 넘어온 권한값(CompareArray[]) 배열과 권한값(AuthArray[]) 비교하기


        if (CompareArray.length > 1) {
            alert(R_ManagerOnlyOne);
            return;
        }
        if (AuthArray.length >= 1) {
            alert(R_ManagerOnlyOne);
            return;
        }
        $.each(CompareArray, function (i, v) {            
            Mod.CompareArrayVal(v.id, v.name, v.title, v.email, v.dept);
        });
    }
    else {
        alert(R_NotCheckUser);
    }
}


/**************************************************
작성자   : shimpark
작성목적 : 조직도 넘어온 값 저장소, 차후 비교값
***************************************************/
Mod.AddCompareArray = function (keyID, keyName, selTitle, selEmail, selDept) {
    
    CompareArray.push({
        'id': keyID,
        'name': escape(keyName),
        'title': escape(selTitle),
        'email': escape(selEmail),
        'dept': escape(selDept)
    });
}


/**************************************************
작성자   : shimpark
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
Mod.CompareArrayVal = function (compID, compName, comTitle, comEmail, comDept) {
    //alert(comTitle);
    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (AuthArray.length >= 1) {
        alert(R_ManagerOnlyOne);
        return;
    }

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
            Mod.AddAuthRow(compID, compName, comTitle, comEmail, comDept);
        }
    }
    else {
        response = 0; //다른 값일 경우
        Mod.AddAuthRow(compID, compName, comTitle, comEmail, comDept);
    }

    return response;
}


/**************************************************
작성자   : shimpark
작성목적 : 권한 배열값 저장소; 비교 대상자 배열값
***************************************************/
Mod.AddAuthArray = function (keyID, keyName) {
    
    AuthArray.push({
        'id': keyID,
        'name': escape(keyName)
    });
}

/**************************************************
작성자   : shimpark
작성목적 : 권한값 table 특정 row 삭제하기
***************************************************/
Mod.DelAuthRow = function (index, keyID, keyName) {
    
    var row = $("#UserAuth" + index);

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
작성일자 : 2011-01-11
작성목적 : listbox 선택한 값 가져오기
***************************************************/
function LbxDeptUser_OnChange() {
    $("#tvSelectedID").empty();
    $("#tvSelectedValue").empty();

    var selIDs = "";
    var selValues = "";
    var isOtherCompany = false;

    $("#" + LBDeptUser + " :selected").each(function (i, selected) {
        var rtnValue = $(selected).val();
        var rtnText = $(selected).text();

        var colSplit = rtnValue.split("^"); //nodeID
        //var rowSplit = colSplit[1].split("$"); //nodeValue
        var selID = colSplit[0]; //nodeID

        var selCompanyID = rtnValue.split("|")[14]; //회사아이디

        if (selCompanyID == CompanyID) {

            selIDs = selIDs.replace(selID + "|", "");
            selIDs += selID + "|";

            var selValue = colSplit[1]; //nodeValue
            selValues = selValues.replace(selValue + "$", "");
            selValues += selValue + "$";
        } else {
            //타사 사용자 차단하기
            isOtherCompany = true;
        }
    });
    
    // type(G) | deptID | deptName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName
    // type(M) | userID | userName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName

    $("#tvSelectedID").val(selIDs);
    $("#tvSelectedValue").val(selValues);

    //타사 사용자 차단하기
    if (isOtherCompany == true) {
        alert(R_NoAddOtherCompany);
    }
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-01-26
작성목적 : 처리하기
***************************************************/
Mod.Process = function () {
    
    //권한값 추출해 오기
    var authVal = "";

    for (var i = 0; i < AuthArray.length; i++) {

        authVal = AuthArray[i].id;
        authVal = authVal.split('$')[1];    
    }
           
    
    $("#tbAuthValue").val(authVal.trimEnds(","));

    if (NowManagerID == "" && authVal != "") {
        __doPostBack("LBTNProcess", "Register");
    }
    else if(NowManagerID != "" && authVal != "") {
        __doPostBack("LBTNProcess", "Modify");
    }
    else {
        __doPostBack("LBTNProcess", "Delete");
    }
}