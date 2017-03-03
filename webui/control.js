var positionBase = [0, 0.1, -2.3]; // Base position of images
var eyeDelta = [0, 0, 0]; // Per-eye offset for each image
var eyeDeltaStep = 0.05;
var positionHead = [0, 0, 0]; // raw value for head's position
var positionOrigin = [0, 0, 0]; // origin head position
var positionOffset = [0, 0, 0]; // Both-eye offset controlled by user's head
var positionOffsetFactor = [1, -1, 1]; // how user's head motion translates into offset
var loadedImage = 0;
var currentImage = 0;
var lastImage = 0;
var timeoutId;

render();
// Load data
console.log(JSON.stringify(scenes));
lastImage = scenes.length - 1;
const scene = document.querySelector('a-scene');
if (scene.hasLoaded) {
    subscribeToEvents();
} else {
    scene.addEventListener('loaded', subscribeToEvents);
}

function render() {
    if (loadedImage != currentImage)
    {
        var imageId = scenes[currentImage].Link;
        console.log("Loading new image: " + imageId);
        document.getElementById("leftImage").src = "images/" + imageId + ".L.jpg";
        document.getElementById("rightImage").src = "images/" + imageId + ".R.jpg";
        document.getElementById("leftPlane").setAttribute("src", "images/" + imageId + ".L.jpg")
        document.getElementById("rightPlane").setAttribute("src", "images/" + imageId + ".R.jpg")
        loadedImage = currentImage;
    }
    var positionR = (positionBase[0] + positionOffset[0]) + " " + (positionBase[1] + positionOffset[1]) + " " + (positionBase[2] + positionOffset[2]);
    var positionL = (positionBase[0] + positionOffset[0] + eyeDelta[0])
             + " " + (positionBase[1] + positionOffset[1] + eyeDelta[1])
             + " " + (positionBase[2] + positionOffset[2] + eyeDelta[2]);

    document.getElementById("leftPlane").setAttribute("position", positionL)
    document.getElementById("rightPlane").setAttribute("position", positionR)
}

function subscribeToEvents() {
    setTimer();
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
            positionHead[0] = evt.detail.newData.x;
            positionHead[1] = evt.detail.newData.y;
            positionHead[2] = evt.detail.newData.z;

            positionOffset[0] = (positionHead[0] - positionOrigin[0]) * positionOffsetFactor[0];
            positionOffset[1] = (positionHead[1] - positionOrigin[1]) * positionOffsetFactor[1];
            positionOffset[2] = (positionHead[2] - positionOrigin[2]) * positionOffsetFactor[2];
            render();
        }
    });
}

function resetPosition() {
    positionOrigin[0] = positionHead[0];
    positionOrigin[1] = positionHead[1];
    positionOrigin[2] = positionHead[2];
}

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
    if(e.keyCode === 82) { // r
        resetPosition();
        render();
    }
    if(e.keyCode === 84) { // t
        if (timeoutId == null) {
            console.log("Enable timer");
            setTimer();
        } else {
            console.log("Disable timer");
            window.clearTimeout(timeoutId);
            timeoutId = null;
        }
    }
    if(e.keyCode === 78) { // n
        nextImage();
    }
    if(e.keyCode === 32) { // space
        nextImage();
    }
    if(e.keyCode === 80) { // p
        previousImage();
    }
});

function setTimer() {
    timeoutId = window.setTimeout(nextImageByTimer, 5000);
}

function nextImageByTimer() {
    nextImage();
    setTimer();
}

function nextImage() {
    console.log("Next image");
    if (currentImage < lastImage) {
        currentImage++;
    } else {
        currentImage = 0;
    }
    render();
}

function previousImage() {
    console.log("Previous image");
    if (currentImage > 0) {
        currentImage--;
    } else {
        currentImage = lastImage;
    }
    render();
}