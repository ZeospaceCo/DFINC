<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Preference.Proxy.View" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
<form id="form1" runat="server">

<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1>
                <span><%=GetResource("ProxyDetailInfo")%></span>                 
            </h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnMod" BeginEndTag="li" class="btn btn-secondary" runat="server" />
                <cc3:ClientSmartButton ID="cbtnDel" BeginEndTag="li" class="btn btn-danger" runat="server" />
                <cc3:ClientSmartButton ID="cbtnList" BeginEndTag="li" runat="server" />
            </ul>
        </div>

        <div id="layout_content">
            <div class="layout_subbox">
                <h2><%=GetResource("SetProxy")%></h2>
            </div>
            <table class="table_typeB">
            <caption><%=GetResource("SetProxy")%></caption>
            <colgroup>
                <col width="110px" />
                <col />
			</colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("FavTitIsDept0")%></th>
                    <td><asp:Label ID="lbUserInfo" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th><%=GetResource("AbsencePeriod")%></th>
                    <td>
                        <asp:Label ID="lbDate" runat="server"></asp:Label>   
                        <span id="spAllDay">
                            <input id="cbxAllDay" type="checkbox" name="cbxAllDay" runat="server" disabled="disabled" /><label for="cbxAllDay"><%=GetResource("AllDay")%></label>               
                        </span>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("TypeAbsence")%></th>
                    <td><asp:Label ID="lbProxyType" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th><%=GetResource("ContentsAbsence")%></th>
                    <td style="min-height: 100px"><asp:Label ID="lbContent" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th><%=GetResource("HomeProxy")%></th>
                    <td>
                        <asp:Label ID="lbProxyInfo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("EnforceStateInfo")%></th>
                    <td>
                        <span id="spProxyIng" class="txt_point"><b><span class="spIng"></span></b></span>
                        <span id="spProxyCall" class="txt_point"><b><span class="spCall"></span></b></span>
                        <span id="spProxyCallEnd"><span class="spCallEnd"></span></span>
                    </td>
                </tr>
            </tbody>
            </table>            
        </div>

    </div>
</div>


<div style="display:none;">
    <asp:LinkButton ID="lbtnDel" runat="server" OnClick="lbtnDel_Click"></asp:LinkButton>
    <asp:HiddenField ID="hdIsCall" runat="server" />
    <asp:TextBox ID="tbxProxyUserID" runat="Server" Width="0"></asp:TextBox>
    <asp:TextBox ID="tbxProxyDeptID" runat="Server" Width="0"></asp:TextBox>
</div>



    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.contextMenu.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.hashtable.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript">
    
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";   

        var CurrentPage = "<%=CurrentPage%>";
        var RecordPerPage = "<%=RecordPerPage%>";

        var TargetProxyID = "<%=TargetProxyID%>"; 
        var IsProxy = "<%=IsProxy%>";
        var AllDay = "<%=AllDay%>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja

        var R_ConfirmDeleteProxyPerson = "<%=GetResource("ConfirmDeleteProxyPerson") %>";
        var R_Msg_Resource06 = "<%=GetResource("Msg_Resource06") %>";
        var R_Msg_Resource07  = "<%=GetResource("Msg_Resource07") %>";
        var R_Msg_Resource08  = "<%=GetResource("Msg_Resource08") %>";
        var R_Msg_Resource09  = "<%=GetResource("Msg_Resource09") %>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Preference/Proxy/View.js?v=1"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Preference/Proxy/View.js");
    </script>

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

</body>
</html>