PopPimAdd = {}
$(function () {
    $(document).ready(function () {

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            PopPimAdd.Confirm();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //시간선택
        $('#ddlstartTime').change(function (e) {
            e.preventDefault();
            PopPimAdd.SettingTime();
        });
        //시간선택
        $('#ddlendTime').change(function (e) {
            e.preventDefault();
            PopPimAdd.SettingTime();
        });


        PopPimAdd.SettingTime();

    });
});


PopPimAdd.Confirm = function () {
    var stime = $("#ddlstartTime").val();
    var etime = $("#ddlendTime").val();

    stime = stime.replace(/:/gi, '');
    etime = etime.replace(/:/gi, '');
    sdate =  stime;
    edate =  etime;
    var sInt = parseInt(sdate, 10);
    var eInt = parseInt(edate, 10);

    if (sInt >= eInt) {
        alert(TimeAlert); //TimeAlert
        return;
    }

    parent.ToDoSelectCalendar($("#ddlstartTime").val() + "|" + $("#ddlendTime").val());

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}



/**************************************************
작성자   : 김광일
작성일자 : 2011-9-8
작성목적 : 시간선택
***************************************************/
PopPimAdd.SettingTime = function () {

    

    var stime = $("#ddlstartTime").val();
    var etime = $("#ddlendTime").val();

    stime = stime.replace(/:/gi, '');
    etime = etime.replace(/:/gi, '');

    sdate =  stime;
    edate =  etime;

    var sInt = parseInt(sdate, 10);
    var eInt = parseInt(edate, 10);

    if (sInt >= eInt) {

        //stime +
        //var index1 = $("#ddlstartTime").index($("#ddlstartTime"));
        //alert(index1);
        var maxindex = $("#ddlstartTime").get(0).options.length;
        var sel = $("#ddlstartTime option").index($("#ddlstartTime option:selected"));

        if ((sel + 2) < maxindex) {

            $("#ddlendTime option:eq(" + (sel + 2) + ")").attr("selected", "selected");
        }
        else {

            $("#ddlendTime option:eq(47)").attr("selected", "selected");
        }


    }
}