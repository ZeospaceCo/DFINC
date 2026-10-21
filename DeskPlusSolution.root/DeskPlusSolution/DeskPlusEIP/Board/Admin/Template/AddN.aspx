<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Admin.Template.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
</head>
<body>
<form id="form1" runat="server">

<div id="layout_body" class="write">
    <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀,버튼 //-->
            <h1><%=GetResource("Template")%></h1>
            <ul class="btn_right">        
                <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnDel" runat="server" Class="btn btn-danger"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnCancel" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
    
        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body" style="padding-right:30px"><!-- width 100%(col-12) -->
                    <table class="table_typeB">
                    <caption>
			        <%=GetResource("AddTemplate")%>
			        </caption>
			        <colgroup>
                        <col width="80px" />
                        <col />
			        </colgroup>
			        <tbody>
                        <tr>
                            <th><%=GetResource("Title")%></th>
                            <td><cc3:ClientSmartTextBox ID="ctbName" InputMode="None" MaxLength="250" runat="server" Required="true" ScriptSubmitFunName="Add.Add()" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
                        </tr>
                    </tbody>
                    </table>
                    <div id="tabs" class="layout_subbox">
                        <ul id="tab" class="tab" style="display:none">
                            <li><a id="ActiveX"><span>ActiveX</span> </a></li>
                            <li><a id="NonActiveX"  class="tab_on"><span>NonActiveX</span> </a></li>
                        </ul>
                        <div class="divNonEditor">
                            <div id="divXFE" style="height:5px;">
                                <script type="text/javascript" src="/DeskPlusEIP/Data/Editor/XFE2/js/xfe_main.js"></script>
                                <script type="text/javascript">
                                    xfe = new XFE({
                                        basePath: "<%=BaseHttp%>" + document.domain + "/DeskPlusEIP/Data/Editor/XFE2",
                                        width: '100%',
                                        height: '550px',
                                        ignoreMinHeight: true
                                    });
                                    xfe.render('divXFE');
                                </script>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
</div>

      <!-- ================================== Hidden Input ===================================== -->	  
    <div style="display:none">
        <!-- 에디터에 필요한 필드 //-->
        <input type="hidden" id="contents" name="contents" value="<%: Contents %>" />
        <asp:TextBox ID="tbHiddenSelectedContent" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenMime_contents" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0"   runat="server"></asp:TextBox>
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"  style="display: none" rows="1" cols="20"></textarea>
	    <asp:linkbutton id="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:linkbutton>      
	</div>
    <!-- ==================================================================================== --> 
</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" >    
        var DelFileCID  = "<%=CompanyID%>";
		var DelFileUID  = "<%=UserID%>";        
		var TemplateID = "<%=TemplateID %>";
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지		
		var uploadHost  = "http://<%=Request.ServerVariables["SERVER_NAME"].ToString()%>";
        var uploadPort  = "<%=Request.ServerVariables["SERVER_PORT"].ToString()%>";
        var uploadURL   = "/DeskPlusEIP/Common/Upload/UploadFileAttach.ashx";
        var uploadPath  = "<%=UploadPath%>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_TemplateName = "<%=GetResource("TemplateName") %>";
        var R_Add_InputContent = "<%=GetResource("Add_InputContent") %>";
        var R_DoubtStcipt = "<%=GetResource("DoubtStcipt") %>";
        var R_ConfirmDeleteList = "<%=GetResource("ConfirmDeleteList") %>";
        var ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";
        var NonActiveX = "<%=NonActiveX%>";
        var ClientQueryString = "<%=Page.ClientQueryString %>";
    </script>        
    <script type="text/javascript" src="../../Js/Admin/Template/Add.js"></script>   
    <script type="text/javascript">
        //JsCall("../../Js/Admin/Template/Add.js");
    </script>
</body>
</html>
