<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Proposal.Web.Content.List" %>
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
            <h1>전체 제안 보기</h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnStats" runat="server" Text="통계보기" BeginEndTag="li" />
            </ul>
        </div>

        <div id="layout_srh">
            <ul class="btn_searchbox">
                <li>
                    <asp:DropDownList ID="ddlJudge" runat="server" onChange="List.Status()" CssClass="searchselect form-control">
                        <asp:ListItem Value="0" Text="전체" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="1" Text="심사 진행"></asp:ListItem>
                        <asp:ListItem Value="2" Text="심사 완료"></asp:ListItem>
                        <asp:ListItem Value="3" Text="심사 기각"></asp:ListItem>
                    </asp:DropDownList>
                </li>
                <li>
                    <asp:DropDownList ID="ddlStartYear" runat="server" CssClass="searchselect form-control">
                        <asp:ListItem Text="2012" Value="2012"></asp:ListItem>
                    </asp:DropDownList>
                </li>
                <li>
                    <asp:DropDownList ID="ddlStartMonth" runat="server" CssClass="searchselect form-control">
                        <asp:ListItem Value="1" Text="1"></asp:ListItem>
                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                        <asp:ListItem Value="4" Text="4"></asp:ListItem>
                        <asp:ListItem Value="5" Text="5"></asp:ListItem>
                        <asp:ListItem Value="6" Text="6"></asp:ListItem>
                        <asp:ListItem Value="7" Text="7"></asp:ListItem>
                        <asp:ListItem Value="8" Text="8"></asp:ListItem>
                        <asp:ListItem Value="9" Text="9"></asp:ListItem>
                        <asp:ListItem Value="10" Text="10"></asp:ListItem>
                        <asp:ListItem Value="11" Text="11"></asp:ListItem>
                        <asp:ListItem Value="12" Text="12"></asp:ListItem>
                    </asp:DropDownList>
                </li>
                <li>~</li>
                <li>
                    <asp:DropDownList ID="ddlEndYear" runat="server" CssClass="searchselect form-control">
                        <asp:ListItem Text="2012" Value="2012"></asp:ListItem>
                    </asp:DropDownList>
                </li>
                <li>
                    <asp:DropDownList ID="ddlEndMonth" runat="server" CssClass="searchselect form-control">
                        <asp:ListItem Value="1" Text="1"></asp:ListItem>
                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                        <asp:ListItem Value="4" Text="4"></asp:ListItem>
                        <asp:ListItem Value="5" Text="5"></asp:ListItem>
                        <asp:ListItem Value="6" Text="6"></asp:ListItem>
                        <asp:ListItem Value="7" Text="7"></asp:ListItem>
                        <asp:ListItem Value="8" Text="8"></asp:ListItem>
                        <asp:ListItem Value="9" Text="9"></asp:ListItem>
                        <asp:ListItem Value="10" Text="10"></asp:ListItem>
                        <asp:ListItem Value="11" Text="11"></asp:ListItem>
                        <asp:ListItem Value="12" Text="12"></asp:ListItem>
                    </asp:DropDownList>
                </li>
                <!--li><cc3:ClientSmartButton ID="cbtnSearching" runat="server" Class="searchbtn" Text="확인"></cc3:ClientSmartButton></li>--> 
                <li>&nbsp;</li> 
            
                <li><div id="loading"></div></li>                
                <li>
                    <asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control">
                        <asp:ListItem Value="Name" Text="제안자" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="Title" Text="제목"></asp:ListItem>
                        <asp:ListItem Value="Contents" Text="내용"></asp:ListItem>
                        <asp:ListItem Value="DeptName" Text="소속"></asp:ListItem>
                        <asp:ListItem Value="ParentDeptName" Text="부서"></asp:ListItem>
                        <asp:ListItem Value="5" Text="등급"></asp:ListItem>
                    </asp:DropDownList>
                </li>
                <li>
                    <input type="text" id="dummy" style="display: none;" />
                    <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None"
                        class="searchtext" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                </li>
                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn" Text="검색"></cc3:ClientSmartButton></li> 
            </ul>
        </div>

        <div id="layout_content">
             <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
            
                    <div id="phList" style="clear: both;"></div>
            
                    <!-- 테이블 끝 //-->

                    <!-- 페이징 시작 //-->
                    <div id="Pager1" name="Pager1" class="paging_num_s1">
                        <h3>
                            <span id="pageInfo"></span>
                        </h3>
                        <span id="pager"></span>
                        <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                            <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <!-- 페이징 끝 //-->
                </div>
            </div>            
        </div>
    </div>
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
        var StartYears = "<%=StartYears %>";
        var StartMonths = "<%=StartMonths %>";
        var EndYears = "<%=EndYears %>";
        var EndMonths = "<%=EndMonths %>";
        var JudgeGubun = "<%=JudgeGubun %>";
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어   
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var ListCount = "<%=ListCount %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var TotalItemCount = "0"; //총게시물수
        var R_Previous = "이전";
        var R_Next = "다음";
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Proposal/Js/Content/List.js"></script>
    <script type="text/javascript">
        //JsCall("../Js/Content/List.js");
    </script>
</body>
</html>
