<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Samples.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Util.Sample.Samples" %>

<!DOCTYPE html>

<html>
<head runat="server"><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="testDiv"></div>
    <div id="testDiv1"></div>
    <div>
        <div style="display:none;">
            <input type="button" id="btn1" value="Load Customer Order" onclick="AjaxUCService('~/Util/Sample/UCSample.ascx','testDiv');" />
            <input type="button" id="btn2" value="Load Login" onclick="AjaxUCService('~/Util/Sample/UCSample.ascx','testDiv1');" />
        </div>
    </div>
    <script type="text/javascript">
        $('#btn1').trigger('click'); //실행하자마자 click이벤트를 트리거 함
        $('#btn2').trigger('click'); //실행하자마자 click이벤트를 트리거 함
    </script>
    </form>
</body>
</html>