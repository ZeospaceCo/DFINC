Default = {}

$(function () {
    $(document).ready(function () {
        //확인
        $('#CbtnModClassInfo').click(function (e) {
            e.preventDefault();
            __doPostBack('lbtnRegister', '');
        });
    });
});