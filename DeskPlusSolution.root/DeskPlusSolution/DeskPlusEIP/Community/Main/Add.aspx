<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Main.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/>    
</head>
 <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        var BtnCheck = "N";
        var R_Processing = "<%=GetResource("Processing") %>";
        var R_Security = "<%=GetResource("LimitSpecialCharacter") %>" + "\n(" + LimitSpeChar + ")"; 
        var R_InputCommunityName = "<%=GetResource("InputCommunityName") %>";
        var R_SelParentID = "<%=GetResource("SelParentID") %>";
        var R_SelChildID = "<%=GetResource("SelChildID") %>";
        var R_InputQuestion = "<%=GetResource("InputQuestion") %>";
        var R_InputAnswer = "<%=GetResource("InputAnswer") %>";
        var R_SelChildID = "<%=GetResource("SelChildID") %>";
        var R_NoCategory = "<%=GetResource("NoCategory") %>";
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Main/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Main/Add.js");
    </script>
<body>
    <form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("MakeCommunity")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->  

                    <!-- 개설신청 시작 //-->
                    <table class="table_typeB">
			        <caption></caption>
			        <colgroup>
                        <col width="120px" />
                        <col />
			        </colgroup>
			        <tbody>
                        <tr>
                            <th><%=GetResource("CategoryGubun")%></th>
                            <td>
                                <asp:DropDownList ID="ddlParent" runat="server" style="width:200px;" CssClass="searchselect form-control"></asp:DropDownList> →                       
                                <asp:DropDownList ID="ddlChild" runat="server" style="width:200px;" CssClass="searchselect form-control"></asp:DropDownList>
                                <span class="cm_subtxt">* <%=GetResource("SelGubun")%></span>
                            </td>
                        </tr>
                        <tr>
					        <th><%=GetResource("CommunityName")%></th>
					        <td>
                                <cc3:ClientSmartTextBox ID="ctbName" InputMode="None" MaxLength="30" runat="server" Required="true" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox>
                                <span class="cm_subtxt">* <%=GetResource("NameAdvice")%></span>
                            </td>
				        </tr>
                        <tr>
                            <th><%=GetResource("CommunityDes")%></th>
                            <td><cc3:ClientSmartTextBox ID="ctbDesc" MaxLength="250" runat="server" CssStyle="width:95% !important;" TextMode="MultiLine"></cc3:ClientSmartTextBox></td>
                        </tr>
                        <tr>
                            <th><%=GetResource("CommunityType")%></th>
                            <td>
                                <asp:RadioButtonList ID="rblType" runat="server" RepeatLayout="UnorderedList"></asp:RadioButtonList>  
                            </td>
                        </tr>
                        <tr id="JoinType1">
                            <th class="last-child"><%=GetResource("CommunityJoinType")%></th>
                            <td>
                                <asp:RadioButtonList ID="rblJoinType" runat="server" RepeatLayout="UnorderedList"></asp:RadioButtonList>                                              
                            </td>
                        </tr>
                        <tr id="JoinType2" style="display:none">
                            <th class="last-child"><%=GetResource("CommunityJoinType")%></th>
                            <td><%=GetResource("JoinType2")%></td>
                        </tr>
                        <tr id="JoinType3" style="display:none">
                            <th class="last-child"><%=GetResource("CommunityJoinType")%></th>
                            <td>
                                <ul>
                                    <li><%=GetResource("Question")%>: <cc3:ClientSmartTextBox ID="ctbQuestion" InputMode="None" MaxLength="20" runat="server" CssStyle="width:200px !important;"></cc3:ClientSmartTextBox></li>
                                    <li><%=GetResource("Answer")%>: <cc3:ClientSmartTextBox ID="ctbAnswer" InputMode="None" MaxLength="20" runat="server" CssStyle="width:200px !important;"></cc3:ClientSmartTextBox></li>
                                </ul> 
                            </td>
                        </tr>
			        </tbody>
		            </table> 
                </div>
             </div>            
        </div>        
    </div>
</div>
    <div style="display:none;">
        <asp:LinkButton ID="lbtnConfirm" runat="server" OnClick="lbtnConfirm_Click"></asp:LinkButton>   
        <asp:TextBox ID="tbChild" Style="display: none" runat="server" Width="0px"></asp:TextBox>    
    </div>
    </form>   
</body>
</html>
