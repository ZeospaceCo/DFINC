
GetPortlet = {}

$(function () {
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            GetPortlet.LoadData("");
        }
    }

    $(document).ready(function () {

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            recordPerPage: RecordPerPage,
            currentPage: CurrentPage,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            GetPortlet.Close();
            //if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });
        //페이지 카운트
        $('#ddlListCount').change(function () {
            GetPortlet.ModPageSize();

        });

        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            //event: "mouseover",
            active: AccordionActive,
            overTop: 180

        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () {
                $("#accordion").accordion("resize");
            }
        });
        
        //관리자 텝이면,개인용포틀릿 화면 감추기
        if (IsAdmin == "1") {
            $("#privateMenu").hide();
            $("#privateMenuSub").hide();
        }

        GetPortlet.LoadData("");
    });
});



/**************************************************
    공용 포틀릿카테고리 트리 onNodeSelect 트리 노드 선택
***************************************************/
function CommonPortletTree_onNodeSelect(node) {
    Gubun = "COMMON"; //공용:COMMON; 개인:PRIVATE
    CatID = node.ID;
    CurrentPage = 1;     //1페이지로 이동처리  
    GetPortlet.LoadData("search");
}


/**************************************************
    개인 포틀릿카테고리 트리 onNodeSelect 트리 노드 선택
***************************************************/
function PrivatePortletTree_onNodeSelect(node) {
    Gubun = "PRIVATE"; //공용:COMMON; 개인:PRIVATE
    CatID = node.ID;
    CurrentPage = 1;     //1페이지로 이동처리  
    GetPortlet.LoadData("search");
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
GetPortlet.LoadData = function (p_gubun) {
    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        GetPortlet.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        GetPortlet.GetCount(); //검색 총 카운트 가져오기
        GetPortlet.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                GetPortlet.LoadData("search");
            }
        }
        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            recordPerPage: RecordPerPage,
            currentPage: CurrentPage,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });
    }

    setTimeout(function () {
        if (p_gubun == "search") {
            loadBoardSearchData();
        }
        else {
            loadBoardData();
        }
    }, 100);
}


/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-18
작성목적 : 웹서비스 포틀릿 카운트  호출
**********************************************************************************************/
GetPortlet.GetCount = function () {
    var url = "";

    if (Gubun == "PRIVATE") {
        url = "/DeskPlusEIP/EIP/Ws/Portal/AddPortletUser.asmx/GetListCount";
        ParamArray = {
            'categoryID': CatID,
            'searchKey': '',
            'searchValue': '',
            'tabID': TabID
        };
    } else {
        url = "/DeskPlusEIP/EIP/Ws/Portal/GetPortlet.asmx/GetListCount";
        ParamArray = {
            'categoryID': CatID,
            'tabID': TabID,
            'searchKey': '',
            'searchValue': ''
        };
    }

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("GetPortlet.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
GetPortlet.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
}


/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-18
작성목적 : 웹서비스 포틀릿 리스트 호출
**********************************************************************************************/
GetPortlet.GetPageList = function () {
    var url = "";
    
    if (Gubun == "PRIVATE") {
        url = "/DeskPlusEIP/EIP/Ws/Portal/AddPortletUser.asmx/GetList";

        ParamArray = {
            'totalCount': TotalItemCount,
            'pageSize': RecordPerPage,
            'pageSizePageNo': CurrentPage,
            'categoryID': CatID,
            'searchKey': '',
            'searchValue': '',
            'order': '',
            'tabID':TabID
        };
    }
    else {
        url = "/DeskPlusEIP/EIP/Ws/Portal/GetPortlet.asmx/GetList";

        ParamArray = {
            'totalCount': TotalItemCount,
            'pageSize': RecordPerPage,
            'pageSizePageNo': CurrentPage,
            'categoryID': CatID,
            'tabID': TabID,
            'searchKey': '',
            'searchValue': '',
            'order': ''
        };
    }    

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("GetPortlet.SuccessPageList");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-02-14
작성목적 : 성공시 리스트 표시
**********************************************************************************************/
GetPortlet.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/EIP/Jt/Portal/JTGetPortlet.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    $('select').html(Options);
    GetPortlet.Resource();
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-02-14
작성목적 : 다국어
**********************************************************************************************/
GetPortlet.Resource = function () {
    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }

    $('.DirectAdd').text(R_DirectAdd);
}

/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}


/**************************************************
작성자:
작성일:2011-11-14
작성목적 : 포틀릿 추가하기
***************************************************/
GetPortlet.Confirm = function (portletID, selectID) {

    var url = "/DeskPlusEIP/EIP/Ws/Portal/GetPortlet.asmx/AddTabPortlet";
    var selColID = $("#" + selectID + " option:selected").val();

    //파라미터값 배열에 담기
    ParamArray = {
        'tabID': TabID,
        'columnID': selColID,
        'portletID': portletID
    };

    var obj = eval("GetPortlet.SuccessConfirm");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자:
작성일:2011-11-14
작성목적 : 성공시, 포틀릿 추가하기
***************************************************/
GetPortlet.SuccessConfirm = function (data, status) {
    if (data.d == true) {
        alert(R_AddPortletCheckedTab);
        GetPortlet.LoadData("search");
    }
    else {
        alert(R_ProcessingError);
    }
}

/**************************************************
작성자:
작성일:2011-11-14
작성목적 : 닫기
***************************************************/
GetPortlet.Close = function () {
    
    ParamArray = {
        'tabID': TabID
    };
    CallEncrypt(CallGetParam());

    parent.location = "/DeskPlusEIP/EIP/Package/NMain.aspx?p=" + GetEncryptValue;

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}


/**************************************************
작성자   : shine
작성일자 :2011-06-15
작성목적 : 페이지 사이즈 설정
***************************************************/
GetPortlet.ModPageSize = function () {
    RecordPerPage = $('#ddlListCount').val();
    CurrentPage = 1; //1페이지로 이동처리  
    GetPortlet.LoadData("search");
}

