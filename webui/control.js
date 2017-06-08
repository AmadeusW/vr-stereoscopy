var positionBase = [0, 0, -9]; // Base position of images
var eyeDelta = [0, 0, 0]; // Per-eye offset for each image
var eyeDeltaStep = 1;
var positionHead = [0, 0, 0]; // raw value for head's position
var positionOrigin = [0, 0, 0]; // origin head position
var positionOffset = [0, 0, 0]; // Both-eye offset controlled by user's head position
var rotationHead = [0, 0, 0]; // raw value for head's rotation
var rotationOrigin = [0, 0, 0]; // origin head rotation
var rotationOffset = [0, 0, 0]; // Both-eye offset controlled by user's head rotation
var positionOffsetFactor = [0, 0, 0]; // how user's head position translates into image offset
var rotationOffsetFactor = [4, -4, 0]; // how user's head rotation translates into image offset
var loadedImage = -1;
var currentImage = 0;
var currentThumbL = 0;
var currentThumbR = 0;
var lastImage = 0;
var timeoutId;
var allCategories = [];
var imagePathPrefix = "";

initialize();

async function initialize() {
    initializeAFrame();
    var categories = initializeMenu();
    buildMenu(await categories);
    allCategories = await categories;
    console.log(allCategories);
    await goToCategory(0, 0); // Initial category to display
    //scenes = await initializeCategory((await categories)[0].Subcategories[0].Feed)
    showMenu();
    showImage();
    render();
}

async function goToCategory(categoryId, subcategoryId) {
    // TODO: I need a proper framework instead of storing data in global variables
    scenes = await initializeCategory((allCategories)[categoryId].Subcategories[subcategoryId].Feed);
    imagePathPrefix = (allCategories)[categoryId].Subcategories[subcategoryId].ImagePathPrefix;
    lastImage = scenes.length - 1;
    currentThumbR = 1;
    currentThumbL = lastImage;
    currentImage = 0;
    loadedImage = -1;
    console.log("Going to ", categoryId, subcategoryId, "; Loaded scenes: ", scenes, "; Prefix: ", imagePathPrefix);
    render();
}

function initializeAFrame() {
    const scene = document.querySelector('a-scene');
    if (scene.hasLoaded) {
        subscribeToEvents();
    } else {
        scene.addEventListener('loaded', subscribeToEvents);
    }
}

function showImage() {
    document.getElementById("mainPane").setAttribute("visible", true);
}

function render() {
    if (loadedImage != currentImage)
    {
        var imageId = scenes[currentImage].Link;
        console.log("Loading new image: " + imageId);
        document.getElementById("imageTitle").setAttribute("value", scenes[currentImage].Title);

        document.getElementById("leftPlane").setAttribute("width", Math.pow(2, scenes[currentImage].W))
        document.getElementById("leftPlane").setAttribute("height", Math.pow(2, scenes[currentImage].H))
        document.getElementById("rightPlane").setAttribute("width", Math.pow(2, scenes[currentImage].W))
        document.getElementById("rightPlane").setAttribute("height", Math.pow(2, scenes[currentImage].H))
        positionBase[2] = -Math.pow(1.88, scenes[currentImage].W); // this will update the distance

        document.getElementById("leftPlane").setAttribute("src", "images/" + imagePathPrefix + imageId + ".L.jpg")
        document.getElementById("rightPlane").setAttribute("src", "images/" + imagePathPrefix + imageId + ".R.jpg")

        document.getElementById("scrollLThumbL").setAttribute("src", "images/" + imagePathPrefix + scenes[currentThumbL].Link + ".L.jpg")
        document.getElementById("scrollLThumbR").setAttribute("src", "images/" + imagePathPrefix + scenes[currentThumbL].Link + ".R.jpg")
        document.getElementById("scrollRThumbL").setAttribute("src", "images/" + imagePathPrefix + scenes[currentThumbR].Link + ".L.jpg")
        document.getElementById("scrollRThumbR").setAttribute("src", "images/" + imagePathPrefix + scenes[currentThumbR].Link + ".R.jpg")

        eyeDelta = [-43, 0, 0];
        loadedImage = currentImage;
    }
    var positionR = (positionBase[0] + positionOffset[0] + rotationOffset[0])
             + " " + (positionBase[1] + positionOffset[1] + rotationOffset[1]) 
             + " " + (positionBase[2] + positionOffset[2] + rotationOffset[2]);
    var positionL = (positionBase[0] + positionOffset[0] + rotationOffset[0] + eyeDelta[0])
             + " " + (positionBase[1] + positionOffset[1] + rotationOffset[1] + eyeDelta[1])
             + " " + (positionBase[2] + positionOffset[2] + rotationOffset[2] + eyeDelta[2]);

    document.getElementById("leftPlane").setAttribute("position", positionL)
    document.getElementById("rightPlane").setAttribute("position", positionR)
}

function subscribeToEvents() {
    //setTimer();
    const p = document.querySelector("#camera");
    p.addEventListener('componentchanged', function (evt) {
        //console.log(evt.detail.name);
        if (evt.detail.name === 'rotation') {
            //console.log('Rotation from ', evt.detail.oldData, 'to', evt.detail.newData, '!');
            rotationHead[0] = evt.detail.newData.y;
            rotationHead[1] = evt.detail.newData.x;
            rotationHead[2] = evt.detail.newData.z; // TODO: consider using as image plane rotation
            rotationOffset[0] = (rotationHead[0] - rotationOrigin[0]) * rotationOffsetFactor[0];
            rotationOffset[1] = (rotationHead[1] - rotationOrigin[1]) * rotationOffsetFactor[1];
            rotationOffset[2] = (rotationHead[2] - rotationOrigin[2]) * rotationOffsetFactor[2];
            render();
        }
        /*
        if (evt.detail.name === 'position') {
            //console.log('Movement from', evt.detail.oldData, 'to', evt.detail.newData, '!');
            positionHead[0] = evt.detail.newData.x;
            positionHead[1] = evt.detail.newData.y;
            positionHead[2] = evt.detail.newData.z;
            positionOffset[0] = (positionHead[0] - positionOrigin[0]) * positionOffsetFactor[0];
            positionOffset[1] = (positionHead[1] - positionOrigin[1]) * positionOffsetFactor[1];
            positionOffset[2] = (positionHead[2] - positionOrigin[2]) * positionOffsetFactor[2];
            render();
        }*/
    });
}

function resetPosition() {
    positionOrigin[0] = positionHead[0];
    positionOrigin[1] = positionHead[1];
    positionOrigin[2] = positionHead[2];
    rotationOrigin[0] = rotationHead[0];
    rotationOrigin[1] = rotationHead[1];
    rotationOrigin[2] = rotationHead[2];
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
        toggleTimer();
    }
    if(e.keyCode === 78) { // n
        var elements = document.querySelectorAll(".categoryThumb");
        for (var i = 0; i < elements.length; i++) {
            elements[i].emit("grow");
        };
        nextImage();
    }
    if(e.keyCode === 32) { // space
        nextImage();
    }
    if(e.keyCode === 80) { // p
        previousImage();
    }
    if(e.keyCode === 13) { // enter
        showMenu();
    }
    if(e.keyCode === 27) { // esc
        hideMenu();
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
    console.info("Next image");
    currentImage = getNextIndex(currentImage);
    currentThumbR = getNextIndex(currentThumbR);
    currentThumbL = getNextIndex(currentThumbL);
    render();
}

function previousImage() {
    console.info("Previous image");
    currentImage = getPreviousIndex(currentImage);
    currentThumbR = getPreviousIndex(currentThumbR);
    currentThumbL = getPreviousIndex(currentThumbL);
    render();
}

function getNextIndex(value) {
    return value < lastImage ? ++value : 0;
}

function getPreviousIndex(value) {
    return value > 0 ? --value : lastImage;
}

function toggleTimer() {
    if (timeoutId == null) {
        console.log("Enable timer");
        setTimer();
    } else {
        console.log("Disable timer");
        window.clearTimeout(timeoutId);
        timeoutId = null;
    }
}