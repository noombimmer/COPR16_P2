/**
 <script src="jsqr/jsQR.js"></script>
 <div class="wrap-qrcode-scanner">
    <h1>QRCode Scanner</h1>
    <div id="loadingMessage">🎥 Unable to access video stream (please make sure you have a webcam enabled)</div>
    <canvas id="canvas" hidden></canvas>
    <div id="output" hidden>
    <div id="outputMessage">No QR code detected.</div>
    <div hidden><b>Data:</b> <span id="outputData"></span></div>
    </div>
    <audio id="beepsound" controls>
    <source src="sound/scanner-beeps-barcode.mp3" type="audio/mpeg">
    Your browser does not support the audio tag.
    </audio>
    <img id="outputqrcode">
    <canvas id="canvas2" ></canvas>

</div>
 * */
$.getScript("/Scripts/qr/js/jsQR/jsQR.min.js", function (data, textStatus, jqxhr) {
    if (jqxhr.status == 200) {
        scriptLoaded = true;
    } else {
        console.error("Error loading QRCode parser script");
    };

});
var scriptLoaded = false;
var video = document.createElement("video");
var canvasElement = null;//document.getElementById("canvas");
var canvas = null;//canvasElement.getContext("2d");
var loadingMessage = null;//document.getElementById("loadingMessage");
var outputContainer = null;//document.getElementById("output");
var outputMessage = null;//document.getElementById("outputMessage");
var outputData = null;//document.getElementById("outputData");
var beepsound = null;//document.getElementById("beepsound");
var outputQrcode = null;//document.getElementById('outputqrcode');
var TLR, TRR, BRL, BLL;
var code;
var waiting;
var tergetTextBox;
var isEnable = false;
var wrap_qrcode_scanner = null;
var localStream = null;


function drawLine(begin, end, color) {
    canvas.beginPath();
    canvas.moveTo(begin.x, begin.y);
    canvas.lineTo(end.x, end.y);
    canvas.lineWidth = 4;
    canvas.strokeStyle = color;
    canvas.stroke();
    return true;
}
function enableQr(targetName) {
    if (isEnable) {
        var qrCamera = document.getElementById('wrap-qrcode-scanner');
        //qrCamera.hidden = true;
        $('.wrap-qrcode-scanner').remove();

        video.pause();
        video.src = "";
        isEnable = false;
        console.log(localStream);
        localStream.getTracks()[0].stop();
        return;

    } else {
        createElement(targetName);
        // Use facingMode: environment to attemt to get the front camera on phones

        navigator.mediaDevices.getUserMedia({ video: { facingMode: "environment" } }).then(function (stream) {
            localStream = stream;
            video.srcObject = stream;
            video.setAttribute("playsinline", true); // required to tell iOS safari we don't want fullscreen
            video.play();
            requestAnimationFrame(tick);
        });
        isEnable = true;
    }
}
function createElement(targetName) {
    var target = $(targetName);
    wrap_qrcode_scanner = '<div class="wrap-qrcode-scanner" id="wrap-qrcode-scanner"></div>';

    $(wrap_qrcode_scanner).insertBefore(targetName);
    
    canvasElement = '<canvas id="canvas" hidden></canvas>';
    $(canvasElement).appendTo('.wrap-qrcode-scanner');
    canvasElement = document.getElementById("canvas");
    console.log(canvasElement);
    canvas = canvasElement.getContext("2d");
    console.log(canvas);

    loadingMessage = '<div id="loadingMessage">🎥 Unable to access video stream (please make sure you have a webcam enabled)</div>';
    $(loadingMessage).appendTo('.wrap-qrcode-scanner');

    outputContainer = '<div id="output" hidden></div>';
    $(outputContainer).appendTo('.wrap-qrcode-scanner');

    outputMessage = '<div id="outputMessage">No QR code detected.</div>';
    $(outputMessage).appendTo('#output');

    outputData = '<span id="outputData"></span>';
    $(outputData).appendTo('#output');
    outputData = document.getElementById("outputData");

    beepsound = '<audio id="beepsound" controls style="display:none;"><source src="/Scripts/qr/audio/beep.mp3" type="audio/mpeg"></audio>';
    $(beepsound).appendTo('.wrap-qrcode-scanner');
    beepsound = document.getElementById("beepsound");

    outputQrcode = '<img id="outputqrcode">';
    $(outputQrcode).appendTo('.wrap-qrcode-scanner');



    tergetTextBox = target;
    /*
    canvasElement = document.getElementById("canvas");

    canvas = canvasElement.getContext("2d");

    loadingMessage = '<div id="loadingMessage">🎥 Unable to access video stream (please make sure you have a webcam enabled)</div>';
    loadingMessage.appendTo('.wrap-qrcode-scanner');

    outputContainer = '<div id="output" hidden></div>';
    outputContainer.appendTo('.wrap-qrcode-scanner');

    outputMessage = '<div id="outputMessage">No QR code detected.</div>';
    outputMessage.appendTo('#output');

    outputData = '<span id="outputData"></span>';
    outputData.appendTo('#output');

    beepsound = '<audio id="beepsound" controls><source src="/Scripts/qr/audio/beep.mp3" type="audio/mpeg"></audio>';
    beepsound.appendTo('.wrap-qrcode-scanner');

    outputQrcode = '<img id="outputqrcode">';
    outputQrcode.appendTo('.wrap-qrcode-scanner');
    tergetTextBox = target;
    */
}

function tick() {
    loadingMessage.innerText = "⏳ Loading video..."
    if (video.readyState === video.HAVE_ENOUGH_DATA) {
        loadingMessage.hidden = true;
        canvasElement.hidden = false;
        outputContainer.hidden = false;

        canvasElement.height = video.videoHeight;
        canvasElement.width = video.videoWidth;
        canvas.drawImage(video, 0, 0, canvasElement.width, canvasElement.height);
        if (!video.paused) {
            var imageData = canvas.getImageData(0, 0, canvasElement.width, canvasElement.height);
            code = jsQR(imageData.data, imageData.width, imageData.height, {
                inversionAttempts: "dontInvert",
            });
        }
        if (code) {
            TLR = drawLine(code.location.topLeftCorner, code.location.topRightCorner, "#FF3B58");
            TRR = drawLine(code.location.topRightCorner, code.location.bottomRightCorner, "#FF3B58");
            BRL = drawLine(code.location.bottomRightCorner, code.location.bottomLeftCorner, "#FF3B58");
            BLL = drawLine(code.location.bottomLeftCorner, code.location.topLeftCorner, "#FF3B58");
            outputMessage.hidden = true;
            outputData.parentElement.hidden = false;
            outputData.innerText = code.data;
            if (code.data != "" && !waiting && TLR == true && TRR == true && BRL == true && BLL == true) {
                console.log(code.data);
                console.log(tergetTextBox);
                tergetTextBox.val(code.data);
                // สามารถส่งค่า code.data ไปทำงานอย่างอื่นๆ ผ่าน ajax ได้
                video.pause();
                beepsound.play();
                beepsound.onended = function () {
                    beepsound.muted = true;
                };
                // ให้เริ่มเล่นวิดีโอก่อนล็กน้อย เพื่อล้างค่ารูป qrcod ล่าสุด เป็นการใช้รูปจากกล้องแทน
                setTimeout(function () {
                    video.play();
                }, 4500);
                // ให้รอ 5 วินาทีสำหรับการ สแกนในครั้งจ่อไป
                waiting = setTimeout(function () {
                    TLR, TRR, BRL, BLL = null;
                    beepsound.muted = false;
                    if (waiting) {
                        clearTimeout(waiting);
                        waiting = null;
                    }
                }, 5000);
            }
        } else {
            outputMessage.hidden = false;
            outputData.parentElement.hidden = true;
        }
    }
    requestAnimationFrame(tick);
}