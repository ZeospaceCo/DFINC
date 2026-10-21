<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JudgeView.aspx.cs" Inherits="Zeospace.DeskPlus.Proposal.Web.Content.Judge.JudgeView" %>
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
            <div id="layout_header"><h1>제안보기 (<%=Status %>)</h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnSave" runat="server" Text="확인" Class="btn btn-secondary" /></li>
                    <li><cc3:ClientSmartButton ID="cbtnMod" runat="server" Text="다시평가" Class="btn btn-secondary"/></li>
                    <li><cc3:ClientSmartButton ID="cbtnChange" runat="server" Text="평가자 변경"  /></li>                    
                    <li><cc3:ClientSmartButton ID="cbtnPrint" runat="server" Text="인쇄" /></li>
                    <li><cc3:ClientSmartButton ID="cbtnList" runat="server" Text="취소" /></li>
                </ul>
            </div>
        </div>
        <!--본문정보-->
        <div id="layout_content">
             <div class="row">
				<div class="col-12"><!-- width 100%(col-12) -->
                    <div id="toPrint">
                        <table class="table_typeC">
                            <caption>제안보기</caption>
                            <colgroup>
                                <col width="100px" />
                                <col width="150px" />
                                <col width="100px" />
                                <col width="150px" />
                                <col width="100px" />
                                <col />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th>소속</th>
                                    <td colspan="3"><asp:Literal ID="ltlDeptName" runat="server"></asp:Literal></td>
                                    <th>접수일자</th>
                                    <td><asp:Literal ID="ltlWriteDate" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th>직위</th>
                                    <td><asp:Literal ID="ltlTitleName" runat="server"></asp:Literal></td>
                                    <th>사번</th>
                                    <td><asp:Literal ID="ltlEmpNo" runat="server"></asp:Literal></td>
                                    <th>접수번호</th>
                                    <td><asp:Literal ID="ltlNo" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th>작성자</th>
                                    <td><asp:Literal ID="ltlWriteName" runat="server"></asp:Literal></td>
                                    <th>제안구분</th>
                                    <td><asp:Literal ID="ltlGubun" runat="server"></asp:Literal></td>
                                    <th>공동 제안자</th>
                                    <td><asp:Literal ID="ltlCoWork" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th>제안제목</th>
                                    <td  colspan="5"><asp:Literal ID="ltlTitle" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th>팀분류</th>
                                    <td colspan="5"><asp:Literal ID="ltlTeamGubun" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th>첨부파일</th>
                                    <td colspan="5"><asp:Literal ID="ltlAttachFile" runat="server"></asp:Literal></td>
                                </tr>
                                <tr style="height: 150px;">
                                    <th>1. 현행 및 문제점</th>
                                    <td colspan="5"><asp:Literal ID="ltlProblem" runat="server"></asp:Literal></td>
                                </tr>
                                <tr style="height: 150px;">
                                    <th>2. 개선안</th>
                                    <td colspan="5"><asp:Literal ID="ltlSuggestion" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th>3. 제안의 예상효과</th>
                                    <td colspan="5"><asp:Literal ID="ltlEffect" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th>비고</th>
                                    <td colspan="5"><asp:Literal ID="ltlEtc" runat="server"></asp:Literal></td>
                                </tr>
                            </tbody>
                        </table>

                        <!--평가표-->
                        <div class="layout_subbox">
                            <h2>평가표</h2>
                        </div>
            
                        <asp:Literal ID="ltlExamList" runat="server"></asp:Literal>            
                        <!--평가표-->

                        <br>
                        <!--심사의견-->
                        <div id="layout_subbox"><h2>심사관 의견</h2></div>
                            <div class="popup_layout_style3">
                               <div class="layout_scroll" style="min-height:120px;height:120px;">
                                    <textarea name="textarea" id="textarea" class="textarea" style="width: 98%; border: 1px solid rgb(210, 210, 210);"></textarea>
                               </div>
                            </div>
                        </div>
                        <!--심사의견-->                                        
                </div>
                <!--본문정보-->
            </div>
        </div>        
    </div>
    <div style="display: none;">
        <asp:LinkButton ID="lbtnMod" runat="server" OnClick="lbtnMod_Click"></asp:LinkButton>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ProposalNo = "<%=ProposalNo %>";
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var ListCount = "<%=ListCount %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var MenuID = "<%=MenuID %>"; // 메뉴구분
        var CodeName = "<%=CodeName %>";
        var Count = "<%=Count %>";
        var ItemNo = "<%=ItemNo %>";
        var Sum = 0;
        var JudgeList = "<%=JudgeList %>";
        var JudgeAuth = "<%=JudgeAuth %>";
        var Score = "";
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     
        var FullDomainPath = "<%=FullDomainPath %>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Proposal/Js/Content/Judge/JudgeView.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Proposal/Js/Content/Judge/JudgeView.js");
    </script>
</body>
</html>
