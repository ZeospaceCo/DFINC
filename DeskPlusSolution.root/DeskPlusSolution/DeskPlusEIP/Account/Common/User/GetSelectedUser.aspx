<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetSelectedUser.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Common.User.GetSelectedUser" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript">
        var CompanyID = "<%=CompanyID %>";
        var DeptID = "0";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var CallBackPage = "<%=CallBackPage %>";
        var AuthArrayList = "";
        var ListID = "<%=ListID%>";
        var RowMax = 0;

        var R_NotCheckUser = "<%=GetResource("NotCheckUser") %>";
        var PageUrl = "<%= Request.Url.ToString() %>";
        var R_Admin_CompanyManager_btnDel = "<%=GetResource("Admin_CompanyManager_btnDel") %>";
    </script>
    <script type="text/javascript">
        JsCall("../../Js/Common/User/GetSelectedUser.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
      <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("UserState")%></h1>  
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="CbtnAddUser" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="CbtnAddDept" runat="server" BeginEndTag="li" />

                    <cc3:ClientSmartButton ID="CbtnSubmit" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="CbtnClose" runat="server" BeginEndTag="li" />
                </ul>
            </div>

            <div class="dotLine"></div>
            <div class="popup_layout_style3">    
		        <table cellspacing="0" border="0" style="width:100%;"> 
                <tr> 
          	        <td valign="top" width="270px" style="text-align:center;"> 
          	            <div class="layout_scroll"  style="width:270px;min-height:417px;height:417px;overflow:hidden">
                            <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" UseActiveDept="true" UseCheckBoxGroup="false" UseCheckBoxMember="true" UseIcon="true" />
                         </div>
                    </td>                              
          	        <td class="center" valign="top" style="width:100px;">
                        <cc3:ClientSmartButton ID="CbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton>
                    </td>
                    <td valign="top">       	        
                        <div class="layout_scroll"  style="min-height:417px;height:417px;"> 
                            <table id="contentTable" cellspacing="0" class="table_typeB" style="width:100% !important;"> 
                            <caption></caption>
                            <colgroup>
                                <col />
                                <col width="60px"/>
                            </colgroup> 
                            <thead>
                                <tr>
                                    <th class="center"><%=GetResource("SelectedUser") %></th>                    
                                    <th class="center""><%=GetResource("Admin_DeptManager_btnDel")%></th>
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
        <!-- 테이블 하단환경 끝 -->
        <div style="display: none">
            <input name="tvSelectedID" type="text" id="tvSelectedID" />
            <input name="tvSelectedValue" type="text" id="tvSelectedValue" />
        </div>           
    </form>
</body>
</html>
