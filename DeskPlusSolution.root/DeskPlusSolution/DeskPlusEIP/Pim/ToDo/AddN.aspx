<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.ToDo.Add" %>
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
    <script type="text/javascript" src="/DeskPlusEIP/Data/Editor/XFE2/js/xfe_main.js"></script> 
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body" class="write"><!-- 쓰기 및 보기화면일때 -->
    <div class="layout_full"><!-- layout_full -->
        <div id="layout_header"><!-- layout_header -->
            <h1><%=GetResource("TODOREG")%> </h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="btnContinue" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="btnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="btnCancel" runat="server" ></cc3:ClientSmartButton></li>
            </ul>
        </div><!-- //layout_header --> 
        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body" style="padding-right:30px"><!-- width 100%(col-12) -->
                    <!-- 테이블 시작 -->
		            <table class="table_typeB"> 
                    <caption><%=GetResource("TODOREG")%></caption>
		            <colgroup>
                        <col width="80px" />
                        <col />
                        <col width="80px" />
                        <col />
		            </colgroup>
                    <tbody>
                    <tr> 
                        <th><%=GetResource("Progress")%></th> 
                        <td  colspan="3">
                            <asp:radiobuttonlist id="rbtnlStatus" runat="server" RepeatLayout="UnorderedList" CssClass="pim_status"></asp:radiobuttonlist>
                        </td>                 
                    </tr> 
                    <tr> 
                        <th><%=GetResource("IMPORTANCE_MEASURE")%></th> 
                        <td><asp:DropDownList ID="ddlPriority" runat="server" style="width:100px;" CssClass="searchselect form-control"></asp:DropDownList></td> 
                        <th><%=GetResource("ViewLimit")%></th> 
                        <td><asp:DropDownList ID="ddlIsOpen" runat="server" style="width:100px;" CssClass="searchselect form-control"></asp:DropDownList></td> 
                    </tr> 
                    <tr id="tdDate" style="vertical-align:top;"> 
                        <th><%=GetResource("PimEbem")%></th> 
                        <td colspan="3" >
                        <div id="startEndDate"  >
                            <asp:DropDownList ID="ddlSchedule" runat="server" style="width:100px;" CssClass="searchselect form-control"></asp:DropDownList>&nbsp;
                            <input type="text" id="startDate" size="12"  runat="server" readonly="readonly"/>
                            <asp:DropDownList ID="ddlstartTime" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            ~
                            <input type="text" id="endDate" size="12"  runat="server" readonly="readonly" /> 
                            <asp:DropDownList ID="ddlendTime" runat="server" CssClass="searchselect form-control" ></asp:DropDownList>
                        </div>
                        </td> 
                    </tr>  
                    <tr> 
                        <th><%=GetResource("Title")%></th> 
                        <td colspan="3"><cc3:ClientSmartTextBox ID="tbTitle" InputMode="None" MaxLength="100" runat="server" Required="true" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>             
                    </tr>
                    </tbody>
                    </table> 
                    
                    <div id="tabs" class="layout_subbox">
                        <ul id="tab" class="tab" style="display:none">
                            <li><a id="ActiveX"><span>ActiveX</span> </a></li>
                            <li><a id="NonActiveX"  class="tab_on"><span>NonActiveX</span> </a></li>
                        </ul>
                        <div class="divNonEditor">
                            <div id="divXFE" style="height: 5px;">                        
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
               </div><!-- end col-9 content-area-body-left -->
            </div>
        </div>        
    </div><!-- //layout_full -->
    </div><!-- //layout_body --> 

    <div style="display:none;">
        <!-- 에디터에 필요한 필드 //-->
        <asp:TextBox ID="tbHiddenSelectedContent" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenMime_contents" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0"   runat="server"></asp:TextBox>
        <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton>        
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"  Style="display: none" rows="1" cols="20"></textarea>
        <!-- ==================================================================================== -->
    </div>
    </form>       
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" ></script>     
    <script type="text/javascript">
        var Add_InputTitle = '<%= GetResource("Add_InputTitle") %>';
        var Add_SelectOptionTop = '<%= GetResource("Add_SelectOptionTop") %>';
        var Add_InputContent = '<%= GetResource("Add_InputContent") %>';
        var Add_InputDate = '<%= GetResource("Add_InputDate") %>';
        var R_ApplyDelete = '<%= GetResource("ApplyDelete") %>';
        //        var R_FileName = '<%=GetResource("FileName") %>';
        //        var R_FileType = '<%=GetResource("FileType") %>';
        //        var R_FileLen = '<%=GetResource("FileLen") %>';
        //        var R_FileDel = '<%=GetResource("Del") %>';
        //        var SwfUploadGUID = '<%=SwfUploadGUID %>'; //flash upload GUID   
        //        var FileSizeLimit = '<%=AttachLimitSize %>'; //upload 사이즈 제한 kb

        var PageUrl = '<%=PageUrl%>'; //<%=TodoID %>
        var s_Distance = '<%=s_Distance %>';
        var s_inquryPerson = '<%=s_inquryPerson %>';
        var s_DatePeriod = '<%=s_DatePeriod %>';
        var s_SearchKind = '<%=s_SearchKind %>';
        var s_SerachValue = '<%=s_SerachValue %>';
        var s_StartDate = '<%=s_StartDate %>';
        var s_EndDate = '<%=s_EndDate %>';
        var s_UserId = '<%=s_UserID %>';
        var s_PageSize = '<%=s_PageSize %>';
        var s_CurrentPage = '<%=s_CurrentPage %>';
        var PreLang = "<%=PreLang%>";
        var ConfirmEditorTab = '<%=GetResource("ConfirmEditorTab") %>';
        var NonActiveX = '<%=NonActiveX%>';
        var ClientQueryString = '<%=Page.ClientQueryString %>';
        var AlertScriptShort = '<%=GetResource("LimitSpecialCharacter") %>' + "\n(" + LimitSpeChar + ")";
        var AlertScriptLong = '<%=GetResource("AlertScriptContnet") %>';
        var RNAME = '<%=GetResource("Name") %>';
        var RDEL = '<%=GetResource("Del") %>';
        var TimeAlert = '<%=GetResource("TimeAlert") %>';
        var WaitMent = '<%=GetResource("waitment")%>';
        var BtnCheck = 'N';
        var PreLang = "<%=PreLang%>";
        //목록버튼
        var PageScope = '<%=PageScope%>';
        var paraDate = '<%=Date%>';
        var paraOpentype = '<%=Opentype%>';
        var TimeAlert = '<%=GetResource("TimeAlert") %>';
        </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/ToDo/Add.js" ></script>  
       <script type="text/javascript">
           //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
           //JsCall("/DeskPlusEIP/Pim/Js/ToDo/Add.js");
        </script>
</body>
</html>
