FaMenu = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            FaMenu.Confirm();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            self.close();
        });
    });
});


/**************************************************
작성목적 : 트리 선택 이벤트
***************************************************/
function MenuNodeSelect_onNodeSelect(node) {
    // 체크된 리스트 세팅
    SetCheckedList(node);

    if (document.getElementById("tvSelectedValue").value != "") {

        var tmpName = "";
        var arrSelectedValue = (document.getElementById("tvSelectedValue").value).split("$");

        for (var i = 0; i < arrSelectedValue.length; i++) {
            if (arrSelectedValue[i] != "") {
                tmpName += arrSelectedValue[i] + ",";
            }
        }

        $("#menuName").val(tmpName);
    }
    else {
        $("#menuName").val("");
    }
}


/**************************************************
작성목적 : 트리 체크한 값 가져오기
***************************************************/
function SetCheckedList(node) {
    var selIDs = document.getElementById("tvSelectedID").value; 	// ID값만 넣기
    var selValues = document.getElementById("tvSelectedValue").value;  // Value 값 넣기

    var selID = node.ID;
    var selValue = node.Text;

    if (selID != "0") {

        // 다중선택
        if (node.Checked) {
            selIDs = selIDs.replace(selID + "|", "");
            selIDs += selID + "|";

            selValues = selValues.replace(selValue + "$", "");
            selValues += selValue + "$";
        } else {
            selIDs = selIDs.replace(selID + "|", "");
            selValues = selValues.replace(selValue + "$", "");
        }
        
        document.getElementById("tvSelectedID").value = selIDs;
        document.getElementById("tvSelectedValue").value = selValues;
    }
}


/**************************************************
작성목적 : 확인
***************************************************/
FaMenu.Confirm = function () {

    var selectID = document.getElementById("tvSelectedID").value;

    if (selectID == "" || selectID == "0") {
        alert(R_SelectFaMenu);
        return;
    }

    var url = "/DeskPlusEIP/EIP/Ws/Menu/FaMenu.asmx/Add";

    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID,
        'menuID': selectID
    };

    var obj = eval("FaMenu.SuccessConfirm");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성목적 : 성공시  즐겨찾기 체크
**********************************************************************************************/
FaMenu.SuccessConfirm = function (data, status) {
    if (data.d == true) {
        alert(R_ProcessingApply);
        opener.MenuReLoad();
        self.close();
    }
    else {
        alert(R_ProcessingError);
    }
}

