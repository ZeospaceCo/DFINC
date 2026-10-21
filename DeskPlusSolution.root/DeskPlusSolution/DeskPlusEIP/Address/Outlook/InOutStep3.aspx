<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InOutStep3.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Outlook.InOutStep3" %>

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
        <div class="outmail">           
            <ul class="outmail_location">
                <li><%=GetResource("Load")%></li>
                <li><%=GetResource("UsePreview")%></li>
                 <li><a class="over"><%=GetResource("Save")%></a></li>            
            </ul>
            <div class="outmail_select">
                <p class="descrip"><%=GetResource("InOutStep3_Descript1")%> <br /><br /> <%=GetResource("InOutStep3_Descript2")%></p>
            </div>     
        </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var WindowObjectReference; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Outlook/InOutStep3.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Address/Js/Outlook/InOutStep3.js");
    </script>  
</body>
</html>
