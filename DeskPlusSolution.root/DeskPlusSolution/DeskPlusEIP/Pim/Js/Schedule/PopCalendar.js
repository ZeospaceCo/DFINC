PopCalendar = {}

$(function () {
    $(document).ready(function () {

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            PopCalendar.Confirm();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });


        $("#divCalen").datepick($.datepicker.regional[PreLang]);
        $("#divCalen").focus();

    });
});


PopCalendar.Confirm = function () {
    var validator = new Validator($("#form1"));

    validator.checkRequired("divCalen", Add_InputDate, true);
    if (!validator.validate()) return;

    parent.ToDoSelectCalendar($("#divCalen").val());
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}