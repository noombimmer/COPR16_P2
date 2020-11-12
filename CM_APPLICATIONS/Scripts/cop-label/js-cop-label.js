
var COP_Label = '<div class="cop-label-main"></div>';
var COP_Table = '<table class="cop-label-table" border="0" ></table>';
var COP_Tr1 = '<tr class="cop-label-table-tr tr1"></tr>';
var COP_Tr1_TD1 = '<td colspan="5" class="cop-label-copno COPNO-TEXT-SB">2001579-02-00A</td>';
var COP_Tr1_TD2 = '<td class="cop-label-type-text LABEL-TYPE">SB</td>';
var COP_Tr2 = '<tr class="tr2"></tr>';
var COP_Tr2_TD1 = '<td colspan="4" class="cop-label-model-text MODEL-TEXT">                2GM                    </td>';
var COP_Tr2_TD2 = '<td rowspan="3" colspan="2" class="cop-label-td-qr"></td>';
var COP_Tr2_TD2_Div = '<DIV class="cop-label-qrcode qr-code-dsp" id="qr-code-dsp" name="qr-code-dsp" ></DIV>';
var COP_Tr3 = '<tr  class="tr3"></tr>';
var COP_Tr3_TD1 = '<td colspan="4" class="cop-label-line-text LINE-TEXT">                R200RRS                    </td>';
var COP_Tr4 = '<tr class="tr4"></tr>';
var COP_Tr4_TD1 = '<td align="center" valign="middle"></td>';
var COP_Tr4_TD1_Label = '< labeL class="cop-label-position-symbo POS-TEXT" > S4 </labeL >';
var COP_Tr4_TD2 = '<td colspan="3" class="cop-label-position-text POS-TEXT-EXT">Rear 2nd LH</td>';
var COP_Tr5 = '<tr class="cop-label-tr1-part tr5"></tr>';
var COP_Tr5_TD1 = '<td class="cop-label-td-lotno SBLOT" > 1234</td >';
var COP_Tr5_TD2 = '<td class="cop-label-td-pipe">|</td>';
var COP_Tr5_TD3 = '<td class="cop-label-td-part-type">SB:</td>';
var COP_Tr5_TD4 = '<td colspan="3" class="SB-PARTNO">123456789</td>';
var COP_Tr6 = '<tr class="cop-label-tr-part tr6"></tr>';
var COP_Tr6_TD1 = '<td class="cop-label-td-lotno BKL1LOT">1234</td>';
var COP_Tr6_TD2 = '<td class="cop-label-td-pipe">|</td>';
var COP_Tr6_TD3 = '<td class="cop-label-td-part-type">BKL1:</td>';
var COP_Tr6_TD4 = '<td colspan="3" class="BKL1-PARTNO">123456789</td>';
var COP_Tr7 = '<tr class="cop-label-tr-part tr7"></tr>';
var COP_Tr7_TD1 = '<td class="cop-label-td-lotno BKL2LOT" > 1234</td >';
var COP_Tr7_TD2 = '<td class="cop-label-td-pipe">|</td>';
var COP_Tr7_TD3 = '<td class="cop-label-td-part-type">BKL2:</td>';
var COP_Tr7_TD4 = '<td colspan="3" class="BKL2-PARTNO">123456789</td>';
var COP_Tr8 = '<tr class="tr8"></tr>';
var COP_Tr8_TD1 = '<td>&nbsp;</td>';
var COP_Tr8_TD2 = '<td>&nbsp;</td>';
var COP_Tr8_TD3 = '<td>&nbsp;</td>';
var COP_Tr8_TD4 = '<td>&nbsp;</td>';
var COP_Tr8_TD5 = '<td>&nbsp;</td>';
var COP_Tr8_TD6 = '<td>&nbsp;</td>';
var _copno = $(COP_Label).find('.COPNO-TEXT-SB')[0];

var nameObjectLabel = null;
function genLabel(nameLabel) {
    if ($('.' + nameLabel) != undefined) {
        $('.' + nameLabel).remove();
    }
    nameObjectLabel = nameLabel;
    COP_Label = '<div class="cop-label-main ' + nameLabel+ '"></div>';
    return $(COP_Label);
}
function setLabel() {
    COP_Label = $('.' + nameObjectLabel);
    COP_Label.append(COP_Table);
    $(COP_Tr1).appendTo('.' + nameObjectLabel + ' .cop-label-table');
    $(COP_Tr2).appendTo('.' + nameObjectLabel + ' .cop-label-table');
    $(COP_Tr3).appendTo('.' + nameObjectLabel + ' .cop-label-table');
    $(COP_Tr4).appendTo('.' + nameObjectLabel + ' .cop-label-table');
    $(COP_Tr5).appendTo('.' + nameObjectLabel + ' .cop-label-table');
    $(COP_Tr6).appendTo('.' + nameObjectLabel + ' .cop-label-table');
    $(COP_Tr7).appendTo('.' + nameObjectLabel + ' .cop-label-table');
    $(COP_Tr8).appendTo('.' + nameObjectLabel + ' .cop-label-table');

    $(COP_Tr1_TD1).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr1');
    $(COP_Tr1_TD2).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr1');

    $(COP_Tr2_TD1).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr2');
    $(COP_Tr2_TD2).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr2');

    $(COP_Tr3_TD1).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr3');

    $(COP_Tr4_TD1).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr4');
    $(COP_Tr4_TD2).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr4');
    
    $(COP_Tr5_TD1).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr5');
    $(COP_Tr5_TD2).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr5');
    $(COP_Tr5_TD3).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr5');
    $(COP_Tr5_TD4).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr5');

    $(COP_Tr6_TD1).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr6');
    $(COP_Tr6_TD2).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr6');
    $(COP_Tr6_TD3).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr6');
    $(COP_Tr6_TD4).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr6');

    $(COP_Tr7_TD1).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr7');
    $(COP_Tr7_TD2).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr7');
    $(COP_Tr7_TD3).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr7');
    $(COP_Tr7_TD4).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr7');

    $(COP_Tr8_TD1).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr8');
    $(COP_Tr8_TD2).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr8');
    $(COP_Tr8_TD3).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr8');
    $(COP_Tr8_TD4).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr8');
    $(COP_Tr8_TD5).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr8');
    $(COP_Tr8_TD6).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr8');

    $(COP_Tr2_TD2_Div).appendTo('.' + nameObjectLabel + ' .cop-label-table .tr2 .cop-label-td-qr');

    /*
    COP_Label.find('.cop-label-table')[0].append(COP_Tr1);
    COP_Label.find('.cop-label-table')[0].append(COP_Tr2);
    COP_Label.find('.cop-label-table')[0].append(COP_Tr3);
    COP_Label.find('.cop-label-table')[0].append(COP_Tr4);
    COP_Label.find('.cop-label-table')[0].append(COP_Tr5);
    COP_Label.find('.cop-label-table')[0].append(COP_Tr6);
    COP_Label.find('.cop-label-table')[0].append(COP_Tr7);
    COP_Label.find('.cop-label-table')[0].append(COP_Tr8);
    */
}
