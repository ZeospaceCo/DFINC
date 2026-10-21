<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.Proposal.Web.Content.Mod" %>
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
    <div id="layout_body" class="write">
        <div class="layout_full">
            <div id="layout_header"><h1>제안하기</h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnMod" runat="server" Text="제출" Class="btn btn-secondary" /></li>
                    <li><cc3:ClientSmartButton ID="cbtnList" runat="server" Text="취소" /></li>
                </ul>
            </div>
        </div>

        <div id="layout_content">
          <div class="row">
            <div class="col-12 col-lg-9 col-xl-8 content-area-body-left"><!-- width 100%(col-12) -->   
                <table class="table_typeC">
                    <caption>제안하기</caption>
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
                            <td colspan="3"><asp:Literal ID="ltlWriteName" runat="server"></asp:Literal></td>
                            <th>제안구분</th>
                            <td>
                                <asp:DropDownList ID="ddlGubun" runat="server" CssClass="searchselect form-control">
                                    <asp:ListItem Value="개인" Text="개인" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="공동" Text="공동"></asp:ListItem>
                                    <asp:ListItem Value="분임조" Text="분임조"></asp:ListItem>
                                    <asp:ListItem Value="과제" Text="과제"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>공동 제안자</th>
                            <td colspan="4" style="border-right:0px">
                                <asp:HiddenField ID="hdCoWorkID" runat="Server"></asp:HiddenField>
                                 <asp:HiddenField ID="hdCoWorkName" runat="Server"></asp:HiddenField>
                                 <div id="InputDiv1" style="height: 32px; width: 100%; padding-left: 2px; padding-top: 2px;  text-overflow: ellipsis;display:table-cell;vertical-align:middle; overflow-y: auto; vertical-align:middle" contenteditable="false"></div>
                            </td>
                            <td>
                                <ul class="btn_right">
                                    <cc3:ClientSmartButton ID="cbtnCowork" Text="추가" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <th>1차 심사자</th>
                            <td colspan="4" style="border-right:0px">
                                <asp:HiddenField ID="hdJudgeID" runat="Server"></asp:HiddenField>
                                <asp:HiddenField ID="hdJudgeName" runat="Server"></asp:HiddenField>
                                <div id="InputDiv2" style="height: 32px; width: 100%; padding-left: 2px; padding-top: 2px;  text-overflow: ellipsis;display:table-cell;vertical-align:middle; overflow-y: auto; vertical-align:middle" contenteditable="false"></div>
                            </td>
                            <td>
                                <ul class="btn_right">
                                    <cc3:ClientSmartButton ID="cbtnJudge" Text="추가" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <th>집계코드</th>
                            <td colspan="5">
                                <asp:RadioButtonList ID="RblCode" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <th>제안제목</th>
                            <td  colspan="5"><cc3:ClientSmartTextBox ID="ctbTitle" runat="server" onclick="Mod.GroupCodeCheck()" InputMode="None" Required="true" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                        </tr>
                        <tr>
                            <th>첨부파일</th>
                            <td colspan="4">
                                <asp:HiddenField ID="hdAttachFileName" runat="Server"></asp:HiddenField>
                                <div id="attachDiv1" style="height: 32px; width: 100%; padding-left: 2px; padding-top: 2px;  text-overflow: ellipsis;display:table-cell;vertical-align:middle; overflow-y: auto; vertical-align:middle" contenteditable="false"></div>
                            </td>
                            <td>
                                <ul class="btn_right">
                                
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <th>1. 현행 및 문제점</th>
                            <td colspan="5">
                                <textarea id="tbBefor" rows="6" cols="1" runat="server"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <th>2. 개선안</th>
                            <td colspan="5">
                                <textarea id="tbAfter" rows="6" cols="1" runat="server"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <th>3. 제안의 예상효과</th>
                            <td colspan="5">
                                <textarea id="tbEffect" rows="6" cols="1" runat="server"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <th>비고</th>
                            <td colspan="5">
                                <textarea id="tbEtc" rows="3" cols="1" runat="server"></textarea>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
			<div class="col-12 col-lg-3 col-xl-4 content-area-body-right">  
                <ul>
                    <!-- 첨부 파일 -->
                    <li class="title">
                            첨부파일
                    </li>
                    <li><div id="placeholder_fileList" style="clear: both;"></div></li>    
                    <!-- 첨부 파일 -->
                </ul>
                <ul>
                    <!-- 첨부 파일 -->
                    <li class="title">
                            추가할 첨부파일
                            <a id="cbtnSwfFileAdd" title="추가>"><i class="fas fa-plus-circle text-secondary"></i></a>
                    </li>
                    <li><div id="placeholder_file" style="clear: both;"></div></li>    
                    <!-- 첨부 파일 -->
                </ul>
              </div>
          </div> 
        </div>        
    </div>
    <div style="display: none;">
        <asp:LinkButton ID="lbtnAdd" runat="server" OnClick="lbtnAdd_Click"></asp:LinkButton>
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
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var EleDom = "";
        var GroupCode = "";
        var JudgeUserList = "";
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var SwfUploadGUID = "<%=SwfUploadGUID %>";
        var CoWorkList = "<%=CoWorkList %>";
        var JudgeList = "<%=JudgeList %>";
        var ProposalNo = "<%=ProposalNo %>";
        var StartYears = "<%=StartYears %>";
        var StartMonths = "<%=StartMonths %>";
        var EndYears = "<%=EndYears %>";
        var EndMonths = "<%=EndMonths %>";
        var JudgeGubun = "<%=JudgeGubun %>";
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Proposal/Js/Content/Mod.js"></script>
    <script type="text/javascript">
        //JsCall("../Js/Content/Mod.js");
    </script>
</body>
</html>
