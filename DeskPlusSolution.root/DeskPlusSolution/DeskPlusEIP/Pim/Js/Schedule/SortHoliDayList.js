SortHoliDayList = {}

$(function () {
    $(document).ready(function () {
        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();

            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });


    });
});

