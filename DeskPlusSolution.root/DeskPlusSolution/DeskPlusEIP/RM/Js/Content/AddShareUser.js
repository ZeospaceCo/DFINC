AddShareUser = {}
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
            AddShareUser.KeyEvents();
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

            AddShareUser.Confirm();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();

            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //추가
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();

            AddShareUser.AddAuth();
        });

        //취소
        $('#CbtnTreeView').click(function (e) {
            e.preventDefault();
            $("#tvSelectedID").val("");
            $("#tvSelectedValue").val("");
            AddShareUser.SearchDeptUserList(2);
        });

        //검색
        $('#CbtnDeptUserSearch').click(function (e) {
            e.preventDefault();

            AddShareUser.SearchDeptUserList(1);
        });
    });
});

function OnData() {
    //__doPostBack("LBTNGetData", "");
}

/**************************************************
작성자   : shimpark
작성목적 : 권한추가 버튼
***************************************************/
AddShareUser.AddAuth = function () {
    //deptID | deptName | parentDeptID | deptOrder
    var selectedValue = $("#tvSelectedValue").val().trimEnds('$');

    if (selectedValue == "") {
        return;
    }

    CompareArray = [];

    if (selectedValue != "") {
        var selStr = selectedValue.split('$');

        for (i = 0; i < selStr.length; i++) {
            var selGubun = selStr[i].split('|')[0];
            var selOwnerID = selStr[i].split('|')[1];

            var selName = '';
            if (selStr[i].split('|')[0] == "G") {
                selName = selStr[i].split('|')[2];

                var arselName = selName.split('/');
                selName = arselName[0];
            }
            else {
                var arselName = selStr[i].split('|')[2].split('/');

                selName = selStr[i].split('|')[5] + "/" + selStr[i].split('|')[11] + "/" + arselName[0];

                //selName = selStr[i].split('|')[5] + "/" + selStr[i].split('|')[11] + "/" + selStr[i].split('|')[2];
            }

            //1. 넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
            AddShareUser.AddCompareArray(selGubun + "$" + selOwnerID, selName);
            var response = false;

            //2. 넘어온 권한값 배열과 권한값 비교하기
            $.each(CompareArray, function (i, v) {
                response = AddShareUser.CompareArrayVal(AuthArray, v);
            });

            if (selGubun != 'G') {
                if (selOwnerID == UserID) {
                    response = 2;
                }
            }

            //3. 중복값은 1을 의미함.
            if (response != 1 && response != 2) {
                AddShareUser.AddAuthRow(selGubun + "$" + selOwnerID, selName);
            }

            if (response == 2) {
                alert(R_Msg_AddMe);
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
AddShareUser.AddCompareArray = function (keyID, keyName) {
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
AddShareUser.AddAuthRow = function (params, name) {
    var rows = $("#contentTable tr");
    var index = rows.length;

    var selStr = params.split('$');

    $("#contentTable").append(
		"<tr id='boardAuth" + index + "'>"
			+ "<td class='center'>"
			+ name
			+ "</td>"
            + "<td class='center'><a class=btn onclick=AddShareUser.DelAuthRow(" + index + ",'" + params + "','" + name + "') >" + R_Del + "</a></td>"
			//+ "<td class='center'><a onclick=\"AddShareUser.DelAuthRow(" + index + ",'" + params + "','" + name + "')\"><img src='" + ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif' style='cursor:hand' /></a></td>"
		+ "</tr>"
	);

    AddShareUser.AddAuthArray(params, name)
}

/**************************************************
작성자   : shimpark
작성목적 : 권한값 table 특정 row 삭제하기
***************************************************/
AddShareUser.DelAuthRow = function (index, keyID, keyName) {
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
        AddShareUser.removeByValue(AuthArray, v);
    });

    //    $.each(DelArray, function (i, v) {
    //    	alert('3)Name: ' + v.name + ' with ID: ' + v.id);
    //    });
}

/**************************************************
작성자   : shimpark
작성목적 : 권한 배열값 저장소; 비교 대상자 배열값
***************************************************/
AddShareUser.AddAuthArray = function (keyID, keyName) {
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
AddShareUser.removeByValue = function (arr, val) {
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
AddShareUser.CompareArrayVal = function (arr, val) {
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
AddShareUser.ApplyAllAuthManage = function () {
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
AddShareUser.ApplyEnterpriceAuthManage = function () {
    if ($("#cbEnterpriceAuthManage").is(":checked")) {
        CompareArray = [];

        //1. 넘어온 권한값 배열에 할당하기(G$1,영업부)
        AddShareUser.AddCompareArray("G$0", Whole);
        var response = false;

        //2. 넘어온 권한값 배열과 권한값 비교하기
        $.each(CompareArray, function (i, v) {
            response = AddShareUser.CompareArrayVal(AuthArray, v);
        });

        //3. 중복값은 1을 의미함.
        if (response != 1) {
            AddShareUser.AddAuthRow("G$0", Whole);
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
AddShareUser.Confirm = function () {

    var SelShareUserArray = [];
    var IsOverlap = false;
    var OverlapShareUser = "";

    if (AuthArray.length > 0) {
        $.each(AuthArray, function (i, v) {
            if (v.name != null && v.name != undefined) {
                if (top.opener.ShareUserArray.length > 0) {
                    for (var i = 0; i < top.opener.ShareUserArray.length; i++) {
                        if (top.opener.ShareUserArray[i].id == v.id) {
                            IsOverlap = true;
                            OverlapShareUser = v.name;
                            return false;
                        }                        
                    }
                    if (!IsOverlap) {
                        SelShareUserArray.push({
                            'id': v.id,
                            'name': v.name
                        });
                    }                    
                }
                else {
                    SelShareUserArray.push({
                        'id': v.id,
                        'name': v.name
                    });
                }
            }


        });
    }
    if (IsOverlap) { alert("[" + OverlapShareUser + "]" + R_Msg_OverlapShareUser); return; }
    if (SelShareUserArray.length <= 0) { alert(R_Msg_AddShareUser); return; }
    eval("top.opener.SuccessAddShareUser")(SelShareUserArray);
    self.close();
}

/**************************************************
작성자   : daniel
작성일자 : 2011-04-11
작성목적 : 엔터키이벤트
***************************************************/
AddShareUser.KeyEvents = function () {
    if (event.keyCode == 13) {
        AddShareUser.SearchDeptUserList(1);
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 조직도 검색
***************************************************/
AddShareUser.SearchDeptUserList = function (param) {
    var searchName = $("#UCUserDeptTreeScript1_tbDeptUser").val();

    //var params = searchName + "" + param;
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

AddShareUser.SetShare = function (strUser) {
    var url = "/DeskPlusEIP/Pim/Ws/Common/AddShareUser.asmx/SetOpenUser";

    ParamArray = {
        'Share': strUser
    };

    CallEncrypt(CallGetParam());

    $("#txttemp").val(GetEncryptValue);

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AddShareUser.SuccessAdd");

    AjaxService(url, data, obj);
}

AddShareUser.SuccessAdd = function (data, status) {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

AddShareUser.AllClear = function () {
    for (var i = 0; i < AuthArray.length; i++) {
        AuthArray.splice(i, 1);
    }
}