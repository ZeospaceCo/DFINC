/// <reference path="../../../../Html/Js/jQuery/jquery.js" />


MultiComUser = {}


var AuthArray = []; //db 에서 부서할당 값을 부서할당배열에 담는다.
var CompareArray = []; //넘어온 부서할당값 배열에 할당하기(G$1$영업부,영업부)
var DelArray = []; //삭제할 값 배열에 담기.

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        /* 2017-02-10 제거
        //자동화완성 autocomplete - written by shimpark
        $("#DeptUserTreeScript1_tbDeptUser").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/DeskPlusEIP/Account/Ws/Common/GetUserDept.asmx/GetDeptUserSearch",
                    data: "{ 'companyID': '" + MultiCompanyID + "','gubun': 'M','searchName': '" + request.term + "' }",
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

        $(".rTableMid").css("background", "#ffff");

        //검색입력필드 엔터 이벤트 기술
        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (event) {
            MultiComUser.KeyEvents();
        });

        //db 에서 부서할당 값을 부서할당배열에 담는다.        
        var AuthArrayListStr = AuthArrayList.split(';');
        for (i = 0; i < AuthArrayListStr.length; i++) {

            if (AuthArrayListStr[i].split("$")[1] != undefined) {

                var selUserID = AuthArrayListStr[i].split("$")[0]; //userid
                var selUserName = AuthArrayListStr[i].split("$")[1]; //name
                var selDeptID = AuthArrayListStr[i].split("$")[2]; //deptid
                var selDeptName = AuthArrayListStr[i].split("$")[3]; //deptname

                MultiComUser.AddAuthArray(selUserID, selUserName, selDeptID, selDeptName);
            }
        }

        //저장
        $('#cbtnSave').click(function (e) {
            e.preventDefault();
            MultiComUser.Process();
        });

        //부서 추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            MultiComUser.AddAuth();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            self.close();
        });

        //멀티회사 선택하기
        $("#ddlMultiCompany").change(function (e) {

            ParamArray = {
                'companyID': CompanyID,
                'userID': UserID,
                'multiCompanyID': $("#ddlMultiCompany").val(),
                'groupGUID': GroupGUID
            };

            CallEncrypt(CallGetParam());
            location = "MultiComUser.aspx?p=" + GetEncryptValue;
        });

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

    });
});


/**************************************************
작성자   : shimpark
작성목적 : 부서할당추가 버튼
***************************************************/
MultiComUser.AddAuth = function () {

    //deptid
    var selectedID = $("#tvSelectedID").val().trimEnds('|');

    // type(G) | deptID | deptName | parentDeptID | titleID | titleName | loginID | mail | Phone | officephone | DutyName
    // type(M) | userID | userName | parentDeptID | titleID | titleName | loginID | mail | Phone | officephone | DutyName | deptName | deptid

    //deptID | deptName | parentDeptID | deptOrder
    var selectedValue = $("#tvSelectedValue").val().trimEnds('$');

    if (selectedID != "" && selectedValue != "") {

        CompareArray = []; //넘어온 부서할당값

        var selStr = selectedValue.split('$');        

        //deptID | deptName | parentDeptID | deptOrder
        for (i = 0; i < selStr.length; i++) {
            var selUserID = selStr[i].split('|')[1];
            var selUserName = selStr[i].split('|')[2];
            var selDeptID = selStr[i].split('|')[12];
            var selDeptName = selStr[i].split('|')[11];

            //1. 넘어온 부서할당값 배열에 할당하기(G$1$영업부,영업부)
            MultiComUser.AddCompareArray(selUserID, selUserName, selDeptID, selDeptName);
        }

        //2. 넘어온 부서할당값(CompareArray[]) 배열과 부서할당값(AuthArray[]) 비교하기
        $.each(CompareArray, function (i, v) {
            MultiComUser.CompareArrayVal(v.uID, v.uName, v.id, v.name);
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
MultiComUser.CompareArrayVal = function (uID, uName, selDeptID, selDeptName) {

    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (AuthArray.length > 0) {
        for (var i = 0; i < AuthArray.length; i++) {

            //alert("AuthArray[i].id : [" + i + "]" + v.id + " | compArr.id : " + compArr.id);
            //userid 로 비교
            if (AuthArray[i].uID == uID) {
                response = 1; //동일한 값이 존재할 경우
                break;
            }
            else {
                response = 0; //다른 값일 경우                
            }
        }

        if (response == 0) {
            //중복되지 않으면 부서할당값 할당하기
            MultiComUser.AddAuthRow(uID, uName, selDeptID, selDeptName);
        }
    }
    else {
        response = 0; //다른 값일 경우
        MultiComUser.AddAuthRow(uID, uName, selDeptID, selDeptName);
    }

    return response;
}


/**************************************************
작성자   : shimpark
작성목적 : 추가 table row 생성하고 부서할당값 AuthArray[] 배열에 할당하기
***************************************************/
MultiComUser.AddAuthRow = function (uID, uName, deptID, deptName) {
    var rows = $("#contentTable tr");
    RowMax = RowMax + 1;    

    $("#contentTable").append(
		"<tr id='boardAuth" + RowMax + "'>"
			+ "<td class='center'>"
			+ deptName
			+ "</td>"
            + "<td class='center'>"
			+ uName
			+ "</td>"
            + "<td class='center'><a class='btn' onclick=\"MultiComUser.DelRow(" + RowMax + ",'" + uID + "')\">" + R_Admin_CompanyManager_btnDel + "</a></td>"
			//+ "<td class='center'><a onclick=\"MultiComUser.DelRow(" + RowMax + ",'" + uID + "')\"><img src='" + ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif' /></a></td>"
		+ "</tr>"
	);

    MultiComUser.AddAuthArray(uID, uName, deptID, deptName)
}


/**************************************************
작성자   : shimpark
작성목적 : 부서할당값 table 특정 row 삭제하기
***************************************************/
MultiComUser.DelRow = function (index, uID) {

    var row = $("#boardAuth" + index);

    for (var i = 0; i < AuthArray.length; i++) {
        //사용자 기준으로 삭제한다.
        if (AuthArray[i].uID == uID) {

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
MultiComUser.AddAuthArray = function (uID , uName, deptID, deptName) {
    AuthArray.push({
        'uID': uID,
        'uName': uName,
        'id': deptID,
        'name': deptName
    });
}

/**************************************************
작성자   : shimpark
작성목적 : 조직도 넘어온 값 저장소, 차후 비교값
***************************************************/
MultiComUser.AddCompareArray = function (uID, uName, deptID, deptName) {
    CompareArray.push({
        'uID': uID,
        'uName': uName,
        'id': deptID,
        'name': deptName
    });
}



/**************************************************
작성자   : 심재운
작성일자 : 2011-01-26
작성목적 : 처리하기
***************************************************/
MultiComUser.Process = function () {

    var selUserID = ""; //userid
    var selDeptID = ""; //deptid

    if (AuthArray.length > 0) {
        for (var i = 0; i < AuthArray.length; i++) {
            selUserID += AuthArray[i].uID + ",";//userid
            selDeptID += AuthArray[i].id + ",";//deptid
        }        
    }

    MultiComUser.Add(selUserID.trimEnds(","), selDeptID.trimEnds(","));

}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 사용자의 부서 저장하기
***************************************************/
MultiComUser.Add = function (uIDList, deptList) {

    var url = "/DeskPlusEIP/Account/Ws/Admin/User/MultiComUser.asmx/AddUserDept";
    
    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID,
        'multiCompanyID': MultiCompanyID,
        'groupGUID': GroupGUID,
        'userList': uIDList,
        'deptList': deptList
    };

    var obj = eval("MultiComUser.SuccessAdd");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시  로그인 체크
**********************************************************************************************/
MultiComUser.SuccessAdd = function (data, status) {
    if (data.d == true) {
        alert(R_Admin_UserManager_MSG_SUCCESS);

        ParamArray = {
            'companyID': CompanyID,
            'userID': UserID,
            'multiCompanyID': MultiCompanyID
        };

        CallEncrypt(CallGetParam());
        location = "/DeskPlusEIP/Account/Admin/User/MultiComUser.aspx?p=" + GetEncryptValue;
    }
    else {
        alert(R_ProcessingError);
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

        selValues = selValues.replace(selValue + "$", "");
        selValues += selValue + "$";
    } else {
        selIDs = selIDs.replace(selID + "|", "");
        selValues = selValues.replace(selValue + "$", "");
    }

    $("#tvSelectedID").val(selIDs); //deptid

    // type(G) | deptID | deptName | parentDeptID | titleID | titleName | loginID | mail | Phone | officephone | DutyName
    // type(M) | userID | userName | parentDeptID | titleID | titleName | loginID | mail | Phone | officephone | DutyName

    //deptID | deptName | parentDeptID | deptOrder
    $("#tvSelectedValue").val(selValues);

}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 엔터키이벤트
***************************************************/
MultiComUser.KeyEvents = function () {
    if (event.keyCode == 13) {
        SearchDeptUserList(1);
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 조직도 검색
***************************************************/
function SearchDeptUserList(param) {
    var searchName = $("#DeptUserTreeScript1_tbDeptUser").val().securityText();
    $("#DeptUserTreeScript1_tbDeptUser").val(searchName);

    var params = searchName + "" + param + "" + MultiCompanyID;    
    DeptUserTreeScript1_CallBack1.Callback(params);
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
        var selID = colSplit[0]; //nodeID      
        var selValue = colSplit[1]; //nodeValue        

        //사용자만 추가하도록 한다.
        if (selValue.split("|")[0] == "M") {
            selIDs = selIDs.replace(selID + "|", "");
            selIDs += selID + "|";
            selValues = selValues.replace(selValue + "$", "");
            selValues += selValue + "$";
        }
    });

    // type(G) | deptID | deptName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName
    // type(M) | userID | userName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName

    $("#tvSelectedID").val(selIDs);
    $("#tvSelectedValue").val(selValues);

    //alert(selValues);
}