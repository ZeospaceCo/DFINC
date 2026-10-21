var AddressSearch = {};
AddressSearch.Control = null;

var UserArray = []; //db 에서 권한 값을 권한배열에 담는다.
var CompareArray = []; //넘어온 권한값 배열에 할당하기

/**************************************************
작성자 : shimpark
작성일자 : 2012-06-07
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //추가
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            AddressSearch.Confirm();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            self.close();
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            AddressSearch.Search();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                AddressSearch.Search();
            }
        });

        //사용자 추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            AddressSearch.UserAdd();
        });

        //사용자 추가
        $('#cbtnAddGroup').click(function (e) {
            e.preventDefault();
            AddressSearch.GroupAdd();
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        AddressSearch.Init();
    });
});

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-06-07
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
AddressSearch.Init = function () {
    //AddressSearch.Control = UCAddressTreeScript_tvAddressTree;

    $('input:radio[name=cType]:input[value=' + Gubun + ']').attr("checked", true);
    $('#phList').setTemplateURL('/DeskPlusEIP/Address/Jt/Mail/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate('');

    AddressSearch.Resource();
    AddressSearch.UserList();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-02-15
작성목적 : 자원담당자 리스트 초기화 보여주기
**********************************************************************************************/
AddressSearch.UserList = function () {
    $("#contentTable").append(
		"<tr id='CMNAdd0'>"
			+ "<td colspan='6' class='center'>"
			+ R_NoContent
			+ "</td>"
		+ "</tr>"
	);

    AddressSearch.AddArray(0, 0, "");
}

/**************************************************
작성자 : shimpark
작성일자 : 2012-06-07
작성목적 : 검색
***************************************************/
AddressSearch.Search = function () {
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    AddressSearch.GetList();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-06-07
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
AddressSearch.Resource = function () {
    $("#r_deptName")[0].innerHTML = R_DeptName;
    $("#r_titleName")[0].innerHTML = R_TitleName;
    $("#r_userName")[0].innerHTML = R_UserName;
    $("#r_email")[0].innerHTML = R_Email;

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {
    AddressSearch.SelectedNodeID = node.ID; //고유키

    FolderCategoryID = node.GetProperty("CategoryID");
    FolderType = node.GetProperty("Type");
    FolderName = node.GetProperty("Text");
    SearchValue = "";
    AddressSearch.GetList();
}

/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
AddressSearch.GetList = function () {
    var url = "/DeskPlusEIP/Address/Ws/Mail/AddressSearch.asmx/GetList";

    var data = "{companyID : '" + CompanyID + "',userID : '" + UserID + "',preLang : '" + PreLang + "',categoryID : '" + FolderCategoryID + "',types : '" + FolderType + "',searchField : '" + SearchField + "',searchValue : '" + SearchValue + "'}";

    var success = eval("AddressSearch.SuccessList");
    AjaxService(url, data, success);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
AddressSearch.SuccessList = function (data, status) {
    $('#phList').setTemplateURL('/DeskPlusEIP/Address/Jt/Mail/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);

    AddressSearch.Resource();
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 전체 체크박스
***************************************************/
AddressSearch.Checked = function () {
    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 선택된 값 가져오기
***************************************************/
AddressSearch.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 배열값 저장소; 비교 대상자 배열값
***************************************************/
AddressSearch.AddArray = function (keyID, keyName, cType, catGroup) {
    UserArray.push({
        'id': keyID,
        'name': keyName,
        'cType': cType,
        'catGroup': catGroup
    });
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 조직도 넘어온 값 저장소, 차후 비교값
***************************************************/
AddressSearch.AddCompareArray = function (keyID, keyName, cType, catGroup) {
    CompareArray.push({
        'id': keyID,
        'name': keyName,
        'cType': cType,
        'catGroup': catGroup
    });
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
AddressSearch.CompareArrayVal = function (compID, compName, cType, catGroup) {
    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (UserArray.length > 0) {
        for (var i = 0; i < UserArray.length; i++) {
            if (UserArray[i].id == compID && UserArray[i].cType == cType) {
                response = 1; //동일한 값이 존재할 경우
                break;
            }
            else {
                response = 0; //다른 값일 경우
            }
        }

        if (response == 0) {
            if (catGroup == "M") {
                //중복되지 않으면 권한값 할당하기(사용자)
                AddressSearch.AddUserRow(compID, compName, cType, catGroup);
            }
            else {
                //중복되지 않으면 권한값 할당하기(그룹)
                AddressSearch.AddGroupRow(compID, compName, cType, catGroup);
            }
        }
    }
    else {
        response = 0; //다른 값일 경우
        if (catGroup == "M") {
            //중복되지 않으면 권한값 할당하기(사용자)
            AddressSearch.AddUserRow(compID, compName, cType, catGroup);
        }
        else {
            //중복되지 않으면 권한값 할당하기(그룹)
            AddressSearch.AddGroupRow(compID, compName, cType, catGroup);
        }
    }

    return response;
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-02-15
작성목적 : table 특정 row 삭제하기
***************************************************/
AddressSearch.DelRow = function (index, keyName, cType) {
    var row = $("#CMNAdd" + index);

    for (var i = 0; i < UserArray.length; i++) {
        if (UserArray[i].name == keyName && UserArray[i].cType == cType) {
            UserArray.splice(i, 1);

            //row.fadeOut("slow", function () {
            row.remove();
            //});

            break;
        }
    }

    if ((UserArray.length == 0) && (keyName != 0)) {
        AddressSearch.UserList();
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 추가 table row 생성하고 권한값 UserArray[] 배열에 할당하기
***************************************************/
AddressSearch.AddUserRow = function (keyID, name, cType, catGroup) {
    RowMax = RowMax + 1;

    $("#contentTable").append(
		        "<tr id='CMNAdd" + RowMax + "'>"
			        + "<td class='center'>"
			        + cType //$("#cType2").val()    //$("#cType1").val()
			        + "</td>"
                    + "<td class='center'>"
			        + $("#dept_" + name).text()
			        + "</td>"
                    + "<td class='center'>"
			        + $("#title_" + name).text()
			        + "</td>"
			        + "<td id='addName_" + name + "' class='center'>"
			        + $("#name_" + name).text()
			        + "</td>"
                    + "<td>"
			        + $("#mail_" + name).text()
			        + "</td>"
			        + "<td class='center'><a class='btn' onclick=AddressSearch.DelRow(" + RowMax + ",'" + name + "','" + cType + "') style='display:inline-block;'>" + R_Del + "</a></td>"
		        + "</tr>"
	        );

    AddressSearch.AddArray(keyID, name, cType, catGroup);
}

/**************************************************
작성자   : freeice122
작성일자 : 2014-03-12
작성목적 : 추가 table row 생성하고 권한값 UserArray[] 배열에 할당하기
***************************************************/
AddressSearch.AddGroupRow = function (keyID, name, cType, catGroup) {
    RowMax = RowMax + 1;

    $("#contentTable").append(
		        "<tr id='CMNAdd" + RowMax + "'>"
			        + "<td class='center'>"
			        + cType //$("#cType2").val()    //$("#cType1").val()
			        + "</td>"
                    + "<td class='center' style='font-weight:bold;'>"
			        + name
			        + "</td>"
                    + "<td class='center'>"
			        + "-"
			        + "</td>"
			        + "<td id='addName_" + name + "' class='center'>"
			        + "-"
			        + "</td>"
                    + "<td class='center'>"
			        + "-"
			        + "</td>"
			        + "<td class='center'><a class='btn' onclick=AddressSearch.DelRow(" + RowMax + ",'" + name + "','" + cType + "') style='display:inline-block;'>" + R_Del + "</a></td>"
		        + "</tr>"
	        );

    AddressSearch.AddArray(keyID, name, cType, catGroup);
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 사용자 추가
***************************************************/
AddressSearch.UserAdd = function () {
    var selValue = AddressSearch.CheckedValue();
    var tmpValue = "";

    var catGroup = "M"

    var cType = "";
    if (document.getElementsByName("cType")[0].checked == true) cType = R_TargetTo;
    if (document.getElementsByName("cType")[1].checked == true) cType = R_TargetCC;
    if (document.getElementsByName("cType")[2].checked == true) cType = R_TargetBCC;

    if (selValue != "") {
        AddressSearch.DelRow(0, 0, "");

        CompareArray = []; //넘어온 권한값

        for (var i = 0; i < selValue.length; i++) {
            //1. 넘어온 권한값 배열에 할당하기
            AddressSearch.AddCompareArray($("#mail_" + selValue[i]).text(), selValue[i], cType, catGroup);
        }

        //2. 넘어온 권한값(CompareArray[]) 배열과 권한값(AuthArray[]) 비교하기
        $.each(CompareArray, function (i, v) {
            AddressSearch.CompareArrayVal(v.id, v.name, cType, catGroup);
        });
    } else {
        alert(R_SelUser);
    }
}

/**************************************************
작성자   : freeice122
작성일자 : 2014-03-12
작성목적 : 그룹 추가
***************************************************/
AddressSearch.GroupAdd = function () {
    var selValue = AddressSearch.CheckedValue();
    var tmpValue = "";

    var catGroup = "G"

    var cType = "";
    if (document.getElementsByName("cType")[0].checked == true) cType = R_TargetTo;
    if (document.getElementsByName("cType")[1].checked == true) cType = R_TargetCC;
    if (document.getElementsByName("cType")[2].checked == true) cType = R_TargetBCC;

    if (FolderCategoryID != "" && FolderCategoryID != "0") {
        AddressSearch.DelRow(0, 0, "");

        CompareArray = []; //넘어온 권한값

        if (FolderType == "TG" && FolderCategoryID == "1") {
            alert("지정할 수 없는 그룹입니다.");
            return;
        }

        if (FolderType == "DG" && FolderCategoryID == "2") {
            alert("지정할 수 없는 그룹입니다.");
            return;
        }

        //1. 넘어온 권한값 배열에 할당하기
        AddressSearch.AddCompareArray(FolderType + "|" + FolderCategoryID, FolderName, cType, catGroup);

        //2. 넘어온 권한값(CompareArray[]) 배열과 권한값(AuthArray[]) 비교하기
        $.each(CompareArray, function (i, v) {
            AddressSearch.CompareArrayVal(v.id, v.name, cType, catGroup);
        });
    } else {
        alert("지정할 수 없는 그룹입니다.");
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 확인버튼
***************************************************/
AddressSearch.Confirm = function () {
    var tmpMailTo = "";
    var tmpMailCC = "";
    var tmpMailBCC = "";
    var arrID = "";

    if (UserArray[0].id == 0) { alert(R_SelUser); return; }

    for (var i = 0; i < UserArray.length; i++) {
        var tmpName = $("#addName_" + UserArray[i].name).text().replace('<', '').replace('>', '');
        var tmpName = $("#addName_" + UserArray[i].name).text();

        if (UserArray[i].catGroup == "M") {
            //사용자 선택
            switch (UserArray[i].cType) {
                case R_TargetTo:
                    tmpMailTo += ";\"" + tmpName + "\" <" + UserArray[i].id + ">";
                    break;
                case R_TargetCC:
                    tmpMailCC += ";\"" + tmpName + "\" <" + UserArray[i].id + ">";
                    break;
                case R_TargetBCC:
                    tmpMailBCC += ";\"" + tmpName + "\" <" + UserArray[i].id + ">";
                    break;
            }
        } else {
            //부서 선택
            arrID = UserArray[i].id.split("|");
            keyGroup = arrID[0];
            keyID = arrID[1];
            var tmpName = UserArray[i].name;

            tmpName = tmpName.replace(" ", "");

            switch (keyGroup) {
                case "GG": //회사주소록
                    switch (UserArray[i].cType) {
                        case R_TargetTo:
                            tmpMailTo += ";#" + keyID + " " + tmpName + "(회사주소록)";
                            break;
                        case R_TargetCC:
                            tmpMailCC += ";#" + keyID + " " + tmpName + "(회사주소록)";
                            break;
                        case R_TargetBCC:
                            tmpMailBCC += ";#" + keyID + " " + tmpName + "(회사주소록)";
                            break;
                    }
                    break;
                case "CG": //공용주소록
                    switch (UserArray[i].cType) {
                        case R_TargetTo:
                            tmpMailTo += ";^" + keyID + " " + tmpName + "(공용주소록)";
                            break;
                        case R_TargetCC:
                            tmpMailCC += ";^" + keyID + " " + tmpName + "(공용주소록)";
                            break;
                        case R_TargetBCC:
                            tmpMailBCC += ";^" + keyID + " " + tmpName + "(공용주소록)";
                            break;
                    }
                    break;
                case "PG": //개인 주소록
                    switch (UserArray[i].cType) {
                        case R_TargetTo:
                            tmpMailTo += ";&" + keyID + " " + tmpName + "(개인주소록)";
                            break;
                        case R_TargetCC:
                            tmpMailCC += ";&" + keyID + " " + tmpName + "(개인주소록)";
                            break;
                        case R_TargetBCC:
                            tmpMailBCC += ";&" + keyID + " " + tmpName + "(개인주소록)";
                            break;
                    }
                    break;
                case "TG": //직급
                    switch (UserArray[i].cType) {
                        case R_TargetTo:
                            tmpMailTo += ";%" + keyID + " " + tmpName + "(직급)";
                            break;
                        case R_TargetCC:
                            tmpMailCC += ";%" + keyID + " " + tmpName + "(직급)";
                            break;
                        case R_TargetBCC:
                            tmpMailBCC += ";%" + keyID + " " + tmpName + "(직급)";
                            break;
                    }
                    break;
                case "DG": //직책
                    switch (UserArray[i].cType) {
                        case R_TargetTo:
                            tmpMailTo += ";!" + keyID + " " + tmpName + "(직책)";
                            break;
                        case R_TargetCC:
                            tmpMailCC += ";!" + keyID + " " + tmpName + "(직책)";
                            break;
                        case R_TargetBCC:
                            tmpMailBCC += ";!" + keyID + " " + tmpName + "(직책)";
                            break;
                    }
                    break;
            }
        }
    }

    if (MailProduct.toUpperCase().indexOf("DEEP") < 0) {
        AddressSearch.ConfirmZetta(tmpMailTo, tmpMailCC, tmpMailBCC);
    } else {
        AddressSearch.ConfirmDeep(tmpMailTo, tmpMailCC, tmpMailBCC);
    }
}
//제타
AddressSearch.ConfirmZetta = function (_tmpMailTo, _tmpMailCC, _tmpMailBCC) {
    zettaForm.to.value = _tmpMailTo;
    zettaForm.cc.value = _tmpMailCC;
    zettaForm.bcc.value = _tmpMailBCC;

    //    alert(zettaForm.to.value);
    //    alert(zettaForm.cc.value);
    //    alert(zettaForm.bcc.value);
    //    return;

    zettaForm.close.value = "1";
    zettaForm.submit();

    self.close();
}
//DEEP
AddressSearch.ConfirmDeep = function (_tmpMailTo, _tmpMailCC, _tmpMailBCC) {
    deepForm.to.value = _tmpMailTo;
    deepForm.cc.value = _tmpMailCC;
    deepForm.bcc.value = _tmpMailBCC;

    //    alert("To : " + deepForm.to.value);
    //    alert("CC : " + deepForm.cc.value);
    //    alert("BCC : " + deepForm.bcc.value);

    deepForm.submit();
    //    self.close();
}