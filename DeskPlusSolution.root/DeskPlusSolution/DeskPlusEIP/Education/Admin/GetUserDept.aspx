<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetUserDept.aspx.cs" Inherits="Zeospace.DeskPlus.Education.Web.Admin.GetUserDept" %>
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

        var R_SelectAdd = "추가할 대상을 선택해주세요";          
                
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Education/Js/Admin/GetUserDept.js?v=1"></script>  
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
       
        <div class="inside">
            <div class="popup_header">
                <h1>조직도</h1>
                <ul class="btn_right">
                   <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" class="btn btn-secondary" /></li>
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
                        <cc3:ClientSmartButton ID="CbtnAdd" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton>
                    </td>
          	        <td valign="top">       	        
                        <div class="layout_scroll"  style="min-height:417px;height:417px;">   
                        <table cellspacing="0" id="contentTable" class="table_typeB" style="width:100% !important;">  
                        <caption></caption>
                        <colgroup>
                            <col />
                            <col width="70px"/>
                        </colgroup>
                        <tr>
                            <th class="center">명칭</th>
                            <th class="center">삭제</th>
                        </tr>                
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

