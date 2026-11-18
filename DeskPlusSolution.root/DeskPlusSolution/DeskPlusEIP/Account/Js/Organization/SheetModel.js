(function (root, factory) {
    if (typeof module === "object" && module.exports) module.exports = factory();
    else root.OrganizationSheetModel = factory();
}(typeof window === "undefined" ? this : window, function () {
    "use strict";
    function clone(value) { return JSON.parse(JSON.stringify(value)); }
    function key(r, c) { return r + ":" + c; }
    function address(r, c) {
        var result = "";
        for (c++; c > 0; c = Math.floor((c - 1) / 26)) result = String.fromCharCode(65 + (c - 1) % 26) + result;
        return result + (r + 1);
    }
    function reference(value) {
        var match = /^\$?([A-Z]+)\$?([0-9]+)$/.exec(value), c = 0;
        if (!match) throw new Error("셀 주소가 올바르지 않습니다.");
        for (var i = 0; i < match[1].length; i++) c = c * 26 + match[1].charCodeAt(i) - 64;
        return { r: Number(match[2]) - 1, c: c - 1 };
    }
    function offsets(sizes) {
        var result = [0];
        sizes.forEach(function (size) { result.push(result[result.length - 1] + size); });
        return result;
    }
    function calculate(document) {
        var cells = Object.create(null), values = Object.create(null), visiting = Object.create(null), failed = Object.create(null), errors = [];
        document.cells.forEach(function (cell) { cells[key(cell.r, cell.c)] = cell; });
        function read(ref) {
            var position = typeof ref === "string" ? reference(ref) : ref;
            if (position.r < 0 || position.r >= document.rows.length || position.c < 0 || position.c >= document.columns.length) throw new Error("수식 범위가 조직도를 벗어났습니다.");
            var id = key(position.r, position.c), cell = cells[id];
            if (failed[id]) throw new Error("참조한 집계 수식에 오류가 있습니다.");
            if (Object.prototype.hasOwnProperty.call(values, id)) return values[id];
            if (!cell) return "";
            if (visiting[id]) throw new Error("집계 수식에 순환 참조가 있습니다.");
            visiting[id] = true;
            try { values[id] = cell.formula ? evaluate(cell.formula) : cell.text; }
            finally { delete visiting[id]; }
            return values[id];
        }
        function evaluate(formula) {
            var text = formula.replace(/\s+/g, ""), tokens = text.match(/\$?[A-Z]+\$?\d+|SUM|COUNTA|\d+(?:\.\d+)?|[(),:+\-]/g) || [], cursor = 0;
            if (tokens.join("") !== text) throw new Error("지원하지 않는 집계 수식입니다.");
            function take(expected) {
                if (tokens[cursor] !== expected) throw new Error("집계 수식의 괄호나 구분자를 확인해 주세요.");
                cursor++;
            }
            function numeric(value) { return typeof value === "number" ? value : /^-?\d+(\.\d+)?$/.test(String(value)) ? Number(value) : 0; }
            function factor() {
                var token = tokens[cursor++];
                if (token === "-") return -numeric(factor());
                if (token === "(") { var nested = expression(); take(")"); return nested; }
                if (token === "SUM" || token === "COUNTA") {
                    take("(");
                    var inputs = [];
                    if (tokens[cursor] === ")") throw new Error("집계 범위를 입력해 주세요.");
                    do {
                        if (tokens[cursor] === ",") cursor++;
                        if (/^\$?[A-Z]+\$?\d+$/.test(tokens[cursor] || "") && tokens[cursor + 1] === ":") {
                            var start = reference(tokens[cursor++]); cursor++;
                            var end = reference(tokens[cursor++]);
                            if (end.r < start.r || end.c < start.c) throw new Error("집계 범위의 순서를 확인해 주세요.");
                            if ((end.r - start.r + 1) * (end.c - start.c + 1) > 20000) throw new Error("집계 범위가 너무 큽니다.");
                            for (var r = start.r; r <= end.r; r++) for (var c = start.c; c <= end.c; c++) inputs.push(read({ r: r, c: c }));
                        } else inputs.push(expression());
                    } while (tokens[cursor] === ",");
                    take(")");
                    return token === "COUNTA" ? inputs.filter(function (value) { return value !== "" && value !== null && value !== undefined; }).length
                        : inputs.reduce(function (sum, value) { return sum + numeric(value); }, 0);
                }
                if (/^\$?[A-Z]+\$?\d+$/.test(token || "")) return read(token);
                if (/^\d+(\.\d+)?$/.test(token || "")) return Number(token);
                throw new Error("집계 수식을 확인해 주세요.");
            }
            function expression() {
                var value = factor();
                while (tokens[cursor] === "+" || tokens[cursor] === "-") {
                    var operator = tokens[cursor++], next = factor();
                    value = numeric(value) + (operator === "+" ? 1 : -1) * numeric(next);
                }
                return value;
            }
            var result = expression();
            if (cursor !== tokens.length || typeof result === "number" && !isFinite(result)) throw new Error("집계 수식을 확인해 주세요.");
            return result;
        }
        document.cells.forEach(function (cell) {
            try { read({ r: cell.r, c: cell.c }); }
            catch (error) { failed[key(cell.r, cell.c)] = true; values[key(cell.r, cell.c)] = "#ERROR!"; errors.push(address(cell.r, cell.c) + ": " + error.message); }
        });
        return { values: values, errors: errors };
    }
    function updateReferences(formula, row, remove) {
        return (formula || "").replace(/(\$?[A-Z]+\$?\d+)(?::(\$?[A-Z]+\$?\d+))?/g, function (_, first, last) {
            var start = reference(first), end = last ? reference(last) : null;
            if (!remove) {
                if (start.r > row || !end && start.r === row) start.r++;
                if (end && end.r >= row - 1) end.r++;
            } else if (end) {
                if (start.r === row && end.r === row) return "#REF!";
                if (start.r > row) start.r--;
                if (end.r >= row) end.r--;
            } else {
                if (start.r === row) return "#REF!";
                if (start.r > row) start.r--;
            }
            return address(start.r, start.c) + (end ? ":" + address(end.r, end.c) : "");
        });
    }
    function changeRow(document, row, remove) {
        var result = clone(document);
        if (remove && result.rows.length <= 1) throw new Error("마지막 행은 삭제할 수 없습니다.");
        if (!remove && result.rows.length >= 200) throw new Error("최대 200행까지 사용할 수 있습니다.");
        if (row < 0 || row >= result.rows.length) throw new Error("행을 먼저 선택해 주세요.");
        var at = remove ? row : row + 1, copies = [];
        if (!remove) result.cells.filter(function (cell) { return cell.r === row && cell.rs === 1; }).forEach(function (cell) {
            var copy = clone(cell); copy.r = at; copy.text = ""; copy.formula = ""; copy.prefix = ""; copy.suffix = ""; copies.push(copy);
        });
        result.cells = result.cells.filter(function (cell) {
            if (remove && cell.r === at && cell.rs === 1) return false;
            if (cell.r > at || !remove && cell.r === at) cell.r += remove ? -1 : 1;
            else if (cell.r <= at && cell.r + cell.rs > at && (remove || cell.r < at)) cell.rs += remove ? -1 : 1;
            cell.formula = updateReferences(cell.formula, at, remove);
            return true;
        }).concat(copies);
        if (remove) result.rows.splice(at, 1);
        else result.rows.splice(at, 0, result.rows[row]);
        function anchor(point) { if (point.r > at || !remove && point.r === at) point.r += remove ? -1 : 1; point.r = Math.min(point.r, result.rows.length); }
        result.images.forEach(function (image) { anchor(image.anchor); });
        result.lines.forEach(function (line) { anchor(line.start); anchor(line.end); });
        return result;
    }
    return { clone: clone, key: key, address: address, reference: reference, offsets: offsets, calculate: calculate, changeRow: changeRow };
}));
