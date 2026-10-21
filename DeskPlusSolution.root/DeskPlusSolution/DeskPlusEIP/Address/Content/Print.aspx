<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Print.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Content.Print" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>    
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Address.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Address.css"/>
</head>
<body>
<form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <h1><%=GetResource("GoPrint")%></h1>
            <!-- 버튼 //-->            
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnPreview" BeginEndTag="li" runat="server"></cc3:ClientSmartButton>
                <cc3:ClientSmartButton ID="cbtnPrint" BeginEndTag="li" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton>
				<cc3:ClientSmartButton ID="cbtnList" BeginEndTag="li" runat="server"></cc3:ClientSmartButton>	
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
            <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
            <div class="add_print_left">
                <table class="table_typeB">
			<caption>
			<%=GetResource("CommunityInfo")%>
			</caption>
			<colgroup>
                <col width="80px" />
                <col />
            </colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("PrintFormat") %></th>
                    <td>
                        <select name="select2" id="select1" class="form-control">
							<option><%=GetResource("List")%></option>
						</select>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("FontSize")%></th>
                    <td><asp:DropDownList ID="ddlFontSize" runat="server" CssClass="searchselect form-control">
                            <asp:ListItem Value="12px" Text="12px" Selected="True" />
                            <asp:ListItem Value="13px" Text="13px" />
                            <asp:ListItem Value="14px" Text="14px" />
                            <asp:ListItem Value="15px" Text="15px" />
						</asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("Repeat")%></th>
                    <td><asp:DropDownList ID="ddlRepeat" runat="server" CssClass="searchselect form-control">
                            <asp:ListItem Value="1" Text="1" Selected="True" />
                            <asp:ListItem Value="2" Text="2" />
                            <asp:ListItem Value="3" Text="3" />
                            <asp:ListItem Value="4" Text="4" />
                            <asp:ListItem Value="5" Text="5" />
                            <asp:ListItem Value="6" Text="6" />
                            <asp:ListItem Value="7" Text="7" />
                            <asp:ListItem Value="8" Text="8" />
                            <asp:ListItem Value="9" Text="9" />
                            <asp:ListItem Value="10" Text="10" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("PrintItemSelect") %></th>
                    <td>
                        <ul class="add_print">
                            <li class="tit"></li>
                            <li>
                                <input type="checkbox" id="cbxAll" onclick="Print.Checked()" /> 
                                <%=GetResource("PrintItems")%>
                            </li>
                            <li>
                                <input type="checkbox" class="cbxID" value="Name" />
								<%=GetResource("FirstName")%>
                            </li>
                            <li>
                                <input type="checkbox" class="cbxID" value="Email" />
								<%=GetResource("E-Mail")%>
                            </li>
                            <li>
                                <input type="checkbox" class="cbxID" value="Mobile" />
								<%=GetResource("MobilePhone")%>
                            </li>
                            <li>
                                <input type="checkbox" class="cbxID" value="HomePhone" />
								<%=GetResource("Phone")%>
                            </li>
                            <li>
                                <input type="checkbox" class="cbxID" value="HomeZipCode" />
								<%=GetResource("HomeZipCode")%>
                            </li>
                            <li>
                                <input type="checkbox" class="cbxID" value="HomeAddress" />
								<%=GetResource("HomeAddress")%>
                            </li>
                        </ul>
                    </td>
                </tr>
            </tbody>
            </table>


				<ul class="add_print">
					<li style="display:none";>
						<span class="tit"></span>
						<span class="cont"></span>
					</li>
					<li>
						<span></span>
						
					</li>
					<li>
						<span class="tit"></span>
                        
					</li>					
				</ul>
			</div>
			<div class="add_print_right">
				<div class="iframe4">
					 <!-- 테이블 시작 -->
                     <!-- print start -->
                    <div id="toPrint" style="width:100%;">
					    <asp:Panel id="plContent" Runat="server"></asp:Panel>
					    <!-- 테이블 끝 -->
                    </div>
				</div>
			</div>
        
        </div>
        </div>
        </div>
    </div>
</div>

</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.placeholder.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script> 
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">
        var KeyID = "<%=KeyID%>";
        var Types = "<%=Types%>";
        var Contents = "";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var FullDomainPath = "<%=FullDomainPath %>";
        var R_GoPrint = "<%=GetResource("GoPrint") %>";
        var R_PrintFormat = "<%=GetResource("PrintFormat") %>";
        var R_FontSize = "<%=GetResource("FontSize") %>";
        var R_Repeat = "<%=GetResource("Repeat") %>";
        var R_PrintItemSelect = "<%=GetResource("PrintItemSelect") %>";
        var R_FirstName = "<%=GetResource("FirstName") %>";
        var R_EMail = "<%=GetResource("E-Mail") %>";
        var R_MobilePhone = "<%=GetResource("MobilePhone") %>";
        var R_Phone = "<%=GetResource("Phone") %>";
        var R_HomeZipCode = "<%=GetResource("HomeZipCode") %>";
        var R_HomeAddress = "<%=GetResource("HomeAddress") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Content/Print.js"></script>   
</body>
</html>
