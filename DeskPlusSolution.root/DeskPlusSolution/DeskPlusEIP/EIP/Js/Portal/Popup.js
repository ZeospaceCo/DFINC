Popup = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //리로드
        $('#ptReload').click(function (e) {
            e.preventDefault();
        });  

        //닫기
        $('#ptClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });        
    });
});

