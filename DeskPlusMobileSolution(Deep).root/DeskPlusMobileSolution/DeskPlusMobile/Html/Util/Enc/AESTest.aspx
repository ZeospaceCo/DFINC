<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AESTest.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Html.Util.Enc.AESTest" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
    <meta charset="uft8" />
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/aes.js"></script>
    
    <script type="text/javascript">

        function btnEncryptNewJavascript_Click() {

            var a = Aes.Ctr.encrypt(form1.DataTextBox.value, form1.KeyBox.value, 256);
            form1.ResultTextBoxNew.value = a;
        }
        function btnDecryptNew_Click() {

            var a = Aes.Ctr.decrypt(form1.ResultTextBoxNew.value, form1.KeyBox.value, 256);

            form1.DecryptTextBoxNew.value = a;
        }
       
    </script>
</head>
<body>
    <form id="form1" runat="server">

    <div>
        텍스트 : <asp:TextBox ID="DataTextBox" name="DataTextBox" runat="server" Width="335px"></asp:TextBox><br />
        암호키 : <asp:TextBox ID="KeyBox" name="KeyBox" runat="server" Width="331px"></asp:TextBox></br>
            
        변환결과 : <asp:TextBox ID="ResultTextBoxNew" name="ResultTextBoxNew" runat="server" Width="330px"></asp:TextBox>
        <asp:Button type="button" id="btnEncryptNew" Text="AES(서버)암호화"  runat="server" onclick="btnEncryptNew_Click" /><input type="button"  id="btnEncryptNewJavascript" value="AES(Javascript)암호화"  onclick="javascript:btnEncryptNewJavascript_Click();"/><br />        
        복호화(신규) :  <asp:TextBox ID="DecryptTextBoxNew" name="DecryptTextBoxNew" runat=server Width="328px"></asp:TextBox><input type="button"  id="Button3" value="AES(Javascript)복호화"  onclick="javascript:btnDecryptNew_Click();"/><br />
        복호화(신규-서버) : <asp:TextBox ID="DecryptTextBoxNewServer" name="DecryptTextBoxNewServer" runat=server Width="328px"></asp:TextBox>
        <asp:Button ID="btnDecryptNewServer" runat="server" Text="AES(서버)복호화" onclick="btnDecryptNewServer_Click" />
    </div>
    
    </form>
</body>
</html>
