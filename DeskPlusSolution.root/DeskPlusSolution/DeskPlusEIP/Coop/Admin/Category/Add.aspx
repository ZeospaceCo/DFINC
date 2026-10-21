<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Coop.Web.Admin.Category.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Coop.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>

    <script type="text/javascript" >
        
		var ItemID = "<%=ItemID %>";
        var ItemSubID = "<%=ItemSubID %>";

        var R_TemplateName = "<%=GetResource("TemplateName") %>";
        var R_Add_InputContent = "<%=GetResource("Add_InputContent") %>";
        var R_DoubtStcipt = "<%=GetResource("DoubtStcipt") %>";
        var R_ConfirmDeleteList = "<%=GetResource("ConfirmDeleteList") %>";

        var ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";
        var ImagePath = "<%=BaseImgPath %>";
        var NonActiveX = "<%=NonActiveX%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja 
        var ClientQueryString = "<%=Page.ClientQueryString %>";        
    </script>
    <script type="text/javascript">
        JsCall("../../Js/Admin/Category/Add.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">

<div class="layer_popup">
        <div class="top-left"></div>
        <div class="top-right"></div>
        <div class="inside">
            <div id="layout_header">
            <!-- 페이지타이틀,버튼 //-->
            <h1><%=GetResource("Template")%></h1>
            <ul class="btn_right">        
                <li class="btn_blue"><cc3:ClientSmartButton ID="CbtnConfirm" runat="server"></cc3:ClientSmartButton></li>                
                <li><cc3:ClientSmartButton ID="CbtnCancel" runat="server"></cc3:ClientSmartButton></li>
            </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:630px;">
                    

                <table class="table_typeB">
                <caption><%=GetResource("Template")%></caption>
			    <colgroup>
                    <col width="80px" />
                    <col />
			    </colgroup>
			    <tbody>
                    <tr>
                        <th><%=GetResource("Title")%></th>
                        <td><cc3:ClientSmartTextBox ID="ctbName" InputMode="None" MaxLength="250" runat="server" Required="true" ScriptSubmitFunName="Add.Add()" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
                    </tr>
                </tbody>
                </table>
                <div id="tabs" class="layout_subbox">
                    <ul id="tab" class="tab">
                        <li><a id="ActvieX"  class="tab_on"><span>ActiveX</span> </a></li>
                        <li><a id="NonActiveX"><span>NonActiveX</span> </a></li>
                    </ul>
                    <div class="divEditor">
                        <script type="text/javascript" src="/DeskPlusEIP/Html/Js/IEActiveX/<%=PreLang%>/activexTagfree.js"></script>
                    </div>
                </div>


            </div>                
        </div>
        <div class="bottom-left"></div>
        <div class="bottom-right"></div>
    </div>

    <div style="display: none;">
        <!-- 에디터에 필요한 필드 //-->
        <asp:TextBox ID="tbHiddenSelectedContent" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenMime_contents" Style="display: none" Width="0" TextMode="MultiLine"  runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0" runat="server"></asp:TextBox>        
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server" style="display: none" rows="1" cols="20"></textarea>        
        <asp:linkbutton id="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:linkbutton>        
        <!-- ==================================================================================== -->
    </div>

</form>
</body>
</html>
