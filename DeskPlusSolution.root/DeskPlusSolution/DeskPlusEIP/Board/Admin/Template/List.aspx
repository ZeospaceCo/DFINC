<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Admin.Template.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
</head>

    <form id="form1" runat="server">        
    <div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("TempManage")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
            </ul>
        </div> 

        <div id="layout_srh">
            <ul class="btn_searchbox">
                <li><div id="loading"></div></li>        
                <li><asp:DropDownList ID="DDLSearchType" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                <li><input type="text" id="dummy" style="display:none;"/><cc3:ClientSmartTextBox runat="server" ID="CTBSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox></li>
                <li><cc3:ClientSmartButton ID="CbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
            
                    <!-- 테이블 시작 //-->            
                    <div id="placeholder" style="clear: both;"></div>            
                    <!-- 테이블 끝 //-->
            
                    <!-- 페이징 시작 //-->
                    <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                        <h3><span id="pageInfo"></span></h3>
	                    <span id="pager"></span>
                        <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight" style="display:none">
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <!-- 페이징 끝 //-->
                  </div><!-- end col content-area-body-right -->                    
			</div><!-- end row  -->
		</div><!-- end layout_content  -->        
	</div><!-- end layout_full  -->
    </div><!-- end layout_body  -->
    
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>       
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>    
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var Searchkey = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var Order = " TemplateID desc"; //정렬
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = 10; //보여줄게시물수        
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var AttachFileInfoList = null;
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_Number = "<%=GetResource("Number") %>";
        var R_Title = "<%=GetResource("Title") %>";
        var R_RegDate = "<%=GetResource("RegDate") %>";
        var R_NotExistList = "<%=GetResource("NotExistList") %>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Admin/Template/List.js" ></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Admin/Template/List.js");
    </script>
</body>
</html>
