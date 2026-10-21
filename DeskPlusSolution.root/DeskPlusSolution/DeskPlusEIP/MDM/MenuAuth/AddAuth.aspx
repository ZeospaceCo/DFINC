<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddAuth.aspx.cs" Inherits="Zeospace.DeskPlus.MDM.Web.MenuAuth.AddAuth" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_MDM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_MDM.css" />   
</head>
<body>
<form id="form1" runat="server">
<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("RegAccessAuth")%></h1>
            <ul class="btn_right">                
                <cc3:ClientSmartButton ID="cbtnConfirm" BeginEndTag="li" Class="btn btn-secondary" runat="server"/>
                <cc3:ClientSmartButton ID="cbtnClose" BeginEndTag="li" runat="server"/>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">           
		    <table cellspacing="0" border="0" style="width:100%;"> 
            <tr> 
          	    <td valign="top" width="270px" style="text-align:center;"> 
          	        <div class="layout_scroll" style="width:270px;min-height:410px;height:410px;margin:1px 15px 5px 1px;padding:10px;outline:1px solid rgba(0,0,0,.1);overflow-y:auto;">
                        <zeoRoleTree:UCRoleTreeScript ID="UCRoleTreeScript1" runat="server" UseIcon="true" UseRole="true" UseGrade="false" SetCallbackComplete="onCallbackComplete" SetNodeSelect="RoleNodeSelect_onNodeSelect" />
                    </div>
                </td>
          	    <td class="center" valign="top" style="width:100px;">
                        <cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton>
                </td>
          	    <td valign="top">        	
                    <div class="layout_scroll"  style="min-height:417px;height:417px;">   
                    <table cellspacing="0" id="contentTable" class="table_typeB" style="width:90% !important;">  
                    <caption></caption>
                    <colgroup>
                        <col width="200px"  />
                        <col width="80px" />
                        <col width="60px" />
                    </colgroup>
                    <thead>
                    <tr>
                        <th class="center"><%=GetResource("Auth")%></th>
                        <th class="center"><%=GetResource("Section")%></th>
                        <th class="center"><%=GetResource("Del")%></th>
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
</div>
    <div style="display: none">                
        <input name="tvSelectedID" type="text" id="tvSelectedID" />
        <input name="tvSelectedValue" type="text" id="tvSelectedValue" />
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script> 
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>           
    <script type="text/javascript">    
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";  
        var MenuID = "<%=MenuID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var AuthArrayList = "<%=AuthArrayList %>";
        var RowMax = "<%=MaxRowMax %>";
        var R_SettingAuthority = "<%=GetResource("SettingAuthority") %>";             
        var R_Apply = "<%=GetResource("Apply") %>";   
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_Del = "<%=GetResource("Del") %>";  
    </script> 
    <script type="text/javascript" src="/DeskPlusEIP/MDM/Js/MenuAuth/AddAuth.js" ></script>     
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/MDM/Js/MenuAuth/AddAuth.js");
    </script>
</body>
</html>
