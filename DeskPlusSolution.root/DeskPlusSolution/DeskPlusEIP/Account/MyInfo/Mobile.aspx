<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mobile.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.MyInfo.Mobile" %>
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
</head>
<body>
    <form id="form1" runat="server">    
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("MobileInfo")%></h1>

                <ul class="btn_right">                    
                    <cc3:ClientSmartButton ID="cbtnUser" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnSave" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnDel" runat="server" BeginEndTag="li" Class="btn btn-danger" />
                    <cc3:ClientSmartButton ID="cbtnList" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:567px;">
            <!-- 기본정보 //-->

            <table class="table_typeB">
                <caption></caption>
                <colgroup>
                    <col width="120px" />
                    <col />
                    <col width="120px" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th><%=GetResource("UUID")%></th>
                        <td><asp:DropDownList ID="ddlUUID" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                        <th><%=GetResource("MobileType")%></th>
                        <td ID="CTBMobileType" runat="server" style="padding:7px;vertical-align:middle;"></td>                        
                    </tr>
                    <tr style="display:none">
                        <th><%=GetResource("PhoneNumber")%></th>
                        <td><cc3:ClientSmartTextBox ID="CTBMobilePhone" ReadOnly="true" runat="server"></cc3:ClientSmartTextBox></td>
                        <th><%=GetResource("DeviceName")%></th>
                        <td><cc3:ClientSmartTextBox ID="CTBDeviceName" ReadOnly="true" runat="server"></cc3:ClientSmartTextBox></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("RequestDate")%></th>
                        <td ID="CTBRequestDate" runat="server" style="padding:7px;vertical-align:middle;"></td>
                        <th><%=GetResource("ActiveMobile")%></th>
                        <td ID="CTBActiveMobile" runat="server" style="padding:7px;vertical-align:middle;"></td>
                    </tr>
                    <tr id="Approve" runat="server">
                        <th><%=GetResource("ApproveDate")%></th>
                        <td ID="CTBApproveDate" runat="server" style="padding:7px;vertical-align:middle;"></td>
                        <th><%=GetResource("LoginType")%></th>
                        <td><asp:DropDownList ID="ddlLoginType" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                    </tr>
                    <tr id="Alarm" runat="server">  
                        <th><%=GetResource("AlarmSetting")%></th>
                        <td colspan="3">
                            <span> * <%=GetResource("MsgNotifi")%> * </span>
                            <br />
                            <span><%=GetResource("Mail")%> : </span>
                            <asp:DropDownList ID="ddlMailAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <span><%=GetResource("Approval")%> : </span>
                            <asp:DropDownList ID="ddlApprovalAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <span><%=GetResource("Board")%> : </span>
                            <asp:DropDownList ID="ddlBoardAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <span><%=GetResource("Pims")%> : </span>
                            <asp:DropDownList ID="ddlPimsAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <span><%=GetResource("Task")%> : </span>
                            <asp:DropDownList ID="ddlTaskAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <br />
                            <span><%=GetResource("Work")%> : </span>
                            <asp:DropDownList ID="ddlWorkAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <span><%=GetResource("Community")%> : </span>
                            <asp:DropDownList ID="ddlCommunityAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <span><%=GetResource("ResourceManager")%> : </span>
                            <asp:DropDownList ID="ddlResourceAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <span><%=GetResource("SNS")%> : </span>
                            <asp:DropDownList ID="ddlSNSAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <span><%=GetResource("Notice")%> : </span>
                            <asp:DropDownList ID="ddlNoticeAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>  
                          </td>                        
                    </tr>
                    <tr>
                        <th><%=GetResource("Memo")%></th>
                        <td colspan="3"><cc3:ClientSmartTextBox ID="ctbDesc" MaxLength="200" runat="server" CssStyle="width:95% !important;" TextMode="MultiLine"></cc3:ClientSmartTextBox></td>
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
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var UserID = "<%=CurrentUserInfo.UserID %>";
        var DeptID = "<%=CurrentUserInfo.DeptID %>";
        var ActiveMobile = "<%=ActiveMobile %>";

        var R_ActiveMobile0 = "<%=GetResource("ActiveMobile0") %>";  
        var R_ActiveMobile1 = "<%=GetResource("ActiveMobile1") %>";    
        var R_ActiveMobile2 = "<%=GetResource("ActiveMobile2") %>";  
        var R_ActiveMobile3 = "<%=GetResource("ActiveMobile3") %>";  
        var R_Private_MyInfo_MSG_ModNo = "<%=GetResource("Private_MyInfo_MSG_ModNo") %>";
        var R_Private_MyInfo_MSG_ModOK   = "<%=GetResource("Private_MyInfo_MSG_ModOK") %>";
        var R_Admin_DeptManager_MSG_DelChk = "<%=GetResource("Admin_DeptManager_MSG_DelChk") %>";
        var R_ModMsg = "<%=GetResource("ModMsg") %>";  
        var R_Mobile = "<%=GetResource("Mobile") %>";  
      
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Account/Js/MyInfo/Mobile.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Account/Js/MyInfo/Mobile.js");
    </script>
</body>
</html>