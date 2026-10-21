function setCookie(name, value, expiredays) {
    var todayDate = new Date();
    var year = todayDate.getFullYear();
    var month = todayDate.getMonth();
    var day = todayDate.getDate() + 1 + expiredays;
    todayDate = new Date(year, month, day);
    document.cookie = name + "=" + escape(value) + "; path=/; expires=" + todayDate.toGMTString() + ";"
}

function saveCookie() {
    setCookie(popid, "yes", 0);
    self.close();
}

function neverShowAgain() {
    setCookie(popid, "yes", 3650);
    self.close();
}

function movePop() {
    var url = "";
    if (url != "") {
        opener.location.href = url;
    }
    self.close();
}

function ViewChecked() {
    setCookie(popid, "yes", 7);
    self.close();
}
