"""Read the visible organization worksheet into the web editor's initial template."""
import argparse
import base64
import datetime
import json
import math
import posixpath
import re
import xml.etree.ElementTree as ET
from zipfile import ZipFile

NS = {'m': 'http://schemas.openxmlformats.org/spreadsheetml/2006/main',
      'a': 'http://schemas.openxmlformats.org/drawingml/2006/main',
      'x': 'http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing',
      'r': 'http://schemas.openxmlformats.org/officeDocument/2006/relationships'}


def address(value):
    match = re.fullmatch(r'([A-Z]+)([0-9]+)', value)
    column = 0
    for char in match[1]:
        column = column * 26 + ord(char) - 64
    return int(match[2]) - 1, column - 1


def extract(source, company):
    with ZipFile(source) as archive:
        workbook = ET.fromstring(archive.read('xl/workbook.xml'))
        sheets = workbook.findall('m:sheets/m:sheet', NS)
        chosen = next(s for s in sheets if s.get('name') == '조직도')
        relationships = {r.get('Id'): r.get('Target') for r in ET.fromstring(archive.read('xl/_rels/workbook.xml.rels'))}
        sheet_path = posixpath.normpath('xl/' + relationships[chosen.get('{' + NS['r'] + '}id')])
        sheet = ET.fromstring(archive.read(sheet_path))
        strings = [''.join(s.itertext()) for s in ET.fromstring(archive.read('xl/sharedStrings.xml'))]
        style_xml = ET.fromstring(archive.read('xl/styles.xml'))
        theme = ET.fromstring(archive.read('xl/theme/theme1.xml'))
        scheme = theme.find('a:themeElements/a:clrScheme', NS)
        colors = []
        for entry in scheme:
            color = list(entry)[0]
            colors.append('#' + color.get('lastClr', color.get('val', '000000'))[-6:])
        # Spreadsheet theme order begins Light1, Dark1, Light2, Dark2.
        colors[0], colors[1] = colors[1], colors[0]
        colors[2], colors[3] = colors[3], colors[2]

        def color(node, default='#000000'):
            if node is None:
                return default
            if node.get('rgb'):
                return '#' + node.get('rgb')[-6:]
            if node.get('theme'):
                return colors[int(node.get('theme'))]
            if node.get('indexed') == '64':
                return '#000000'
            return default

        fonts = style_xml.find('m:fonts', NS)
        fills = style_xml.find('m:fills', NS)
        borders = style_xml.find('m:borders', NS)
        formats = {int(n.get('numFmtId')): n.get('formatCode') for n in style_xml.findall('m:numFmts/m:numFmt', NS)}
        styles = []
        for xf in style_xml.find('m:cellXfs', NS):
            font = fonts[int(xf.get('fontId', '0'))]
            fill = fills[int(xf.get('fillId', '0'))].find('m:patternFill', NS)
            alignment = xf.find('m:alignment', NS)
            border = borders[int(xf.get('borderId', '0'))]
            style = {'fontSize': round(float(font.find('m:sz', NS).get('val')) * 4 / 3, 2),
                     'bold': font.find('m:b', NS) is not None,
                     'color': color(font.find('m:color', NS)), 'fill': '#ffffff',
                     'align': alignment.get('horizontal', 'center') if alignment is not None else 'center',
                     'vertical': alignment.get('vertical', 'center') if alignment is not None else 'center'}
            if style['align'] not in ['left', 'center', 'right']:
                style['align'] = 'center'
            if fill is not None and fill.get('patternType') == 'solid':
                style['fill'] = color(fill.find('m:fgColor', NS), '#ffffff')
            for side in ['top', 'right', 'bottom', 'left']:
                edge = border.find('m:' + side, NS)
                edge_style = edge.get('style') if edge is not None else None
                weight = 2 if edge_style in ['medium', 'thick', 'double'] else 1
                style[side] = {'width': weight if edge_style else 0,
                               'color': color(edge.find('m:color', NS)) if edge is not None else '#000000'}
            styles.append(style)

        row_count, column_count = 31, 50
        columns = [64] * column_count
        for col in sheet.findall('m:cols/m:col', NS):
            for index in range(int(col.get('min')) - 1, min(column_count, int(col.get('max')))):
                columns[index] = math.floor(float(col.get('width', '8.43')) * 7 + 5)
        rows = [20] * row_count
        for row in sheet.findall('m:sheetData/m:row', NS):
            index = int(row.get('r')) - 1
            if index < row_count:
                rows[index] = round(float(row.get('ht', '15')) * 4 / 3, 2)
        merges, covered = {}, set()
        for merge in sheet.findall('m:mergeCells/m:mergeCell', NS):
            begin, end = merge.get('ref').split(':')
            r, c = address(begin)
            rr, cc = address(end)
            if r >= row_count or c >= column_count:
                continue
            rr, cc = min(rr, row_count - 1), min(cc, column_count - 1)
            merges[r, c] = (rr - r + 1, cc - c + 1)
            covered.update((y, x) for y in range(r, rr + 1) for x in range(c, cc + 1) if (y, x) != (r, c))
        source_cells = {}
        for cell in sheet.findall('m:sheetData/m:row/m:c', NS):
            source_cells[address(cell.get('r'))] = cell
        cells = []
        for r in range(row_count):
            for c in range(column_count):
                if (r, c) in covered:
                    continue
                xml = source_cells.get((r, c))
                style_id = int(xml.get('s', '1')) if xml is not None else 1
                value = xml.find('m:v', NS) if xml is not None else None
                formula = xml.find('m:f', NS) if xml is not None else None
                text = value.text if value is not None else ''
                if xml is not None and xml.get('t') == 's':
                    text = strings[int(text)]
                xf = list(style_xml.find('m:cellXfs', NS))[style_id]
                number_format = formats.get(int(xf.get('numFmtId', '0')), '')
                prefix, suffix = '', ''
                if (int(xf.get('numFmtId', '0')) in list(range(14, 23)) + list(range(27, 37)) or re.search(r'[ymd]', number_format.lower())) and text:
                    text = (datetime.datetime(1899, 12, 30) + datetime.timedelta(days=float(text))).strftime('%Y-%m-%d')
                elif number_format and re.search(r'[0#?]', number_format):
                    numeric = re.search(r'[0#?][0#?,.]*', number_format)
                    before, after = number_format[:numeric.start()], number_format[numeric.end():]
                    prefix = ''.join(re.findall(r'"([^"]*)"', before))
                    suffix = ''.join(re.findall(r'"([^"]*)"', after))
                    if r'\ ' in before:
                        prefix += ' '
                rs, cs = merges.get((r, c), (1, 1))
                # Merged cells keep the outer borders carried by the end cells.
                if rs > 1 or cs > 1:
                    merged_style = dict(styles[style_id])
                    for side, locations in [('right', [(y, c + cs - 1) for y in range(r, r + rs)]),
                                            ('bottom', [(r + rs - 1, x) for x in range(c, c + cs)])]:
                        for location in locations:
                            edge_cell = source_cells.get(location)
                            edge_id = int(edge_cell.get('s', '1')) if edge_cell is not None else 1
                            if styles[edge_id][side]['width'] > merged_style[side]['width']:
                                merged_style[side] = styles[edge_id][side]
                    if merged_style != styles[style_id]:
                        style_id = len(styles)
                        styles.append(merged_style)
                cells.append({'r': r, 'c': c, 'rs': rs, 'cs': cs, 'text': text or '',
                              'formula': formula.text if formula is not None else '',
                              'prefix': prefix, 'suffix': suffix, 'style': style_id})

        images, lines = [], []
        drawing = ET.fromstring(archive.read('xl/drawings/drawing2.xml'))
        drawing_rels = {n.get('Id'): n.get('Target') for n in ET.fromstring(archive.read('xl/drawings/_rels/drawing2.xml.rels'))}

        def anchor(node):
            return {'r': int(node.find('x:row', NS).text), 'c': int(node.find('x:col', NS).text),
                    'dx': round(int(node.find('x:colOff', NS).text) / 9525, 2),
                    'dy': round(int(node.find('x:rowOff', NS).text) / 9525, 2)}

        for index, item in enumerate(drawing):
            photo = item.find('x:pic', NS)
            if photo is not None:
                relationship = photo.find('.//a:blip', NS).get('{' + NS['r'] + '}embed')
                path = posixpath.normpath('xl/drawings/' + drawing_rels[relationship])
                ext = item.find('x:ext', NS)
                mime = 'image/png' if path.lower().endswith('.png') else 'image/jpeg'
                images.append({'id': 'image-' + str(index), 'anchor': anchor(item.find('x:from', NS)),
                               'width': round(int(ext.get('cx')) / 9525, 2), 'height': round(int(ext.get('cy')) / 9525, 2),
                               'src': 'data:' + mime + ';base64,' + base64.b64encode(archive.read(path)).decode('ascii')})
            connector = item.find('x:cxnSp', NS)
            if connector is not None:
                line = connector.find('x:spPr/a:ln', NS)
                transform = connector.find('x:spPr/a:xfrm', NS)
                start, end = anchor(item.find('x:from', NS)), anchor(item.find('x:to', NS))
                if transform is not None and transform.get('flipV') == '1':
                    start['r'], end['r'] = end['r'], start['r']
                    start['dy'], end['dy'] = end['dy'], start['dy']
                lines.append({'id': 'line-' + str(index), 'start': start, 'end': end,
                              'width': round(int(line.get('w', '12700')) / 9525, 2), 'color': '#000000'})
        return {'companyID': company, 'document': {'schemaVersion': 1, 'title': '조직도', 'columns': columns,
                'rows': rows, 'styles': styles, 'cells': cells, 'images': images, 'lines': lines}}


if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('source')
    parser.add_argument('output')
    parser.add_argument('--company-id', type=int, required=True)
    args = parser.parse_args()
    result = extract(args.source, args.company_id)
    with open(args.output, 'w', encoding='utf-8') as stream:
        json.dump(result, stream, ensure_ascii=False, separators=(',', ':'))
    print('Imported visible organization sheet: {} cells, {} images, {} connectors.'.format(
        len(result['document']['cells']), len(result['document']['images']), len(result['document']['lines'])))
