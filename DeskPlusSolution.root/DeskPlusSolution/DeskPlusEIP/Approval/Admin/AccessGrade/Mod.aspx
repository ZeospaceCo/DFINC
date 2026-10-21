<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.AccessGrade.Mod" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
       
        var CompanyID = "<%=CompanyID %>";
        var AccessID = "<%=AccessID %>";
        var MultiLang = "<%=MultiLang %>";
        
        var R_Msg_Del = "<%=GetResource("Msg_Del") %>";
        var R_Msg_ErrAccessGradeName = "<%=GetResource("Msg_ErrAccessGradeName") %>";                                    
        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS") %>";                                    
        var R_MSG_FAIL = "<%=GetResource("MSG_FAIL") %>";                                            
        
        var TitleName2 = "<%=GetResource("TitleName2") %>";
        var IsActiveTit = "<%=GetResource("IsActiveTit") %>";        
        var R_NoContent = "<%=GetResource("NoContent") %>";
        
        var R_Msg_ErrAccessGradeModify = "<%=GetResource("Msg_ErrAccessGradeModify") %>";                            
        var R_MSG_SelItem = "<%=GetResource("MSG_SelItem") %>";   

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/AccessGrade/Mod.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Preference/Flow/Mod.js");
    </script>
<body>
    <form id="form1" name="form1" runat="server">
        <div id="layout_body">
            <div class="layout_full">
                <div id="layout_header">
                    <!-- 페이지타이틀 //-->
                    <h1><%=GetResource("AccessGrade")%><%=GetResource("Btn_Edit")%></h1>
                    <!-- 버튼 //-->
                    <ul class="btn_right">                        
                        <li><cc3:ClientSmartButton ID="cbtnMod" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                        <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
                    </ul>
                </div>

                <div id="layout_content">
                    <div class="row">
                        <div class="col col-lg-12 content-area-body-left" style="min-height:680px"><!-- width 100%(col-12) -->    
                            <!-- 이름.기타정보 //-->
                            <table class="table_typeB">
                                <caption></caption>
                                <colgroup>
                                    <col width="100px" />
                                    <col />
                                </colgroup>
                                <tbody>
                                    <tr>
                                        <th class="center"><%=GetResource("AccessGrades")%></th>
                                        <td>
                                            <asp:Label ID="lbAccessGrade" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th class="center"><%=GetResource("AccessGradeName")%></th>
                                        <td>
                                            <cc3:ClientSmartTextBox ID="ctbAccessGradeName" InputMode="None" MaxLength="250" runat="server" Required="true" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th class="center">
                                            <%=GetResource("MultiLang")%>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_Add.gif" /></a>
                                        </th>
                                        <td>
                                            <cc3:ClientSmartTextBox runat="server" ID="ctbMultiLang" Style="width: 95% !important;" InputMode="None" CssStyle="width:98% !important;" ></cc3:ClientSmartTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th class="center"><%=GetResource("IsActiveTit")%></th>
                                        <td>
                                            <asp:RadioButtonList ID="rblIsAccess" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList>
                                        </td>
                                    </tr>            
                                </tbody>
                            </table>
     
                            <!-- 적용직급 시작 //-->
                            <div class="layout_subbox">
                                <h2 style="display:inline;"><%=GetResource("ItemSet")%><%=GetResource("TitleName2")%>
                                <font color="red"> * <%=GetResource("AccessGradeDeptN")%><%=GetResource("TitleName2")%> (<span id="spnAccessGradeDeptN" runat="server"></span>)</font></h2>   
                                <ul class="btn_right">                             
                                    <li><asp:DropDownList ID="ddlAccessTitle" runat="server" CssClass="form-control" style="margin-right: 5px;"></asp:DropDownList><asp:DropDownList ID="ddlAccessTitleIs" runat="server" CssClass="form-control" style="display:none"></asp:DropDownList></li>                   
                                    <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server"/></li>                   
                                    <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" class="btn btn-danger" /></li>                   
                                </ul>
                            </div>
                            <div id="placeholderTitleList" style="clear: both;"></div>
                            <!-- 적용직급 끝 //-->
                        </div><!-- end col content-area-body-left -->
                    </div><!-- end row  -->
                </div>

            </div>
        </div>
        <div style="display:none">
            <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
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
</body>
</html>