Mod = {}

$(function () {
    $(document).ready(function () {
        //확인
        $('#CbtnSubmit').click(function (e) {
            e.preventDefault();
            Mod.Confirm();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); };
        });

        //추가
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            Mod.Add();
        });
    });
});