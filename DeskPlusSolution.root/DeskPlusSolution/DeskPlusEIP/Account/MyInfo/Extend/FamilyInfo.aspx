<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FamilyInfo.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.MyInfo.Extend.FamilyInfo" %>
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
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("UserInfo")%></h1>
                <ul class="btn_right">                
                    <cc3:ClientSmartButton ID="cbtnUser" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnExtend" runat="server" Visible="false"  BeginEndTag="li" />    
                    <cc3:ClientSmartButton ID="cbtnOrder" runat="server" BeginEndTag="li" />                     
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
                <ul id="tab" class="tab">
                    <li><a href="#" id="family" class="tab_on"><span><%=GetResource("FamilyInfo")%></span> </a></li>
                    <li><a href="#" id="education"><span><%=GetResource("EducationInfo")%></span></a></li>
                    <li><a href="#" id="reward"><span><%=GetResource("RewardInfo")%></span></a></li>
                    <li><a href="#" id="career"><span><%=GetResource("CareerInfo")%></span> </a></li>
                    <li><a href="#" id="school"><span><%=GetResource("ScholarshipInfo")%></span> </a></li>
                </ul>


                <div class="eip_tabCont" style="min-height:535px;height:535px;">
                    <!-- 테이블 시작 -->
                    
                    <table class="table_typeA">
                        <caption><%=GetResource("FamilyInfo")%></caption>
                        <colgroup>
                            <col width="150px" />
                            <col width="120px" />
                            <col width="100px" />
                            <col width="*" />
                            <col width="200px" />
                            <col width="150px" />
                            <col width="60px" />
                        </colgroup>
                        <thead>
                            <tr>
                                <th class="center"><span style="color:Red">*</span> <%=GetResource("Relationship")%></th>
                                <th class="center"><span style="color:Red">*</span> <%=GetResource("Private_AccountInfo_ltUserName")%></th>
                                <th class="center"><%=GetResource("Admin_UserManager_ltBirthDay")%></th>
                                <th class="center"><%=GetResource("Job")%></th>
                                <th class="center"><%=GetResource("Academic")%></th>
                                <th class="center"><%=GetResource("Cohabitation")%></th>
                                <th class="center">&nbsp;</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="center"><asp:DropDownList ID="ddlRelationship" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                                <td class="center"><cc3:ClientSmartTextBox runat="server" ID="ctbName" Width="95%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <td class="center"><cc3:ClientSmartTextBox runat="server" ID="ctbBirth" Width="95%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <td class="center"><cc3:ClientSmartTextBox runat="server" ID="ctbJob" Width="95%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <td class="center"><asp:DropDownList ID="ddlScholarship" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                                <td class="center"><asp:DropDownList ID="ddlIsWithLive" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                                <td class="center"><a class="btn" id="Save" href="#"><%=GetResource("Admin_UserRegister_Save")%></a></td>
                            </tr>
                        </tbody>
                    </table>
                    

                    <div class="layout_subbox">
                    <h2><%=GetResource("FamilyInfoState")%></h2>
                    </div>

                    <!-- 테이블 끝 -->
                    <div class="boardW">
                        
                        <div id="placeholder" style="clear: both;"></div>
                        
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var UserID = "<%=CurrentUserInfo.UserID %>";
        var DeptID = "<%=CurrentUserInfo.DeptID %>";

        var R_Relationship = "<%=GetResource("Relationship")%>";
        var R_Private_AccountInfo_ltUserName = "<%=GetResource("Private_AccountInfo_ltUserName")%>";
        var R_Admin_UserManager_ltBirthDay = "<%=GetResource("Admin_UserManager_ltBirthDay")%>";
        var R_Job = "<%=GetResource("Job")%>";
        var R_Academic = "<%=GetResource("Academic")%>";
        var R_Cohabitation = "<%=GetResource("Cohabitation")%>";
        var R_CompleteSave = "<%=GetResource("CompleteSave")%>";
        var R_DeleteInfo = "<%=GetResource("DeleteInfo")%>";
        var R_CompleteDelete = "<%=GetResource("CompleteDelete")%>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Admin_CompanyManager_btnDel = "<%=GetResource("Admin_CompanyManager_btnDel") %>";
        var R_SelectRelation = "<%=GetResource("SelectRelation") %>";
        var R_InputName = "<%=GetResource("InputName") %>";

        var PreLang = "<%=PreLangReal %>"; //ja
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal %>.js"></script>
    <script type="text/javascript" src="../../Js/MyInfo/Extend/FamilyInfo.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("../../Js/MyInfo/Extend/FamilyInfo.js");
    </script>

</body>
</html>
