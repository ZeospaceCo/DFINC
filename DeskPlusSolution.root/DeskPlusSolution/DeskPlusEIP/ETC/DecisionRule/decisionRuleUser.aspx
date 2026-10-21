<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="decisionRuleUser.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.DecisionRule.decisionRuleUser" %>
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

        //사업부 리스트
        var categoryID = "<%=categoryID%>";
        var categoryName = "<%=categoryName%>";
        var course = "<%=course%>";
        var remark = "<%=remark%>";

        //전결권자 리스트
        var targetIDList = "<%=targetID%>";
        var targetNameList = "<%=targetName%>";

    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/ETC/Js/DecisionRule/decisionRuleUser.js");
    </script>
</head>
<body>
<form id="form1" runat="server">

    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1>전결 규정</h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server" Text="닫기"></cc3:ClientSmartButton></li>
                </ul>            
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:170px;">

                <table cellspacing="5">
                <tr>
                    <td style="vertical-align:top;">
                        <!-- 회사 //-->
                        <table class="table_typeB">
                        <colgroup>
                            <col width="" />
                        </colgroup>
                        <tr>
                            <th style="font-weight:bold;">회사</th>
                        </tr>
                        <tr>
                            <td><select size="10" style="width:100%" name="categoryID1" id="categoryID1" onclick="selectItem(1)"></select></td>
                        </tr>
                        </table>
                    </td>
                    <td style="vertical-align:top;">
                        <!-- 대분류 //-->
                        <table class="table_typeB">
                        <colgroup>
                            <col width="" />
                        </colgroup>
                        <tr>
                            <th style="font-weight:bold;">대분류</th>
                        </tr>
                        <tr>
                            <td><select size="10" style="width:100%" name="categoryID2" id="categoryID2" onclick="selectItem(2)"></select></td>
                        </tr>
                        </table>
                    </td>
                    <td style="vertical-align:top;">
                        <!-- 중분류 //-->
                        <table class="table_typeB">
                        <colgroup>
                            <col width="" />
                        </colgroup>
                        <tr>
                            <th style="font-weight:bold;">중분류</th>
                        </tr>
                        <tr>
                            <td><select size="10" style="width:100%" name="categoryID3" id="categoryID3" onclick="selectItem(3)"></select></td>
                        </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align:top;" colspan="2">
                        <!-- 소분류 & 전결사항 //-->
                        <table class="table_typeB">
                        <colgroup>
                            <col width="" />
                        </colgroup>
                        <tr>
                            <th style="font-weight:bold;">소분류 & 전결사항</th>
                        </tr>
                        <tr>
                            <td><select size="10" style="width:100%" name="categoryID4" id="categoryID4" onclick="selectItem(4)"></select></td>
                        </tr>
                        </table>
                    </td>
                    <td style="vertical-align:top;">
                        <!-- 전결권자 //-->
                        <table class="table_typeB">
                        <colgroup>
                            <col width="" />
                        </colgroup>
                        <tr>
                            <th style="font-weight:bold;">전결권자</th>
                        </tr>
                        <tr>
                            <td>
                                <select size="10" style="width:100%" name="targetID" id="targetID" readonly>
						            <option value="0" selected>규정없음</option>
						        </select>
                            </td>
                        </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align:top;" colspan="3">
                        <!-- 선택사항 & 비고 //-->
                        <table class="table_typeB">
                        <colgroup>
                            <col width="60px" />
                            <col width="" />
                        </colgroup>
                        <tr>
                            <th style="font-weight:bold;">선택사항</th>
                            <td><textarea class="InputText" type="text" name="selTarget" id="selTarget" style="width:98%;height:50px;" readonly></textarea></td>
                        </tr>
                        <tr>
                            <th style="font-weight:bold;">비고</th>
                            <td><textarea class="InputText" type="text" name="selItem" id="selItem" style="width:98%;height:70px;" readonly></textarea></td>
                        </tr>
                        </table>
                    </td>
                </tr>
                </table>



            </div>                
        </div>
    </div>


<div style="display:none;">
<input type="hidden" id="etcVal1" name="etcVal1">
<input type="hidden" id="etcVal2" name="etcVal2">
<input type="hidden" id="etcVal3" name="etcVal3">
<input type="hidden" id="etcVal4" name="etcVal4">
</div>
</form>
</body>
</html>
