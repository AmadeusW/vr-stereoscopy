  var select = function(sender, params) {
    sender.emit('grow');
  };
  var deselect = function(sender, params) {
    sender.emit('shrink');
  };

AFRAME.registerComponent('listener', {
  multiple: true,
  schema: {
    event: {type: 'string' },
    callback: {type: 'string', default: null},
    params: {type: 'array', default: []}
  },
  init: function () {
    console.log('We are ', this.id);
    var hack = this;
    this.el.addEventListener(this.data.event, function (ev) {
      var fn = window[hack.data.callback];
      if(typeof fn === 'function') {
        console.log('Calling function ', fn, ' on ', hack.el, ' with params ', hack.data.params);
        fn(hack.el, hack.data.params);
      }
    });
  },
});