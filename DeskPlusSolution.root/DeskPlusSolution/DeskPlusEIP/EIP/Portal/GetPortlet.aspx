<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetPortlet.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Portal.GetPortlet" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CurrentPage = "1"; //현재페이지
        var TotalItemCount = "<%=TotalCount%>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var CatID = "<%=CategoryID %>";
        var TabID = "<%=TabID %>";
        var FirstColumn = "<%=FirstColumn %>";
        var PhotoName = "<%=PhotoName %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>";
        var AccordionActive = 0;
        var Gubun = "COMMON"; //공용:COMMON; 개인:PRIVATE
        var IsAdmin = "<%=IsAdmin %>";
        var TabLayout = "<%=TabLayout%>";
        var Options = "<%=Options%>";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_DirectAdd = "<%=GetResource("DirectAdd") %>";
        var R_AddPortletCheckedTab = "<%=GetResource("AddPortletCheckedTab") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
    </script>
    <!--script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Portal/GetPortlet.js"></script-->
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/EIP/Js/Portal/GetPortlet.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
    <ul class="left-menu-container left-menu-2">
    <li>
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("AddPortlet")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"/></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height: 450px;">
                <div class="eip_portlet_left" style="width:180px;">

                <div id="eip_portlet_treetop" style="display:none">
                    <span id="folderName" class="treetop_title"><%=GetResource("PortletList")%></span>
                </div>

                <div id="layout_treeboxAcc">
                    <div class="accordion" style="height:310px;">
                        <div class="menu"><a href="#" onclick="" ><%=GetResource("CommonPortlet")%></a></div>
                        <div class="menusub">
                            <zeoPortletTree:UCPortletTreeScript ID="CommonPortletTree" runat="server" UseIcon="true" UseActive="true"
                                SetCallbackComplete="onCallbackComplete" SetNodeSelect="CommonPortletTree_onNodeSelect" />
                        </div>
                        <div id="privateMenu" class="menu"><a href="#" onclick="" ><%=GetResource("PrivatePortlet")%></a></div>
                        <div id="privateMenuSub" class="menusub">
                            <zeoPortletTree:UCPortletTreeScript ID="PrivatePortletTree" runat="server" UseIcon="true" UseActive="true"
                                SetCallbackComplete="onCallbackComplete" SetNodeSelect="PrivatePortletTree_onNodeSelect" />
                        </div>        
                    </div>    
               </div>

    	        <div style="margin-bottom:0px;padding:5px 0 0 0 ;">
                    <fieldset>
                    <legend><label title="<%=GetResource("Layout")%>"><%=GetResource("Layout")%></label></legend>
                    <div style=" text-align:center; margin:5px 5px 5px 5px; padding:5px 5px 5 5px; border:2px solid #AEAEAE">            
            	        <img src="<%=BaseImgPath%>/layout/layout<%=TabLayout%>.png" alt="Layout" />
                    </div>
                    </fieldset>
                </div>   

                </div>
                <div class="eip_portlet_mid">
                    
                        <div class="table_typeLtop"><p></p></div>
                        <div id="placeholder"></div> 
                        <div class="table_typeLbottom"><p></p></div>

                        <!-- 페이징 시작 //-->
                        <div id="Pager1" name="Pager1" class="paging_num_s1">
                            <h3>
                                <span id="pageInfo"></span>
                            </h3>
                            <span id="pager"></span>
                            <div style="display:none;">
                            <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight">
                                <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                                <asp:ListItem Text="10 " Value="10"></asp:ListItem>
                                <asp:ListItem Text="20 " Value="20"></asp:ListItem>
                                <asp:ListItem Text="30 " Value="30"></asp:ListItem>
                            </asp:DropDownList>
                            </div>
                        </div>
                        <!-- 페이징 끝 //-->

                    
                    
                </div>
            </div>
        </div>
    </li>
    </ul>
    </div>
    </form>
</body>
</html>
