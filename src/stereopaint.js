// todo: draw on canvas component
AFRAME.registerComponent("stereopaint", {
    dependencies: ["draw"],

    update: function() {
        console.log("stereo paint")
        var draw = this.el.components.draw; //get access to the draw component
        paint(this.data.url, draw);
        draw.render(); //tell it to update the texture
    }
});

function paint(url, draw) {
    var dynamic = new Image();
    dynamic.src = url;
    dynamic.onload = function() {
        console.log("loaded");
        var imageL = [0, 0, 0, 0]; // x, y, width, height
        var imageR = [0, 0, 0, 0]; // x, y, width, height
        if (1 == 1) { // todo: get selected scene's crop property
            imageL = [0, 0, dynamic.width/2, dynamic.height];
            imageR = [dynamic.width/2, 0, dynamic.width/2, dynamic.height];
        }
        else {
            console.log("not implemented");
        }
        var canvasWidth = sizeup(dynamic.width/2);
        var canvasHeight = sizeup(dynamic.height);
        var deltaWidth = canvasWidth - dynamic.width / 2;
        var deltaHeight = canvasHeight - dynamic.height;
        var canvasPosition = [deltaWidth/2, deltaHeight/2, dynamic.width/2, dynamic.height]; // x, y, width, height

        console.log(canvasWidth);
        console.log(canvasHeight);
        console.log(deltaWidth);
        console.log(deltaHeight);
        console.log(JSON.stringify(canvasPosition));

        // TODO: Learn how to correctly access properties of DRAW https://github.com/maxkrieger/aframe-draw-component
        draw.width = canvasWidth;
        draw.height = canvasHeight;
        draw.background = "#111";

        // todo: fill canvas with dark gray color
        draw.ctx.drawImage(
            dynamic, 
            imageL[0], imageL[1], imageL[2], imageL[3],
            canvasPosition[0], canvasPosition[1], canvasPosition[2], canvasPosition[3]);
        /* below is code for rendering second picture
        document.getElementById('canvasR').getContext('2d').drawImage(
            dynamic, 
            imageR[0], imageR[1], imageR[2], imageR[3],
            canvasPosition[0], canvasPosition[1], canvasPosition[2], canvasPosition[3]);
        */
    }
}

function sizeup(x) {
    var r = Math.pow(2, Math.ceil(Math.log2(x)));
    console.log(x + " > " + r)
    return r;
}
