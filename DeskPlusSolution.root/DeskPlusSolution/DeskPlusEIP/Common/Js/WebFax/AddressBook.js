var AddressBook = {};
AddressBook.Control = null;

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
            AddressBook.Confirm();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            AddressBook.Search();
        });

        //사용자 추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            AddressBook.UserAdd();
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        AddressBook.Init();
    });
});

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-06-07
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
AddressBook.Init = function () {

    $('#phList').setTemplateURL('/DeskPlusEIP/Common/Jt/WebFax/JTAddressBookList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate('');

    AddressBook.Resource();
    AddressBook.UserList();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-02-15
작성목적 : 자원담당자 리스트 초기화 보여주기
**********************************************************************************************/
AddressBook.UserList = function () {

    $("#contentTable").append(
		"<tr id='CMNAdd0'>"
			+ "<td colspan='5' class='center'>"
			+ R_NoContent
			+ "</td>"
		+ "</tr>"
	);

    AddressBook.AddArray(0, 0);
}


/**************************************************
작성자 : shimpark
작성일자 : 2012-06-07
작성목적 : 검색
***************************************************/
AddressBook.Search = function () {
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    AddressBook.GetList();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-06-07
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
AddressBook.Resource = function () {

    $("#r_userName")[0].innerHTML = R_UserName;
    //$("#r_deptName")[0].innerHTML = R_DeptName;
    //$("#r_titleName")[0].innerHTML = R_TitleName;
    $("#r_faxName")[0].innerHTML = R_Fax;

    if ($(".NoListData")[0] != undefined) {
        $(".NoListData")[0].innerHTML = R_NoContent;
    }
}


/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {
    //AddressBook.SelectedNodeID = node.ID; //고유키

    FolderCategoryID = node.GetProperty("CategoryID");
    FolderType = node.GetProperty("Type");

    SearchValue = "";
    AddressBook.GetList();
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
AddressBook.GetList = function () {

    var url = "/DeskPlusEIP/Address/Ws/Mail/AddressSearch.asmx/GetList";

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': FolderCategoryID,
        'types': FolderType,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    var obj = eval("AddressBook.SuccessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
AddressBook.SuccessList = function (data, status) {

    $('#phList').setTemplateURL('/DeskPlusEIP/Common/Jt/WebFax/JTAddressBookList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);

    AddressBook.Resource();
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 전체 체크박스
***************************************************/
AddressBook.Checked = function () {

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
작성목적 : 선택된 값 가져오기(GM102,GM105)
***************************************************/
AddressBook.CheckedValue = function () {
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
AddressBook.AddArray = function (keyID, keyName) {
    UserArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 조직도 넘어온 값 저장소, 차후 비교값
***************************************************/
AddressBook.AddCompareArray = function (keyID, keyName) {
    CompareArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
AddressBook.CompareArrayVal = function (compID, compName) {

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
            //중복되지 않으면 권한값 할당하기
            AddressBook.AddUserRow(compID, compName);
        }
    }
    else {
        response = 0; //다른 값일 경우
        AddressBook.AddUserRow(compID, compName);
    }

    return response;
}



/**************************************************
작성자   : shimpark
작성일자 : 2012-02-15
작성목적 : table 특정 row 삭제하기
***************************************************/
AddressBook.DelRow = function (index, keyName) {
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
        AddressBook.UserList();
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 추가 table row 생성하고 권한값 UserArray[] 배열에 할당하기
***************************************************/
AddressBook.AddUserRow = function (keyID, name) {

    RowMax = RowMax + 1;

    $("#contentTable").append(
		        "<tr id='CMNAdd" + RowMax + "'>"
			        + "<td id='addName_" + name + "'>"
			        + $("#name_" + name).text()
			        + "</td>"
                    + "<td>"
			        + $("#dept_" + name).text()
			        + "</td>"
                    + "<td>"
			        + $("#title_" + name).text()
			        + "</td>"
                    + "<td>"
			        + $("#fax_" + name).text()
			        + "</td>"
			        + "<td><a class='btn' onclick=AddressBook.DelRow(" + RowMax + ",'" + name + "') style='display:inline-block;'>" + R_Del + "</a></td>"
		        + "</tr>"
	        );

    AddressBook.AddArray(keyID, name);
}


/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 사용자 추가 (GM102,GM105)
***************************************************/
AddressBook.UserAdd = function () {

    var selValue = AddressBook.CheckedValue();
    var tmpValue = "";

    if (selValue != "") {

        AddressBook.DelRow(0, 0);

        CompareArray = []; //넘어온 권한값

        for (var i = 0; i < selValue.length; i++) {

            //팩스 번호가 있어야 함.
            if ($("#fax_" + selValue[i]).text() != "") {
                AddressBook.AddCompareArray($("#fax_" + selValue[i]).text(), selValue[i]);
            }
        }

        //2. 넘어온 권한값(CompareArray[]) 배열과 권한값(AuthArray[]) 비교하기
        $.each(CompareArray, function (i, v) {
            AddressBook.CompareArrayVal(v.id, v.name);
        });

    } else {
        alert(R_SelUser);
    }
}


/**************************************************
작성자   : shimpark
작성일자 : 2012-06-08
작성목적 : 확인버튼 
***************************************************/
AddressBook.Confirm = function () {
    var tmpFax = "";

    if (UserArray[0].id == 0) {
        alert(R_SelUser);
        return;
    }

    var tmpName = "";

    for (var i = 0; i < UserArray.length; i++) {
        tmpName = $("#addName_" + UserArray[i].name).text().replace('<', '').replace('>', '');
        tmpFax += tmpName + "|" + UserArray[i].id + "$";
    }

    //이름|팩스$이름|팩스$
    parent.GetFaxUser(tmpFax);

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}
