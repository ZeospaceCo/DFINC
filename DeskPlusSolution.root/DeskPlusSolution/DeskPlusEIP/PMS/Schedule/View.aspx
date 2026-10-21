<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Schedule.View" %>
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
            <h1><%=GetResource("Schedule_Title") %></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnMod" runat="server" Class="btn btn-secondary" /></li>    
                <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" Class="btn btn-danger" /></li>    
                <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server" /></li>  
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
                    </colgroup>
                    <tbody>
                    <tr>
                        <th><asp:literal id="ltTitleName" runat="server"></asp:literal></th>
                        <td><asp:label id="lbTitleName" runat="server"></asp:label></td>
                    </tr>
                    <tr>
                        <th><asp:literal id="ltTime" runat="server"></asp:literal></th>
                        <td>
                            <asp:label id="lbFullYMD1" runat="server"></asp:label>
                            <asp:label id="ltStartTime" runat="server"></asp:label>
                            ~
                            <asp:label id="lbFullYMD2" runat="server"></asp:label>
                            <asp:label id="ltEndTime" runat="server"></asp:label>
                        </td> 
                    </tr>
                    </tbody>
                </table>

                <div class="content_box"  style="overflow:auto;">
                <table cellspacing="0" style="min-height:300px;width:100%;">
                    <tr>
                        <td valign="top">
                        <asp:Label title="" ID="lbContent" runat="server"></asp:Label>
                   
                        </td>
                    </tr>
                </table>
                </div>
            </div>
          </div>
        </div>    
    </div>
    </div>


    <!--//############################### HIDDEN 시작 #####################################//-->
    <div style="display: none">
    <asp:TextBox ID="hdRepeatSel" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
    <asp:TextBox ID="hdRepeatChk" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
    <asp:linkbutton id="cbtnDelete" runat="server" OnClick="cbtnDelete_Click"></asp:linkbutton>    
    </div>
    <!--//############################### HIDDEN 끝 #####################################//-->
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>    
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var scheduleID = "<%=ScheduleID %>";
        var status = "<%=Status %>";
        var projectID = "<%=ProjectID %>";
        var menuID = "<%=MenuID %>";
        var menuType = "<%=MenuType %>";
        var rqProjectID = "<%=RqProjectID %>";
        var DisplayID = "<%=DisplayID %>";
        var ProjParam = "<%=ProjParam %>";
        var Private_BoardView_IsDelMSG ="<%=GetResource("Private_BoardView_IsDelMSG") %>";   
          
          
        function RepeatMod(arg) {
            $("#hdRepeatSel").val(arg);
        
            __doPostBack('cbtnDelete', '');
        }
        
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Schedule/View.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Schedule/View.js");
    </script>
</body>
</html>
