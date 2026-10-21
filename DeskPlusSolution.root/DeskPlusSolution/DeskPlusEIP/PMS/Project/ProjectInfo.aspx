<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ProjectInfo.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Project.ProjectInfo" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PMS.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PMS.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
<form id="form1" runat="server">

<div id="layout_body">
<div class="layout_full">

    <div id="layout_header">
        <h1><asp:Literal ID="ltlMenuName" runat="server"></asp:Literal></h1>
    </div>

    <div id="layout_content">
        <div class="row">
            <div class="col-12 content-area-body"><!-- width 100%(col-12) -->

                <table class="table_typeC">
                <caption></caption>
                <colgroup>
                    <col width="80px" />
                    <col />
                    <col width="80px" />
                    <col />
                </colgroup>
                <tbody>
                <tr>
                    <th><asp:Literal ID="ltlWriteUserName" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblWriteUserName" runat="server"></asp:Label></td>          
                    <th><asp:Literal ID="ltlWriteDate" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblWriteDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Literal ID="ltlTitle" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblTitle" runat="server"></asp:Label></td>          
                    <th><asp:Literal ID="ltlCharger" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblCharger" runat="Server"></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Literal ID="ltlFolder" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblFolder" runat="server"></asp:Label></td>          
                    <th><asp:Literal ID="ltlIsOpen" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblIsOpen" runat="Server"></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Literal ID="ltlPlanStartDate" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblPlanStartDate" runat="server"></asp:Label></td>          
                    <th><asp:Literal ID="ltlPlanEndDate" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblPlanEndDate" runat="Server"></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Literal ID="ltlStartDate" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblStartDate" runat="server"></asp:Label></td>          
                    <th><asp:Literal ID="ltlEndDate" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblEndDate" runat="Server"></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Literal ID="ltlWholeDay" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblWholeDay" runat="server"></asp:Label></td>          
                    <th><asp:Literal ID="ltlInjecDay" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblInjecDay" runat="Server"></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Literal ID="ltlRestDay" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblRestDay" runat="server"></asp:Label></td>          
                    <th><asp:Literal ID="ltlDelayDay" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblDelayDay" runat="Server"></asp:Label></td>
                </tr>

                <tr>
                    <th><asp:Literal ID="ltlProcess" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblProcess" runat="server"></asp:Label></td>
                    <th><asp:Literal ID="ltlStatus" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblStatus" runat="Server"></asp:Label></td>
                </tr> 
                </tbody>
            </table>

            <div class="content_box"  style="overflow:auto;">
            <table cellspacing="0" style="min-height:300px;width:100%;">
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblContent" runat="Server"></asp:Label>
                    </td>
                </tr>
            </table>
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
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var R_Project_MSGFolder = "<%=GetResource("R_Project_MSGFolder") %>";
        var R_Project_MSGPlanDate = "<%=GetResource("R_Project_MSGPlanDate") %>";
        var R_Project_MSGProjectName = "<%=GetResource("R_Project_MSGProjectName") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Project/ProjectInfo.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Project/ProjectInfo.js");
    </script>
</body>
</html>
