var crop = false; // if false, split image in two. if true, use a cropping algorithm
var dynamic = new Image();
dynamic.src = 'http://orig00.deviantart.net/8afe/f/2012/092/1/4/parish_church_st__georg_3d_____cross_eye_hdr_by_zour-d4upy31.jpg';
dynamic.onload = function() {
    console.log("loaded");
    var imageL = [0, 0, 0, 0]; // x, y, width, height
    var imageR = [0, 0, 0, 0]; // x, y, width, height
    if (crop === false) {
        imageL = [0, 0, dynamic.width/2, dynamic.height];
        imageR = [dynamic.width/2, 0, dynamic.width/2, dynamic.height];
    }
    else {
        console.log("not implemented");
    }
    var canvasPosition = [0, 0, dynamic.width/2, dynamic.height]; // x, y, width, height
    document.getElementById('canvasL').setAttribute("width", dynamic.width/2);
    document.getElementById('canvasL').setAttribute("height", dynamic.height);
    document.getElementById('canvasR').setAttribute("width", dynamic.width/2);
    document.getElementById('canvasR').setAttribute("height", dynamic.height);

    document.getElementById('canvasL').getContext('2d').drawImage(
        dynamic, 
        imageL[0], imageL[1], imageL[2], imageL[3],
        canvasPosition[0], canvasPosition[1], canvasPosition[2], canvasPosition[3]);
    document.getElementById('canvasR').getContext('2d').drawImage(
        dynamic, 
        imageR[0], imageR[1], imageR[2], imageR[3],
        canvasPosition[0], canvasPosition[1], canvasPosition[2], canvasPosition[3]);
}
