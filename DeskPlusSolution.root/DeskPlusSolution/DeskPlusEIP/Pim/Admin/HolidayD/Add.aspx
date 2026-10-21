<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Admin.HolidayD.Add" %>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" ></script>    
    <script type="text/javascript">                            
        var SearchField = "<%=SearchField%>";
        var SearchValue = "<%=SearchValue%>";

        //에러
        var R_Holiday_Error1 = "<%=GetResource("Holiday_Error1") %>";  //휴일명
        var R_Holiday_Error2 = "<%=GetResource("Holiday_Error2") %>";  //일자
        var R_Holiday_Error4 = "<%=GetResource("Holiday_Error4") %>";  //중복

        var MultiLang = "<%=MultiLang %>";
        var PreLang = "<%=PreLang%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Admin/HolidayD/Add.js" ></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("/DeskPlusEIP/Pim/Js/Admin/HolidayD/Add.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
 <div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("HolidayManageDay_Add")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnCancel" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div id="layout_content">
            <div class="row">
                <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <table class="table_typeB">
			        <caption>
                    <%=GetResource("HolidayManageDay_Add")%>			
			        </caption>
			        <colgroup>
                        <col width="130px" />
                        <col width="130px" />
                        <col />
			        </colgroup>
			        <tbody>
				        <tr>
					        <th><%=GetResource("Holiday_Title")%></th>
					        <td colspan="2"><cc3:ClientSmartTextBox ID="ctbTitle" InputMode="None" MaxLength="50" runat="server" Width="95%" ></cc3:ClientSmartTextBox></td>
				        </tr> 
                        <tr>
                            <th><%=GetResource("MultiLang")%>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" alt="add" /></a></th>
                            <td colspan="2"><cc3:ClientSmartTextBox ID="ctbMultiName" MaxLength="100" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
                        </tr> 
                        <tr>
					        <th><%=GetResource("Holiday_IsHolidayKind2")%></th>
					        <td colspan="2"><asp:RadioButtonList ID="rblHoliDayKind" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList></td>
				        </tr>                                   
				        <tr>
					        <th><%=GetResource("Holiday_HoliDay2")%></th>
					        <td><asp:RadioButtonList ID="rblSolarLunar" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList> </td>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbHoliDayY" Width="100px" InputMode="None" MaxLength="10" class="searchtext"></cc3:ClientSmartTextBox>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbHoliDayN" Width="100px" InputMode="None" MaxLength="10" class="searchtext" ></cc3:ClientSmartTextBox>
                            </td>
				        </tr> 				                
                        <tr>
					        <th><%=GetResource("Holiday_IsHoliday")%></th>
					        <td colspan="2"><asp:RadioButtonList ID="rblIsHoliday" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList></td>
				        </tr>                  
			        </tbody>
		            </table> 
                </div>
            </div>
        </div>
    </div>
</div>
    <div style="display:none;">
    <asp:LinkButton ID="lbtnAdd" runat="server" OnClick="lbtnAdd_Click"></asp:LinkButton>    
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
</body>
</html>