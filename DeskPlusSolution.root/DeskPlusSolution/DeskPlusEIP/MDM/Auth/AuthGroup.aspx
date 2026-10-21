<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuthGroup.aspx.cs" Inherits="Zeospace.DeskPlus.MDM.Web.Auth.AuthGroup" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_MDM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_MDM.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />   
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
     <div class="layout_full">
        <div id="layout_header">
            <h1><%=GetResource("AuthGroup")%></h1>
            <ul class="btn_right"><li><cc3:ClientSmartButton ID="CbtnSave" runat="server" Class="btn btn-secondary" /></li></ul>
            <ul><li><span class="mdm_subtxt"><%=GetResource("Desc3")%></span></li></ul>
        </div>

        <div id="layout_content">  
            <div class="row">
			  <div class="col-12 content-area-body"><!-- width 100%(col-12) --> 
		        <table cellspacing="0" border="0" style="width:100%;"> 
                <tr> 
          	        <td valign="top" width="370px" style="text-align:center;"> 
          	            <div class="layout_scroll"  style="width:370px;min-height:417px;height:417px;">
                            <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" UseActiveDept="true" UseCheckBoxGroup="true" UseCheckBoxMember="true" UseIcon="true" />
                         </div>
                    </td>
          	        <td class="center" valign="top" style="width:100px;">
                        <cc3:ClientSmartButton ID="CbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton>
                    </td>
          	        <td valign="top">       	        
                        <div class="layout_scroll" style="min-height:417px;height:417px;">   
                        <table cellspacing="0" id="contentTable" class="table_typeB" style="width:95% !important;">  
                        <caption></caption>
                        <colgroup>
                            <col />
                            <col width="70px"/>
                        </colgroup>
                        <thead>
                        <tr>
                            <th class="center"><%=GetResource("HearderName")%></th>
                            <th class="center"><%=GetResource("Del")%></th>
                        </tr>      
                         <%=AuthTable%>          
                        </table>
                        </div>
          	        </td>            
                </tr> 
		        </table>    
              </div>
            </div>
        </div>
    </div>
    </div>  
    <div style="display: none">    
    <asp:TextBox ID="tbAuthValue" runat="server" Width="0"></asp:TextBox>      
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
        var R_CheckUserGroup = "<%=GetResource("CheckUserGroup") %>";
        var R_Del = "<%=GetResource("Del") %>";                
    </script>      
    <script type="text/javascript" src="/DeskPlusEIP/MDM/Js/Auth/AuthGroup.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/MDM/Js/Auth/AuthGroup.js");
    </script>
</body>
</html>

