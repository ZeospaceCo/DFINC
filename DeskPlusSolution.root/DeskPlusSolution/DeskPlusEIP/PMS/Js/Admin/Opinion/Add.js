Add = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnSubmit').click(function (e) {
            e.preventDefault();
            Add.Confirm();
        });

        //닫기
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });
    });
});

Add.Confirm = function () {
    var content = $("#tbxContent").val();

    if (content == "") {
        alert(Private_BoardAdd_DPEditorTEXT);
        return;
    }

    if (window.confirm(Project_RegisterOpinion_Save)) {
        __doPostBack('lbtnSubmit', '');
    }
}