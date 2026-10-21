Preview = {}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/
$(function () {

    $(document).ready(function () {
        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });
    });

    Preview.Init();
});

Preview.Init = function () {//alert("dd")

    if (FormPath == "") return;

    // 양식로드
    ifrmForm.document.location.href = FormPath;
    ifrmObj = ifrmForm.document;

    // 공문내용처리
    setTimeout("Common.InitEnforce();", 100);
}

