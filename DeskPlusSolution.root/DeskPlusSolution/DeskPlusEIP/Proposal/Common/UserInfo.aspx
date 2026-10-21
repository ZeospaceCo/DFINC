<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserInfo.aspx.cs" Inherits="Zeospace.DeskPlus.Proposal.Web.Common.UserInfo" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />    
</head>
<body>
    <form id="form1" runat="server">
 <div class="layer_popup">
    <div class="inside">        
        <div class="popup_header">
            <h1>상세정보</h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server" Text="닫기" /></li>
            </ul>
        </div>        
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="overflow:hidden !Important;">
        <div class="layout_scroll" style="min-height:300px;height:300px;">
        	<!-- 테이블 시작 -->
            <table class="table_typeB">
                <caption>
                상세정보
                </caption>
                <colgroup>
                    <col width="80px"/>
                    <col/>
                    <col width="110px"/>
                </colgroup>
                <tbody>
                    <tr>
                        <th>이름</th>
                        <td>&nbsp;<asp:Literal id="ltName" runat="server"></asp:Literal>
                            <asp:Literal ID="ltImg" runat="server"></asp:Literal>
                        </td>
                        <td rowspan="5" class="last-child2" style="padding-top:9px;"><asp:image id="imgPhoto" runat="server" width="94px" height="124px" ></asp:image></td>
                    </tr>
                    <tr>
                        <th>소속부서</th>
                        <td>&nbsp;<asp:Literal ID="ltlPDeptName" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <th>기본부서</th>
                        <td>&nbsp;<asp:Literal id="ltDeptName" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <th>겸직부서</th>
                        <td>&nbsp;<asp:Literal id="ltDeptNameEtc" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <th>휴대폰</th>
                        <td>&nbsp;<asp:Literal id="ltMobile" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <th>회사전화</th>
                        <td colspan="2">&nbsp;<asp:Literal id="ltPhone" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <th>메일</th>
                        <td>&nbsp;<asp:Literal id="ltEmail" runat="server"></asp:Literal></td>
                        <td><asp:Literal id="ltSNS" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <th class="last-child">업무담당</th>
                        <td class="last-child" colspan="2">
                        <div class="layout_scroll"  style="min-height:40px;height:40px;">
                        <asp:Literal id="ltMemo" runat="server"></asp:Literal>
                        </div>
                        </td>
                    </tr>
                </tbody>
            </table>  
            <!-- 테이블 끝 -->
        </div>
        </div>

    </div>
</div>
<!-- //layout_info -->
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Proposal/Js/Common/UserInfo.js"></script>
</body>
</html>
