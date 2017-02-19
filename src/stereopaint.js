// todo: draw on canvas component
AFRAME.registerComponent("stereopaint", {
    dependencies: ["draw"],
    schema: {
        url: {default: ''}
    },
    init: function() {
        console.log("init");
        this.draw = this.el.components.draw;
        this.draw.register(this.render.bind(this));
    },
    update: function() {
        console.log("update");
        this.draw.render(); //tell it to update the texture
    },
    render: function() { //TODO: when everything works, see if I can remove this function
        console.log("render")
        paint(this.data.url, this.draw)
    }
});

function paint(url, draw) {
    console.log("paint with ");
    console.log(draw.ctx);
    console.log(draw.canvas);
    var dynamic = new Image();
    dynamic.src = url;
    dynamic.onload = function() {
        console.log("loaded " + dynamic.src);
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
        canvasPosition = [0, 0, 256, 256];

        console.log("Canvas:");
        console.log(canvasWidth);
        console.log(canvasHeight);
        console.log("Delta:");
        console.log(deltaWidth);
        console.log(deltaHeight);
        console.log("Image:");
        console.log(imageL);
        console.log(imageR);
        console.log(JSON.stringify(canvasPosition));

        // TODO: Learn how to correctly access properties of DRAW https://github.com/maxkrieger/aframe-draw-component
        draw.width = canvasWidth;
        draw.height = canvasHeight;
        draw.background = "#111";

        // todo: fill canvas with dark gray color
        draw.ctx.fillStyle = "#AFC5FF";
        draw.ctx.fillRect(0, 0, 256, 256);
        /*
        draw.ctx.drawImage(
            dynamic, 
            imageL[0], imageL[1], imageL[2], imageL[3],
            canvasPosition[0], canvasPosition[1], canvasPosition[2], canvasPosition[3]);
        */
        document.getElementById('canvasL').getContext('2d').drawImage(
            dynamic, 
            imageR[0], imageR[1], imageR[2], imageR[3],
            canvasPosition[0], canvasPosition[1], canvasPosition[2], canvasPosition[3]);
        console.log("finished")
    }
}

function sizeup(x) {
    var r = Math.pow(2, Math.ceil(Math.log2(x)));
    console.log(x + " > " + r)
    return r;
}
