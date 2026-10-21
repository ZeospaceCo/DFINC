<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="decisionRuleList.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.DecisionRule.decisionRuleList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta name="SKYPE_TOOLBAR" content ="SKYPE_TOOLBAR_PARSER_COMPATIBLE"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.modaldialog.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">

        var companyID = "<%=CurrentUserInfo.CompanyID %>";
        var categoryID = "<%=categoryID%>";
        var categoryName = "<%=categoryName%>";
        var course = "<%=course%>";
        var remark = "<%=remark%>";

        var targetID = "<%=targetID%>";
        var targetName = "<%=targetName%>";

    var parentCategoryID = 0;
    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/ETC/Js/DecisionRule/decisionRuleList.js");
    </script>
</head>
<body>
<form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1>전결 규정 등록</h1>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <table cellspacing="10">
                    <tr>
                        <td style="vertical-align:top;">
                            <!-- 회사 //-->
                            <table class="table_typeB">
                            <colgroup>
                                <col width="60px" />
                                <col width="" />
                            </colgroup>
                            <tr>
                                <th colspan="2" style="font-weight:bold;">회사</th>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <select size="10" style="width:100%" name="categoryID1" id="categoryID1" onclick="selectItem(1)"></select>
                                </td>
                            </tr>
                            <tr>
                                <th>입력</th>
                                <td><input class="InputText" type="text" name="categoryName1" id="categoryName1" style="width:80%" maxlength="250"></td>
                            </tr>
                            <tr>
                                <th>근거</th>
                                <td><input class="InputText" type="text" name="categoryCourse1" id="categoryCourse1" style="width:80%" maxlength="25"></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                <ul class="btn_left">
                                    <cc3:ClientSmartButton ID="cbtnAdd1" runat="server" BeginEndTag="li" Text="추가" />
                                    <cc3:ClientSmartButton ID="cbtnMod1" runat="server" BeginEndTag="li" Text="수정" />
                                    <cc3:ClientSmartButton ID="cbtnDel1" runat="server" BeginEndTag="li" Text="삭제" />
                                </ul>
                                <ul class="btn_right">
                                    <cc3:ClientSmartButton ID="cbtnUp1" runat="server" BeginEndTag="li" Text="▲" />
                                    <cc3:ClientSmartButton ID="cbtnDown1" runat="server" BeginEndTag="li" Text="▼" />
                                </ul>
                                </td>
                            </tr>
                            </table>
                        </td>
                        <td style="vertical-align:top;">
                            <!-- 대분류 //-->
                            <table class="table_typeB">
                            <colgroup>
                                <col width="60px" />
                                <col width="" />
                            </colgroup>
                            <tr>
                                <th colspan="2" style="font-weight:bold;">대분류</th>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <select size="10" style="width:100%" name="categoryID2" id="categoryID2" onclick="selectItem(2)"></select>
                                </td>
                            </tr>
                            <tr>
                                <th>입력</th>
                                <td><input class="InputText" type="text" name="categoryName2" id="categoryName2" style="width:80%" maxlength="250"></td>
                            </tr>
                            <tr>
                                <th>근거</th>
                                <td><input class="InputText" type="text" name="categoryCourse2" id="categoryCourse2" style="width:80%" maxlength="25"></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                <ul class="btn_left">
                                    <cc3:ClientSmartButton ID="cbtnAdd2" runat="server" BeginEndTag="li" Text="추가" />
                                    <cc3:ClientSmartButton ID="cbtnMod2" runat="server" BeginEndTag="li" Text="수정" />
                                    <cc3:ClientSmartButton ID="cbtnDel2" runat="server" BeginEndTag="li" Text="삭제" />
                                </ul>
                                <ul class="btn_right">
                                    <cc3:ClientSmartButton ID="cbtnUp2" runat="server" BeginEndTag="li" Text="▲" />
                                    <cc3:ClientSmartButton ID="cbtnDown2" runat="server" BeginEndTag="li" Text="▼" />
                                </ul>
                                </td>
                            </tr>
                            </table>
                        </td>
                        <td style="vertical-align:top;">
                            <!-- 중분류 //-->
                            <table class="table_typeB">
                            <colgroup>
                                <col width="60px" />
                                <col width="" />
                            </colgroup>
                            <tr>
                                <th colspan="2" style="font-weight:bold;">중분류</th>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <select size="10" style="width:100%" name="categoryID3" id="categoryID3" onclick="selectItem(3)"></select>
                                </td>
                            </tr>
                            <tr>
                                <th>입력</th>
                                <td><input class="InputText" type="text" name="categoryName3" id="categoryName3" style="width:80%" maxlength="250"></td>
                            </tr>
                            <tr>
                                <th>근거</th>
                                <td><input class="InputText" type="text" name="categoryCourse3" id="categoryCourse3" style="width:80%" maxlength="25"></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                <ul class="btn_left">
                                    <cc3:ClientSmartButton ID="cbtnAdd3" runat="server" BeginEndTag="li" Text="추가" />
                                    <cc3:ClientSmartButton ID="cbtnMod3" runat="server" BeginEndTag="li" Text="수정" />
                                    <cc3:ClientSmartButton ID="cbtnDel3" runat="server" BeginEndTag="li" Text="삭제" />
                                </ul>
                                <ul class="btn_right">
                                    <cc3:ClientSmartButton ID="cbtnUp3" runat="server" BeginEndTag="li" Text="▲" />
                                    <cc3:ClientSmartButton ID="cbtnDown3" runat="server" BeginEndTag="li" Text="▼" />
                                </ul>
                                </td>
                            </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top;">
                            <!-- 소분류 & 전결사항 //-->
                            <table class="table_typeB">
                            <colgroup>
                                <col width="60px" />
                                <col width="" />
                            </colgroup>
                            <tr>
                                <th colspan="2" style="font-weight:bold;">소분류 & 전결사항</th>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <select size="10" style="width:100%" name="categoryID4" id="categoryID4" onclick="selectItem(4)"></select>
                                </td>
                            </tr>
                            <tr>
                                <th>입력</th>
                                <td><input class="InputText" type="text" name="categoryName4" id="categoryName4" style="width:80%" maxlength="250"></td>
                            </tr>
                            <tr>
                                <th>근거</th>
                                <td><input class="InputText" type="text" name="categoryCourse4" id="categoryCourse4" style="width:80%" maxlength="25"></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                <ul class="btn_left">
                                    <cc3:ClientSmartButton ID="cbtnAdd4" runat="server" BeginEndTag="li" Text="추가" />
                                    <cc3:ClientSmartButton ID="cbtnMod4" runat="server" BeginEndTag="li" Text="수정" />
                                    <cc3:ClientSmartButton ID="cbtnDel4" runat="server" BeginEndTag="li" Text="삭제" />
                                    <cc3:ClientSmartButton ID="cbtnLeft" runat="server" BeginEndTag="li" Text="◀" />
                                    <cc3:ClientSmartButton ID="cbtnRight" runat="server" BeginEndTag="li" Text="▶" />
                                </ul>
                                <ul class="btn_right">
                                    <cc3:ClientSmartButton ID="cbtnUp4" runat="server" BeginEndTag="li" Text="▲" />
                                    <cc3:ClientSmartButton ID="cbtnDown4" runat="server" BeginEndTag="li" Text="▼" />
                                </ul>
                                </td>
                            </tr>
                            </table>
                        </td>
                        <td style="vertical-align:top;">
                            <!-- 전결권자 //-->
                            <table class="table_typeB">
                            <colgroup>
                                <col width="60px" />
                                <col width="" />
                            </colgroup>
                            <tr>
                                <th colspan="2" style="font-weight:bold;">전결권자</th>
                            </tr>
                            <tr style="height:187px;">
                                <td colspan="2">
                                    <select size="12" style="width:100%" name="targetID" id="targetID" onclick="targetItem()">
						               <option value="0" selected>규정없음</option>
						            </select>
                                </td>
                            </tr>
                            <tr>
                                <th>입력</th>
                                <td><input class="InputText" type="text" name="targetName" id="targetName" style="width:80%" maxlength="250"></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                <ul class="btn_left">
                                    <cc3:ClientSmartButton ID="cbtnAdd5" runat="server" BeginEndTag="li" Text="추가" />
                                    <cc3:ClientSmartButton ID="cbtnMod5" runat="server" BeginEndTag="li" Text="수정" />
                                    <cc3:ClientSmartButton ID="cbtnDel5" runat="server" BeginEndTag="li" Text="삭제" />
                                </ul>
                                <ul class="btn_right">
                                    <cc3:ClientSmartButton ID="cbtnUp5" runat="server" BeginEndTag="li" Text="▲" />
                                    <cc3:ClientSmartButton ID="cbtnDown5" runat="server" BeginEndTag="li" Text="▼" />
                                </ul>
                                </td>
                            </tr>
                            </table>
                        </td>
                        <td style="vertical-align:top;">
                            <!-- 비고 //-->
                            <table class="table_typeB">
                            <colgroup>
                                <col width="" />
                            </colgroup>
                            <tr>
                                <th style="font-weight:bold;">비고</th>
                            </tr>
                            <tr>
                                <td><textarea id="remark" name="remark" style="width:95%;height:238px;"></textarea></td>
                            </tr>
                            </table>
                        </td>
                    </tr>
                    </table>
                 </div>
             </div>
        </div>
    </div>
</div>

<div style="display:none;">
<input type="hidden" name="categoryGubun" id="categoryGubun">
<input type="hidden" name="selIdx" id="selIdx">
</div>
</form>
</body>
</html>
