<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InGoogleStep3.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Outlook.InGoogleStep3" %>
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
                <li class="selected"><a><%=GetResource("GetGoogleMail")%></a></li>
            </ul>
            </div>
        </div>     
        <div id="layout_content" class="outmail">
        <div class="row">
            <div class="col-12 content-area-body"> <!-- width 100%(col-12) -->      
                <ul class="outmail_location">
                    <li><%=GetResource("Load")%></li>
                    <li><%=GetResource("UsePreview")%></li>
                     <li><a class="over"><%=GetResource("Save")%></a></li>            
                </ul>
                <div class="outmail_select">
                    <p class="descrip"><%=GetResource("InGoogleStep3_Descript1")%> <br /><br /> <%=GetResource("InGoogleStep3_Descript2")%></p>
                </div>     
            </div>
        </div>
        </div>
    </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
    </script>  
</body>
</html>
