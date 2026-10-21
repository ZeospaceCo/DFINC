<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LinkageInfo.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Admin.User.LinkageInfo" %>
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
    <script type="text/javascript">
        var CompanyID = "<%=CompanyID %>";
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

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja
    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        JsCall("../../Js/Admin/User/LinkageInfo.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_one">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("LinkageInfo")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">   
                <cc3:ClientSmartButton ID="cbtnUser" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="cbtnExtend" runat="server" Visible="false" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="cbtnDept" runat="server" Visible="false" BeginEndTag="li" />

                <cc3:ClientSmartButton ID="cbtnConfirm" runat="server" BeginEndTag="li" Class="btn btn-secondary" />              
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body" style="padding-right:30px"><!-- width 100%(col-12) -->

                    <!-- 테이블 시작 -->
                
                    <table class="table_typeB">
                        <caption></caption>
                        <colgroup>
                            <col width="130px"/>
                            <col />
                            <col width="130px"/>
                            <col />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th>전표용부서</th>
                                <td><cc3:ClientSmartTextBox ID="ctbDeptName" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>은행계좌번호</th>
                                <td><cc3:ClientSmartTextBox ID="ctbBankNum" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>커뮤니티 포인트</th>
                                <td><cc3:ClientSmartTextBox ID="ctbComPoint" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>은행명</th>
                                <td><cc3:ClientSmartTextBox ID="ctbBankName" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>KMS사용자코드</th>
                                <td><cc3:ClientSmartTextBox ID="ctbKmsUserCode" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>KMS사용자등급명</th>
                                <td><cc3:ClientSmartTextBox ID="ctbKmsUserGrade" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>직군코드</th>
                                <td><cc3:ClientSmartTextBox ID="ctbJobCode" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>직군명</th>
                                <td><cc3:ClientSmartTextBox ID="ctbJobName" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>차량명</th>
                                <td><cc3:ClientSmartTextBox ID="ctbCarName" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>차량번호</th>
                                <td><cc3:ClientSmartTextBox ID="ctbCarNumber" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>카페머니</th>
                                <td><cc3:ClientSmartTextBox ID="ctbMoney" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>종합검진대상유무</th>
                                <td><cc3:ClientSmartTextBox ID="ctbAllCheck" runat="server" Width="90%" class="searchtext" MaxLength="1"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>인사평가대상</th>
                                <td><cc3:ClientSmartTextBox ID="ctbMPDS" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>인사평가제외사유</th>
                                <td><cc3:ClientSmartTextBox ID="ctbReason" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>1차평가자명</th>
                                <td><cc3:ClientSmartTextBox ID="ctbFName" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>2차평가자명</th>
                                <td><cc3:ClientSmartTextBox ID="ctbSName" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>1차평가자유무</th>
                                <td><cc3:ClientSmartTextBox ID="ctbFGubun" runat="server" Width="90%" class="searchtext" MaxLength="1"></cc3:ClientSmartTextBox></td>
                                <th>2차평가자유무</th>
                                <td><cc3:ClientSmartTextBox ID="ctbSGubun" runat="server" Width="90%" class="searchtext" MaxLength="1"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>주민등록번호</th>
                                <td><cc3:ClientSmartTextBox ID="ctbIDNumber" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>개인이메일</th>
                                <td><cc3:ClientSmartTextBox ID="ctbPrivateMail" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>신장</th>
                                <td><cc3:ClientSmartTextBox ID="ctbHeight" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>몸무게</th>
                                <td><cc3:ClientSmartTextBox ID="ctbWeight" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>시력 좌</th>
                                <td><cc3:ClientSmartTextBox ID="ctbSightL" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>시력 우</th>
                                <td><cc3:ClientSmartTextBox ID="ctbSightR" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>혈액형</th>
                                <td><cc3:ClientSmartTextBox ID="ctbBloodType" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>특기</th>
                                <td><cc3:ClientSmartTextBox ID="ctbSkill" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>취미</th>
                                <td><cc3:ClientSmartTextBox ID="ctbHobby" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>종교</th>
                                <td><cc3:ClientSmartTextBox ID="ctbReligion" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>주거형태</th>
                                <td><cc3:ClientSmartTextBox ID="ctbLivingType" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>소유구분</th>
                                <td><cc3:ClientSmartTextBox ID="ctbOwnType" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>병역필 유무</th>
                                <td><cc3:ClientSmartTextBox ID="ctbArmyType" runat="server" Width="90%" class="searchtext" MaxLength="1"></cc3:ClientSmartTextBox></td>
                                <th>병역필 구분</th>
                                <td><cc3:ClientSmartTextBox ID="ctbArmyGubun" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>군번</th>
                                <td><cc3:ClientSmartTextBox ID="ctbArmyNumber" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>군별</th>
                                <td><cc3:ClientSmartTextBox ID="ctbArmyGroup" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>병과</th>
                                <td><cc3:ClientSmartTextBox ID="ctbArmyBranch" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>입대년월</th>
                                <td><cc3:ClientSmartTextBox ID="ctbArmyJoinDate" runat="server" Width="90%" InputMode="None" MaxLength="10" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>전역년월</th>
                                <td><cc3:ClientSmartTextBox ID="ctbArmyRetireDate" runat="server" Width="90%" InputMode="None" MaxLength="10" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th>계급</th>
                                <td><cc3:ClientSmartTextBox ID="ctbClass" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>보훈대상유무</th>
                                <td><cc3:ClientSmartTextBox ID="ctbVeteranType" runat="server" Width="90%" class="searchtext" MaxLength="1"></cc3:ClientSmartTextBox></td>
                                <th>보훈번호</th> 
                                <td><cc3:ClientSmartTextBox ID="ctbVeteranNumber" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>장애 유무</th>
                                <td><cc3:ClientSmartTextBox ID="ctbObstacleType" runat="server" Width="90%" class="searchtext" MaxLength="1"></cc3:ClientSmartTextBox></td>
                                <th>장애번호</th>
                                <td><cc3:ClientSmartTextBox ID="ctbObstacleNumber" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <th>비고</th>
                                <td colspan="3"><cc3:ClientSmartTextBox ID="ctbEtc" TextMode="MultiLine" runat="server" Rows="3"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr class="noVisible">
                                <th></th>
                                <td><cc3:ClientSmartTextBox ID="ctbEtcPlus1" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th></th>
                                <td><cc3:ClientSmartTextBox ID="ctbEtcPlus2" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr class="noVisible">
                                <th></th>
                                <td><cc3:ClientSmartTextBox ID="ctbEtcPlus3" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th></th>
                                <td><cc3:ClientSmartTextBox ID="ctbEtcPlus4" runat="server" Width="90%"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr class="noVisible">
                                <th></th>
                                <td><cc3:ClientSmartTextBox ID="ctbEtcPlus5" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th></th>
                                <td><cc3:ClientSmartTextBox ID="ctbEtcPlus6" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr class="noVisible">
                                <th></th>
                                <td><cc3:ClientSmartTextBox ID="ctbEtcPlus7" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th></th>
                                <td><cc3:ClientSmartTextBox ID="ctbEtcPlus8" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr class="noVisible">
                                <th></th>
                                <td><cc3:ClientSmartTextBox ID="ctbEtcPlus9" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
                                <th></th>
                                <td><cc3:ClientSmartTextBox ID="ctbEtcPlus10" runat="server" Width="90%" class="searchtext"></cc3:ClientSmartTextBox></td>
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
    </div>
    </form>
</body>
</html>
