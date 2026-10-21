ViewQRCode = {}

/*******************************************************
작성자: famo23
작성일: 2012-03-08
작성목적: 초기화
********************************************************/
$(function () {
    $(document).ready(function () {
        $(".imageSlider .imageList").jCarouselLite({
            btnNext: ".imageSlider .right",
            btnPrev: ".imageSlider .left",
            visible: 1
            //auto: 1200,
            //speed: 1000
        });
    });
});