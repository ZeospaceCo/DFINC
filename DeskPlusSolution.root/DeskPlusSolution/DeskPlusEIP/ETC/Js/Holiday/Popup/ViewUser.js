ViewUser = {}

$(function () {

    $(document).ready(function () {
        // 페이저 초기화
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });


    });
});