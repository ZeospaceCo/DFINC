Tree = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        Tree.Init();

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
        'gubun': '1'
    };

    CallEncrypt(CallGetParam());
    parent.frmFolderContent.location.href = "List.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 부서 선택
***************************************************/
function FaxNodeSelect_onNodeSelect(node) {

    var gubun = node.Value;    

    //파라미터값 배열에 담기
    ParamArray = {
        'gubun': gubun
    };
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    parent.frmFolderContent.location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************************
onCallbackComplete 
**************************************************************/
function FaxNodeSelect_onNodeComplete(node) {

}

