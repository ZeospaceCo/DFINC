<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Search.Web.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Search.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Search.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ModuleName = "<%=ModuleName %>";
        var SearchTerm = "";
        var SearchTermValue = "";
        var SearchKey = "";
        var SearchValue = "";
        var CurrentPage = "1";
        var Order = "";
        var TotalItemCount = 0;
        var RecordPerPage = 0;

        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_ApplyRegDate = "<%=GetResource("ApplyRegDate") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Number = "<%=GetResource("Number") %>";
        var R_Section = "<%=GetResource("Section") %>";
        var R_SearchOutputTitle = "<%=GetResource("SearchOutputTitle") %>";
        var R_AttachFile = "<%=GetResource("AttachFile") %>";
        var R_Registrant = "<%=GetResource("Registrant") %>";
        var R_SearchOutputDate = "<%=GetResource("SearchOutputDate") %>";
        var R_AddPortletCheckedTab = "<%=GetResource("AddPortletCheckedTab") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_ConfirmCheckValue = "<%=GetResource("ConfirmCheckValue") %>";
        var R_Schedule = "<%=GetResource("Schedule") %>";
        var R_ToDo = "<%=GetResource("TODO") %>";


        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja
        var MailProduct = "<%=MailProduct %>";
    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        JsCall("./Js/List.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body" class="write">
         <div class="layout_full">
            <div id="layout_header">
                <h1>Search</h1>
                <br />
                <span class="app_subtxt">* <%=GetResource("SearchWord1")%> <span class="srh_emp">AND</span> <%=GetResource("SearchWord2")%> - <%=GetResource("SearchWord1n2")%></span>
                <br />
                <span class="app_subtxt">* <%=GetResource("SearchWord1")%> <span class="srh_emp">OR</span> <%=GetResource("SearchWord2")%> - <%=GetResource("SearchWord1or2")%></span>

                <div class="btn_right"> 
                     <fieldset class="app_srch">  
                       <ul>
                        <li style="display:none;"><asp:DropDownList ID="ddlSearchKey" runat="server"></asp:DropDownList></li>
                        <li>
                           <input type="text" id="dummy" style="display:none;" />
                           <cc3:ClientSmartTextBox ID="ctbSearch" InputMode="None" MaxLength="250" runat="server" Required="true" CssClass="searchtext form-control" CssStyle="width:200px;height: 30px;margin:0 5px"></cc3:ClientSmartTextBox>
                        </li>
                        <li>
                        <cc3:ClientSmartButton ID="cbtnSearch" runat="server" class="searchbtn btn-secondary"></cc3:ClientSmartButton>
                        </li>         
                    </ul>
                    </fieldset>
                </div>
            </div>

            <div style="padding-left:30px; padding-right:30px">
                <div class="row"> 
                    <table class="table_typeB" style="margin-top:10px">
                        <colgroup>
                            <col width="160px" />
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th><%=GetResource("SearchSection")%></th>
                                <td colspan="2">
                                    <input type="radio" name="rblModule" value="" /><%=GetResource("SearchFieldTotal")%>
                                    <input type="radio" name="rblModule" value="APPROVAL"  /><%=GetResource("SearchFieldApproval")%>
                                    <input type="radio" name="rblModule" value="BOARD"  /><%=GetResource("SearchFieldBoard")%>
                                    <span  style="display:none;"><input type="radio" name="rblModule" value="COOP" /><%=GetResource("SearchFieldCoop")%></span>
                                    <input type="radio" name="rblModule" value="TASK"  /><%=GetResource("SearchFieldTask")%>   
                                    <input type="radio" name="rblModule" value="EDM"  /><%=GetResource("SearchFieldEDM")%>                                       
                                    <input type="radio" name="rblModule" value="ADDRESS"  /><%=GetResource("SearchFieldAddress")%>   
                                    <input type="radio" name="rblModule" value="WEBHARD"  /><%=GetResource("SearchFieldWebHard")%>   
                                    <input type="radio" name="rblModule" value="PIM"  /><%=GetResource("SearchFieldPim")%>   
                                    <input type="radio" name="rblModule" value="RM"  /><%=GetResource("SearchFieldRM")%>   
                                </td>
                            </tr>
                            <tr>
                                <th><%=GetResource("ShowSearch")%></th>
                                <!--
                                <td>
                                    <span><%=GetResource("SortWay")%>:</span>
                                    <asp:DropDownList ID="ddlSort" runat="server">
                                    </asp:DropDownList>
                                </td>
                                -->
                                <td colspan="2">
                                    <span><%=GetResource("SearchResultCntTitle")%>:</span>
                                    <input type="radio" name="rblListCount" value="10" />10<%=GetResource("SearchResultCnt")%>
                                    <input type="radio" name="rblListCount" value="20"  />20<%=GetResource("SearchResultCnt")%>
                                    <input type="radio" name="rblListCount" value="30"  />30<%=GetResource("SearchResultCnt")%>
                                </td>  
                            </tr>
                            <tr>
                                <th><%=GetResource("SearchTimePeriod")%></th>
                                <td>                                    
                                    <span><%=GetResource("PeriodDate")%>: </span>
                                    <asp:DropDownList ID="ddlSearchTerm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>                                      
                                </td>
                                <td>
                                    <div id="startEndDate" class="searchterm">
                                        <input type="text" id="startDate" size="10" class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="10" class="clsDate searchtext form-control" />
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

            <div>&nbsp;</div>

            <div class="layout_srh clearfix"> 
                <!-- 탭 //-->
                <div class="float-left">
                    <ul id="ulTabMenu" class="tab" style="margin-left:30px !important">              
                        <%=MenuList%>
                    </ul>                
                </div>
                <div class="search-container float-right"> </div> 
            </div>

            <!-- 리스트 //-->
            <div id="layout_content" style="padding-right:30px">    
                <div id="divLoading" align="center" class="divLoading"></div>
                <div id="useApproval">
                    <div class="layout_subbox">
                        <h2><%=GetResource("SearchFieldApproval")%> <%=GetResource("SearchFieldApprovalInfo")%></h2>
                        <ul class="btn_right">
                            <cc3:ClientSmartButton ID="cbtnApproval" runat="server" BeginEndTag="li" />
                        </ul>
                    </div>
                    <!-- 전자결재 시작 //-->                    
                    <div id="phApproval" style="clear: both;"></div>                    
                </div>
                
                <div id="useBoard">
                    <div class="layout_subbox">
                        <h2><%=GetResource("SearchFieldBoard")%> <%=GetResource("SearchFieldBoardInfo")%></h2>
                        <ul class="btn_right">
                            <cc3:ClientSmartButton ID="cbtnBoard" runat="server" BeginEndTag="li" />
                        </ul>
                    </div>
                    <!-- 게시판 시작 //-->                    
                    <div id="phBoard" style="clear: both;"></div>
                </div>                

                <div id="useCoop" style="display:none;">
                    <div class="layout_subbox">
                        <h2><%=GetResource("SearchFieldCoop")%> <%=GetResource("SearchFieldCoopInfo")%></h2>
                        <ul class="btn_right">
                            <cc3:ClientSmartButton ID="cbtnCoop" runat="server" BeginEndTag="li" />
                        </ul>
                    </div>
                    <!-- 업무협업 시작 //-->                    
                    <div id="phCoop" style="clear: both;"></div>
                </div>

                <div id="useTask">
                    <div class="layout_subbox">
                        <h2><%=GetResource("SearchFieldTask")%> <%=GetResource("SearchFieldTaskInfo")%></h2>
                        <ul class="btn_right">
                            <cc3:ClientSmartButton ID="cbtnTask" runat="server" BeginEndTag="li" />
                        </ul>
                    </div>
                    <!-- 업무협업 시작 //-->                    
                    <div id="phTask" style="clear: both;"></div>
                </div>

                <div id="useEDM">
                    <div class="layout_subbox">
                        <h2><%=GetResource("SearchFieldEDM")%> <%=GetResource("SearchFieldEDMInfo")%></h2>
                        <ul class="btn_right">
                            <cc3:ClientSmartButton ID="cbtnEDM" runat="server" BeginEndTag="li" />
                        </ul>
                    </div>
                    <!-- EDM 시작 //-->                    
                    <div id="phEDM" style="clear: both;"></div>         
                </div>

                <div id="useAddress">
                    <div class="layout_subbox">
                        <h2><%=GetResource("SearchFieldAddress")%> <%=GetResource("SearchFieldAddressInfo")%></h2>
                        <ul class="btn_right">
                            <cc3:ClientSmartButton ID="cbtnAddress" runat="server" BeginEndTag="li" />
                        </ul>
                    </div>
                    <!-- ADDRESS 시작 //-->                    
                    <div id="phAddress" style="clear: both;"></div>                                  
                </div>

                <div id="useWebHard">
                    <div class="layout_subbox">
                        <h2><%=GetResource("SearchFieldWebHard")%> <%=GetResource("SearchFieldWebHardInfo")%></h2>
                        <ul class="btn_right">
                            <cc3:ClientSmartButton ID="cbtnWebHard" runat="server" BeginEndTag="li" />
                        </ul>
                    </div>
                    <!-- WEBHARD 시작 //-->                    
                    <div id="phWebHard" style="clear: both;"></div>  
                </div>

                <div id="usePim">
                    <div class="layout_subbox">
                        <h2><%=GetResource("SearchFieldPim")%> <%=GetResource("SearchFieldPimInfo")%></h2>
                        <ul class="btn_right">
                            <cc3:ClientSmartButton ID="cbtnPim" runat="server" BeginEndTag="li" />
                        </ul>
                    </div>
                    <!-- PIM 시작 //-->                    
                    <div id="phPim" style="clear: both;"></div>
                </div>

                <div id="useRM">
                    <div class="layout_subbox">
                        <h2><%=GetResource("SearchFieldRM")%> <%=GetResource("SearchFieldRMInfo")%></h2>
                        <ul class="btn_right">
                            <cc3:ClientSmartButton ID="cbtnRM" runat="server" BeginEndTag="li" />
                        </ul>
                    </div>
                    <!-- 자원관리 시작 //-->                    
                    <div id="phRM" style="clear: both;"></div>         
                </div>
              

                <!-- 페이징 시작 //-->
                <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                    <h3><span id="pageInfo"></span></h3>
	                <span id="pager"></span>                    
                </div>
                <!-- 페이징 끝 //-->
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
</body>
</html>
