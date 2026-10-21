<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Apps.Schedule.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />       
</head>
<body>
<form id="form1" runat="server">
<div id="layout_body" class="write">
  <div class="layout_full">

    <div id="layout_header">
        <h1><%=GetResource("ScheduleAdd")%></h1>
        <ul class="btn_right">
            <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary" /></li>
            <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server" /></li>
        </ul>
    </div>
    <div id="layout_content">
         <div class="row">
			 <div class="col-12 content-area-body" style="padding-right:30px"><!-- width 100%(col-12) -->   
                <table cellspacing="0" class="table_typeB">
		        <colgroup>
                    <col width="80px" />
                    <col />
		        </colgroup>
		        <tbody>
                <tr>
                    <th><%=GetResource("Title")%></th>
                    <td><cc3:ClientSmartTextBox ID="ctbName" InputMode="None" MaxLength="150" runat="server" Required="true" ScriptSubmitFunName="Add.Add()" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
		            <th><%=GetResource("Date")%></th>
		            <td id="tdDate">
                        <div id="startEndDate">
                            <input type="text" id="startDate" size="12"  runat="server" class="clsDate searchtext form-control" readonly /> 
				            <asp:DropDownList ID="ddlstartTime" runat="server" CssClass="searchselect form-control" ></asp:DropDownList>
				            ~
				            <input type="text" id="endDate" size="12"  runat="server" class="clsDate searchtext form-control" readonly /> 
				            <asp:DropDownList ID="ddlendTime" runat="server" CssClass="searchselect form-control" ></asp:DropDownList>
				            </div>
                    </td>
                </tr>
                <tr>
			        <th><%=GetResource("Repeat")%></th>
			        <td>
                        <span class="aLeft"><input  type="text" class="text required" value="" id="txtrepeat" runat="server" readonly style="width:400px !important;"/>&nbsp;</span>
                        <ul class="btn_smallL">
                        <li><cc3:ClientSmartButton ID="cbtnRepeat" runat="server"></cc3:ClientSmartButton></li>
                        <li><cc3:ClientSmartButton ID="cbtnRepCancel" runat="server"></cc3:ClientSmartButton></li>
                        </ul>
                    </td>
		        </tr>
                <tr>
			        <th class="last-child"><%=GetResource("ScheduleAlarm")%></th>
			        <td> 
                        <asp:DropDownList ID="ddlnotikind" runat="server" CssClass="searchselect form-control" ></asp:DropDownList>                       
                        <asp:DropDownList ID="ddltime" runat="server" CssClass="searchselect form-control"><asp:ListItem Text="Time" Value=""></asp:ListItem></asp:DropDownList>
                        <span id="divment">
                        <%=GetResource("BeforeNoti")%>
                        </span>
                    </td>
		        </tr> 
                </tbody>
                </table>

                <div id="tabs" class="layout_subbox">
                    <ul id="tab" class="tab" style="display:none">
                        <li><a id="ActiveX" ><span>ActiveX</span> </a></li>
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
 <div style="display:none;">
<asp:TextBox ID="tbRepeat" Style="display: none" runat="server" Width="0px"></asp:TextBox>   
<asp:TextBox ID="hdRepeatType" Style="display: none" runat="server" Width="0px"></asp:TextBox>
<asp:TextBox ID="hdSelectType" Style="display: none" runat="server" Width="0px"></asp:TextBox>
<asp:TextBox ID="hdIntervalCnt" Style="display: none" runat="server" Width="0px"></asp:TextBox>
<asp:TextBox ID="hdOption1" Style="display: none" runat="server" Width="0px"></asp:TextBox>
<asp:TextBox ID="hdOption2" Style="display: none" runat="server" Width="0px"></asp:TextBox>
<asp:TextBox ID="hdOption3" Style="display: none" runat="server" Width="0px"></asp:TextBox>
<asp:TextBox ID="hdOption4" Style="display: none" runat="server" Width="0px"></asp:TextBox>
<asp:TextBox ID="hdOption5" Style="display: none" runat="server" Width="0px"></asp:TextBox>
<asp:TextBox ID="hdOption6" Style="display: none" runat="server" Width="0px"></asp:TextBox>
<asp:TextBox ID="hdOption7" Style="display: none" runat="server" Width="0px"></asp:TextBox>
<asp:TextBox ID="hdMaxReptCnt" Style="display: none" runat="server" Width="0px"></asp:TextBox>
<asp:TextBox ID="hdEndDate" Style="display: none" runat="server" Width="0px"></asp:TextBox> 
<asp:LinkButton ID="lbtnConfirm" runat="server" OnClick="lbtnConfirm_Click"></asp:LinkButton> 
<!-- 에디터에 필요한 필드 //-->
<asp:TextBox ID="tbHiddenMime_contents" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>
<asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0"   runat="server"></asp:TextBox>
<textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"  Style="display: none" rows="1" cols="20"></textarea>                 
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
    var NonActiveX = "<%=NonActiveX%>";
    var ClientQueryString = "<%=Page.ClientQueryString %>";
    var RowMax = 0;
    var CommunityID = "<%=CommunityID %>";
    var MenuID = "<%=MenuID %>";
    var TmpDate = "<%=TmpDate%>";
    var BtnCheck = "N";
    var Grade = "<%=Grade %>"; 
    var R_ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";       
    var R_InputTitle = "<%=GetResource("InputTitle") %>";        
    var R_EditorScript = "<%=GetResource("EditorScript") %>";
    var R_ApplyRegDate1 = "<%=GetResource("ApplyRegDate1") %>";
    var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
    var R_StartDateEndDiffer = "<%=GetResource("StartDateEndDiffer") %>";
    var R_Processing = "<%=GetResource("Processing") %>";
    var R_Security = "<%=GetResource("LimitSpecialCharacter") %>" + "\n(" + LimitSpeChar + ")"; 
    var R_ApplyStartDate = "<%=GetResource("ApplyStartDate") %>";
    var R_NoRepeatDay = "<%=GetResource("NoRepeatDay") %>";                
    var PreLang = "<%=PreLang%>";
    var PreLangReal = "<%=PreLangReal %>"; //ja             
</script>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal %>.js" ></script>    
<script type="text/javascript" src="/DeskPlusEIP/Community/Js/Site/Apps/Schedule/Add.js" ></script>    
<script type="text/javascript">
    //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
    //JsCall("/DeskPlusEIP/Community/Js/Site/Apps/Schedule/Add.js");
</script> 
</body>
</html>
