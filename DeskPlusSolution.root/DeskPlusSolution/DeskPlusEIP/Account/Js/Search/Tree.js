
Tree = {}

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
            Tree.KeyEvents();
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

    // 처음 로딩 시 CompanyID 값이 0으로 세팅되서...
    Tree.Default();
});


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 엔터키이벤트
***************************************************/
Tree.KeyEvents = function () {
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

    // type(G) | deptID | deptName | parentDeptID | titleID | titleName | loginID | mail | Phone | officephone | DutyName    
    // type(M) | userID | userName | parentDeptID | titleID | titleName | loginID | mail | Phone | officephone | DutyName

    var selValue = node.Value;
    var selType = "";
    var selOwnerID = "";
    var selDeptID = "";
    //alert(selValue);

    if (node.ID != "0") {
        selType = selValue.split("|")[0];
        // 부서
        if(selType == "G")
        {
            selOwnerID = "0",
            selDeptID = selValue.split("|")[1];
        }
        else
        {
            selOwnerID = selValue.split("|")[1];
            selDeptID = selValue.split("|")[3];
        }
    }
    else {
        //전체용
        selType = "G";
        selOwnerID = "0";
        selDeptID = "0";
    }

    ParamArray = {
            'companyID': CompanyID,
            'types': selType,
            'ownerID': selOwnerID,
            'deptID': selDeptID
        };
   
    

    CallEncrypt(CallGetParam());
    parent.frmContent.location.href = "List.aspx?p=" + GetEncryptValue;
}

Tree.Default = function () {

    selType = "G";
    selOwnerID = "0";
    selDeptID = "0";

    ParamArray = {
        'companyID': CompanyID,
        'types': selType,
        'ownerID': selOwnerID,
        'deptID': selDeptID
    };



    CallEncrypt(CallGetParam());
    parent.frmContent.location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : listbox 선택한 값 가져오기
***************************************************/
function LbxDeptUser_OnChange() {

    // type(G) | deptID | deptName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName
    // type(M) | userID | userName | ParentOwnerID | titleID | titleName | loginID | mail | phone | OfficePhone | DutyName
    var selType = "";
    var selOwnerID = "";
    var selDeptID = "";

    $("#" + LBDeptUser + " :selected").each(function (i, selected) {
        var rtnValue = $(selected).val();
        //alert(rtnValue);
        var valueStr = rtnValue.split("^")[1].split("|"); //nodeValue
        selType = valueStr[0]; //gubun
        selOwnerID = valueStr[1]; //ownerid
        selDeptID = rtnValue.split("^")[0].split("-")[1]; // deptID
    });
    
    ParamArray = {
        'companyID': CompanyID,
        'types': selType,
        'ownerID': selOwnerID,
        'deptID': selDeptID
    };
    
    CallEncrypt(CallGetParam());
    parent.frmContent.location.href = "List.aspx?p=" + GetEncryptValue;
}