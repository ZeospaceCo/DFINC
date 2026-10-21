List = {}

$(function () {
    $(document).ready(function () {

        //확인
        $('#CbtnModClassInfo').click(function (e) {
            e.preventDefault();
            __doPostBack('lbtnRegister', '');
        });

        $('#rblCarbonUserViewOption').change(function (e) {
            e.preventDefault();
            List.SetCarbon();
        });

        List.SetCarbon();
    });
});

List.SetCarbon = function () {

    var vals = "0";
    var objs = document.getElementsByName("rblCarbonUserViewOption");
    for (var i = 0; i < objs.length; i++) {
        if (objs[i].checked) {
            vals = $(objs[i]).val();
        }
    }
    //alert(vals)
    if (vals == "0") {
        // 결재 진행중에도 열람
        $("#trreport").show();

    } else {
        // 결재 완료후에만 열람
        $("#trreport").hide();
        $("#rblReportUserCarbonViewOption_0").removeAttr("checked");
        $("#rblReportUserCarbonViewOption_1").attr("checked", "checked");


    }
    //alert($("#rblReportUserCarbonViewOption_1").attr("checked"))
}