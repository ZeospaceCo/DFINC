<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddDetail.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Customize.User.AddDetail" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Etc.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Etc.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body style="overflow:auto !important;">
    <form id="form1" runat="server">

        <div style="margin:0px;padding:0px;height:100%;width:100%;">

            <!-- 테이블 시작 //-->
            <table class="table_typeC">
            <colgroup>
                <col width="25px" />
                <col />
                <col />
                <col />
                <col />
                <col />
                <col />
                 <col />
            </colgroup>
            <thead>
            <tr>
                <th></th>
                <th>Voucher No.</th>
                <th>Vendor</th>
                <th>Debit Amount</th>
                <th>Credit Amount</th>
                <th>Description</th>
                <th>DepartMent Name</th>
                <th>Emp Name</th>
            </tr>
            </thead>
            <tbody>
            <tr>
                <td><input type="checkbox" class="" id="" value="" /></td>
                <td>EV13-0009001</td>
                <td>서승범</td>
                <td>1,000</td>
                <td>0</td>
                <td>교통비 톨비</td>
                <td>SBS</td>
                <td>서승범</td>
            </tr>
            <tr>
                <td><input type="checkbox" class="" id="Checkbox1" value="" /></td>
                <td>EV13-0009001</td>
                <td>서승범</td>
                <td>50,000</td>
                <td>0</td>
                <td>서울-대전 유류비 톨비</td>
                <td>SBS</td>
                <td>서승범</td>
            </tr>
            <tr>
                <td><input type="checkbox" class="" id="Checkbox2" value="" /></td>
                <td>EV13-0009001</td>
                <td>서승범</td>
                <td>0</td>
                <td>51,000</td>
                <td>대전 출장 비용</td>
                <td>SBS</td>
                <td>서승범</td>
            </tr>
            <tr>
                <td><input type="checkbox" class="" id="Checkbox3" value="" /></td>
                <td>EV13-0009002</td>
                <td>홍길동</td>
                <td>75,000</td>
                <td>0</td>
                <td>식사접대(먼디파마)</td>
                <td>경영지원팀</td>
                <td>홍길동</td>
            </tr>
            <tr>
                <td><input type="checkbox" class="" id="Checkbox4" value="" /></td>
                <td>EV13-0009002</td>
                <td>홍길동</td>
                <td>0</td>
                <td>75,000</td>
                <td>접대비</td>
                <td>경영지원팀</td>
                <td>홍길동</td>
            </tr>
            </tbody>
            </table>
            <!-- 테이블 끝 //-->

            <div style="width:1000px"></div>

        </div>

    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/ETC/Js/Customize/User/AddDetail.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/ETC/Js/Customize/User/AddDetail.js");
    </script>
</body>
</html>



