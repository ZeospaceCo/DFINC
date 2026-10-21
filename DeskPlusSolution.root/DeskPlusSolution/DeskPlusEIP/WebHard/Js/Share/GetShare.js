GetShare = {}
GetShare.TreeFrame = parent.parent.frmLeft;

var BeforeArray = []; //db 에서 넘어 온 값을 배열에 담는다.
var CompareArray = []; //넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
var DelArray = []; //삭제할 값 배열에 담기.

/**************************************************
작성자   : famo23
작성일자 : 2011-10-27
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        /* 2017-02-10 제거
        //자동화완성 autocomplete - written by shimpark
        $("#DeptUserTreeScript1_tbDeptUser").autocomplete({
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
                $("#DeptUserTreeScript1_tbDeptUser").val(ui.item.value);
            },
            minLength: 1
        }).autocomplete("widget").addClass("fixed-height");
        */

        //검색입력필드 엔터 이벤트 기술
        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (event) {
            GetShare.KeyEvents();
        });

        //db 에서 권한 값을 권한배열에 담는다.        
        var ShareArrayListStr = ShareArrayList.split(';');
        for (i = 0; i < ShareArrayListStr.length; i++) {
            if (ShareArrayListStr[i].split("$")[1] != undefined) {
                var keyid = ShareArrayListStr[i].split("$")[0] + "$" + ShareArrayListStr[i].split("$")[1];
                var keyName = ShareArrayListStr[i].split("$")[2];
                GetShare.AddBeforeArray(keyid, keyName);
            }
        }

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            GetShare.Confirm();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //폴더 공유 해제
        $('#CbtnDel').click(function (e) {
            e.preventDefault();
            GetShare.DelShare();
        });

        //공유 추가
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            GetShare.AddShare();
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
작성자   : famo23
작성일자 : 2011-10-27
작성목적 : 엔터키이벤트
***************************************************/
GetShare.KeyEvents = function () {
    if (event.keyCode == 13) {
        SearchDeptUserList(1);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-10-27
작성목적 : 조직도 검색
***************************************************/
function SearchDeptUserList(param) {
    var searchName = $("#DeptUserTreeScript1_tbDeptUser").val().securityText();
    $("#DeptUserTreeScript1_tbDeptUser").val(searchName);

    var params = searchName + "" + param + "" + CompanyID;
    DeptUserTreeScript1_CallBack1.Callback(params);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-10-27
작성목적 : 조직도 선택시
***************************************************/
function DeptUserTreeScript_onNodeSelect(node) {

    $("#tvSelectedID").empty();
    $("#tvSelectedValue").empty();

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
작성자   : famo23
작성일자 : 2011-10-27
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

    //alert(selValues);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-10-27
작성목적 : 권한값 table 특정 row 삭제하기
***************************************************/
GetShare.DelAuthRow = function (index, keyID) {
    var row = $("#webHardSend" + index);

    for (var i = 0; i < BeforeArray.length; i++) {

        if (BeforeArray[i].id == keyID) {

            BeforeArray.splice(i, 1);

            row.fadeOut("slow", function () {
                row.remove();
            });

            break;
        }
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2011-10-27
작성목적 : 권한 배열값 저장소; 비교 대상자 배열값
***************************************************/
GetShare.AddBeforeArray = function (keyID, keyName) {
    BeforeArray.push({
        'id': keyID,
        'name': keyName
    });
}


/**************************************************
작성자   : famo23
작성일자 : 2011-10-27
작성목적 : 조직도 넘어온 값 저장소, 차후 비교값
***************************************************/
GetShare.AddCompareArray = function (keyID, keyName) {
    CompareArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : famo23
작성일자 : 2011-10-27
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
GetShare.CompareArrayVal = function (compID, compName) {
    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (BeforeArray.length > 0) {
        for (var i = 0; i < BeforeArray.length; i++) {

            //alert("BeforeArray[i].id : [" + i + "]" + v.id + " | compArr.id : " + compArr.id);

            if (BeforeArray[i].id == compID) {
                response = 1; //동일한 값이 존재할 경우
                break;
            }
            else {
                response = 0; //다른 값일 경우                
            }
        }

        if (response == 0) {
            //중복되지 않으면 권한값 할당하기
            GetShare.AddAuthRow(compID, compName);
        }
    }
    else {
        response = 0; //다른 값일 경우
        GetShare.AddAuthRow(compID, compName);
    }

    return response;
}


/**************************************************
작성자   : famo23
작성일자 : 2011-10-27
작성목적 : 추가 table row 생성하고 권한값 BeforeArray[] 배열에 할당하기
***************************************************/
GetShare.AddAuthRow = function (keyID, name) {
    var rows = $("#contentTable tr");    

    var tmpKeyID = keyID.replace('$','');

    $("#contentTable").append(
		"<tr id='webHardSend" + RowMax + "'>"
			+ "<td>"
			+ name
			+ "</td>"
            + "<td class='center'><input type='checkbox' id='chxDown" + tmpKeyID + "'></td>"
			+ "<td class='center'><input type='checkbox' id='chxUpload" + tmpKeyID + "'></td>"
            + "<td class='center'><a class='btn' onclick=\"GetShare.DelAuthRow(" + RowMax + ",'" + keyID + "')\">" + R_Del + "</a></td>"
			//+ "<td class='center'><a onclick=\"GetShare.DelAuthRow(" + RowMax + ",'" + keyID + "')\"><img src='" + ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif' style='cursor:hand' /></a></td>"
		+ "</tr>"
	);

    RowMax = RowMax + 1;

    GetShare.AddBeforeArray(keyID, name)
}

/**************************************************
작성자   : famo23
작성일자 : 2011-10-27
작성목적 : 공유 추가 
***************************************************/
GetShare.AddShare = function () {

    //deptid
    var selectedID = $("#tvSelectedID").val().trimEnds('|');
    //deptID | deptName | parentDeptID | deptOrder
    var selectedValue = $("#tvSelectedValue").val().trimEnds('$')

    if (selectedID != "" && selectedValue != "") {

        CompareArray = []; //넘어온 권한값

        var selStr = selectedValue.split('$');
        for (i = 0; i < selStr.length; i++) {
            var selGubun = selStr[i].split('|')[0];
            var selOwnerID = selStr[i].split('|')[1];
            var selName = selStr[i].split('|')[2];

            //1. 넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
            GetShare.AddCompareArray(selGubun + "$" + selOwnerID, selName);
        }

        //2. 넘어온 권한값(CompareArray[]) 배열과 이미 들어온 값(BeforeArray[]) 비교하기
        $.each(CompareArray, function (i, v) {
            GetShare.CompareArrayVal(v.id, v.name);
        });

    }
    else {
        alert(R_SelectAdd);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-10-27
작성목적 : 확인 
***************************************************/
GetShare.Confirm = function () {

    var tmpValue = "";

    if (BeforeArray.length > 0) {
        for (var i = 0; i < BeforeArray.length; i++) {

            if (BeforeArray[i].id == ("M$" + UserID)) {
                alert(R_SelectMe);
                return;
            }

            var tmpKeyID = BeforeArray[i].id.replace('$', '');
            var tmpCheck = "";
            if ($("#chxDown" + tmpKeyID).is(':checked')) {
                tmpValue += BeforeArray[i].id + "$0";
                tmpCheck = "1";
            }
            else {
                tmpValue += BeforeArray[i].id + "$1";
            }

            if ($("#chxUpload" + tmpKeyID).is(':checked')) {
                tmpValue += "$0,";
                tmpCheck = "1";
            }
            else {
                tmpValue += "$1,";
            }

            if (tmpCheck == "") {
                alert(R_SelectCheckBox);
                return;
            }
        }

        tmpValue = tmpValue.trimEnds(",");
 

        //파라미터값 배열에 담기
        ParamArray = {
            'folderID': FolderID,
            'tmpValue': tmpValue,
            'types': 'S'
        };

        CallEncrypt(CallGetParam());
        var url = "/DeskPlusEIP/WebHard/Ws/Share/GetShare.asmx/AddShare";
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("GetShare.SuccessAddShare");

        AjaxService(url, data, obj);
    }
    else {
        alert(R_SelectAdd);
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-10-28
작성목적 : 확인 (웹서비스 값 받기)
**********************************************************************************************/
GetShare.SuccessAddShare = function (data, status) {
    if (data.d) {
        alert(R_CompleteConfirm);

        ParamArray = {        
            'accActive': 1
        };

        CallEncrypt(CallGetParam());
        var goUrl = "/DeskPlusEIP/WebHard/Folder/Tree.aspx?p=" + GetEncryptValue;

        if (GetShare.TreeFrame) GetShare.TreeFrame.location.href = goUrl;

        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else {
        alert(R_ProcessingError);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-10-28
작성목적 : 폴더 공유 해제 
***************************************************/
GetShare.DelShare = function () {

    if (window.confirm(R_DeleteShare)) {
        
        //파라미터값 배열에 담기
        ParamArray = {
            'folderID': FolderID,
            'types': 'S'
        };

        CallEncrypt(CallGetParam());
        var url = "/DeskPlusEIP/WebHard/Ws/Share/GetShare.asmx/DelShare";
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("GetShare.SuccessDelShare");

        AjaxService(url, data, obj);
    }  
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-10-28
작성목적 : 폴더 공유 해제 (웹서비스 값 받기)
**********************************************************************************************/
GetShare.SuccessDelShare = function (data, status) {
    if (data.d) {
        alert(R_CompleteConfirm);

        ParamArray = {
            'accActive': 1
        };

        CallEncrypt(CallGetParam());
        var goUrl = "/DeskPlusEIP/WebHard/Folder/Tree.aspx?p=" + GetEncryptValue;

        if (GetShare.TreeFrame) GetShare.TreeFrame.location.href = goUrl;

        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else {
        alert(R_ProcessingError);
    }
}
