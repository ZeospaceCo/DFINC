AddDept = {}

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
            AddDept.AddDept();
        });

        //취소
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            opener.ReloadPage();
            self.close();
        });

    });
});



/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 회사 부서 추가
***************************************************/
AddDept.AddDept = function () {


    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbDeptName", R_InputDeptName, true);                      //"이름을 입력해 주세요"
    
    if (!validator.validate()) return;

    var deptName = $("#ctbDeptName").val();    
    var shortName = $("#ctbShortName").val();
    var deptCode = $("#ctbDeptCode").val();    

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Company/AddDept.asmx/Add";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'deptID': DeptID,
        'deptName': deptName,        
        'shortName': shortName,
        'deptCode': deptCode
    };

    var obj = eval("AddDept.SuccessAddDept");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 회사 부서 추가
**********************************************************************************************/
AddDept.SuccessAddDept = function (data, status) {
    if (data.d == true) {
        alert(R_SUCCESS);
        opener.ReloadPage();
        self.close();
    }
    else {
        alert(R_ProcessingError);
    }
}


