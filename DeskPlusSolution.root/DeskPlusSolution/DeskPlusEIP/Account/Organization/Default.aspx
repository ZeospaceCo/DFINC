<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Organization.Default" EnableViewState="false" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>조직도</title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
    <link rel="stylesheet" href="<%=ResolveUrl("~/Account/Resource/Organization.css")%>" />
    <script defer src="<%=ResolveUrl("~/Account/Js/Organization/Default.js")%>"></script>
</head>
<body>
    <main id="layout_body" class="org-page">
        <div class="layout_full">
        <header id="layout_header">
            <h1>조직도</h1>
            <div class="btn_right org-header-actions">
                <span class="org-summary">부서 <strong id="department-count">0</strong><span>구성원 <strong id="member-count">0</strong>명</span></span>
                <a class="btn btn-outline-light" href="<%=ResolveUrl("~/Account/Search/Default.aspx")%>">인사검색</a>
                <button class="btn btn-outline-light" type="button" id="refresh-chart">새로고침</button>
            </div>
        </header>
        <div class="org-content">
        <div class="org-toolbar">
            <form id="chart-search-form" class="org-search" role="search">
                <label class="org-sr-only" for="chart-search">부서 또는 구성원 검색</label>
                <input id="chart-search" class="searchtext form-control" type="search" placeholder="부서, 이름, 직위 검색" maxlength="100" autocomplete="off" />
                <button type="submit" class="searchbtn btn-secondary">검색</button>
                <button type="button" id="clear-search" class="btn btn-outline-light">초기화</button>
            </form>
            <div class="org-tree-tools">
                <button type="button" id="my-department" class="btn btn-outline-light">내 부서</button>
                <button type="button" id="expand-all" class="btn btn-outline-light">전체 펼치기</button>
                <button type="button" id="collapse-all" class="btn btn-outline-light">전체 접기</button>
                <div class="org-zoom" aria-label="조직도 배율">
                    <button type="button" id="zoom-out" class="btn btn-outline-light" aria-label="축소">−</button>
                    <output id="zoom-value" aria-live="polite">100%</output>
                    <button type="button" id="zoom-in" class="btn btn-outline-light" aria-label="확대">+</button>
                </div>
                <button type="button" id="fit-chart" class="btn btn-outline-light">화면 맞춤</button>
            </div>
        </div>
        <div class="org-guide">
            <p><span class="org-legend org-legend-root"></span>최상위 조직 <span class="org-legend org-legend-division"></span>본부 / 부문 <span class="org-legend org-legend-team"></span>하위 부서</p>
            <span>인원 수는 하위 부서와 겸직을 포함하며, 같은 구성원은 한 번만 계산합니다.</span>
        </div>
        <div class="org-search-result" id="search-result" hidden>
            <span id="search-status" role="status" aria-live="polite"></span>
            <div><button type="button" id="previous-result" class="btn btn-outline-light">이전 결과</button><button type="button" id="next-result" class="btn btn-outline-light">다음 결과</button></div>
        </div>
        <p id="chart-error" class="org-error" role="alert" hidden>조직도 정보를 불러오지 못했습니다. 잠시 후 새로고침해 주세요.</p>
        <noscript><p class="org-error">조직도를 보려면 브라우저의 JavaScript를 켜 주세요.</p></noscript>
        <div class="org-viewport" id="chart-viewport" role="region" aria-label="조직도. 스크롤하여 전체 부서를 볼 수 있습니다." tabindex="0">
            <div class="org-stage" id="chart-stage"><div class="org-canvas" id="chart-canvas"><ul class="org-roots" id="chart-tree" aria-label="조직 계층"></ul></div></div>
            <p id="chart-empty" class="org-empty" role="status" hidden>등록된 부서와 구성원이 없습니다.</p>
        </div>
        <footer class="org-footer"><span>부서명을 누르면 하위 조직이 접히거나 펼쳐집니다. 구성원을 누르면 상세 정보를 볼 수 있습니다.</span><span>활성 부서 · 재직 사용자 기준</span></footer>
        </div>
        </div>
    </main>
    <div id="member-dialog" class="org-dialog-backdrop" hidden>
        <section class="org-dialog" role="dialog" aria-modal="true" aria-labelledby="dialog-name" tabindex="-1">
            <button id="close-dialog" type="button" class="btn btn-outline-light org-dialog-close" aria-label="상세 정보 닫기">닫기</button>
            <p class="org-dialog-label">구성원 정보</p>
            <h2 id="dialog-name"></h2><div id="dialog-content"></div>
        </section>
    </div>
    <script id="organization-data" type="application/json"><%=ChartJson%></script>
</body>
</html>
