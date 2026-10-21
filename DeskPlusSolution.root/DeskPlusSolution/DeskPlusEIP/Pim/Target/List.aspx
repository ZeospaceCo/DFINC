<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Target.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />      
</head>
<body>
<form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">
        <div id="layout_header" >
            <h1><%=SiteMap%></h1>
            <ul class="btn_right">
                <li><div id="loading"></div></li>
                <li><cc3:ClientSmartButton ID="cbtnAdmin" Visible="false" runat="server" ></cc3:ClientSmartButton></li>
                <li><asp:DropDownList ID="ddlSearchYear" runat="server" CssClass="searchselect form-control" ></asp:DropDownList>&nbsp;<%=GetResource("YearViewMark")%>&nbsp; </li>           
            
                <li><cc3:ClientSmartButton ID="CbtnAdd" runat="server" Class="btn btn-secondary" ></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <!-- 테이블 시작 //-->    
                    <div id="placeholder" style="clear: both;"></div>    
                    <!-- 테이블 끝 //-->
                </div>
            </div>
        </div>   
    </div>
</div>
</form>         
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" ></script>   
    <script type="text/javascript">
        var UserId = "<%=UserID %>";
        var CompanyID = "<%=CompanyID%>";
        var AttachFileInfoList = null;
        var ImagePath = "<%=BaseImgPath%>";
        var R_NoFileData = '<%=GetResource("NoFileData") %>';
        var R_ErrorAttachFile = '<%=GetResource("ErrorAttachFile") %>';
        var R_AllDownLoad = '<%=GetResource("AllDownLoad") %>';
        var ApplyDelete = '<%=GetResource("ApplyDelete") %>';
        var TextEmpty = '<%=GetResource("TextEmpty") %>';
        var col1 = '<%=GetResource("MarkName") %>';
        var col2 = '<%=GetResource("AttachName") %>';
        var col3 = '<%=GetResource("Period") %>';
        var col4 = '<%=GetResource("Menu") %>';
        var Del1 = '<%=GetResource("Del") %>';
        var add1 = '<%=GetResource("ADD") %>';
        var PreLang = "<%=PreLang%>";
        var DeptID = "<%=DeptID %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Target/List.js" ></script>  
     <script type="text/javascript">
         //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
         //JsCall("/DeskPlusEIP/Pim/Js/Target/List.js");
    </script>
</body>
</html>
