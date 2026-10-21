Tree = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //Tree.Init();

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


    });
});


/**************************************************************
Tree.Init 폴더 초기화
작성자   : shimpark
작성일자 : 2008-08-11
작성목적 : 첫화면 확장 
**************************************************************/
Tree.Init = function () {
    
    var gubun = (DeptID != "0") ? "mod" : "add";

    ParamArray = {
        'deptID': DeptID,
        'gubun': gubun
    };

    CallEncrypt(CallGetParam());

    //alert(GetEncryptValue);
    parent.frmFolderContent.location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 추가
***************************************************/
Tree.TreeAdd = function () {

    ParamArray = {
        'deptID': DeptID,
        'gubun': 'add'
    };

    CallEncrypt(CallGetParam());
    parent.frmFolderContent.location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 부서 선택
***************************************************/
function DeptNodeSelect_onNodeSelect(node) {
    
    DeptID = node.ID;
    ParentDeptID = node.GetProperty("ParentDeptID");

    var gubun = (DeptID != "0") ? "mod" : "add";

    //파라미터값 배열에 담기
    ParamArray = {
        'deptID': node.ID,
        'gubun': gubun
    };
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    parent.frmFolderContent.location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}

