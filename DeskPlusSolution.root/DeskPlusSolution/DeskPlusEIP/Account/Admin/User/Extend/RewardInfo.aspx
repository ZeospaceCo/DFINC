<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RewardInfo.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Admin.User.Extend.RewardInfo" %>

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
                    <cc3:ClientSmartButton ID="cbtnUser" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnExtend" runat="server" BeginEndTag="li" Visible="false"/>
                    <cc3:ClientSmartButton ID="cbtnDept" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnOrder" runat="server" BeginEndTag="li" />
                </ul>
            </div>

            <div id="layout_content">
                <div class="row">
				    <div class="col-12 content-area-body" style="padding-right:30px"><!-- width 100%(col-12) -->
                        <ul id="tab" class="tab">
                            <li><a href="#" id="family"><span><%=GetResource("FamilyInfo")%></span> </a></li>
                            <li><a href="#" id="education"><span><%=GetResource("EducationInfo")%></span> </a></li>
                            <li><a href="#" id="reward" class="tab_on"><span><%=GetResource("RewardInfo")%></span> </a></li>
                            <li><a href="#" id="career"><span><%=GetResource("CareerInfo")%></span> </a></li>
                            <li><a href="#" id="school"><span><%=GetResource("ScholarshipInfo")%></span> </a></li>
                        </ul>

                        <div class="eip_tabCont">

                        <!-- 테이블 시작 -->
                
                        <table class="table_typeA">
                            <caption><%=GetResource("RewardInfo")%></caption>
                            <colgroup>
                                <col width="100px" />
                                <col width="80px" />
                                <col width="200px" />
                                <col  />
                                <col width="100px" />
                                <col width="60px" />
                            </colgroup>
                            <thead>
                                <tr>
                                    <th class="center"><span style="color:Red">*</span> <%=GetResource("RewardDate")%></th>
                                    <th class="center"><%=GetResource("Reward")%></th>
                                    <th class="center"><%=GetResource("RewardType")%></th>
                                    <th class="center"><span style="color:Red">*</span> <%=GetResource("Summary")%></th>
                                    <th class="center"><span style="color:Red">*</span> <%=GetResource("RewardPerson")%></th>
                                    <th class="center">&nbsp;</th>
                                </tr>
                            </thead>
                            <tbody>
                                                            <tr>
                                <td class="center"><cc3:ClientSmartTextBox runat="server" ID="ctbRewardDate" Width="95%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <td class="center"><asp:DropDownList ID="ddlGubun" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                                <td class="center"><cc3:ClientSmartTextBox runat="server" ID="ctbRewardType" Width="95%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <td class="center"><cc3:ClientSmartTextBox runat="server" ID="ctbContents" Width="95%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <td class="center"><cc3:ClientSmartTextBox runat="server" ID="ctbRewarder" Width="95%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <td class="center"><a class="btn" id="Save" href="#"><%=GetResource("Admin_UserRegister_Save")%></a></td>
                            </tr>
                            </tbody>
                        </table>
                
                        <div class="layout_subbox">
                        <h2><%=GetResource("RewardInfoState")%></h2>
                        </div>
                        <!-- 테이블 끝 -->
                        <div class="boardW">
                    
                            <div id="placeholder" style="clear: both;"></div>
                    
                        </div>
                      </div>
                    </div>
            </div>
        </div>
    </div>
</div>
    <!-- //layout_info -->
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
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var UserID = "<%=UserID %>";
        var DeptID = "<%=DeptID %>";

        var CurrentPage = "<%=CurrentPage %>";
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값
        var SearchGubun = "<%=SearchGubun %>"; //검색구분자
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var Order = "<%=Order %>";
        var ListPage = "<%=ListPage %>";

        var R_RewardDate = "<%=GetResource("RewardDate")%>";
        var R_Reward = "<%=GetResource("Reward")%>";
        var R_RewardType = "<%=GetResource("RewardType")%>";
        var R_Summary = "<%=GetResource("Summary")%>";
        var R_RewardPerson = "<%=GetResource("RewardPerson")%>";
        var R_CompleteSave = "<%=GetResource("CompleteSave")%>";
        var R_DeleteInfo = "<%=GetResource("DeleteInfo")%>";
        var R_Admin_DeptManager_MSG_DelSUCCESS = "<%=GetResource("Admin_DeptManager_MSG_DelSUCCESS")%>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Admin_DeptManager_btnDel = "<%=GetResource("Admin_DeptManager_btnDel")%>";
        var R_SelectRewardDate = "<%=GetResource("SelectRewardDate") %>";
        var R_WriteSummary = "<%=GetResource("WriteSummary") %>";
        var R_WriteRewarder = "<%=GetResource("WriteRewarder") %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja
        
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js"></script>
    <script type="text/javascript" src="../../../Js/Admin/User/Extend/RewardInfo.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("../../../Js/Admin/User/Extend/RewardInfo.js");
    </script>
</html>
