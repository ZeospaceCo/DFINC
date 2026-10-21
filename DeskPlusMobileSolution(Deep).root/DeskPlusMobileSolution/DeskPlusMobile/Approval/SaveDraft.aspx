<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SaveDraft.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Approval.SaveDraft" %>
<!DOCTYPE html>
<html>
<head>
    <title></title>
    <meta charset="uft8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/<%=SkinType%>/navimobile.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Html/Js/JModal/DeskPlusModal.css"/>
</head>
<body class="mobile navihide scrolldown">
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div style="min-height:120px;">
            <div class="introcontentblock bottomcontent">
                <ul class="mobliecontent" >
                    <li class="poptitle text-primary">
                        <span>
                            <i class="fas fa-file-signature  dropbtn "></i><%=GetResource("PrivateDraft")%>
                        </span>
                        <div class="float-right date text-dark">
                            <button type="button" class="btn btn-xs btn-primary" id="btSave" style="float:none;"><%=GetResource("Save")%></button>
                            <button type="button" class="btn btn-xs btn-primary" id="btClose" style="float:none;"><%=GetResource("Close")%></button>
                        </div>
                    </li> 
                    <li class="contentblock">
                        <div class="title"><%=GetResource("PrivateDraftName")%></div>    
                        <div class="container-fluid" >   
                           <input type="text" id="tbDraftName" class="keyword form-control" maxlength="100" runat="server" />                                    
                        </div>                                           
                    </li>
                 </ul>                
             </div>
          </div>
    </div> 
    <div style="display: none">
        <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>

        <textarea id="htaFormHTML" style="DISPLAY: none" name="htaFormHTML" rows="2" cols="40" runat="server"></textarea>
        <textarea id="htaFormTEXT" style="DISPLAY: none" name="htaFormTEXT" rows="2" cols="40" runat="server"></textarea>
        <textarea id="htaEditorHTML" style="DISPLAY: none" name="htaEditorHTML" rows="2" cols="40" runat="server"></textarea>
        <asp:TextBox ID="htaEditorMime" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>

        <asp:textbox id="tbFormID" Runat="server" Width="0"></asp:textbox>
        <asp:textbox id="tbSwfUploadGUID" Runat="server" Width=""></asp:textbox>
        <asp:textbox id="tbUploadPath" Runat="server" Width=""></asp:textbox>
        <asp:textbox id="tbIsUseFlow" Runat="server" Width=""></asp:textbox>

        <asp:textbox id="tbIsGuard" Runat="server" Width="0"></asp:textbox>
        <asp:textbox id="tbAccessTitleID" Runat="server" Width="0"></asp:textbox>
        <asp:textbox id="tbIsLinkedEtc" Runat="server" Width="0"></asp:textbox>
        <asp:textbox id="tbCarbonView" Runat="server" Width="0"></asp:textbox>
        <asp:TextBox ID="tbBaseDocIDs" runat="server"></asp:TextBox>
        <textarea id="htaXmlSeq" style="DISPLAY: none" name="htaXmlSeq" rows="2" cols="40" runat="server"></textarea>
        <textarea id="htaXmlSeqZQ" style="DISPLAY: none" name="htaXmlSeqZQ" rows="2" cols="40" runat="server"></textarea>
        <textarea id="htaXmlSeqZC" style="DISPLAY: none" name="htaXmlSeqZC" rows="2" cols="40" runat="server"></textarea>

        <asp:TextBox ID="tbDraftID" runat="server" Width="0" Text="0"></asp:TextBox>
        <asp:TextBox ID="tbWriteTimeYMD" runat="server" Width="0" Text=""></asp:TextBox>
    </div>

    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">

        var R_Msg_InputDraftName = "<%=GetResource("Msg_InputDraftName") %>";
        var R_Msg_ConfirmUpdate = "<%=GetResource("Msg_ConfirmUpdate") %>";
        
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Approval/SaveDraft.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Approval/SaveDraft.js");
    </script>

</form>
</body>
</html>
