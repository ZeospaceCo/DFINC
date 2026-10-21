<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZipCodeNew.aspx.cs" Inherits="Zeospace.DeskPlus.Common.Web.ZipCode.ZipCodeNew" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Common.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Common.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript">
        var CallBackPage = "<%=CallBackPage %>";
        var R_ZipCode = "<%=GetResource("ZipCode") %>";
        var R_Country = "<%=GetResource("Country") %>";
        var R_City = "<%=GetResource("City") %>";
        var R_State = "<%=GetResource("State") %>";
        var R_Detail = "<%=GetResource("Detail") %>";
        var R_HouseNumber = "<%=GetResource("HouseNumber") %>";
        var R_NotExistList = "<%=GetResource("NotExistList") %>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Common/Js/ZipCode/ListNew.js"></script>
</head>
<body>
    <form id="form1" runat="server">

    <div class="layer_popup">
        <div class="inside">
            <div id="layout_header">
                <h1><%=GetResource("InsertContact")%></h1>
                <ul class="btn_right">    
                    <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>                    
                </ul>
            </div>
            <div>
                <ul>
                    <li>
                        <input type="radio" name="rbLZipCode" value="1" id="rbLZipCode1" runat="server" onclick="List.GoZipCode()" checked /><label title="지번검색">지번검색(검색 : 읍/면/동)</label>                        
                        <input type="radio" name="rbLZipCode" value="2" id="rbLZipCode2" runat="server" onclick="List.GoZipCode()" /><label title="도로명검색">도로명검색(검색 : 도로명)</label>   
                     </li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="height:335px;border:0px solid #f00;">

                <div class="popup_srh" style="width:99%;">
                    <ul class="btn_searchbox">
                        <li><%=GetResource("State")%>
                            <input type="text" id="dummy" style="display: none;" />
                            <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                        </li>
                        <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>
                    </ul>
                </div>
                <div class="popup_content">
                    <div id="placeholder" style="overflow:auto;clear:both;height:280px;"></div>
                </div>
            </div>
        </div>
    </div>
   
    </form>
</body>
</html>
