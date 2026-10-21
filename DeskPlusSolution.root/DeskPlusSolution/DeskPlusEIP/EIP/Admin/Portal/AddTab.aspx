<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddTab.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Portal.AddTab" %>
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

        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("AdminTabSetting")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnList" runat="server" /></li>
                    <li><cc3:ClientSmartButton ID="cbtnOrderTab" runat="server" /></li>
                    <li class="sec"></li>
                    <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary" /></li>
                    <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
           
                <!--ul class="tab" >
                    <li><a href="#tabs-1" class="tab_on" title=""> <span><%=GetResource("AddTabInfo")%></span></a></li>
                </ul-->
                <table class="table_typeB">
			    <caption><%=GetResource("AddTabInfo")%></caption>
			    <colgroup>
                    <col width="100px" />
                    <col />
			    </colgroup>
			    <tbody>
                    <tr>
                        <th><%=GetResource("TabName")%>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" alt="" title="" /></a></th>
                        <td>
                            <cc3:ClientSmartTextBox ID="ctbBaseName" InputMode="None" MaxLength="250" runat="server" Required="true" style="width:40% !important;"></cc3:ClientSmartTextBox>
                            <cc3:ClientSmartTextBox ID="ctbMultiName" InputMode="None" MaxLength="250" runat="server" style="width:40% !important;"></cc3:ClientSmartTextBox>                            
                        </td>
                    </tr>
                    <tr style="display:none">
                        <th><%=GetResource("PersonalPortlet")%></th>
                        <td>
                            <asp:DropDownList CssClass="searchselect form-control" ID="ddlIsModify" runat="server" />
                            <%=GetResource("DelPortletMsg")%>
                        </td>
                    </tr>
                    <tr>
                        <th><label title="<%=GetResource("Layout")%>"><%=GetResource("Layout")%></label></th>
                        <td>
                        
                            <ul class="aLeft">
                                <li><img id="imglayout1" runat="server" /><br /><input type="radio" name="rbLayout" checked value="1" id="rbLayout1" runat="server" /><label title="<%=GetResource("Col_1")%>"><%=GetResource("Col_1")%></label></li>
                                <li><img id="imglayout2" runat="server" /><br /><input type="radio" name="rbLayout" value="2" id="rbLayout2" runat="server" /><label title="<%=GetResource("Col_2_Share")%>"><%=GetResource("Col_2_Share")%></label></li>
                                <li><img id="imglayout3" runat="server" /><br /><input type="radio" name="rbLayout" value="3" id="rbLayout3" runat="server" /><label title="<%=GetResource("Col_2_Left")%>"><%=GetResource("Col_2_Left")%></label></li>
                                <li><img id="imglayout4" runat="server" /><br /><input type="radio" name="rbLayout" value="4" id="rbLayout4" runat="server" /><label title="<%=GetResource("Col_2_Right")%>"><%=GetResource("Col_2_Right")%></label></li>
                                <li><img id="imglayout5" runat="server" /><br /><input type="radio" name="rbLayout" value="5" id="rbLayout5" runat="server" /><label title="<%=GetResource("Col_3_Center")%>"><%=GetResource("Col_3_Center")%></label></li>
                            </ul>
                            <ul class="aLeft" style="padding-top:5px;">
                                <li><img id="imglayout6" runat="server" /><br /><input type="radio" name="rbLayout" value="6" id="rbLayout6" runat="server" /><label title="<%=GetResource("Col_3_Share")%>"><%=GetResource("Col_3_Share")%></label></li>
                                <li><img id="imglayout7" runat="server" /><br /><input type="radio" name="rbLayout" value="7" id="rbLayout7" runat="server" /><label title="<%=GetResource("Col_4_Share")%>"><%=GetResource("Col_4_Share")%></label></li>
                            </ul>

                        </td>
                    </tr>
                    <tr>
                        <th><%=GetResource("IsActive")%></th>
                        <td>
                            <asp:DropDownList CssClass="searchselect form-control" ID="ddlActive" runat="server" />                            
                        </td>
                    </tr>
                
                </tbody>
                </table>
                <div style="min-height:80px;height:80px;"><%=GetResource("GotoTabAuth")%></div>
                <div class="layout_scroll" style="min-height:230px;height:230px;"></div>
            </div>
        </div>

        <div class="bottom-left"></div>
        <div class="bottom-right"></div>
    </div>
    <div style="display:none;">
        <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>  
    <script type="text/javascript" >
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_InputTabName = "<%=GetResource("InputTabName") %>";
        var R_SettingLayout = "<%=GetResource("SettingLayout") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Admin/Portal/AddTab.js" ></script>  
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/EIP/Js/Admin/Portal/AddTab.js");
    </script>
</body>
</html>
