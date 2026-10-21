<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetUserList.aspx.cs" Inherits="Zeospace.DeskPlus.Coop.Web.Common.GetUserList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PMS.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PMS.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var PreLang = "<%=PreLang%>";
        var Gubun = "<%=Gubun%>";

        var RowMax = "1";        
        var ShareArrayList = "";
        var ImagePath = "<%=BaseImgPath%>";
    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Coop/Js/Common/GetUserList.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
<div class="layer_popup">
        <div class="top-left"></div>
        <div class="top-right"></div>

        <div class="inside">
            <div class="popup_header">
                <h5><%=GetResource("SelTarget")%></h5>
                <ul class="btn_right">                    
                   <cc3:ClientSmartButton ID="cbtnSubmit" runat="server" BeginEndTag="li" BeginEndTagClass="btn_blue"></cc3:ClientSmartButton>
                   <cc3:ClientSmartButton ID="cbtnCancel" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
		        <table cellspacing="0" border="0" style="width:100%;"> 
                <tr> 
          	        <td valign="top" width="35%" style="text-align:center;"> 
          	            <div style="min-height:400px;height:400px;">
                            <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" />
                         </div>
                    </td>
          	        <td class="center" valign="top" style="padding-top:60px;width:100px;">
                            <cc3:ClientSmartButton ID="CbtnAdd" runat="server"></cc3:ClientSmartButton>
                    </td>
          	        <td valign="top">
                        <!--ul>
                        <li><asp:CheckBox ID="CBSubDept" runat="server"/></li>
                        </ul-->          	
                        <div class="layout_scroll"  style="min-height:380px;height:380px;">   
                        <table cellspacing="0" border="0" id="contentTable" class="table_typeB">  
                        <caption></caption>
                        <colgroup>
                            <col/>
                            <col width="70px"/>
                        </colgroup>
                        <thead>
                        <tr>
                            <th><%=GetResource("Target")%></th>
                            <th><%=GetResource("Del")%></th>
                        </tr>      
                        </thead>
                        <tbody>                        
                        </tbody>  
                        </table>
                        </div>
          	        </td>            
                </tr> 
		        </table>    
		        
            </div>
        </div>

        <div class="bottom-left"></div>
        <div class="bottom-right"></div>
    </div> 
 
    <!-- ================================== Hidden Input ===================================== -->
    <div style="display: none">
        <input id="hdnUserID" type="hidden" name="hdnUserID" runat="server">
        <asp:TextBox ID="tvSelectedID" runat="server" Width="0" Height="0" Style="display: none"></asp:TextBox>
        <asp:TextBox ID="tvSelectedValue" runat="server" Width="0" Height="0" Style="display: none"></asp:TextBox>
        <asp:TextBox ID="tvTypes" runat="server" Width="0" Height="0" Style="display: none"></asp:TextBox>
    </div>
    <!-- ==================================================================================== -->
    </form>
</body>
</html>
