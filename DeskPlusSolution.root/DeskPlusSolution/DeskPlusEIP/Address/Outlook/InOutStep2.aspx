<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InOutStep2.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Outlook.InOutStep2" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Address.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Address.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_full">
        <div id="layout_header"></div>
        <div class="layout_srh clearfix"> 
            <div class="float-left">
            <ul id="ulTabMenu" class="tab">
                <li class="selected"><a id="csvContact" href="#"><%=GetResource("GetOutLooK")%></a></li>
                <li style="display:none"><a href="#" id="googleContact"><%=GetResource("GetGoogleMail")%></a></li>
                <li><a href="#"  id="exportCsv"><%=GetResource("ExportAdress")%></a></li>
            </ul>    
            </div>
        </div> 
    <!-- //layout_header -->
    <div id="layout_content" class="outmail">
        <div class="row">
        <div class="col-12 content-area-body"> <!-- width 100%(col-12) -->             
            <ul class="outmail_location">
                <li><%=GetResource("Load")%></li>
                <li><a class="over"><%=GetResource("UsePreview")%></a></li>
                <li class="last-child"><%=GetResource("Save")%></li>
            </ul>
            <div class="outmail_select">
                <p class="descrip"><%=GetResource("Total")%> <em class="ft_blue"><%=CntAll%></em><%=GetResource("InOutStep2_Descript1")%></p>
            </div>
            <ul class="btn_right">            
                <cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" />
            </ul>
            <div id="layout_info2">
                <div class="listAtoZ fl_left">
                    <ul>
                        <li class="first-child">&lowast;<a class="over" href="#" id="GetAll"><%=GetResource("Whole")%>(<%=CntAll%>)</a></li>
                        <li><a href="#" id="GetExcept"><%=GetResource("ExceptRepetition")%>(<%=CntExcept%>)</a></li>
                        <li><a href="#" id="GetOverlap"><%=GetResource("Repetition")%>(<%=CntOverlap%>)</a></li>
                    </ul>
                </div>
                <!-- 테이블 시작 -->
                        
                <div id="placeholder" style="clear: both;"></div>
            
                <!-- 테이블 끝 -->
            </div>
        </div>
        </div>
    </div>
    </div>
    </div>
    <div style="display:none;">
        <asp:LinkButton ID="lbtnRegister" runat="server" OnClick="lbtnRegister_Click"></asp:LinkButton>
        <asp:TextBox ID="tbxCSVID" runat="server" Width="0"></asp:TextBox>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript">
        var WindowObjectReference; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_SelectSaveAddress = "<%=GetResource("SelectSaveAddress")%>";
        var R_FirstName = "<%=GetResource("FirstName") %>";
        var R_Email = "<%=GetResource("Email") %>";
        var R_CellPhone = "<%=GetResource("CellPhone") %>";
        var R_Phone = "<%=GetResource("Phone") %>";
        var R_CompanyTel = "<%=GetResource("CompanyTel") %>";
        var R_CompanyName = "<%=GetResource("CompanyName") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Outlook/InOutStep2.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Address/Js/Outlook/InOutStep2.js");
    </script>  
</body>
</html>
