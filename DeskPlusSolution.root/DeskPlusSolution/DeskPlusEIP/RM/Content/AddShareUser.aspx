<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddShareUser.aspx.cs" Inherits="Zeospace.DeskPlus.RM.Web.Content.AddShareUser" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
</head>
<body>
    <form id="form1" runat="server">
        <!-- 테이블 상단환경 -->
    <div class="layer_popup">
    
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("ShareUserAdd")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnSubmit" runat="server" Class="btn btn-secondary" /></li>
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server" /></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
		        <table cellspacing="0" border="0" style="width:100%;"> 
                <tr> 
          	        <td valign="top" width="270px" style="text-align:center;">                      
          	            <div class="layout_scroll"  style="width:270px;min-height:417px;height:417px;overflow:hidden">
                         <zeoTree:UCUserDeptTreeScript ID="UCUserDeptTreeScript1" runat="server" UseActiveDept="true" UseCheckBoxGroup="true" UseCheckBoxMember="true" UseIcon="true" />
                        </div> 
                    </td>
          	        <td class="center" valign="top" style="width:100px;">
                        <cc3:ClientSmartButton ID="CbtnAdd" runat="server" Class="btn btn-secondary" ></cc3:ClientSmartButton>
                    </td>
          	        <td valign="top">
                        <div class="layout_scroll"  style="min-height:417px;height:417px;">
                            <table class="table_typeB" id="contentTable">
                                <caption>
                                    <%=GetResource("ObjePerson")%>
                                </caption>
                            <colgroup>
                                <col/>
                                <col width="100px"  />                    
			                </colgroup>
                            <thead>
                                <tr>
                                    <th class="center"><%=GetResource("ObjePerson")%></th>                            
                                    <th class="center"><%=GetResource("Del")%></th>
                                </tr>
                            </thead>
                            <tbody>                    
                                   <%=AuthTable%>                    
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
        <asp:TextBox ID="tbAuthValue" runat="server" Width="0"></asp:TextBox>
    </div>    
       
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" ></script>    
    <script type="text/javascript">
        var CatID = "<%=CatID %>";
        var CategoryID = "<%=CategoryID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var AuthArrayList = "<%=AuthArrayList %>";
        var CompanyID = "<%=CompanyID %>";
        var UserID = "<%=UserID %>";
        var PreLang = "<%=PreLang%>";        
        var R_Msg_AddShareUser = "<%=GetResource("Msg_AddShareUser") %>";
        var R_Msg_OverlapShareUser = "<%=GetResource("Msg_OverlapShareUser") %>";
        var R_Msg_AddMe = "<%=GetResource("Msg_AddMe") %>";
        var R_Del = "<%=GetResource("Del") %>"; 
    </script> 
    <script type="text/javascript" src="/DeskPlusEIP/RM/Js/Content/AddShareUser.js"></script>
</body>
</html>