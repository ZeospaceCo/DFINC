Add = {}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        if (parent.SelectUser.length > 0) {
            var arSel = parent.SelectUser.split('|');
            if (arSel.length > 0) {
                UserID = arSel[1];
                $("#txtUser").val(arSel[0]);
                Add.GetShareList();
            }
        }

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
            Add.KeyEvents();
        });


        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            parent.List.Search();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();

            parent.List.Search();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //공유 대상 선택
        $('#CbtnAddTarget').click(function (e) {
            e.preventDefault();
            Add.AddTarget();
        });

        //공유 추가
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            Add.AddShare();
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

        //전체 삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            Add.AllDell();
        });



    });
});




/**************************************************
작성자   : 
작성일자 : 
작성목적 : 공유대상 전체 삭제
***************************************************/
Add.AllDell = function () {
    if (UserID == 0) {
        alert(NoneSelUser); //선택된 유저가 없습니다. NoneSelUser
        return;
    }

    if (confirm(allDelete)) {//allDelete
        Add.DelUserList("ALL");
    }

}
/**************************************************
작성자   : 
작성일자 : 
작성목적 : 리소스 
***************************************************/
Add.Resource = function () {
    //$(".imgDel").attr("src", ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif");
    
    $("#divt1").text(t_object);
    $("#divt2").text(t_del);
    $("#divt3").text(UserSelect);

}


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 공유대상 선택
***************************************************/
Add.AddTarget = function () {
    
    ParamArray = {

    };

    CallEncrypt(CallGetParam());

    var url = "GetUser.aspx?";
    var param = "p=" + GetEncryptValue;
    //url = url + param;
    //PopupWin(250, 360, url, "ff", "", false);
    WindowObjectReference = PopupWinOpen(url, param, "GetUser", "320", "480")
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 엔터키이벤트
***************************************************/
Add.KeyEvents = function () {
    if (event.keyCode == 13) {
        SearchDeptUserList(1);
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 조직도 검색
***************************************************/
function SearchDeptUserList(param) {
    var searchName = $("#DeptUserTreeScript1_tbDeptUser").val().securityText();
    $("#DeptUserTreeScript1_tbDeptUser").val(searchName);

    var params = searchName + "" + param + "" + CompanyID;
    DeptUserTreeScript1_CallBack1.Callback(params);
}

/**************************************************
작성자   : 
작성일자 : 
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
        

        selValues = selValues.replace(selValue + ";", "");
        selValues += selValue + ";";
    } else {
        selIDs = selIDs.replace(selID + "|", "");
        selValues = selValues.replace(selValue + ";", "");
    }

    $("#tvSelectedID").val(selIDs);

    // type(G) | deptID | deptName | parentDeptID | titleID | titleName | loginID | mail | Phone | officephone | DutyName
    // type(M) | userID | userName | parentDeptID | titleID | titleName | loginID | mail | Phone | officephone | DutyName
    $("#tvSelectedValue").val(selValues);

}


/**************************************************
작성자   : 
작성일자 : 
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
작성자   : 
작성일자 : 
작성목적 : 추가 table row 생성하고 권한값 BeforeArray[] 배열에 할당하기
***************************************************/
Add.AddAuthRow = function (keyID, name) {

}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 공유 추가 
***************************************************/
Add.AddShare = function () {
    //alert($("#tvScopeUser").val());
    if ($("#txtUser").val().length == 0) {
        alert(AddUserSelect); //AddUserSelect
        return;
    }

    Add.ValidateAddUser();
    //alert($("#tvSelectedValue").val());
    //alert($("#tvSelectedID").val());

}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 확인 
***************************************************/
Add.Confirm = function () {
    
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-10-28
작성목적 : 확인 (웹서비스 값 받기)
**********************************************************************************************/
Add.SuccessAddShare = function (data, status) {
    
}



/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-12-26
작성목적 : 리스트 가져오기
**********************************************************************************************/

Add.GetShareList = function () {

    var url = "/DeskPlusEIP/Pim/Ws/Admin/ShareUser/Add.asmx/GetAdminSharePopList";
    
    ParamArray = {
        'CompanyID': CompanyID,
        'UserID': UserID
    };
    CallEncrypt(CallGetParam());    
    
    
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessShareList");
    

    AjaxService(url, data, obj);
}

Add.SuccessShareList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Pim/Jt/Admin/ShareUser/JTAddList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    Add.Resource();
}


Add.ValidateAddUser = function () {
    //UserList
    //tvSelectedValue
    UserList = "";
    var arSelValue = $("#tvSelectedValue").val().split(';');

    for (var i = 0; i < arSelValue.length; i++) {
        var arFor = arSelValue[i].split('|');
        if (arFor.length > 0) {
            if (arFor[0] == "M") //유저
            {

                if ($("#M_" + arFor[1]).html() == null) {
                    UserList += "M_" + arFor[1] + "|";
                }
                

            }
            if (arFor[0] == "G") //부서
            {

                if ($("#G_" + arFor[1]).html() == null) {
                    UserList += "G_" + arFor[1] + "|";
                }
            }
        }

    }
    //alert(UserList);
    Add.AddShareList();
}



/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-12-26
작성목적 : 공유자추가
**********************************************************************************************/
Add.AddShareList = function () {

    var url = "/DeskPlusEIP/Pim/Ws/Admin/ShareUser/Add.asmx/AddOpenUserList";

    ParamArray = {
        'UserList': UserList,
        'AddUserID': UserID
    };
    CallEncrypt(CallGetParam());


    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessAddShareList");


    AjaxService(url, data, obj);
}

Add.SuccessAddShareList = function (data, status) {
    
    Add.GetShareList();
    //$('#placeholder').setTemplateURL('/DeskPlusEIP/Pim/Jt/Admin/ShareUser/JTAddList.htm?seq=' + Guids(), null, { filter_data: false });
    //$('#placeholder').processTemplate(data.d);
}


Add.DelUserList = function (arg) {
    

    var OpenUserID = 0;
    var GroupID = 0;

    if (arg.length > 0) {

        if (arg != "ALL") {

            if (!confirm(ApplyDelete)) //ApplyDelete
                return;

            var arArg = arg.split('_');

            if (arArg[0] == "G") {
                GroupID = arArg[1];
            }
            if (arArg[0] == "M") {
                OpenUserID = arArg[1];
            }
        }
        var url = "/DeskPlusEIP/Pim/Ws/Admin/ShareUser/Add.asmx/DelOpenuserAdmin";
        ParamArray = {
            'CompanyID': CompanyID,
            'OpenUserID': OpenUserID,
            'GroupID': GroupID,
            'UserID': UserID
        };

        CallEncrypt(CallGetParam());

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Add.SuccessDel");

        AjaxService(url, data, obj);
    }
}

Add.SuccessDel = function (data, status) {
    
    Add.GetShareList();
}

