<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddressAdd.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Mail.AddressAdd" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C/Css/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C1/Css/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C/Css/Func_Community.css"/><link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C1/Css/Func_Community.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">                                                                                                                                                                                                                                                                       <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("AddAdress")%></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary" />
                <cc3:ClientSmartButton ID="cbtnClose" runat="server" />
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            
        <div id="layout_content"><!-- layout_info -->
        <div class="row">
        <div class="col-12 content-area-body"> <!-- width 100%(col-12) -->            
            <!-- 테이블 시작 -->
		    <table class="table_typeB"> 
            <caption>주소추가</caption>
		    <colgroup>
                <col width="80px" />
                <col />
		    </colgroup>
            <tbody>
            <tr> 
                <th><%=GetResource("FirstName")%></th> 
                <td><input name="ctbName" type="text" maxlength="100" id="ctbName" class="text required" style="width:90% !important;" /></td>                 
            </tr> 
            <tr> 
                <th><%=GetResource("Email")%></th> 
                <td><input name="ctbMail" type="text" maxlength="100" id="ctbMail" class="text required" style="width:95% !important;" readOnly /></td> 
            </tr> 
            <tr> 
                <th><%=GetResource("PrivateAddress")%></th>
                <td>
                <asp:DropDownList ID="ddlAddressBook" runat="server" CssClass="searchselect form-control" />
                </td>
            </tr> 
            </tbody>
            </table> 
            <!-- 테이블 끝 -->      
        </div>
        </div>     
        </div>

        </div>
    </div>
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
    <script language="javascript">   
        var R_NotCorrectEmailFormat = "<%=GetResource("NotCorrectEmailFormat") %>";
		var R_Complete = "<%=GetResource("Complete") %>";
        var UserName = "<%= Name %>";
        var UserEmail = "<%= Email %>";
        var CompanyID = "<%=CompanyID %>";
        var UserID = "<%=UserID %>";
        var PreLang = "<%=PreLang %>";       
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Mail/AddressAdd.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Address/Js/Mail/AddressAdd.js");
    </script>
</body>
</html>
