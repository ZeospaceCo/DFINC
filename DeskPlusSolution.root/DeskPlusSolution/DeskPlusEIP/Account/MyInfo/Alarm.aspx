<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Alarm.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.MyInfo.Alarm" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <style type="text/css">
    .spddl {float:left;padding-left:10px;color:#41484f;}
    .sptit {float:left;padding-left:20px;color:#ff6600;padding-top:5px;font-weight:bold;}
    .spitem {float:left;padding-left:5px;color:#41484f;}
    </style>
</head>
<body>
    <form id="form1" runat="server">    
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("AlarmTitle")%></h1>
                <ul class="btn_right">                    
                    <cc3:ClientSmartButton ID="cbtnUser" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnSave" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnList" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:595px;">
            <!-- 기본정보 //-->

                <table class="table_typeB">
                <colgroup>
                    <col width="120px" />
                    <col />
                </colgroup>
                <tbody>
                <tr>
                    <th><%=GetResource("Approval")%></th>
                    <td id="AlarmApproval">
                        <span class="spddl">
                            <asp:DropDownList ID="ddlAlarmApproval" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        </span>
                        <span class="sptit">- <%=GetResource("AlarmOption")%></span>
                        <span class="spitem">
                            <asp:CheckBoxList ID="cbAlarmApproval" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:CheckBoxList>
                        </span>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("Board")%></th>
                    <td id="AlarmBoard">
                        <span class="spddl">
                            <asp:DropDownList ID="ddlAlarmBoard" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        </span>
                        <span class="sptit">- <%=GetResource("AlarmOption")%></span>
                        <span class="spitem">
                            <asp:CheckBoxList ID="cbAlarmBoard" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:CheckBoxList>
                        </span>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("Pims")%></th>
                    <td id="AlarmPims">
                        <span class="spddl">
                            <asp:DropDownList ID="ddlAlarmPims" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        </span>
                        <span class="sptit">- <%=GetResource("AlarmOption")%></span>
                        <span class="spitem">
                            <asp:CheckBoxList ID="cbAlarmPims" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:CheckBoxList>
                        </span>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("Task")%></th>
                    <td id="AlarmTask">
                        <span class="spddl">
                            <asp:DropDownList ID="ddlAlarmTask" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        </span>
                        <span class="sptit">- <%=GetResource("AlarmOption")%></span>
                        <span class="spitem">
                            <asp:CheckBoxList ID="cbAlarmTask" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:CheckBoxList>
                        </span>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("Work")%></th>
                    <td id="AlarmWork">
                        <span class="spddl">
                            <asp:DropDownList ID="ddlAlarmWork" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        </span>
                        <span class="sptit">- <%=GetResource("AlarmOption")%></span>
                        <span class="spitem">
                            <asp:CheckBoxList ID="cbAlarmWork" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:CheckBoxList>
                        </span>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("Community")%></th>
                    <td id="AlarmCommunity">
                        <span class="spddl">
                            <asp:DropDownList ID="ddlAlarmCommunity" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        </span>
                        <span class="sptit">- <%=GetResource("AlarmOption")%></span>
                        <span class="spitem">
                            <asp:CheckBoxList ID="cbAlarmCommunity" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:CheckBoxList>
                        </span>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("ResourceManager")%></th>
                    <td id="AlarmResource">
                        <span class="spddl">
                            <asp:DropDownList ID="ddlAlarmResource" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        </span>
                        <span class="sptit">- <%=GetResource("AlarmOption")%></span>
                        <span class="spitem">
                            <asp:CheckBoxList ID="cbAlarmResource" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:CheckBoxList>
                        </span>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("SNS")%></th>
                    <td id="AlarmSNS">
                        <span class="spddl">
                            <asp:DropDownList ID="ddlAlarmSNS" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        </span>
                        <span class="sptit">- <%=GetResource("AlarmOption")%></span>
                        <span class="spitem">
                            <asp:CheckBoxList ID="cbAlarmSNS" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:CheckBoxList>
                        </span>
                    </td>
                </tr>
                </tbody>
                </table>

            <!-- 기본정보 //-->
            </div>
        </div>
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
    <iframe src="about:blank" id="ifrmSLO" name="ifrmSLO" width="0" height="0"></iframe>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CompanyID %>";
        var UserID = "<%=UserID %>";
        var IsAdmin = "<%=IsAdmin%>";       // 0 or 1

        var MSG_SUCCESS = "<%=GetResource("Admin_DeptManager_MSG_SUCCESS") %>";
        var MSG_FAILURE = "<%=GetResource("Admin_DeptManager_MSG_FAILURE") %>";
        var MSG_SELITEM = "<%=GetResource("MSG_SELITEM") %>";
      
    </script>
    <!-- script type="text/javascript" src="/DeskPlusEIP/Account/Js/MyInfo/Alarm.js"></script //-->
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Account/Js/MyInfo/Alarm.js");
    </script>
</body>
</html>