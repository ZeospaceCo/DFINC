EPMenu = {}

/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : UI를 위한 테스트 코드이므로 별도 개발 필요
***************************************************/
EPMenu.MovePage = function (url, target) {
    if (target == "EP") {
        location.href = url;
    } else {
        location.href = "Default.aspx?target=" + url;
    }
}