var AddPrivate = {};
AddPrivate.Control = null;

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
            AddPrivate.Confirm();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            parent.List.TreeReload();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }


        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            AddPrivate.Search();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                AddPrivate.Search();
            }
        });

        //사용자 추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            AddPrivate.UserAdd();
        });

        //사용자 추가
        $('#cbtnAddGroup').click(function (e) {
            e.preventDefault();
            AddPrivate.GroupAdd();
        });

        //본인회사만
        $('#cbtnListOrg').click(function (e) {
            e.preventDefault();
            AddPrivate.ListOrg();
        });

        //전체회사 조회
        $('#cbtnListOrgAll').click(function (e) {
            e.preventDefault();
            AddPrivate.ListOrgAll();
        });

        //계열사 조회
        $('#cbtnSubsidiary').click(function (e) {
            e.preventDefault();
            AddPrivate.ListSubsidiary();
        });

        $('#cbtnAddPrivate').click(function (e) {
            e.preventDefault();
            AddPrivate.SavePrivate();
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        AddPrivate.Init();
    });
});

AddPrivate.SavePrivate = function () {

    var listAll = '';

    var tmpDept = ""
    var tmpTitle = ""
    var tmpName = ""
    var tmpEmail = ""

    for (var i = 0; i < UserArray.length; i++) {

        tmpDept = $("#addDept_" + UserArray[i].name).text();
        tmpTitle = $("#addTitle_" + UserArray[i].name).text();
        tmpName = $("#addName_" + UserArray[i].name).text();
        tmpEmail = $("#addMail_" + UserArray[i].name).text();

        listAll += tmpDept + ',' + tmpTitle + ',' + tmpName + ',' + tmpEmail + '$';
    }
    //alert(listAll)

    if (tmpName == "") {
        alert(R_NotUserSelect);
        return;
    }

    window.open('about:blank', "open", 'width=400, height=430');
    var frm = document.PrivateForm;
    frm.action = '/DeskPlusEIP/Address/Content/PrivateTree.aspx';
    frm.target = "open";
    frm.method = "post"

    frm.listAll.value = listAll;

    PrivateForm.submit();
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-06-07
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
AddPrivate.Init = function () {
    //AddressSearch.Control = UCAddressTreeScript_tvAddressTree;


    $('#phList').setTemplateURL('/DeskPlusEIP/Address/Jt/Mail/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate('');

    AddPrivate.Resource();
    AddPrivate.UserList();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-02-15
작성목적 : 자원담당자 리스트 초기화 보여주기
**********************************************************************************************/
AddPrivate.UserList = function () {
    $("#contentTable").append(
		"<tr id='CMNAdd0'>"
			+ "<td colspan='6' class='center'>"
			+ R_NoContent
			+ "</td>"
		+ "</tr>"
	);

    AddPrivate.AddArray(0, 0, "");
}

/**************************************************
작성자 : shimpark
작성일자 : 2012-06-07
작성목적 : 검색
***************************************************/
AddPrivate.Search = function () {
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    AddPrivate.GetList();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-06-07
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
AddPrivate.Resource = function () {
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

    AddPrivate.SelectedNodeID = node.ID; //고유키

    FolderCategoryID = node.GetProperty("CategoryID");
    FolderType = node.GetProperty("Type");
    FolderName = node.GetProperty("Text");
    SelCompanyID = node.GetProperty("CompanyID"); //선택한 조직도의 회사 아이디
    SearchValue = "";

    AddPrivate.GetList();
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
AddPrivate.GetList = function () {
    var url = "/DeskPlusEIP/Address/Ws/Mail/AddressSearch.asmx/GetList";
        
    var vUserID = UserID;    

    var data = "{companyID : '" + CompanyID + "',userID : '" + vUserID + "',preLang : '" + PreLang + "',categoryID : '" + FolderCategoryID + "',types : '" + FolderType + "',searchField : '" + SearchField + "',searchValue : '" + SearchValue + "'}";

    var success = eval("AddPrivate.SuccessList");
    AjaxService(url, data, success);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
AddPrivate.SuccessList = function (data, status) {
    $('#phList').setTemplateURL('/DeskPlusEIP/Address/Jt/Content/JTListPrivate.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);

    AddPrivate.Resource();
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 전체 체크박스
***************************************************/
AddPrivate.Checked = function () {
    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);

        setTimeout(function () {
            AddPrivate.UserAdd();
        }, 100);
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
AddPrivate.CheckedValue = function () {
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
AddPrivate.AddArray = function (keyID, keyName, catGroup) {

    UserArray.push({
        'id': keyID,
        'name': keyName,
        //'cType': cType,
        'catGroup': catGroup
    });
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 조직도 넘어온 값 저장소, 차후 비교값
***************************************************/
AddPrivate.AddCompareArray = function (keyID, keyName, catGroup) {
    CompareArray.push({
        'id': keyID,
        'name': keyName,
        //'cType': cType,
        'catGroup': catGroup
    });
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
AddPrivate.CompareArrayVal = function (compID, compName, catGroup) {

    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (UserArray.length > 0) {
        for (var i = 0; i < UserArray.length; i++) {
            if (UserArray[i].id == compID) {
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
                AddPrivate.AddUserRow(compID, compName, catGroup);
            }
            else {
                //중복되지 않으면 권한값 할당하기(그룹)
                AddPrivate.AddGroupRow(compID, compName, catGroup);
            }
        }
    }
    else {
        response = 0; //다른 값일 경우
        if (catGroup == "M") {
            //중복되지 않으면 권한값 할당하기(사용자)
            AddPrivate.AddUserRow(compID, compName, catGroup);
        }
        else {
            //중복되지 않으면 권한값 할당하기(그룹)
            AddPrivate.AddGroupRow(compID, compName, catGroup);
        }
    }

    return response;
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-02-15
작성목적 : table 특정 row 삭제하기
***************************************************/
AddPrivate.DelRow = function (index, keyName) {
    var row = $("#CMNAdd" + index);

    for (var i = 0; i < UserArray.length; i++) {
        if (UserArray[i].name == keyName) {
            UserArray.splice(i, 1);

            //row.fadeOut("slow", function () {
            row.remove();
            //});

            break;
        }
    }

    if ((UserArray.length == 0) && (keyName != 0)) {
        AddPrivate.UserList();
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 추가 table row 생성하고 권한값 UserArray[] 배열에 할당하기
***************************************************/
AddPrivate.AddUserRow = function (keyID, name, catGroup) {

    RowMax = RowMax + 1;

    $("#contentTable").append(
		        "<tr id='CMNAdd" + RowMax + "'>"
                    + "<td id='addDept_" + name.replace('/', '／') + "' class='center'>"
			        + $("#dept_" + name).text()
			        + "</td>"
                    + "<td id='addTitle_" + name.replace('/', '／') + "' class='center'>"
			        + $("#title_" + name).text()
			        + "</td>"
			        + "<td id='addName_" + name.replace('/', '／') + "' class='center'>"
			        + $("#name_" + name).text()
			        + "</td>"
                    + "<td id='addMail_" + name.replace('/', '／') + "'>"
			        + $("#mail_" + name).text()
			        + "</td>"
			        + "<td class='center'><a class='btn' onclick=AddPrivate.DelRow(" + RowMax + ",'" + name + "') style='display:inline-block;'>" + R_Del + "</a></td>"
		        + "</tr>"
	        );

    //UserListAll += $("#dept_" + name).text() + ',' + $("#title_" + name).text() + ',' + $("#name_" + name).text() + ',' + $("#mail_" + name).text() + '$';

    //alert($("#title_" + name).text());

    AddPrivate.AddArray(keyID, name, catGroup);
}

/**************************************************
작성자   : freeice122
작성일자 : 2014-03-12
작성목적 : 추가 table row 생성하고 권한값 UserArray[] 배열에 할당하기
***************************************************/
AddPrivate.AddGroupRow = function (keyID, name, cType, catGroup) {
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
			        + "<td id='addName_" + name.replace('/', '／') + "' class='center'>"
			        + "-"
			        + "</td>"
                    + "<td class='center'>"
			        + "-"
			        + "</td>"
			        + "<td class='center'><a class='btn' onclick=AddPrivate.DelRow(" + RowMax + ",'" + name + "','" + cType + "') style='display:inline-block;'>" + R_Del + "</a></td>"
		        + "</tr>"
	        );

    AddPrivate.AddArray(keyID, name, cType, catGroup);
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 사용자 추가
***************************************************/
AddPrivate.UserAdd = function () {

    var selValue = AddPrivate.CheckedValue();
    var tmpValue = "";

    var catGroup = "M"

    //    var cType = "";
    //    if (document.getElementsByName("cType")[0].checked == true) cType = R_TargetTo;
    //    if (document.getElementsByName("cType")[1].checked == true) cType = R_TargetCC;
    //    if (document.getElementsByName("cType")[2].checked == true) cType = R_TargetBCC;

    if (selValue != "") {
        AddPrivate.DelRow(0, 0);

        CompareArray = []; //넘어온 권한값

        for (var i = 0; i < selValue.length; i++) {
            //1. 넘어온 권한값 배열에 할당하기
            //AddPrivate.AddCompareArray($("#mail_" + selValue[i]).text(), selValue[i], cType, catGroup);
            AddPrivate.AddCompareArray($("#mail_" + selValue[i]).text(), selValue[i], catGroup);
        }

        //2. 넘어온 권한값(CompareArray[]) 배열과 권한값(AuthArray[]) 비교하기
        $.each(CompareArray, function (i, v) {
            //AddPrivate.CompareArrayVal(v.id, v.name, cType, catGroup);
            AddPrivate.CompareArrayVal(v.id, v.name, catGroup);
        });

    }
}

/**************************************************
작성자   : freeice122
작성일자 : 2014-03-12
작성목적 : 그룹 추가
***************************************************/
AddPrivate.GroupAdd = function () {
    var selValue = AddPrivate.CheckedValue();
    var tmpValue = "";

    var catGroup = "G"

    var cType = "";
    if (document.getElementsByName("cType")[0].checked == true) cType = R_TargetTo;
    if (document.getElementsByName("cType")[1].checked == true) cType = R_TargetCC;
    if (document.getElementsByName("cType")[2].checked == true) cType = R_TargetBCC;

    if (FolderCategoryID != "" && FolderCategoryID != "0") {
        AddPrivate.DelRow(0, 0, "");

        CompareArray = []; //넘어온 권한값

        if (FolderType == "TG" && FolderCategoryID == "1") {
            alert(R_NoSelGroup);
            return;
        }

        if (FolderType == "DG" && FolderCategoryID == "2") {
            alert(R_NoSelGroup);
            return;
        }

        //1. 넘어온 권한값 배열에 할당하기
        AddPrivate.AddCompareArray(FolderType + "|" + FolderCategoryID, FolderName, cType, catGroup);

        //2. 넘어온 권한값(CompareArray[]) 배열과 권한값(AuthArray[]) 비교하기
        $.each(CompareArray, function (i, v) {
            AddPrivate.CompareArrayVal(v.id, v.name, cType, catGroup);
        });
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 확인버튼
***************************************************/
AddPrivate.Confirm = function () {
    var tmpMailTo = "";
    var tmpMailCC = "";
    var tmpMailBCC = "";
    var arrID = "";

    if (UserArray[0].id == 0) { alert(R_SelUser); return; }

    for (var i = 0; i < UserArray.length; i++) {
        var tmpName = $("#addName_" + UserArray[i].name.replace('/', '／')).text().replace('<', '').replace('>', '');
        var tmpName = $("#addName_" + UserArray[i].name.replace('/', '／')).text();

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
                            tmpMailTo += ";#" + keyID + " " + tmpName + "(" + R_CompanyAddress + ")";
                            break;
                        case R_TargetCC:
                            tmpMailCC += ";#" + keyID + " " + tmpName + "(" + R_CompanyAddress + ")";
                            break;
                        case R_TargetBCC:
                            tmpMailBCC += ";#" + keyID + " " + tmpName + "(" + R_CompanyAddress + ")";
                            break;
                    }
                    break;
                case "CG": //공용주소록
                    switch (UserArray[i].cType) {
                        case R_TargetTo:
                            tmpMailTo += ";^" + keyID + " " + tmpName + "(" + R_CommonAddress + ")";
                            break;
                        case R_TargetCC:
                            tmpMailCC += ";^" + keyID + " " + tmpName + "(" + R_CommonAddress + ")";
                            break;
                        case R_TargetBCC:
                            tmpMailBCC += ";^" + keyID + " " + tmpName + "(" + R_CommonAddress + ")";
                            break;
                    }
                    break;
                case "PG": //개인 주소록
                    switch (UserArray[i].cType) {
                        case R_TargetTo:
                            tmpMailTo += ";&" + keyID + " " + tmpName + "(" + R_PrivateAddress + ")";
                            break;
                        case R_TargetCC:
                            tmpMailCC += ";&" + keyID + " " + tmpName + "(" + R_PrivateAddress + ")";
                            break;
                        case R_TargetBCC:
                            tmpMailBCC += ";&" + keyID + " " + tmpName + "(" + R_PrivateAddress + ")";
                            break;
                    }
                    break;
                case "TG": //직급
                    switch (UserArray[i].cType) {
                        case R_TargetTo:
                            tmpMailTo += ";%" + keyID + " " + tmpName + "(" + R_Title + ")";
                            break;
                        case R_TargetCC:
                            tmpMailCC += ";%" + keyID + " " + tmpName + "(" + R_Title + ")";
                            break;
                        case R_TargetBCC:
                            tmpMailBCC += ";%" + keyID + " " + tmpName + "(" + R_Title + ")";
                            break;
                    }
                    break;
                case "DG": //직책
                    switch (UserArray[i].cType) {
                        case R_TargetTo:
                            tmpMailTo += ";!" + keyID + " " + tmpName + "(" + R_Duty + ")";
                            break;
                        case R_TargetCC:
                            tmpMailCC += ";!" + keyID + " " + tmpName + "(" + R_Duty + ")";
                            break;
                        case R_TargetBCC:
                            tmpMailBCC += ";!" + keyID + " " + tmpName + "(" + R_Duty + ")";
                            break;
                    }
                    break;
            }
        }
    }

    if (MailProduct.toUpperCase().indexOf("DEEP") < 0) {
        AddPrivate.ConfirmZetta(tmpMailTo, tmpMailCC, tmpMailBCC);
    } else {
        AddPrivate.ConfirmDeep(tmpMailTo, tmpMailCC, tmpMailBCC);
    }
}
//제타
AddPrivate.ConfirmZetta = function (_tmpMailTo, _tmpMailCC, _tmpMailBCC) {
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
AddPrivate.ConfirmDeep = function (_tmpMailTo, _tmpMailCC, _tmpMailBCC) {

    deepForm.to.value = _tmpMailTo;
    deepForm.cc.value = _tmpMailCC;
    deepForm.bcc.value = _tmpMailBCC;

    //    alert("To : " + deepForm.to.value);
    //    alert("CC : " + deepForm.cc.value);
    //    alert("BCC : " + deepForm.bcc.value);

    deepForm.submit();
    //    self.close();
}

//자사
AddPrivate.ListOrg = function () {
    $("#dvAddressMailTree").show();
    $("#dvAddressMailTreeAll").hide();
    $("#dvAddressMailTreeSubsidiary").hide();
    SearchByDeptAll = "0";
}
//전체
AddPrivate.ListOrgAll = function () {
    $("#dvAddressMailTree").hide();
    $("#dvAddressMailTreeAll").show();
    $("#dvAddressMailTreeSubsidiary").hide();
    SearchByDeptAll = "1";
}
//계열사
AddPrivate.ListSubsidiary = function () {
    $("#dvAddressMailTree").hide();
    $("#dvAddressMailTreeAll").hide();
    $("#dvAddressMailTreeSubsidiary").show();
    SearchByDeptAll = "2";
}