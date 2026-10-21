AddUser = {}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    $(document).ready(function () {

        //회사담당자 추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            AddUser.Add();
        });

        //취소
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            self.close();
        });


        //로긴중복체크
        $('#cbtnLoginCheck').click(function (e) {
            e.preventDefault();
            AddUser.LoginCheck();
        });


    });
});



/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 회사 부서 추가
***************************************************/
AddUser.Add = function () {

    if (DeptID == "0" || DeptID == "") {
        alert("사용자를 추가할 부서를 선택해 주세요");
        return;
    }    

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbName", R_InputName, true);                      //"이름을 입력해 주세요"
    validator.checkRequired("ctbLoginID", R_InputLoginID, true);                //"로그인 아이디를 입력해 주세요"    
    validator.checkRequired("ctbPassword", R_InputPassword, true);              //"패스워드를 입력해 주세요"
    if (!validator.validate()) return;

    if (LoginCheck == false) {
        alert(R_CheckLoginRepetition);
        return;
    }

    var name = $("#ctbName").val();
    var loginID = $("#ctbLoginID").val();
    var pwd = $("#ctbPassword").val();

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Company/AddUser.asmx/Add";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'deptID': DeptID,
        'name': name,
        'loginID': loginID,
        'pwd': pwd
    };

    var obj = eval("AddUser.SuccessAdd");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 회사 부서 추가
**********************************************************************************************/
AddUser.SuccessAdd = function (data, status) {
    if (data.d == true) {
        alert(R_SUCCESS);
        opener.ReloadPage();
        self.close();
    }
    else {
        alert(R_ProcessingError);
    }
}



/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 로그인 체크
***************************************************/
AddUser.LoginCheck = function () {
    LoginCheck = false;
    var url = "/DeskPlusEIP/Account/Ws/Admin/User/Add.asmx/GetLoginCheck";
    var loginID = $("#ctbLoginID").val();

    if (loginID == "") {
        alert(R_InputLoginID);          //"로그인 아이디를 입력해 주세요"
        $("#ctbLoginID").focus();
        return;
    }

    ParamArray = {
        'companyID': CompanyID,
        'loginID': loginID,
        'gubun': 'add'
    };

    var obj = eval("AddUser.SuccessLoginCheck");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시  로그인 체크
**********************************************************************************************/
AddUser.SuccessLoginCheck = function (data, status) {
    if (data.d == false) {
        LoginCheck = true;
        alert(R_OnlyOneLoginID);
    }
    else {
        alert(R_RepetitionLoginID);
    }
}