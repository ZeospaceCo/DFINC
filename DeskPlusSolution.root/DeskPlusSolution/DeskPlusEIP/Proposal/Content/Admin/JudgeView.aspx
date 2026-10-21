<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JudgeView.aspx.cs" Inherits="Zeospace.DeskPlus.Proposal.Web.Content.Admin.JudgeView" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Proposal.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Proposal.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1>심사처리</h1>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
            
                    <table class="table_typeA"> 
                         <caption>
                            심사처리
                        </caption> 
                        <colgroup>
                            <col width="150px" />
                            <col width="150px" />
                            <col width="150px"/>
                            <col  />
                        </colgroup>
                        <tr>
                            <th class="center">구분</th>
                            <th class="center">건수</th>
                            <th class="center">메뉴</th>
                            <th class="center">비고</th>
                        </tr>
                        <tr>
                            <td class="center"> 1차심사 중</td>
                            <td class="center"><asp:Literal ID="ltFirstProcess" runat="server"></asp:Literal></td>
                            <td class="center"><a onclick="View.FirstProcess();return false;" style="cursor:pointer">1차 심사 중</a></td>
                            <td class="center">1차 심사 미완료 제안 -> 2차 심사 의뢰 및 평가 완료 처리</td>
                        </tr>
                        <tr>
                            <td class="center"> 1차 심사완료</td>
                            <td class="center"><asp:Literal ID="ltFirstResult" runat="server"></asp:Literal></td>
                            <td class="center"><a onclick="View.FirstResult();return false;" style="cursor:pointer">1차 심사 결과</a></td>
                            <td class="center">1차 심사 완료 제안 중 -> 1차 심사 등급 처리</td>
                        </tr>
                    </table>
                </div>
            </div>            
        </div>
    </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var R_Previous = "이전";
        var R_Next = "다음";
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Proposal/Js/Content/Admin/JudgeView.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Proposal/Js/Content/Admin/JudgeView.js");
    </script>
</body>
</html>
