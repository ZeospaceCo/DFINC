ListLinked = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //거부
        $('#cbtnReject').click(function (e) {
            e.preventDefault();
            ListLinked.Reject();
        });

        //승인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            ListLinked.Approval();
        });

        //검색
        $('#cbtnFindit').click(function (e) {
            e.preventDefault();
            ListLinked.Search()
        });

        //검색입력필드 엔터 이벤트 기술
        $("#tbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListLinked.Search();
            }
        });

        //검색어입력
        $("#tbSearch").attr("placeholder", R_PlaceholderText);        
    });
});

/**********************************************************************************************
[검색]
**********************************************************************************************/
ListLinked.Search = function () {
    var searchValue = $("#tbSearch").val().securityText();

    $("#tbSearch").val(searchValue);
    __doPostBack('lbtnSearch', '');
}

/**********************************************************************************************
[엔터]
**********************************************************************************************/
ListLinked.checkMods = function (evt, theForm) {
    if (evt.keyCode == 13)
        return false;
}

/**********************************************************************************************
[승인]
**********************************************************************************************/
ListLinked.Approval = function () {
    var selectedItems = SelectedItems();
    var rowSplit = selectedItems.split("");

    if (rowSplit.length > 1) {
        if (window.confirm(Admin_ListLinked_IsReceipt)) {
            __doPostBack('lbtnApproval', '');
        }
    }
    else {
        alert(R_ConfirmProject);
        return;
    }
}

/**********************************************************************************************
[거절]
**********************************************************************************************/
ListLinked.Reject = function () {
    var selectedItems = SelectedItems();
    var rowSplit = selectedItems.split("");

    if (rowSplit.length > 1) {
        if (window.confirm(Admin_ListLinked_IsReject)) {
            __doPostBack('lbtnReject', '');
        }
    }
    else {
        alert(R_RejectProject);
        return;
    }
}