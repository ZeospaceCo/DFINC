<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListFinancial.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Customize.Admin.ListFinancial" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Etc.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Etc.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">

    <div id="layout_body">
        <div class="layout_full">
    
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1>재경팀 전용 페이지 </h1>
                <!-- 버튼 //-->
                <ul class="btn_left">
                    <li><a href="" title=""><span>버튼1</span></a></li>
                </ul>
                <ul class="btn_right">
                    <li><a href="" title=""><span>버튼2</span></a></li>
                </ul>
            </div>

            <div id="layout_srh">
                <ul class="btn_searchbox">
                    <li><div id="loading"></div></li>
                    <li>
                        <div class="searchterm">
                            <asp:DropDownList ID="ddlSearchTerm" runat="server" CssClass="searchselect">
                                <asp:ListItem Value=""></asp:ListItem>
                                <asp:ListItem Value="week"></asp:ListItem>
                                <asp:ListItem Value="month"></asp:ListItem>
                                <asp:ListItem Value="period"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div id="startEndDate" class="searchterm">
                            <input type="text" id="startDate" size="10" class="clsDate" />-<input type="text" id="endDate" size="10" class="clsDate" />
                        </div>
                    </li>
                    <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect"></asp:DropDownList></li>  
                    <li>
                        <input type="text" id="dummy" style="display: none;" />
                        <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                    </li>
                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn" Text="검색"></cc3:ClientSmartButton></li>
                </ul>
            </div>

            <div id="layout_content">

                <!-- 테이블 시작 //-->
                

                <table class="table_typeA">
                <colgroup>
                    <col />
                    <col />
                    <col />
                </colgroup>
                <thead>
                    <tr>
                        <th>문서양식</th>
                        <th><a href="#">제목</a></th>
                        <th><a href="#">상신일</a></th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>기안</td>
                        <td><a href="" title="문서보기/수정후재상신/후열처리 등의 서브메뉴">MS Certified Partner 재등록 사용료 지급.....</a></td>
                        <td>2012-01-01</td>
                    </tr>
                    <tr>
                        <td colspan="3">내용이 없습니다</td>
                    </tr>
                </tbody>
                </table>

                
                <!-- 테이블 끝 //-->

                <!-- 페이징 시작 //-->
                <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                    <h3><span id="pageInfo"></span></h3>
	                <span id="pager"></span>
                    <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight">
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

    </form>
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
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/ETC/Js/Customize/Admin/ListFinancial.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/ETC/Js/Customize/Admin/ListFinancial.js");
    </script>
</body>
</html>
