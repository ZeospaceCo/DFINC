List = {}

$(function () {
    $(document).ready(function () {
        //목표등록
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            List.Add();
        });
    });
});

/**************************************************
작성자   : freeman
작성일자 : 2011-04-11
작성목적 : 목표 추가
***************************************************/
List.Add = function (arg) {
    //var encParam = AjaxGetEncrypt(encodeURIComponent("folderID=" + FolderID + "&classID=" + ClassID + "&sortKey=" + SortKey + "&sortOrder=" + SortOrder));
    //document.location.href = "Add.aspx?p=" + encodeURIComponent(encParam);
    
    //목표등록 버튼을 눌렀을때와 메뉴에서 추가 버튼을 눌렀을때 Add 화면에서 상위 목표명/기간을 visible 처리함
    document.location.href = "./Add.aspx";
}

/**************************************************
작성자   : freeman
작성일자 : 2011-04-11
작성목적 : 삭제
***************************************************/
List.Del = function (arg) {
}

