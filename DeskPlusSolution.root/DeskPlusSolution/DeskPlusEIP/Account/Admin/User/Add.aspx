<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Admin.User.Add" %>

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
</head>
<body>
    <form id="form1" runat="server">

<div id="layout_body" class="write">
    <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("UserInfo")%></h1>
            <!-- 버튼 //-->            
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnSave" runat="server" Class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="cbtnList" runat="server"  /></li>
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body" style="padding-right:30px"><!-- width 100%(col-12) -->

                    <!-- 기본정보 //-->
                    <table class="table_typeB">
                        <caption><%=GetResource("UserInfo")%></caption>
                        <colgroup>
                            <col width="120px" />
                            <col width="80px" />
                            <col />
                            <col width="100px" />
                            <col />
                        </colgroup>
                        <tbody>
                            <tr>
                                <td rowspan="15">
                                    <ul class="acc_space">
                                        <li><div id="photoImg"><img src="<%=BaseImgPath%>/Icon/ico_noimg.gif" alt="photo" /></div></li>
                                        <li class="space">
                                            <ul class="btn_smallL">
                                                <li><a id="photoAdd" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Admin_UserList_cbsUserAdd")%></a></li>
                                                <li><a id="photoDel" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Admin_UserList_cbsUserDel")%></a></li>
                                            </ul>
                                        </li>
                                        <li><div id="sealImg" class="acc_sealimg"><img src="<%=BaseImgPath%>/Icon/ico_noimg2.gif" alt="Seal" /></div></li>
                                        <li class="space">
                                            <ul class="btn_smallL">
                                                <li><a id="sealAdd" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Admin_UserList_cbsUserAdd")%></a></li>
                                                <li><a id="sealDel" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Admin_UserList_cbsUserDel")%></a></li>
                                            </ul>
                                        </li>
                                    </ul>
                                </td>
                                <th><%=GetResource("Admin_UserList_MSG_HTName")%></th>
                                <td><cc3:ClientSmartTextBox runat="server" ID="ctbName" Width="98%" InputMode="None" Required="true" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th><%=GetResource("MultilingualName")%>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif"/></a></th>
                                <td><cc3:ClientSmartTextBox runat="server" ID="ctbMuliLang" Width="98%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th><%=GetResource("Admin_UserList_MSG_HTLoginID")%></th>
                                <td>
                                    <span class="aLeft">
                                        <cc3:ClientSmartTextBox runat="server" ID="ctbLoginID" Width="98%"  Required="true" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                                    </span>
                                    <ul class="btn_smallL" style="padding-left:10px;">
                                        <li><cc3:ClientSmartButton ID="cbtnLoginCheck" runat="server" Class="btn btn-sm" /></li>
                                    </ul>
                                </td>
                                <th><%=GetResource("Admin_UserManager_ltPassword")%></th>
                                <td><cc3:ClientSmartTextBox runat="server" ID="ctbPassword" Width="98%"  Required="true" TextMode="Password" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                        
                            </tr>
                            <tr>
                                <th><%=GetResource("Admin_UserManager_ltMail")%></th>
                                <td>
                                    <span class="aLeft">
                                        <cc3:ClientSmartTextBox runat="server" ID="ctbMailID" Width="100px"  Required="true" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>@
                                        <asp:DropDownList ID="ddlMailDomain" runat="server" CssClass="searchselect form-control"></asp:DropDownList>&nbsp;
                                    </span>
                                    <ul class="btn_smallL" style="padding-left:10px;">
                                        <li><cc3:ClientSmartButton ID="cbtnMailCheck" runat="server" Class="btn btn-sm" /></li>
                                    </ul>
                                </td>
                                <th><%=GetResource("Admin_UserManager_ltMailPassword")%></th>
                                <td><cc3:ClientSmartTextBox runat="server" ID="ctbMailPassword" Width="98%"  Required="true" TextMode="Password" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                        
                            </tr>
                    
                            <tr style="display:none;">
                                <th><%=GetResource("Admin_UserManager_ltRegistrationNumber")%></th>
                                <td colspan="3"><cc3:ClientSmartTextBox runat="server" ID="ctbResNumber" Width="100px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th><%=GetResource("Admin_UserManager_ltEmployeeNumber")%></th>
                                <td><cc3:ClientSmartTextBox runat="server" ID="ctbEmployeeNumber" Width="100px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th><%=GetResource("Admin_UserManager_ltSex")%></th>
                                <td>
                                    <asp:DropDownList ID="ddlSex" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th><%=GetResource("Admin_UserManager_ltJoinDate")%></th>
                                <td><cc3:ClientSmartTextBox runat="server" ID="ctbJoinDate" Width="100px" InputMode="None" MaxLength="10" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th><%=GetResource("Admin_UserRegisterExtend_RetireDate")%></th>
                                <td><cc3:ClientSmartTextBox runat="server" ID="ctbRetireDate" Width="100px" InputMode="None" MaxLength="10" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th><%=GetResource("Admin_UserManager_ltOfficePhone")%></th>
                                <td>
                                    <cc3:ClientSmartTextBox runat="server" ID="ctbOfficePhone" Width="100px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                                    (<cc3:ClientSmartTextBox runat="server" ID="ctbExtOfficePhone" Width="100px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>)
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
                            <tr>
                                <th><%=GetResource("Admin_UserManager_ltBirthAddress")%>&nbsp;<a id="cbtnBirthPost"><img src="<%=BaseImgPath%>/Icon/ico_add.gif"/></a></th>
                                <td colspan="3">
                                    <cc3:ClientSmartTextBox runat="server" ID="ctbBirthZipCode" Width="50px" MaxLength="7" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
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
                                <td><cc3:ClientSmartTextBox runat="server" ID="ctbBirthDay" Width="100px" MaxLength="10" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>&nbsp;<asp:CheckBox ID="cbxLunar" runat="server" />
                                <%=GetResource("Admin_UserRegisterExtend_lunar")%></td>
                                <th><%=GetResource("Admin_UserRegisterExtend_WeddingDate")%></th>
                                <td><cc3:ClientSmartTextBox runat="server" ID="ctbWedAnn" Width="100px" MaxLength="10" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>File <%=GetResource("PrivateCapacity")%></th>
                                <td><cc3:ClientSmartTextBox runat="server" ID="ctbPrivateQuota" Width="100px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>(MB)</td>                        
                                <th><%=GetResource("BasicLanguage")%></th>
                                <td><asp:DropDownList ID="ddlBasicLanguage" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <th><%=GetResource("Admin_DeptManager_ltSecurityLogIn")%></th>
                                <td><cc3:ClientSmartTextBox runat="server" ID="ctbSecurityLogIn" Width="100px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>SNS &nbsp;<a id="cbtnSNS"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" alt="" /></a></th>
                                <td><span id="snsValue"></span></td>
                            </tr>
                            <tr>
                                <th><%=GetResource("Admin_DeptManager_cbActive")%></th>
                                <td>
                                    <asp:CheckBox ID="cbxActiveUser" Checked="true" runat="server" />
                                </td>
                                <th><%=GetResource("SystemCode")%></th>
                                <td>
                                    <cc3:ClientSmartTextBox runat="server" ID="ctbSysCode" Width="100px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                                </td>
                            </tr>
                            <tr>
                                <th><%=GetResource("AuthEmail")%></th>
                                <td><cc3:ClientSmartTextBox runat="server" ID="ctbAuthEmail" Width="200px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>OTP사용</th>
                                <td><span class="aLeft"><asp:CheckBox ID="cbxOTPYN" runat="server" /></span></td>
                            </tr>
                            <tr>
                                <th><%=GetResource("Admin_UserManager_ltDescription")%></th>
                                <td colspan="3"><cc3:ClientSmartTextBox runat="server" ID="ctbDescription" TextMode="MultiLine" Wrap="true" Rows="10" Columns="70" Style="height: 98%; width: 100%;" InputMode="None"></cc3:ClientSmartTextBox></td>
                            </tr>
                        </tbody>
                    </table>
                 </div>
              </div>
        </div>
    </div>
</div>




    <div style="display: none;">
        <asp:LinkButton ID="lbtnRegister" runat="server" OnClick="lbtnRegister_Click"></asp:LinkButton>
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
        var DeptID = "<%=DeptID %>";
        var LoginCheck = false;
        var MailCheck = false;

        var AddCheck = false;

        var CurrentPage = "<%=CurrentPage %>";
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값
        var SearchGubun = "<%=SearchGubun %>"; //검색구분자
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var Order = "<%=Order %>";
        var ListPage = "<%=ListPage %>";     
        var MultiLang = "<%=MultiLang %>";   
        var MailProduct = "<%=MailProduct %>";   //메일 제품

        var AttachGubun = "";        
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID           
        var FileType = "*.jpg;*.jpeg;*.png;*.gif";
        var FileTypeDesc = "Images (*.jpg *.jpeg *.png *.gif)";

        var WorkYN = "<%=WorkYN %>";

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
        var R_SpecialChar = "<%=GetResource("Common_revSpecialChar") %>";

        var R_Admin_User_Input_JoinDate = "<%=GetResource("Admin_User_Input_JoinDate") %>";
        var R_Admin_User_Input_RetireDate = "<%=GetResource("Admin_User_Input_RetireDate") %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja
        
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js"></script>
    <script type="text/javascript" src="../../Js/Admin/User/Add.js"></script>
     <script type="text/javascript">
         //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
         //JsCall("../../Js/Admin/User/Add.js");
    </script>

</body>
</html>
