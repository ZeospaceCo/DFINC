<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Auth.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Address.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Address.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=HeaderTitle%></h1>
                <ul class="btn_right">
                   <cc3:ClientSmartButton ID="cbtnMove" runat="server" BeginEndTag="li" />
                   <cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                </ul>
            </div>
            <div id="layout_content">
            <div class="row">
            <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                <table class="table_typeB">
                    <caption>
                        <%=GetResource("AuthManage")%></caption>
                    <colgroup>
                        <col width="100px" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>
                                <%=GetResource("AuthCode")%>
                            </th>
                            <td>
                                <span class="aLeft"><cc3:ClientSmartTextBox ID="ctbRoleCode" runat="server" InputMode="None" Width="200px" class="searchtext form-control"></cc3:ClientSmartTextBox>&nbsp;</span>
                                <ul>
                                    <li><a href="#" id="CheckRoleCode" class="btn btn-sm btn-secondary"><span><%=GetResource("CheckMultiValue")%></span></a></li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("AuthName")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbRoleName" runat="server" InputMode="None" class="searchtext form-control" CssStyle="width: 98% !important;"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("AuthExplain")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbRoleDesc" runat="server" InputMode="None" class="searchtext form-control" CssStyle="width: 98% !important;"></cc3:ClientSmartTextBox>
                            </td>
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
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>    
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var RoleID = "<%=RoleID %>";
        var Gubun = "<%=Gubun %>";
        var CountRoleCode = -1;
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_UseAuthCode = "<%=GetResource("UseAuthCode") %>";
        var R_InputAuthCode = "<%=GetResource("InputAuthCode") %>";
        var R_InputAuthName = "<%=GetResource("InputAuthName") %>";
        var R_CheckChoiceMultiValue = "<%=GetResource("CheckChoiceMultiValue") %>";
        var R_DuplicateCheckMultiValue = "<%=GetResource("DuplicateCheckMultiValue") %>";
        var R_SUCCESS = "<%=GetResource("SUCCESS") %>";
        var R_ErrorProcessing = "<%=GetResource("ErrorProcessing") %>";        
    </script>
    <script type="text/javascript" src="../../Js/Admin/Auth/Add.js"></script>
    <script type="text/javascript">
        //JsCall("../../Js/Admin/Auth/Add.js");
    </script>
</body>
</html>
