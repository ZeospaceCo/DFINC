<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InOutStep1.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Outlook.InOutStep1" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Address.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Address.css"/>  
</head>
<body>
<form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_full">
            <div id="layout_header"></div>
            <div class="layout_srh clearfix"> 
                <div class="float-left">
                    <ul id="ulTabMenu" class="tab">
                        <li class="selected"><a id="csvContact" href="#"><%=GetResource("GetOutLooK")%></a></li>
                        <li style="display:none"><a href="#" id="googleContact"><%=GetResource("GetGoogleMail")%></a></li>
                        <li><a href="#"  id="exportCsv"><%=GetResource("ExportAdress")%></a></li>
                    </ul>
                </div>
            </div> 

        <div class="outmail">
            <p class="outmail_descrip"><%=GetResource("InOutStep1_Descript1")%><br />
                <%=GetResource("InOutStep1_Descript2")%> <em class="ft_blue"><%=GetResource("ExportAdress")%></em><%=GetResource("InOutStep1_Descript3")%><br />
                <%=GetResource("InOutStep1_Descript4")%> </p>
            <ul class="outmail_location">
                <li><a class="over"><%=GetResource("Load")%></a></li>
                <li><%=GetResource("UsePreview")%></li>
                <li class="last-child"><%=GetResource("Save")%></li>
            </ul>
            <div class="outmail_select"> 
    	        <span class="btnWrap"> 
                    <span class="btn_address"><input id="csvDaum" name="csvGubun" type="radio" value="daum" /><img src="<%=BaseImgPath%>/Icon/ico_mobile.gif" alt="<%=GetResource("DaumCsv")%>" /><a href="#"> <%=GetResource("DaumCsv")%></a></span>
                    <span class="btn_address"><input id="csvNaver" name="csvGubun" type="radio" value="naver" /><img src="<%=BaseImgPath%>/Icon/ico_address.gif" alt="<%=GetResource("NaverCsv")%>" /><a href="#"> <%=GetResource("NaverCsv")%></a></span>
                    <span class="btn_address"><input id="csvGoogle" name="csvGubun" type="radio" value="google" /><img src="<%=BaseImgPath%>/Icon/ico_outlook.gif" alt="<%=GetResource("GoogleCsv")%>" /><a href="#"> <%=GetResource("GoogleCsv")%></a> </span>
                    <span class="btn_address last-child"><input id="csvOutlook" name="csvGubun" type="radio" value="outlook" /><img src="<%=BaseImgPath%>/Icon/ico_outlook.gif" alt="<%=GetResource("OutlookCsv")%>" /><a href="#"> <%=GetResource("OutlookCsv")%></a> </span>
                </span>
                <div class="addfile">
        	        <span class="btnWrap">
                        <span class="list_add_style4">
                            <label for="fromfile"><%=GetResource("SelectBringAddressFile")%></label>
                            <asp:FileUpload ID="fuFile" runat="server" CssClass="searchselect form-control"/>
                            <a href="#" id="cbtnGetCSV" class="btn btn-secondary"><%=GetResource("GetAddressFile")%></a> 
                        </span>
                    </span>
                </div>        
            </div>
        </div>
    </div>
    </div>


    <div style="display:none">
        <asp:TextBox ID="tbTypes" runat="server" Width="0"></asp:TextBox>
        <asp:LinkButton ID="lbtnSumbit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var WindowObjectReference; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_SelectOutLookFormat = "<%=GetResource("SelectOutLookFormat")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Outlook/InOutStep1.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Address/Js/Outlook/InOutStep1.js");
    </script>
</body>
</html>
