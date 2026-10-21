<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetUserDept.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Common.GetUserDept" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var RowMax = 0;
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var CallBackEvent = "<%=CallBackEvent%>";
        var Gubun = "<%=Gubun%>";

        var R_SelectAdd = "<%= GetResource("SelectAdd") %>";   
        var R_Del = "<%=GetResource("Del")%>";             
                
    </script>
      
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Board/Js/Common/GetUserDept.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">

        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("Account")%></h1>
                <ul class="btn_right">
                   <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Class="btn btn-secondary" /></li>
                   <li><cc3:ClientSmartButton ID="CbtnClose" runat="server" /></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
   
		        <table cellspacing="0" border="0" style="width:100%;"> 
                <tr> 
          	        <td valign="top" width="270px" style="text-align:center;"> 
          	            <div class="layout_scroll"  style="width:270px;min-height:417px;height:417px;overflow:hidden">
                            <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" />
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
                            <col width="70px"/>
                        </colgroup>
                        <thead>
                        <tr>
                            <th class="center"><%=GetResource("TitleAll")%></th>
                            <th class="center"><%=GetResource("Del")%></th>
                        </tr>
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
    <input name="tvSelectedID" type="text" id="tvSelectedID" />
    <input name="tvSelectedValue" type="text" id="tvSelectedValue" />
    </div>    
    </form>
</body>
</html>
