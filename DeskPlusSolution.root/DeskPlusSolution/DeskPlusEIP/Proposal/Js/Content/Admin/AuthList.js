AuthList = {}

var AuthArray = []; //db 에서 권한 값을 권한배열에 담는다.
var CompareArray = []; //넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        /* 2017-02-10 제거
        //자동화완성 autocomplete - written by shimpark (사용자)
        $("#DeptUserTreeScript1_tbDeptUser").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/DeskPlusEIP/Account/Ws/Common/GetUserDept.asmx/GetDeptUserSearch",
                    data: "{ 'companyID': '" + CompanyID + "','gubun': 'M','searchName': '" + request.term + "' }",
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
                $("#DeptUserTreeScript1_tbDeptUser").val(ui.item.value);
            },
            minLength: 1
        }).autocomplete("widget").addClass("fixed-height");
        */

        //검색입력필드 엔터 이벤트 기술
        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (event) {
            AuthList.KeyEvents();
        });

        //UserID$name;  
        var AuthArrayListStr = AuthArrayList.split(';');
        for (i = 0; i < AuthArrayListStr.length; i++) {
            if (AuthArrayListStr[i].split("$")[0] != undefined) {
                //UserID
                var keyid = AuthArrayListStr[i].split("$")[0];
                //이름
                var keyName = AuthArrayListStr[i].split("$")[1];
                AuthList.AddAuthArray(keyid, keyName);
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

        //추가
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            AuthList.AddAuth();
        });

        //수정
        $('#CbtnMod').click(function (e) {
            e.preventDefault();
            AuthList.Process();
        });

    });
});

/**************************************************
작성자   : shine
작성목적 : 권한추가 버튼
***************************************************/
AuthList.AddAuth = function () {

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

            //keyid: UserID
            //keyName: 이름

            //배열에 할당하기(UserID$이름)
            AuthList.AddCompareArray(selOwnerID, selName);
        }

        //2. 넘어온 권한값(CompareArray[]) 배열과 권한값(AuthArray[]) 비교하기
        $.each(CompareArray, function (i, v) {
            AuthList.CompareArrayVal(v.id, v.name);
        });

    }
    else {
        alert(R_SettingAuthority);
    }
}

/**************************************************
작성자   : shine
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
AuthList.CompareArrayVal = function (compID, compName) {

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
            AuthList.AddAuthRow(compID, compName);
        }
    }
    else {
        response = 0; //다른 값일 경우
        AuthList.AddAuthRow(compID, compName);
    }

    return response;
}

/**************************************************
작성자   : shine
작성목적 : 추가 table row 생성하고 권한값 AuthArray[] 배열에 할당하기
***************************************************/
AuthList.AddAuthRow = function (keyID, name) {
    var rows = $("#contentTable tr");
    RowMax = RowMax + 1;

    var selStr = keyID.split('$');

    $("#contentTable").append(
		"<tr id='boardAuth" + RowMax + "'>"
			+ "<td id='" + keyID + "'>"
			+ unescape(name)
			+ "</td>"
            + "<td><a class='btn' onclick=\"AuthList.DelAuthRow(" + RowMax + ",'" + keyID + "')\">삭제</a></td>"
			//+ "<td><a onclick=\"AuthList.DelAuthRow(" + RowMax + ",'" + keyID + "')\"><img src='" + ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif' /></a></td>"
		+ "</tr>"
	);

    AuthList.AddAuthArray(keyID, name)
}

/**************************************************
작성자   : shine
작성목적 : 권한값 table 특정 row 삭제하기
***************************************************/
AuthList.DelAuthRow = function (index, keyID) {
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
작성자   : shine
작성목적 : 권한 배열값 저장소; 비교 대상자 배열값
keyid : UserID
keyName : 이름
***************************************************/
AuthList.AddAuthArray = function (keyID, keyName) {

    AuthArray.push({
        'id': keyID,
        'name': escape(keyName)
    });
}

/**************************************************
작성자   : shine
작성목적 : 조직도 넘어온 값 저장소, 차후 비교값
***************************************************/
AuthList.AddCompareArray = function (keyID, keyName) {
    
    CompareArray.push({
        'id': keyID,
        'name': escape(keyName)
    });
}

/**************************************************
작성자   : shine
작성일자 : 2011-01-26
작성목적 : 처리하기
***************************************************/
AuthList.Process = function () {

    var rows = $("#contentTable tbody tr");
    var authVal = "";

    // UserID 가져오기
    $(rows).each(function (idx) {
        if (idx > 0) {
            for (var i = 0; i < 1; i++) {
                authVal += $(rows[idx]).find('td').eq(i).context.firstChild.id + "^";
            }
        }
    });

    authVal = authVal.trimEnds('^');

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
    $('#loading').html(loadingPath);

    var url = "/DeskPlusEIP/Proposal/Ws/Content/Admin/AuthList.asmx/Add";

    ParamArray = {
        'authList': authVal,
        'degree': 1,
        'status': 'A'
    };

    var obj = eval("AuthList.Success");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}

/************************************************************************
작성자   : shine
작성일자 :2011-06-15
작성목적 : 1차 심사자 수정 완료
*************************************************************************/
AuthList.Success = function (data, status) {
    $('#loading').html("");
    if (data.d == true) {
        alert("처리되었습니다");
    }
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 엔터키이벤트
***************************************************/
AuthList.KeyEvents = function () {
    if (event.keyCode == 13) {
        SearchDeptUserList(1);
    }
}

/**************************************************
작성자   : shine
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
작성자   : shine
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
작성자   : shine
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
        selValues = selValues.replace(selValue + "$", "");
        selValues += selValue + "$";
    });

    // type(G) | deptID | deptName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName
    // type(M) | userID | userName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName

    $("#tvSelectedID").val(selIDs);
    $("#tvSelectedValue").val(selValues);

    //alert(selValues);
}