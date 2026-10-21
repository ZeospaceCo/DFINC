
$(function () {

    $(document).ready(function () {

        // [회사 - 추가/수정/삭제/▲/▼]
        $('#cbtnAdd1').click(function (e) { e.preventDefault(); decisionAdd(1); });
        $('#cbtnMod1').click(function (e) { e.preventDefault(); decisionMod(1); });
        $('#cbtnDel1').click(function (e) { e.preventDefault(); decisionDel(1); });
        $('#cbtnUp1').click(function (e) { e.preventDefault(); decisionUp(1); });
        $('#cbtnDown1').click(function (e) { e.preventDefault(); decisionDown(1); });

        // [대분류 - 추가/수정/삭제/▲/▼]
        $('#cbtnAdd2').click(function (e) { e.preventDefault(); decisionAdd(2); });
        $('#cbtnMod2').click(function (e) { e.preventDefault(); decisionMod(2); });
        $('#cbtnDel2').click(function (e) { e.preventDefault(); decisionDel(2); });
        $('#cbtnUp2').click(function (e) { e.preventDefault(); decisionUp(2); });
        $('#cbtnDown2').click(function (e) { e.preventDefault(); decisionDown(2); });

        // [중분류 - 추가/수정/삭제/▲/▼]
        $('#cbtnAdd3').click(function (e) { e.preventDefault(); decisionAdd(3); });
        $('#cbtnMod3').click(function (e) { e.preventDefault(); decisionMod(3); });
        $('#cbtnDel3').click(function (e) { e.preventDefault(); decisionDel(3); });
        $('#cbtnUp3').click(function (e) { e.preventDefault(); decisionUp(3); });
        $('#cbtnDown3').click(function (e) { e.preventDefault(); decisionDown(3); });

        // [소분류&전결사항 - 추가/수정/삭제/▲/▼/◀/▶]
        $('#cbtnAdd4').click(function (e) { e.preventDefault(); decisionAdd(4); });
        $('#cbtnMod4').click(function (e) { e.preventDefault(); decisionMod(4); });
        $('#cbtnDel4').click(function (e) { e.preventDefault(); decisionDel(4); });
        $('#cbtnUp4').click(function (e) { e.preventDefault(); decisionUp(4); });
        $('#cbtnDown4').click(function (e) { e.preventDefault(); decisionDown(4); });
        $('#cbtnLeft').click(function (e) { e.preventDefault(); decisionLeft(); });
        $('#cbtnRight').click(function (e) { e.preventDefault(); decisionRight(); });

        // [전결권자 - 추가/수정/삭제/▲/▼]
        $('#cbtnAdd5').click(function (e) { e.preventDefault(); targetAdd(); });
        $('#cbtnMod5').click(function (e) { e.preventDefault(); targetMod(); });
        $('#cbtnDel5').click(function (e) { e.preventDefault(); targetDel(); });
        $('#cbtnUp5').click(function (e) { e.preventDefault(); targetUp(); });
        $('#cbtnDown5').click(function (e) { e.preventDefault(); targetDown(); });

        //사업부 초기데이터 Load
        loadCategory();
        //전결권자 초기데이터 Load
        loadTarget();

    });
});

function loadCategory() {
    //사업부 초기데이터 Load
    var selectCtl = document.all("categoryID1");
    var objOptionItem = document.createElement("option");

    var selectCtlLen = selectCtl.length
    for (i = 0; i <= selectCtlLen; i++) {
        selectCtl.remove(0);
    }

    arrCategoryID = categoryID.split("∫");
    arrCategoryName = categoryName.split("∫");
    arrCourse = course.split("∫");
    arrRemark = remark.split("∫");

    for (i = 0; i < arrCategoryID.length - 1; i++) {
        var selectCtl = document.all("categoryID1");
        var objOptionItem = document.createElement("option");

        objOptionItem.value = arrCategoryID[i];
        objOptionItem.text = arrCategoryName[i];
        objOptionItem.course = arrCourse[i];
        objOptionItem.remark = arrRemark[i];

        selectCtl.add(objOptionItem, selectCtl.length + 1);
    }
}

function loadTarget() {
    //전결권자 초기데이터 load
    var selectCtl = document.all("targetID");
    var objOptionItem = document.createElement("option");

    var selectCtlLen = selectCtl.length
    for (k = 0; k <= selectCtlLen; k++) {
        selectCtl.remove(0);
    }

    var selectCtl = document.all("targetID");
    var objOptionItem = document.createElement("option");

    objOptionItem.value = "0";
    objOptionItem.text = "규정없음";

    selectCtl.add(objOptionItem, selectCtl.length + 1);


    arrTargetID = targetID.split("∫");
    arrTargetName = targetName.split("∫");

    for (i = 0; i < arrTargetID.length - 1; i++) {
        var selectCtl = document.all("targetID");
        var objOptionItem = document.createElement("option");

        objOptionItem.value = arrTargetID[i];
        objOptionItem.text = arrTargetName[i];

        selectCtl.add(objOptionItem, selectCtl.length + 1);
    }
}


/*** 회사/대분류/중분류/소분류 & 전결사항 ********************************************************************/

// [추가]
function decisionAdd(gubun) {
    document.all("categoryGubun").value = gubun;

    switch (gubun) {
        case 1:
            //사업부 추가
            if (document.all("categoryName1").value == "") {
                alert("사업부를 입력하세요.");
                return;
            }

            parentCategoryID = 0;
            categoryName = document.all("categoryName1").value;
            course = document.all("categoryCourse1").value;
            targetID = 0;
            remark = document.all("remark").value;
            selIdx = document.all("categoryID1").selectedIndex;

            break;

        case 2:
            //대분류 추가
            if (document.all("categoryID1").value == "") {
                alert("사업부를 선택하세요.");
                return;
            }

            if (document.all("categoryName2").value == "") {
                alert("대분류를 입력하세요.");
                return;
            }

            parentCategoryID = document.all("categoryID1").value;
            categoryName = document.all("categoryName2").value;
            course = document.all("categoryCourse2").value;
            targetID = 0;
            remark = document.all("remark").value;
            selIdx = document.all("categoryID2").selectedIndex;

            break;

        case 3:
            //중분류 추가
            if (document.all("categoryID1").value == "") {
                alert("사업부를 선택하세요.");
                return;
            }

            if (document.all("categoryID2").value == "") {
                alert("대분류를 선택하세요.");
                return;
            }

            if (document.all("categoryName3").value == "") {
                alert("중분류를 입력하세요.");
                return;
            }

            parentCategoryID = document.all("categoryID2").value;
            categoryName = document.all("categoryName3").value;
            course = document.all("categoryCourse3").value;
            targetID = 0;
            remark = document.all("remark").value;
            selIdx = document.all("categoryID3").selectedIndex;

            break;

        case 4:
            //소분류 & 전결사항 추가
            if (document.all("categoryName4").value == "") {
                alert("소분류 & 전결사항을 입력하세요.");
                return;
            }

            if (document.all("categoryID1").value == "") {
                alert("사업부를 선택하세요.");
                return;
            }

            if (document.all("categoryID2").value == "") {
                alert("대분류를 선택하세요.");
                return;
            }

            if (document.all("categoryID3").value == "") {
                alert("중분류를 선택하세요.");
                return;
            }

            if (document.all("targetID").value == "") {
                alert("전결권자를 선택하세요.");
                return;
            }

            parentCategoryID = document.all("categoryID3").value;
            categoryName = document.all("categoryName4").value;
            course = document.all("categoryCourse4").value;
            targetID = document.all("targetID").value;
            remark = document.all("remark").value;
            selIdx = document.all("categoryID4").selectedIndex;

            break;

    }
    document.all("selIdx").value = selIdx;

    rtnVal = decisionListAdd(parentCategoryID, categoryName.replace(/&/g, "＆"), course.replace(/&/g, "＆"), targetID, remark.replace(/&/g, "＆"), selIdx, companyID);

    arrRtnVal = rtnVal.split("∬");

    var selectCtl = document.all("categoryID" + gubun);
    var objOptionItem = document.createElement("option");

    if (selIdx == "-1") selIdx = selectCtl.length;

    objOptionItem.value = arrRtnVal[0];
    objOptionItem.text = arrRtnVal[1].replace(/&/g, "＆");
    objOptionItem.course = arrRtnVal[2].replace(/&/g, "＆");
    objOptionItem.remark = arrRtnVal[3].replace(/&/g, "＆");

    selectCtl.add(objOptionItem, parseInt(selIdx) + 1);
    selectCtl.value = "";
    alert("추가되었습니다.");
}

// [수정]
function decisionMod(gubun) {
    document.all("categoryGubun").value = gubun;

    switch (gubun) {
        case 1:
            //사업부 수정
            if (document.all("categoryID1").value == "") {
                alert("사업부를 선택하세요.");
                return;
            }

            if (document.all("categoryName1").value == "") {
                alert("사업부를 입력하세요.");
                return;
            }

            categoryID = document.all("categoryID1").value;
            categoryName = document.all("categoryName1").value;
            course = document.all("categoryCourse1").value;
            targetID = 0;
            remark = document.all("remark").value;

            selIdx = document.all("categoryID1").selectedIndex;

            break;

        case 2:
            //대분류 수정
            if (document.all("categoryID1").value == "") {
                alert("사업부를 선택하세요.");
                return;
            }

            if (document.all("categoryID2").value == "") {
                alert("대분류를 선택하세요.");
                return;
            }

            if (document.all("categoryName2").value == "") {
                alert("대분류를 입력하세요.");
                return;
            }

            categoryID = document.all("categoryID2").value;
            categoryName = document.all("categoryName2").value;
            course = document.all("categoryCourse2").value;
            targetID = 0;
            remark = document.all("remark").value;

            selIdx = document.all("categoryID2").selectedIndex;

            break;

        case 3:
            //중분류 수정
            if (document.all("categoryID1").value == "") {
                alert("사업부를 선택하세요.");
                return;
            }

            if (document.all("categoryID2").value == "") {
                alert("대분류를 선택하세요.");
                return;
            }

            if (document.all("categoryID3").value == "") {
                alert("중분류를 선택하세요.");
                return;
            }

            if (document.all("categoryName3").value == "") {
                alert("중분류를 입력하세요.");
                return;
            }

            categoryID = document.all("categoryID3").value;
            categoryName = document.all("categoryName3").value;
            course = document.all("categoryCourse3").value;
            targetID = 0;
            remark = document.all("remark").value;

            selIdx = document.all("categoryID3").selectedIndex;

            break;

        case 4:
            //소분류 & 전결사항 수정
            if (document.all("categoryID1").value == "") {
                alert("사업부를 선택하세요.");
                return;
            }

            if (document.all("categoryID2").value == "") {
                alert("대분류를 선택하세요.");
                return;
            }

            if (document.all("categoryID3").value == "") {
                alert("중분류를 선택하세요.");
                return;
            }

            if (document.all("targetID").value == "") {
                alert("전결권자를 선택하세요.");
                return;
            }

            if (document.all("categoryName3").value == "") {
                alert("소분류 & 전결사항을 입력하세요.");
                return;
            }

            categoryID = document.all("categoryID4").value;
            categoryName = document.all("categoryName4").value;
            course = document.all("categoryCourse4").value;
            targetID = document.all("targetID").value;
            remark = document.all("remark").value;

            selIdx = document.all("categoryID4").selectedIndex;

            break;

    }

    rtnVal = decisionListMod(categoryID, categoryName.replace(/&/g, "＆"), course.replace(/&/g, "＆"), targetID, remark.replace(/&/g, "＆"), companyID);
    document.all("categoryID" + gubun).options[selIdx].text = categoryName.replace(/&/g, "＆");
    document.all("categoryID" + gubun).options[selIdx].course = course.replace(/&/g, "＆");
    document.all("categoryID" + gubun).options[selIdx].remark = remark.replace(/&/g, "＆");
    alert("수정되었습니다.");
}

// [삭제]
function decisionDel(gubun) {
    document.all("categoryGubun").value = gubun;

    switch (gubun) {
        case 1:
            if (document.all("categoryID1").value == "") {
                alert("삭제 하실 사업부를 선택하세요.");
                return;
            }

            if (document.all("categoryID2").length != 0) {
                alert("대분류가 존재 합니다. 대분류를 삭제해야 사업부를 삭제하실 수 있습니다.");
                return;
            }
            parentCategoryID = 0;
            categoryID = document.all("categoryID1").value;

            break;

        case 2:
            if (document.all("categoryID2").value == "") {
                alert("삭제 하실 대분류를 선택하세요.");
                return;
            }

            if (document.all("categoryID3").length != 0) {
                alert("중분류가 존재 합니다. 중분류를 삭제해야 대분류를 삭제하실 수 있습니다.");
                return;
            }
            parentCategoryID = document.all("categoryID1").value;
            categoryID = document.all("categoryID2").value;

            break;

        case 3:
            if (document.all("categoryID3").value == "") {
                alert("삭제 하실 중분류를 선택하세요.");
                return;
            }

            if (document.all("categoryID4").length != 0) {
                alert("소분류 & 전결사항이 존재 합니다. 소분류 & 전결사항을 삭제해야 중분류를 삭제하실 수 있습니다.");
                return;
            }
            parentCategoryID = document.all("categoryID2").value;
            categoryID = document.all("categoryID3").value;

            break;

        case 4:
            if (document.all("categoryID4").value == "") {
                alert("삭제 하실 소분류 & 전결사항을 선택하세요.");
                return;
            }
            parentCategoryID = document.all("categoryID3").value;
            categoryID = document.all("categoryID4").value;

            break;

    }
    rtnVal = decisionListDel(parentCategoryID, categoryID, companyID);

    var selectCtl = document.all("categoryID" + gubun);
    var select_index = selectCtl.selectedIndex;

    selectCtl.remove(select_index);
    alert("삭제되었습니다.");
}

//전결권자 추가
function targetAdd() {
    //전결권자 추가
    if (document.all("targetName").value == "") {
        alert("전결권자를 입력하세요.");
        return;
    }

    targetName = document.all("targetName").value;

    rtnVal = decisionTargetAdd(targetName.replace(/&/g, "＆"), companyID);
    arrTarget = rtnVal.split("∬");


    var selectCtl = document.all("targetID");
    var objOptionItem = document.createElement("option");

    objOptionItem.value = arrTarget[0];
    objOptionItem.text = arrTarget[1];

    selectCtl.add(objOptionItem, selectCtl.length + 1);

    alert("추가되었습니다.");
}

//전결권자 수정
function targetMod() {
    //전결권자 수정
    if (document.all("targetID").value == "") {
        alert("전결권자를 선택하세요.");
        return;
    }

    if (document.all("targetName").value == "") {
        alert("전결권자를 입력하세요.");
        return;
    }

    targetID = document.all("targetID").value;
    targetName = document.all("targetName").value;

    rtnVAL = decisionTargetMod(targetID, targetName.replace(/&/g, "＆"), companyID);
    document.all("targetID").options[document.all("targetID").selectedIndex].text = rtnVAL;
    alert("수정되었습니다.");

}

//전결권자 삭제
function targetDel() {
    //전결권자 삭제
    if (document.all("targetID").value == "") {
        alert("전결권자를 선택하세요.");
        return;
    }

    targetID = document.all("targetID").value;

    rtnVal = decisionTargetDel(targetID, companyID);

    var selectCtl = document.all("targetID");
    var select_index = selectCtl.selectedIndex;

    selectCtl.remove(select_index);

    alert("삭제되었습니다.");

}

// 리스트 선택
function selectItem(gubun) {

    parentCategoryID = 0;
    switch (gubun) {
        case 1:
            //사업부 선택
            if (document.all("categoryID1").value != "") {
                document.all("categoryName1").value = document.all("categoryID1").options[document.all("categoryID1").selectedIndex].text;
                document.all("categoryCourse1").value = document.all("categoryID1").options[document.all("categoryID1").selectedIndex].course;

                document.all("categoryName2").value = "";
                document.all("categoryName3").value = "";
                document.all("categoryName4").value = "";

                document.all("categoryCourse2").value = "";
                document.all("categoryCourse3").value = "";
                document.all("categoryCourse4").value = "";

                document.all("targetID").value = "";

                document.all("remark").value = document.all("categoryID1").options[document.all("categoryID1").selectedIndex].remark;

                removeItem(3);
                removeItem(4);
                parentCategoryID = document.all("categoryID1").value;
            }
            break;

        case 2:
            //대분류 선택
            if (document.all("categoryID2").value != "") {
                document.all("categoryName2").value = document.all("categoryID2").options[document.all("categoryID2").selectedIndex].text;
                document.all("categoryCourse2").value = document.all("categoryID2").options[document.all("categoryID2").selectedIndex].course;

                document.all("categoryName3").value = "";
                document.all("categoryName4").value = "";

                document.all("categoryCourse3").value = "";
                document.all("categoryCourse4").value = "";

                document.all("targetID").value = "";

                document.all("remark").value = document.all("categoryID2").options[document.all("categoryID2").selectedIndex].remark;

                removeItem(4);
                parentCategoryID = document.all("categoryID2").value;
            }
            break;

        case 3:
            //중분류 선택
            if (document.all("categoryID3").value != "") {
                document.all("categoryName3").value = document.all("categoryID3").options[document.all("categoryID3").selectedIndex].text;
                document.all("categoryCourse3").value = document.all("categoryID3").options[document.all("categoryID3").selectedIndex].course;

                document.all("categoryName4").value = "";
                document.all("categoryCourse4").value = "";

                document.all("targetID").value = "";

                document.all("remark").value = document.all("categoryID3").options[document.all("categoryID3").selectedIndex].remark;

                parentCategoryID = document.all("categoryID3").value;
            }
            break;

        case 4:
            //소분류 & 전결사항 선택
            if (document.all("categoryID4").value != "") {
                document.all("categoryName4").value = document.all("categoryID4").options[document.all("categoryID4").selectedIndex].text;
                document.all("categoryCourse4").value = document.all("categoryID4").options[document.all("categoryID4").selectedIndex].course;
                document.all("remark").value = document.all("categoryID4").options[document.all("categoryID4").selectedIndex].remark;


                categoryID = document.all("categoryID4").value;
            }
            break;
    }

    if (gubun == 4) {
        //소분류 & 전결사항 선택시 전결권자 선택 및 비고 표시
        if (categoryID != "") {
            rtnVal = decisionTargetLoad(categoryID, companyID);

            var targetID = document.all("targetID");
            for (i = 0; i < targetID.length; i++) {
                if (targetID[i].value == parseInt(rtnVal)) {
                    targetID.options[i].selected = true;
                    break;
                }
            }
            document.all("targetName").value = document.all("targetID").options[document.all("targetID").selectedIndex].text;
        }

    } else {

        //하위 분류 리스트 가져오기
        if (parentCategoryID != "" && document.all("categoryID" + gubun).value != "") {

            rtnVal = decisionListLoad(gubun, parentCategoryID, companyID);

            arrRtnVal = rtnVal.split("∬");
            categoryID = arrRtnVal[0];
            categoryName = arrRtnVal[1];
            course = arrRtnVal[2];
            remark = arrRtnVal[3];

            var selectCtl = document.all("categoryID" + parseInt(gubun + 1));
            var objOptionItem = document.createElement("option");

            var selectCtlLen = selectCtl.length
            for (i = 0; i <= selectCtlLen; i++) {
                selectCtl.remove(0);
            }

            arrCategoryID = categoryID.split("∫");
            arrCategoryName = categoryName.split("∫");
            arrCourse = course.split("∫");
            arrRemark = remark.split("∫");

            for (i = 0; i < arrCategoryID.length - 1; i++) {
                var selectCtl = document.all("categoryID" + parseInt(gubun + 1));
                var objOptionItem = document.createElement("option");

                objOptionItem.value = arrCategoryID[i];
                objOptionItem.text = arrCategoryName[i];
                objOptionItem.course = arrCourse[i];
                objOptionItem.remark = arrRemark[i];

                selectCtl.add(objOptionItem, selectCtl.length + 1);
            }
        }
    }
}

// [삭제]
function removeItem(gubun) {
    //SelectBox Item삭제
    var selectCtl = document.all("categoryID" + gubun);
    var objOptionItem = document.createElement("option");

    var selectCtlLen = selectCtl.length;

    for (i = 0; i <= selectCtlLen; i++) {
        selectCtl.remove(0);
    }

}

// 전결권자 선택시
function targetItem() {
    //전결권자 선택시
    if (document.all("targetID").value != "") {
        document.all("targetName").value = document.all("targetID").options[document.all("targetID").selectedIndex].text
    }
}

//[위]
function decisionUp(gubun) {
    //전결사항 위로
    var selectCtl = document.all("categoryID" + gubun);

    switch (gubun) {
        case 1:
            if (selectCtl.value == "") {
                alert("사업부를 선택하세요."); return;
            }
            parentCategoryID = 0;
            break;

        case 2:
            if (selectCtl.value == "") {
                alert("대분류를 선택하세요."); return;
            }
            parentCategoryID = document.all("categoryID" + (gubun - 1)).value;
            break;

        case 3:
            if (selectCtl.value == "") {
                alert("중분류를 선택하세요."); return;
            }
            parentCategoryID = document.all("categoryID" + (gubun - 1)).value;
            break;

        case 4:
            if (selectCtl.value == "") {
                alert("소분류 & 전결사항을 선택하세요."); return;
            }
            parentCategoryID = document.all("categoryID" + (gubun - 1)).value;
            break;
    }

    var select_index = selectCtl.selectedIndex;
    var select_length = selectCtl.length;

    if (select_index == 0) {
        alert("더 이상 위로 올릴 수 없습니다.."); return;
    }

    rtnVal = decisionListUp(selectCtl.value, parentCategoryID, companyID);

    arrRtnVal = rtnVal.split("∬");
    categoryID = arrRtnVal[0];
    categoryName = arrRtnVal[1];
    course = arrRtnVal[2];
    remark = arrRtnVal[3];

    var objOptionItem = document.createElement("option");

    var selectCtlLen = selectCtl.length;
    for (k = 0; k <= selectCtlLen; k++) {
        selectCtl.remove(0);
    }

    var selectCtl = document.all("categoryID" + gubun);
    var objOptionItem = document.createElement("option");

    arrCategoryID = categoryID.split("∫");
    arrCategoryName = categoryName.split("∫");
    arrCourse = course.split("∫");
    arrRemark = remark.split("∫");

    for (i = 0; i < arrCategoryID.length - 1; i++) {
        var selectCtl = document.all("categoryID" + gubun);
        var objOptionItem = document.createElement("option");

        objOptionItem.value = arrCategoryID[i];
        objOptionItem.text = arrCategoryName[i];
        objOptionItem.course = arrCourse[i];
        objOptionItem.remark = arrRemark[i];

        selectCtl.add(objOptionItem, selectCtl.length + 1);
    }
    selectCtl.options[select_index - 1].selected = true;
}

// [아래]
function decisionDown(gubun) {
    //전결사항 아래로
    var selectCtl = document.all("categoryID" + gubun);

    switch (gubun) {
        case 1:
            if (selectCtl.value == "") {
                alert("사업부를 선택하세요."); return;
            }
            parentCategoryID = 0;
            break;

        case 2:
            if (selectCtl.value == "") {
                alert("대분류를 선택하세요.");  return;
            }
            parentCategoryID = document.all("categoryID" + (gubun - 1)).value;
            break;

        case 3:
            if (selectCtl.value == "") {
                alert("중분류를 선택하세요."); return;
            }
            parentCategoryID = document.all("categoryID" + (gubun - 1)).value;
            break;

        case 4:
            if (selectCtl.value == "") {
                alert("소분류 & 전결사항을 선택하세요."); return;
            }
            parentCategoryID = document.all("categoryID" + (gubun - 1)).value;
            break;
    }

    var select_index = selectCtl.selectedIndex;
    var select_length = selectCtl.length;

    if (select_index == select_length - 1) {
        alert("더 이상 아래로 내릴 수 없습니다.."); return;
    }

    rtnVal = decisionListDown(selectCtl.value, parentCategoryID, companyID);

    arrRtnVal = rtnVal.split("∬");
    categoryID = arrRtnVal[0];
    categoryName = arrRtnVal[1];
    course = arrRtnVal[2];
    remark = arrRtnVal[3];

    var objOptionItem = document.createElement("option");

    var selectCtlLen = selectCtl.length;
    for (k = 0; k <= selectCtlLen; k++) {
        selectCtl.remove(0);
    }

    var selectCtl = document.all("categoryID" + gubun);
    var objOptionItem = document.createElement("option");

    arrCategoryID = categoryID.split("∫");
    arrCategoryName = categoryName.split("∫");
    arrCourse = course.split("∫");
    arrRemark = remark.split("∫");

    for (i = 0; i < arrCategoryID.length - 1; i++) {
        var selectCtl = document.all("categoryID" + gubun);
        var objOptionItem = document.createElement("option");

        objOptionItem.value = arrCategoryID[i];
        objOptionItem.text = arrCategoryName[i];
        objOptionItem.course = arrCourse[i];
        objOptionItem.remark = arrRemark[i];

        selectCtl.add(objOptionItem, selectCtl.length + 1);
    }
    selectCtl.options[select_index + 1].selected = true;
}

// [소분류 - 좌측]
function decisionLeft() {
    //소분류 & 전결사항 공백 제거
    var selectCtl = document.all("categoryID4");
    var select_index = selectCtl.selectedIndex;

    if (selectCtl.value == "") {
        alert("소분류 & 전결사항을 선택하세요."); return;
    }

    rtnVal = decisionListLeft(selectCtl.value, companyID);

    selectCtl.options[select_index].text = selectCtl.options[select_index].text.replace("   ", "");
    document.all("categoryName4").value = selectCtl.options[select_index].text;
}

// [소분류 - 우측]
function decisionRight() {
    //소분류 & 전결사항 공백 추가
    var selectCtl = document.all("categoryID4");
    var select_index = selectCtl.selectedIndex;

    if (selectCtl.value == "") {
        alert("소분류 & 전결사항을 선택하세요."); return;
    }
    rtnVal = decisionListRight(selectCtl.value, companyID);

    selectCtl.options[select_index].text = "   " + selectCtl.options[select_index].text;
    document.all("categoryName4").value = selectCtl.options[select_index].text;
}

// [전결권자-위]
function targetUp() {

    //전결권자 위로
    var selectCtl = document.all("targetID");

    if (selectCtl.value == "") {
        alert("전결권자를 선택하세요.");  return;
    }

    if (selectCtl.value != "0") {

        var select_index = selectCtl.selectedIndex;
        var select_length = selectCtl.length;

        if (select_index == 1) {
            alert("더 이상 위로 올릴 수 없습니다.."); return;
        }

        rtnVal = decisionTargetUp(selectCtl.value, companyID);

        arrRtnVal = rtnVal.split("∬");
        targetID = arrRtnVal[0];
        targetName = arrRtnVal[1];


        var objOptionItem = document.createElement("option");

        var selectCtlLen = selectCtl.length;
        for (k = 0; k <= selectCtlLen; k++) {
            selectCtl.remove(0);
        }

        var selectCtl = document.all("targetID");
        var objOptionItem = document.createElement("option");

        objOptionItem.value = "0";
        objOptionItem.text = "규정없음";

        selectCtl.add(objOptionItem, selectCtl.length + 1);


        arrTargetID = targetID.split("∫");
        arrTargetName = targetName.split("∫");

        for (i = 0; i < arrTargetID.length - 1; i++) {
            var selectCtl = document.all("targetID");
            var objOptionItem = document.createElement("option");

            objOptionItem.value = arrTargetID[i];
            objOptionItem.text = arrTargetName[i];

            selectCtl.add(objOptionItem, selectCtl.length + 1);
        }
        selectCtl.options[select_index - 1].selected = true;
    }
}

// [전결권자-아래]
function targetDown() {

    //전결권자 아래로
    if (document.all("targetID").value == "") {
        alert("전결권자를 선택하세요."); return;
    }

    if (document.all("targetID").value != "0") {
        var selectCtl = document.all("targetID");

        var select_index = selectCtl.selectedIndex;
        var select_length = selectCtl.length;

        if (select_index == select_length - 1) {
            alert("더 이상 아래로 내릴 수 없습니다.."); return;
        }

        rtnVal = decisionTargetDown(selectCtl.value, companyID);

        arrRtnVal = rtnVal.split("∬");
        targetID = arrRtnVal[0];
        targetName = arrRtnVal[1];


        var objOptionItem = document.createElement("option");

        var selectCtlLen = selectCtl.length;
        for (k = 0; k <= selectCtlLen; k++) {
            selectCtl.remove(0);
        }

        var selectCtl = document.all("targetID");
        var objOptionItem = document.createElement("option");

        objOptionItem.value = "0";
        objOptionItem.text = "규정없음";

        selectCtl.add(objOptionItem, selectCtl.length + 1);


        arrTargetID = targetID.split("∫");
        arrTargetName = targetName.split("∫");

        for (i = 0; i < arrTargetID.length - 1; i++) {
            var selectCtl = document.all("targetID");
            var objOptionItem = document.createElement("option");

            objOptionItem.value = arrTargetID[i];
            objOptionItem.text = arrTargetName[i];

            selectCtl.add(objOptionItem, selectCtl.length + 1);
        }
        selectCtl.options[select_index + 1].selected = true;
    }
}