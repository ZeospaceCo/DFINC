List = {}
var AuthArray = [];
var CompareArray = [];
var DelArray = [];

$(function () {
    $(document).ready(function () {

        /* 2017-02-10 제거
        //자동화완성 autocomplete - written by shimpark
        $("#UCUserDeptTreeScript1_tbDeptUser").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/DeskPlusEIP/Account/Ws/Common/GetUserDept.asmx/GetDeptUserSearch",
                    data: "{ 'companyID': '" + CompanyID + "','gubun': '','searchName': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        //Callback method when the precess is success
                        response($.map(data.d, function (item) {
                            return {
                                label: item.Name,
                                value: item.Values
                            }
                        }))
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Communication Error");
                    }
                });
            },
            focus: function (event, ui) {
                // prevent autocomplete from updating the textbox
                event.preventDefault();
                // manually update the textbox
                $(this).val(ui.item.value);
            },
            select: function (event, ui) {
                // prevent autocomplete from updating the textbox
                event.preventDefault();
                // manually update the textbox and hidden field
                $(this).val(ui.item.value);
                $("#UCUserDeptTreeScript1_tbDeptUser").val(ui.item.value);
            },
            minLength: 1
        }).autocomplete("widget").addClass("fixed-height");
        */

        //검색입력필드 엔터 이벤트 기술
        $("#UCUserDeptTreeScript1_tbDeptUser").keydown(function (event) {
            List.KeyEvents();
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

        //취소
        $('#CbtnTreeView').click(function (e) {
            e.preventDefault();
            List.SearchDeptUserList(2);
        });

        //검색
        $('#CbtnDeptUserSearch').click(function (e) {
            e.preventDefault();

            List.SearchDeptUserList(1);
        });

        //공유자추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.AddAuth();
            // ShareUserPop.SearchDeptUserList(1);
        });

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            List.AddConfirm();
            // ShareUserPop.SearchDeptUserList(1);
        });
    });
});

/*****************
작성자   : DANIEL
작성목적 : 권한추가
**********************/
List.AddConfirm = function () {
    if ($("#hdseluser").val().length == 0) {
        alert(objectSelect); ///objectSelect
        return;
    }

    var rtSulrt = "";
    if (AuthArray.length > 0) {
        $.each(AuthArray, function (i, v) {
            if (v.name != null) {
                rtSulrt += v.name + '|' + v.id + '#'
            }
        });
    }
    List.AddOpenUser(rtSulrt);
    //tbAuthValue

}

/**************************************************
작성자   : shimpark
작성목적 : 권한추가 버튼
***************************************************/
List.AddAuth = function () {
    if ($("#hdseluser").val().length == 0) {
        alert(objectSelect); //objectSelect
        return;
    }

    var selectedID = $("#tvSelectedID").val().trimEnds(';');
    var selectedValue = $("#tvSelectedValue").val().trimEnds(';');

    selectedValue = replaceAll(selectedValue, '$', '|');
    selectedValue = replaceAll(selectedValue, '/', '|');

    if (selectedID == "" && selectedValue == "") {
        alert(ShareSelect); //ShareSelect
        return;
    }

    CompareArray = [];

    if (selectedID != "" && selectedValue != "") {
        var selStr = selectedValue.split(';');
        for (i = 0; i < selStr.length; i++) {
            var selGubun = selStr[i].split('|')[0];
            var selOwnerID = selStr[i].split('|')[1];
            var selName = selStr[i].split('|')[2];

            //1. 넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
            List.AddCompareArray(selGubun + "$" + selOwnerID, selName);
            var response = false;

            //2. 넘어온 권한값 배열과 권한값 비교하기
            $.each(CompareArray, function (i, v) {
                response = List.CompareArrayVal(AuthArray, v);
            });

            //3. 중복값은 1을 의미함.
            if (response != 1) {
                List.AddAuthRow(selGubun + "$" + selOwnerID, selName);
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
List.AddCompareArray = function (keyID, keyName) {
    CompareArray.push({
        'id': keyID,
        'name': keyName
    });

    //    $.each(CompareArray, function (i, v) {
    //        alert('1)Name: ' + v.name + ' with ID: ' + v.id);
    //    });
}

/**************************************************
작성자   : shimpark
작성목적 : 권한값 table 생성하기
***************************************************/
List.AddAuthRow = function (params, name) {
    var rows = $("#contentTable tr");
    var index = rows.length;

    //var selStr = params.split('$');

    $("#contentTable").append(
		"<tr id='boardAuth" + index + "'>"
			+ "<td class='center'>"
			+ name
			+ "</td>"

			+ "<td class='center'><a onclick=List.DelAuthRow(" + index + ",'" + params + "','" + name + "')><img src='" + ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif' style='cursor:hand' /></a></td>"
		+ "</tr>"
	);

    List.AddAuthArray(params, name)
}

/**************************************************
작성자   : shimpark
작성목적 : 권한값 table 특정 row 삭제하기
***************************************************/
List.DelAuthRow = function (index, keyID, keyName) {

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
        List.removeByValue(AuthArray, v);
    });

    //    $.each(DelArray, function (i, v) {
    //    	alert('3)Name: ' + v.name + ' with ID: ' + v.id);
    //    });
}

/**************************************************
작성자   : shimpark
작성목적 : 권한 배열값 저장소; 비교 대상자 배열값
***************************************************/
List.AddAuthArray = function (keyID, keyName) {
    AuthArray.push({
        'id': keyID,
        'name': keyName
    });

    //	$.each(AuthArray, function (i, v) {
    //	    alert('2)Name: ' + v.name + ' with ID: ' + v.id);
    //	});
}

/**************************************************
작성자   : shimpark
작성목적 : 권한 배열값에서 특정값 삭제 : table row 삭제시 사용됨
***************************************************/
List.removeByValue = function (arr, val) {
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
List.CompareArrayVal = function (arr, val) {
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
작성자   : daniel
작성일자 : 2011-04-11
작성목적 : 엔터키이벤트
***************************************************/
List.KeyEvents = function () {
    if (event.keyCode == 13) {
        List.SearchDeptUserList(1);
    }
}

List.KeyEvents2 = function () {
    if (event.keyCode == 13) {
        List.SearchDeptUserList2(1);
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 조직도 검색
***************************************************/
List.SearchDeptUserList = function (param) {
    var searchName = $("#UCUserDeptTreeScript1_tbDeptUser").val();

    var params = searchName + "" + param;
    UCUserDeptTreeScript1_CallBack1.Callback(params);
}

/***********************
작성자   : Daniel
작성일자 : 2011-09-20
작성목적 : 공유자가져오기
**************************************/
List.GetOpenUser = function (userid) {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Admin/Share/List.asmx/GetOpenUser";

    ParamArray = {
        'companyID': CompanyID,
        'UserId': userid
    };

    var obj = eval("List.SuccessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/***********************
작성자   : Daniel
작성일자 : 2011-09-20
작성목적 : 공유자가져오기성공
**************************************/
List.SuccessList = function (data, status) {
    AuthArrayList = '';
    var rows = $("#contentTable tr");
    var index = rows.length;
    for (var i = 0; i < index; i++) {
        var row = $("#boardAuth" + i);
        row.remove();
    }

    var sId = "";
    var sVal = "";

    $.each(data.d, function (index, list) {
        if (list.GroupID == "0") {
            sVal = sVal + "M$" + list.OpenUserID + "$" + list.Name;

            List.AddAuthRow("M$" + list.OpenUserID, list.Name);
        }
        else {
            sVal = sVal + "G$" + list.GroupID + "$" + list.Name;
            List.AddAuthRow("G$" + list.GroupID, list.Name);
        }

        sVal = sVal + ";";
    });

    AuthArrayList = sVal;

    AuthArray = [];
    var AuthArrayListStr = AuthArrayList.split(';');
    for (i = 0; i < AuthArrayListStr.length; i++) {
        AuthArray.push({
            'id': AuthArrayListStr[i].split("$")[0] + "$" + AuthArrayListStr[i].split("$")[1],
            'name': AuthArrayListStr[i].split("$")[2]
        });
    }
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

        selValues = selValues.replace(selValue + ";", "");
        selValues += selValue + ";";
    } else {
        selIDs = selIDs.replace(selID + "|", "");
        selValues = selValues.replace(selValue + ";", "");
    }

    $("#tvSelectedID").val(selIDs);

    // type(G) | deptID | deptName | parentDeptID | titleID | titleName | loginID | mail | officephone | DutyName
    // type(M) | userID | userName | parentDeptID | titleID | titleName | loginID | mail | officephone | DutyName
    $("#tvSelectedValue").val(selValues);
}

/********************

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

List.SetShare = function (strUser) {
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Admin/Share/List.asmx/SetOpenUser";

    ParamArray = {
        'Share': strUser
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessAdd");

    AjaxService(url, data, obj);
}

List.SuccessAdd = function (data, status) {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**************************************************
작성자   : 김광일
작성목적 : 유저 저장
***************************************************/
List.AddOpenUser = function (arg) {
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Admin/Share/List.asmx/AddAdminOpenUser";

    ParamArray = {
        'UserList': arg,
        'UserId': $("#hdseluser").val()
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessOpenAdd");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 김광일
작성목적 : 유저 저장 성공
***************************************************/
List.SuccessOpenAdd = function (data, status) {
    alert(SharePesonSuccess); //SharePesonSuccess
    List.GetOpenUser($("#hdseluser").val());
}