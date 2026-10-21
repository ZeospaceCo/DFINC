Tree = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            active: AccordionActive,
            overTop: 55
        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () {
                $("#accordion").accordion("resize");
            }
        });

        $('#UserList').click(function (e) {
            var url = "/DeskPlusEIP/Education/Content/List.aspx";
            parent.frmContent.location.href = url;
        });

        $('#AdminDashBoard').click(function (e) {
            var url = "/DeskPlusEIP/Education/Admin/DashBoard.aspx";
            parent.frmContent.location.href = url;
        });

        $('#AdminList').click(function (e) {
            var url = "/DeskPlusEIP/Education/Admin/List.aspx";
            parent.frmContent.location.href = url;
        });
    });
});


