<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Content.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Home.Content" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Home.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Home.css" />   
    <link rel='stylesheet' href='/DeskPlusEIP/Html/Js/ChartNwagon/Nwagon.css' type='text/css' />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
	<style>
	    .orangeSquare badge badge-info{ line-height:20px;padding:2px 3px;margin:0 0 2px 0; text-align:center; color:#FFFFFF; background-color:#ED6405; font-size:12px; border-radius:2px;  }
	</style>
</head>
<body>

    <form id="form1" runat="server">
	<div id="layout_body">
    	<div class="layout_full ">
            <div id="layout_header">
				<h1><%=GetResource("Board")%></h1>
                <div class="btn_right"> 
                    <!-- 검색 box -->        
                    <fieldset class="app_srch">
                        <input id="searchValue" title="<%=GetResource("SearchTarget")%>" class="keyword" accesskey="s" type="text" placeholder="<%=GetResource("PlaceholderMenu")%>" />            
                        <span id="cbtnSearch" class="s_btn" style="cursor:pointer" title="<%=GetResource("Search")%>"><%=GetResource("Search")%></span>
                    </fieldset>
                </div>

			</div><!-- end layout_header  -->            
            

            <div class="layout_srh clearfix"> 
                <div class="float-left">
                    <!-- Tab Menu //-->
                    <ul id="ulTabMenu" class="tab">
                        <li id="allPage" runat="server"><a><%=GetResource("TotalBoard")%></a></li>
                        <li id="myPage" runat="server"><a><%=GetResource("MyBoard")%></a></li>            
                        <!--<li id="interested" class="tab_interested" runat="server"><a><i class="fas fas fa-share-alt"></i><%=GetResource("InterestedPersonAdd")%></a></li>-->   
                    </ul>
                </div>
                <div class="search-container float-right"> 
                </div> 
            </div> 
            
            
            
             <div id="layout_content"> 
            	<div class="row"> 
                    <div id="allBoardList" runat="server" class="src_body col-12 col-lg-9 content-area-body" style="vertical-align:top;"> 
        
                        <!-- 차트영역 -->
                        <div id="chartUser" runat="server" class="c_c1_b col-12" >
                            <div class="l_list"><span class="s_tit"><%=GetResource("BoardStatus")%></span></div>
                            <table border="0" cellspacing="0" cellpadding="0" align="center" style="width:100%">
                                <tr>
                                    <td style="text-align:center;">
                                    	<div id="chartStacked" style="display:inline-block; "></div>
                                    </td>
                                    <td style="text-align:center;">
                                        <div id="chartDonut" style="display:inline-block;"></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:center"><%=GetResource("MyTotalBoardStatus")%></td>
                                    <td style="text-align:center"><%=GetResource("YearMaxWriter")%></td>
                                </tr>
                            </table>
                        </div>
                        <!-- //차트영역 -->
 
                        <div class="clearfix p-3"></div>                        


                        <!-- Tab Menu -->       
                        <div   class="c_t_Bac_c1_b  col-12" >
                        	<div class="l_list">
                            <ul id="MainMenu" class="tab_menu clearfix">
                                 <li class="selected"><a onclick="Content.GetPageList('RecentWrite');return false;"><%=GetResource("RecentWrite")%> <% if(GetNewMenu.RecentWrite) { %><img class="BtnNew" alt="" src="/DeskPlusEIP/Design/C/Img/btn/i_new.gif" /><%} %></a><i id="RecentWrite" class="fas fa-arrow-circle-right "></i></li>
                                 <li><a onclick="Content.GetPageList('MaxShort');return false;"><%=GetResource("MaxShort")%> <% if(GetNewMenu.MaxShort) { %><img class="BtnNew" alt="" src="/DeskPlusEIP/Design/C/Img/btn/i_new.gif" /><%} %></a><i id="MaxShort" class="fas fa-arrow-circle-right "></i></li>
                                 <li><a onclick="Content.GetPageList('MaxRead');return false;"><%=GetResource("MaxRead")%> <% if(GetNewMenu.MaxRead) { %><img class="BtnNew" alt="" src="/DeskPlusEIP/Design/C/Img/btn/i_new.gif" /><%} %></a><i id="MaxRead" class="fas fa-arrow-circle-right "></i></li>
                                 <li><a onclick="Content.GetPageList('MaxRate');return false;"><%=GetResource("MaxRate")%> <% if(GetNewMenu.MaxRate) { %><img class="BtnNew" alt="" src="/DeskPlusEIP/Design/C/Img/btn/i_new.gif" /><%} %></a><i id="MaxRate" class="fas fa-arrow-circle-right "></i></li>
                            </ul>
                            </div>
                            <!-- //최신글, 최다댓글, 최다조회, 평가우수 -->
                            <div id="placeholderMain" style="clear: both;">
                                 <table class="tbl_LBoard" border="1" cellspacing="0" summary="">
                                     <tbody>
                                         <% foreach (var entity in GetRecentWrite) { %>
                                         <tr>
                                             <td class="suj">
                                                 <div class="Hb_tit">
                                                     <a onclick="Content.PopupView('<%= entity.CategoryID%>','<%= entity.ListID%>');return false;"><%= entity.Title%></a>
                                                     <% if (entity.ShowNew == "Y") { %>
                                                     <img class="BtnNew" alt="" src="/DeskPlusEIP/Design/C/Img/btn/i_new.gif" />
                                                     <% } %>
                                                     <div class="Hb_dt"><%=entity.WriteDateDT.ToShortDateString() %></div><div class="Hb_nm" onclick="AccountInfo('<%=entity.WriteUserID%>');return false;" style="cursor:pointer"><%=entity.WriteUserName%></div>
                                                 </div>
                                                 <div class="Hb_cnt">
                                                     <a onclick="Content.PopupView('<%= entity.CategoryID%>','<%= entity.ListID%>');return false;">
                                                     <%= entity.MobileContent%>
                                                     </a>
                                                 </div>
                                             </td>
                                         </tr>
                                         <% } %>
                                     </tbody>
                                 </table>
                             </div>
                         </div>
                        <!-- //List -->
                        
					</div><!-- end col  -->
                    <div id="maxYearUser" runat="server" class="src_body col-12 col-lg-3  content-area-body" style="vertical-align:top;"> 
                        


                       <!-- 금년도 최다 작성자 -->
                        <div  class="src_Sh  col-6 col-lg-12" >
                            <div class="l_AFlist"><a href="#"><span><%=GetResource("YearMaxWriter")%></span></a></div>
                            <table class="tbl_Board" border="1" cellspacing="0" summary="금년도 최다 작성자">
                                <caption>
                                    <%=GetResource("YearMaxWriter")%></caption>
                                <colgroup>
                                    <col width="70px" />
                                    <col />
                                </colgroup>
                                <tbody>
                                <% if (GetMaxWriter.Count == 0) {%>
                                <%=GetResource("YearNoMaxWriter")%>
                                <%} %>
                
                                    <% foreach (var entity in GetMaxWriter) { %>
                                    <tr>
                                        <td>
                                            <%= entity.PhotoImage%>
                                        </td>
                                        <td class="suj" style="vertical-align:middle">
                                            <%= entity.DeptName%><span class="badge bg-info text-white ml-2" style="font-size:11px;"><%= entity.TitleName%></span><br />
                                            <a onclick="AccountInfo('<%= entity.UserID %>');return false;" style="cursor:pointer">
                                                <%= entity.Name%><span class="badge number text-white"><%= entity.Cnt%></span>
                                            </a>
                                        </td>
                                    </tr>
                                    <% } %>                    
                                </tbody>
                            </table>
                        </div>


                        <!-- 금년도 최다 댓글자 -->
                        <div  id="maxYearShort" runat="server" class="src_Sh  col-6 col-lg-12">
                            <div class="l_AFlist">
                                <a href="#"><span><%=GetResource("YearMaxShort")%></span></a>
                            </div>
                            <table class="tbl_Board" border="1" cellspacing="0" summary="금년도 최다 작성자">
                                <caption>
                                   <%=GetResource("YearMaxShort")%></caption>
                                <colgroup>
                                    <col width="70px" />
                                    <col />
                                </colgroup>
                                <tbody>
                                <% if (GetMaxShortReplyer.Count == 0) {%>
                                <%=GetResource("YearNoMaxWriter")%>
                                <%} %>
                
                                    <% foreach (var entity in GetMaxShortReplyer) { %>
                                    <tr>
                                        <td>
                                            <%= entity.PhotoImage%>
                                        </td>
                                        <td class="suj" style="vertical-align:middle">
                                            <%= entity.DeptName%><span class="badge bg-info text-white ml-2" style="font-size:11px;"><%= entity.TitleName%></span><br />                     
                                            <a onclick="AccountInfo('<%= entity.UserID %>');return false;" style="cursor:pointer">
                                                <%= entity.Name%><span class="badge number text-white"><%= entity.Cnt%></span>
                                            </a>
                                        </td>
                                    </tr>
                                    <% } %>
                                </tbody>
                            </table>
                        </div>
                        <!-- //금년도 최다 작성자 -->
                
					</div><!-- end col  -->
                    <div id="newMyBoardList" runat="server" class="src_body col-12  content-area-body" style="vertical-align:top;"> 
        
              			<!-- 신규 나의글 디자인 -->
                        <div >
                           <div class="row">
                              <!-- list -->
                              <div class="c_t_a col-12 col-lg-6 col-xl-4">
                                 <div class="l_list">
                                    <span class="s_tit"><%=GetResource("MyWrite")%></span>
									<span class="float-right">
                                        <a href="#" id="myWriteL" title="<%=GetResource("Previous")%>"><i class="fas fa-chevron-left"></i></a>
                                        <a href="#" id="myWriteR" title="<%=GetResource("Next")%>"><i class="fas fa-chevron-right"></i></a>
                                     	<a id="MyWrite" onclick="Content.GetPageList('MyWrite');return false;" ><i class="fas fa-arrow-circle-right" title="more"></i></a>                 
                                    </span>
                                 </div>
                                 
                                 <!-- 테이블 시작 //-->
                                 <div id="phMyWriteList" style="clear: both;"></div>
                                 <!-- 테이블 끝 //-->
                              </div>
                              <!-- //List -->
                              <!-- list -->
                              <div class="c_t_a col-12 col-lg-6 col-xl-4">
									<div class="l_list">
                                    <span class="s_tit"><%=GetResource("MyListByShort")%></span>
                                        <span class="float-right">
                                            <a href="#" id="myListByShortL" title="<%=GetResource("Previous")%>"><i class="fas fa-chevron-left"></i></a>
                                            <a href="#" id="myListByShortR" title="<%=GetResource("Next")%>"><i class="fas fa-chevron-right"></i></a>
                                            <a id="MyListByShort" onclick="Content.GetPageList('MyListByShort');return false;"><i class="fas fa-arrow-circle-right" title="more"></i></a>
                                        </span>
                                	</div>
                                     <!-- 테이블 시작 //-->
                                     <div id="phMyListByShortList" style="clear: both;"></div>
                                     <!-- 테이블 끝 //-->
                              </div>
                              <!-- //List -->
                              <!-- list -->
                              <div class="c_t_a col-12 col-lg-6 col-xl-4">
                              		<div class="l_list">
                                        <span class="s_tit"><%=GetResource("MyShort")%></span>
                                        <span class="float-right">
                                            <a href="#" id="myShortL" title="<%=GetResource("Previous")%>"><i class="fas fa-chevron-left"></i></a>
                                            <a href="#" id="myShortR" title="<%=GetResource("Next")%>"><i class="fas fa-chevron-right"></i></a>
                                            <a id="MyShort" onclick="Content.GetPageList('MyShort');return false;"><i class="fas fa-arrow-circle-right" title="more"></i></a>
                                        </span>
                                    </div>
                                    <!-- 테이블 시작 //-->
                                    <div id="phMyShortList" style="clear: both;"></div>
                                    <!-- 테이블 끝 //-->
                                </div>
                                <!-- //List -->
                                <!-- list -->
                                <div class="c_t_a col-12 col-lg-6 col-xl-4 ">
                                    <div class="l_list">
                                    <span class="s_tit"><%=GetResource("MyImpo")%></span>
                                        <span class="float-right">
                                            <a href="#" id="myImpoL" title="<%=GetResource("Previous")%>"><i class="fas fa-chevron-left"></i></a>
                                            <a href="#" id="myImpoR" title="<%=GetResource("Next")%>"><i class="fas fa-chevron-right"></i></a>
                                        </span>
                                    </div>
                                    <!-- 테이블 시작 //-->
                                    <div id="phMyImpoList" style="clear: both;"></div>
                                    <!-- 테이블 끝 //-->
                                </div>
                                <!-- //List -->
                            </div>
                        </div>
                        <!-- 신규 나의글 디자인 끝 -->



                    </div><!-- end col  -->

            	</div><!-- end row  -->
            </div><!-- end layout_content  -->

		</div><!-- end layout_full  -->
	</div><!-- end layout_body  -->
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src='/DeskPlusEIP/Html/Js/ChartNwagon/Nwagon.js'></script>
    <script type="text/javascript">
        var MenuTab = ""; //tab
        var Gubun = ""; //main :1 ; my : 2
        var MyWritePageNo = 1;
        var MyListByShortPageNo = 1;
        var MyShortPageNo = 1;
        var MyImpoPageNo = 1;
        var StartTab = <%= StartTab %>; 
        var R_FirstList = "<%=GetResource("FirstList")%>";
        var R_LastList = "<%=GetResource("LastList")%>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
        var R_LengthOver =  "<%=GetResource("LengthOver")%>";
        
    </script>        
    <script type="text/javascript" src='/DeskPlusEIP/Board/Js/Home/Content.js'></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Home/Content.js");
    </script>
    <script>
        var xChartTitle = ["<%=GetResource("BoardList")%>", "<%=GetResource("CommentsAttachedList")%>", "<%=GetResource("ShortList")%>"];
        var chartWidth = 345;   //420
        var chartHeight = 250;
        var xChartTitleLen = xChartTitle.length;
        if (xChartTitleLen > 0) {
            //chartWidth = Number(xChartTitleLen * 80) + 150; //막대차트 개당 넓이공간 55px
        }

        var options = {
            'legend': {
                names: xChartTitle,
                hrefs: []
            },
            'dataset': {
                title: "<%=GetResource("MyYearActivity")%>",
                values: [<%= GetMyBarChart.MyWriteCnt %>, <%= GetMyBarChart.MyListByShortCnt %>, <%= GetMyBarChart.MyShortCnt %>],
                colorset: ['#7898f3'],
                fields: ['<%=GetResource("CountNumber")%>']
            },
            'chartDiv': 'chartStacked',
            'chartType': 'column',
            'chartSize': { width: chartWidth, height: chartHeight },
            'maxValue': <%= GetMyBarChart.ChartMaxValue %>,
            'increment': <%= GetMyBarChart.ChartIncre %>
        };

        Nwagon.chart(options);
	</script>
    <script>
        var options = {
            'dataset': {
                title: '<%=GetResource("CountNumber")%>',
                values: [<%= GetMaxWriterCnt %>],
                colorset: ['#3d6efc', '#8ea7ef', '#d1dbfb', '#ade0f9', '#c0adf9'],
                fields: [<%= GetMaxWriterName %>]
            },
            'donut_width': 40,
            'core_circle_radius': 45,
            'chartDiv': 'chartDonut',
            'chartType': 'donut',
            'chartSize': { width: 345, height: 250 }
        };

        Nwagon.chart(options);
    </script>
</body>
</html>
