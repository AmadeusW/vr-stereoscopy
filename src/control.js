// Base position of images
var baseX = 0;
var baseY = 0.1;
var baseZ = -2.3;
// Offset controlled by user's head
var panX = 0;
var panY = 0;
var panZ = 0;
// Per-eye offset for each image
var deltaX = 0;
var deltaY = 0;
var deltaZ = 0;
var step = 0.05;

render();

window.addEventListener("keydown", function(e){
    if(e.keyCode === 37) { // left
        deltaX -= step;
        render();
    }
    if(e.keyCode === 39) { // right
        deltaX += step;
        render();
    }
    if(e.keyCode === 38) { // up
        deltaY += step;
        render();
    }
    if(e.keyCode === 40) { // down
        deltaY -= step;
        render();
    }
});

function render()
{
    var positionR = (baseX + panX) + " " + (baseY + panY) + " " + (baseZ + panZ) + " ";
    var positionL = (baseX + panX + deltaX) + " " + (baseY + panY + deltaY) + " " + (baseZ + panZ + deltaZ) + " ";
    console.log(positionL + "; " + positionR);
    document.getElementById("leftPlane").setAttribute("position", positionL)
    document.getElementById("rightPlane").setAttribute("position", positionR)
}