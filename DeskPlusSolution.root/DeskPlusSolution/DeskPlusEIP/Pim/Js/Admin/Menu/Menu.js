Menu = {}

$(function () {
    $(document).ready(function () {
        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide'
            //event: "mouseover",
        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () {
                $("#accordion").accordion("resize");
            }
        });

        //기념일 설정
        $('#Holiday').click(function () {
            parent.frmContent.location.href = "../Holiday/List.aspx";
        });

        //공유자 관리
        $('#ShareUser').click(function () {
            parent.frmContent.location.href = "../ShareUser/List.aspx";
        });

        //일정분류 관리
        $('#Category').click(function () {
            parent.frmContent.location.href = "../Category/List.aspx";
        });

        //환경설정
        $('#configManage').click(function () {
            parent.frmContent.location.href = "../Config/Default.aspx";
        });

        //공휴일 설정
        $('#HolidayR').click(function () {
            parent.frmContent.location.href = "../HolidayR/List.aspx";
        });

        //지정휴일 설정
        $('#HolidayD').click(function () {
            parent.frmContent.location.href = "../HolidayD/List.aspx";
        });

    });
});





