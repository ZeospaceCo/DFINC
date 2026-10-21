Tree = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            //event: "mouseover"
            active: AccordionActive,
            overTop: 55
        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () {
                $("#accordion").accordion("resize");
            }
        });


        //중요표시
        $('#ImpoUser').click(function (e) {
            var url = "/DeskPlusEIP/Task/ImpoUser/List.aspx";
            parent.frmContent.location.href = url;
        });

        //임시저장
        $('#DraftList').click(function (e) {
            var url = "/DeskPlusEIP/Task/Draft/List.aspx";
            parent.frmContent.location.href = url;
        });


        //메뉴설정
        $('#MenuList').click(function (e) {
            var url = "/DeskPlusEIP/Task/Menu/Setting.aspx";
            parent.frmContent.location.href = url;
        }); 

        //관리자 - 분류관리 클릭
        $('#AdminItem').click(function (e) {
            var url = "/DeskPlusEIP/Task/Admin/Menu/List.aspx";
            parent.frmContent.location.href = url;

        });


        //관리자 - 출처관리 클릭
        $('#AdminSource').click(function (e) {           
            var url = "/DeskPlusEIP/Task/Admin/Source/List.aspx";
            parent.frmContent.location.href = url;
        });


        //관리자 - 전체 협업 리스트
        $('#AdminList').click(function (e) {
            var url = "/DeskPlusEIP/Task/Admin/Content/List.aspx";
            parent.frmContent.location.href = url;
        });
        
        // 요청하기 (저장)
        $('#ReserveAdd').click(function (e) {
            e.preventDefault();
            Tree.Add();
        });

        Tree.Init();
    });
});


/**************************************************
초기화
***************************************************/
Tree.Init = function () {
    //1이면 우측 보기페이지에서 갱신처리한것
    if (ReLoad == "") {

        if (IsRoleTask == "False") {
            parent.frmContent.location = "/DeskPlusEIP/Task/Home/Main.aspx";
        }
        else {

            if (ItemID > 0) {
                //차트에서 이동시
                ParamArray = {
                    'itemID': ItemID,
                    'hStatus': HStatus,
                    'searchField': SearchField,
                    'searchDateValue': SearchDateValue

                };
                CallEncrypt(CallGetParam());

                var url = "/DeskPlusEIP/Task/Content/Total/List.aspx?p=" + GetEncryptValue;
                parent.frmContent.location = url;
            }
            else {
                if (HStatus == "") {
                    parent.frmContent.location = "/DeskPlusEIP/Task/Home/" + DefaultTab + ".aspx";
                }
                else {
                    //차트에서 이동시
                    ParamArray = {
                        'hStatus': HStatus,
                        'searchField': SearchField,
                        'searchDateValue': SearchDateValue

                    };
                    CallEncrypt(CallGetParam());

                    var url = "/DeskPlusEIP/Task/Content/Total/List.aspx?p=" + GetEncryptValue;
                    parent.frmContent.location = url;
                }
            }
        }

    }
}

/**************************************************
페이지 이동하기
***************************************************/
Tree.GoList = function (targetPage, itemID) {

    if (IsRoleTask == "True") {
        if (targetPage == "Home/Main.aspx") {
            //targetPage = "Home/AdminMain.aspx";
            targetPage = "Home/" + DefaultTab + ".aspx";
        }
    }

    var targetPage = "/DeskPlusEIP/Task/" + targetPage;

    if (targetPage != "") {
        ParamArray = {
            'itemID': itemID
        };
        CallEncrypt(CallGetParam());

        var url = targetPage + "?p=" + GetEncryptValue;
        parent.frmContent.location = url;
    }
}

/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {
    var targetPage = node.GetProperty("TargetPage");
    var itemID = node.GetProperty("ItemID");

    if (targetPage != "" && targetPage != undefined) {
        Tree.GoList(targetPage, itemID);
    }
}

/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-23
작성목적 : 작성하기
**********************************************************************************************/
Tree.Add = function () {
    ParamArray = {
        'reLoad': "1"
    };

    CallEncrypt(CallGetParam());

    parent.frmContent.location = "/DeskPlusEIP/Task/Content/Request/AddN.aspx?p=" + GetEncryptValue;
}

