// This file helps reduce flicker and discomfort when changing pictures
// by asynchronously loading new pictures, 
//  dimming the current picture 
//  and revealing the new picture only when it's fully loaded
preloadImage = function(srcL, srcR) {
    tempImagesLoaded = 0;
    tempImageL.src = srcL;
    tempImageR.src = srcR;
    document.getElementById("leftPlane").setAttribute("visibile", "false");
    document.getElementById("rightPlane").setAttribute("visibile", "false");
}

onTempImageLoaded = function() {
    tempImagesLoaded++;
    if (tempImagesLoaded == 2) {
        document.getElementById("leftPlane").setAttribute("visibile", "true");
        document.getElementById("rightPlane").setAttribute("visibile", "true");
        render();
    }
}

tempImageL = new Image();
tempImageR = new Image();
tempImageL.onload = onTempImageLoaded;
tempImageR.onload = onTempImageLoaded;
tempImagesLoaded = 0;
