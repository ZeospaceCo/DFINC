var Tree = {};

Tree.ContentFrame = null;

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //--jquery-ui 1.10.3 는 resize 없어짐
        var accordHeight = $(window).height() - 200;
        $(".menusub").css("height", accordHeight);
        $(".menusub").css("background", "#ffff");

        Common.SetMenu(0);  //트리메뉴제어(기본확장메뉴-양식리스트)

        //트리검색(검색버튼)
        $('#CbtnDeptUserSearch').click(function (e) { e.preventDefault(); SearchDeptUserList(1); });

        //트리검색(검색취소)
        $('#CbtnTreeView').click(function (e) { e.preventDefault(); SearchDeptUserList(2); });

        //검색입력필드 엔터 이벤트 기술
        $("#UCDeptUserTreeScript1_tbDeptUser").keydown(function (event) { Common.KeyEvents(); });

        //열람권한
        $("#ddlAccessTitle").change(function (e) { e.preventDefault(); Common.SetGradeInfo($("#ddlAccessTitle").val()); });

        //입력값 체크하기
        Common.CheckOpinion();

        Common.Init();
    });
});

/**************************************************************
리스트 페이지 호출하기
**************************************************************/
Tree.GoPage = function (_menuID) {

    FormID = "281"; // 결재망 자동 추가
    //FormID = "89"; // 결재망 자동 추가

    //var nonActiveX = ($.browser.msie) ? "F" : "T";      // activeX(F) , nonActiveX(T)
    var nonActiveX = "T";      // activeX(F) , nonActiveX(T)

    var url = "/DeskPlusEIP/Approval/FormFlow/Receive/Add.aspx";
    ParamArray = {
            // 수신문서상신
              'docID': DocumentID                //발신문서ID
            , 'approvalTag': ApprovalTag
            , 'targetUserID': TargetUserID
            , 'targetDeptID': TargetDeptID
            , 'groupID': GroupID
            , 'sequence': Sequence
            , 'passedDocID': PassedDocID
            , 'nonActiveX': nonActiveX
            , 'formid': FormID
            , 'folderStatusID': FolderStatusID
            , 'currentPage': CurrentPage
            , 'searchTerm': SearchTerm
            , 'searchTermValue': SearchTermValue
            , 'searchField': SearchField
            , 'searchValue': SearchValue
            , 'order': Order
            , 'proxyUserID': ProxyUserID
    };

    if (url != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = url + "?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : onNodeSelect 양식트리 노드 선택
ex)   node.ID : C-G-folderid
node.Attributes["ID"] : folderid
node.Attributes["PID"] : parentfolderid
node.Attributes["Section"] : C
node.Attributes["Type"] : G
***************************************************/
var tnode = null;
function onFormNodeSelect(node) {   //    alert(node.ID + "," + node.Text + "," + node.GetProterty(""));

    if (node == null) { return; }

    var id = node.GetProperty("ID");
    var pid = node.GetProperty("PID");
    var section = node.GetProperty("Section");
    var type = node.GetProperty("Type");

    tnode = node;
    setTimeout("tnode.Expand()", 300); //node.Expand();  //하위노드 확장

    if (type.toUpperCase() == "G") { return; }      //alert(R_Msg_ErrSelForm); 

    FormID = id;
    Tree.GoPage("");
}
