Add = {}
Add.TreeFrame = parent.frmLeft;
Add.ContentFrame = parent.frmContent;

var BeforeArray = []; //db 에서 넘어 온 값을 배열에 담는다.
var CompareArray = []; //넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
var DelArray = []; //삭제할 값 배열에 담기.

/**************************************************
작성자 : 이지원
작성일자 : 2017-06-19
작성목적 : 초기화 
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            Add.Add();
        });

        //다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#CTBMultiName").val(), "GetMultiName", MultiLang);
        });

        $('#CTBMultiName').attr("disabled", true);

        //검색입력필드 엔터 이벤트 기술
        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (event) {
            Add.KeyEvents();
        });

        //db 에서 권한 값을 권한배열에 담는다.        
        var PublicArrayListStr = PublicArrayList.split(';');
        for (i = 0; i < PublicArrayListStr.length; i++) {
            if (PublicArrayListStr[i].split("$")[1] != undefined) {
                var keyid = PublicArrayListStr[i].split("$")[0] + "$" + PublicArrayListStr[i].split("$")[1];
                var keyName = PublicArrayListStr[i].split("$")[2];
                Add.AddBeforeArray(keyid, keyName);
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

        //사용자 추가
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            Add.AddPublic();
        });
    });
});

/**************************************************
작성자 : 이지원
작성일자 : 2017-06-19
작성목적 : 폴더 추가 
***************************************************/
Add.Add = function () {

    $('#CTBMultiName').attr("disabled", false);
    var validator = new Validator($("#form1"));
    validator.checkRequired("CTBName", R_InputFolderName, true);
    validator.checkRequired("CTBFolderLength", R_InputFolderLength, true);
    validator.checkRequired("CTBFileLength", R_InputFileLength, true);

    validator.checkOnlyNumber("CTBFolderLength", R_InputFolderLengthInt, true);
    validator.checkOnlyNumber("CTBFileLength", R_InputFileLengthInt, true);

    if (!validator.validate()) return;

    var tmpValue = "";

    if (BeforeArray.length > 0) {
        for (var i = 0; i < BeforeArray.length; i++) {
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

        $("#tbAuthValue").val(tmpValue);
    }

    __doPostBack('lbtnConfirm', '');
}

/**************************************************
작성자 : 이지원
작성일자 : 2017-06-19
작성목적 : 추가, 수정 후 트리 새로고침
***************************************************/
Add.GoTree = function () {
    alert(R_CompleteConfirm);
    /*
    ParamArray = {
    'folderID': FolderID,
    'gubun': Gubun
    };

    CallEncrypt(CallGetParam());

    var listPage = "Add.aspx";
    location.href = listPage + "?p=" + GetEncryptValue;

    */

    if (Gubun != "add") {
    ParamArray = {
    'folderID': FolderID,
    'gubun': Gubun
    };

    CallEncrypt(CallGetParam());

    var listPage = "Add.aspx";
    location.href = listPage + "?p=" + GetEncryptValue;
    }
    else {
    ParamArray = {
    'accActive': 0
    };

    CallEncrypt(CallGetParam());

    var goUrl = "/DeskPlusEIP/WebHard/Folder/TreeAdmin.aspx?p=" + GetEncryptValue;
    if (Add.TreeFrame) Add.TreeFrame.location.href = goUrl;
    }
 
}



/**************************************************
작성자 : 이지원
작성일자 : 2017-06-19
작성목적 : 다국이름값 가져와서 설정하기
***************************************************/
function GetMultiName(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {

    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#CTBMultiName").val(multiNames);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-21
작성목적 : 탭 디자인 적용
***************************************************/
Add.SetTab = function (arg) {
    if (arg == "1") {
        $("#tabDivInfo").attr("class", "tab_on");
        $("#divInfo").show();

        $("#tabDivAuth").attr("class", "");
        $("#divAuth").hide();
    } else {
        $("#tabDivInfo").attr("class", "");
        $("#divInfo").hide();

        $("#tabDivAuth").attr("class", "tab_on");
        $("#divAuth").show();
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-21
작성목적 : 엔터키이벤트
***************************************************/
Add.KeyEvents = function () {
    if (event.keyCode == 13) {
        SearchDeptUserList(1);
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-21
작성목적 : 조직도 검색
***************************************************/
function SearchDeptUserList(param) {
    var searchName = $("#DeptUserTreeScript1_tbDeptUser").val().securityText();
    $("#DeptUserTreeScript1_tbDeptUser").val(searchName);

    var params = searchName + "" + param + "" + CompanyID;
    DeptUserTreeScript1_CallBack1.Callback(params);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-21
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
작성자   : 이지원
작성일자 : 2017-06-21
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

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-21
작성목적 : 권한값 table 특정 row 삭제하기
***************************************************/
Add.DelAuthRow = function (index, keyID) {
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
작성자   : 이지원
작성일자 : 2017-06-21
작성목적 : 권한 배열값 저장소; 비교 대상자 배열값
***************************************************/
Add.AddBeforeArray = function (keyID, keyName) {
    BeforeArray.push({
        'id': keyID,
        'name': keyName
    });
}


/**************************************************
작성자   : 이지원
작성일자 : 2017-06-21
작성목적 : 조직도 넘어온 값 저장소, 차후 비교값
***************************************************/
Add.AddCompareArray = function (keyID, keyName) {
    CompareArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-21
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
Add.CompareArrayVal = function (compID, compName) {
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
            Add.AddAuthRow(compID, compName);
        }
    }
    else {
        response = 0; //다른 값일 경우
        Add.AddAuthRow(compID, compName);
    }

    return response;
}


/**************************************************
작성자   : 이지원
작성일자 : 2017-06-21
작성목적 : 추가 table row 생성하고 권한값 BeforeArray[] 배열에 할당하기
***************************************************/
Add.AddAuthRow = function (keyID, name) {
    var rows = $("#contentTable tr");

    var tmpKeyID = keyID.replace('$', '');

    $("#contentTable").append(
		"<tr id='webHardSend" + RowMax + "'>"
			+ "<td class='center'>"
			+ name
			+ "</td>"
            + "<td class='center'><input type='checkbox' id='chxDown" + tmpKeyID + "'></td>"
			+ "<td class='center'><input type='checkbox' id='chxUpload" + tmpKeyID + "'></td>"
            + "<td class='center'><a class='btn' onclick=\"Add.DelAuthRow(" + RowMax + ",'" + keyID + "')\">" + R_Del + "</a></td>"
			//+ "<td class='center'><a onclick=\"Add.DelAuthRow(" + RowMax + ",'" + keyID + "')\"><img src='" + ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif' style='cursor:hand' /></a></td>"
		+ "</tr>"
	);

    RowMax = RowMax + 1;

    Add.AddBeforeArray(keyID, name)
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-12
작성목적 : 사용자 추가 
***************************************************/
Add.AddPublic = function () {

    var selectedID = $("#tvSelectedID").val().trimEnds('|');
    var selectedValue = $("#tvSelectedValue").val().trimEnds('$')

    if (selectedID != "" && selectedValue != "") {

        CompareArray = []; //넘어온 권한값

        var selStr = selectedValue.split('$');
        for (i = 0; i < selStr.length; i++) {
            var selGubun = selStr[i].split('|')[0];
            var selOwnerID = selStr[i].split('|')[1];
            var selName = selStr[i].split('|')[2];

            //1. 넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
            Add.AddCompareArray(selGubun + "$" + selOwnerID, selName);
        }

        //2. 넘어온 권한값(CompareArray[]) 배열과 이미 들어온 값(BeforeArray[]) 비교하기
        $.each(CompareArray, function (i, v) {
            Add.CompareArrayVal(v.id, v.name);
        });

    }
    else {
        alert(R_SelectAdd);
    }
}