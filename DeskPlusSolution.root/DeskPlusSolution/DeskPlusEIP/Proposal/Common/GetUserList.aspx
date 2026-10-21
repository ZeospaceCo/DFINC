<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetUserList.aspx.cs" Inherits="Zeospace.DeskPlus.Proposal.Web.Common.GetUserList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Proposal.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Proposal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
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
    <script type="text/javascript" src="/DeskPlusEIP/Proposal/Js/Common/GetUserList.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Proposal/Js/Common/GetUserList.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">    
        <div class="inside">
            <div class="popup_header">
                <h1>대상선택</h1>
                <ul class="btn_right">                    
                   <cc3:ClientSmartButton ID="cbtnSubmit" runat="server" Text="확인" BeginEndTag="li" Class="btn btn-secondary" ></cc3:ClientSmartButton>
                   <cc3:ClientSmartButton ID="cbtnCancel" runat="server" Text="닫기" BeginEndTag="li"></cc3:ClientSmartButton>
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
                            <cc3:ClientSmartButton ID="CbtnAdd" runat="server" Text="추가" Class="btn btn-secondary"></cc3:ClientSmartButton>
                    </td>
          	        <td valign="top">
                        <!--ul>
                        <li><asp:CheckBox ID="CBSubDept" runat="server"/></li>
                        </ul-->          	
                        <div class="layout_scroll"  style="min-height:417px;height:417px;">   
                        <table cellspacing="0" border="0" id="contentTable" class="table_typeB">  
                        <caption></caption>
                        <colgroup>
                            <col/>
                            <col width="70px"/>
                        </colgroup>
                        <thead>
                        <tr>
                            <th>대상자</th>
                            <th class="center">삭제</th>
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
