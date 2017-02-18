// todo: draw on canvas component
AFRAME.registerComponent("stereopaint", {
    dependencies: ["draw"],

    update: function() {
        console.log("stereo paint")
        var draw = this.el.components.draw; //get access to the draw component
        var ctx = draw.ctx;
        var canvas = draw.canvas;
        ctx.fillStyle = "#AFC5FF";
        ctx.fillRect(0, 0, canvas.width, canvas.height);
        ctx.fillStyle = "blue";
        ctx.fillRect(68, 68, 120, 120);
        ctx.fillStyle = "white";
        ctx.font = "36px Georgia";
        ctx.fillText(this.data.url, 80, 140);
        draw.render(); //tell it to update the texture
    }
});
