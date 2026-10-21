<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminMain.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Home.AdminMain" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Home.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Home.css" /> 
    <link rel='stylesheet' href='/DeskPlusEIP/Html/Js/ChartNwagon/Nwagon.css' type='text/css' />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" /> 
</head>
<body>
   <form id="form1" runat="server">
 <div id="layout_body">
	<div class="layout_full">   
		    <div id="layout_header">
				<h1><%=GetResource("Task") %></h1>
                <div class="btn_right"> 
                    <fieldset class="app_srch"> 
                       <input id="searchValue" title="검색어" class="keyword" accesskey="s" type="text" placeholder="" />
                       <span id="cbtnSearch" class="s_btn" alt="<%=GetResource("OrganBtnSearch")%>" style="cursor:pointer"><%=GetResource("OrganBtnSearch")%> </span>
                    </fieldset>
                </div>
			</div>
            
            <div class="layout_srh clearfix"> 
                <div class="float-left">
                    <!-- Tab Menu //-->
                    <ul id="ulTabMenu" class="tab">
                        <li class="selected"><a><%=GetResource("AllTask")%></a></li>
                        <li><a id="tabPage"><%=GetResource("UserTask")%></a></li>
                    </ul>
                </div>
                <div class="search-container float-right"> 
                </div> 
            </div>             
                       
            <div id="layout_content"><!-- 컨텐츠 하단영역 -->
                <div class="row">
                    <div class="col-12 content-area-body"><!-- width 100%(col-12) -->   
                        <div class="c_t_Wa col-12 p-0">
                            <div class="l_list"><span class="s_tit"><asp:DropDownList id="ddlYear" runat="server" CssClass="searchselect form-control"></asp:DropDownList> <%=GetResource("Year")%> <%=GetResource("MainState")%></span></div>
                            <div class="c_t_Wa col-12 col-lg-6 pl-0">
                                <table border="0" cellspacing="0">
                                    <tr>
                                        <td><div id="chartCnt"></div></td>
                                        </tr>
                                </table>
                            </div>     

                            <div class="c_t_Wa col-12 col-lg-6 p-0 pb-5 pb-lg-0">
                                <asp:Repeater ID="rpList" runat="server">
                                <HeaderTemplate>
                                    <table class="tbl_sum" cellspacing="0" summary="업무협업 현황 Table">
                                    <caption>업무협업 현황 Table</caption>
                                    <colgroup>
                                    <col width="70px"><col><col><col><col>
                                    </colgroup>
                                    <tbody>
                                    <tr>
                                        <th scope="col"><%=GetResource("IsGubun")%></th>
                                        <th scope="col" title="<%=GetResource("ListType0")%>"><%=GetResource("ListType0")%></th>
                                        <th scope="col" class="bg-success text-white" title="<%=GetResource("Receipt")%>+<%=GetResource("Processing")%>"><%=GetResource("ProgressStr")%></th>
                                        <th scope="col" class="bg-info text-white" title="<%=GetResource("Delay")%>"><%=GetResource("Delay")%></th>
                                        <th scope="col" class="bg-dark text-white" title="<%=GetResource("Complete")%>+<%=GetResource("Reject")%>"><%=GetResource("CompleteStr")%></th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                    <th title="<%=GetResource("Category")%>"><a onclick="Main.GoList('<%# DataBinder.Eval(Container.DataItem, "ItemID").ToString()%>')">
                                        <%# GetItemName(DataBinder.Eval(Container.DataItem, "ItemNameBase").ToString(),
                                                        DataBinder.Eval(Container.DataItem, "ItemNameKo").ToString(),
                                                        DataBinder.Eval(Container.DataItem, "ItemNameEn").ToString(),
                                                        DataBinder.Eval(Container.DataItem, "ItemNameJa").ToString(),
                                                        DataBinder.Eval(Container.DataItem, "ItemNameFr").ToString(),
                                                        DataBinder.Eval(Container.DataItem, "ItemNameZhcn").ToString(),
                                                        DataBinder.Eval(Container.DataItem, "ItemNameZhtw").ToString())%></a></th>
                                    <td title="<%=GetResource("ListType0")%>">
                                        <a onclick="Main.GoCntList('<%# DataBinder.Eval(Container.DataItem, "ItemID").ToString()%>','<%# DataBinder.Eval(Container.DataItem, "Tot").ToString()%>','2,3,4,5,6')">
                                            <%# DataBinder.Eval(Container.DataItem, "Tot").ToString()%></a></td>
                                    <td title="<%=GetResource("Receipt")%>+<%=GetResource("Processing")%>" >
                                        <a class="ing" onclick="Main.GoCntList('<%# DataBinder.Eval(Container.DataItem, "ItemID").ToString()%>','<%# DataBinder.Eval(Container.DataItem, "I").ToString()%>','2,3')">
                                            <%# DataBinder.Eval(Container.DataItem, "I").ToString()%></a></td>                                                        
                                    <td title="<%=GetResource("Delay")%>">
                                        <a class="del" onclick="Main.GoCntList('<%# DataBinder.Eval(Container.DataItem, "ItemID").ToString()%>','<%# DataBinder.Eval(Container.DataItem, "D").ToString()%>','6')">
                                            <%# DataBinder.Eval(Container.DataItem, "D").ToString()%></a></td>
                                    <td title="<%=GetResource("Complete")%>+<%=GetResource("Reject")%>">
                                        <a class="fin" onclick="Main.GoCntList('<%# DataBinder.Eval(Container.DataItem, "ItemID").ToString()%>','<%# DataBinder.Eval(Container.DataItem, "C").ToString()%>','4,5')">
                                            <%# DataBinder.Eval(Container.DataItem, "C").ToString()%></a></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <tr>
                                    <th><%=GetResource("Sum")%></th>
                                    <th><a class="sum" onclick="Main.GoTotalList('2,3,4,5,6')"><span id="spTot"></span></a></th>                            
                                    <th><a class="sum" onclick="Main.GoTotalList('2,3')"><span id="spI"></span></a></th>
                                    <th><a class="sum" onclick="Main.GoTotalList('6')"><span id="spD"></span></a></th>
                                    <th><a class="sum" onclick="Main.GoTotalList('4,5')"><span id="spC"></span></a></th>
                                    </tr>       
                                    </tbody>
                                    </table>
                                </FooterTemplate>
                                </asp:Repeater>
                            </div> 

                            <div>&nbsp;</div>

                            <div class="src_C col-12">
                              <div class="c_t_Fa">
                                  <div class="l_list">
                                      <span class="s_tit"><%=GetResource("RecentList")%></span>
                                      <span class="float-right">
                                         <a href="#" id="recentL" title="<%=GetResource("BeforePage")%>"><i class="fas fa-chevron-left"></i></a>
                                         <a href="#" id="recentR" title="<%=GetResource("NextPage")%>"><i class="fas fa-chevron-right"></i></a>
                                      </span>
                                  </div>
                                  <div id="phRecentList" style="clear: both;"></div>
                              </div>
                           </div>

                            <div class="src_C col-12" >      
                              <div class="c_t_Fa">
                                  <div class="l_list">
                                    <span class="s_tit"><%=GetResource("ProcessList")%></span>
                                    <span class="float-right">
                                        <a href="#" id="processL" title="<%=GetResource("BeforePage")%>"><i class="fas fa-chevron-left"></i></a>
                                        <a href="#" id="processR" title="<%=GetResource("NextPage")%>"><i class="fas fa-chevron-right"></i></a>
                                    </span>
                                  </div>
                                  <div id="phProcessList" style="clear: both;"></div>
                              </div>
                           </div>

                            <div class="src_C col-12">
                              <div class="c_t_Fa">
                                <div class="l_list">
                                  <span class="s_tit"><%=GetResource("DelayList")%></span>
                                  <span class="float-right">
                                        <a href="#" id="delayL" title="<%=GetResource("BeforePage")%>"><i class="fas fa-chevron-left"></i></a>
                                        <a href="#" id="delayR" title="<%=GetResource("NextPage")%>"><i class="fas fa-chevron-right"></i></a>
                                  </span>
                                 </div>
                                 <div id="phDelayList" style="clear: both;"></div>
                              </div>
                           </div>

                            <div class="src_C  col-12">
                              <div class="c_t_Fa">
                                  <div class="l_list">
                                      <span class="s_tit"><%=GetResource("CompleteList")%></span>
                                      <span class="float-right">
                                        <a href="#" id="comL" title="<%=GetResource("BeforePage")%>"><i class="fas fa-chevron-left"></i></a>
                                        <a href="#" id="comR" title="<%=GetResource("NextPage")%>"><i class="fas fa-chevron-right"></i></a>
                                      </span>
                                  </div>
                                  <div id="phComList" style="clear: both;"></div>
                              </div>
                           </div>

                            <div class="src_C  col-12" style="display:none">
                              <div class="c_t_Fa">
                                  <div class="imp_list">
                                    <span class="s_tit"><%=GetResource("ImpoList")%></span>
                                    <span class="float-right">
                                        <a href="#" id="impoL" title="<%=GetResource("BeforePage")%>"><i class="fas fa-chevron-left"></i></a>
                                        <a href="#" id="impoR" title="<%=GetResource("NextPage")%>"><i class="fas fa-chevron-right"></i></a>
                                    </span>
                                  </div>
                                  <div id="phImpoList" style="clear: both;"></div>
                              </div>
                           </div>

				        </div>
                    </div>                                    
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
    <!-- ############# end of modal ############## -->
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>  
    <script type='text/javascript' src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src='/DeskPlusEIP/Html/Js/ChartNwagon/Nwagon.js'></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var RecentPageNo = "<%=RecentPageNo%>";        
        var ProcessPageNo = "<%=ProcessPageNo%>";
        var DelayPageNo = "<%=DelayPageNo%>";
        var ComPageNo = "<%=ComPageNo%>";
        var ImpoPageNo = "<%=ImpoPageNo%>";        
        var CntTot = "<%=CntTot%>";
        var CntI = "<%=CntI%>";
        var CntD = "<%=CntD%>";
        var CntC = "<%=CntC%>";
        var ReLoad = "<%=ReLoad%>";

        var R_ProgressNoContent = "<%=GetResource("ProgressNoContent")%>";
        var R_DelayNoContent = "<%=GetResource("DelayNoContent")%>";
        var R_ComNoContent = "<%=GetResource("ComNoContent")%>";
        var R_ImpoNoContent = "<%=GetResource("ImpoNoContent")%>";        
        var R_Title   = "<%=GetResource("Title")%>";
        var R_RequestDept = "<%=GetResource("RequestDept")%>";
        var R_Requester   = "<%=GetResource("Requester")%>";
        var R_RequestDate = "<%=GetResource("RequestDate")%>";
        var R_EndDate = "<%=GetResource("HEndDate")%>";
        var R_End = "<%=GetResource("End")%>";
        var R_Receiver = "<%=GetResource("Receiver")%>";
        var R_ToDeptName = "<%=GetResource("ToDeptName")%>";  
        var R_State = "<%=GetResource("State") %>";         
        var R_FirstList   = "<%=GetResource("FirstList")%>";
        var R_LastList   = "<%=GetResource("LastList")%>";
        var R_ErrorProcessing = "<%=GetResource("ErrorProcessing")%>";
        var R_InputSearch = "<%=GetResource("InputSearch")%>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src='/DeskPlusEIP/Task/Js/Home/AdminMain.js'></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Task/Js/Home/AdminMain.js");
    </script>
      <script>
        //var tmpChartWidth = $( document ).width() / 2;
        var options = {
            'legend': {
                names: [<%=ChartTitle%>],
                hrefs: []
            },
            'dataset': {
                title: '건수별',
                values: [<%=ChartValue%>],
                colorset: ['#8bc63e', '#9e9080', "#343a40"],
                fields: ['<%=GetResource("Progress")%>', '<%=GetResource("Delay")%>', '<%=GetResource("Complete")%>']

            },
            'chartDiv': 'chartCnt',
            'chartType': 'multi_column',
            'chartSize': { width: 540, height: 220 },
            'maxValue': <%= ChartMaxValue %>,
            'increment': <%= ChartIncre %>

        };

        Nwagon.chart(options);
	</script>
</body>
</html>
