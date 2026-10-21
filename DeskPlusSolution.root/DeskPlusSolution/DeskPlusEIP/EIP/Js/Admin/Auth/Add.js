
Add = {}

/**************************************************
초기화
***************************************************/
$(function () {

    $(document).ready(function () {

        //등록하기
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            Add.Confirm();
        });

        //중복체크
        $('#CheckRoleCode').click(function () {
            Add.GetCountRoleCode();
            if (CountRoleCode == 0) {
                alert(R_UseAuthCode);
            }
        });

        //순서변경
        $('#cbtnMove').click(function (e) {
            e.preventDefault();
            Add.MoveUpdown();

        });

        if (RoleID != -1) {
            $('#ctbRoleCode').attr("disabled", true);
            $('#CheckRoleCode').hide();
        }
    });
});

/**************************************************
작성자   : shimpark
작성목적 : 순서 변경하기
***************************************************/
Add.MoveUpdown = function () {

    ParamArray = {
        'roleID': RoleID,
        'gubun': Gubun
    };

    CallEncrypt(CallGetParam());
    var url = "MoveUpdown.aspx?p=" + GetEncryptValue;
    PopupWin(500, 382, url, "", "", true);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 확인
**********************************************************************************************/
Add.Confirm = function () {

    $('#ctbRoleCode').attr("disabled", false);

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbRoleCode", R_InputAuthCode, true);
    validator.checkRequired("ctbRoleName", R_InputAuthName, true);
    if (!validator.validate()) return;    

    if (RoleID == -1) {
        if (CountRoleCode != 0) {
            alert(R_CheckChoiceMultiValue);
            return;
        }
    }

    Add.AddCode();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 권한코드로 카운트 가져오기 (중복검열)
**********************************************************************************************/
Add.GetCountRoleCode = function () {

    $('#ctbRoleCode').attr("disabled", false);
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbRoleCode", R_InputAuthCode, true);

    if (!validator.validate()) return;

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Auth/Add.asmx/GetRoleCountByRoleCode";    

    ParamArray = {
        'companyID': CompanyID,
        'roleCode': $("#ctbRoleCode").val(),
        'gubun': Gubun
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessGetCountRoleCode");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, rolecode 로 카운트 가져오기
**********************************************************************************************/
Add.SuccessGetCountRoleCode = function (data, status) {
    
    if (data.d == 0) {
        CountRoleCode = data.d;
    }
    else {
        alert(R_DuplicateCheckMultiValue);
        CountRoleCode = -1;
    }

}

/**************************************************
작성자   : shimpark
작성목적 : 목록
***************************************************/
Add.GoList = function () {
    ParamArray = {
        'parentCodeID': ParentCodeID,        
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 코드저장하기
**********************************************************************************************/
Add.AddCode = function () {

    $('#ctbRoleCode').attr("disabled", false);

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Auth/Add.asmx/AddRole";

    if (RoleID == -1) {
        ParamArray = {
            'companyID': CompanyID,
            'roleCode': $("#ctbRoleCode").val(),
            'roleName': $("#ctbRoleName").val(),
            'roleDesc': $("#ctbRoleDesc").val(),
            'gubun': Gubun
        };
    }
    else {
        url = "/DeskPlusEIP/EIP/Ws/Admin/Auth/Add.asmx/ModRole";
        ParamArray = {
            'companyID': CompanyID,
            'roleID': RoleID,
            'roleCode': $("#ctbRoleCode").val(),
            'roleName': $("#ctbRoleName").val(),
            'roleDesc': $("#ctbRoleDesc").val(),
            'gubun': Gubun
        };
    }    

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessAddCode");

    AjaxService(url, data, obj);


}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, rolecode 추가하기
**********************************************************************************************/
Add.SuccessAddCode = function (data, status) {

    if (data.d == true) {
        alert(R_SUCCESS);

        ParamArray = {
            'roleID': RoleID,
            'gubun': Gubun
        };

        CallEncrypt(CallGetParam());
        parent.frmLeft.location.href = "Tree.aspx?p=" + GetEncryptValue;
    }
    else {
        alert(R_ErrorProcessing);
    }

}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 트리이동
***************************************************/
Add.GoTree = function () {

    ParamArray = {        
        'roleID': RoleID,
        'gubun': Gubun
    };

    CallEncrypt(CallGetParam());

    //이동하기    
    parent.frmLeft.location = "Tree.aspx?p=" + GetEncryptValue;
}