<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Project.Add" ValidateRequest="false" %>
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
            <h1><%=GetResource("Admin_Menu1")%></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="CbtnConfirm" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
            </ul>
        </div>
        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->

                    <table class="table_typeB">
                    <caption></caption>
                    <colgroup>
                        <col width="80px" />
                        <col />
                        <col width="80px" />
                        <col />
                    </colgroup>
                    <tbody>
                    <tr>
                        <th><asp:Literal ID="ltlFolder" runat="server"></asp:Literal></th>
                        <td>
                            <ul class="aLeft">
                            <li><asp:TextBox ID="tbxFolder" runat="server"  onkeydown="Add.GetFolder()"></asp:TextBox></li>
                            <li>
                                <ul class="btn_smallL">
                                <li><cc3:ClientSmartButton ID="CbtnFolder" runat="server" Class="btn btn-sm"></cc3:ClientSmartButton></li>
                                </ul>
                            </li>
                            </ul>            
                        </td>          
                        <th><asp:Literal ID="ltlDate" runat="server"></asp:Literal></th>
                        <td>
                        <asp:TextBox ID="tbxPlanStartDate" runat="server"></asp:TextBox>
                        ~
                        <asp:TextBox ID="tbxPlanEndDate" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="ltlTitle" runat="server"></asp:Literal></th>
                        <td><asp:TextBox ID="tbxTitle" runat="server" Width="90%"></asp:TextBox></td>
                        <th><asp:Literal ID="ltlOpen" runat="server"></asp:Literal></th>
                        <td><asp:CheckBox ID="cbxIsOpen" runat="server" /></td>
                    </tr> 
                    </tbody>
                </table>
        
                <div class="layout_subbox">
                    <div style="padding-bottom:20px; height:450px; !important">
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
        <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton>
        <asp:TextBox ID="tbxFolderID" runat="server" Width="0"></asp:TextBox>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script> 
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>    
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var MultiLang = "<%=MultiLang %>";
        var R_Project_MSGFolder = "<%=GetResource("R_Project_MSGFolder") %>";
        var R_Project_MSGPlanDate = "<%=GetResource("R_Project_MSGPlanDate") %>";
        var R_Project_MSGProjectName = "<%=GetResource("R_Project_MSGProjectName") %>";
        var R_InputStartDays = "<%=GetResource("InputStartDays") %>";
        var R_StartDateEndDate =  "<%=GetResource("StartDateEndDate") %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Project/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/PMS/Js/Project/Add.js");
    </script>
</body>
</html>
