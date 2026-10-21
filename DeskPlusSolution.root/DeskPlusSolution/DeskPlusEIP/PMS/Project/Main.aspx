<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Main.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Project.Main" %>
<%@ Register src="../Module/NoticeBoard.ascx" tagname="NoticeBoard" tagprefix="uc1" %>
<%@ Register Src="../Module/UnitList.ascx" TagName="UnitList" TagPrefix="uc2" %>
<%@ Register TagPrefix="cc2" Namespace="Zeospace.DeskPlus.Component.Grids" Assembly="Zeospace.DeskPlus.Component.Grids" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PMS.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PMS.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ProjectID = "<%=ProjectID %>";
        var MenuID = "<%=MenuID %>";
        var MenuType = "<%=MenuType %>";
        var RqProjectID = "<%=RqProjectID %>";
        var CurrentPage = "<%=CurrentPage %>"; //현재 페이지
        var RecordPerPage = 5; //pagesize
        var TotalItemCount = 5; //5개만 나오도록 한다.
        var WeightTotal = "<%=WeightTotal %>";

        var OrderB = "";
        var OrederU = "";
        var BoaList = "<%=BoaList %>";
        var BoaHeaderList = "<%=BoaHeaderList %>";
        var BoaID = 0;
        var BoaItem = 0;
        var UnitList = "<%=UnitList %>";
        var UnitHeaderList = "<%=UnitHeaderList %>";

        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var SearchTermValue = "<%=SearchTermValue %>";
        var SearchTerm = "<%=SearchTerm %>";

        var R_MaxList = "<%=GetResource("MaxList") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Unit_Code = "<%=GetResource("Unit_Code") %>";
        var R_Unit_Title = "<%=GetResource("Unit_Title") %>";
        var R_Unit_Status = "<%=GetResource("Unit_Status") %>";
        var R_Unit_Owner = "<%=GetResource("Unit_Owner") %>";
        var R_Unit_PlanStartDate = "<%=GetResource("Unit_PlanStartDate") %>";
        var R_Unit_PlanEndDate1 = "<%=GetResource("Unit_PlanEndDate1") %>";
        var R_Unit_PlanDay = "<%=GetResource("Unit_PlanDay") %>";
        var R_Unit_Register_ltlWeightPercent = "<%=GetResource("Unit_Register_ltlWeightPercent") %>";
        var R_ProjectList_Header6 = "<%=GetResource("ProjectList_Header6") %>";
        var R_Unit_RealProcess = "<%=GetResource("Unit_RealProcess") %>";
        var R_Board_List_Name = "<%=GetResource("Board_List_Name") %>";
        var R_Board_List_Title = "<%=GetResource("Board_List_Title") %>";
        var R_Board_List_DateTime = "<%=GetResource("Board_List_DateTime") %>";
        var R_Board_List_Viewer = "<%=GetResource("Board_List_Viewer") %>";

        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   
        var ImagePath = "<%=BaseImgPath%>"; 
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Project/Main.js"></script>
    <script type="text/javascript">        
        //JsCall("/DeskPlusEIP/PMS/Js/Project/Main.js");    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <h1><asp:Label ID="lbProjectName" runat="server"></asp:Label></h1>
            <ul style="float:right">
                <li><h1><%=GetResource("Unit_Register_ltlCharger") %> : <asp:Label ID="lbProjectWUserName" runat="server"></asp:Label></h1></li>
            </ul>
        </div>

        <div id="layout_content"> 
            <div class="row">
                <div class="col-12">

	                <div class="table_typeLtop"><p></p></div>
                    <div class="table_typeLmiddle">
                                                                                                                                                                                                                                                                                                                                            <div class="project_right">
                        <!-- 테이블 시작 -->
                        <table class="table_typeC">
                            <caption></caption>
                            <colgroup>
                                <col width="15%"/>
                                <col width="35%"/>
                                <col width="15%"/>
                                <col width="35%"/>
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th><%=GetResource("ProjectInfo_PlanSDate") %></th>
                                    <td><asp:Label ID="lbPlanSDate" runat="server"></asp:Label></td>
                                    <th><%=GetResource("ProjectInfo_PlanEDate") %></th>
                                    <td><asp:Label ID="lbPlanEDate" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("ProjectInfo_SDate") %></th>
                                    <td><asp:Label ID="lbSDate" runat="server"></asp:Label></td>
                                    <th> <%=GetResource("ProjectInfo_EDate") %></th>
                                    <td><asp:Label ID="lbEDate" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("ProjectInfo_TotalDays") %></th>
                                    <td><asp:Label ID="lbTotalDays" runat="server"></asp:Label></td>
                                    <th><%=GetResource("ProjectInfo_InDays") %></th>
                                    <td><asp:Label ID="lbInDays" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("ProjectInfo_RemainDays") %></th>
                                    <td><asp:Label ID="lbRemainDays" runat="server"></asp:Label></td>
                                    <th><%=GetResource("ProjectInfo_DelayDays") %></th>
                                    <td><asp:Label ID="lbDelayDays" runat="server"></asp:Label></td>
                                </tr>
                            </tbody>
                        </table>      
                        <!-- 테이블 끝 -->
                    </div>    
                        <div class="project_left">
            	            <div class="project_per">
                                <div style="display:none;">
                                <asp:PlaceHolder ID="phProcess" runat="server"></asp:PlaceHolder>
                                </div>
                                <div class="tree_webhard_volume" style="width:70%;">            
                                <%=GetResource("Unit_Register_ltlProcess")%> 
                                <div class="progress">
                                <div id="progress1" class="progress-bar bg-secondary " role="progressbar" style="width:<%=ProjProcessStr%>" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div>
                                </div>
                                <span class="volume_percent"><%=ProjProcess%></span>
                              </div>
                            </div>
                            <div class="project_per2">
                                <div style="display:none;">
                                <asp:PlaceHolder ID="phPeriod" runat="server"></asp:PlaceHolder>
                                </div>
                                <div class="tree_webhard_volume" style="width:70%;">            
                                  <%=GetResource("Main_DatePercent")%>  
                                 <div class="progress">
                                    <div id="progress2" class="progress-bar bg-warning " role="progressbar" style="width:<%=ProjPeriodStr%>" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div>
                                 </div>
                                <span class="volume_percent"><%=ProjPeriod%></span>
                              </div>
                            </div>     

                            <div style="display:none;">
                                <table>
                                <tr>
                                <td bgcolor="white" align="left"><asp:Literal ID="ltlStartDate" runat="server"></asp:Literal></td>
                                <td bgcolor="white" align="center"><asp:Literal ID="ltlProcessDate" runat="server"></asp:Literal></td>
                                <td bgcolor="white" align="right"><asp:Literal ID="ltlEndDate" runat="server"></asp:Literal></td>
                                </tr>
                                </table>
                            </div>  
                        </div>   
                    </div>

                    <div class="table_typeLbottom"><p></p></div>                            

                    <div class='cont'><h2><span id="headerUnit"></span></h2></div>           
                    <div id="placeholderU" style="clear: both;"></div>           


                    <div id="boa0" style="display:none">
                        <div class='cont'><h2><span id="header_0"></span></h2></div>                
                        <div id="placeholderB_0" style="clear: both;"></div>                
                    </div>

                    <div id="boa1" style="display:none">
                        <div class='cont'><h2><span id="header_1"></span></h2></div>                
                        <div id="placeholderB_1" style="clear: both;"></div>
                    </div>

                    <div id="boa2" style="display:none">
                        <div class='cont'><h2><span id="header_2"></span></h2></div>               
                        <div id="placeholderB_2" style="clear: both;"></div>    
                    </div>

                    <div id="boa3" style="display:none">
                        <div class='cont'><h2><span id="header_3"></span></h2></div>
                        <div id="placeholderB_3" style="clear: both;"></div>
                    </div>

                    <div id="boa4" style="display:none">
                        <div class='cont'><h2><span id="header_4"></span></h2></div>                
                        <div id="placeholderB_4" style="clear: both;"></div>
                    </div>
                </div>
              
              </div>             
            </div>
        </div>
    </div>
    </div>
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
</body>
</html>
