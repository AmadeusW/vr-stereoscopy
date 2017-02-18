var canvas = document.getElementById('canvas'),
context = canvas.getContext('2d');
var dynamic = new Image();
dynamic.src = 'images/l.jpg';        
dynamic.onload = function() {
    console.log("loaded");
    context.drawImage(dynamic, 0, 0, 512, 512);
}