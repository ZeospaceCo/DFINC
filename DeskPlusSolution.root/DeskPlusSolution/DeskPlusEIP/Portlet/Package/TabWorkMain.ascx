<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TabWorkMain.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.TabWorkMain" EnableViewState="false" %>
<script type="text/javascript">
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";

    $(document).ready(function () {

        //ContextMenu 생성하기
        var conMenu = '<ul id="basicMenu_<%=TabPortletID%>_<%=PortletID%>" class="contextMenu">';
        conMenu += '<li class="del"><a href="#del"><%=GetResource("DelPortlet")%></a></li>';        
        conMenu += '<li class="info"><a href="#info"><%=GetResource("PortletSetting")%></a></li>';
        conMenu += '</ul>';
        $("#MenuList").append(conMenu);

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 리스트행 개수
        $("#hdnTabPageSize_" + portletDom).val("<%=PageSize %>");
        $("#boardListCount_" + portletDom).val("<%=PageSize %>");

        var tabBPageSize = $("#hdnTabPageSize_" + portletDom).val();        

        if ("<%=IsAuth%>" != "True") {
            $("#basicMenu_<%=TabPortletID%>_<%=PortletID%> .del").remove();
        }
        

        // 포틀릿 설정
        $("#ptConfig_" + portletDom).contextMenu({ menu: 'basicMenu_<%=TabPortletID%>_<%=PortletID%>', leftButton: true }, function (action, el, pos) {
            var pID = el[0].id.replace('ptConfig_', '');
            Main.ContextMenuWork(pID, action, el, pos);

            // 리스트 행 개수 바인딩
            $("#boardListCount_" + portletDom).val($("#hdnTabPageSize_" + portletDom).val());
        });

        //Email 바로가기
        $("#portlet_" + portletDom + " .title").on("click", function (e) {
            e.preventDefault();

			   LoadSelTab_<%=TabPortletID%>_<%=PortletID%>("<%=TabPortletFirst %>");

            //parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webmail";
        });


        //새로고침
        $("#ptReload_" + portletDom).on("click", function (e) {

            e.preventDefault();

            var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
            $(this).html(loadingPath);

            setTimeout(function () {

                // 첫 페이지로 세팅
                $("#hdnTabCount_" + portletDom).val("1");
                //LoadBoardList_<%=TabPortletID%>_<%=PortletID%>();

                var loadingPath = "<img src='<%=BaseImgPath%>/Main/ico_ref.gif' />";
                $("#ptReload_" + portletDom).html(loadingPath);

            }, 1);
        });

        //바로가기
        $("#ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();
            var paramValue = $("#hdnTabSelParamValue_" + portletDom).val();
            if(paramValue != ""){
                //menuCatID=1^catID=57^dcparm=BOARD1
                var paramValueStr = paramValue.split("^");
                var menuCatID = "";
                var catID = "";
                var dcparm = "";

                for(var i=0;i<paramValueStr.length;i++)
                {
                    switch(paramValueStr[i].split("=")[0])
                    {
                        case "menuCatID" :
                            menuCatID = paramValueStr[i].split("=")[1];
                            break;
                        case "catID" :
                            catID = paramValueStr[i].split("=")[1];
                            break;
                        case "dcparm" :
                            dcparm = paramValueStr[i].split("=")[1];
                            break;
                    }
                }
                
                ParamArray = {
                    'menuCatID': menuCatID,
                    'catID': catID,
                    'dcparm': dcparm
                };

                CallEncrypt(CallGetParam());            
                location = "/DeskPlusEIP/Board/Default.aspx?p=" + GetEncryptValue;
            }
        });

        //환경설정 저장
        $("#cbtnConfirm_" + portletDom).click(function (e) {
            e.preventDefault();

            // 리스트 행 개수 설정
            tabBPageSize = $("#boardListCount_" + portletDom).val();
            $("#hdnTabPageSize_" + portletDom).val(tabBPageSize);
            
            var mailbox = "";
            if($("#hdnTabSelModule_" + portletDom).val() == "mailbox") mailbox = $("#mailBox_<%=TabPortletID%>_<%=PortletID%>").val();

            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", tabBPageSize, mailbox);

            // 첫 페이지로 세팅
            $("#hdnTabCount_" + portletDom).val("1");
            //LoadBoardList_<%=TabPortletID%>_<%=PortletID%>();
            LoadSelTab_<%=TabPortletID%>_<%=PortletID%>("<%=TabPortletFirst %>");

            $("#ptListConfig_" + portletDom).slideToggle(600);
        });


        // 환경설정 닫기
        $("#cbtnClose_" + portletDom).click(function (e) {
            e.preventDefault();
            $("#ptListConfig_" + portletDom).slideToggle(600);
        });

        //$("#ptListConfig_" + portletDom).hide();

        $("#hdnTabCount_" + portletDom).val("1"); 
        
        //setTimeout(function () {
		        LoadSelTab_<%=TabPortletID%>_<%=PortletID%>("<%=TabPortletFirst %>");
		 //   }, 1);          

        /*
	    if ($.browser.msie) {
		        LoadSelTab_<%=TabPortletID%>_<%=PortletID%>("<%=TabPortletFirst %>");
	    }
	    else
	    {
		    setTimeout(function () {
		        LoadSelTab_<%=TabPortletID%>_<%=PortletID%>("<%=TabPortletFirst %>");
		    }, 1);        
	    }
        */
    });


    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 탭선택시
    ***************************************************/
    function LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(p_portletID)
    {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        $("#seltemptab_" + portletDom + "_" + $("#hdnTabSelID_" + portletDom).val()).attr('class','');        
        $("#seltemptab_" + portletDom + "_" + p_portletID).attr('class','on hovercol');
        // 첫 페이지로 세팅
        $("#hdnTabCount_" + portletDom).val("1"); 
        $("#hdnTabSelID_" + portletDom).val(p_portletID); 

        // 앨범형태이면 하단의 페이징이 보이지 않아야함
        if($("#hdnTabUrl_" + portletDom).val() == "3")
        {
            $(".ptl_more").hide();
        }
        else
        {
            $(".ptl_more").show();
        }

        //선택 탭 파라미터 정의
        LoadSelTabParam_<%=TabPortletID%>_<%=PortletID%>(p_portletID);
        //선택 탭 총 카운트
        //LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>();
        //선택 탭 리스트
        LoadTabList_<%=TabPortletID%>_<%=PortletID%>();
        
    }


    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 선택한 탭별 파리미터 설정
    ***************************************************/
    function LoadSelTabParam_<%=TabPortletID%>_<%=PortletID%>(p_portletID)
    {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        //203:categoryID=1^pageSize=1,204:listcount=5
        var paramValueList = $("#hdnTabParamValue_" + portletDom).val();  
        
        //203:웹서비스경로,204:웹서비스경로
        var urlValue = $("#hdnUrlValue_" + portletDom).val();

        //카테고리값 초기화
        $("#hdnTabCategoryID_" + portletDom).val("");
        
        if(paramValueList != "")
        {
            //초기화
            $("#hdnTabCategoryID_" + portletDom).val("");
            $("#hdnTabUrl_" + portletDom).val("");
            $("#hdnTabSelModule_" + portletDom).val("");
            //$("#hdnTabPageSize_" + portletDom).val("");
            $("#hdnTabSelJTList_" + portletDom).val("");

            var paramValueListStr = paramValueList.split(",");
            //203:categoryID=1
            for (var i = 0; i < paramValueListStr.length; i++) {
                var portletID = paramValueListStr[i].split(":")[0];
                
                if(portletID == p_portletID)
                {
                    //catID=2^listType=1
                    var paramValue = paramValueListStr[i].split(":")[1];  

                    $("#hdnTabSelParamValue_" + portletDom).val(paramValue);
                    
                    //menuCatID=1                  
                    var paramValueStr = paramValue.split("^");                    
                    
                    for (var j = 0; j < paramValueStr.length; j++) {
                        var keyValue = paramValueStr[j].split("=");
                        if(keyValue[0] == "catID")
                        {
                           //게시판 카테고리값
                            $("#hdnTabCategoryID_" + portletDom).val(keyValue[1]);
                        } else if(keyValue[0] == "listType")
                        {
                           //list 형태 :리스트(1);블로그(2);갤러리(3)
                            $("#hdnTabUrl_" + portletDom).val(keyValue[1]);
                        } else if(keyValue[0] == "module")
                        {
                           //module (board, mail.....)
                            $("#hdnTabSelModule_" + portletDom).val(keyValue[1]);
                        } else if(keyValue[0] == "jtListUrl")
                        {
                           //jt html 경로
                            $("#hdnTabSelJTList_" + portletDom).val(keyValue[1]);
                        }
                    }
                }
            }            
        }
        
        //203:웹서비스경로,204:웹서비스경로
        if(urlValue != "")
        {
            var urlValueStr = urlValue.split(",");
            for(var i = 0; i < urlValueStr.length; i++)
            {
                var portletID = urlValueStr[i].split(":")[0];

                if(portletID == p_portletID)
                {
                    //웹서비스경로값 설정
                    $("#hdnTabSelWS_" + portletDom).val( urlValueStr[i].split(":")[1]);
                }
            }
        }

        //mailbox 환경값 설정하기
        $("#showMailBox_" + portletDom).hide();
        if($("#hdnTabSelModule_" + portletDom).val() == "mailbox") $("#showMailBox_" + portletDom).show();           
    }


    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 총카운트 가져오기 (미사용)
    ***************************************************/
    function LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        
        //게시판 리스트
        var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/TabMain.asmx/GetPageTotCount";

        ParamArray = {            
            'catID': $("#hdnTabCategoryID_" + portletDom).val(),
            'module': $("#hdnTabSelModule_" + portletDom).val()
        };

        //alert($("#hdnTabSelModule_" + portletDom).val());

        CallEncrypt(CallGetParam());
        var paramData = "{p:'" + GetEncryptValue + "'}";
        
        $.ajax({
            type: "POST",
            url: urlPage,
            async: false,
            data: paramData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $("#hdnTabTotCount_" + portletDom).val(data.d)
            }
            ,
            error: function (data, status, err) {
                alert(R_ErrorConnection + data);
            }
        });        
    }


    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 페이지 가져오기
    ***************************************************/
    function LoadTabList_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        var module = $("#hdnTabSelModule_" + portletDom).val();
        var urlPage = $("#hdnTabSelWS_" + portletDom).val();        
        
        ParamArray = {
            'pageSize': $("#hdnTabPageSize_" + portletDom).val(),     // 리스트 행 개수
            'pageSizeNo': $("#hdnTabCount_" + portletDom).val(),      // 페이지Nuumber
            'totalCount': $("#hdnTabTotCount_" + portletDom).val() // 전체 페이지 카운트
        };
        
        CallEncrypt(CallGetParam());
        var paramData = "{p:'" + GetEncryptValue + "'}";
        
        $.ajax({
            type: "POST",
            url: urlPage,
            async: false,
            data: paramData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                var jtListUrl =  $("#hdnTabSelJTList_" + portletDom).val(); //jtList 경로 반환                    
                $('#plList_' + portletDom).setTemplateURL(jtListUrl + "?seq=" + Guids(), null, { filter_data: false });
                $('#plList_' + portletDom).processTemplate(data.d);
                
                if($(".NoListData") != undefined) {                    
                    $(".NoListData").text(R_NoContent);
                }
                
                if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
                if ($(".IconShort") != undefined) { $(".IconShort").attr('src', ImagePath + '/Icon/icon_comment.gif'); }
            }
            ,
            error: function (data, status, err) {
                alert(R_ErrorConnection + data);
            }
        });

        
    }

    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 포틀릿 페이징(arg; next(이후), before(이전))
    ***************************************************/
    function TabBList_Change_<%=TabPortletID%>_<%=PortletID%>(arg) {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        var boardList_PageSize = $("#hdnTabPageSize_" + portletDom).val();        // 리스트 행 개수
        var boardList_Count = $("#hdnTabCount_" + portletDom).val();              // 페이지Nuumber
        var boardList_TotalCount = $("#hdnTabTotCount_" + portletDom).val();    // 전체 페이지 카운트   
        
        // MAX페이지 가져오기
        var boardList_MaxCount = 1;
        if (boardList_TotalCount > 0) {

            // 몫
            var mok = parseInt((boardList_TotalCount / boardList_PageSize));
            // 나머지
            var balance = boardList_TotalCount % boardList_PageSize;

            if (balance != 0) {
                boardList_MaxCount = mok + 1;                
            }
            else {
                boardList_MaxCount = mok;
            }
        }        

        if (arg == "next") {
            boardList_Count = parseInt(boardList_Count) + 1;
        }
        else if (arg == "before") {
            boardList_Count = parseInt(boardList_Count) - 1;
        }
        
        // 1페이지에서 이전을 누를 때
        if (boardList_Count < 1) {
            boardList_Count = 1;
            $("#hdnTabCount_" + portletDom).val(boardList_Count);
            alert(R_FirstPage);
            return;
        }

        // 최종페이지 에서 이후버튼 누를 때 
        if (boardList_Count > boardList_MaxCount) {            
            boardList_Count = boardList_MaxCount;
            $("#hdnTabCount_" + portletDom).val(boardList_Count);

            alert(R_LastPage);
            return;
        }

        // 현재 페이지Nuumber 세팅
        $("#hdnTabCount_" + portletDom).val(boardList_Count);

        setTimeout(function () {
            //LoadSelTab_<%=TabPortletID%>_<%=PortletID%>("<%=TabPortletFirst %>");
            LoadTabList_<%=TabPortletID%>_<%=PortletID%>();
        }, 1);
    }  

    /**************************************************
    작성자   : 
    작성일자 : 2011-05-10
    작성목적 : 모달창으로 게시판 댓글리스트 보기
    ***************************************************/
    function TabWorkMain_PopupBoardViewShort(p_categoryID, p_listID) {

        ParamArray = {
            'categoryID': p_categoryID,
            'listID': p_listID
        };

        CallEncrypt(CallGetParam());

        //var url = "/DeskPlusEIP/Board/Content/ShortReplyList.aspx?p=" + GetEncryptValue;
        var url = "/DeskPlusEIP/Board/Content/ViewShortReplyList.aspx?p=" + GetEncryptValue;
        //PopupWin(800, 690, url, "", "", true); 
        WindowObjectReference = PopupWinOpen(url, "", "ViewShortReplyList", "600", "690", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
    }


    /**************************************************
    작성자   : shimpark
    작성일자 : 2011-04-11
    작성목적 : 새창으로 게시물 보기
    ***************************************************/
    function TabWorkMain_PopupBoardViewList (p_categoryID, p_listID) {

        ParamArray = {
            'catID': p_categoryID,
            'categoryID': p_categoryID,
            'listID': p_listID,
            'currentPage': 1,
            'searchGroup': '',
            'searchField': '',
            'searchValue': '',
            'order': ''
        };

        CallEncrypt(CallGetParam());
        
        var url = "/DeskPlusEIP/Board/Content/ViewPopup.aspx";
        var param = "p=" + GetEncryptValue;
        WindowObjectReference = PopupWinOpen(url, param, "PopupBoardViewList", "1200", "800")
    }

    /**************************************************
    작성자   : famo23
    작성일자 : 2012-12-13
    작성목적 : 일정보기
    ***************************************************/
    function TabWorkMain_PopupScheduleViewList(p_companyID, p_categoryID, p_listID) {

        ParamArray = {
            'CompanyID': p_companyID,
            'ScheduleID': p_listID,
            'DisplayID': p_categoryID            
        };

        CallEncrypt(CallGetParam());
        
        var url = "/DeskPlusEIP/Pim/Schedule/ViewPopup.aspx";
        var param = "p=" + GetEncryptValue;
        WindowObjectReference = PopupWinOpen(url, param, "PopupScheduleViewList", "1200", "800", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
    }

    /**************************************************
    작성자   : famo23
    작성일자 : 2012-12-14
    작성목적 : 할일보기
    ***************************************************/
    function TabWorkMain_PopupToDoViewList(p_listID) {

        ParamArray = {
            'TodoID': p_listID       
        };

        CallEncrypt(CallGetParam());
        
        var url = "/DeskPlusEIP/Pim/ToDo/ViewPopup.aspx";
        var param = "p=" + GetEncryptValue;
        WindowObjectReference = PopupWinOpen(url, param, "PopupToDoViewList", "1200", "800", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
    }

    

    /**************************************************
    작성자   : shimpark
    작성일자 : 2011-04-11
    작성목적 : 모달창 일정댓글 보기
    ***************************************************/
    function TabWorkMain_PopupScheduleShort(p_scheduleID) { 

        ParamArray = { 
            'scheduleID': p_scheduleID 
        }; 
 
        CallEncrypt(CallGetParam());
        
        var url = "/DeskPlusEIP/Pim/Schedule/ShortReplyList.aspx?p=" + GetEncryptValue;
        var param = "p=" + GetEncryptValue;
        PopupWin(700, 500, url, "", "", true);
    }

    /**************************************************
    작성자   : shimpark
    작성일자 : 2011-04-11
    작성목적 : 모달창 할일댓글 보기
    ***************************************************/
    function TabWorkMain_PopupToDoShort(p_toDoID) { 

        ParamArray = { 
            'toDoID': p_toDoID 
        }; 
 
        CallEncrypt(CallGetParam());
        
        var url = "/DeskPlusEIP/Pim/Todo/ShortReplyList.aspx?p=" + GetEncryptValue; 
        var param = "p=" + GetEncryptValue;
        PopupWin(700, 500, url, "", "", true);
    }

    /**************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 문서보기(결재진행중/완료)
    ***************************************************/
    function TabWorkMain_PopupAppViewList(p_docID) { //alert(p_docID);

        ParamArray = { 'documentID': p_docID };
        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx";
        var param = "p=" + GetEncryptValue;
        WindowObjectReference = PopupWinOpen(url, param, "SHOWDOC", "1000", "700")
    }
    /**************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 문서보기(결재진행중/완료)
    ***************************************************/
    function TabWorkMain_PopupAppShort(p_docID, p_isComp, p_groupID, p_accountTag) { //alert(p_docID + "," + p_isComp);

        ParamArray = {
            'docID': p_docID
            ,'groupID' : p_groupID
            ,'accountTag' : p_accountTag
            ,'isComp' : p_isComp
         };
        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Approval/Common/Popup/ShortReplyList.aspx?p=" + GetEncryptValue; 
        PopupWin(700, 500, url, "", "", true);
    }
</script>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.rolling.js"></script>

<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">    
    <div class="pomailbox bg"> 

	    <div class="mailmenubox">
            <!-- 상단 타이틀 //-->
            <ul class="mailmenu headtitle">
            <li class="title" style="cursor:pointer;"><%=Title%></li>
            <%=TabPortletTitle %>
            <li class="poset"><a class="callContextMenu" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>"><img src="<%=BaseImgPath%>/Package/poset.gif" alt="<%=GetResource("Setting")%>" title="<%=GetResource("Setting")%>" /></a></li>
            </ul>
        </div>

        <div class="maillistline">
            <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="maillistbox">
                <!-- 환경설정 //-->
                <div id="ptListConfig_<%=TabPortletID%>_<%=PortletID%>" style="display:none;">
                    <li class="poboxlistset">
                        <span class="listsetbox">
                            <div id="showMailBox_<%=TabPortletID%>_<%=PortletID%>">
                                <select id="mailBox_<%=TabPortletID%>_<%=PortletID%>"></select>
                            </div>                                          
                            <select id="boardListCount_<%=TabPortletID%>_<%=PortletID%>">
                                <option title="5" value="5">5 <%=GetResource("ListRowCnt")%></option>
                                <option title="10" value="10">10 <%=GetResource("ListRowCnt")%></option>
                                <option title="20" value="20">20 <%=GetResource("ListRowCnt")%></option>
                                <option title="30" value="30">30 <%=GetResource("ListRowCnt")%></option>
                            </select> 
                        </span>
                        <span class="listsetbtn">
                            <a id="cbtnConfirm_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("Save")%></span></a>
                        </span>
                    </li>
                </div>
                <!-- 내용 //-->
                <ul class="pomailbox">
                    <div id="plList_<%=TabPortletID%>_<%=PortletID%>">
                        <div style="padding-top:50px;text-align:center;">
                            <img src="<%=BaseImgPath%>/pims/loading.gif" alt="Loading..." title="Loading..." />
                        </div>
                    </div>
                </ul>
                <div style="display: none">
                    <input type="hidden" id="hdnTabCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabCount_<%=TabPortletID%>_<%=PortletID%>" />
                    <input type="hidden" id="hdnTabCategoryID_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabCategoryID_<%=TabPortletID%>_<%=PortletID%>" />
                    <input type="hidden" id="hdnTabTotCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabTotCount_<%=TabPortletID%>_<%=PortletID%>" />
                    <input type="hidden" id="hdnTabPageSize_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabPageSize_<%=TabPortletID%>_<%=PortletID%>" />                
                    <input type="hidden" id="hdnTabParamValue_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabParamValue_<%=TabPortletID%>_<%=PortletID%>" value="<%=ParamValueList %>" />
                    <input type="hidden" id="hdnUrlValue_<%=TabPortletID%>_<%=PortletID%>" name="hdnUrlValue_<%=TabPortletID%>_<%=PortletID%>" value="<%=UrlValueList %>" />
                    <input type="hidden" id="hdnTabUrl_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabUrl_<%=TabPortletID%>_<%=PortletID%>" />
                    <input type="hidden" id="hdnTabSelID_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSelID_<%=TabPortletID%>_<%=PortletID%>" />
                    <input type="hidden" id="hdnTabSelParamValue_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSelParamValue_<%=TabPortletID%>_<%=PortletID%>" />
                    <input type="hidden" id="hdnTabSelWS_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSelWS_<%=TabPortletID%>_<%=PortletID%>" />
                    <input type="hidden" id="hdnTabSelModule_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSelModule_<%=TabPortletID%>_<%=PortletID%>" />
                    <input type="hidden" id="hdnTabSelJTList_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSelJTList_<%=TabPortletID%>_<%=PortletID%>" />
                </div>
            </div>
        </div>

    </div>
</div>
<!-- 포틀릿(리스트:일반형) End //-->
