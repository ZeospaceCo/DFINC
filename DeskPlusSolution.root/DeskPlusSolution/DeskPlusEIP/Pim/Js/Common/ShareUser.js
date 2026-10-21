ShareUser = {}
var AuthArray = [];
var CompareArray = [];
var DelArray = [];

$(function () {
    $(document).ready(function () {

        //검색입력필드 엔터 이벤트 기술
        $("#UCUserDeptTreeScript1_tbDeptUser").keydown(function (event) {
            ShareUser.KeyEvents();
        });

        //db 에서 권한 값을 권한배열에 담는다.
        AuthArray = [];
        var AuthArrayListStr = AuthArrayList.split(';');
        for (i = 0; i < AuthArrayListStr.length; i++) {
            AuthArray.push({
                'id': AuthArrayListStr[i].split("$")[0] + "$" + AuthArrayListStr[i].split("$")[1],
                'name': AuthArrayListStr[i].split("$")[2]
            });
        }

        //확인
        $('#CbtnSubmit').click(function (e) {
            e.preventDefault();
            ShareUser.Process();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();

            //parent.ModalHide();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //추가
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            ShareUser.AddAuth();
        });

        //취소
        $('#CbtnTreeView').click(function (e) {
            e.preventDefault();
            ShareUser.SearchDeptUserList(2);
        });

        //검색
        $('#CbtnDeptUserSearch').click(function (e) {
            e.preventDefault();

            ShareUser.SearchDeptUserList(1);
        });
    });
});

/**************************************************
작성자   : shimpark
작성목적 : 권한추가 버튼
***************************************************/
ShareUser.AddAuth = function () {
    var selectedID = $("#tvSelectedID").val().trimEnds('|');
    //deptID | deptName | parentDeptID | deptOrder
    var selectedValue = $("#tvSelectedValue").val().trimEnds('$');

    if (selectedID == "" && selectedValue == "") {
        return;
    }

    CompareArray = [];

    if (selectedID != "" && selectedValue != "") {
        var selStr = selectedValue.split('$');
        for (i = 0; i < selStr.length; i++) {
            var selGubun = selStr[i].split('|')[0];
            var selOwnerID = selStr[i].split('|')[1];
            var selName = selStr[i].split('|')[2];

            //1. 넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
            ShareUser.AddCompareArray(selGubun + "$" + selOwnerID, selName);
            var response = false;

            //2. 넘어온 권한값 배열과 권한값 비교하기
            $.each(CompareArray, function (i, v) {
                response = ShareUser.CompareArrayVal(AuthArray, v);
            });

            //3. 중복값은 1을 의미함.
            if (response != 1) {
                ShareUser.AddAuthRow(selGubun + "$" + selOwnerID, selName);
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
작성자   : shimpark
작성목적 : 조직도 넘어온 값 저장소, 차후 비교값
***************************************************/
ShareUser.AddCompareArray = function (keyID, keyName) {
    CompareArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : shimpark
작성목적 : 권한값 table 생성하기
***************************************************/
ShareUser.AddAuthRow = function (params, name) {
    var rows = $("#contentTable tr");
    var index = rows.length;

    var selStr = params.split('$');

    $("#contentTable").append(
		"<tr id='boardAuth" + index + "'>"
			+ "<td class='center'>"
			+ name
			+ "</td>"
            + "<td class='center'><a class=btn onclick=\"ShareUser.DelAuthRow(" + index + ",'" + params + "','" + name + "')\">" + R_Del + "</a></td>"
			//+ "<td class='center'><a onclick=\"ShareUser.DelAuthRow(" + index + ",'" + params + "','" + name + "')\"><img src='" + ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif' style='cursor:hand' /></a></td>"
		+ "</tr>"
	);

    ShareUser.AddAuthArray(params, name)
}

/**************************************************
작성자   : shimpark
작성목적 : 권한값 table 특정 row 삭제하기
***************************************************/
ShareUser.DelAuthRow = function (index, keyID, keyName) {
    var row = $("#boardAuth" + index);
    row.fadeOut("slow", function () {
        row.remove();
    });

    DelArray = [];

    DelArray.push({
        'id': keyID,
        'name': keyName
    });

    $.each(DelArray, function (i, v) {
        ShareUser.removeByValue(AuthArray, v);
    });
}

/**************************************************
작성자   : shimpark
작성목적 : 권한 배열값 저장소; 비교 대상자 배열값
***************************************************/
ShareUser.AddAuthArray = function (keyID, keyName) {
    AuthArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : shimpark
작성목적 : 권한 배열값에서 특정값 삭제 : table row 삭제시 사용됨
***************************************************/
ShareUser.removeByValue = function (arr, val) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i].id == val.id) {
            AuthArray.splice(i, 1);
            break;
        }
    }
}

/**************************************************
작성자   : shimpark
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
ShareUser.CompareArrayVal = function (arr, val) {
    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (arr.length > 0) {
        for (var i = 0; i < arr.length; i++) {
            //            alert(arr[i].id);
            //            alert(val.id);
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
작성자   : shimpark
작성목적 : 일괄 권한 설정 기술
***************************************************/
ShareUser.ApplyAllAuthManage = function () {
    if ($("#cbAllAuthManage").is(":checked")) {
        if (window.confirm(R_ApplyFolderAllAuthority)) {
            var selValue = $("#ddlAllAuthManage").val();
            $("#contentTable select").each(
                function (i) {
                    $(this).children("option:eq(" + parseInt(selValue - 1) + ")").attr("selected", true);
                }
            );
        }
        else {
            $("#cbAllAuthManage").attr("checked", false);
        }
    }
}

/**************************************************
작성자   : shimpark
작성목적 : 전사권한 설정 기술
***************************************************/
ShareUser.ApplyEnterpriceAuthManage = function () {
    if ($("#cbEnterpriceAuthManage").is(":checked")) {
        CompareArray = [];

        //1. 넘어온 권한값 배열에 할당하기(G$1,영업부)
        ShareUser.AddCompareArray("G$0", Whole);
        var response = false;

        //2. 넘어온 권한값 배열과 권한값 비교하기
        $.each(CompareArray, function (i, v) {
            response = ShareUser.CompareArrayVal(AuthArray, v);
        });

        //3. 중복값은 1을 의미함.
        if (response != 1) {
            ShareUser.AddAuthRow("G$0", Whole);
        }
        else {
            alert(R_AlreadySettingAuthority);
        }      
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-01-26
작성목적 : 처리하기
***************************************************/
ShareUser.Process = function () {
    var rtSulrt = "";
    var WriteUser = "";
    var WriteUid = "";

    if (AuthArray.length > 0) {
        $.each(AuthArray, function (i, v) {
            if (v.name != null) {
                rtSulrt += v.name + '|' + v.id + '#'
                WriteUser += v.name + "     ";
                WriteUid += v.id + "$" + v.name + ",";
            }
        });
    }

    $("#tbAuthValue").val(rtSulrt);

    if (opener != null) {
        opener.AddShare(WriteUser, WriteUid);
    }
    else {
        parent.AddShare(WriteUser, WriteUid);
    }

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**************************************************
작성자   : daniel
작성일자 : 2011-04-11
작성목적 : 엔터키이벤트
***************************************************/
ShareUser.KeyEvents = function () {
    if (event.keyCode == 13) {
        ShareUser.SearchDeptUserList(1);
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 조직도 검색
***************************************************/
ShareUser.SearchDeptUserList = function (param) {
    var searchName = $("#UCUserDeptTreeScript1_tbDeptUser").val().securityText();
    $("#UCUserDeptTreeScript1_tbDeptUser").val(searchName);

    var params = searchName + "" + param + "" + CompanyID;
    UCUserDeptTreeScript1_CallBack1.Callback(params);
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

    // type(G) | deptID | deptName | parentDeptID | titleID | titleName | loginID | mail | officephone | DutyName
    // type(M) | userID | userName | parentDeptID | titleID | titleName | loginID | mail | officephone | DutyName
    $("#tvSelectedValue").val(selValues);
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

    // type(G) | deptID | deptName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName
    // type(M) | userID | userName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName

    $("#tvSelectedID").val(selIDs);
    $("#tvSelectedValue").val(selValues);
}

/* 웹서비스 */

ShareUser.SetShare = function (strUser) {
    var url = "/DeskPlusEIP/Pim/Ws/Common/ShareUser.asmx/SetOpenUser";

    ParamArray = {
        'Share': strUser
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ShareUser.SuccessAdd");

    AjaxService(url, data, obj);
}

ShareUser.SuccessAdd = function (data, status) {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}