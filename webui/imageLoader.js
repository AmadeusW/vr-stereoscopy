// This file helps reduce flicker and discomfort when changing pictures
// by asynchronously loading new pictures, 
//  dimming the current picture 
//  and revealing the new picture only when it's fully loaded
class ImageLoader {

    constructor() {
        this.tempImageL = new Image();
        this.tempImageR = new Image();
        tempImageL.onload = onImageLoaded;
        tempImageR.onload = onImageLoaded;
        this.imagesLoaded = 0;
    }

    load = function(srcL, srcR, callback) {
        this.imagesLoaded = 0;
        this.tempImageL.src = srcL;
        this.tempImageR.src = srcR;
        this.callback = callback;
        document.getElementById("rightPlane").setAttribute("visibile", "false");
    }

    onImageLoaded = function() {
        imagesLoaded++;
        if (imagesLoaded == 2) {
            onSceneLoaded();
        }
    }

    onSceneLoaded = function() {
        document.getElementById("leftPlane").setAttribute("src", tempImageL.src);
        document.getElementById("rightPlane").setAttribute("src", tempImageR.src);
        document.getElementById("rightPlane").setAttribute("visibile", "true");
        this.callback();
    }
}