SortTodoList = {}

$(function () {
    $(document).ready(function () {
        //닫기
        $('#CbtnClosetodo').click(function (e) {
            e.preventDefault();

            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });


    });
});

SortTodoList.TodoPage = function (arg) {
    
    //opener.test(arg);
    parent.test(arg);
}