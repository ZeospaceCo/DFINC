ItemSubUser = {}

var SubToArray = []; //db 에서 수신권한 값을 권한배열에 담는다.
var SubCcArray = []; //db 에서 참조권한 값을 권한배열에 담는다.

var CompareToArray = []; //넘어온 수신권한값 배열에 할당하기(G$1$영업부,영업부)
var CompareCcArray = []; //넘어온 참조권한값 배열에 할당하기(G$1$영업부,영업부)

var DelArray = []; //삭제할 값 배열에 담기.


$(function () {
    $(document).ready(function () {
        var SubToArrayStr = SubToArrayList.split(';');
        var SubCcArrayStr = SubCcArrayList.split(';');


        //저장된 수신받은 사용자 리스트 Load
        for (i = 0; i < SubToArrayStr.length; i++) {
            if (SubToArrayStr[i].split("$")[1] != undefined) {
                var keyid = SubToArrayStr[i].split("$")[0] + "$" + SubToArrayStr[i].split("$")[1];
                var keyDeptid = SubToArrayStr[i].split("$")[2];
                var keyName = SubToArrayStr[i].split("$")[3];
                ItemSubUser.AddToAuthArray(keyid, keyName, keyDeptid);
            }
        }

        //저장된 참조받은 사용자 리스트 Load
        for (i = 0; i < SubCcArrayStr.length; i++) {
            if (SubCcArrayStr[i].split("$")[1] != undefined) {
                var keyid = SubCcArrayStr[i].split("$")[0] + "$" + SubCcArrayStr[i].split("$")[1];
                var keyDeptid = SubCcArrayStr[i].split("$")[2];
                var keyName = SubCcArrayStr[i].split("$")[3];
                ItemSubUser.AddCcAuthArray(keyid, keyName, keyDeptid);
            }
        }


        //수신자추가
        $('#CbtnToAdd').click(function (e) {
            e.preventDefault();
            ItemSubUser.AddTo();
        });

        //참조자추가
        $('#CbtnCcAdd').click(function (e) {
            e.preventDefault();
            ItemSubUser.AddCc();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (event) {
            ItemSubUser.KeyEvents();
        });


        //저장
        $('#CbtnSave').click(function (e) {
            e.preventDefault();
            ItemSubUser.Save();
        });

        //취소
        $('#CbtnCancel').click(function (e) {
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
            $("#tvSelectedID").val("")
            $("#tvSelectedValue").val("");    
            e.preventDefault();
            SearchDeptUserList(2);
        });

        //alert(SubToArrayStr);
        //alert(SubCcArrayStr);
    });
});



/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 저장버튼 클릭 이벤트
**********************************************************************************************/
ItemSubUser.Save = function () {

    var selectedToValue = "";
    var selectedCcValue = "";

    for (var i = 0; i < SubToArray.length; i++) {

        selectedToValue += SubToArray[i].id + '$' + SubToArray[i].deptID + ",";
        //alert(selectedToValue);
    }

    for (var i = 0; i < SubCcArray.length; i++) {

        selectedCcValue += SubCcArray[i].id + '$' + SubCcArray[i].deptID + ",";
    }

    //alert(selectedToValue);
    //alert(selectedCcValue);

    $("#tbItemSubToUserValue").val(selectedToValue.trimEnds(","));
    $("#tbItemSubCcUserValue").val(selectedCcValue.trimEnds(","));

    __doPostBack("LBTNProcess", "Register");
}


//소분류 수신자 추가
ItemSubUser.AddTo = function () {
    var selectedID = $("#tvSelectedID").val().trimEnds('|');
    var selectedValue = $("#tvSelectedValue").val().trimEnds('$');


    if (selectedID != "" && selectedValue != "") {

        CompareToArray = []; //넘어온 권한값

        var selStr = selectedValue.split('$');
        //alert(selStr);

        for (i = 0; i < selStr.length; i++) {
            var selGubun = selStr[i].split('|')[0]; //사용자 M / 부서 G
            var selOwnerID = selStr[i].split('|')[1]; //수신은 그룹이 될 수 없으니 UserID            
            var selName = selStr[i].split('|')[2];
            if (selName.indexOf('/') > 1) {
                selName = selName.split('/')[0]
            }
            var selDeptID = selStr[i].split('|')[3]; //부서ID도 가져옴
            var selTitle = selStr[i].split('|')[5];
            var selDeptName = selStr[i].split('|')[11];
            if (selGubun == 'G') {
                alert(R_ToDept);
            }
            else {
                //1. 넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
                ItemSubUser.AddToCompareArray(selGubun + "$" + selOwnerID, selName, selTitle, selDeptName, selDeptID);
            }

        }

        //2. 넘어온 권한값(CompareToArray[]) 배열과 권한값(AuthArray[]) 비교하기
        $.each(CompareToArray, function (i, v) {
            ItemSubUser.CompareToArrayVal(v.id, v.name, v.title, v.deptName, v.deptID);
        });

    }
    else {
        alert(R_SelectAdd);
    }
}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 수신대상자 중복 확인
**********************************************************************************************/
ItemSubUser.AddToCompareArray = function (keyID, keyName, keyTitle, keyDeptName, keyDeptID) {
    CompareToArray.push({
        'id': keyID,
        'name': escape(keyName),
        'title': escape(keyTitle),
        'deptName': escape(keyDeptName),
        'deptID': keyDeptID
    });
}



/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 수신대상자 중복 확인
**********************************************************************************************/
ItemSubUser.CompareToArrayVal = function (compID, compName, comTitle, comDeptName, comDeptID) {

    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (SubToArray.length > 0) {
        for (var i = 0; i < SubToArray.length; i++) {

            //alert("AuthArray[i].id : [" + i + "]" + v.id + " | compArr.id : " + compArr.id);

            if (SubToArray[i].id == compID) {
                response = 1; //동일한 값이 존재할 경우
                break;
            }
            else {
                response = 0; //다른 값일 경우                
            }
        }

        if (response == 0) {
            //중복되지 않으면 권한값 할당하기
            ItemSubUser.AddToAuthRow(compID, compName, comTitle, comDeptName, comDeptID);
        }
    }
    else {
        response = 0; //다른 값일 경우
        ItemSubUser.AddToAuthRow(compID, compName, comTitle, comDeptName, comDeptID);
    }

    return response;
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 오른쪽 사용자 넘어가는 부분
**********************************************************************************************/
ItemSubUser.AddToAuthRow = function (keyID, name, title, deptName, deptID) {

    if (title.length > 0) {
        deptName = unescape(deptName) + ' / ';
        title = unescape(title) + ' / ';
        name = unescape(name);
    }
    else {
        deptName = unescape(deptName);
        title = '';
        name = ''; ;
    }


    var rows = $("#contentToTable tr");
    ToRowMax = ToRowMax + 1;

    var selStr = keyID.split('$');

    $("#contentToTable").append(
		"<tr id='itemSubUserTo" + ToRowMax + "'>"
			+ "<td>"
			+ deptName
            + title
            + name
            + "</td>"
			+ "<td class='center'><a class = 'btn' onclick=\"ItemSubUser.DelToAuthRow(" + ToRowMax + ",'" + keyID + "','" + name + "')\">" + R_Del + "</a></td>"
		+ "</tr>"
	);
    //alert(keyID);
    //alert(unescape(name));
    ItemSubUser.AddToAuthArray(keyID, name, deptID)
}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 수신대상자 배열 담기
**********************************************************************************************/
ItemSubUser.AddToAuthArray = function (keyID, keyName, keyDeptid) {
    //alert(AddToAuthArray);

    SubToArray.push({
        'id': keyID,
        'name': escape(keyName),
        'deptID': keyDeptid
    });

}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-29
작성목적 : 참조대상자 배열 담기
**********************************************************************************************/
ItemSubUser.AddCcAuthArray = function (keyID, keyName, deptID) {

    SubCcArray.push({
        'id': keyID,
        'name': escape(keyName),
        'deptID': deptID
    });
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 참조대상자 중복 확인
**********************************************************************************************/
ItemSubUser.AddCcCompareArray = function (keyID, keyName, keyTitle, keyDeptName, keyDeptID) {
    CompareCcArray.push({
        'id': keyID,
        'name': escape(keyName),
        'title': escape(keyTitle),
        'deptName': escape(keyDeptName),
        'deptID': keyDeptID
    });
}

//사용자 참조 추가 버튼 클릭시
ItemSubUser.AddCc = function () {

    var selectedID = $("#tvSelectedID").val().trimEnds('|');
    var selectedValue = $("#tvSelectedValue").val().trimEnds('$');


    if (selectedID != "" && selectedValue != "") {

        CompareCcArray = []; //넘어온 권한값

        var selStr = selectedValue.split('$');
        //alert(selStr);
        for (i = 0; i < selStr.length; i++) {
            var selGubun = selStr[i].split('|')[0]; //참조는 부서도 들어갈 수 있음            
            if (selGubun == 'M') {
                var selOwnerID = selStr[i].split('|')[1]; //gubun값이 M 이면 UserID / G 이면 부서ID가 됨
                var selDeptID = selStr[i].split('|')[3]; //부서ID도 가져옴                               
            }
            else {
                var selOwnerID = selStr[i].split('|')[1];
                var selDeptID = selStr[i].split('|')[1]; //Group이 G면 해당ID값을 부서ID 에 할당
            }
            var selName = selStr[i].split('|')[2];
            if (selName.indexOf('/') > 1) {
                selName = selName.split('/')[0]
            }
            var selTitle = selStr[i].split('|')[5];
            var selDeptName = selStr[i].split('|')[11];

            //1. 넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
            ItemSubUser.AddCcCompareArray(selGubun + "$" + selOwnerID, selName, selTitle, selDeptName, selDeptID);

        }

        //2. 넘어온 권한값(CompareCcArray[]) 배열과 권한값(AuthArray[]) 비교하기
        $.each(CompareCcArray, function (i, v) {
            ItemSubUser.CompareCcArrayVal(v.id, v.name, v.title, v.deptName, v.deptID);
        });

    }
    else {
        alert(R_SelectAdd);
    }

}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 수신대상자 제거
**********************************************************************************************/
ItemSubUser.DelToAuthRow = function (index, keyID, keyName) {
    //alert(index + '+' + keyID);
    var row = $("#itemSubUserTo" + index);

    for (var i = 0; i < SubToArray.length; i++) {

        if (SubToArray[i].id == keyID) {

            SubToArray.splice(i, 1);

            row.fadeOut("slow", function () {
                row.remove();
            });

            break;
        }
    }
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 참조대상자 제거
**********************************************************************************************/
ItemSubUser.DelCcAuthRow = function (index, keyID, keyName) {
    //alert(index +'+'+ keyID);
    var row = $("#itemSubUserCc" + index);

    for (var i = 0; i < SubCcArray.length; i++) {

        if (SubCcArray[i].id == keyID) {

            SubCcArray.splice(i, 1);

            row.fadeOut("slow", function () {
                row.remove();
            });

            break;
        }
    }
}



/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 참조대상자 중복 확인
**********************************************************************************************/

ItemSubUser.CompareCcArrayVal = function (compID, compName, comTitle, comDeptName, comDeptID) {

    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (SubCcArray.length > 0) {
        for (var i = 0; i < SubCcArray.length; i++) {

            //alert("AuthArray[i].id : [" + i + "]" + v.id + " | compArr.id : " + compArr.id);

            if (SubCcArray[i].id == compID) {
                response = 1; //동일한 값이 존재할 경우
                break;
            }
            else {
                response = 0; //다른 값일 경우                
            }
        }

        if (response == 0) {
            //중복되지 않으면 권한값 할당하기
            ItemSubUser.AddCcAuthRow(compID, compName, comTitle, comDeptName, comDeptID);
        }
    }
    else {
        response = 0; //다른 값일 경우
        ItemSubUser.AddCcAuthRow(compID, compName, comTitle, comDeptName, comDeptID);
    }

    return response;
}




/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 오른쪽 사용자 넘어가는 부분
**********************************************************************************************/
ItemSubUser.AddCcAuthRow = function (keyID, name, title, deptName, deptID) {

    if (title.length > 0) {
        deptName = unescape(deptName) + ' / ';
        title = unescape(title) + ' / ';
        name = unescape(name);
    }
    else {
        deptName = unescape(deptName);
        title = '';
        name = ''; ;
    }


    var rows = $("#contentCcTable tr");
    CcRowMax = CcRowMax + 1;
    //alert(CcRowMax + '+' + keyID);
    var selStr = keyID.split('$');

    $("#contentCcTable").append(
		"<tr id='itemSubUserCc" + CcRowMax + "'>"
			+ "<td>"
			+ deptName
            + title
            + name
            + "</td>"
			+ "<td class='center'><a class = 'btn' onclick=\"ItemSubUser.DelCcAuthRow(" + CcRowMax + ",'" + keyID + "','" + name + "')\">" + R_Del + "</a></td>"
		+ "</tr>"
	);

    ItemSubUser.AddCcAuthArray(keyID, name, deptID)
}




/**************************************************
작성자   : maniaysw
작성일자 : 2015-04-28
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

//엔터키 이벤트
ItemSubUser.KeyEvents = function () {
    if (event.keyCode == 13) {
        SearchDeptUserList(1);
    }
}

/**************************************************
작성자   : maniaysw
작성일자 : 2014-04-21
작성목적 : 트리 검색버튼 클릭 이벤트
***************************************************/
function SearchDeptUserList(param) {

    var searchName = $("#DeptUserTreeScript1_tbDeptUser").val().securityText();
    $("#DeptUserTreeScript1_tbDeptUser").val(searchName);

    var params = searchName + "" + param + "" + CompanyID;
    DeptUserTreeScript1_CallBack1.Callback(params);
}


/**************************************************
작성자   : maniaysw
작성일자 : 2014-04-22
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

        selIDs = selIDs.replace(selID + "|", "");
        selIDs += selID + "|";
        selValues = selValues.replace(selValue + "$", "");
        selValues += selValue + "$";

    });

    // type(G) | deptID | deptName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName
    // type(M) | userID | userName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName

    $("#tvSelectedID").val(selIDs);
    $("#tvSelectedValue").val(selValues);

    //alert(selValues);
}
