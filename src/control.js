var baseX = 0;
var baseY = 1.5;
var baseZ = -2.3;
var deltaX = 0;
var deltaY = 0;
var deltaZ = 0;
var step = 0.01;

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
        deltaZ -= step;
        render();
    }
    if(e.keyCode === 40) { // down
        deltaZ -= step;
        render();
    }
});

function render()
{
    var position = baseX + deltaX + " " + baseY + deltaY + " " + baseZ + deltaZ + " ";
    console.log(position);
    document.getElementById("leftPlane").setAttribute("position", position)
}