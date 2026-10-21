<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminRoleAdd.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Attendance.AdminRoleAdd" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/ETC/Js/Attendance/AdminRoleAdd.js"></script>
    <script type="text/javascript">
    
    var UserList = "";
    var CompanyID = "<%=CompanyID %>";
    var UserID = 0;

    var R_DeptName = "<%=GetResource("DeptName") %>";
    var R_HoliDel = "<%=GetResource("HoliDel") %>";
    var R_NoContent = "<%=GetResource("NoContent") %>";
    var R_MSG_DELSUCCESS = '<%=GetResource("MSG_DELSUCCESS") %>'; 	     

    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>"; 

    </script> 
   
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("View")%><%=GetResource("AuthorityManagement")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Class="btn btn-secondary"  /></li>
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"   /></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
  
            <table class="table_typeB">
			<caption></caption>
			<colgroup>
                <col width="80px" />
                <col />
                <col width="100px" />
            </colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("Target")%></th>
                    <td><input type="text" style="width:98% !important;" id="txtUser" readonly="readonly" disabled="disabled" />
                    </td>
                </tr>
            </tbody>
            </table>
            
            <div class="layout_subbox">                
            </div>

		    <table cellspacing="0" border="0" style="width:100%;"> 
            <tr> 
          	    <td valign="top" width="270px" style="text-align:center;"> 
                    <div class="com_treebox">
                    <div class="com_srhSec">
                        <h2><%=GetResource("Attendance_Department")%></h2>
                    </div>
                    <div class="com_cotSec">
          	            <div class="layout_scroll"  style="width:270px;min-height:417px;height:417px;overflow:hidden">
                        <zeoDeptTree:UCDeptTreeScript ID="DeptTreeScript1" runat="server" />
                        </div>
                    </div>
                    </div>
                </td>
          	    <td class="center" valign="top" style="width:100px;">
                    <cc3:ClientSmartButton ID="CbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton>
                </td>
          	    <td valign="top">
                    <ul> 
                    <li><input type="checkbox" id="cbAuthSubApply" /><%=GetResource("List_SubDept")%></li>           
                    </ul>
                    <div class="layout_scroll"  style="min-height:380px;height:380px;">
                        <div id="placeholder" style="clear: both;"></div>
                    </div>
                        
          	    </td>            
            </tr> 
		    </table>    
		        
            </div>
        </div>
    </div> 

        
    <div style="display: none">
        <input name="tvSelectedID" type="text" id="tvSelectedID" />
        <input name="tvSelectedValue" type="text" id="tvSelectedValue" />
    </div>    
    </form>
</body>
</html>