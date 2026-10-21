<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tab.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Board.__Tab"  EnableViewState="false" %>
<script type="text/javascript">
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";

    $(document).ready(function () {

        //ContextMenu 생성하기
        var conMenu = '<ul id="basicMenu_<%=TabPortletID%>_<%=PortletID%>" class="contextMenu">';
        conMenu += '<li class="del"><a href="#del"><%=GetResource("DelPortlet")%></a></li>';
        conMenu += '<li class="min"><a href="#min"><%=GetResource("SmallPortlet")%></a></li>';
        conMenu += '<li class="max"><a href="#max"><%=GetResource("LargePortlet")%></a></li>';
        conMenu += '<li class="info"><a href="#info"><%=GetResource("PortletSetting")%></a></li>';
        conMenu += '</ul>';
        $("#MenuList").append(conMenu);

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 리스트행 개수
        $("#hdnTabPageSize_" + portletDom).val("<%=PageSize %>");
        var tabBPageSize = $("#hdnTabPageSize_" + portletDom).val();
        

        if ("<%=IsAuth%>" != "True") {
            $("#basicMenu_<%=TabPortletID%>_<%=PortletID%> .del").remove();
        }

        //최소화 여부
        if ("<%=IsMinimize %>" == "1") {
            $("#ptContent_" + portletDom).fadeOut("slow");
        }

        // 포틀릿 설정
        $("#ptConfig_" + portletDom).contextMenu({ menu: 'basicMenu_<%=TabPortletID%>_<%=PortletID%>', leftButton: true }, function (action, el, pos) {
            var pID = el[0].id.replace('ptConfig_', '');
            Main.ContextMenuWork(pID, action, el, pos);

            // 리스트 행 개수 바인딩
            $("#boardListCount_" + portletDom).val($("#hdnTabPageSize_" + portletDom).val());
        });

        //새로고침
        $("#ptReload_" + portletDom).on("click", function (e) {

            e.preventDefault();

            var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
            $(this).html(loadingPath);

            setTimeout(function () {

                // 첫 페이지로 세팅
                $("#hdnTabPageNo_" + portletDom).val("1");
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

        // 환경설정 저장
        $("#cbtnConfirm_" + portletDom).click(function (e) {
            e.preventDefault();

            //페이지 사이즈 할당하기
            tabBPageSize = $("#boardListCount_" + portletDom).val();
            $("#hdnTabPageSize_" + portletDom).val(tabBPageSize);

            var tabFirstlID = $("#hdnTabFirst_" + portletDom).val();//초기값 탭 값            
            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", tabBPageSize, tabFirstlID);

            // 첫 페이지로 세팅
            $("#hdnTabPageNo_" + portletDom).val("1");            
            LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>();            
            LoadTabList_<%=TabPortletID%>_<%=PortletID%>();

            $("#ptListConfig_" + portletDom).slideToggle(600);
        });


        // 환경설정 닫기
        $("#cbtnClose_" + portletDom).click(function (e) {
            e.preventDefault();
            $("#ptListConfig_" + portletDom).slideToggle(600);
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

        $("#hdnTabPageNo_" + portletDom).val("1");

	    if ($.browser.msie) {
		    var tabFirstID = $("#hdnTabFirst_" + portletDom).val();
		    LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(tabFirstID);
	    }
	    else
	    {
		    setTimeout(function () {
		        var tabFirstID = $("#hdnTabFirst_" + portletDom).val();
		        LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(tabFirstID);
		    }, 1);
	    }
    });




    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 탭선택시
    ***************************************************/
    function LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(p_portletID)
    {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        $("#selboardtab_" + portletDom + "_" + $("#hdnTabSelID_" + portletDom).val()).attr('class','');//모든 tab        
        $("#selboardtab_" + portletDom + "_" + p_portletID).attr('class','poboxon'); //선택한 tab     
        // 첫 페이지로 세팅
        $("#hdnTabPageNo_" + portletDom).val("1");  
        $("#hdnTabSelID_" + portletDom).val(p_portletID);
        
        LoadTabCategory_<%=TabPortletID%>_<%=PortletID%>(p_portletID);

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
        LoadSelTabParam_<%=TabPortletID%>_<%=PortletID%>();
        //선택 탭 총 카운트
        LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>();
        //선택 탭 리스트
        LoadTabList_<%=TabPortletID%>_<%=PortletID%>();

        

        
    }

    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 탭별 게시판의 파라미터값 설정하기
    ***************************************************/
    function LoadSelTabParam_<%=TabPortletID%>_<%=PortletID%>()
    {
    
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";        
        var selPortletID = $("#hdnTabSelID_" + portletDom).val();
        //포틀릿아이디:menuCatID=47,~
        var paramValueList = $("#hdnTabParamValue_" + portletDom).val();

        if(paramValueList != "")
        {
            //menuCatID=1^catID=57,~
            var paramValueListStr = paramValueList.split(",");
            for (var i = 0; i < paramValueListStr.length; i++) {
                var portletID = paramValueListStr[i].split(":")[0];
                
                if(portletID == selPortletID)
                {
                    //menuCatID=1^catID=57
                    var paramValue = paramValueListStr[i].split(":")[1];                      
                    $("#hdnTabSelParamValue_" + portletDom).val(paramValue);                    
                }
            }            
        }        
    }

    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 카테고리값 설정하기
    ***************************************************/
    function LoadTabCategory_<%=TabPortletID%>_<%=PortletID%>(p_portletID)
    {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        //포틀릿아이디:menuCatID=47,~
        var paramValueList = $("#hdnTabParamValue_" + portletDom).val();  
        
        //포틀릿아이디:페이지      
        var urlValue = $("#hdnUrlValue_" + portletDom).val();
        
        if(paramValueList != "")
        {
            //menuCatID=1^catID=57,~
            var paramValueListStr = paramValueList.split(",");
            for (var i = 0; i < paramValueListStr.length; i++) {
                var portletID = paramValueListStr[i].split(":")[0];
                
                if(portletID == p_portletID)
                {
                    //menuCatID=1^catID=57
                    var paramValue = paramValueListStr[i].split(":")[1];  
                    
                    //menuCatID=1                  
                    var paramValueStr = paramValue.split("^");                    
                    
                    for (var j = 0; j < paramValueStr.length; j++) {
                    
                        var keyValue = paramValueStr[j].split("=");
                        if(keyValue[0] == "catID")
                        {
                           //게시판 카테고리값
                            $("#hdnTabCategoryID_" + portletDom).val(keyValue[1]);
                            break;
                        }
                    }
                }
            }            
        }
        
        if(urlValue != "")
        {
            var urlValueStr = urlValue.split(",");
            for(var i = 0; i < urlValueStr.length; i++)
            {
                var portletID = urlValueStr[i].split(":")[0];

                if(portletID == p_portletID)
                {
                    var url = urlValueStr[i].split(":")[1];
                   
                    switch(url)
                    {
                        case "/DeskPlusEIP/Portlet/Package/Board/List.ascx":
                        {
                            $("#hdnTabUrl_" + portletDom).val("1");
                            break;
                        }
                        case "/DeskPlusEIP/Portlet/Package/Board/Blog.ascx":
                        {
                            $("#hdnTabUrl_" + portletDom).val("2");
                            break;
                        }
                        case "/DeskPlusEIP/Portlet/Package/Board/Album.ascx":
                        {
                            $("#hdnTabUrl_" + portletDom).val("3");
                            break;
                        }
                        default:
                        {
                            $("#hdnTabUrl_" + portletDom).val("1");
                            break;
                        }
                    }
                }
            }
        }
    }


    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 총카운트 가져오기
    ***************************************************/
    function LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        //게시판 리스트
        var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Board/Tab.asmx/GetBoardTotCount";

        ParamArray = {            
            'categoryID': $("#hdnTabCategoryID_" + portletDom).val()  // 카테고리 아이디
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
    작성목적 : 페이지 가져오기
    ***************************************************/
    function LoadTabList_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        var urlPage = "";
        
        switch($("#hdnTabUrl_" + portletDom).val())
        {
            case "1":
            urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Board/List.asmx/GetBoardList";   //게시판 리스트
            break;
            case "2":
            urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Board/Blog.asmx/GetBoardList";   //블로그 리스트
            break;
            case "3":
            urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Board/Album.asmx/GetBoardList";  //앨범 리스트
            break;
            default:
            urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Board/List.asmx/GetBoardList";   //게시판 리스트
            break;
        }
        
        ParamArray = {
            'pageSize': $("#hdnTabPageSize_" + portletDom).val(),     // 리스트 행 개수
            'pageSizeNo': $("#hdnTabPageNo_" + portletDom).val(),      // 페이지Nuumber
            'totalCount': $("#hdnTabTotCount_" + portletDom).val(), // 전체 페이지 카운트
            'categoryID': $("#hdnTabCategoryID_" + portletDom).val()  // 카테고리 아이디
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
                switch($("#hdnTabUrl_" + portletDom).val())
                {
                    case "1":       // 리스트형
                        {
                            $('#plList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/Board/JTList.htm?seq=" + Guids(), null, { filter_data: false });
                            $('#plList_' + portletDom).processTemplate(data.d); 
                            break;
                        }
                    case "2":       // 블로그형
                        {
                            $('#plList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/Board/JTBlog.htm?seq=" + Guids(), null, { filter_data: false });
                            $('#plList_' + portletDom).processTemplate(data.d); 
                            break;
                        }
                    case "3":       // 앨범형
                        {
                            $('#plList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/Board/JTAlbum.htm?seq=" + Guids(), null, { filter_data: false });
                            $('#plList_' + portletDom).processTemplate(data.d); 
                            
                             $("#plList_" + portletDom + " .imageSlider .imageList").jCarouselLite({
                                btnNext: "#plList_" + portletDom + " .imageSlider .right",
                                btnPrev: "#plList_" + portletDom + " .imageSlider .left",
                                visible: Math.floor($("#plList_" + portletDom).width() / 160),
                                speed: 800
                            });
                            break;
                        }
                    default:
                        {
                            $('#plList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/Board/JTList.htm?seq=" + Guids(), null, { filter_data: false });
                            $('#plList_' + portletDom).processTemplate(data.d); 
                            break;
                        }
                }
                
                if($(".NoListData") != undefined) {                    
                    $(".NoListData").text(R_NoContent);
                }

                if ($(".TitleIcon") != undefined) { $(".TitleIcon").attr('src', ImagePath + '/main/ico_bullet2.gif'); }
                if ($(".PtlType3Left") != undefined) { $(".PtlType3Left").attr('src', ImagePath + '/Main/ptl_laytype3_left.gif'); }
                if ($(".IconNoImg") != undefined) { $(".IconNoImg").attr('src', ImagePath + '/Icon/ico_noimg2.gif'); }
                if ($(".PtlType3Right") != undefined) { $(".PtlType3Right").attr('src', ImagePath + '/Main/ptl_laytype3_right.gif'); }   
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
            //선택 탭 리스트
            LoadTabList_<%=TabPortletID%>_<%=PortletID%>();
        }, 1);
    } 
    
    /**************************************************
    작성자   : 
    작성일자 : 2011-05-10
    작성목적 : 새창으로 댓글리스트 보기
    ***************************************************/
    function TabBoardShort(p_categoryID, p_listID) {
    //alert(p_categoryID);
    //alert(p_listID);
        ParamArray = {
            'categoryID': p_categoryID,
            'listID': p_listID
        };

        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Board/Content/ShortReplyList.aspx?p=" + GetEncryptValue;
        PopupWin(600, 420, url, "", "", true);
    } 

    
</script>

<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">    
    <ul class="pobox" onmouseover="this.className='pobox poboxon ';poset_<%=TabPortletID%>_<%=PortletID%>.className='posetover'" onmouseout="this.className='pobox ';poset_<%=TabPortletID%>_<%=PortletID%>.className='poset'" > 

        <div class="portlet-header">
            <!-- 상단 타이틀 //-->
            <li class="title headtitle"><a class="ptReload" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%></a></li>
            <li id="poset_<%=TabPortletID%>_<%=PortletID%>" class="poset"><a class="callContextMenu" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>"></a></li>
        </div>

        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-content">
            <!-- 환경설정 //-->
            <div id="ptListConfig_<%=TabPortletID%>_<%=PortletID%>" style="display:none;">
                <li class="poboxlistset">
                    <span class="listsetbox">
                        <select id="boardListCount_<%=TabPortletID%>_<%=PortletID%>">
                            <option title="5" value="5">5 <%=GetResource("ListRowCnt")%></option>
                            <option title="10" value="10">10 <%=GetResource("ListRowCnt")%></option>
                            <option title="20" value="20">20 <%=GetResource("ListRowCnt")%></option>
                            <option title="30" value="30">30 <%=GetResource("ListRowCnt")%></option>
                        </select>
                    </span>
                    <span class="listsetbtn" style="padding-left:10px;">
                        <a id="cbtnFirst_<%=TabPortletID%>_<%=PortletID%>" title="<%=GetResource("Save")%>"><span><%=GetResource("FirstTabSetting")%></span></a>
                    </span>
                    <span class="listsetbtn">
                        <a id="cbtnConfirm_<%=TabPortletID%>_<%=PortletID%>" title="<%=GetResource("Save")%>"><span><%=GetResource("Save")%></span></a>
                    </span>
                    <span class="listsetbtn">
                        <a id="cbtnClose_<%=TabPortletID%>_<%=PortletID%>" title="<%=GetResource("Close")%>"><span><%=GetResource("Close")%></span></a>
                    </span>
                </li>
            </div>

            <!-- 탭 //-->
            <li class="poboxtab">
                <%=TabPortletTitle %>
            </li>

            <!-- 내용 //-->
            <div id="plList_<%=TabPortletID%>_<%=PortletID%>">
                <div style="padding-top:50px;text-align:center;">
                    <img src="<%=BaseImgPath%>/pims/loading.gif" alt="Loading..." title="Loading..." />
                </div>
            </div>

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
                <input type="hidden" id="hdnTabFirst_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabFirst_<%=TabPortletID%>_<%=PortletID%>" value="<%=TabPortletFirst %>" />
            </div>
        </div>
    </ul>
</div>
<!-- 포틀릿(리스트:일반형) End //-->
