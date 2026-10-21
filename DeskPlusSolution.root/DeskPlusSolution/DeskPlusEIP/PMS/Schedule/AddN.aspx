<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Schedule.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PMS.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PMS.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />     
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body" class="write">
    <div class="layout_full">

        <div id="layout_header">
            <h1><%=GetResource("Schedule_Title") %></h1>
            <ul class="btn_right">        
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server" /></li>
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
			  <div class="col-12 content-area-body" style="padding-right:30px"><!-- width 100%(col-12) -->  

                    <table class="table_typeB">
                    <caption></caption>
                    <colgroup>
                        <col width="100px" />
                        <col />
                    </colgroup>
                    <tbody>
                    <tr>
                        <th><asp:Literal ID="ltTitleName" runat="server"></asp:Literal></th>
                        <td><cc3:ClientSmartTextBox ID="tbTitleName" InputMode="None" MaxLength="150" runat="server" Required="true" ScriptSubmitFunName="Add.Add()" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>          
                    </tr>
                    <asp:Panel ID="plRepeatNo" runat="server">
                    <tr>
                        <th><asp:Literal ID="ltTime" runat="server"></asp:Literal></th>
                        <td id="tdDate">
                            <asp:textbox id="tbFullYMD1" runat="server" ReadOnly="true" Width="100px"></asp:textbox>
                            <asp:dropdownlist id="ddlStartTime"  CssClass="searchselect form-control"  runat="server"></asp:dropdownlist>
                            ~
                            <asp:textbox id="tbFullYMD2" runat="server" ReadOnly="true" Width="100px"></asp:textbox>
                            <asp:dropdownlist id="ddlEndTime"  CssClass="searchselect form-control"  runat="server"></asp:dropdownlist>
                        </td>
                    </tr>
                    </asp:Panel>
                    <asp:Panel ID="plRepeat" runat="server" Visible="false">
                    <tr>
                        <th><asp:literal id="ltScheduleRepeatInfo" runat="server"></asp:literal></th>
                        <td>    
                         <span class="aLeft"><input  type="text" class="text required" value="" id="txtrepeat" readonly="readonly" runat="server" style="width:200px !important;"/>
                         &nbsp;
                         <asp:dropdownlist id="ddlStartTimeRepeat" runat="server" CssClass="searchselect form-control" ></asp:dropdownlist>&nbsp;~							
                         <asp:dropdownlist id="ddlEndTimeRepeat" runat="server" CssClass="searchselect form-control" ></asp:dropdownlist>
                         &nbsp;
                         </span>
                          <ul class="btn_smallL">
                                <li><cc3:ClientSmartButton ID="cbtnSetRepeat" runat="server" Text=""></cc3:ClientSmartButton></li>
                                <li> <cc3:ClientSmartButton ID="cbtnRepCancel" runat="server" Text=""></cc3:ClientSmartButton></li>
                           </ul>                      
                        </td>
                    </tr>
                    </asp:Panel>
                    <asp:Panel ID="plAddPims" runat="server">
                    <tr>
                        <th><%=GetResource("Schedule_Header6") %></th>
                        <td>
                            <asp:CheckBox ID="cbAddPims"  runat="Server" /> <%=GetResource("Schedule_AddPims") %>
                            <asp:Literal ID="ltAlarm" runat="server" Visible="false"></asp:Literal>  
                            <asp:DropDownList ID="ddlAlarm" runat="server" Visible="false"></asp:DropDownList> 
                         </td>
                    </tr>
                    </asp:Panel>
                    </tbody>
                </table>

                <div id="tabs" class="layout_subbox">
                    <ul id="tab" class="tab" style="display:none">
                        <li><a id="ActiveX"><span>ActiveX</span> </a></li>
                        <li><a id="NonActiveX" class="tab_on"><span>NonActiveX</span> </a></li>
                    </ul>
                    <div class="divNonEditor">
                        <div id="divXFE" style="height: 5px;">     
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
    <asp:TextBox ID="hdStart" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
    <asp:TextBox ID="hdEnd" Style="display: none"   runat="server" Width="0px"></asp:TextBox>

    <asp:TextBox ID="hdRepeat" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
    <asp:TextBox ID="hdShareList" Style="display: none"   runat="server" Width="0px"></asp:TextBox>

    
    <asp:textbox id="tbScheduleRepeat" Runat="server" Width="0" style="DISPLAY:none"></asp:textbox>    
    <asp:textbox id="tbContentHTML" TextMode="MultiLine" Width="0" runat="server" style="DISPLAY:none"></asp:textbox>
    <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0"   runat="server"></asp:TextBox>
    <asp:linkbutton id="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:linkbutton>    
    <asp:TextBox ID="tbHiddenMime_contents" TextMode="MultiLine" Width="0" runat="server"></asp:TextBox>
    <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"></textarea>
    </div>
    <!-- ==================================================================================== -->     
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script> 
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>    
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
	    var companyID = "<%=CompanyID %>";
		var isRepeat = "<%=IsRepeat %>";
		var scheduleID = "<%=ScheduleID %>";
		var status = "<%=Status %>";
        var projectID = "<%=ProjectID %>";
        var menuID = "<%=MenuID %>";
        var menuType = "<%=MenuType %>";
        var rqProjectID = "<%=RqProjectID %>";
		var Schedule_CompareTime = "<%=GetResource("Schedule_CompareTime")%>";
		var Schedule_Title = "<%=GetResource("Schedule_Title")%>";	    
        var R_ApplyRegDate = "<%=GetResource("ApplyRegDate") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var RepeatValue = "";
		var DelFileCID = "<%=CompanyID%>";
		var DelFileUID = "<%=UserID%>";
        var Add_InputTitle = "<%=GetResource("Add_InputTitle") %>";
        var Add_InputContent = "<%=GetResource("NoContent") %>";
        var AlertScriptLong = '<%=GetResource("DoubtStcipt") %>';
		var ProjParam = "<%=ProjParam %>";

        //에디터 본문 이미지 업로드		
        var uploadHost		= "http://<%=Request.ServerVariables["SERVER_NAME"].ToString()%>";
        var uploadPort		= "<%=Request.ServerVariables["SERVER_PORT"].ToString()%>";
        var uploadURL		= "/DeskPlusEIP/Common/Upload/UploadFileAttach.ashx";
        var uploadPath		= "<%=UploadPath%>";
        var R_DoubtStcipt = "<%=GetResource("DoubtStcipt") %>";
        var ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";   
        var NonActiveX = "<%=NonActiveX%>";
        var ClientQueryString = "<%=Page.ClientQueryString %>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja 

        function AddMent(ment) 
        {            
            $("#txtrepeat").val(ment);
        }

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Schedule/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/PMS/Js/Schedule/Add.js");
    </script>
</body>
</html>
