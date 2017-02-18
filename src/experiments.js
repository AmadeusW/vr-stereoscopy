var dynamic = new Image();
dynamic.src = 'http://orig00.deviantart.net/8afe/f/2012/092/1/4/parish_church_st__georg_3d_____cross_eye_hdr_by_zour-d4upy31.jpg';
dynamic.onload = function() {
    console.log("loaded");
    var imageL = [0, 0, 700, 1080]; // x, y, width, height
    var imageR = [700, 0, 700, 1080]; // x, y, width, height
    var canvasPosition = [0, 0, 256, 256]; // x, y, width, height
    document.getElementById('canvasL').getContext('2d').drawImage(
        dynamic, 
        imageL[0], imageL[1], imageL[2], imageL[3],
        canvasPosition[0], canvasPosition[1], canvasPosition[2], canvasPosition[3]);
    document.getElementById('canvasR').getContext('2d').drawImage(
        dynamic, 
        imageR[0], imageR[1], imageR[2], imageR[3],
        canvasPosition[0], canvasPosition[1], canvasPosition[2], canvasPosition[3]);
}
