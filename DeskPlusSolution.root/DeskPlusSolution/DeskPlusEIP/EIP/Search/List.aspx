<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Search.List" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Search.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Search.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Home.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Home.css"/>    
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <!-- 상단 //-->
        <div class="layout_full ">
            <div id="layout_header">
                <h1><%=GetResource("Search") %></h1>
                <div class="btn_right" >
                    <fieldset class="app_srch">
                        <ul>
                            <li><asp:DropDownList ID="ddlSearchKey" runat="server" class="searchselect form-control"></asp:DropDownList></li>
                            <li>
                            <input type="text" id="dummy" style="display: none;" />
                                <cc3:ClientSmartTextBox ID="ctbSearch" InputMode="None" MaxLength="250" runat="server"
                                    Required="true" CssClass="searchtext form-control" placeholder="검색어 입력" CssStyle="width:200px;height: 30px;margin:0 5px"></cc3:ClientSmartTextBox>
                            </li>
                            <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li> 
                            
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
                            <th>
                                <%=GetResource("SearchSection")%>
                            </th>
                            <td colspan="2">
                                <input type="radio" name="rblModule" value="" /><%=GetResource("SearchFieldTotal")%>
                                <input type="radio" name="rblModule" value="APPROVAL"  /><%=GetResource("SearchFieldApproval")%>
                                <input type="radio" name="rblModule" value="BOARD"  /><%=GetResource("SearchFieldBoard")%>
                                <input type="radio" name="rblModule" value="RM"  /><%=GetResource("SearchFieldRM")%>
                                <input type="radio" name="rblModule" value="REPORT"  /><%=GetResource("SearchFieldREPORT")%>
                                <!-- input type="radio" name="rblModule" value="MAIL"  /><%=GetResource("SearchFieldMail")%> -->                                       
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
                            <td  colspan="2">
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
                <div class="float-left">
                    <ul id="ulTabMenu" class="tab">                    
                        <%=MenuList%>
                    </ul>
                </div>
                <div class="search-container float-right"> </div> 
            </div>

            <div id="layout_content">
                <div class="row"> 
                <div class="src_body col-12 content-area-body row" style="vertical-align:top;"> 

                <div class="col-12" ><!-- 전자결재 & 게시판-->
            
                    <div id="useApproval">
                        <div class="layout_header">
                            <h1><%=GetResource("SearchFieldApproval")%></h1> <a href="#" id="cbtnApproval"><i class="fas fa-arrow-circle-right bullet text-primary" title="<%=GetResource("GoToDetail")%>"></i></a>
                        </div>
                        <!-- 전자결재 시작 //-->
                    
                        <div id="phApproval" style="clear: both;"></div>
                    
                        <!-- 전자결재 끝 //-->
                    </div>
                
                    <div id="useBoard">
                        <div class="layout_header">
                            <h1><%=GetResource("SearchFieldBoard")%></h1> <a href="#" id="cbtnBoard"><i class="fas fa-arrow-circle-right bullet text-primary" title="<%=GetResource("GoToDetail")%>"></i></a>
                        </div>
                        <!-- 게시판 시작 //-->
                    
                        <div id="phBoard" style="clear: both;"></div>
                                    
                        <!-- 게시판 끝 //-->
                    </div>
                
                    <div id="useMail" style="display:none;">
                        <div class="layout_header">
                            <h1><%=GetResource("SearchFieldMail")%></h1> <a href="#" id="cbtnMail"><i class="fas fa-arrow-circle-right bullet text-primary" title="<%=GetResource("GoToDetail")%>"></i></a>
                        </div>
                        <!-- 메일 시작 //-->
                    
                        <div id="phMail" style="clear: both;"></div>
                    
                        <!-- 메일 끝 //-->                
                    </div>

                    <div id="useRM">
                        <div class="layout_header">
                            <h1><%=GetResource("SearchFieldRM")%></h1> <a href="#" id="cbtnRM"><i class="fas fa-arrow-circle-right bullet text-primary" title="<%=GetResource("GoToDetail")%>"></i></a>
                        </div>
                        <!-- 자원관리 시작 //-->
                    
                        <div id="phRM" style="clear: both;"></div>
                                    
                        <!-- 자원관리 끝 //-->
                    </div>

                    <div id="useReport">
                        <div class="layout_header">
                            <h1><%=GetResource("SearchFieldREPORT")%></h1> <a href="#" id="cbtnReport"><i class="fas fa-arrow-circle-right bullet text-primary" title="<%=GetResource("GoToDetail")%>"></i></a>
                        </div>
                        <!-- 업무일지 시작 //-->
                    
                        <div id="phReport" style="clear: both;"></div>
                                    
                        <!-- 업무일지 끝 //-->
                    </div>
                </div>
                </div><!-- end src_body  -->
                </div><!-- end row  -->
            </div><!-- end layout_content  -->

            <!-- 페이징 시작 //-->
            <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                <h3><span id="pageInfo"></span></h3>
	            <span id="pager"></span>                    
            </div>
            <!-- 페이징 끝 //-->
        </div>
    </div>


    </form>
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
        var ModuleName = "";
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
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="../Js/Search/List.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/Design/popper.min.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/Design/bootstrap.min.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("../Js/Search/List.js");
    </script>
</body>
</html>
