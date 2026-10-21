<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddPrivate.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Content.AddPrivate" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register src="../Common/AddressMailTree.ascx" tagname="AddressMailTree" tagprefix="uc1" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>   
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C/Css/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C1/Css/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C/Css/Func_Community.css" /><link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C1/Css/Func_Community.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form name="PrivateForm" action="/DeskPlusEIP/Address/Content/PrivateTree.aspx?listAll=" method="post">
        <input type="hidden" name="listAll">
    </form>

    <form id="form1" runat="server">
    <div class="layer_popup">      
<div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("AddressList")%></h1>
            <ul class="btn_right">                
                <cc3:ClientSmartButton id="cbtnAddPrivate" BeginEndTag="li" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton>
                <cc3:ClientSmartButton ID="cbtnClose" runat="server" BeginEndTag="li" />
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">            
            <table cellspacing="0" border="0" style="width:99%;">                
                <tr> 
          	        <td style="vertical-align:top;" id="treeTD" class="cm_srhline"> 
                        <div style="height:280px;width:230px;overflow:auto">
                        <uc1:AddressMailTree ID="UCAddressMailTree" UseActiveDept="true" UseIcon="true" UseCheckBoxGroup="false" UseCheckBoxMember="false" UseCategory="GG-1,CG-2,PG-3,RG-4,TG-5" runat="server" SetNodeSelect="onNodeSelect" SetCallbackComplete="onCallbackComplete" />
                        </div>
                    </td>
                    <td>&nbsp;</td>
          	        <td style="width:620px; vertical-align:top;">
                        <div id="layout_srh">
                            <ul class="btn_searchbox">
                                <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                                <li><cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px"  InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox></li>
                                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>
                            </ul>
                        </div>
                        <div class="cm_srhcont">
                        <div style="min-height:230px;height:230px; overflow-y:auto; overflow-x:hidden;" id="divData">
                        
                        <div id="phList" style="clear: both;"></div>  
                        
                        </div> 
                        </div>    

                    </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div style="min-height:230px;height:230px;overflow-y:auto; overflow-x:hidden;padding-top: 5px;">
                    <!-- 사용자 리스트 리스트 -->   
                    <table cellspacing="0" class="table_typeA" id="contentTable" >
                        <caption></caption>
                        <colgroup>                         
                            <col width="150px" />
                            <col width="100px" />
                            <col width="120px" />
                            <col width="*"/>  
                            <col width="80px" />
                        </colgroup>
                        <tr>                         
                            <th><%=GetResource("Dept")%></th> 
                            <th><%=GetResource("Title")%></th>                             
                            <th><%=GetResource("FirstName")%></th>
                            <th><%=GetResource("Email")%></th>
                            <th><%=GetResource("Delete")%></th>
                        </tr>          
                    </table>                    
                    </div>
                </td>
            </tr>
            </table>

        </div>
    </div>  
</div>    
    <div style="display:none">
        <!-- ================================== Hidden Input ===================================== -->
        <input id="keyValue" type="hidden" name="keyValue" />
        <input id="tvSelectedValue" type="text" size="0" name="tvSelectedValue" runat="server" />
        <input id="lbxSelectedValue" type="text" size="0" name="lbxSelectedValue" runat="server" />
        <input id="lbxToSelectedValue" type="text" size="0" name="lbxToSelectedValue" runat="server" />
        <input id="lbxCCSelectedValue" type="text" size="0" name="lbxCCSelectedValue" runat="server" />
        <input id="lbxBCCSelectedValue" type="text" size="0" name="lbxBCCSelectedValue" runat="server" />
        <!-- ==================================================================================== -->
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        var MailProduct = "";
        var Gubun = "";
        var FolderType = "";        
        var FolderCategoryID = "0";       
        var SearchField = ""; 
        var SearchValue = ""; //검색어
        var RowMax = 0;
        var CompanyID = "<%=CompanyID %>";
        var UserID = "<%=UserID %>";
        var PreLang = "<%=PreLang %>";
        var R_DeptName = "<%=GetResource("Dept")%>";
        var R_TitleName = "<%=GetResource("Title")%>";
        var R_UserName = "<%=GetResource("FirstName")%>";
        var R_Email = "<%=GetResource("Email")%>";
        var R_Del = "<%=GetResource("Delete") %>";
        var R_NoContent = "<%=GetResource("NoContent")%>";
        var R_SelUser = "<%=GetResource("SelectUser")%>"; 
        var R_NotUserSelect = "<%=GetResource("NotUserSelect")%>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Content/AddPrivate.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Address/Js/Content/AddPrivate.js");
    </script>
</body>
</html>
