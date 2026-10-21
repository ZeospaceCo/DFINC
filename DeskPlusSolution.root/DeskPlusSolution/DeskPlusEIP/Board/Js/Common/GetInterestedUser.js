GetInterestedUser = {}
var InterestedUserArray = [];
var CompareArray = [];
var DelArray = [];

GetInterestedUser.TreeFrame = parent.parent.frmLeft;

$(function () {
    $(document).ready(function () {
       
        //검색입력필드 엔터 이벤트 기술
        $("#UCUserDeptTreeScript1_tbDeptUser").keydown(function (event) {
            GetInterestedUser.KeyEvents();
        });

        //확인
        $('#CbtnSubmit').click(function (e) {
            e.preventDefault();

            GetInterestedUser.Process();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();

            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //추가
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();

            GetInterestedUser.AddInterestedUser();
        });

        //취소
        $('#CbtnTreeView').click(function (e) {
            e.preventDefault();
            GetInterestedUser.SearchDeptUserList(2);
        });

        //검색
        $('#CbtnDeptUserSearch').click(function (e) {
            e.preventDefault();

            GetInterestedUser.SearchDeptUserList(1);
        });

        InterestedUserArray = [];
        var InterestedUserArrayListStr = InterestedUserArrayList.split(';');
        for (i = 0; i < InterestedUserArrayListStr.length; i++) {
            InterestedUserArray.push({
                'id': InterestedUserArrayListStr[i].split("$")[0],
                'name': InterestedUserArrayListStr[i].split("$")[1]
            });
        }

    });
});

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-23
작성목적 : 관심대상자추가 버튼
***************************************************/
GetInterestedUser.AddInterestedUser = function () {
    //deptID | deptName | parentDeptID | deptOrder
    var selectedValue = $("#tvSelectedValue").val().trimEnds('$');

    if (selectedValue == "") {
        return;
    }

    CompareArray = [];

    if (selectedValue != "") {
        var selStr = selectedValue.split('$');

        for (i = 0; i < selStr.length; i++) {
            var selOwnerID = selStr[i].split('|')[1];
            var selName = '';
            var arselName = selStr[i].split('|')[2].split('/');
            selName = selStr[i].split('|')[5] + "/" + selStr[i].split('|')[11] + "/" + arselName[0];

            //1. 넘어온 권한값 배열에 할당하기(영업부,영업부)
            GetInterestedUser.AddCompareArray(selOwnerID, selName);
            var response = false;

            //2. 넘어온 권한값 배열과 권한값 비교하기
            $.each(CompareArray, function (i, v) {
                response = GetInterestedUser.CompareArrayVal(InterestedUserArray, v);
            });

            if (selOwnerID == UserID) {
                response = 1;
            }

            //3. 중복값은 1을 의미함.
            if (response != 1) {
                GetInterestedUser.AddInterestedUserRow(selOwnerID, selName);
            }
        }
    }
}

function replaceAll(str, searchStr, replaceStr) {
    while (str.indexOf(searchStr) != -1) {
        str = str.replace(searchStr, replaceStr);
    }
    return str;
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-23
작성목적 : 조직도 넘어온 값 저장소, 차후 비교값
***************************************************/
GetInterestedUser.AddCompareArray = function (keyID, keyName) {
    CompareArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-23
작성목적 : 관심대상자 table 생성하기
***************************************************/
GetInterestedUser.AddInterestedUserRow = function (params, name) {
    var rows = $("#contentTable tr");
    var index = rows.length;

    var selStr = params.split('$');

    $("#contentTable").append(
		"<tr id='boardInterestedUser" + index + "'>"
			+ "<td class='center'>"
			+ name
			+ "</td>"
            + "<td class='center'><a class=btn onclick=GetInterestedUser.DelInterestedUserRow(" + index + ",'" + params + "','" + name + "') >" + R_Del + "</a></td>"
			//+ "<td class='center'><a onclick=\"GetInterestedUser.DelInterestedUserRow(" + index + ",'" + params + "','" + name + "')\"><img src='" + ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif' style='cursor:hand' /></a></td>"
		+ "</tr>"
	);

    GetInterestedUser.AddInterestedUserArray(params, name)
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-23
작성목적 : 관심대상자 table 특정 row 삭제하기
***************************************************/
GetInterestedUser.DelInterestedUserRow = function (index, keyID, keyName) {
    var row = $("#boardInterestedUser" + index);
    row.fadeOut("slow", function () {
        row.remove();
    });

    DelArray = [];

    DelArray.push({
        'id': keyID,
        'name': keyName
    });

    $.each(DelArray, function (i, v) {
        GetInterestedUser.removeByValue(InterestedUserArray, v);
    });
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-23
작성목적 : 권한 배열값 저장소; 비교 대상자 배열값
***************************************************/
GetInterestedUser.AddInterestedUserArray = function (keyID, keyName) {
    InterestedUserArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-23
작성목적 : 권한 배열값에서 특정값 삭제 : table row 삭제시 사용됨
***************************************************/
GetInterestedUser.removeByValue = function (arr, val) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i].id == val.id) {
            InterestedUserArray.splice(i, 1);
            break;
        }
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-23
작성목적 : 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
GetInterestedUser.CompareArrayVal = function (arr, val) {
    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (arr.length > 0) {
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].id == val.id) {
                response = 1; //동일한 값이 존재할 경우
                break;
            }
            else {
                response = 0; //다른 값일 경우
            }
        }
    }
    return response;
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-23
작성목적 : 엔터키이벤트
***************************************************/
GetInterestedUser.KeyEvents = function () {
    if (event.keyCode == 13) {
        GetInterestedUser.SearchDeptUserList(1);
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-23
작성목적 : 조직도 검색
***************************************************/
GetInterestedUser.SearchDeptUserList = function (param) {
    var searchName = $("#UCUserDeptTreeScript1_tbDeptUser").val();
    var params = searchName + "" + param + "" + CompanyID;
    UCUserDeptTreeScript1_CallBack1.Callback(params);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-23
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

    // type(G) | deptID | deptName | parentDeptID | titleID | titleName | loginID | mail | officephone | DutyName
    // type(M) | userID | userName | parentDeptID | titleID | titleName | loginID | mail | officephone | DutyName
    $("#tvSelectedValue").val(selValues);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-23
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

        var colSplit = rtnValue.split("^"); 
        var rowSplit = colSplit[1].split("$");
        var selID = colSplit[0];

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
작성자   : 이지원
작성일자 : 2017-03-23
작성목적 : 저장전 설정
***************************************************/
GetInterestedUser.Process = function () {
    var rtSulrt = "";
    if (InterestedUserArray.length > 0) {
        $.each(InterestedUserArray, function (i, v) {
            if (v.name != null) {
                rtSulrt += v.id + '|';
            }
        });
    }

    $("#tbIterestedUserIDValue").val(rtSulrt);

    GetInterestedUser.SetInterestedUser(rtSulrt);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-23
작성목적 : 저장
***************************************************/
GetInterestedUser.SetInterestedUser = function (_interestedUser) {

    var url = "/DeskPlusEIP/Board/Ws/Common/InterestedUser.asmx/SetInterestedUser";

    ParamArray = {
        'interestedUser': _interestedUser
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("GetInterestedUser.SetInterestedUserSuccess");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-23
작성목적 : 저장 성공시
***************************************************/
GetInterestedUser.SetInterestedUserSuccess = function (data, status) {
    GetInterestedUser.GoTree();
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-27
작성목적 : 저장 후 트리 새로고침
***************************************************/
GetInterestedUser.GoTree = function () {

    ParamArray = {
        'dcparm': Dcparm,
        'menuCatID': MenuCatID,
        'catID': CatID,
        'accActive': AccordionActive
    };

    CallEncrypt(CallGetParam());
   
    if (Kind != "HOME") {
        goUrl = "/DeskPlusEIP/Board/Category/Tree.aspx?p=" + GetEncryptValue;
        if (GetInterestedUser.TreeFrame) GetInterestedUser.TreeFrame.location.href = goUrl;
    }
}