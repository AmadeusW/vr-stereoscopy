var baseX = 0;
var baseY = 0.1;
var baseZ = -2.3;
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
    var positionR = baseX + " " + baseY + " " + baseZ + " ";
    var positionL = (baseX + deltaX) + " " + (baseY + deltaY) + " " + (baseZ + deltaZ) + " ";
    console.log(positionL + "; " + positionR);
    document.getElementById("leftPlane").setAttribute("position", positionL)
    document.getElementById("rightPlane").setAttribute("position", positionR)
}