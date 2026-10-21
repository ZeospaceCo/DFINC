<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetUser.aspx.cs" Inherits="Zeospace.DeskPlus.SNS.Web.MyTalk.Content.GetUser" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_SNS.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_SNS.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
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

        var R_Del = "<%=GetResource("Del") %>";
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
    </script> 
    <script type="text/javascript" src="/DeskPlusEIP/SNS/Js/MyTalk/Content/GetUser.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- 테이블 상단환경 -->
    <div class="layer_popup">
        
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("OpenUserManager")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnSubmit" runat="server" class="btn btn-secondary" /></li>
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"   /></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
		        <table cellspacing="0" border="0" style="width:100%;"> 
                <tr> 
          	        <td valign="top" width="270px" style="text-align:center;"> 
          	            <div class="layout_scroll"  style="width:270px;min-height:417px;height:417px;overflow:hidden">
                          <zeoTree:UCUserDeptTreeScript ID="UCUserDeptTreeScript1" runat="server" UseActiveDept="true" UseCheckBoxGroup="true" UseCheckBoxMember="true" UseIcon="true"  />
                        </div> 
                    </td>
          	        <td class="center" valign="top" style="width:100px;">
                        <cc3:ClientSmartButton ID="CbtnAdd" runat="server" Class="btn btn-secondary" ></cc3:ClientSmartButton>
                    </td>
          	        <td valign="top">
                        <div class="layout_scroll"  style="min-height:417px;height:417px;">   
                            <table class="table_typeB" id="contentTable">
                                <caption>
                                    <%=GetResource("OpenUserManager")%>
                                </caption>
                            <colgroup>
                                <col/>
                                <col width="70px"  />                    
			                </colgroup>
                            <thead>
                                <tr>
                                    <th><%=GetResource("CommentShare")%></th>                            
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
        <asp:linkbutton id="LBTNGetData" runat="server" OnClick="LBTNGetData_Click"></asp:linkbutton>
        <asp:TextBox ID="tbAuthValue" runat="server" Width="0"></asp:TextBox>
    </div>    
       
    </form>
</body>
</html>
