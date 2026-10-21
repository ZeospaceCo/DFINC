ShareUserPop = {}


$(function () {
    $(document).ready(function () {

        /* 2017-02-10 제거
        //자동화완성 autocomplete - written by shimpark
        $("#UCUserDeptTreeScript2_tbDeptUser").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/DeskPlusEIP/Account/Ws/Common/GetUserDept.asmx/GetDeptUserSearch",
                    data: "{ 'companyID': '" + CompanyID + "','gubun': 'M','searchName': '" + request.term + "' }",
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
                $("#UCUserDeptTreeScript2_tbDeptUser").val(ui.item.value);
            },
            minLength: 1
        }).autocomplete("widget").addClass("fixed-height");
        */

        $("#UCUserDeptTreeScript2_tbDeptUser").keydown(function (event) {
            //event.preventDefault();            
            ShareUserPop.KeyEvents();
        });


        //취소
        $('#CbtnTreeView').click(function (e) {
            e.preventDefault();
            ShareUserPop.SearchDeptUserList(2);
        });

        //검색 
        $('#CbtnDeptUserSearch').click(function (e) {
            e.preventDefault();

            ShareUserPop.SearchDeptUserList(1);
        });



    });
});




/**************************************************
작성자   : shimpark
작성목적 : 권한추가 버튼
***************************************************/
ShareUserPop.AddAuth = function () {
    var selectedID = $("#tvSelectedID").val().trimEnds(';');
    var selectedValue = $("#tvSelectedValue").val().trimEnds(';');

    
    selectedValue = replaceAll(selectedValue, '$', '|');
    selectedValue = replaceAll(selectedValue, '/', '|');

    if (selectedID == "" && selectedValue == "") {
        return;
    }

    CompareArray = [];

    if (selectedID != "" && selectedValue != "") {
        var selStr = selectedValue.split(';');
        for (i = 0; i < selStr.length; i++) {
            var selGubun = selStr[i].split('|')[0];
            var selOwnerID = selStr[i].split('|')[1];
            var selName = selStr[i].split('|')[2];

            //1. 넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
            ShareUserPop.AddCompareArray(selGubun + "$" + selOwnerID, selName);
            var response = false;

            //2. 넘어온 권한값 배열과 권한값 비교하기
            $.each(CompareArray, function (i, v) {
                response = ShareUserPop.CompareArrayVal(AuthArray, v);
            });

            //3. 중복값은 1을 의미함.
            if (response != 1) {
                ShareUserPop.AddAuthRow(selGubun + "$" + selOwnerID, selName);
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
작성자   : daniel
작성일자 : 2011-04-11
작성목적 : 엔터키이벤트
***************************************************/
ShareUserPop.KeyEvents = function () {

    if (event.keyCode == 13) {
        
        ShareUserPop.SearchDeptUserList(1);
        
    }
}

 


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 조직도 검색
***************************************************/
ShareUserPop.SearchDeptUserList = function (param) {

    var searchName = $("#UCUserDeptTreeScript2_tbDeptUser").val();
     
 

    var params = searchName + "" + param;


    UCUserDeptTreeScript2_CallBack1.Callback(params);

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

    


    var arSel = selValue.split('|');

    if (arSel[0] == "M") {
    
           parent.PopAddUser(selValue);        
    }


}


/********************



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

    var arSel = selValues.split('|');

    if (arSel[0] == "M") {
        parent.PopAddUser(selValues);        
    }
}


 