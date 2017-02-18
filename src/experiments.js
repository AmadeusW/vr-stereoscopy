window.addEventListener("keydown", function(e){
    if(e.keyCode === 37) { // left
        dynamic.src = scenes[0].url;
    }
    if(e.keyCode === 39) { // right
        dynamic.src = scenes[1].url;
    }
});

function sizeup(x) {
    var r = Math.pow(2, Math.ceil(Math.log2(x)));
    console.log(x + " > " + r)
    return r;
}

var dynamic = new Image();
dynamic.src = 'http://orig00.deviantart.net/8afe/f/2012/092/1/4/parish_church_st__georg_3d_____cross_eye_hdr_by_zour-d4upy31.jpg';
dynamic.onload = function() {
    console.log("loaded");
    var imageL = [0, 0, 0, 0]; // x, y, width, height
    var imageR = [0, 0, 0, 0]; // x, y, width, height
    if (1 == 1) { // todo: get selected scene's crop property
        imageL = [0, 0, dynamic.width/2, dynamic.height];
        imageR = [dynamic.width/2, 0, dynamic.width/2, dynamic.height];
    }
    else {
        console.log("not implemented");
    }
    var canvasWidth = sizeup(dynamic.width/2);
    var canvasHeight = sizeup(dynamic.height);
    var deltaWidth = canvasWidth - dynamic.width / 2;
    var deltaHeight = canvasHeight - dynamic.height;
    var canvasPosition = [deltaWidth/2, deltaHeight/2, dynamic.width/2, dynamic.height]; // x, y, width, height

    console.log(canvasWidth);
    console.log(canvasHeight);
    console.log(deltaWidth);
    console.log(deltaHeight);
    console.log(JSON.stringify(canvasPosition));

    document.getElementById('canvasL').setAttribute("width", canvasWidth);
    document.getElementById('canvasL').setAttribute("height", canvasHeight);
    document.getElementById('canvasR').setAttribute("width", canvasWidth);
    document.getElementById('canvasR').setAttribute("height", canvasHeight);

    document.getElementById('canvasL').getContext('2d').drawImage(
        dynamic, 
        imageL[0], imageL[1], imageL[2], imageL[3],
        canvasPosition[0], canvasPosition[1], canvasPosition[2], canvasPosition[3]);
    document.getElementById('canvasR').getContext('2d').drawImage(
        dynamic, 
        imageR[0], imageR[1], imageR[2], imageR[3],
        canvasPosition[0], canvasPosition[1], canvasPosition[2], canvasPosition[3]);
}

