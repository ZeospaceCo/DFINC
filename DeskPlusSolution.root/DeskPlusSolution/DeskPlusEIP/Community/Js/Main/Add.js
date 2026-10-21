var Add = {};

/**************************************************
작성자 : famo23
작성일자 : 2012-05-03
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Add.Confirm();
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            Add.Cancel();
        });

        //대분류 
        $('#ddlParent').change(function () {
            Add.GetChildCategory();
        });

        // 커뮤니티 성격
        $('#rblType').click(function () {
            Add.SetJoinType();
        });

    });
});



/**************************************************************
작성자 : famo23
작성일자 : 2012-05-03
작성목적 : 커뮤니티 등록 확인
**************************************************************/
Add.Confirm = function () {

    if (BtnCheck == "Y") return;

    //커뮤니티 이름 입력 필수
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbName", R_InputCommunityName, true);
    if (!validator.validate()) return;

    var ParentCategory = $("#ddlParent").val();
    var ChildCategory = $("#ddlChild").val();

    if (ParentCategory == "0") {
        alert(R_SelParentID + "\n" + R_NoCategory);
        return;
    }

    if (ChildCategory == "0") {
        alert(R_SelChildID + "\n" + R_NoCategory);
        return;
    }

    $("#tbChild").val(ChildCategory);

    var type = $('input[name="rblType"]:checked').val();

    if (type == "3") {

        validator.checkRequired("ctbQuestion", R_InputQuestion, true);
        if (!validator.validate()) return;

        validator.checkRequired("ctbAnswer", R_InputAnswer, true);
        if (!validator.validate()) return;
    }

    BtnCheck = "Y";
    __doPostBack('lbtnConfirm', '');
}


/**************************************************************
작성자 : famo23
작성일자 : 2012-05-03
작성목적 : 취소
**************************************************************/
Add.Cancel = function () {

    location.href = "Content.aspx";
}


/**************************************************************
작성자 : famo23
작성일자 : 2012-05-07
작성목적 : 소분류 값 가지고 오기 (웹서비스 호출)
**************************************************************/
Add.GetChildCategory = function () {

    var ParentCategory = $("#ddlParent").val();

    if (ParentCategory == "0") {
        $("select[id='ddlChild']").find('option').each(function () {
            $(this).remove();

        });

        $("#ddlChild").get(0).options[0] = new Option(R_SelChildID, "0");
    }
    else {

        ParamArray = {
            'parentCategory': ParentCategory
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Add.SuccessChildCategory");
        var url = "/DeskPlusEIP/Community/Ws/Main/Add.asmx/GetCategory";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-07
작성목적 : 소분류 값 가지고 오기 (웹서비스 값 받기)
**********************************************************************************************/
Add.SuccessChildCategory = function (data, status) {

    $("select[id='ddlChild']").find('option').each(function () {
        $(this).remove();

    });

    $("#ddlChild").get(0).options[0] = new Option(R_SelChildID, "0");

    for (var i = 0; i < data.d.length; i++) {
        $("#ddlChild").get(0).options[i + 1] = new Option(data.d[i].NameBase, data.d[i].CategoryID);
    }

}

/**************************************************
작성자 : famo23
작성일자 : 2012-05-08
작성목적 : 개설 신청후 트리 추가
***************************************************/
Add.GoMain = function () {

    alert(R_CompleteConfirm);
    var goUrl = "/DeskPlusEIP/Community/Default.aspx";
    parent.location.href = goUrl;
}

/**************************************************
작성자 : famo23
작성일자 : 2012-07-05
작성목적 : 커뮤니티 가입방식
***************************************************/
Add.SetJoinType = function () {

    var type = $('input[name="rblType"]:checked').val();

    if (type == "1") {
        $("#JoinType1").css("display", "");
        $("#JoinType2").css("display", "none");
        $("#JoinType3").css("display", "none");
    }
    else if (type == "2") {
        $("#JoinType1").css("display", "none");
        $("#JoinType2").css("display", "");
        $("#JoinType3").css("display", "none");
    }
    else if (type == "3") {
        $("#JoinType1").css("display", "none");
        $("#JoinType2").css("display", "none");
        $("#JoinType3").css("display", "");
    }
}



