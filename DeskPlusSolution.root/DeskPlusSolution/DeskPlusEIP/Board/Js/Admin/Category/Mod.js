/// <reference path="../../../../Html/Js/jQuery/jquery.js" />

Mod = {}

var AuthArray = []; //db 에서 권한 값을 권한배열에 담는다.
var CompareArray = []; //넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
var DelArray = []; //삭제할 값 배열에 담기.

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

//        //자동화완성 autocomplete - written by shimpark
//        $("#DeptUserTreeScript1_tbDeptUser").autocomplete({
//            source: function (request, response) {
//                $.ajax({
//                    url: "/DeskPlusEIP/Account/Ws/Common/GetUserDept.asmx/GetDeptUserSearch",
//                    data: "{ 'companyID': '" + CompanyID + "','gubun': '','searchName': '" + request.term + "' }",
//                    dataType: "json",
//                    type: "POST",
//                    contentType: "application/json; charset=utf-8",
//                    success: function (data) {
//                        //Callback method when the precess is success
//                        response($.map(data.d, function (item) {
//                            return {
//                                label: item.Name,
//                                value: item.Values
//                            }
//                        }))
//                    },
//                    error: function (XMLHttpRequest, textStatus, errorThrown) {
//                        alert("Communication Error");
//                    }
//                });
//            },
//            focus: function (event, ui) {
//                // prevent autocomplete from updating the textbox
//                event.preventDefault();
//                // manually update the textbox
//                $(this).val(ui.item.value);
//            },
//            select: function (event, ui) {
//                // prevent autocomplete from updating the textbox
//                event.preventDefault();
//                // manually update the textbox and hidden field
//                $(this).val(ui.item.value);
//                $("#DeptUserTreeScript1_tbDeptUser").val(ui.item.value);
//            },
//            minLength: 1
//        }).autocomplete("widget").addClass("fixed-height");

        //검색입력필드 엔터 이벤트 기술
        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (event) {
            Mod.KeyEvents();
        });

        //db 에서 권한 값을 권한배열에 담는다.
        var AuthArrayListStr = AuthArrayList.split(';');
        for (i = 0; i < AuthArrayListStr.length; i++) {
            if (AuthArrayListStr[i].split("$")[1] != undefined) {
                var keyid = AuthArrayListStr[i].split("$")[0] + "$" + AuthArrayListStr[i].split("$")[1];
                var keyName = AuthArrayListStr[i].split("$")[2];
                Mod.AddAuthArray(keyid, keyName);
            }
        }

        //일괄 권한 설정
        $('#ddlAllAuthManage').change(function () {
            Mod.ApplyAllAuthManage();
        });

        //전사 권한 설정
        $('#cbEnterpriceAuthManage').change(function () {
            Mod.ApplyEnterpriceAuthManage();
        });

        //상위 폴더 변경
        $('#CbtnOpenFolder').click(function (e) {
            e.preventDefault();
            Mod.SetParentFolder();
        });

        //폴더 순서 변경
        $('#CbtnMove').click(function (e) {
            e.preventDefault();
            Mod.MoveUpdown();
        });

        //수정
        $('#CbtnMod').click(function (e) {
            e.preventDefault();
            Mod.Process();
        });

        //추가
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            Mod.AddAuth();
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

        //말머리
        $('#CbtnHeaders').click(function (e) {
            e.preventDefault();
            Mod.Headers();
        });

        //메뉴생성
        $('#CbtnCreateMenu').click(function (e) {
            e.preventDefault();
            Mod.CreateMenu();
        });

        //폴더명(다국이름)
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMultiLang").val(), "GetMultiName", MultiLang);
        });

        //스크린샷 생성
        $('#CbtnScreenShot').click(function (e) {
            e.preventDefault();
            Mod.CreateScreenshot();
        });

        $('#ctbMultiLang').attr("disabled", true);
    });
});

/**************************************************
작성자   : shimpark
작성목적 : 스크린샷 생성
***************************************************/
Mod.CreateScreenshot = function () {
    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CatID
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Board/Admin/Category/CreateThum.aspx?p=" + GetEncryptValue;

    PopupWin(400, 120, url, "-500", "", true);
}

/**************************************************
작성자   : shimpark
작성목적 : 메뉴 생성하기
***************************************************/
Mod.CreateMenu = function () {
    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CatID,
        'modules': 'Board'
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/EIP/Portal/AddMenu.aspx?p=" + GetEncryptValue;

    PopupWin(400, 400, url, "-500", "", true);
}

/**************************************************
작성자   : shimpark
작성목적 : 폴더순서 변경하기
***************************************************/
Mod.MoveUpdown = function () {
    ParamArray = {
        'menuCatID': MenuCatID,
        'catID': CatID
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Board/Admin/Category/MoveUpdown.aspx?p=" + GetEncryptValue;

    PopupWin(400, 395, url, "-500", "", true);
}

/**************************************************
작성자   : shimpark
작성목적 : 상위폴더변경
***************************************************/
Mod.SetParentFolder = function () {
    ParamArray = {
        'menuCatID': MenuCatID,
        'catID': CatID
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Board/Admin/Category/Move.aspx?p=" + GetEncryptValue;

    PopupWin(400, 395, url, "-500", "", true);
}

/**************************************************
작성자   : shimpark
작성목적 : 권한추가 버튼
***************************************************/
Mod.AddAuth = function () {
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

            //1. 넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
            Mod.AddCompareArray(selGubun + "$" + selOwnerID, selName);
        }

        //2. 넘어온 권한값(CompareArray[]) 배열과 권한값(AuthArray[]) 비교하기
        $.each(CompareArray, function (i, v) {
            Mod.CompareArrayVal(v.id, v.name);
        });
    }
    else {
        alert(R_SettingAuthority);
    }
}

/**************************************************
작성자   : shimpark
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
Mod.CompareArrayVal = function (compID, compName) {
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
            Mod.AddAuthRow(compID, compName);
        }
    }
    else {
        response = 0; //다른 값일 경우
        Mod.AddAuthRow(compID, compName);
    }

    return response;
}

/**************************************************
작성자   : shimpark
작성목적 : 추가 table row 생성하고 권한값 AuthArray[] 배열에 할당하기
***************************************************/
Mod.AddAuthRow = function (keyID, name) {
    var rows = $("#contentTable tr");
    RowMax = RowMax + 1;

    var selStr = keyID.split('$');

    $("#contentTable").append(
		"<tr id='boardAuth" + RowMax + "'>"
			+ "<td class='center'>"
			+ unescape(name)
			+ "</td>"
            + "<td class='center'>"
			+ "<select name='ddlListType' id='ddlListType" + RowMax + "' class='searchselect form-control'>"
			+ "	<option selected='selected' value='1$" + keyID + "'>" + R_Read + "</option>"
			+ "	<option value='2$" + keyID + "'>" + R_Write + "</option>"
			+ "	<option value='3$" + keyID + "'>" + R_ReadReply + "</option>"
            + "	<option value='4$" + keyID + "'>" + R_WriteReply + "</option>"
			+ "</select></td>"
            + "<td class='center'><a class=btn onclick=\"Mod.DelAuthRow(" + RowMax + ",'" + keyID + "','" + name + "')\">" + R_Del + "</a></td>"
			//+ "<td class='center'><a onclick=\"Mod.DelAuthRow(" + RowMax + ",'" + keyID + "','" + name + "')\" style='display:inline-block;'><img src='" + ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif' /></a></td>"
		+ "</tr>"
	);

    Mod.AddAuthArray(keyID, name)
}

/**************************************************
작성자   : shimpark
작성목적 : 권한값 table 특정 row 삭제하기
***************************************************/
Mod.DelAuthRow = function (index, keyID, keyName) {
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
작성자   : shimpark
작성목적 : 권한 배열값 저장소; 비교 대상자 배열값
***************************************************/
Mod.AddAuthArray = function (keyID, keyName) {
    AuthArray.push({
        'id': keyID,
        'name': escape(keyName)
    });
}

/**************************************************
작성자   : shimpark
작성목적 : 조직도 넘어온 값 저장소, 차후 비교값
***************************************************/
Mod.AddCompareArray = function (keyID, keyName) {
    CompareArray.push({
        'id': keyID,
        'name': escape(keyName)
    });
}

/**************************************************
작성자   : shimpark
작성목적 : 일괄 권한 설정 기술
***************************************************/
Mod.ApplyAllAuthManage = function () {
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
Mod.ApplyEnterpriceAuthManage = function () {
    if ($("#cbEnterpriceAuthManage").is(":checked")) {
        CompareArray = [];

        //1. 넘어온 권한값 배열에 할당하기(G$1,영업부)
        Mod.AddCompareArray("G$0", R_Whole);

        //2. 넘어온 권한값 배열과 권한값 비교하기
        $.each(CompareArray, function (i, v) {
            Mod.CompareArrayVal(v.id, v.name);
        });

        /*
        if (window.confirm("설정한 권한은 제외되고, 전체사용자에게 권한을 재설정하시겠습니까?")) {
        var row = $("#contentTable tr");
        var index = row.length;

        row.fadeOut("fast", function () {
        row.remove();
        });

        $("#contentTable").append(
        "<tr><th class='table_title'>명칭</th>"
        + "<th class='table_100'>권한</th>"
        + "<th class='table_lock'>삭제</th>"
        + "</tr>");

        AuthArray = [];
        CompareArray = [];
        DelArray = [];

        $("#tvSelectedID").val("");
        $("#tvSelectedValue").val("");
        }
        else {
        $("#cbEnterpriceAuthManage").attr("checked", false);
        }

        */
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2011-01-26
작성목적 : 탭 디자인 적용
***************************************************/
Mod.SetTab = function (arg) {
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
작성자   : 심재운
작성일자 : 2011-01-26
작성목적 : 처리하기
***************************************************/
Mod.Process = function () {
    $('#ctbMultiLang').attr("disabled", false);
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbTitle", R_FolderName, true);
    if (!validator.validate()) return;

    //권한값 추출해 오기
    var authVal = "";
    $("#contentTable select option:selected").each(function () {
        authVal += $(this).val() + ",";
    });

    //권한값$구분$키값$이름
    $("#tbAuthValue").val(authVal.trimEnds(","));

    if (CatID == "0") {
        __doPostBack("LBTNProcess", "Register");
    }
    else {
        __doPostBack("LBTNProcess", "Modify");
    }
}

/**************************************************
추가, 수정 후 트리 새로고침
***************************************************/
Mod.GoTree = function () {
    //파라미터값 배열에 담기
    ParamArray = {
        'menuCatID': MenuCatID,
        'catID': CatID,
        'optionUseDept': OptionUseDept
    };
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    parent.frmLeft.location.href = "/DeskplusEIP/Board/Admin/Menu/Menu.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 엔터키이벤트
***************************************************/
Mod.KeyEvents = function () {
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

    var params = searchName + "" + param + "" + CompanyID;
    DeptUserTreeScript1_CallBack1.Callback(params);
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
    var selCompanyID = node.GetProperty("CompanyID"); //회사아이디



    if (selCompanyID != CompanyID) {
        alert(R_NoAddOtherCompany);
        return;
    }

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
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : listbox 선택한 값 가져오기
***************************************************/
function LbxDeptUser_OnChange() {
    $("#tvSelectedID").empty();
    $("#tvSelectedValue").empty();

    var selIDs = "";
    var selValues = "";
    var isOtherCompany = false;

    $("#" + LBDeptUser + " :selected").each(function (i, selected) {
        var rtnValue = $(selected).val();
        var rtnText = $(selected).text();

        var colSplit = rtnValue.split("^"); //nodeID
        //var rowSplit = colSplit[1].split("$"); //nodeValue
        var selID = colSplit[0]; //nodeID

        var selCompanyID = rtnValue.split("|")[14]; //회사아이디

        if (selCompanyID == CompanyID) {

            selIDs = selIDs.replace(selID + "|", "");
            selIDs += selID + "|";

            var selValue = colSplit[1]; //nodeValue
            selValues = selValues.replace(selValue + "$", "");
            selValues += selValue + "$";
        } else {
            //타사 사용자 차단하기
            isOtherCompany = true;
        }
    });

    // type(G) | deptID | deptName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName
    // type(M) | userID | userName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName

    $("#tvSelectedID").val(selIDs);
    $("#tvSelectedValue").val(selValues);

    //타사 사용자 차단하기
    if (isOtherCompany == true) {
        alert(R_NoAddOtherCompany);
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 다국이름값 가져와서 설정하기
***************************************************/
function GetMultiName(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {
    name_base = decodeURIComponent(name_base);
    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#ctbMultiLang").val(multiNames);
}

/**************************************************
작성자   : shimpark
작성목적 : 말머리 추가
***************************************************/
Mod.Headers = function () {
    ParamArray = {
        'catID': CatID
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Board/Admin/Category/Headers.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "-500", "", true);
}