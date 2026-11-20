<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Education.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Education" %>

<style type="text/css">
    .portlet-education .content-tabs { padding-top: 8px; }
    .portlet-education .content-tabs > ul { margin: 0; }
    .portlet-education .content-tabs > ul > li { padding-top: 3px; padding-bottom: 3px; }
    .portlet-education .content-tabs > ul > li > a { display: flex; align-items: center; flex-wrap: wrap; }
    .portlet-education .content-tabs > ul > li > a > .education-portlet-status { display: inline-block; flex: 0 0 auto; height: auto; min-width: 40px; margin-right: 10px; padding: 3px 8px; border-radius: 2px; color: #fff; font-size: 12px; font-weight: bold; line-height: 1.2; text-align: center; }
    .portlet-education .education-portlet-status[data-status="1"] { background: #75ace9; }
    .portlet-education .education-portlet-status[data-status="2"] { background: #8bc63e; }
    .portlet-education .education-portlet-status[data-status="3"] { background: #343a40; }
    .portlet-education .education-portlet-status[data-status="4"] { background: #9e9080; }
    .portlet-education .content-tabs > ul > li > a > .education-portlet-title {
        flex: 1 1 3em;
        min-width: 3em;
        height: auto;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
    }
    .portlet-education .content-tabs > ul > li > a > .education-portlet-period {
        flex: 0 0 auto;
        height: auto;
        margin-left: auto;
        padding-left: 8px;
        font-size: 12px;
        text-align: right;
        white-space: nowrap;
    }
</style>

<script type="text/javascript">
    $(document).ready(function () {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 리스트행 개수
        var educationPageSize = parseInt("<%=PageSize %>", 10);
        if (educationPageSize != 5 && educationPageSize != 10 && educationPageSize != 20 && educationPageSize != 30) {
            educationPageSize = 5;
        }
        $("#hdnEducationPageSize_" + portletDom).val(educationPageSize);

        // 리스트 전체 카운트
        $("#hdnEducationTotalCount_" + portletDom).val("<%=EducationTotalCount %>");

        if ("<%=IsAuth%>" != "True") {
            $("#cbtnPDel_" + portletDom).remove();
        }

        //최소화 여부
        if ("<%=IsMinimize %>" == "1") {
            $("#ptSlide_" + portletDom).children('i').toggleClass('fa-chevron-up');
            $("#ptContent_" + portletDom).fadeOut("slow");
        }

        //포틀릿 최소화/최대화
        $("#ptSlide_" + portletDom).on("click", function (e) {
            e.preventDefault();
            if ($("#hdnTabSlide_" + portletDom).val() == "1") {
                //최대화
                Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "0");
                $("#ptContent_" + portletDom).fadeIn("slow");
                $("#hdnTabSlide_" + portletDom).val("0");
            }
            else {
                //최소화
                Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "1");
                $("#ptContent_" + portletDom).fadeOut("slow");
                $("#hdnTabSlide_" + portletDom).val("1");
            }
        });


        //포틀릿 설정
        $("#ptConfig_" + portletDom).on("click", function (e) {
            e.preventDefault();
            // 리스트 행 개수 바인딩
            $("#educationCount_" + portletDom).val($("#hdnEducationPageSize_" + portletDom).val());
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

        //새로고침
        $("#ptReload_" + portletDom).on("click", function (e) {

            e.preventDefault();

            var loadingPath = "<img src='<%=BaseImgPath%>/Icon/loading.gif' width='16' height='16' />";
            $(this).html(loadingPath);

            setTimeout(function () {

                // 첫 페이지로 세팅
                $("#hdnEducationCount_" + portletDom).val("1");
                LoadEducation_<%=TabPortletID%>_<%=PortletID%>();

                var loadingPath = "<img src='<%=BaseImgPath%>/Main/ico_ref.gif' />";
                $("#ptReload_" + portletDom).html(loadingPath);

            }, 1);
        });

        //상세바로가기
        $("#ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();
            parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=education";
        });

        // 환경설정 저장
        $("#cbtnConfirm_" + portletDom).click(function (e) {
            e.preventDefault();

            // 리스트 행 개수 설정
            educationPageSize = parseInt($("#educationCount_" + portletDom).val(), 10);
            if (educationPageSize != 5 && educationPageSize != 10 && educationPageSize != 20 && educationPageSize != 30) {
                return;
            }
            $("#hdnEducationPageSize_" + portletDom).val(educationPageSize);

            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", educationPageSize);

            // 첫 페이지로 세팅
            $("#hdnEducationCount_" + portletDom).val("1");
            LoadEducation_<%=TabPortletID%>_<%=PortletID%>();

            $("#ptListConfig_" + portletDom).slideToggle(600);
        });

        // 환경설정 닫기
        $("#cbtnClose_" + portletDom).click(function (e) {
            e.preventDefault();
            $("#ptListConfig_" + portletDom).slideToggle(600);
        });      
       
        $("#hdnEducationCount_" + portletDom).val("1");

        Education_SetPaging_<%=TabPortletID%>_<%=PortletID%>();
        LoadEducation_<%=TabPortletID%>_<%=PortletID%>();

    });

    /*****************************************************/
    function LoadEducation_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        var loaded = false;
        
        // 현재 사용자의 교육 목록
        var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Education.asmx/GetList";

        ParamArray = {
            'pageSize': $("#hdnEducationPageSize_" + portletDom).val(),      // 리스트 행 개수
            'pageSizeNo': $("#hdnEducationCount_" + portletDom).val()       // 페이지 번호
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
                var result = data ? data.d : null;
                if (!result || !$.isArray(result.List)) {
                    alert(R_ErrorConnection);
                    return;
                }

                var totalCount = parseInt(result.TotalCount, 10);
                var pageNo = parseInt(result.PageNo, 10);
                var pageSize = parseInt(result.PageSize, 10);
                if (isNaN(totalCount) || totalCount < 0 || isNaN(pageNo) || pageNo < 1
                    || (pageSize != 5 && pageSize != 10 && pageSize != 20 && pageSize != 30)) {
                    alert(R_ErrorConnection);
                    return;
                }

                var content = $('#phEducation_' + portletDom);
                content.setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/JTEducation.htm?seq=" + Guids(), null, { filter_data: false });
                content.processTemplate(result.List);
                content.find(".NoListData").text(R_NoContent);

                // 건수와 페이지는 서버에서 다시 조회한 값으로 갱신합니다.
                $("#hdnEducationTotalCount_" + portletDom).val(totalCount);
                $("#hdnEducationCount_" + portletDom).val(pageNo);
                $("#hdnEducationPageSize_" + portletDom).val(pageSize);
                Education_SetPaging_<%=TabPortletID%>_<%=PortletID%>();
                loaded = true;
            }
            ,
            error: function (data, status, err) {
                alert(R_ErrorConnection);
            }
        });
        return loaded;
    }

    /****************************************************/
    function Education_SetPaging_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        var pageSize = parseInt($("#hdnEducationPageSize_" + portletDom).val(), 10) || 5;
        var totalCount = Math.max(0, parseInt($("#hdnEducationTotalCount_" + portletDom).val(), 10) || 0);
        var maxPage = Math.max(1, Math.ceil(totalCount / pageSize));
        var pageNo = Math.min(maxPage, Math.max(1, parseInt($("#hdnEducationCount_" + portletDom).val(), 10) || 1));
        $("#hdnEducationCount_" + portletDom).val(pageNo);

        var before = $("#educationBefore_" + portletDom);
        var next = $("#educationNext_" + portletDom);
        // 다른 포틀릿처럼 버튼은 클릭 가능하게 두고 경계에서는 안내합니다.
        before.parent().toggleClass("disabled", false);
        next.parent().toggleClass("disabled", false);
        before.attr("aria-disabled", "false").attr("tabindex", "0");
        next.attr("aria-disabled", "false").attr("tabindex", "0");
    }

    /****************************************************/
    function Education_Change_<%=TabPortletID%>_<%=PortletID%>(arg) {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        Education_SetPaging_<%=TabPortletID%>_<%=PortletID%>();

        var pageSize = parseInt($("#hdnEducationPageSize_" + portletDom).val(), 10) || 5;
        var pageNo = parseInt($("#hdnEducationCount_" + portletDom).val(), 10) || 1;
        var totalCount = Math.max(0, parseInt($("#hdnEducationTotalCount_" + portletDom).val(), 10) || 0);
        var maxPage = Math.max(1, Math.ceil(totalCount / pageSize));
        var nextPage = pageNo;
        if (arg == "next") nextPage++;
        else if (arg == "before") nextPage--;
        else return false;

        // 0건도 한 페이지로 취급하여 공통 첫/마지막 페이지 안내를 표시합니다.
        if (nextPage < 1) {
            alert(R_FirstPage);
            return false;
        }
        if (nextPage > maxPage) {
            alert(R_LastPage);
            return false;
        }

        $("#hdnEducationCount_" + portletDom).val(nextPage);
        if (!LoadEducation_<%=TabPortletID%>_<%=PortletID%>()) {
            $("#hdnEducationCount_" + portletDom).val(pageNo);
            Education_SetPaging_<%=TabPortletID%>_<%=PortletID%>();
        }
        return false;
    }

    /***************************************************/
    function ViewEducation(p_listID) {
        if (!/^\d+$/.test(String(p_listID)) || parseInt(p_listID, 10) <= 0) return false;

        ParamArray = {
            'listID': p_listID
        };

        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Education/Content/ViewPopupTotal.aspx?p=" + GetEncryptValue;
        var popupWidth = Math.min(1200, Math.max(320, screen.availWidth - 40));
        var popupHeight = Math.min(800, Math.max(240, screen.availHeight - 100));
        PopupWinOpen(url, "", "ViewEducation", popupWidth, popupHeight, 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
        return false;
    }

</script>

<!-- 포틀릿(리스트:일반형) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-notice portlet-education">    

       <div class="portlet-title portlet-header">
            <!-- 상단 타이틀 //-->
            <div class="caption">
                <span class="caption-subject text-uppercase"><a class="ptReload text-primary" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%>&nbsp;&nbsp;<i class="fas fa-chevron-right"></i></a></span>
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
                <div class="col col-lg portlet-setting-select">
                    <div class="form-group">
                        <select id="educationCount_<%=TabPortletID%>_<%=PortletID%>" class="form-control form-control-sm">
                            <option title="5" value="5">5 <%=GetResource("ListRowCnt")%></option>
                            <option title="10" value="10">10 <%=GetResource("ListRowCnt")%></option>
                            <option title="20" value="20">20 <%=GetResource("ListRowCnt")%></option>
                            <option title="30" value="30">30 <%=GetResource("ListRowCnt")%></option>
                        </select>
                    </div>
                </div>
                <div class="col-md-auto justify-content-end">                    
                    <span>
                        <a class="btn btn-sm btn-outline-light" id="cbtnPDel_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("DeletePortlet")%></span></a>
                    </span>
                    <span>
                        <a class="btn btn-sm btn-outline-light" id="cbtnConfirm_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("Save")%></span></a>
                    </span>
                </div>    
            </div>
        </div>

        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-body collapse show">           
            <div class="portlet-contents">

                <!-- 내용 //-->
                <div class="content-tabs" id="phEducation_<%=TabPortletID%>_<%=PortletID%>">    
                </div>
                <!-- 이전/이후 //-->
                <nav aria-label="Page navigation example">
                    <ul class="pagination pagination-sm justify-content ptl_more">
                    <li class="page-item"><a href="#" id="educationBefore_<%=TabPortletID%>_<%=PortletID%>" class="page-link" onclick="return Education_Change_<%=TabPortletID%>_<%=PortletID%>('before')"><i class="fas fa-chevron-left"></i></a></li>
                    <li class="page-item"><a href="#" id="educationNext_<%=TabPortletID%>_<%=PortletID%>" class="page-link" onclick="return Education_Change_<%=TabPortletID%>_<%=PortletID%>('next')"><i class="fas fa-chevron-right"></i></a></li>
                    </ul>
                </nav>
            </div>
        </div>   

        <div style="display: none">
            <input type="hidden" id="hdnEducationTotalCount_<%=TabPortletID%>_<%=PortletID%>"  name="hdnEducationTotalCount_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnEducationPageSize_<%=TabPortletID%>_<%=PortletID%>" name="hdnEducationPageSize_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnEducationCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnEducationCount_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
        </div>

</div>
<!-- 포틀릿(리스트:일반형) End //-->
