Tree = {}
Tree.ContentFrame = parent.frmContent;

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

        //기본 공유자 관리
        $('#Share').click(function () {
            Tree.ContentFrame.location.href = "Share.aspx";
        });
       
    });
});





