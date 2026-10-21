<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetPortlet.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Portal.GetPortlet" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
</head>
<body>
    <form id="form1" runat="server">

<div class="layer_popup">
    <div class="top-left"></div>
    <div class="top-right"></div>

    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("ConfigTabUser")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server" /></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="min-height:450px;">

            <div class="eip_portlet_left">
            <zeoPortletTree:UCPortletTreeScript ID="UCUCPortletTreeScript1" runat="server" UseIcon="true" UseActive="false" SetCallbackComplete="onCallbackComplete" SetNodeSelect="PortletNodeSelect_onNodeSelect" />
            </div>
            <div class="eip_portlet_mid">
                <div class="layout_scroll"  style="min-height:470px;height:470px;">

                <div class="table_typeLtop"><p></p></div>

<div class="table_typeLmiddle">
    <!--blog 내용 시작-->
    <div class="blog_cont">
        <ul>
            <li>
                <table class="blog_cont_table" border="0">
				    <caption>BLOG CONTENT</caption>
                    <colgroup>
                        <col width="120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td rowspan="3"><img src="http://www.google.com/images/gadgets/rss.gif" alt="img" style="width:120px;height:60px;" /></td>
                        <td>
                            <span style="font-weight:bold;"><%=GetResource("SearchOutputTitle")%></span>
                            <span class="subDate">
                                <ul class="btn_smallL" style="padding-right:10px;">
                                    <li><a href="#"  title="<%=GetResource("DirectAdd")%>"><span><%=GetResource("DirectAdd")%></span></a></li>
                                </ul>
                            </span>
                            </td>
                    </tr>
                    <tr>
                        <td>
                            ABCDEFGHIJKLMN
                       </td>
                    </tr>
                </table>
            </li>
        </ul>
    </div>


    <div class="blog_cont">
        <ul>
            <li>
                <table class="blog_cont_table" border="0">
				    <caption>BLOG CONTENT</caption>
                    <colgroup>
                        <col width="120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td rowspan="3"><img src="http://www.google.com/images/gadgets/rss.gif" alt="img" style="width:120px;height:60px;" /></td>
                        <td>
                            <span style="font-weight:bold;"><%=GetResource("SearchOutputTitle")%></span>
                            <span class="subDate">
                                <ul class="btn_smallL" style="padding-right:10px;">
                                    <li><a href="#"  title="<%=GetResource("DirectAdd")%>"><span><%=GetResource("DirectAdd")%></span></a></li>
                                </ul>
                            </span>
                            </td>
                    </tr>
                    <tr>
                        <td>
                            ABCDEFGHIJKLMN
                       </td>
                    </tr>
                </table>
            </li>
        </ul>
    </div>


    <div class="blog_cont">
        <ul>
            <li>
                <table class="blog_cont_table" border="0">
				    <caption>BLOG CONTENT</caption>
                    <colgroup>
                        <col width="120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td rowspan="3"><img src="http://g0.gstatic.com/ig/images/no_image/no_image_gadget_thm.png" alt="img" style="width:120px;height:60px;" /></td>
                        <td>
                            <span style="font-weight:bold;"><%=GetResource("SearchOutputTitle")%></span>
                            <span class="subDate">
                                <ul class="btn_smallL" style="padding-right:10px;">
                                    <li><a href="#"  title="<%=GetResource("DirectAdd")%>"><span><%=GetResource("DirectAdd")%></span></a></li>
                                </ul>
                            </span>
                            </td>
                    </tr>
                    <tr>
                        <td>
                            ABCDEFGHIJKLMN
                       </td>
                    </tr>
                </table>
            </li>
        </ul>
    </div>

    <div class="blog_cont">
        <ul>
            <li>
                <table class="blog_cont_table" border="0">
				    <caption>BLOG CONTENT</caption>
                    <colgroup>
                        <col width="120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td rowspan="3"><img src="http://www.google.com/images/gadgets/rss.gif" alt="img" style="width:120px;height:60px;" /></td>
                        <td>
                            <span style="font-weight:bold;"><%=GetResource("SearchOutputTitle")%></span>
                            <span class="subDate">
                                <ul class="btn_smallL" style="padding-right:10px;">
                                    <li><a href="#"  title="<%=GetResource("DirectAdd")%>"><span><%=GetResource("DirectAdd")%></span></a></li>
                                </ul>
                            </span>
                            </td>
                    </tr>
                    <tr>
                        <td>
                            ABCDEFGHIJKLMN
                       </td>
                    </tr>
                </table>
            </li>
        </ul>
    </div>

    <div class="blog_cont">
        <ul>
            <li class="last-child">
                <table class="blog_cont_table" border="0">
				    <caption>BLOG CONTENT</caption>
                    <colgroup>
                        <col width="120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td rowspan="3"><img src="http://www.google.com/images/gadgets/rss.gif" alt="img" style="width:120px;height:60px;" /></td>
                        <td>
                            <span style="font-weight:bold;"><%=GetResource("SearchOutputTitle")%></span>
                            <span class="subDate">
                                <ul class="btn_smallL" style="padding-right:10px;">
                                    <li><a href="#"  title="<%=GetResource("DirectAdd")%>"><span><%=GetResource("DirectAdd")%></span></a></li>
                                </ul>
                            </span>
                            </td>
                    </tr>
                    <tr>
                        <td>
                            ABCDEFGHIJKLMN
                       </td>
                    </tr>
                </table>
            </li>
        </ul>
    </div>

</div>

                <div class="table_typeLbottom"><p></p></div>

                </div>

                <!-- 페이징 시작 //-->
                <div id="Pager1" name="Pager1" class="paging_num_s1">
                    <h3>
                        <span id="pageInfo"></span>
                    </h3>
                    <span id="pager"></span>
                    <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight">
                        <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                        <asp:ListItem Text="10 " Value="10"></asp:ListItem>
                        <asp:ListItem Text="20 " Value="20"></asp:ListItem>
                        <asp:ListItem Text="30 " Value="30"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <!-- 페이징 끝 //-->            
            </div>
        </div>
    </div>

    <div class="bottom-left"></div>
    <div class="bottom-right"></div>
</div>

    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>  
    <script type="text/javascript" >
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_Notice = "<%=GetResource("Notice") %>";
        var R_Common = "<%=GetResource("Common") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Admin/Portal/GetPortlet.js" ></script>  
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/EIP/Js/Admin/Portal/GetPortlet.js");
    </script>
</body>
</html>
