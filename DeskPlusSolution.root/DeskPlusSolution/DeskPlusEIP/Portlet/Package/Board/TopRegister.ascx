<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopRegister.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Board.TopRegister" %>
<script type="text/javascript">
    var BoardList_Count = 1;
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";

    var R_R1 = "<%=GetResource("R1")%>";
    var R_R2 = "<%=GetResource("R2")%>";
    var R_R3 = "<%=GetResource("R3")%>";

    $(document).ready(function () {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 리스트행 개수
        $("#hdnBoardListPageSize_" + portletDom).val("<%=PageSize %>");
        var boardList_PageSize = $("#hdnBoardListPageSize_" + portletDom).val();        

        if ("<%=IsAuth%>" != "True") {
            $("#ptConfig_" + portletDom).remove();  
        }

        //최소화 여부
        if ("<%=IsMinimize %>" == "1") {
            $("#ptSlide_" + portletDom).children('i').toggleClass('fa-chevron-up');
            $("#ptContent_" + portletDom).fadeOut("slow"); 
        }
        
          //포틀릿 최소화/최대화
        $("#ptSlide_" + portletDom).on("click", function (e) {
            if($("#hdnTabSlide_" + portletDom).val() == "1")
            {
                //최대화
                Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "0");
                $("#ptContent_" + portletDom).fadeIn("slow");     
                $("#hdnTabSlide_" + portletDom).val("0"); 
            }
            else{
                //최소화
                Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "1");
                $("#ptContent_" + portletDom).fadeOut("slow");    
                $("#hdnTabSlide_" + portletDom).val("1");
            }            
        });

        
        //포틀릿 설정
        $("#ptConfig_" + portletDom).on("click", function (e) {
            // 리스트 행 개수 바인딩
            $("#boardListCount_" + portletDom).val($("#hdnBoardListPageSize_" + portletDom).val());
            $("#ptListConfig_" + portletDom).slideToggle(600);      
         });
        
        // 포틀릿 삭제
        $("#cbtnPDel_" + portletDom).click(function (e) {
            e.preventDefault();

            Main.DelTabPortlet(portletDom);
            if (PortletAuth == "True") {
                $("#portlet_" + portletDom).fadeOut("slow");
            }
        });

        /*
        //새로고침
        $("#ptReload_" + portletDom).on("click", function (e) {

            e.preventDefault();

            var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
            $(this).html(loadingPath);

            setTimeout(function () {

                // 첫 페이지로 세팅
                $("#hdnBoardListCount_" + portletDom).val("1");
                LoadBoardList_<%=TabPortletID%>_<%=PortletID%>();

                var loadingPath = "<img src='<%=BaseImgPath%>/Main/ico_ref.gif' />";
                $("#ptReload_" + portletDom).html(loadingPath);

            }, 1);            
        });
        */

        // 환경설정 저장
        $("#cbtnConfirm_" + portletDom).click(function (e) {
            e.preventDefault();

            // 리스트 행 개수 설정
            boardList_PageSize = $("#boardListCount_" + portletDom).val();
            $("#hdnBoardListPageSize_" + portletDom).val(boardList_PageSize);

            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", boardList_PageSize, "");

            // 첫 페이지로 세팅
            $("#hdnBoardListCount_" + portletDom).val("1");
            LoadBoardList_<%=TabPortletID%>_<%=PortletID%>();

            $("#ptListConfig_" + portletDom).slideToggle(600);
        });

        // 환경설정 닫기
        $("#cbtnClose_" + portletDom).click(function (e) {
            e.preventDefault();
            $("#ptListConfig_" + portletDom).slideToggle(600);
        });        

       // $("#ptListConfig_" + portletDom).hide();

        $("#hdnBoardListCount_" + portletDom).val("1");

        //setTimeout(function () {
            LoadBoardList_<%=TabPortletID%>_<%=PortletID%>();
        //}, 1);

    });

    /**************************************************
    작성자: freeman
    작성일:2012-02-07
    작성목적 : 페이지 가져오기
    ***************************************************/
    function LoadBoardList_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        //게시판 리스트
        var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Board/List.asmx/GetBoardTopRegister";

        ParamArray = {

        };

        CallEncrypt(CallGetParam());
        var paramData = "{p:'" + GetEncryptValue + "'}";

        $.ajax({
            type: "POST",
            url: urlPage,
            async: false,
            data: paramData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#plList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/Board/JTTopRegister.htm?seq=" + Guids(), null, { filter_data: false });
                $('#plList_' + portletDom).processTemplate(data.d);
                if($(".NoListData") != undefined) {$(".NoListData").text(R_NoContent);}
                if($(".TitleIcon") != undefined) {$(".TitleIcon").attr('src', ImagePath + '/main/ico_bullet2.gif');}
                
                if($(".r1") != undefined) {$(".r1").text(R_R1);}
                if($(".r2") != undefined) {$(".r2").text(R_R2);}
                if($(".r3") != undefined) {$(".r3").text(R_R3);}
            }
            ,
            error: function (data, status, err) {
                alert(R_ErrorConnection + data);
            }
        });

    }
    
</script>

<!-- 포틀릿(리스트:일반형) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">  

    <div class="portlet-title portlet-header">
        <!-- 상단 타이틀 //-->
        <div class="caption">
            <span class="caption-subject text-uppercase"><a class="ptReload text-primary" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%></a></span>
            <span class="caption-helper"></span>
        </div>
        <div class="actions">
            <a href="" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>" data-toggle="collapse" aria-expanded="false" class="btn portlet-setting-icon"><i class="fas fa-cog"></i></a>
            <a href="" id="ptSlide_<%=TabPortletID%>_<%=PortletID%>" data-toggle="collapse" aria-expanded="false" class="btn portlet-roll-icon"><i class="fas fa-chevron-down"></i></a>
        </div>
    </div>

    <div id="ptListConfig_<%=TabPortletID%>_<%=PortletID%>" class="portlet-setting" style="display:none;">
        <!-- 환경설정 //-->             
        <div class="row">
            <div class="col-md-auto justify-content-end">                    
                <span>
                    <a class="btn btn-sm btn-outline-light" id="cbtnPDel_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("DeletePortlet")%></span></a>
                </span>
            </div>    
        </div>
    </div>

     <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-body collapse show">
        <div class="portlet-contents">
            <!-- 탭 //-->
            <div class="nav-tabs">                      
            </div>
            <!-- 내용 //-->
            <div class="content-tabs" id="plList_<%=TabPortletID%>_<%=PortletID%>">
                <div style="padding-top:50px;text-align:center;">
                    <img src="<%=BaseImgPath%>/pims/loading.gif" alt="Loading..." title="Loading..." />
                </div>
            </div>
        </div>
    </div>     


    <div style="display: none">
        <input type="hidden" id="hdnBoardListCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardListCount_<%=TabPortletID%>_<%=PortletID%>" />
        <input type="hidden" id="hdnBoardListCategoryID_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardListCategoryID_<%=TabPortletID%>_<%=PortletID%>" />
        <input type="hidden" id="hdnBoardListTotalCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardListTotalCount_<%=TabPortletID%>_<%=PortletID%>" />
        <input type="hidden" id="hdnBoardListPageSize_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardListPageSize_<%=TabPortletID%>_<%=PortletID%>" />
        <input type="hidden" id="hdnBoardListParamValue_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardListParamValue_<%=TabPortletID%>_<%=PortletID%>" value="<%=ParamValue %>" />
        <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
    </div>
 
</div>
<!-- 포틀릿(리스트:일반형) End //-->