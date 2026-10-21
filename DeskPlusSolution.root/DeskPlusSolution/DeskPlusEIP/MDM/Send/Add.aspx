<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.MDM.Web.Send.Add" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_MDM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_MDM.css"/>
    <link rel="stylesheet" href="/DeskPlusEIP/Html/Js/jQuery/jquery-ui.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>     
    <script type='text/javascript' src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script> 
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>       
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.autogrow-textarea.js"></script>  
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery-ui.js"></script>  
    <script type="text/javascript" >
        var ImagePath = "<%=BaseImgPath %>";
        var PreLang = "<%=PreLang %>";
        var BtnCheck = "N";     

        var R_InputHeader =  "<%=GetResource("InputHeader") %>"; 
        var R_InputReceive =  "<%=GetResource("InputReceive") %>"; 
        var R_InputContent =  "<%=GetResource("InputContent") %>"; 
        var R_MsgSendAlarm =  "<%=GetResource("MsgSendAlarm") %>"; 
        var R_MsgOverlap = "<%=GetResource("MsgOverlap") %>"; 
        var R_MsgNoUser = "<%=GetResource("MsgNoUser") %>"; 
        var R_MsgKey = "<%=GetResource("MsgKey") %>"; 
        var R_MsgBoardAuth = "<%=GetResource("MsgBoardAuth") %>"; 
        var R_Processing = "<%=GetResource("Processing") %>"; 
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>"; 
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/MDM/Js/Send/Add.js"></script>
    <script type="text/javascript">
         //JsCall("/DeskPlusEIP/MDM/Js/Send/Add.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">    
        <div class="inside" style="min-height:410px;">
            <div class="popup_header">
                <h1><%=GetResource("PushAdd")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnSend" runat="server" Class="btn btn-secondary" /></li>
                    <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style">     
               <div class="layout_scroll" style="min-height:300px;height:300px;padding:10px;">
                    <table class="table_typeB">
                        <caption></caption>
                        <colgroup>
                            <col width="80px" />
                            <col />
                            <col width="55px" />
                            <col width="55px" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th><%=GetResource("Headers")%></th>
                                <td colspan="3"><cc3:ClientSmartTextBox runat="server" ID="tbxHeader" Width="98%"></cc3:ClientSmartTextBox></td>
                            </tr> 
                            <tr>
                                <th><%=GetResource("ReceiveName")%></th>
                                <td colspan="2"><textarea rows="1" style="height:2.5em;float:left; width:90%; margin-right:5px; padding:5px;" id="tbReceive"></textarea></td>
                                <td><ul class="btn_right"><cc3:ClientSmartButton ID="cbtnDept" runat="server" BeginEndTag="li" Class="btn btn-sm"></cc3:ClientSmartButton></ul></td>
                            </tr> 
                            <tr>
                                <th><%=GetResource("Content")%></th>
                                <td colspan="3" style="height:150px">  
                                <asp:TextBox ID="tbContent" runat="server" TextMode="MultiLine" Width="93%" Height="140px" MaxLength="200"></asp:TextBox>                       
                                </td>
                            </tr> 
                            <tr>
                                <th><%=GetResource("TargetUrl")%></th>
                                <td><span id="sTargetUrl"></span></td>
                                <td><ul class="btn_right"><cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" Class="btn btn-sm"></cc3:ClientSmartButton></ul></td>
                                <td><ul class="btn_right"><cc3:ClientSmartButton ID="cbtnDel" runat="server" BeginEndTag="li" Class="btn btn-sm"></cc3:ClientSmartButton></ul></td>
                            </tr>          
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <!-- ================================== Hidden Input ===================================== -->
    <div style="display: none">
        <input id="hdnCategoryID" type="hidden" name="hdnCategoryID" runat="server">
        <input id="hdnListID" type="hidden" name="hdnListID" runat="server">
    </div>
    <!-- ==================================================================================== -->
    </form>
</body>
</html>
