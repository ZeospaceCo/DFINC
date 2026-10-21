<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmPassword.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Process.Basic.ConfirmPassword" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">

<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("ApproveS")%></h1>
            <ul class="btn_right">
                <li ID="Sign" style="display:none"><cc3:ClientSmartButton ID="cbtnSign" runat="server"></cc3:ClientSmartButton></li>
                <li class="sec"></li>
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="Width:750px; height:270px">

            <table class="table_typeB">
                <caption><%=GetResource("ApproveS")%></caption>
				<colgroup>
					<col width="80px" />
					<col />
				</colgroup>
				<tbody>
					<tr id="trpwd">
						<th><%=GetResource("Password")%></th>
						<td><cc3:ClientSmartTextBox ID="ctbPwd" InputMode="None" runat="server" Required="true" TextMode="Password"></cc3:ClientSmartTextBox></td>
					</tr>
					<tr>
						<th><%=GetResource("Opinion")%> (<span class="charsRemaining"></span><%=GetResource("InChar")%>)</th>
						<td><asp:TextBox ID="tbOpinion" runat="server" TextMode="MultiLine" Width="99%" Height="210px" MaxLength="500"></asp:TextBox></td>
					</tr>
				</tbody>
			</table> 
         
        </div> 
        <div class="popup_layout_style3" style="overflow:auto;min-height:240px;height:240px">
            <!-- 알람대상 //-->
            <div class="layout_subbox">
                <h2><%=GetResource("AlarmTarget")%></h2>
            </div>
            <table class="table_typeB">
            <colgroup>
			<col width="100%" />
            </colgroup>
		    <tbody>
		    <tr>	            
                <td><input type="checkbox" id="addAlarm" name="cbxAlarm2" value="Add" /><%=GetResource("AddAlarmTarget")%></td>
            </tr>
		    </tbody>
            </table>
            
            <div id="addAlarmDiv">
                <!-- 추가 알람대상자 //-->
                <div class="layout_subbox">
                    <h2><%=GetResource("AddAlarmTarget")%></h2>
                </div>
                                
                <table class="table_typeA" id="contentTable">
                    <colgroup>
                        <col />
                        <col width="140px"/>
                        <col width="90px"/>
                        <col width="60px"/>
			        </colgroup>
			        <tbody>
				    <tr>
                        <th><%=GetResource("DeptName")%></th>
                        <th><%=GetResource("TitleName")%></th>
                        <th><%=GetResource("Name")%></th>
                        <th class="center" id="r_del"><%=GetResource("Btn_Del")%></th>
                    </tr>                
                    </tbody>
                </table>                
                
            </div>            
        </div> 
    </div>
</div>

    <div style="display: none">
        <input name="tvCompare" type="text" id="tvCompare" />
        <input name="tvAuth" type="text" id="tvAuth" />
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script> 
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ReturnType = "<%=ReturnType%>";
        var FullDomainPath = "<%=FullDomainPath %>";
     
        var RowMax = 1;
        var LoginID = "<%=CurrentUserInfo.LogInID%>";
        var Password = "<%=CurrentUserInfo.Password%>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var IsApprovePwd = "<%=IsApprovePwd%>"; // N or Y
        var IsSign = "<%=IsSign%>"; // N or Y

        var R_Msg_ErrOpinion = "<%=GetResource("Msg_ErrOpinion") %>";
        var R_Msg_ErrOpinionNon = "<%=GetResource("Msg_ErrOpinionNon") %>";
        var R_Password2 = "<%=GetResource("Password2") %>";
        var R_Password3 = "<%=GetResource("Password3") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Process/Basic/ConfirmPassword.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Process/Basic/ConfirmPassword.js");
    </script>

</body>
</html>
