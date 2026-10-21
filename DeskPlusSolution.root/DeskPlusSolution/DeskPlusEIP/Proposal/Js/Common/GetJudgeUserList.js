JudgeUserList = {}

var BeforeArray = []; //db 에서 넘어 온 값을 배열에 담는다.
var CompareArray = []; //넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
var DelArray = []; //삭제할 값 배열에 담기.

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/
$(function () {

    $(document).ready(function () {

        //추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            JudgeUserList.Add();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        JudgeUserList.Init();
    });
});

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 초기화
**********************************************************************************************/
JudgeUserList.Init = function () {
    JudgeUserList.GetList();

    // 1차 평가자가 평가 시, 1차 심사리스트 수정 할 때
    if(MenuID == "Judge")
        JudgeUserList.CheckUserID();
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
JudgeUserList.GetList = function () {
    
    var url = "/DeskPlusEIP/Proposal/Ws/Common/GetJudgeUserList.asmx/GetList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0    
    ParamArray = {
        "degree": "1"
    };

    var obj = eval("JudgeUserList.SuccessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
JudgeUserList.SuccessList = function (data, status) {
    $('#phJudgeList').setTemplateURL('/DeskPlusEIP/Proposal/Jt/Common/JTGetJudgeUserList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phJudgeList').processTemplate(data.d);
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 전체 체크박스
***************************************************/
JudgeUserList.Checked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}


/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 선택된 값 가져오기
***************************************************/
JudgeUserList.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 심사자 추가하기
***************************************************/
JudgeUserList.Add = function () {

    //var checkCount = $('.cbxID:checked').length;

    //UserID$이름
    var selValue = JudgeUserList.CheckedValue();

    if (selValue.length > 0) {
        var tempUserID = "";
        var tempName = "";

        for (var i = 0; i < selValue.length; i++) {
            tempUserID += selValue[i].split("$")[0] + ",";
            tempName += selValue[i].split("$")[1] + ",";
        }

        var response = tempUserID + "‡" + tempName + "‡" + "";

        if (parent.GetDept) {
            parent.GetDept(escape(response));
        }
    }

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 먼저 선택한 
***************************************************/
JudgeUserList.CheckUserID = function () {
    var judgeUser = [];

    if (JudgeList.length > 0) {
        // 선택한 1차 심사자UserID 정보
        judgeUser = JudgeList.split(',');

        // 1차 심사자리스트 카운트
        var userCount = $('.cbxID').length;

        if (userCount > 0) {
            for (var i = 0; i < judgeUser.length; i++) {
                var userID = "cbxID" + judgeUser[i];
                //$("input:checkbox[id=" + userID + "]").attr("checked", true);
                $("input:checkbox[id=" + userID + "]").attr('disabled', 'disabled');
            }

        }
    }

}