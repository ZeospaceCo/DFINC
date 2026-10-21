var Tree = {};

Tree.ContentFrame = null;

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //--jquery-ui 1.10.3 는 resize 없어짐
        var accordHeight = $(window).height() - 250;
        $(".menusub").css("height", accordHeight);
        $(".menusub").css("background", "#ffff");

        Common.SetMenu(0);  //트리메뉴제어(기본확장메뉴-양식리스트)

        // 부서정보 변경 시, 변수값 재설정.
        $("#ddlAdditionDept").change(function (e) {
            e.preventDefault();
            if ($("Common.ModDeptInfo", parent.frmContent)) { parent.frmContent.Common.ModDeptInfo(); }
        });

        //트리검색(검색버튼)
        $('#CbtnDeptUserSearch').click(function (e) { e.preventDefault(); SearchDeptUserList(1); });

        //트리검색(검색취소)
        $('#CbtnTreeView').click(function (e) { e.preventDefault(); SearchDeptUserList(2); });

        //검색입력필드 엔터 이벤트 기술
        $("#UCDeptUserTreeScript1_tbDeptUser").keydown(function (event) { Common.KeyEvents(); });

        //열람권한
        $("#ddlAccessTitle").change(function (e) { e.preventDefault(); Common.SetGradeInfo($("#ddlAccessTitle").val()); });
        Common.SetGradeInfo($("#ddlAccessTitle").val());

        //입력값 체크하기
        Common.CheckOpinion();

        Common.Init();
    });
});

/**************************************************************
리스트 페이지 호출하기
**************************************************************/
Tree.GoPage = function (_menuID) {
    //var nonActiveX = ($.browser.msie) ? "F" : "T";      // activeX(F) , nonActiveX(T)
    var nonActiveX = "T";      // activeX(F) , nonActiveX(T)

    var url = "";
    switch (_menuID.toUpperCase()) {
        case "TEMPORARY":
            //임시보관함
            url = "/DeskPlusEIP/Approval/FormFlow/Temporary/List.aspx";
            ParamArray = { 'menuID': "Temporary" };

            break;
        case "SUBMIT":
            //상신문서함
            url = "/DeskPlusEIP/Approval/FormFlow/Submit/List.aspx";
            ParamArray = { 'menuID': "Submit" };

            break;
        default:
            //양식
            if ($("#ddlAdditionDept").val() == "") {
                alert(R_Msg_ErrUserInfo); return;       // deptID | deptName | titleID | titleName | dutyID | dutyName
            }

            var tmpval = $("#ddlAdditionDept").val().split("|");

            var url = "/DeskPlusEIP/Approval/FormFlow/Basic/Add.aspx";
            ParamArray = {
                'formID': FormID
            , 'deptID': tmpval[0]
            , 'deptName': tmpval[1].replace("&", "zeo$")
            , 'titleID': tmpval[2]
            , 'titleName': tmpval[3]
            , 'dutyID': tmpval[4]
            , 'dutyName': tmpval[5]
            , 'rootDeptID': tmpval[6]
            , 'rootDeptName': tmpval[7].replace("&", "zeo$")
            , 'nonActiveX': nonActiveX
            };

            if (FormID != 0 && PopupApp.toUpperCase() == "Y") {
                var width = 1100;
                var height = screen.height;

                var sw = screen.availWidth;
                var sh = screen.availHeight;

                px = (sw - width) / 2;
                py = (sh - height) / 2;

                var opt = "";
                opt = 'top=' + py + ',left=' + px;
                opt += ',width=' + width + ',height=' + height;

                url = "/DeskplusEIP/Approval/FormFlow/Default.aspx";

                ParamArray = {
                    'formID': FormID
                , 'listID': "LEGACY"
                };
                CallEncrypt(CallGetParam());
                url = url + "?p=" + GetEncryptValue;
                window.open(url, "ApprovalAddPop1", opt + ",menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
                return;
            }
            break;
    }

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

/**************************************************
작성자   :
작성일자 :
작성목적 : onNodeSelect 개인보관함 노드 선택
ex)   node.ID : P-G-Draft / P-G-Submit
node.Attributes["ID"] : Draft / Submit
node.Attributes["PID"] : 0
node.Attributes["Section"] : P
node.Attributes["Type"] : G
***************************************************/
function onPrivateSelect(node) {       //alert(node.ID + "," + node.Text);return;
    if (node == null) { return; }

    var id = node.GetProperty("ID");
    if (id == "") return;

    Tree.GoPage(id);
}