
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
        $('#cbtnCheckMultiValue').click(function () {
            Add.GetCountCodeID();

            if (CountCodeID == 0) {
                alert(R_ApplyUseCode);
            }
            else if (CountCodeID > 0) {
                alert(R_DuplicateCode);
                $("#ctbCodeID").focus();
                return;
            }
        });

        //다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMultiLang").val(), "GetMultiName", MultiLang);
        });

        if (CodeID != "") {
            $('#ctbCodeID').attr("disabled", true);
        }

        $('#ctbMultiLang').attr("disabled", true);

    });
});




/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 확인
**********************************************************************************************/
Add.Confirm = function () {

    $('#ctbCodeID').attr("disabled", false);
    $('#ctbMultiLang').attr("disabled", false);

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbCodeID", R_InputCode, true);
    validator.checkRequired("ctbCodeName", R_InputCodeName, true);

    if (!validator.validate()) return;

    if (CodeID == "") {
        Add.GetCountCodeID();
        if (CountCodeID > 0) {
            alert(R_DuplicateCode);
            return;
        }
    }

    __doPostBack('lbtnSubmit', '');
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 상위 codeid 로 카운트 가져오기 (중복검열)
**********************************************************************************************/
Add.GetCountCodeID = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbCodeID", R_InputCode, true);
    CountCodeID = "-1";
    if (!validator.validate()) return;

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Code/AddCategory.asmx/GetSysCodeCountByRootCodeID";
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'companyID': CompanyID,
        'codeID': $("#ctbCodeID").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessGetCountCodeID");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, codeid 로 카운트 가져오기
**********************************************************************************************/
Add.SuccessGetCountCodeID = function (data, status) {
    CountCodeID = data.d;
}



/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
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

    $("#ctbMultiLang").val(multiNames);
}