List = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        //이전달
        $('#pvMonth').click(function (e) {
            e.preventDefault();
            __doPostBack('lbtnBefore', '');
        });

        //이후달
        $('#nxMonth').click(function (e) {
            e.preventDefault();
            __doPostBack('lbtnNext', '');
        });

    });
});

/**********************************************************************************************
[페이지 이동]
**********************************************************************************************/
List.Add = function () {
    location.href = "AddN.aspx?" + ProjParam;
}