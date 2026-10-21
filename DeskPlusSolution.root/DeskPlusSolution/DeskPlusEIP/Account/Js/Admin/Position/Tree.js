Tree = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            overTop: 0,
            active: 0
        });

        //확인
        $('#TreeAdd').click(function (e) {
            e.preventDefault();
            Tree.TreeAdd();
        });

        //Tree.Init();
    });
});


/**************************************************************
Tree.Init 폴더 초기화
작성자   : shimpark
작성일자 : 2008-08-11
작성목적 : 첫화면 확장 
**************************************************************/
Tree.Init = function () {

    $("#TreeAdd").hide();

    if (PCodeID == "-1") {
        $("#TreeAdd").show();
    }
    
    ParamArray = {
        'codeID': CodeID,
        'scope': Scope,
        'gubun': 'mod'
    };

    CallEncrypt(CallGetParam());
    parent.frmContent.location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 추가
***************************************************/
Tree.TreeAdd = function () {

    ParamArray = {
        'codeID': CodeID,
        'scope': Scope,
        'gubun': 'add'
    };

    CallEncrypt(CallGetParam());
    parent.frmContent.location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 선택
***************************************************/
function CodeNodeSelect_onNodeSelect(node) {

    CodeID = node.ID;
    PCodeID = node.GetProperty("ParentCodeID");
    Scope = node.GetProperty("Scope");    

    $("#TreeAdd").hide();
    if (PCodeID == "0") {
        $("#TreeAdd").show();
    }

    //파라미터값 배열에 담기
    ParamArray = {
        'codeID': node.ID,
        'scope': Scope,
        'gubun': 'mod'
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    parent.frmContent.location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}

