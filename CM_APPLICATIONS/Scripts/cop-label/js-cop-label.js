
var scriptLoaded = false;
$.getScript("/Scripts/jquery.qrcode.min.js", function (data, textStatus, jqxhr) {
    if (jqxhr.status == 200) {
        scriptLoaded = true;
    } else {
        console.error("Error loading QRCode parser script");
    };

});

; (function ($, window, document, undefined) {

    COPR16Label = {
        //constructor: COPR16Label,
        init: function () {

            // build the HTML 
            console.log('init.. COPR16Label');
            //copr16.nameObjectLabel = 'copR16_';
            this.sureFix = this.settings.nameObjectLabel == 'SB' ? 'A' : 'B';
            this.copnoDisplay = this.settings.copnoText + this.sureFix;
            this.COP_Label = $('<div class="cop-label-main"></div>');
            this.COP_Table = $('<table class="cop-label-table" border="0" ></table>');
            this.COP_Tr1 = $('<tr class="cop-label-table-tr tr1"></tr>');
            this.COP_Tr1_TD1 = $('<td colspan="5" class="cop-label-copno COPNO-TEXT-SB">' + this.copnoDisplay +'</td>');
            this.COP_Tr1_TD2 = $('<td class="cop-label-type-text LABEL-TYPE">SB</td>');

            this.COP_Tr2 = $('<tr class="tr2"></tr>');
            this.COP_Tr2_TD1 = $('<td colspan="4" class="cop-label-model-text MODEL-TEXT">' + this.settings.modelText+'</td>');
            this.COP_Tr2_TD2 = $('<td rowspan="3" colspan="2" class="cop-label-td-qr"></td>');
            this.COP_Tr2_TD2_Div = $('<DIV class="cop-label-qrcode qr-code-dsp" id="qr-code-dsp" name="qr-code-dsp" ></DIV>');

            this.COP_Tr3 = $('<tr  class="tr3"></tr>');
            this.COP_Tr3_TD1 = $('<td colspan="4" class="cop-label-line-text LINE-TEXT">' + this.settings.lineText + '</td>');

            this.COP_Tr4 = $('<tr class="tr4"></tr>');
            this.COP_Tr4_TD1 = $('<td align="center" valign="middle" class="cop-label-pos-td"></td>');
            this.COP_Tr4_TD1_Label = $('<labeL class="cop-label-position-symbo POS-TEXT" >' + this.settings.posSymbo + '</labeL>');
            this.COP_Tr4_TD2 = $('<td colspan="3" class="cop-label-position-text POS-TEXT-EXT">' + this.settings.posText + '</td>');

            this.COP_Tr5 = $('<tr class="cop-label-tr1-part tr5"></tr>');
            this.COP_Tr5_TD1 = $('<td class="cop-label-td-lotno SBLOT" > ' + this.settings.sbLotText + '</td >');
            this.COP_Tr5_TD2 = $('<td class="cop-label-td-pipe">|</td>');
            this.COP_Tr5_TD3 = $('<td class="cop-label-td-part-type">SB:</td>');
            this.COP_Tr5_TD4 = $('<td colspan="3" class="SB-PARTNO">' + this.settings.sbPartText + '</td>');

            this.COP_Tr6 = $('<tr class="cop-label-tr-part tr6"></tr>');
            this.COP_Tr6_TD1 = $('<td class="cop-label-td-lotno BKL1LOT">' + this.settings.bkl1LotText + '</td>');
            this.COP_Tr6_TD2 = $('<td class="cop-label-td-pipe">|</td>');
            this.COP_Tr6_TD3 = $('<td class="cop-label-td-part-type">BKL1:</td>');
            this.COP_Tr6_TD4 = $('<td colspan="3" class="BKL1-PARTNO">' + this.settings.bkl1PartText + '</td>');
            this.COP_Tr7 = $('<tr class="cop-label-tr-part tr7"></tr>');
            this.COP_Tr7_TD1 = $('<td class="cop-label-td-lotno BKL2LOT" > ' + this.settings.bkl2LotText + '</td >');
            this.COP_Tr7_TD2 = $('<td class="cop-label-td-pipe">|</td>');
            this.COP_Tr7_TD3 = $('<td class="cop-label-td-part-type">BKL2:</td>');
            this.COP_Tr7_TD4 = $('<td colspan="3" class="BKL2-PARTNO">' + this.settings.bkl2PartText + '</td>');
            this.COP_Tr8 = $('<tr class="cop-label-tr-last tr8"></tr>');
            this.COP_Tr8_TD1 = $('<td>&nbsp;</td>');
            this.COP_Tr8_TD2 = $('<td>&nbsp;</td>');
            this.COP_Tr8_TD3 = $('<td>&nbsp;</td>');
            this.COP_Tr8_TD4 = $('<td>&nbsp;</td>');
            this.COP_Tr8_TD5 = $('<td>&nbsp;</td>');
            this.COP_Tr8_TD6 = $('<td>&nbsp;</td>');
            this._copno = $(this.COP_Label).find('.COPNO-TEXT-SB')[0];

        },
        printDebug: function () {
            console.log(this);
            //console.log(copr16.settings.nameObjectLabel);
        },
        setupLabel: function () {
            this.COP_Label = $('<div class="cop-label-main ' + this.settings.nameObjectLabel + '"></div>');
            //this.COP_Label = $('.' + this.nameObjectLabel);
            this.COP_Label.append(this.COP_Table);

            this.COP_Tr1.appendTo(this.COP_Table);
            this.COP_Tr2.appendTo(this.COP_Table);
            this.COP_Tr3.appendTo(this.COP_Table);
            this.COP_Tr4.appendTo(this.COP_Table);
            this.COP_Tr5.appendTo(this.COP_Table);
            this.COP_Tr6.appendTo(this.COP_Table);
            this.COP_Tr7.appendTo(this.COP_Table);
            this.COP_Tr8.appendTo(this.COP_Table);

            this.COP_Tr1_TD1.appendTo(this.COP_Tr1);
            this.COP_Tr1_TD2.appendTo(this.COP_Tr1);

            this.COP_Tr2_TD1.appendTo(this.COP_Tr2);
            this.COP_Tr2_TD2.appendTo(this.COP_Tr2);

            this.COP_Tr3_TD1.appendTo(this.COP_Tr3);

            this.COP_Tr4_TD1.appendTo(this.COP_Tr4);
            this.COP_Tr4_TD2.appendTo(this.COP_Tr4);

            this.COP_Tr5_TD1.appendTo(this.COP_Tr5);
            this.COP_Tr5_TD2.appendTo(this.COP_Tr5);
            this.COP_Tr5_TD3.appendTo(this.COP_Tr5);
            this.COP_Tr5_TD4.appendTo(this.COP_Tr5);

            this.COP_Tr6_TD1.appendTo(this.COP_Tr6);
            this.COP_Tr6_TD2.appendTo(this.COP_Tr6);
            this.COP_Tr6_TD3.appendTo(this.COP_Tr6);
            this.COP_Tr6_TD4.appendTo(this.COP_Tr6);

            this.COP_Tr7_TD1.appendTo(this.COP_Tr7);
            this.COP_Tr7_TD2.appendTo(this.COP_Tr7);
            this.COP_Tr7_TD3.appendTo(this.COP_Tr7);
            this.COP_Tr7_TD4.appendTo(this.COP_Tr7);

            this.COP_Tr8_TD1.appendTo(this.COP_Tr8);
            this.COP_Tr8_TD2.appendTo(this.COP_Tr8);
            this.COP_Tr8_TD3.appendTo(this.COP_Tr8);
            this.COP_Tr8_TD4.appendTo(this.COP_Tr8);
            this.COP_Tr8_TD5.appendTo(this.COP_Tr8);
            this.COP_Tr8_TD6.appendTo(this.COP_Tr8);

            this.COP_Tr2_TD2_Div.appendTo(this.COP_Tr2_TD2);
            this.COP_Tr4_TD1_Label.appendTo(this.COP_Tr4_TD1);
            this.COP_Tr2_TD2_Div.qrcode({ width: 100, height: 100, text: this.copnoDisplay});
            this.COP_Tr1_TD2.text(this.settings.nameObjectLabel);

            
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

        },
        setOptions: function (element, options) {
            //console.log(options);
            //console.log(element);
            // data-attributes options
            var dataOptions = {
                multiple: $(element).data("copr16-multiple"),
                qrcodeRegexp: new RegExp($(element).data("copr16-qrcode-regexp")),
                audioFeedback: $(element).data("copr16-audio-feedback"),
                repeatTimeout: $(element).data("copr16-repeat-timeout"),
                target: $(element).data("copr16-target"),
                skipDuplicates: $(element).data("copr16-skip-duplicates"),
                lineColor: $(element).data("copr16-line-color"),
                callback: $(element).data("copr16-callback"),
                nameObjectLabel: $(element).data("copr16-nameObjectLabel"),

                copnoText: $(element).data("copr16-copnoText"),
                modelText: $(element).data("copr16-modelText"),
                lineText: $(element).data("copr16-lineText"),
                posText: $(element).data("copr16-posText"),
                posSymbo: $(element).data("copr16-posSymbo"),

                sbLotText: $(element).data("copr16-sbLotText"),
                bkl1LotText: $(element).data("copr16-bkl1LotText"),
                bkl2LotText: $(element).data("copr16-bkl2LotText"),

                sbPartText: $(element).data("copr16-sbPartText"),
                bkl1PartText: $(element).data("copr16-bkl1PartText"),
                bkl2PartText: $(element).data("copr16-bkl2PartText")
            }

            // argument options override data-attributes options
            options = $.extend({}, dataOptions, options);

            // extend defaults with options
            var settings = $.extend({}, $.copR16_Label.defaults, options);

            // save options in the data attributes
            $(element).data("copr16", settings);
        },
        getOptions: function (element) {
            this.settings = $(element).data("copr16");
        },
        createLabel: function (element) {
            $('.cop-label-main .' + this.nameLabel).remove();
            this.COP_Label.insertBefore($(element));
            //console.log(copr16.COP_Label);
            //console.log('asfas');
        }
    };

    $.copR16_Label = {
        instance: null,
        defaults: {
            // single read or multiple readings/
            multiple: false,
            // only triggers for QRCodes matching the regexp
            qrcodeRegexp: /./,
            // play "Beep!" sound when reading qrcode successfully 
            audioFeedback: true,
            // in case of multiple readings, after a successful reading,
            // wait for repeatTimeout milliseconds before trying for the next lookup. 
            // Set to 0 to disable automatic re-tries: in such case user will have to 
            // click on the webcam canvas to trigger a new reading tentative
            repeatTimeout: 1500,
            // target input element to fill in with the readings in case of successful reading 
            // (newline separated in case of multiple readings).
            // Such element can be specified as jQuery object or as string identifier, e.g. "#target-input"
            target: null,
            // in case of multiple readings, skip duplicate readings
            skipDuplicates: true,
            // color of the lines highlighting the QRCode in the image when found
            lineColor: "#FF3B58",

            nameObjectLabel: "nameObjectLabel",

            copnoText: "copnoText",
            modelText: "modelText",
            lineText: "lineText",
            posText: "posText",
            posSymbo: "posSymbo",

            sbLotText: "sbLotText",
            bkl1LotText: "bkl1LotText",
            bkl2LotText: "bkl2LotText",

            sbPartText: "sbPartText",
            bkl1PartText: "bkl1PartText",
            bkl2PartText: "bkl2PartText"

        }
        
    };

 
    $.fn.COPR16newLabel = function (options) {

        // Instantiate the plugin only once (singletone) in the page:
        // when called again (or on a different element), we simply re-set the options 
        // and display the QrCode reader interface with the right options.
        // Options are saved in the data attribute of the bound element.

        //copr16 = new COPR16_Label();
        var localVar = Object.create(COPR16Label);
        //console.log(copr16);
        this.each(function () {
            localVar.setOptions(this, options);
        });
        localVar.getOptions(this);
        localVar.init();
        //$.copR16_Label.instance = copr16;
       
        localVar.printDebug();
        localVar.setupLabel();
        /*
        return this.each(function () {
            copr16.setOptions(this, options);
            //$(this).off("click.qrCodeReader").on("click.qrCodeReader", copr16.open);
        });
        */
        return localVar;

    };
}(jQuery, window, document));
