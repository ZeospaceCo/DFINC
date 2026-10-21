<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeviceInfo.aspx.cs" Inherits="Zeospace.DeskPlus.MDM.Web.Device.DeviceInfo" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_MDM.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_MDM.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />  
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <h1><%=GetResource("MobileInfo")%></h1>
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
                                <col width="120px" />
                                <col />
                                <col width="120px" />
                                <col />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th><%=GetResource("DeptName")%></th>
                                    <td><asp:Literal ID="ltDeptName" runat="server"></asp:Literal></td>
                                    <th><%=GetResource("Title")%></th>
                                    <td><asp:Literal ID="ltTitle" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("Name")%></th>
                                    <td><asp:Literal ID="ltName" runat="server"></asp:Literal></td>
                                    <th><%=GetResource("OfficePhone")%></th>
                                    <td><asp:Literal ID="ltOfficePhone" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                <th><%=GetResource("AccessRight")%></th>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rblAccessRight" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList>
                                </td>
                                </tr>
                            </tbody>
                        </table>
                        <div class="dotLine"></div>
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
                                    <th><%=GetResource("MobilePhone")%></th>
                                    <td><cc3:ClientSmartTextBox ID="CTBMobilePhone" ReadOnly="true" runat="server"></cc3:ClientSmartTextBox></td>
                                    <th><%=GetResource("DeviceName")%></th>
                                    <td><cc3:ClientSmartTextBox ID="CTBDeviceName" ReadOnly="true" runat="server"></cc3:ClientSmartTextBox></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("RequestDate")%></th>
                                    <td ID="CTBRequestDate" runat="server" style="padding:7px;vertical-align:middle;"></td>
                                    <th><%=GetResource("ActiveMobile")%></th>
                                    <td><asp:RadioButtonList ID="rblActiveMobile" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList>  </td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("ApproveDate")%></th>
                                    <td ID="CTBApproveDate" runat="server" style="padding:7px;vertical-align:middle;"></cc3:ClientSmartTextBox></td>
                                    <th><%=GetResource("LoginType")%></th>
                                    <td><asp:DropDownList ID="ddlLoginType" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>                           
                                </tr>
                                <tr>  
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
                    </div>
                </div>                
            </div>
        </div>
    </div>
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank">
        </iframe>
    </div>
    <div id="zeoModalBG">
    </div>
    <!-- ############# end of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        var UserID = "<%=UserID %>"; 
        var DeptID = "<%=DeptID %>"; 
        var CurrentPage = "<%=CurrentPage %>";    
        var RecordPerPage = "<%=RecordPerPage %>"; 
        var Order = "<%=Order %>";                 
        var SearchField = "<%=SearchField %>"; 
        var SearchValue = "<%=SearchValue %>"; 
        var ListPage = "<%=ListPage %>";
        var R_ConfirmDel = "<%=GetResource("ConfirmDel") %>";        
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>"; 
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_UUID = "<%=GetResource("UUID") %>"; 
        var R_Mobile = "<%=GetResource("Mobile") %>"; 
        var R_DelNoMobile = "<%=GetResource("DelNoMobile") %>";    
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja
        var PageUrl = "<%= Request.Url.ToString() %>";        
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/MDM/Js/Device/DeviceInfo.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/MDM/Js/Device/DeviceInfo.js");
    </script>
</body>
</html>
