<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllSchedule.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Schedule.AllSchedule" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />    
</head>

<body>
    <form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_full">
    
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("pim3")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnMySchedule" runat="server" ></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnExcel" runat="server" ></cc3:ClientSmartButton></li>
                <li style="display:none"><cc3:ClientSmartButton ID="CbtnTarget" runat="server" ></cc3:ClientSmartButton></li>

                <li id="HelpCategory"><span class="badge text-secondary"><i class="fas fa-info-circle" title="<%=GetResource("CategoryHelp")%>"></i></span></li>
            </ul>
        </div>

        <div id="layout_srh">
            <ul class="btn_searchbox">
                <li><div id="divShare" style="display:none"></div></li>
                <li><div id="loading"></div></li>
                <li><asp:DropDownList ID="ddlSearchKind" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
				<li>
                    <input type="text" id="dummy" style="display: none;" />                
                    <span id="spanDate" style="display:none;">
                        <input type="text" id="startDate" size="12" runat="server" readonly="readonly" class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="12" runat="server" readonly="readonly" class="clsDate searchtext form-control" />
                    </span>
				</li>
                <li>
                    <asp:DropDownList ID="ddlkey" runat="server" CssClass="searchselect form-control"></asp:DropDownList>  
                    <input name="ctbSearch" type="text" id="ctbSearch" runat="server" class="searchtext form-control" placeholder="검색어 입력" />
                </li>
				<li><a href="#" class="searchbtn btn-secondary" id="btnsearch"  ><%= GetResource("SEARCHTEXT") %></a></li>
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <!-- 테이블 시작 //-->
                    <div id="placeholder" style="clear: both;"></div>
                    <!-- 테이블 끝 //-->    
                </div>
             </div>        
        </div>
        
    </div>
    </div>

    <div style="display:none;">
    <!-- 에디터에 필요한 필드 //-->       
    <asp:TextBox ID="hdShareList" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
    <asp:LinkButton ID="lbtnexcel" runat="server" OnClick="lbtnexcel_Click"></asp:LinkButton>        
    <input type="text" id="hdexcel" runat="server" />
    <input type="text" id="hdShareTemp" runat="server" />
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script src="../../Html/Js/DeskPlusEIPComponent.js" type="text/javascript"></script>   
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ImagePath = "<%=BaseImgPath%>";
        var UserID = "<%=UserID %>";
        var CompanyID = "<%=CompanyID %>";
        var OpenType = '1,2';
        var TotalItemCount = 0;
        var PageSize = 10;  //페이지크기
        var PageSizePageNo = 1; //현재페이지
        var RecordPerPage = 10;
        var CurrentPage = "<%=CurrentPage %>";
        var R_Previous = '<%=GetResource("Previous") %>';
        var R_Next = '<%=GetResource("Next") %>';
        var StartDate = "<%=Starttime %>";
        var EndDate = "<%=Endtime %>";
        var UserList = "<%=UserList %>";
        var title = "<%=Subject %>";
        var name = "<%=name %>";
        var PreLang = "<%=PreLang%>";
        var _Searchkey = "<%=Searchkey%>";
        var _SearchValue = "<%=SearchValue %>";
        var SelectDateS = "<%=SelectDateS%>";
        var SelectDateE = "<%=SelectDateE%>";
        //목록버튼
        var PageScope = '<%=PageScope%>';
        var paraDate = '<%=Date%>';
        var paraOpentype = '<%=Opentype%>';

        function AddShare(user, uid) {
           
            var uidlist = "";
            var arrayid = uid.split(',');            
            var strHtml = "";

            for (var i = 0; i < arrayid.length - 1; i++) {
                var arrayName = arrayid[i].split('$');

                if (arrayName[0].length > 0) {
                    strHtml += arrayName[2] + "&nbsp;<a  onclick='javascript:AllSchedule.ShareUserClick(this)' value='" + arrayid[i] + "' ><font color='red'>삭제</font></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    
                    uidlist = uidlist + arrayName[1] + ",";
                }
            }
            
            if (uidlist.length > 0) {
                UserList = uidlist;
                AllSchedule.GetPageList();
                $("#hdShareList").val(UserList);
                $("#hdShareTemp").val(UserList);
            } 
        }
        var col1 = '<%=GetResource("Name") %>';
        var col2 = '<%=GetResource("KIND") %>';
        var col3 = '<%=GetResource("DATE2") %>';
        var col4 = '<%=GetResource("Time") %>';
        var col5 = '<%=GetResource("Title") %>';
        var TextEmpty = '<%=GetResource("TextEmpty") %>';
        var PreLangReal = "<%=PreLangReal %>"; //ja             
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var InputSearchDay = "<%=GetResource("InputSearchDay") %>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Schedule/AllSchedule.js" ></script> 
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("/DeskPlusEIP/Pim/Js/Schedule/AllSchedule.js");
    </script>
</body>
</html>
