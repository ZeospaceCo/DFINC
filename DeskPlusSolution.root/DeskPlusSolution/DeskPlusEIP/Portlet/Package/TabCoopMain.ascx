<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TabCoopMain.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.TabCoopMain" EnableViewState="false" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
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
        
        $("#hdnTabPageSize_" + portletDom).val("<%=PageSize %>");
        $("#boardListCount_" + portletDom).val("<%=PageSize %>");
        $("#ptListConfigBG_" + portletDom).hide();
        $("#ptListConfig_" + portletDom).hide();        
        $("#<%=ctbSearch.ClientID %>").focus();

        var tabBPageSize = $("#hdnTabPageSize_" + portletDom).val();        

        if ("<%=IsAuth%>" != "True") {
            $("#basicMenu_<%=TabPortletID%>_<%=PortletID%> .del").remove();
        }        
        
        // 상위메뉴 - 포틀릿 환경설정
        $("#ptConfig_" + portletDom).contextMenu({ menu: 'basicMenu_<%=TabPortletID%>_<%=PortletID%>', leftButton: true }, function (action, el, pos) {
            var pID = el[0].id.replace('ptConfig_', '');
            TabCoopMain_ContextMenuWork_<%=TabPortletID%>_<%=PortletID%>(pID, action, el, pos);

            // 리스트 행 개수 바인딩
            $("#boardListCount_" + portletDom).val($("#hdnTabPageSize_" + portletDom).val());
        });

        // 검색화면 열고, 닫기
        $("#ptOption_" + portletDom).click(function (e) {
            e.preventDefault();
            $("#ptListConfigBG_" + portletDom).slideToggle("slow");
            $("#ptListConfig_" + portletDom).hide();

            var txtVal = $("#ptOption_" + portletDom)[0].innerHTML;	    
            if(txtVal == "&gt;")
            {
                $("#ptOption_" + portletDom)[0].innerHTML = "&lt;";
            } 
            else
            {
                $("#ptOption_" + portletDom)[0].innerHTML = "&gt;";
            }
        });

        // 상위메뉴 - 검색버튼
        $("#cbtnSearch_" + portletDom).click(function (e) {
            e.preventDefault();

            $("#hdnTabPageNo_" + portletDom).val("1"); 
            LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>();  
            LoadTabList_<%=TabPortletID%>_<%=PortletID%>();
        });

        //상위메뉴 - 검색입력필드 엔터 이벤트 기술
        $("#<%=ctbSearch.ClientID %>").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();

                $("#hdnTabPageNo_" + portletDom).val("1"); 
                LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>();
                LoadTabList_<%=TabPortletID%>_<%=PortletID%>();
            }
        });

        // 서브메뉴 - 저장 (페이지카운트)
        $("#cbtnConfirm_" + portletDom).click(function (e) {
            e.preventDefault();

            //페이지 사이즈 할당하기
            tabBPageSize = $("#boardListCount_" + portletDom).val();
            $("#hdnTabPageSize_" + portletDom).val(tabBPageSize);

            var tabFirstlID = $("#hdnTabFirst_" + portletDom).val();//초기값 탭 값
            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", tabBPageSize, tabFirstlID);
            ClearSession_<%=TabPortletID%>_<%=PortletID%>();//세션초기화(pageSize 땜시)

            $("#hdnTabPageNo_" + portletDom).val("1"); 
            LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>();
            LoadTabList_<%=TabPortletID%>_<%=PortletID%>();

            $("#ptListConfigBG_" + portletDom).show();
            $("#ptListConfig_" + portletDom).hide();
        });


        // 서브메뉴 - 닫기
        $("#cbtnClose_" + portletDom).click(function (e) {
            e.preventDefault();
            $("#ptListConfigBG_" + portletDom).show();
            $("#ptListConfig_" + portletDom).hide();
        });

        // 서브메뉴 - 초기탭 설정
        $("#cbtnFirst_" + portletDom).click(function (e) {
            e.preventDefault();

            var tabPageSize = $("#hdnTabPageSize_" + portletDom).val();
            var tabSelID = $("#hdnTabSelID_" + portletDom).val();//선택한 탭
            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", tabPageSize, tabSelID);

            $("#hdnTabFirst_" + portletDom).val(tabSelID);//초기탭 값 변경하기
                        
            $("#ptListConfigBG_" + portletDom).show();
            $("#ptListConfig_" + portletDom).hide();
        });


        //새로고침
        $("#ptReload_" + portletDom).on("click", function (e) {
            e.preventDefault();

            var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
            $(this).html(loadingPath);
            
            setTimeout(function () {

                var response = ClearSession_<%=TabPortletID%>_<%=PortletID%>();

                if(response)
                {
                    $("#hdnTabPageNo_" + portletDom).val("1"); 
                    LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>();  
                    LoadTabList_<%=TabPortletID%>_<%=PortletID%>();

                    var loadingPath = "<img src='<%=BaseImgPath%>/Main/ico_ref.gif' />";
                    $("#ptReload_" + portletDom).html(loadingPath);
                }

            }, 1);            
        });   
	
	 //바로가기
        $("#ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();            
            //parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webmail";

	     parent.frmTop.Menu.GoParam("coop"); 
             document.location.href = "/DeskPlusEIP/Coop/Default.aspx";
        });

        var tabFirstID = $("#hdnTabFirst_" + portletDom).val();
	    if ($.browser.msie) {
		    LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(tabFirstID);
	    }
	    else
	    {
		    setTimeout(function () {
		        LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(tabFirstID);
		    }, 1);        
	    }
    });

    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 세션 초기화
    ***************************************************/
    function ClearSession_<%=TabPortletID%>_<%=PortletID%>()
    {
        var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Coop/List.asmx/ClearSession";
        var response = false;

        $.ajax({
            type: "POST",
            url: urlPage,
            async: false,
            data: "",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

               response = true;
            },
            error: function (data, status, err) {
                alert(R_ErrorConnection + data);
            }
        });

        return response;
    }


    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 탭선택시
    ***************************************************/
    function LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(p_portletID)
    {
        $("#<%=ctbSearch.ClientID %>").focus();
        $("#<%=ctbSearch.ClientID %>").val("");

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        $("#seltemptab_" + portletDom + "_" + $("#hdnTabSelID_" + portletDom).val()).attr('class','');        
        $("#seltemptab_" + portletDom + "_" + p_portletID).attr('class','on hovercol');        
        
        $("#hdnTabSelID_" + portletDom).val(p_portletID);         

        //선택 탭 파라미터 정의
        LoadSelTabParam_<%=TabPortletID%>_<%=PortletID%>(p_portletID);     
        
        $("#hdnTabPageNo_" + portletDom).val("1"); 
        LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>();   
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

                        if(keyValue[0] == "jtListUrl")
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
    }


    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 페이지 가져오기
    ***************************************************/
    function LoadTabList_<%=TabPortletID%>_<%=PortletID%>() {
    
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        var urlPage = $("#hdnTabSelWS_" + portletDom).val();        
        
        ParamArray = {
            'pageSize': $("#hdnTabPageSize_" + portletDom).val(),     // 페이지 사이즈
            'pageSizeNo': $("#hdnTabPageNo_" + portletDom).val(),      // 페이지 번호
            'totalCount': $("#hdnTabTotCount_" + portletDom).val(), // 전체 페이지 카운트
            'searchStatus': $("#ptListSrhStatus_" + portletDom).val(), // 검색상태
            'searchField': $("#ptListSrhField_" + portletDom).val(), // 검색필드
            'searchValue': $("#<%=ctbSearch.ClientID %>").val() //검색어
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
    작성목적 : 총카운트 가져오기
    ***************************************************/
    function LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        //게시판 리스트
        var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Coop/List.asmx/GetCount";
        var selWs =  $("#hdnTabSelWS_" + portletDom).val(); //jtList 경로 반환

        var gubun = "";
        if (selWs.indexOf("GetRequestList") != -1) {
            gubun = "REQUEST";
        }else if (selWs.indexOf("GetTOList") != -1) {
            gubun = "TO";
        }else if (selWs.indexOf("GetCCList") != -1) {
            gubun = "CC";
        }else if (selWs.indexOf("GetForwardList") != -1) {
            gubun = "FORWARD";
        }

        ParamArray = {            
            'gubun': gubun,
            'searchStatus': $("#ptListSrhStatus_" + portletDom).val(), // 검색상태
            'searchField': $("#ptListSrhField_" + portletDom).val(), // 검색필드
            'searchValue': $("#<%=ctbSearch.ClientID %>").val() //검색어
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
    작성목적 : 포틀릿 페이징(arg; next(이후), before(이전))
    ***************************************************/
    function TabBList_Change_<%=TabPortletID%>_<%=PortletID%>(arg) {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        var boardList_PageSize = $("#hdnTabPageSize_" + portletDom).val();        // 리스트 행 개수
        var boardList_Count = $("#hdnTabPageNo_" + portletDom).val();              // 페이지Nuumber
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
            $("#hdnTabPageNo_" + portletDom).val(boardList_Count);
            alert(R_FirstPage);
            return;
        }

        // 최종페이지 에서 이후버튼 누를 때 
        if (boardList_Count > boardList_MaxCount) {            
            boardList_Count = boardList_MaxCount;
            $("#hdnTabPageNo_" + portletDom).val(boardList_Count);

            alert(R_LastPage);
            return;
        }

        // 현재 페이지Nuumber 세팅
        $("#hdnTabPageNo_" + portletDom).val(boardList_Count);
	
	setTimeout(function () {            
	    LoadTabList_<%=TabPortletID%>_<%=PortletID%>();
	}, 1);
    }   
    

    
    /**************************************************
    작성자   : shimpark
    작성일자 : 2011-04-11
    작성목적 : 설정 이벤트
    ***************************************************/
    function TabCoopMain_ContextMenuWork_<%=TabPortletID%>_<%=PortletID%>(id, action, el, pos) {

        switch (action) {
            case "del":
                {
                    Main.DelTabPortlet(id);
                    if (PortletAuth == "True") {
                        $("#portlet_" + id).fadeOut("slow");
                    }
                    break;
                }        
            case "info":
                {                
                    $("#ptListConfigBG_" + id).hide();
                    $("#ptListConfig_" + id).show();
                    break;
                }
        }
    }  


    /**************************************************
    작성자   : shimpark
    작성일자 : 2011-04-11
    작성목적 : 업무협업
    ***************************************************/
    function TabCoopMain_PopupCoop(p_listID, p_groupID) {

        ParamArray = {
            'listID': p_listID,
            'groupID': p_groupID
        };

        CallEncrypt(CallGetParam());
        
        var url = "/DeskPlusEIP/Coop/Content/Total/ViewPopup.aspx?p=" + GetEncryptValue;        
        WindowObjectReference = window.open(url, "TabCoopMain_PopupCoop", "width=1000px,height=700px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
        WindowObjectReference.focus();
    }


     /**************************************************
    작성자   : shimpark
    작성일자 : 2011-04-11
    작성목적 : 업무협업 - 전달
    ***************************************************/
    function TabCoopMain_PopupCoopFWD(p_listID, p_groupID) {

        ParamArray = {
            'forwardListID': p_listID,
            'forwardGroupID': p_groupID
        };

        CallEncrypt(CallGetParam());
        
        var url = "/DeskPlusEIP/Coop/Content/Forward/ViewPopup.aspx?p=" + GetEncryptValue;        
        WindowObjectReference = window.open(url, "TabCoopMain_PopupCoopFWD", "width=1000px,height=700px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
        WindowObjectReference.focus();
    }

     /**************************************************
    작성자   : shimpark
    작성일자 : 2011-04-11
    작성목적 : 업무협업
    ***************************************************/
    function TabCoopMain_PopupReport(p_listID) {

        ParamArray = {
            'listID': p_listID
        };

        CallEncrypt(CallGetParam());
        
        var url = "/DeskPlusEIP/Report/Content/ViewPopup.aspx?p=" + GetEncryptValue;        
        WindowObjectReference = window.open(url, "TabCoopMain_PopupReport", "width=1000px,height=700px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
        WindowObjectReference.focus();
    }

     /**************************************************
    작성자   : shimpark
    작성일자 : 2011-04-11
    작성목적 : 자원관리
    ***************************************************/
    function TabCoopMain_PopupReserv(p_categoryID, p_reserveID) {

        ParamArray = {
            'categoryID': p_categoryID,
            'reservationID': p_reserveID,
            'tagGubun' : 'month'
        };

        CallEncrypt(CallGetParam());
        
        var url = "/DeskPlusEIP/RM/Content/ViewPopup.aspx?p=" + GetEncryptValue;        
        WindowObjectReference = window.open(url, "TabCoopMain_PopupReserv", "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
        WindowObjectReference.focus();
    }    

</script>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.rolling.js"></script>

<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">
    <div class="pomailbox bg">

	    <div class="mailmenubox">
            <!-- 상단 타이틀 //-->
            <ul class="mailmenu headtitle">
            <li class="title" style="cursor:pointer;">
                <a class="ptReload" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%></a>
                <span class="mcow_por" title="" id="ptOption_<%=TabPortletID%>_<%=PortletID%>" >></span>      
            </li>
            <%=TabPortletTitle %>
            <li class="poset"><a class="callContextMenu" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>"><img src="<%=BaseImgPath%>/Package/poset.gif" alt="<%=GetResource("Setting")%>" title="<%=GetResource("Setting")%>" /></a></li>
            </ul>
        </div>

        <div class="maillistline">
            <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="maillistbox">
                <!-- 환경설정 //-->
                <div id="ptListConfigBG_<%=TabPortletID%>_<%=PortletID%>"  style="display: none">
                    <li class="poboxlistset">                                                
                        <span class="listsetbox" style="padding-left:10px;">                        
                            <select id="ptListSrhStatus_<%=TabPortletID%>_<%=PortletID%>" name="ptListSrhStatus_<%=TabPortletID%>_<%=PortletID%>" style="margin:1px 5px 0px 0px ;">
                                 <option value=""><%=GetResource("AllPimSort")%></option>
                                 <option value="1"><%=GetResource("COOPStandby")%></option>
                                 <option value="2"><%=GetResource("COOPAccept")%></option>
                                 <option value="3"><%=GetResource("APPING")%></option>
                                 <option value="4"><%=GetResource("COOPComplete")%></option>  
                                 <option value="5"><%=GetResource("COOPReject")%></option>  
                            </select>
                            <select id="ptListSrhField_<%=TabPortletID%>_<%=PortletID%>" name="ptListSrhField_<%=TabPortletID%>_<%=PortletID%>" style="margin:1px 5px 0px 0px ;">
                                 <option value="title"><%=GetResource("Title")%></option>
                                 <option value="name"><%=GetResource("SearchName")%></option>
                            </select>
                        </span>

                        <span class="listsetbox" style="padding-left:10px;">                        
                            <cc3:ClientSmartTextBox ID="ctbSearch" CssClass="ptl_srhtext" InputMode="None" MaxLength="250" runat="server" Required="true" CssStyle="width:200px !important;"></cc3:ClientSmartTextBox>
                        </span>
                        <span style="vertical-align:bottom;">                        
                            <a id="cbtnSearch_<%=TabPortletID%>_<%=PortletID%>" href="#" title="<%=GetResource("Search")%>"><img src="/DeskPlusEIP/Design/C/Img/Icon/icon_search.gif" alt='' title='' /></a>
                            <a class="ptReload" id="ptReload_<%=TabPortletID%>_<%=PortletID%>"><img src="/DeskPlusEIP/Design/C/Img/Main/ico_ref.gif" alt="Reload" /></a>
                        </span>
                    </li>
                </div>
                <div id="ptListConfig_<%=TabPortletID%>_<%=PortletID%>" style="display: none">
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
                        <span class="listsetbtn" style="padding-left:10px;">
                            <a id="cbtnFirst_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("FirstTabSetting")%></span></a>
                        </span>
                        <span class="listsetbtn">
                            <a id="cbtnConfirm_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("Save")%></span></a>
                        </span>
                        <span class="listsetbtn">
                            <a id="cbtnClose_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("Close")%></span></a>
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
                <!-- 이전/이후 //-->
                <ul class="ptl_more">
                    <a onclick="TabBList_Change_<%=TabPortletID%>_<%=PortletID%>('before')"><img src="<%=BaseImgPath%>/Main/ico_b.gif" alt="<%=GetResource("Prev")%>" title="<%=GetResource("Prev")%>" /></a>
                    <a onclick="TabBList_Change_<%=TabPortletID%>_<%=PortletID%>('next')"><img src="<%=BaseImgPath%>/Main/ico_a.gif" alt="<%=GetResource("Next")%>" title="<%=GetResource("Next")%>" /></a>
                </ul>
                <div style="display: none">
                    <input type="hidden" id="hdnTabPageNo_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabPageNo_<%=TabPortletID%>_<%=PortletID%>" />
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
                    <input type="hidden" id="hdnTabFirst_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabFirst_<%=TabPortletID%>_<%=PortletID%>" value="<%=TabPortletFirst %>" />
                    
                </div>
            </div>
        </div>
    
    </div>
</div>
<!-- 포틀릿(리스트:일반형) End //-->

