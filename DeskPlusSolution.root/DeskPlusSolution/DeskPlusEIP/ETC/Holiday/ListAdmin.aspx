<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListAdmin.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Holiday.ListAdmin" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>  
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_ETC.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_ETC.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><asp:Literal ID="ltlHeader" runat="server"></asp:Literal></h1>
                <!-- 버튼 //-->
                <ul class="btn_right">  
                    <cc3:ClientSmartButton ID="cbtnExcel" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnExcelUpload" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnSample" runat="server" BeginEndTag="li" />
                                      
                    <cc3:ClientSmartButton ID="cbtnPost" runat="server" BeginEndTag="li"  Class="btn btn-secondary" />
                </ul>
            </div>
            <div id="layout_srh">            
                <ul class="btn_searchbox">
                    <li><div id="loading"></div></li>     
                    <li><asp:DropDownList ID="ddlYearField" runat="server" Width="140" CssClass="searchselect form-control"></asp:DropDownList></li>                 
                    <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>   
                    <li>
                        <input type="text" id="dummy" style="display: none;" />
                        <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                    </li>
                    <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>         
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
        <div style="display:none">        
            <asp:LinkButton ID="lbtnexcel" runat="server" OnClick="lbtnexcel_Click"></asp:LinkButton>        
            <input type="text" id="hdexcel" runat="server" />
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
        var StartDate = "<%=StartDate %>";
        var EndDate = "<%=EndDate %>";
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬  
        var R_Number = "<%=GetResource("Number") %>";
        var R_DeptName = "<%=GetResource("DeptName") %>";
        var R_UserName = "<%=GetResource("UserName") %>";
        var R_TitleName = "<%=GetResource("TitleName") %>";
        var R_DutyName = "<%=GetResource("DutyName") %>";
        var R_Gubun = "<%=GetResource("Gubun") %>";
        var R_HoliUsed = "<%=GetResource("HoliUsed") %>";
        var R_HoliReceive = "<%=GetResource("HoliReceive") %>";
        var R_HoliType1 = "<%=GetResource("HoliType1") %>";
        var R_HoliType2 = "<%=GetResource("HoliType2") %>";
        var R_HoliType3 = "<%=GetResource("HoliType3") %>";
        var R_YearText = "<%=GetResource("YearText") %>";
        var R_History = "<%=GetResource("ModHistory") %>";
        var R_ApplyRegDate = "<%=GetResource("ApplyRegDate") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS")%>";
        var R_MSG_FAIL = "<%=GetResource("MSG_FAIL") %>";
        var PageUrl = "<%= Request.Url.ToString() %>";
        var AttachFileInfoList = null;
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja
             
        var SwfUploadGUID = "<%=SwfUploadGUID %>";
        var FileType = "*.xls;*.xlsx";
        var FileTypeDesc = "Excel (*.xls *.xlsx)"; 

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal %>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/ETC/Js/Holiday/ListAdmin.js"></script>
    <script type="text/javascript">
//        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
//        JsCall("/DeskPlusEIP/ETC/Js/Holiday/ListAdmin.js");
    </script>
</body>
</html>
