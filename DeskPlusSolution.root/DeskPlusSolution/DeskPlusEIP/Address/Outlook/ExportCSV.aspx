<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportCSV.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Outlook.ExportCSV" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Address.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Address.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
        <div id="layout_header"></div>
        <div class="layout_srh clearfix"> 
        <div class="float-left">
            <ul id="ulTabMenu" class="tab">
            <li><a id="csvContact" href="#"><%=GetResource("GetOutLooK")%></a></li>
            <li style="display:none"><a href="#" id="googleContact"><%=GetResource("GetGoogleMail")%></a></li>
            <li class="selected"><a href="#"  id="exportCsv"><%=GetResource("ExportAdress")%></a></li>
            </ul>
        </div>
    </div> 
    <!-- //layout_header -->
    <div class="outmail">
        <p class="outmail_descrip">
            <%=GetResource("ExportCommand1")%><br />
            <%=GetResource("ExportCommand2")%>
        </p>
        <div class="outmail_select">
            <span class="btnWrap">
                <span class="btn_address"><input id="csvDaum" name="csvGubun" type="radio" value="daum" /><img src="<%=BaseImgPath%>/Icon/ico_mobile.gif" alt="<%=GetResource("DaumCsv")%>" /><a href="#"> <%=GetResource("DaumCsv")%></a></span>
                <span class="btn_address"><input id="csvNaver" name="csvGubun" type="radio" value="naver" /><img src="<%=BaseImgPath%>/Icon/ico_address.gif" alt="<%=GetResource("NaverCsv")%>" /><a href="#"> <%=GetResource("NaverCsv")%></a></span>
                <span class="btn_address"><input id="csvGoogle" name="csvGubun" type="radio" value="google" /><img src="<%=BaseImgPath%>/Icon/ico_outlook.gif" alt="<%=GetResource("GoogleCsv")%>" /><a href="#"> <%=GetResource("GoogleCsv")%></a> </span>
                <span class="btn_address"><input id="csvOutlook" name="csvGubun" type="radio" value="outlook" /><img src="<%=BaseImgPath%>/Icon/ico_outlook.gif" alt="<%=GetResource("OutlookCsv")%>" /><a href="#"> <%=GetResource("OutlookCsv")%></a> </span>
                <span class="btn_address last-child"><input id="csvExpress" name="csvGubun" type="radio" value="express" /><img src="<%=BaseImgPath%>/Icon/ico_outlook.gif" alt="<%=GetResource("ExpressCsv")%>" /><a href="#"> <%=GetResource("ExpressCsv")%></a></span>
           </span>

            <div class="addfile" style="border:0px solid #f00;">
                <span class="btnWrap">
                    <span class="list_add_style4">                
                        <a href="#" id="cbtnExportCSV" Class="btn btn-secondary" ><%=GetResource("Export")%></a> 
                    </span>
                </span>
        </div>        
    </div>    

            </div>
        </div>
    </div>
    <div style="display:none">
        <asp:TextBox ID="tbTypes" runat="server" Width="0"></asp:TextBox>
        <asp:LinkButton ID="lbtnSumbit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var WindowObjectReference; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_SelectOutLookFormat = "<%=GetResource("SelectOutLookFormat")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Outlook/ExportCSV.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Address/Js/Outlook/ExportCSV.js");
    </script>
</body>
</html>
