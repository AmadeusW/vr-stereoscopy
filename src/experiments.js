var dynamic = new Image();
dynamic.src = 'http://orig00.deviantart.net/8afe/f/2012/092/1/4/parish_church_st__georg_3d_____cross_eye_hdr_by_zour-d4upy31.jpg';
dynamic.onload = function() {
    console.log("loaded");
    document.getElementById('canvasL').getContext('2d').drawImage(dynamic, 0, 0, 700, 1080, 0, 0, 256, 256);
    document.getElementById('canvasR').getContext('2d').drawImage(dynamic, 700, 0, 700, 1080, 0, 0, 256, 256);
}
