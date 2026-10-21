<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Apps.Schedule.View" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />        
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <h1><%=GetResource("ScheduleView")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnPrint" runat="server" /></li>

                <li><cc3:ClientSmartButton ID="cbtnMod" runat="server" Class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" Class="btn btn-danger" /></li>
                <li><cc3:ClientSmartButton ID="cbtnList" runat="server" /></li>
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
				  <div class="col-12 content-area-body"><!-- width 100%(col-12) -->   
                    <div id="toPrint">
                    <!--기본정보-->
                    <table class="table_typeB">
                    <caption>View</caption>
		            <colgroup>
			            <col width="80px" />
                        <col />
                        <col width="80px" />
                        <col />
		            </colgroup>
		            <tbody>
                        <tr>
                            <th><%=GetResource("Title")%></th>
                            <td colspan="3"><asp:Label title="" ID="lbTitle" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <th class="last-child"><%=GetResource("Date") %></th>
                            <td><asp:Label title="" ID="lbDate" runat="server"></asp:Label></td>
                            <th class="last-child"><%=GetResource("Registrant")%></th>
                            <td><asp:Label title="" ID="lbWriteName" runat="server"></asp:Label></td>
                        </tr>
                    </tbody>
                    </table>
                    <!--기본정보-->
            
                    <!--본문정보-->
                    <div class="content_box"  style="overflow:auto;">
                    <table cellspacing="0" style="min-height:300px;width:100%;">
                        <tr>
                            <td valign="top">
                                <asp:Label title="" ID="lbContent" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </div>
                    <!--본문정보-->       
                    </div>
                  </div>
            </div>
        </div>
    </div>
    </div>
    <div style="display:none;">
        <asp:LinkButton ID="lbtnConfirm" runat="server" OnClick="lbtnConfirm_Click"></asp:LinkButton> 
        <asp:TextBox ID="hdRepeatSel" Style="display: none" runat="server" Width="0px"></asp:TextBox>    
    </div>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>  
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript" >        
        var CommunityID = "<%=CommunityID %>";
        var MenuID = "<%=MenuID %>";
        var ScheduleID = "<%=ScheduleID%>";
        var ParentScheduleID = "<%=ParentScheduleID%>";
        var TmpDate = "<%=TmpDate%>";
        var Grade = "<%=Grade %>"; 
        var FullDomainPath = "<%=FullDomainPath %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_DelSchedule = "<%=GetResource("DelSchedule") %>";            
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Site/Apps/Schedule/View.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Apps/Schedule/View.js");
    </script>
</body>
</html>
