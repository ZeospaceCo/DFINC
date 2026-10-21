GallerySubList = {}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {
    $(document).ready(function () {
        $("a[rel='example3']").colorbox({ transition: "none", width: "80%", height: "80%" });

        $("#zeoModalContact").draggable({ disabled: false });
    });

    //닫기
    $('#CbtnClose').click(function (e) {
        e.preventDefault();
        window.open('about:blank', '_self').close();
    });
});