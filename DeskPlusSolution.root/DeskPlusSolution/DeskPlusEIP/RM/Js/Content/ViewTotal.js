View = {}

/**************************************************
작성자   : famo23
작성일자 : 2016-04-19
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        //목록
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            self.close();            
        });

        // 구분값(이미지) 선택금지
        $("[name=RblPattern]:not(:checked)").attr('disabled', 'disabled');

    });
});


/**************************************************
작성자   : famo23
작성일자 : 2016-04-19
작성목적 : 인쇄
***************************************************/
View.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}


