var Tree = {};

Tree.ContentFrame = null;

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //--jquery-ui 1.10.3 는 resize 없어짐
        var accordHeight = $(window).height() - 180;
        $(".menusub").css("height", accordHeight);
        $(".menusub").css("background", "#ffff");

        Common.SetMenu(0);  //트리메뉴제어(기본확장메뉴-조직도)


        // 부서정보 변경 시, 변수값 재설정.
        $("#ddlAdditionDept").change(function (e) {
            e.preventDefault();
            if ($("Common.ModDeptInfo", parent.frmContent)) { parent.frmContent.Common.ModDeptInfo(); }
        });

        //트리검색(검색버튼)
        $('#CbtnDeptUserSearch').click(function (e) { e.preventDefault(); SearchDeptUserList(1); });

        //트리검색(검색취소)
        $('#CbtnTreeView').click(function (e) { e.preventDefault(); SearchDeptUserList(2); });

        //검색입력필드 엔터 이벤트 기술
        $("#UCDeptUserTreeScript1_tbDeptUser").keydown(function (event) { Common.KeyEvents(); });

        //입력값 체크하기
        Common.CheckOpinion();

        Common.Init();
    });
});

/**************************************************************
리스트 페이지 호출하기
**************************************************************/
Tree.GoPage = function (_menuID) {

    //var nonActiveX = ($.browser.msie) ? "F" : "T";      // activeX(F) , nonActiveX(T)
    var nonActiveX = "T";      // activeX(F) , nonActiveX(T)

    //양식
    if ($("#ddlAdditionDept").val() == "") {
        alert(R_Msg_ErrUserInfo); return;       // deptID | deptName | titleID | titleName | dutyID | dutyName
    }

    var tmpval = $("#ddlAdditionDept").val().split("|");

    if (GetFormID != 0) {
        FormID = GetFormID;
    }

    var url = "/DeskPlusEIP/Approval/FormFlow/Basic/Add.aspx";
    ParamArray = {
        'formID': FormID
            , 'nickname': NickName
            , 'legacykey': LegacyKey
            , 'parametc': ParamEtc
            , 'deptID': tmpval[0]
            , 'deptName': tmpval[1].replace("&", "zeo$")
            , 'titleID': tmpval[2]
            , 'titleName': tmpval[3]
            , 'dutyID': tmpval[4]
            , 'dutyName': tmpval[5]
            , 'rootDeptID': tmpval[6]
            , 'rootDeptName': tmpval[7].replace("&", "zeo$")
            , 'nonActiveX': nonActiveX
    };

    if (url != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = url + "?p=" + GetEncryptValue;
    }
}
