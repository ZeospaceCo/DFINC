<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddChange.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Coop.AddChange" %>
<%@ Register src="../Portal/Menu.ascx" tagname="Menu" tagprefix="uc1" %>
<%@ Register src="../Portal/Footer.ascx" tagname="Footer" tagprefix="uc2" %>
<!DOCTYPE html>
<html lang="ko">
<head>
<title></title>
<meta charset="uft8" />
<meta name="viewport" content="initial-scale=1.0; maximum-scale=1.0; minimum-scale=1.0; user-scalable=no;" />
<link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/mobile_main.css"/>
<link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/mobile_main_<%=SkinType%>.css"/>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js" ></script>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
<script type="text/javascript">
    var BPage = "<%=bPage%>", ListID = "<%=listID%>", GroupID = "<%=groupID%>";
    var PageNo = "<%=pageNo%>", MoreSize = "<%=moreSize%>";
    var CompanyID = "<%=CompanyID%>";
    var UserID = "<%=UserID%>";    
    var SearchValue = "<%=searchValue%>";
    var Status = "<%=status%>";
    var StartCalMonth = "<%=startCalMonth%>";
    var EndCalMonth = "<%=endCalMonth%>";
    var BtnClickCheck = ""; //	버튼 클릭여부 체크
    var CalCheck = "";
    var PreLang = "<%=PreLang %>";

    var R_ProcessingWait = "<%=GetResource("ProcessingWait") %>";     
    var R_ProcessingError = "<%= GetResource("ProcessingError") %>";
    var R_Menu = "<%=GetResource("Menu")%>";    
    var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>"; 
    var R_StartDateEndDiffer = "<%=GetResource("StartDateEndDiffer") %>"; 
    var R_Sun = "<%=GetResource("Sun") %>";     
    var R_Mon = "<%=GetResource("Mon") %>";     
    var R_Tue = "<%=GetResource("Tue") %>";     
    var R_Wed = "<%=GetResource("Wed") %>";     
    var R_Thu = "<%=GetResource("Thu") %>";     
    var R_Fri = "<%=GetResource("Fri") %>";     
    var R_Sat = "<%=GetResource("Sat") %>";   

</script>
<script type="text/javascript">
    JsCall("/DeskPlusMobile/Js/Coop/AddChange.js");
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divprg" align="center" class="loading"></div>
    <div data-role="page" id="CoopAdd">
    <div data-role="header">

    <div id="hd"><!-- hd -->
    <div class="tit"><a href="../Portal/Portal.aspx"><p><%=CompanyName%></p></a></div>
    <div class="hd_menu"><p class="hd_select"><%=GetResource("Menu")%> ▼</p></div>
    </div><!-- // hd -->

    <div class="to"><a href="#" class="mct"><font id="listTitle" class="my"><%=GetResource("CoopComplete")%></font></a>

    <p class="we"> 
    <span class="dy"><%=Today%></span> </p>
    </div>

    <uc1:Menu ID="Menu1" runat="server" />
    </div>    

    <!--   달력 -->
    <ul class="di2" id="coop_calendar" style="display:none;">
       <li>
             <div class="chh">
                <h1>
                   <a href="#" class="bt5 bt5pvm" title="이전달" id="pvMonth"><div><span class="hc2"><%=GetResource("BeforeMonth")%></span></div></a>                   
                   <span id="timeList"></span>                   
                   <a href="#" class="bt5 bt5nxm" title="다음달" id="nxMonth"><div><span class="hc2"><%=GetResource("NextMonth")%></span></div></a>
                </h1>
             </div>
       </li>
      <li>
       <div  class="mct" style="background:#C4C6E6;">
         <div id="phCal"></div> 
	   </div>
      </li>
    </ul>
    <!--  // 달력 -->

    <div data-role="content">

    <!-- content -->
		<div class="to3">
				<div class="pl">
			        <button type="button" class="bt5" id="btCancel"><span class="btnb"><%=GetResource("Cancel")%></span></button> 
			        <button type="button" class="bt5" id="btAdd"><span class="btnb"><%=GetResource("Save")%></span></button> 
				</div>
		</div>

        <div class="wr" >
            <dl class="wr1">
            <dt><label for="ds"><%=GetResource("StartDate")%></label></dt>
            <dd>&nbsp;&nbsp;&nbsp;&nbsp;<input type="text" id="tbStartDate" class="it1" runat="server" readonly  />
            </dd>
            </dl>
        </div>      
        <div class="wr">
            <dl class="wr1">
            <dt><label for="ds"><%=GetResource("EndDate")%></label></dt>
            <dd>&nbsp;&nbsp;&nbsp;&nbsp;<input type="text" id="tbEndDate" class="it1" runat="server" readonly/>
            </dd>
            </dl>
        </div>

         <div class="wr">
            <dl class="wr1">
            <dt><label for="ds"><%=GetResource("Difficulty")%></label></dt>
            <dd>&nbsp;&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddlDifficulty" runat="server"></asp:DropDownList>
            </dd>
            </dl>
        </div>

        <div class="wr">
            <dl class="wrc">
            <dt><label for="w2"></label></dt>
            <dd>
                <textarea id="memo" class="ta3"></textarea>
            </dd>
            </dl>
        </div>

    <!-- //content -->

    <div class="bnb">
    <p class="nb">
		<a class="w100" href="#" id="before"><%=GetResource("List")%></a>
    </p>
    </div>
    </div>
    </div>
    
    <uc2:footer ID="Footer1" runat="server" />
    </form>
</body>
</html>
