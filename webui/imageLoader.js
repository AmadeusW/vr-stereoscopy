// This file helps reduce flicker and discomfort when changing pictures
// by asynchronously loading new pictures, 
//  dimming the current picture 
//  and revealing the new picture only when it's fully loaded
preloadImage = function(srcL, srcR) {
    tempImagesLoaded = 0;
    tempImageL.src = srcL;
    tempImageR.src = srcR;
    document.getElementById("leftPlane").setAttribute("visible", "false");
    document.getElementById("rightPlane").setAttribute("visible", "false");
}

onTempImageLoaded = function() {
    tempImagesLoaded++;
    if (tempImagesLoaded == 2) {
        render();
        setTimeout(function() {
            if (tempImagesLoaded == 2) { // 2 means reloadImage wasn't called during timeout
                document.getElementById("leftPlane").setAttribute("visible", "true");
                document.getElementById("rightPlane").setAttribute("visible", "true");
            }
        }, 150)
    }
}

onActualImageLoaded = function() {
    console.log("onActualImageLoaded");
}

tempImageL = new Image();
tempImageR = new Image();
// NOTE: Ideally, I would use document.getElementById("leftPlane").materialtextureloaded
// but this event doesn't seem to be fired, even if I update the material on render like so:
// document.getElementById("leftPlane").setAttribute("material", "src: url(...)")
// This means that instead of reacting to when image actually changes,
// we get to react to when tempImage loads, hoping that web browser caches the image
// and lets leftPlane load immediately (we give it small timeout to redraw)
tempImageL.onload = onTempImageLoaded;
tempImageR.onload = onTempImageLoaded;
document.getElementById("leftPlane").materialtextureloaded  = onActualImageLoaded;
document.getElementById("rightPlane").materialtextureloaded  = onActualImageLoaded;
tempImagesLoaded = 0;
