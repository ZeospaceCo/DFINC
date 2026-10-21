<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Html.Sample.WebForm1" %>

<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <script src="/DeskPlusEIP/Html/Js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="/DeskPlusEIP/Html/Js/jQuery/validator.js" type="text/javascript"></script>    
    <script src="../Js/jQuery/jquery.money.js" type="text/javascript"></script>
    <script type="text/javascript">
        function test() {
            var validator = new Validator($("#form1"));
            validator.checkRequired("tbxID", "내역을  입력해주세요.", true);
            validator.checkMaxLength("tbxID", 30, "내역은 30자 이내입니다.", true);
            validator.checkRequired("tbxAmount", "금액을  입력해주세요.", true);
            validator.checkRegex("tbxAmount", /^[\d\-]*$/g, true, "금액은 숫자만 입력 가능합니다.", true);
            validator.checkIsNumber("tbxAmount", "금액은 숫자만 입력 가능합니다.", true);
            validator.checkMaxLength("tbxAmount", 9, "금액은 9자리까지 입력 가능합니다.", true);
            validator.checkRequired("ddlList", "종류를 입력해주세요.", true);            
            validator.checkAtLeastOneChecked("rdoList", "라디오를 입력해주세요.", true);
            validator.checkboxListChecked("cbxList", "체크박스를 입력해주세요.", true);

            if (!validator.validate()) return;
        }

        $(function () {
//            $('#tbxAmount').keyup(function (event) {
//                $(this).toPrice();
//                var strText = $(this).getOnlyNumeric();
//                //		var strText = getOnlyNumeric($(this).val());
//                $('#number').text(strText);
//            })
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="text" id="price" name="price" style="text-align: right;" /><br>
    get Original price value : <div id="number"></div>

    <div>
        <table style="width:100%;" border="1">
            <tr>
                <td>
                    &nbsp;아이디</td>
                <td>
                    &nbsp;<asp:TextBox ID="tbxID" runat="server"></asp:TextBox></td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;금액</td>
                <td>
                    &nbsp;<asp:TextBox ID="tbxAmount" runat="server" style="text-align: right;"></asp:TextBox></td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;종류</td>
                <td>
                    &nbsp;
                        <div id="list">
                            <asp:DropDownList ID="ddlList" runat="server">
                                <asp:ListItem Text="----" Value=""></asp:ListItem>
                                <asp:ListItem Text="피차" Value="1"></asp:ListItem>
                                <asp:ListItem Text="고기" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;종류</td>
                <td>
                    &nbsp;
                    <asp:CheckBoxList ID="cbxList" RepeatLayout="Flow" runat="server">
                        <asp:ListItem Text="test1" Value="test1"></asp:ListItem>
                        <asp:ListItem Text="test2" Value="test2"></asp:ListItem>
                    </asp:CheckBoxList>
                    </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;라디오</td>
                <td>
                    &nbsp;
                    
                    <asp:RadioButtonList ID="rdoList" RepeatLayout="Flow" runat="server">
                        <asp:ListItem Text="test1" Value="test"></asp:ListItem>
                        <asp:ListItem Text="test2" Value="test"></asp:ListItem>
                    </asp:RadioButtonList>
                    
                    </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;처리</td>
                <td>
                    &nbsp;<a onclick="test()">test</a></td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
