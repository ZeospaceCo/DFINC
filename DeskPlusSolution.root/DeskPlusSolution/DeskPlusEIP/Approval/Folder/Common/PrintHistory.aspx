<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintHistory.aspx.cs" EnableViewState="false" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.Common.PrintHistory" %>

<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>    
</head>
<body>
    <form id="form1" runat="server">
    <div id="toPrint">
        <div id="ContentHTML"></div>
    </div>
    </form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">

        Print = {}

        var DocID = "<%=DocID %>";
        var HistoryID = "<%=HistoryID %>";

        $(function () {
            $(document).ready(function () {
                Print.GetContentHtmlList();
            });
        });

        Print.printElem = function (options) {
            $('#toPrint').printElement(options);
        }

        //문서 호출
        Print.GetContentHtmlList = function () {

            ParamArray = {
                'docID': DocID,
                'historyID': HistoryID
            };

            CallEncrypt(CallGetParam())

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("Print.SuccessContentHtml");
            var url = "/DeskPlusEIP/Approval/Ws/Folder/Common/ViewHistory.asmx/GetContentHtml";

            AjaxService(url, data, obj);
        }
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
