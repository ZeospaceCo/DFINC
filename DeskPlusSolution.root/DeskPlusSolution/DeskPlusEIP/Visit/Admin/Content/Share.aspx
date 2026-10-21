<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Share.aspx.cs" Inherits="Zeospace.DeskPlus.Visit.Web.Admin.Content.Share" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Visit.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Visit.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <h1><%=GetResource("ShareManager")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnSave" runat="server" Class="btn btn-secondary" /></li>
            </ul>
        </div>

        <div id="layout_content">   
		    <table cellspacing="0" border="0" style="width:100%;"> 
            <tr> 
          	    <td valign="top" width="270px" style="text-align:center;">                      
          	        <div class="layout_scroll"  style="width:270px;min-height:417px;height:417px;overflow:hidden">
                        <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" UseActiveDept="true" UseCheckBoxGroup="true" UseCheckBoxMember="true" UseIcon="true" />
                        </div>
                </td>
          	    <td class="center" valign="top" style="width:100px;">
                        <cc3:ClientSmartButton ID="CbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton>
                </td>
          	    <td valign="top">       	        
                    <div class="layout_scroll"  style="min-height:417px;height:417px;">
                    <table cellspacing="0" id="contentTable" class="table_typeB" style="width:100% !important;">  
                    <caption></caption>
                    <colgroup>
                        <col />
                        <col width="60px"/>
                    </colgroup>
                    <thead>
                    <tr>
                        <th style="font-weight:bold;text-align:center;"><%=GetResource("Name")%></th>
                        <th style="font-weight:bold;text-align:center;"><%=GetResource("Del")%></th>
                    </tr>      
                        <%=AuthTable%>  
                    </thead>        
                    </table>
                    </div>
          	    </td>            
            </tr> 
		    </table>    
        </div>
    </div>
    </div>  
    <div style="display: none">    
    <asp:TextBox ID="tbShareValue" runat="server" Width="0"></asp:TextBox>      
    <asp:linkbutton id="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:linkbutton>
    <input name="tvSelectedID" type="text" id="tvSelectedID" />
    <input name="tvSelectedValue" type="text" id="tvSelectedValue" />
    </div>    
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var RowMax = 0;
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var AuthArrayList = "<%=AuthArrayList %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";   
        var R_Del = "<%=GetResource("Del") %>";             
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Visit/Js/Admin/Content/Share.js"></script>  
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Visit/Js/Admin/Content/Share.js");
    </script>
</body>
</html>

