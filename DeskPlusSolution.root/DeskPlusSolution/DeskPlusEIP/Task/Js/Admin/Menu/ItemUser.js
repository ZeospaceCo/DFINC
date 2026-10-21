ItemUser = {}
ItemUser.TreeFrame = parent.parent.frmLeft;

var AuthToArray = []; //db 에서 수신권한 값을 권한배열에 담는다.
var AuthCcArray = []; //db 에서 참조권한 값을 권한배열에 담는다.

var CompareToArray = []; //넘어온 수신권한값 배열에 할당하기(G$1$영업부,영업부)
var CompareCcArray = []; //넘어온 참조권한값 배열에 할당하기(G$1$영업부,영업부)

var DelArray = []; //삭제할 값 배열에 담기.

$(function () {
    $(document).ready(function () {

        var AuthToArrayListStr = AuthToArrayList.split(';');
        var AuthCcArrayListStr = AuthCcArrayList.split(';');

        //저장된 수신받은 사용자 리스트 Load
        for (i = 0; i < AuthToArrayListStr.length; i++) {
            if (AuthToArrayListStr[i].split("$")[1] != undefined) {
                var keyid = AuthToArrayListStr[i].split("$")[0] + "$" + AuthToArrayListStr[i].split("$")[1];
                var keyDeptid = AuthToArrayListStr[i].split("$")[2];
                var keyName = AuthToArrayListStr[i].split("$")[3];
                ItemUser.AddToAuthArray(keyid, keyName, keyDeptid);
            }
        }

        //저장된 참조받은 사용자 리스트 Load
        for (i = 0; i < AuthCcArrayListStr.length; i++) {
            if (AuthCcArrayListStr[i].split("$")[1] != undefined) {
                var keyid = AuthCcArrayListStr[i].split("$")[0] + "$" + AuthCcArrayListStr[i].split("$")[1];
                var keyDeptid = AuthCcArrayListStr[i].split("$")[2];
                var keyName = AuthCcArrayListStr[i].split("$")[3];
                ItemUser.AddCcAuthArray(keyid, keyName, keyDeptid);
            }
        }

        //검색입력필드 엔터 이벤트 기술
        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (event) {
            ItemUser.KeyEvents();
        });

        //저장
        $('#CbtnSave').click(function (e) {
            e.preventDefault();
            ItemUser.Save();
        });

        //수신자추가
        $('#CbtnToAdd').click(function (e) {
            e.preventDefault();
            ItemUser.AddTo();
        });

        //참조자추가
        $('#CbtnCcAdd').click(function (e) {
            e.preventDefault();
            ItemUser.AddCc();
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

    });
});

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 저장버튼 클릭 이벤트
**********************************************************************************************/
ItemUser.Save = function () {

    var selectedToValue = "";
    var selectedCcValue = "";

    for (var i = 0; i < AuthToArray.length; i++) {

        selectedToValue += AuthToArray[i].id + '$' + AuthToArray[i].deptID + ",";
        //alert(selectedToValue);
    }

    for (var i = 0; i < AuthCcArray.length; i++) {

        selectedCcValue += AuthCcArray[i].id + '$' + AuthCcArray[i].deptID + ",";
    }

    //alert(selectedToValue);
    //alert(selectedCcValue);

    $("#tbItemToUserValue").val(selectedToValue.trimEnds(","));
    $("#tbItemCcUserValue").val(selectedCcValue.trimEnds(","));

    __doPostBack("LBTNProcess", "Register");
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 수신대상자 버튼 클릭
**********************************************************************************************/

ItemUser.AddTo = function () {

    var selectedID = $("#tvSelectedID").val().trimEnds('|');
    var selectedValue = $("#tvSelectedValue").val().trimEnds('$');


    if (selectedID != "" && selectedValue != "") {

        CompareToArray = []; //넘어온 권한값

        var selStr = selectedValue.split('$');
        //alert(selStr);

        for (i = 0; i < selStr.length; i++) {
            var selGubun = selStr[i].split('|')[0]; //사용자 M / 부서 G
            var selOwnerID = selStr[i].split('|')[1]; //수신은 그룹이 될 수 없으니 UserID            
            
            var selName = selStr[i].split('|')[2]; //리스트박스에서 선택한 이름은 부서,직급 모두 붙어있으므로 잘라줘야 함    
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
                ItemUser.AddToCompareArray(selGubun + "$" + selOwnerID, selName, selTitle, selDeptName, selDeptID);
            }

        }

        //2. 넘어온 권한값(CompareToArray[]) 배열과 권한값(AuthArray[]) 비교하기
        $.each(CompareToArray, function (i, v) {
            ItemUser.CompareToArrayVal(v.id, v.name, v.title, v.deptName, v.deptID);
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
ItemUser.CompareToArrayVal = function (compID, compName, comTitle, comDeptName, comDeptID) {
 
    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (AuthToArray.length > 0) {
        for (var i = 0; i < AuthToArray.length; i++) {

            //alert("AuthArray[i].id : [" + i + "]" + v.id + " | compArr.id : " + compArr.id);

            if (AuthToArray[i].id == compID) {
                response = 1; //동일한 값이 존재할 경우
                break;
            }
            else {
                response = 0; //다른 값일 경우                
            }
        }

        if (response == 0) {
            //중복되지 않으면 권한값 할당하기
            ItemUser.AddToAuthRow(compID, compName, comTitle, comDeptName, comDeptID);
        }
    }
    else {
        response = 0; //다른 값일 경우
        ItemUser.AddToAuthRow(compID, compName, comTitle, comDeptName, comDeptID);
    }

    return response;
}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 오른쪽 사용자 넘어가는 부분
**********************************************************************************************/
ItemUser.AddToAuthRow = function (keyID, name, title, deptName, deptID) {

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
		"<tr id='itemUserTo" + ToRowMax + "'>"
			+ "<td>"
			+ deptName
            + title
            + name
            + "</td>"
			+ "<td class='center'><a class = 'btn' onclick=\"ItemUser.DelToAuthRow(" + ToRowMax + ",'" + keyID + "','" + name + "')\">" + R_Del + "</a></td>"
		+ "</tr>"
	);
    //alert(keyID);
    //alert(unescape(name));
    ItemUser.AddToAuthArray(keyID, name, deptID)
}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 수신대상자 배열 담기
**********************************************************************************************/
ItemUser.AddToAuthArray = function (keyID, keyName, keyDeptid) {
    //alert(AddToAuthArray);

    AuthToArray.push({
        'id': keyID,
        'name': escape(keyName),
        'deptID': keyDeptid
    });
   
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 수신대상자 중복 확인
**********************************************************************************************/
ItemUser.AddToCompareArray = function (keyID, keyName, keyTitle, keyDeptName, keyDeptID) {
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
작성목적 : 수신대상자 제거
**********************************************************************************************/
ItemUser.DelToAuthRow = function (index, keyID, keyName) {
    //alert(index + '+' + keyID);
    var row = $("#itemUserTo" + index);

    for (var i = 0; i < AuthToArray.length; i++) {

        if (AuthToArray[i].id == keyID) {

            AuthToArray.splice(i, 1);

            row.fadeOut("slow", function () {
                row.remove();
            });

            break;
        }
    }
}

/********************상단 수신 *****************************************************/




/********************하단 참조 *****************************************************/

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 참조대상자 추가 버튼 클릭
**********************************************************************************************/
ItemUser.AddCc = function () {

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

            var selName = selStr[i].split('|')[2]; //리스트박스에서 선택한 이름은 부서,직급 모두 붙어있으므로 잘라줘야 함
            if (selName.indexOf('/') > 1) {
                selName = selName.split('/')[0]
            }

            var selTitle = selStr[i].split('|')[5];
            var selDeptName = selStr[i].split('|')[11];

            //1. 넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
            ItemUser.AddCcCompareArray(selGubun + "$" + selOwnerID, selName, selTitle, selDeptName, selDeptID);

        }

        //2. 넘어온 권한값(CompareCcArray[]) 배열과 권한값(AuthArray[]) 비교하기
        $.each(CompareCcArray, function (i, v) {
            ItemUser.CompareCcArrayVal(v.id, v.name, v.title, v.deptName, v.deptID);
        });

    }
    else {
        alert(R_SelectAdd);
    }
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 참조대상자 중복 확인
**********************************************************************************************/

ItemUser.CompareCcArrayVal = function (compID, compName, comTitle, comDeptName, comDeptID) {

    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (AuthCcArray.length > 0) {
        for (var i = 0; i < AuthCcArray.length; i++) {

            //alert("AuthArray[i].id : [" + i + "]" + v.id + " | compArr.id : " + compArr.id);

            if (AuthCcArray[i].id == compID) {
                response = 1; //동일한 값이 존재할 경우
                break;
            }
            else {
                response = 0; //다른 값일 경우                
            }
        }

        if (response == 0) {
            //중복되지 않으면 권한값 할당하기
            ItemUser.AddCcAuthRow(compID, compName, comTitle, comDeptName, comDeptID);
        }
    }
    else {
        response = 0; //다른 값일 경우
        ItemUser.AddCcAuthRow(compID, compName, comTitle, comDeptName, comDeptID);
    }

    return response;
}



/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 오른쪽 사용자 넘어가는 부분
**********************************************************************************************/
ItemUser.AddCcAuthRow = function (keyID, name, title, deptName, deptID) {
   
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
		"<tr id='itemUserCc" + CcRowMax + "'>"
			+ "<td>"
			+ deptName
            + title
            + name
            + "</td>"
			+ "<td class='center'><a class='btn' onclick=\"ItemUser.DelCcAuthRow(" + CcRowMax + ",'" + keyID + "','" + name + "')\">" + R_Del + "</a></td>"
		+ "</tr>"
	);

    ItemUser.AddCcAuthArray(keyID, name, deptID)
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 참조대상자 배열 담기
**********************************************************************************************/
ItemUser.AddCcAuthArray = function (keyID, keyName, deptID) {

    AuthCcArray.push({
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
ItemUser.AddCcCompareArray = function (keyID, keyName, keyTitle, keyDeptName, keyDeptID) {
    CompareCcArray.push({
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
작성목적 : 참조대상자 제거
**********************************************************************************************/
ItemUser.DelCcAuthRow = function (index, keyID, keyName) {
    //alert(index +'+'+ keyID);
    var row = $("#itemUserCc" + index);

    for (var i = 0; i < AuthCcArray.length; i++) {

        if (AuthCcArray[i].id == keyID) {

            AuthCcArray.splice(i, 1);

            row.fadeOut("slow", function () {
                row.remove();
            });

            break;
        }
    }
}


/**************************************************
작성자   : maniaysw
작성일자 : 2015-04-16
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
ItemUser.KeyEvents = function () {    
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