<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.MyInfo.Mod" %>
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
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.placeholder.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var UserID = "<%=CurrentUserInfo.UserID %>";
        var LogInID = "<%=CurrentUserInfo.LogInID %>";
        var DeptID = "<%=CurrentUserInfo.DeptID %>";        
        var MultiLang = "<%=MultiLang %>";
        var Mail = "<%=Mail %>";  
        var MailProduct = "<%=MailProduct %>";   //메일 제품

        var AttachGubun = "";        
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID           
        var FileType = "*.jpg;*.jpeg;*.png;*.gif";
        var FileTypeDesc = "Images (*.jpg *.jpeg *.png *.gif)";

        var R_InputLoginID = "<%=GetResource("InputLoginID") %>";        
        var R_InputName = "<%=GetResource("InputName") %>";        
        var R_InputBaseMail = "<%=GetResource("InputBaseMail") %>";
        var R_InputPassword = "<%=GetResource("InputPassword") %>";
        var R_InputMailPassword = "<%=GetResource("InputMailPassword") %>";

        var R_OnlyOneLoginID = "<%=GetResource("OnlyOneLoginID") %>";
        var R_RepetitionLoginID = "<%=GetResource("RepetitionLoginID") %>";
        var R_CheckLoginRepetition = "<%=GetResource("CheckLoginRepetition") %>";
        var R_OnlyOneMail = "<%=GetResource("OnlyOneMail") %>";
        var R_RepetitionMail = "<%=GetResource("RepetitionMail") %>";
        var R_CheckMailRepetition = "<%=GetResource("CheckMailRepetition") %>";
        var R_Confirm = "<%=GetResource("Confirm") %>";
        var R_DeleteInfo = "<%=GetResource("DeleteInfo") %>";
        var R_SpecialChar = "<%=GetResource("Common_revSpecialChar") %>";
        var R_MailPwdChange = "<%=GetResource("MailPwdChange") %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja

    </script>
    
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        JsCall("../Js/MyInfo/Mod.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">    
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("UserInfo")%></h1>                
                <ul class="btn_right"> 
                    <cc3:ClientSmartButton ID="cbtnUser" runat="server" Visible="false"  BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnSummary" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnAlarm" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnMobile" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnExtend" runat="server" BeginEndTag="li" />                
                    <cc3:ClientSmartButton ID="cbtnPassword" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnMailPassword" runat="server" BeginEndTag="li" />
                    <li class="sec"></li>
                    <cc3:ClientSmartButton ID="cbtnSave" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnList" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div class="dotLine">
            </div>
            <div class="popup_layout_style3">
            <!-- 기본정보 //-->
            <table class="table_typeB">
                <caption><%=GetResource("UserInfo")%></caption>
                <colgroup>
                    <col width="120px" />
                    <col width="120px" />
                    <col />
                    <col width="120px" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <td rowspan="14">
                            <ul class="acc_space">
                                <li><div id="photoImg"><%=PhotoImage %></div></li>
                                <li class="space">
                                    <ul class="btn_smallL" style="display:none">
                                        <li><a href="#" id="photoAdd" class="btn btn-sm" ><%=GetResource("Admin_UserList_cbsUserAdd")%></a></li>
                                        <li><a href="#" id="photoDel" class="btn btn-sm" ><%=GetResource("Admin_UserList_cbsUserDel")%></a></li>
                                    </ul>
                                </li>
                                <li><div id="sealImg" class="acc_sealimg"><%=SealImage%></div></li>
                                <li class="space">
                                    <ul class="btn_smallL" style="display:none">
                                        <li><a href="#" id="sealAdd" class="btn btn-sm" ><%=GetResource("Admin_UserList_cbsUserAdd")%></a</li>
                                        <li><a href="#" id="sealDel" class="btn btn-sm" ><%=GetResource("Admin_UserList_cbsUserDel")%></a></li>
                                    </ul>
                                </li>
                            </ul>
                        </td>
                        <th><%=GetResource("Admin_UserList_MSG_HTName")%></th>
                        <td><cc3:ClientSmartTextBox runat="server" ID="ctbName" Width="98%" InputMode="None" class="searchtext" Required="true"></cc3:ClientSmartTextBox></td>
                        <th><%=GetResource("MultilingualName")%>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif"/></a></th>
                        <td><cc3:ClientSmartTextBox runat="server" ID="ctbMuliLang" Width="98%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Admin_UserList_MSG_HTLoginID")%></th>
                        <td>
                            <span class="aLeft">
                                <cc3:ClientSmartTextBox runat="server" ID="ctbLoginID" Width="98%" InputMode="None"  Required="true" class="searchtext"></cc3:ClientSmartTextBox>
                            </span>
                        </td>
                        <th><%=GetResource("Admin_UserManager_ltMail")%></th>
                        <td>
                            <span class="aLeft">
                                <asp:DropDownList ID="ddlMail" runat="server" CssClass="searchselect form-control"></asp:DropDownList>                                
                            </span>
                        </td>
                        
                    </tr>
                                        
                    <tr style="display:none;">
                        <th><%=GetResource("Admin_UserManager_ltRegistrationNumber")%></th>
                        <td colspan="3"><cc3:ClientSmartTextBox runat="server" ID="ctbResNumber" Width="100px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Admin_UserManager_ltEmployeeNumber")%></th>
                        <td><cc3:ClientSmartTextBox runat="server" ID="ctbEmployeeNumber" Width="100px" InputMode="None" ReadOnly="true" class="searchtext"></cc3:ClientSmartTextBox></td>
                        <th><%=GetResource("Admin_UserManager_ltSex")%></th>
                        <td>
                            <asp:DropDownList ID="ddlSex" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Admin_UserManager_ltJoinDate")%></th>
                        <td><cc3:ClientSmartTextBox runat="server" ID="ctbJoinDate" Width="100px" InputMode="None" MaxLength="10" ReadOnly="true" class="searchtext"></cc3:ClientSmartTextBox></td>
                        <th><%=GetResource("DeptInfo")%></th>
                        <td><asp:DropDownList ID="ddlDeptInfo" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                        <th style="display:none"><%=GetResource("Admin_UserRegisterExtend_RetireDate")%></th>
                        <td style="display:none"><cc3:ClientSmartTextBox runat="server" ID="ctbRetireDate" Width="100px" InputMode="None" MaxLength="10" ReadOnly="true" class="searchtext"></cc3:ClientSmartTextBox></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Admin_UserManager_ltOfficePhone")%></th>
                        <td>
                            <cc3:ClientSmartTextBox runat="server" ID="ctbOfficePhone" Width="100px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                            (<cc3:ClientSmartTextBox runat="server" ID="ctbExtOfficePhone" Width="30px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>)
                            &nbsp;<span class="sample">ex.02-1234-5678 (101)</span>
                        </td>
                        <th><%=GetResource("Admin_DeptManager_ltFax")%></th>
                        <td><cc3:ClientSmartTextBox runat="server" ID="ctbFax" Width="100px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>&nbsp;<span class="sample">ex.02-1234-5678</span></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Admin_UserManager_ltHomeAddress")%>&nbsp;<a id="cbtnHomePost"><img src="<%=BaseImgPath%>/Icon/ico_add.gif"/></a></th>
                        <td colspan="3">
                            <cc3:ClientSmartTextBox runat="server" ID="ctbHomeZipCode" Width="50px" MaxLength="7" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                            <cc3:ClientSmartTextBox runat="server" ID="ctbHomeAddress1" Width="40%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                            <cc3:ClientSmartTextBox runat="server" ID="ctbHomeAddress2" Width="40%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                        </td>
                    </tr>
                    <tr style="display:none">
                        <th><%=GetResource("Admin_UserManager_ltBirthAddress")%>&nbsp;<a id="cbtnBirthPost"><img src="<%=BaseImgPath%>/Icon/ico_add.gif"/></a></th>
                        <td colspan="3">
                            <cc3:ClientSmartTextBox runat="server" ID="ctbBirthZipCode" Width="50px" MaxLength="7"  InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                            <cc3:ClientSmartTextBox runat="server" ID="ctbBirthAddress1" Width="40%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                            <cc3:ClientSmartTextBox runat="server" ID="ctbBirthAddress2" Width="40%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Admin_UserManager_ltHomePhone")%></th>
                        <td><cc3:ClientSmartTextBox runat="server" ID="ctbHomePhone" Width="100px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>&nbsp;<span class="sample">ex.02-1234-5678</span></td>
                        <th><%=GetResource("Private_AccountInfo_ltMobile")%></th>
                        <td><cc3:ClientSmartTextBox runat="server" ID="ctbMobilePhone" Width="100px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>&nbsp;<span class="sample">ex.010-1234-5678</span></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Admin_UserManager_ltBirthDay")%></th>
                        <td><cc3:ClientSmartTextBox runat="server" ID="ctbBirthDay" Width="100px" InputMode="None" MaxLength="10" class="searchtext"></cc3:ClientSmartTextBox><asp:CheckBox ID="cbxLunar" runat="server" />
                        <%=GetResource("Admin_UserRegisterExtend_lunar")%></td>
                        <th><%=GetResource("Admin_UserRegisterExtend_WeddingDate")%></th>
                        <td><cc3:ClientSmartTextBox runat="server" ID="ctbWedAnn" Width="100px" InputMode="None" MaxLength="10" class="searchtext"></cc3:ClientSmartTextBox></td>
                    </tr>
                    <tr>                        
                        <th><%=GetResource("BasicLanguage")%></th>
                        <td><asp:DropDownList ID="ddlBasicLanguage" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                        <th style="background-color:White"><div style="display:none"><%=GetResource("BasicTimeZone")%></div></th>
                        <td><div style="display:none"><asp:DropDownList ID="ddlBasicTimeZone" Width="270px" runat="server" CssClass="searchselect form-control"></asp:DropDownList></div></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("SystemCode")%></th>
                        <td>
                            <cc3:ClientSmartTextBox runat="server" ID="ClientSmartTextBox1" Width="100px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                        </td>
                        <th>OTP사용</th>
                        <td><span class="aLeft"><asp:CheckBox ID="cbxOTPYN" runat="server" />&nbsp;&nbsp;</span> <cc3:ClientSmartButton ID="cbtnOTP" runat="server" BeginEndTag="li" /></td>
                    </tr>
                    <tr>                        
                        <th>SNS &nbsp;<a id="cbtnSNS"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" alt="" /></a></th>
                        <td><span id="snsValue"></span></td>
                        <th style="background-color:White"><div style="display:none"><%=GetResource("TodoTitle")%></div></th>
                        <td>
                            <div style="display:none">
                            <asp:RadioButtonList ID="rbTodo" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH">
                                <asp:ListItem Text="미사용" Value="N"></asp:ListItem>
                                <asp:ListItem Text="사용" Value="Y"></asp:ListItem>
                            </asp:RadioButtonList>
                            </div>
                        </td>
                    </tr>
                    <tr style="display:none">
                        <th><%=GetResource("SystemCode")%></th>
                        <td><cc3:ClientSmartTextBox runat="server" ID="ctbSysCode" Width="100px" InputMode="None" ReadOnly="true" class="searchtext"></cc3:ClientSmartTextBox> </td>
                        <th><%=GetResource("Admin_DeptManager_ltSecurityLogIn")%></th>
                        <td><cc3:ClientSmartTextBox runat="server" ID="ctbSecurityLogIn" Width="100px" InputMode="None" ReadOnly="true" class="searchtext"></cc3:ClientSmartTextBox></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("AuthEmail")%></th>
                        <td><cc3:ClientSmartTextBox runat="server" ID="ctbAuthEmail" Width="200px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                        <th style="background-color:White"><div style="display:none">File <%=GetResource("PrivateCapacity")%></div></th>
                        <td><div style="display:none" ><cc3:ClientSmartTextBox runat="server" ID="ctbPrivateQuota" Width="100px" InputMode="None" ReadOnly="true" class="searchtext"></cc3:ClientSmartTextBox>(MB)</div></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Admin_UserManager_ltDescription")%></th>
                        <td colspan="3"><cc3:ClientSmartTextBox runat="server" ID="ctbDescription" TextMode="MultiLine" Wrap="true" Rows="10" Columns="70" Style="height: 98%; width: 100%;" InputMode="None"></cc3:ClientSmartTextBox></td>
                    </tr>
                </tbody>
            </table>
            <!-- 기본정보 //-->
            </div>
        </div>        
    </div>
    
    <div style="display: none;">
        <asp:LinkButton ID="lbtnRegister" runat="server" OnClick="lbtnRegister_Click"></asp:LinkButton>
        <asp:CheckBox ID="cbxActiveUser" Checked="true" Enabled="false" runat="server" />
        <asp:TextBox ID="tbPhotoNEW" runat="server" Width="0"></asp:TextBox>        
        <asp:TextBox ID="tbSealNEW" runat="server" Width="0"></asp:TextBox>
        <asp:TextBox ID="tbFacebook" runat="server" Width="0"></asp:TextBox>
        <asp:TextBox ID="tbFacebookPwd" runat="server" Width="0"></asp:TextBox>
        <asp:TextBox ID="tbTwitter" runat="server" Width="0"></asp:TextBox>
        <asp:TextBox ID="tbTwitterPwd" runat="server" Width="0"></asp:TextBox>
        <asp:TextBox ID="tbMe2day" runat="server" Width="0"></asp:TextBox>
        <asp:TextBox ID="tbMe2dayPwd" runat="server" Width="0"></asp:TextBox>
        <asp:TextBox ID="tbGoogle" runat="server" Width="0"></asp:TextBox>
        <asp:TextBox ID="tbGooglePwd" runat="server" Width="0"></asp:TextBox>    
        <asp:TextBox ID="tbNaverBlog" runat="server" Width="0"></asp:TextBox>
        <asp:TextBox ID="tbNaverBlogPwd" runat="server" Width="0"></asp:TextBox>    
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
</body>
</html>