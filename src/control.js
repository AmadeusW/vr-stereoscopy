var positionBase = [0, 0.1, -2.3]; // Base position of images
var positionOffset = [0, 0, 0]; // Offset controlled by user's head
var positionOffsetFactor = [1, -1, 1]; // how user's head motion translates into offset
var eyeDelta = [0, 0, 0]; // Per-eye offset for each image
var eyeDeltaStep = 0.05;
// TODO User defined origin for the camera position

render();

window.addEventListener("keydown", function(e){
    if(e.keyCode === 37) { // left
        eyeDelta[0] -= eyeDeltaStep;
        render();
    }
    if(e.keyCode === 39) { // right
        eyeDelta[0] += eyeDeltaStep;
        render();
    }
    if(e.keyCode === 38) { // up
        eyeDelta[1] += eyeDeltaStep;
        render();
    }
    if(e.keyCode === 40) { // down
        eyeDelta[1] -= eyeDeltaStep;
        render();
    }
});

function render()
{
    var positionR = (positionBase[0] + positionOffset[0]) + " " + (positionBase[1] + positionOffset[1]) + " " + (positionBase[2] + positionOffset[2]);
    var positionL = (positionBase[0] + positionOffset[0] + eyeDelta[0])
             + " " + (positionBase[1] + positionOffset[1] + eyeDelta[1])
             + " " + (positionBase[2] + positionOffset[2] + eyeDelta[2]);

    document.getElementById("leftPlane").setAttribute("position", positionL)
    document.getElementById("rightPlane").setAttribute("position", positionR)
}

const scene = document.querySelector('a-scene');
if (scene.hasLoaded) {
    subscribeToEvents();
} else {
    scene.addEventListener('loaded', subscribeToEvents);
}

function subscribeToEvents() {
    const p = document.querySelector("#camera");
    p.addEventListener('componentchanged', function (evt) {
        //console.log(evt.detail.name);
        if (evt.detail.name === 'rotation') {
            //console.log('Rotation from ', evt.detail.oldData, 'to', evt.detail.newData, '!');
            /*
            panX = evt.detail.newData.x * 0.1;
            panY = evt.detail.newData.y * 0.1;
            panZ = evt.detail.newData.z * 0.1;
            render();
            */
        }
        if (evt.detail.name === 'position') {
            //console.log('Movement from', evt.detail.oldData, 'to', evt.detail.newData, '!');
            positionOffset[0] = evt.detail.newData.x * positionOffsetFactor[0];
            positionOffset[1] = evt.detail.newData.y * positionOffsetFactor[1];
            positionOffset[2] = evt.detail.newData.z * positionOffsetFactor[2];
            render();
        }
    });
}