<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Print.aspx.cs" EnableViewState="false" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.Carboncopy.Print" %>

<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
</head>
<body>
    <form id="form1" runat="server">
    
    <div id="toPrint">
        <div id="layout_body">
            <div class="layout_full">                 
                <div id="ContentHTML"></div> 
            </div>           
        </div>
    </div>

    </form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">
    /**********************************************************************************************
    전역 변수들
    **********************************************************************************************/
    
    Print = {}

    var CirDocumentID = "<%=CirDocumentID %>";

    $(function () {

            $(document).ready(function () {
                Print.GetContentHtmlList();
            });
    });


    Print.printElem = function (options) {
            $('#toPrint').printElement(options);
    }


    /**********************************************************************************************
        작성자   : 이문호
        작성일자 : 2012-06-08
        작성목적 : 문서 호출
    **********************************************************************************************/
    Print.GetContentHtmlList = function () {

            ParamArray = {
                'cirDocumentID': CirDocumentID
            };

            CallEncrypt(CallGetParam())

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("Print.SuccessContentHtml");
            var url = "/DeskPlusEIP/Approval/Ws/Folder/Carboncopy/View.asmx/GetContentHtml";

            AjaxService(url, data, obj);
    }

    /**********************************************************************************************
        작성자   : 이문호
        작성일자 : 2012-06-08
        작성목적 : 문서 보여주기
    **********************************************************************************************/
    Print.SuccessContentHtml = function (data, status) {
            var tmpHTML = data.d;

            if (tmpHTML != "") {
                $('#ContentHTML')[0].innerHTML = tmpHTML;
                Print.printElem({ leaveOpen: true, printMode: '' });
            }
    }
    </script>
</body>
</html>
