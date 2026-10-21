DeptUserSearch = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //닫기버튼
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            parent.disablePopup();
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

        //검색입력필드 엔터 이벤트 기술
        $("#DeptSearchTree1_tbDeptSearch").keydown(function (event) {
            KeyEvents();
        });

    });
});


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 조직도 검색
***************************************************/
function SearchDeptUserList(param) {

    var searchName = $("#DeptSearchTree1_tbDeptSearch").val().securityText();
    $("#DeptSearchTree1_tbDeptSearch").val(searchName);

    if (searchName == "" && param != "2") {
        alert(R_InputDeptName);
        return;
    }

    var params = searchName + "" + param + "" + CompanyID;
    DeptSearchTree1_CallBack1.Callback(params);    
}

/**************************************************
작성자   : 심재운
작성목적 : 조직도 선택시
***************************************************/
function DeptSearchTreeScript_onNodeSelect(node) {

    var selID = node.ID;
    var selValue = node.Value;
    //alert(selValue);
    var encValue = escape(selValue)
    
    parent.disablePopup();
    parent.SetInfo(selID, encValue);

}


function KeyEvents() {
    if (event.keyCode == 13) {
        SearchDeptUserList(1);
    }
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

        var selID = rtnValue.split('|')[1]; 
        var encValue = escape(rtnValue.split('|')[2] + '|' + rtnValue.split('|')[11]);

        parent.disablePopup();
        parent.SetInfo(selID, encValue);

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
