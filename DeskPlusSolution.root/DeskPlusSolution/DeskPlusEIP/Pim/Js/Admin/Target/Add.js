Add = {}

$(function () {
    $(document).ready(function () {
        //확인
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            Add.Confirm();
        });

        //취소
        $('#CbtnCancel').click(function (e) {
            e.preventDefault();
            Add.Cancel();
        });

        //추가
        $('#ClientSmartButton1').click(function (e) {
            e.preventDefault();
            Add.Add();
        });
    });
});
/**************************************************
작성자   : 
작성일자 : 
작성목적 : 
***************************************************/



/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 목표 추가 취소 
***************************************************/
Add.Cancel = function () {
    //document.location.href = "List.aspx?p=" + encodeURIComponent(Param);
    document.location.href = "../Target/List.aspx";
}

/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 목표 공개대상
***************************************************/
Add.GetUser = function () {

    var url = "../Common/GetUser.aspx";
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "GetUser", "800", "500")
}

/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 삭제
***************************************************/
Add.Del = function (OwnerID, Type) {
    
}

