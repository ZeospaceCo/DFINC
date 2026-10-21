<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ProjectInfo.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Admin.Project.ProjectInfo" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
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
        <h1><%=GetResource("Module_Node1")%></h1>
        <ul class="btn_right">
            <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary" /></li>
        </ul>
    </div>
    <div id="layout_content">
         <div class="row">
            <div class="col-12 content-area-body"><!-- width 100%(col-12) -->

                <table class="table_typeB">
                <caption></caption>
                <colgroup>
                    <col width="100px" />
                    <col />
                    <col width="100px" />
                    <col />
                </colgroup>
                <tbody>
                <tr>
                    <th><asp:Literal ID="ltlPlanStartDate" runat="server"></asp:Literal></th>
                    <td><asp:TextBox ID="tbxPlanStartDate" runat="Server" ReadOnly="true" Width="80%"></asp:TextBox>
                                        <input type="checkbox" id="cbxPlanStartDate" /> <%=GetResource("Ditto")%></td>          
                    <th><asp:Literal ID="ltlPlanEndDate" runat="server"></asp:Literal></th>
                    <td><asp:TextBox ID="tbxPlanEndDate" runat="server" ReadOnly="true" Width="80%"></asp:TextBox>
                                        <input type="checkbox" id="cbxPlanEndDate" /> <%=GetResource("Ditto")%></td>
                </tr>
                <tr>
                    <th><asp:Literal ID="ltlStartDate" runat="server"></asp:Literal></th>
                    <td><asp:TextBox ID="tbxStartDate" runat="Server" ReadOnly="true" Width="97%"></asp:TextBox></td>          
                    <th><asp:Literal ID="ltlEndDate" runat="server"></asp:Literal></th>
                    <td><asp:TextBox ID="tbxEndDate" runat="server" ReadOnly="true" Width="97%"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Literal ID="ltlWriteUserName" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblWriteUserName" runat="server"></asp:Label></td>          
                    <th><asp:Literal ID="ltlWriteDate" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblWriteDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Literal ID="ltlTitle" runat="server"></asp:Literal></th>
                    <td><asp:TextBox ID="tbxTitle" runat="Server" Width="97%"></asp:TextBox></td>          
                    <th><asp:Literal ID="ltlCharger" runat="server"></asp:Literal>
                        <ul class="btn_smallR">
                        <li><cc3:ClientSmartButton ID="cbtnDeptUserTree" runat="server" Class="btn btn-sm btn-secondary"></cc3:ClientSmartButton></li>
                        </ul>
                    </th>
                    <td>
                        <asp:HiddenField ID="hdChargerID" runat="Server"></asp:HiddenField>
                        <asp:HiddenField ID="hdChargetName" runat="Server"></asp:HiddenField>
                        <div id="InputDiv1" style="height:32px;width:100%; padding-left:2px; padding-top:2px; text-overflow:ellipsis; overflow-y:auto;" CONTENTEDITABLE="false"></div>
                    </td>
                </tr>
                <tr>
                    <th><asp:Literal ID="ltlFolder" runat="server"></asp:Literal></th>
                    <td>
                        <ul class="aLeft">
                        <li><asp:TextBox ID="tbxFolder" runat="server"  onkeydown="ProjectInfo.GetFolder()"></asp:TextBox></li>
                        <li>
                            <ul class="btn_smallL">
                            <li><cc3:ClientSmartButton ID="cbtnFolder" runat="server" ></cc3:ClientSmartButton></li>
                            </ul>
                        </li>
                        </ul>            
                    </td>   
                    <th><asp:Literal ID="ltlIsOpen" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblIsOpen" runat="Server"></asp:Label></td>
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
                    <td>
                        <asp:DropDownList ID="ddlProcess" runat="server" CssClass="searchselect form-control">
                            <asp:ListItem Text="10%" Value="10"></asp:ListItem>
                            <asp:ListItem Text="20%" Value="20"></asp:ListItem>
                            <asp:ListItem Text="30%" Value="30"></asp:ListItem>
                            <asp:ListItem Text="40%" Value="40"></asp:ListItem>
                            <asp:ListItem Text="50%" Value="50"></asp:ListItem>
                            <asp:ListItem Text="60%" Value="60"></asp:ListItem>
                            <asp:ListItem Text="70%" Value="70"></asp:ListItem>
                            <asp:ListItem Text="80%" Value="80"></asp:ListItem>
                            <asp:ListItem Text="90%" Value="90"></asp:ListItem>
                            <asp:ListItem Text="100%" Value="100"></asp:ListItem>
                        </asp:DropDownList>
                    </td>          
                    <th><asp:Literal ID="ltlStatus" runat="server"></asp:Literal></th>
                    <td><asp:DropDownList ID="ddlStatus" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                </tr>
                </tbody>
                </table>

                <div class="layout_subbox">
                    <div style="padding-bottom:20px; height:350px; !important">
                        <asp:TextBox ID="tbxContent" runat="server" TextMode="MultiLine" Width="100%" Height="100%"></asp:TextBox>
                    </div>
                </div>  
             </div>
            </div>
    </div>
</div>
</div>

<!-- ================================== Hidden Input ===================================== -->
<div style="display: none">
    <asp:TextBox ID="tbxFolderID" runat="server" Width="0"></asp:TextBox>
    <asp:LinkButton ID="lbtnOk" runat="server" OnClick="lbtnOk_Click"></asp:LinkButton>
</div>
<!-- ==================================================================================== -->
</form>
<!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script> 
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var R_Project_MSGFolder = "<%=GetResource("R_Project_MSGFolder") %>";
        var R_Project_MSGPlanDate = "<%=GetResource("R_Project_MSGPlanDate") %>";
        var R_Project_MSGProjectName = "<%=GetResource("R_Project_MSGProjectName") %>";
        var R_Project_MSGPProcess = "<%=GetResource("R_Project_MSGPProcess") %>";
        var Private_BoardAdd_DPEditorTEXT = "<%=GetResource("Private_BoardAdd_DPEditorTEXT") %>";
		var MSG_DUF_USERDEPT = "<%=GetResource("MSG_DUF_USERDEPT") %>";
        var projectID           = "<%=ProjectID %>";
        var menuID              = "<%=MenuID %>";  
        var menuType            = "<%=MenuType %>";  
        var ProjParam           = "<%=ProjParam %>"; 
        var preLangReal         = "<%=PreLangReal %>";
        var chargerName         = "<%=ChargerName %>";  
        var EleDom = ""; 
        var dialogURL;
		var rtnVal;
		var ranNum;
		ranNum = Math.random();
		ranNum = ranNum.toString();
		ranNum = ranNum.substring(2,ranNum.length);
        var ImagePath = "<%=BaseImgPath %>";
        var R_Del = "<%=GetResource("Unit_Register_Delete") %>";
        var R_InputDate = "<%=GetResource("InputDate") %>";
        var R_CheckDate = "<%=GetResource("CheckDate") %>";
        var R_InputStartDate = "<%=GetResource("InputStartDate") %>";
        var R_CheckEndDate = "<%=GetResource("CheckEndDate") %>";
        var ClientQueryString = "<%=Page.ClientQueryString %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal %>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Admin/Project/ProjectInfo.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/PMS/Js/Admin/Project/ProjectInfo.js");
    </script>
</body>
</html>