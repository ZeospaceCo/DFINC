PrivateTree = {};

var UserArr = "";

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        UserArr = ListAll;


        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            PrivateTree.Confirm();
        });

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //하위추가

        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            PrivateTree.AddCategory();
        });
    });
});


//확인버튼클릭
PrivateTree.Confirm = function () {
    $('#tbUserValue').val(UserArr);
    $('#tbCategoryID').val(FolderID);

    if (FolderID == 0) {
        alert(R_SelectCategory);
        return;
    }
    else {
        __doPostBack("LBTNProcess", "Register");
    }

}

//하위추가 버튼 클릭
PrivateTree.AddCategory = function () {
    var subName = $('#subFolderName').val();
    $('#tbCategoryID').val(FolderID);

    //    if (FolderID == 0) {
    //        alert(R_SelectCategory);
    //        return;
    //    }


    if (subName != '') {
        $('#tbSubName').val(subName)
    }
    else {
        alert(R_WriteName);
        return;
    }

    __doPostBack("LBTNAddCategory", "AddCategory");

}


/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {

    PrivateTree.SelectedNodeID = node.ID; //고유키

    FolderID = node.GetProperty("CategoryID");
    $("#folderName").val("");
    if (FolderID != 0) {
        $("#folderName").val(node.Text);
    }
}


/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}