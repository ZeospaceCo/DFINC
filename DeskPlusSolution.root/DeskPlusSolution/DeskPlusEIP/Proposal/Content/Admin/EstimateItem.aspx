<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EstimateItem.aspx.cs" Inherits="Zeospace.DeskPlus.Proposal.Web.Content.Admin.EstimateItem" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Proposal.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Proposal.css"/>    
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1>평가 항목 설정</h1>
            <!-- 버튼 //-->  
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" Text="확인" Class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" Text="다시작성" Class="btn btn-danger" /></li>
            </ul>          
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <div class="top">                
                        <div style="clear:both;"></div>
                        <table class="table_typeB">
                        <caption></caption>
                        <colgroup>
                            <col width="40px" />
                            <col width="170px" />
                            <col width="50px" />
                            <col width="250px" />
                            <col />
			            </colgroup>
                        <tbody>
                            <tr>
                                <th style="text-align:center">번호</th>
                                <th style="text-align:center">평가항목</th>
                                <th style="text-align:center">배점</th>
                                <th style="text-align:center">배점단위(,구분)</th>
                                <th style="text-align:center">설명</th>
                            </tr>
                            <tr>
                                <td align="center">1</td>
                                <td><cc3:ClientSmartTextBox ID="CTBName1" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:96% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBScore1" runat="server" InputMode="None" MaxLength="4" Required="true" CssStyle="width:87% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBUnit1" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBExplain1" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:97% !important;"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <td align="center">2</td>
                                <td><cc3:ClientSmartTextBox ID="CTBName2" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:96% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBScore2" runat="server" InputMode="None" MaxLength="4" Required="true" CssStyle="width:87% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBUnit2" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBExplain2" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:97% !important;"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <td align="center">3</td>
                                <td><cc3:ClientSmartTextBox ID="CTBName3" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:96% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBScore3" runat="server" InputMode="None" MaxLength="4" Required="true" CssStyle="width:87% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBUnit3" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBExplain3" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:97% !important;"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <td align="center">4</td>
                                <td><cc3:ClientSmartTextBox ID="CTBName4" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:96% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBScore4" runat="server" InputMode="None" MaxLength="4" Required="true" CssStyle="width:87% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBUnit4" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBExplain4" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:97% !important;"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <td align="center">5</td>
                                <td><cc3:ClientSmartTextBox ID="CTBName5" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:96% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBScore5" runat="server" InputMode="None" MaxLength="4" Required="true" CssStyle="width:87% !important;" onkeydown="Item.NumberCheck('0')"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBUnit5" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBExplain5" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:97% !important;"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <td align="center">6</td>
                                <td><cc3:ClientSmartTextBox ID="CTBName6" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:96% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBScore6" runat="server" InputMode="None" MaxLength="4" Required="true" CssStyle="width:87% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBUnit6" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBExplain6" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:97% !important;"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <td align="center">7</td>
                                <td><cc3:ClientSmartTextBox ID="CTBName7" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:96% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBScore7" runat="server" InputMode="None" MaxLength="4" Required="true" CssStyle="width:87% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBUnit7" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBExplain7" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:97% !important;"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <td align="center">8</td>
                                <td><cc3:ClientSmartTextBox ID="CTBName8" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:96% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBScore8" runat="server" InputMode="None" MaxLength="4" Required="true" CssStyle="width:87% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBUnit8" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBExplain8" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:97% !important;"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <td align="center">9</td>
                                <td><cc3:ClientSmartTextBox ID="CTBName9" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:96% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBScore9" runat="server" InputMode="None" MaxLength="4" Required="true" CssStyle="width:87% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBUnit9" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBExplain9" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:97% !important;"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr>
                                <td align="center">10</td>
                                <td><cc3:ClientSmartTextBox ID="CTBName10" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:96% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBScore10" runat="server" InputMode="None" MaxLength="4" Required="true" CssStyle="width:87% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBUnit10" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                                <td><cc3:ClientSmartTextBox ID="CTBExplain10" runat="server" InputMode="None" MaxLength="150" Required="true" CssStyle="width:97% !important;"></cc3:ClientSmartTextBox></td>
                            </tr>
                        </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- ================================== Hidden Input ===================================== -->	
    <div style="display:none;">
    <asp:LinkButton ID="lbtnSave" runat="server" OnClick="lbtnSave_Click"></asp:LinkButton>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Proposal/Js/Content/Admin/EstimateItem.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Proposal/Js/Content/Admin/EstimateItem.js");
    </script>
</body>
</html>
