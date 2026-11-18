<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sheet.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Organization.Sheet" EnableViewState="false" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>표형 조직도</title>
    <link rel="stylesheet" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" href="<%=ResolveUrl("~/Account/Resource/OrganizationSheet.css")%>?v=20261117-upload" />
    <script defer src="<%=ResolveUrl("~/Account/Js/Organization/SheetModel.js")%>?v=20261117-upload"></script>
    <script defer src="<%=ResolveUrl("~/Account/Js/Organization/Sheet.js")%>?v=20261117-upload"></script>
</head>
<body>
    <main id="layout_body" class="sheet-page">
        <div class="layout_full">
            <header id="layout_header">
                <h1>표형 조직도</h1>
                <div class="btn_right">
                    <button id="sheet-upload" class="btn btn-secondary" type="button" hidden>엑셀 업로드</button>
                    <input id="sheet-workbook" type="file" accept=".xlsx,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" hidden />
                </div>
            </header>
            <div class="sheet-content">
                <p id="sheet-upload-help" class="sheet-upload-help" hidden>엑셀에서 조직도를 수정한 뒤 .xlsx 파일을 업로드해 주세요. 업로드하면 모든 사용자에게 바로 반영됩니다.</p>
                <div class="sheet-toolbar">
                    <div><span id="sheet-state">불러오는 중</span><span id="sheet-source"></span><span id="sheet-modified"></span></div>
                    <div class="sheet-view-tools">
                        <button id="sheet-zoom-out" class="btn btn-outline-light" type="button" aria-label="축소">−</button>
                        <output id="sheet-zoom-value">100%</output>
                        <button id="sheet-zoom-in" class="btn btn-outline-light" type="button" aria-label="확대">+</button>
                        <button id="sheet-fit" class="btn btn-outline-light" type="button">가로 맞춤</button>
                        <button id="sheet-print" class="btn btn-outline-light" type="button">인쇄</button>
                    </div>
                </div>
                <p id="sheet-message" class="sheet-message" role="status" aria-live="polite" hidden></p>
                <noscript><p class="sheet-message">조직도를 보려면 JavaScript를 켜 주세요.</p></noscript>
                <div id="sheet-viewport" class="sheet-viewport" role="region" tabindex="0" aria-label="표형 조직도">
                    <div id="sheet-stage" class="sheet-stage"><div id="sheet-canvas" class="sheet-canvas"></div></div>
                </div>
            </div>
        </div>
    </main>
    <script id="sheet-data" type="application/json"><%=SheetJson%></script>
</body>
</html>
