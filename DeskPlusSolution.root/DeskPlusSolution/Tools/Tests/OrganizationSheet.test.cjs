const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const Model = require('../../DeskPlusEIP/Account/Js/Organization/SheetModel.js');
const template = JSON.parse(fs.readFileSync(path.join(__dirname, '../../DeskPlusEIP/Account/App_Data/OrganizationSheetTemplate.json'), 'utf8')).document;
function cell(doc, ref) { const p = Model.reference(ref); return doc.cells.find(c => c.r === p.r && c.c === p.c); }
function value(doc, ref) { const p = Model.reference(ref); return Model.calculate(doc).values[Model.key(p.r, p.c)]; }

assert.deepEqual(Model.calculate(template).errors, []);
for (const [ref, expected] of Object.entries({ AU3: 101, D11: 10, P11: 31, AD11: 22, AM11: 17, AT11: 18 })) assert.equal(value(template, ref), expected, ref);

const cleared = Model.clone(template);
cell(cleared, 'C18').text = '';
assert.equal(value(cleared, 'D11'), 9);
assert.equal(value(cleared, 'AU3'), 100);
assert.equal(value(template, 'AU3'), 101, 'editing a clone must preserve the source');

const added = Model.changeRow(template, 17, false);
assert.equal(added.rows.length, 32);
assert.deepEqual(Model.calculate(added).errors, []);
assert.equal(value(added, 'AU3'), 101);
cell(added, 'C19').text = '추가 구성원';
assert.equal(value(added, 'D11'), 11);
assert.equal(value(added, 'AU3'), 102);
cell(added, 'K19').text = '집계 시작 행에 추가한 구성원';
assert.equal(value(added, 'P11'), 32, 'inserting at the first counted row must include the new row');
assert.equal(value(added, 'AU3'), 103);
assert.equal(cell(added, 'C20').text, cell(template, 'C19').text);
assert.equal(added.lines[0].start.r, template.lines[0].start.r);

const removed = Model.changeRow(added, 18, true);
assert.deepEqual(Model.calculate(removed).errors, []);
assert.equal(value(removed, 'AU3'), 101);
assert.equal(removed.rows.length, 31);

const appended = Model.changeRow(template, 30, false);
cell(appended, 'AQ32').text = '추가 구성원';
assert.equal(value(appended, 'AT11'), 19, 'adding after the last counted row must extend the range');
assert.equal(value(appended, 'AU3'), 102);

const cycle = Model.clone(template);
cell(cycle, 'D11').formula = 'AU3';
assert.ok(Model.calculate(cycle).errors.some(e => e.includes('순환')));
assert.equal(value(cycle, 'AU3'), '#ERROR!');

const bad = Model.clone(template);
cell(bad, 'D11').formula = 'SUM(A99999)';
assert.equal(value(bad, 'D11'), '#ERROR!');
assert.equal(value(bad, 'AU3'), '#ERROR!', 'a failed dependency must not become a healthy zero');
cell(bad, 'D11').formula = 'SUM(A1,,B1)';
assert.ok(Model.calculate(bad).errors.length > 0);
cell(bad, 'D11').formula = 'fetch(A1)';
assert.ok(Model.calculate(bad).errors.length > 0);

assert.equal(Model.address(30, 49), 'AX31');
assert.deepEqual(Model.reference('$AX$31'), { r: 30, c: 49 });
console.log('OrganizationSheet model: source totals, edits, row changes, references, cycle and invalid-formula checks passed.');
