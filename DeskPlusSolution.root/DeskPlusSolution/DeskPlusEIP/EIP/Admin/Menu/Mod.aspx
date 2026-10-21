<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Menu.Mod" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DStyle/all.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><asp:Literal ID="ltlHeader" runat="server"></asp:Literal></h1>
                <ul class="btn_right">                    
                    <cc3:ClientSmartButton ID="cbtnChagneParent" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>
                    <cc3:ClientSmartButton ID="cbtnMove" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>                    
                    <cc3:ClientSmartButton ID="cbtnMod" BeginEndTag="li" class="btn btn-secondary" runat="server"></cc3:ClientSmartButton>                    
                    <cc3:ClientSmartButton ID="cbtnDel" BeginEndTag="li" class="btn btn-danger" runat="server"></cc3:ClientSmartButton>
                </ul>
            </div>
            <div id="layout_content">
            <div class="row">
            <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                <!-- 기본정보 -->
                <table class="table_typeB">
                    <caption>
                        <%=GetResource("MenuManagement")%></caption>
                    <colgroup>
                        <col width="80px" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>
                                <%=GetResource("UpperMenuName")%>
                            </th>
                            <td>
                                <asp:Literal ID="ltlParentName" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <label title="<%=GetResource("MenuName")%>">
                                    <%=GetResource("MenuName")%></label>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbMenuName" Required="true" MaxLength="100" runat="server"
                                    CssStyle="width:95% !important;"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <label title="<%=GetResource("MultiLang")%>">
                                    <%=GetResource("MultiLang")%></label>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif"
                                        alt="add" /></a>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbMultiName" MaxLength="100" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <label title="<%=GetResource("MenuExplain")%>">
                                    <%=GetResource("MenuExplain")%></label>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbDesc" MaxLength="200" runat="server" CssStyle="width:95% !important;"
                                    TextMode="MultiLine"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <label title="<%=GetResource("Call_ID_Title")%>">
                                    <%=GetResource("Call_ID")%></label>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbDirectCall" MaxLength="100" runat="server" CssStyle="width:300px !important;"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("TargetURL")%>
                            </th>
                            <td>
                                <ul class="aLeft">
                                    <li>
                                        <cc3:ClientSmartTextBox ID="ctbTargetURL" Required="true" runat="server" CssStyle="width:500px !important;"></cc3:ClientSmartTextBox>
                                    </li>
                                    <li>
                                        <asp:DropDownList ID="ddlOpenURL" runat="server" class="searchselect form-control">
                                        </asp:DropDownList>
                                    </li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <th><%=GetResource("MenuIcon")%></th>
                            <td >
                            <asp:RadioButtonList ID="rbClassName" runat="server" RepeatLayout="UnorderedList" class="menuIconRadio">
                                <asp:ListItem Text="1" Value="fas fa-align-left dropbtn" selected><span class="menuIcon"><i class="fas fa-align-left dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="2" Value="fas fa-archive dropbtn"><span class="menuIcon"><i class="fas fa-archive dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="3" Value="fas fa-calculator dropbtn"><span class="menuIcon"><i class="fas fa-calculator dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="4" Value="fas fa-calendar-minus dropbtn"><span class="menuIcon"><i class="fas fa-calendar-minus dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="5" Value="fas fa-file-signature dropbtn"><span class="menuIcon"><i class="fas fa-file-signature dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="6" Value="fas fa-folder-open dropbtn"><span class="menuIcon"><i class="fas fa-folder-open dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="7" Value="fas fa-puzzle-piece dropbtn"><span class="menuIcon"><i class="fas fa-puzzle-piece dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="8" Value="fas fa-search dropbtn"><span class="menuIcon"><i class="fas fa-search dropbtn"></i></span></asp:ListItem>    
                                <asp:ListItem Text="9" Value="fas fa-envelope dropbtn"><span class="menuIcon"><i class="fas fa-envelope dropbtn"></i></span></asp:ListItem>                                                        
                                <asp:ListItem Text="10" Value="fas fa-address-book dropbtn"><span class="menuIcon"><i class="fas fa-address-book dropbtn"></i></span></asp:ListItem> 
                                <asp:ListItem Text="11" Value="fas fa-id-card dropbtn"><span class="menuIcon"><i class="fas fa-id-card dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="12" Value="fas fa-bell dropbtn"><span class="menuIcon"><i class="fas fa-bell dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="13" Value="fas fa-book dropbtn"><span class="menuIcon"><i class="fas fa-book dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="14" Value="fas fa-book-open dropbtn"><span class="menuIcon"><i class="fas fa-book-open dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="15" Value="fas fa-book-reader dropbtn"><span class="menuIcon"><i class="fas fa-book-reader dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="16" Value="fas fa-bookmark dropbtn"><span class="menuIcon"><i class="fas fa-bookmark dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="17" Value="fas fa-box dropbtn"><span class="menuIcon"><i class="fas fa-box dropbtn"></i></span></asp:ListItem>                                
                                <asp:ListItem Text="18" Value="fas fa-broadcast-tower dropbtn"><span class="menuIcon"><i class="fas fa-broadcast-tower dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="19" Value="fas fa-building dropbtn"><span class="menuIcon"><i class="fas fa-building dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="20" Value="fas fa-bullhorn dropbtn"><span class="menuIcon"><i class="fas fa-bullhorn dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="21" Value="fas fa-bullseye dropbtn"><span class="menuIcon"><i class="fas fa-bullseye dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="22" Value="fas fa-bus dropbtn"><span class="menuIcon"><i class="fas fa-bus dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="23" Value="fas fa-camera dropbtn"><span class="menuIcon"><i class="fas fa-camera dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="24" Value="fas fa-car dropbtn"><span class="menuIcon"><i class="fas fa-car dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="25" Value="fas fa-clipboard dropbtn"><span class="menuIcon"><i class="fas fa-clipboard dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="26" Value="fas fa-clock dropbtn"><span class="menuIcon"><i class="fas fa-clock dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="27" Value="fas fa-clone dropbtn"><span class="menuIcon"><i class="fas fa-clone dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="28" Value="fas fa-cloud dropbtn"><span class="menuIcon"><i class="fas fa-cloud dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="29" Value="fas fa-cog dropbtn"><span class="menuIcon"><i class="fas fa-cog dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="30" Value="fas fa-coins dropbtn"><span class="menuIcon"><i class="fas fa-coins dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="31" Value="fas fa-comment dropbtn"><span class="menuIcon"><i class="fas fa-comment dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="32" Value="fas fa-comments dropbtn"><span class="menuIcon"><i class="fas fa-comments dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="33" Value="fas fa-database dropbtn"><span class="menuIcon"><i class="fas fa-database dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="34" Value="fas fa-desktop dropbtn"><span class="menuIcon"><i class="fas fa-desktop dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="35" Value="fas fa-download dropbtn"><span class="menuIcon"><i class="fas fa-download dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="36" Value="fas fa-edit dropbtn"><span class="menuIcon"><i class="fas fa-edit dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="37" Value="fas fa-folder dropbtn"><span class="menuIcon"><i class="fas fa-folder dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="38" Value="fas fa-pen dropbtn"><span class="menuIcon"><i class="fas fa-pen dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="39" Value="fas fa-phone dropbtn"><span class="menuIcon"><i class="fas fa-phone dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="40" Value="fas fa-tablet-alt dropbtn"><span class="menuIcon"><i class="fas fa-tablet-alt dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="41" Value="fas fa-user dropbtn"><span class="menuIcon"><i class="fas fa-user dropbtn"></i></span></asp:ListItem>                                
                                <asp:ListItem Text="42" Value="fas fa-warehouse dropbtn"><span class="menuIcon"><i class="fas fa-warehouse dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="43" Value="fas fa-window-restore dropbtn"><span class="menuIcon"><i class="fas fa-window-restore dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="44" Value="fas fa-calendar dropbtn"><span class="menuIcon"><i class="fas fa-calendar dropbtn"></i></span></asp:ListItem>
                                <asp:ListItem Text="45" Value="fas fa-users dropbtn"><span class="menuIcon"><i class="fas fa-users dropbtn"></i></span></asp:ListItem>                                                       
                            </asp:RadioButtonList>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <!-- 기본정보 -->
                <!-- 페이지연결 -->
                <div class="layout_subbox">
                    <h1>
                        <%=GetResource("DataLink")%></h1>
                    <ul class="btn_right">
                        <li>   
                            <asp:DropDownList ID="ddlSendMethod" runat="server" class="searchselect form-control">
                                <asp:ListItem Text="GET" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="POST" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </li>                        
                        <cc3:ClientSmartButton ID="cbtnADDParam" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>
                        
                    </ul>
                </div>
                <div id="placeholder_DataLink" style="clear: both;"></div>
                <!-- 페이지연결 -->
                <!-- 권한/역활 접근제어 -->
                <div class="layout_subbox">
                    <h1>
                        <%=GetResource("AuthAccessControl")%></h1>
                    <ul class="btn_right">                        
                        <cc3:ClientSmartButton ID="cbtnADDAuth" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>                        
                    </ul>
                </div>
                <div id="placeholder_Role" style="clear: both;"></div>
                <!-- 권한/역활 접근제어 -->
                <!-- IP 접근제어 -->
                <div class="layout_subbox">
                    <h1>
                        <%=GetResource("IPAccessControl")%></h1>
                    <ul class="btn_right">
                        <cc3:ClientSmartButton ID="cbtnADDIP" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>                        
                    </ul>
                </div>
                <div id="placeholder_IP" style="clear: both;"></div>
                <!-- IP 접근제어 -->
            </div>
        </div>
        </div>
        </div>
    </div>
    <div style="display:none;">
        <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton> 
    </div>
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->
        <div id="zeoModalDrag" class="modal-drag">
            <p>
            </p>
        </div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post">
        </form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank">
        </iframe>
    </div>
    <div id="zeoModalBG">
    </div>
    <!-- ############# end of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var MenuID = "<%=MenuID %>";
        var MultiLang = "<%=MultiLang %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_InputMenuName = "<%=GetResource("InputMenuName") %>";
        var R_ConfirmDeleteMenu = "<%=GetResource("ConfirmDeleteMenu") %>";
        var R_ExistSubMenu = "<%=GetResource("ExistSubMenu") %>";
        var R_NotConfirmUserAccessAuth = "<%=GetResource("NotConfirmUserAccessAuth") %>";
        var R_ErrorProcessing = "<%=GetResource("ErrorProcessing") %>";
        var R_ConfirmDelete = "<%=GetResource("ConfirmDelete") %>";
        var R_EncryptionGubun = "<%=GetResource("EncryptionGubun") %>";
        var R_EncryptionValue = "<%=GetResource("EncryptionValue") %>";
        var R_UserName = "<%=GetResource("UserName") %>";
        var R_Values = "<%=GetResource("Values") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_AccessCheck = "<%=GetResource("AccessCheck") %>";
        var R_IPBand = "<%=GetResource("IPBand") %>";
        var R_NotExistContent = "<%=GetResource("NotExistContent") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Admin/Menu/Mod.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/EIP/Js/Admin/Menu/Mod.js");
    </script>
</body>
</html>
