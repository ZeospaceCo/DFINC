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

    });
});


/**************************************************************
Tree.Init 폴더 초기화
작성자   : shimpark
작성일자 : 2008-08-11
작성목적 : 첫화면 확장 
**************************************************************/
Tree.Init = function () {    

    ParamArray = {
        'deptID': 0
    };

    CallEncrypt(CallGetParam());
    parent.frmFolderContent.location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 문서추가
***************************************************/
function DeptNodeSelect_onNodeSelect(node) {

    DeptID = node.ID;

    //파라미터값 배열에 담기
    ParamArray = {
        'deptID': DeptID
    };
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    parent.frmFolderContent.location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}

