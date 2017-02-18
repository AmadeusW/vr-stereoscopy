var dynamicL = new Image();
dynamicL.src = 'http://orig00.deviantart.net/8afe/f/2012/092/1/4/parish_church_st__georg_3d_____cross_eye_hdr_by_zour-d4upy31.jpg';
dynamicL.onload = function() {
    console.log("loaded");
    document.getElementById('canvasL').getContext('2d').drawImage(dynamicL, 0, 0, 256, 512, 0, 0, 256, 512);
}
var dynamicL = new Image();
dynamicL.src = 'http://orig00.deviantart.net/8afe/f/2012/092/1/4/parish_church_st__georg_3d_____cross_eye_hdr_by_zour-d4upy31.jpg'; 
dynamicL.onload = function() {
    console.log("loaded");
    document.getElementById('canvasR').getContext('2d').drawImage(dynamicL, 256, 0, 256, 512, 0, 0, 256, 512);
}