var dynamicL = new Image();
dynamicL.src = 'images/l.jpg';
dynamicL.onload = function() {
    console.log("loaded");
    document.getElementById('canvasL').getContext('2d').drawImage(dynamicL, 0, 0, 256, 512, 0, 0, 256, 512);
}
var dynamicL = new Image();
dynamicL.src = 'images/r.jpg'; 
dynamicL.onload = function() {
    console.log("loaded");
    document.getElementById('canvasR').getContext('2d').drawImage(dynamicL, 256, 0, 256, 512, 0, 0, 256, 512);
}