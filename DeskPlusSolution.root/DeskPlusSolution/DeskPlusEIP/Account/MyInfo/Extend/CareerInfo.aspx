<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CareerInfo.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.MyInfo.Extend.CareerInfo" %>

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
                    <li><a href="#" id="family"><span><%=GetResource("FamilyInfo")%></span> </a></li>
                    <li><a href="#" id="education"><span><%=GetResource("EducationInfo")%></span> </a></li>
                    <li><a href="#" id="reward"><span><%=GetResource("RewardInfo")%></span> </a></li>
                    <li><a href="#" id="career" class="tab_on"><span><%=GetResource("CareerInfo")%></span> </a></li>
                    <li><a href="#" id="school"><span><%=GetResource("ScholarshipInfo")%></span> </a></li>
                </ul>


                <div class="eip_tabCont" style="min-height:535px;height:535px;">
                    <!-- 테이블 시작 -->
                    
                    <table class="table_typeA">
                        <caption><%=GetResource("CareerInfo")%></caption>
                        <colgroup>
                            <col width="180px" />
                            <col width="150px" />
                            <col width="80px" />
                            <col width="100px" />
                            <col width="100px" />
                            <col  />
                            <col width="60px" />
                        </colgroup>
                        <thead>
                            <tr>
                                <th class="center"><span style="color:Red">*</span> <%=GetResource("WorkPeriod")%></th>
                                <th class="center"><span style="color:Red">*</span> <%=GetResource("CompanyName")%></th>
                                <th class="center"><%=GetResource("CareerPeriod")%></th>
                                <th class="center"><%=GetResource("TitleName")%></th>
                                <th class="center"><%=GetResource("JobClass")%></th>
                                <th class="center"><%=GetResource("Admin_UserManager_tabs3")%></th>
                                <th class="center">&nbsp;</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="center">
                                    <cc3:ClientSmartTextBox runat="server" ID="ctbStartDate" Width="70px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox> - 
                                    <cc3:ClientSmartTextBox runat="server" ID="ctbEndDate" Width="70px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                                </td>
                                <td class="center" style="padding:0px;">
                                     <cc3:ClientSmartTextBox runat="server" ID="ctbCompanyName" Width="90%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                                </td>
                                <td class="center" style="padding:0px;">
                                    <cc3:ClientSmartTextBox runat="server" ID="ctbCareerPeriod" Width="90%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                                </td>
                                <td class="center" style="padding:0px;">
                                    <cc3:ClientSmartTextBox runat="server" ID="ctbTitleName" Width="90%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                                </td>
                                <td class="center" style="padding:0px;">
                                    <cc3:ClientSmartTextBox runat="server" ID="ctbJobType" Width="70px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                                </td>
                                <td class="center" style="padding:0px;">
                                    <cc3:ClientSmartTextBox runat="server" ID="ctbEtc" Width="95%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                                </td>
                                <td class="center" style="padding:0px;">
                                    <a class="btn" id="Save" href="#"><%=GetResource("Admin_UserRegister_Save")%></a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    

                    <div class="layout_subbox">
                    <h2><%=GetResource("CareerInfoStatus")%></h2>
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

        var R_WorkPeriod = "<%=GetResource("WorkPeriod")%>";
        var R_CompanyName = "<%=GetResource("CompanyName")%>";
        var R_CareerPeriod = "<%=GetResource("CareerPeriod")%>";
        var R_TitleName = "<%=GetResource("TitleName")%>";
        var R_JobClass = "<%=GetResource("JobClass")%>";
        var R_Admin_UserManager_tabs3 = "<%=GetResource("Admin_UserManager_tabs3")%>";
        var R_DeleteInfo = "<%=GetResource("DeleteInfo")%>";
        var R_CompleteDelete = "<%=GetResource("CompleteDelete")%>";
        var R_CompleteSave = "<%=GetResource("CompleteSave")%>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Admin_CompanyManager_btnDel = "<%=GetResource("Admin_CompanyManager_btnDel") %>";
        var R_SelectCareerDate = "<%=GetResource("SelectCareerDate") %>";
        var R_WriteCompanyName = "<%=GetResource("WriteCompanyName") %>";

        var PreLang = "<%=PreLangReal %>"; //ja
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal %>.js"></script>
    <script type="text/javascript" src="../../Js/MyInfo/Extend/CareerInfo.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("../../Js/MyInfo/Extend/CareerInfo.js");
    </script>
</body>
</html>
