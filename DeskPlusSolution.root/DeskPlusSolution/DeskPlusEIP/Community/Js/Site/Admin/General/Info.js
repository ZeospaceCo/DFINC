var Info = {};

/**************************************************
작성자 : famo23
작성일자 : 2012-05-11
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Info.Confirm();
        });

        //대분류 
        $('#ddlParent').change(function () {
            Info.GetChildCategory();
        });

        // 커뮤니티 성격
        $('#rblType').click(function () {
            Info.SetJoinType();
        });

        Info.SetJoinType();
    });
});


/**************************************************************
작성자 : famo23
작성일자 : 2012-05-11
작성목적 : 커뮤니티정보수정 (웹서비스 호출)
**************************************************************/
Info.Confirm = function () {

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

    var type = $('input[name="rblType"]:checked').val();
    var joinType = "2";

    if (type == "1") {
        joinType = $('input[name="rblJoinType"]:checked').val();
    }
    else if (type == "3") {
        joinType = "3";

        validator.checkRequired("ctbQuestion", R_InputQuestion, true);
        if (!validator.validate()) return;

        validator.checkRequired("ctbAnswer", R_InputAnswer, true);
        if (!validator.validate()) return;
    }

    BtnCheck = "Y";

    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/General/Info.asmx/ModCom";
    ParamArray = {
        'communityID': CommunityID,
        'name': $("#ctbName").val(),
        'description': $("#ctbDesc").val(),
        'question': $("#ctbQuestion").val(),
        'answer': $("#ctbAnswer").val(),
        'parentID': ParentCategory,
        'childID': ChildCategory,
        'type': type,
        'joinType': joinType
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Info.SuccessModCom");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-11
작성목적 : 커뮤니티정보수정 (웹서비스 값 받기)
**********************************************************************************************/
Info.SuccessModCom = function (data, status) {

    if (data.d) {
        alert(R_CompleteConfirm);
        BtnCheck = "N";
    }
}


/**************************************************************
작성자 : famo23
작성일자 : 2012-05-11
작성목적 : 소분류 값 가지고 오기 (웹서비스 호출)
**************************************************************/
Info.GetChildCategory = function () {

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
        var obj = eval("Info.SuccessChildCategory");
        var url = "/DeskPlusEIP/Community/Ws/Site/Admin/General/Info.asmx/GetCategory";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-11
작성목적 : 소분류 값 가지고 오기 (웹서비스 값 받기)
**********************************************************************************************/
Info.SuccessChildCategory = function (data, status) {

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
작성일자 : 2012-07-05
작성목적 : 커뮤니티 가입방식
***************************************************/
Info.SetJoinType = function () {

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

