  var select = function(obj) {
    var data = obj.getAttribute('listener'); // because this.data doesn't work...
    console.log('Select. My params: ', data.params);
    obj.emit('grow');
  };
  var deselect = function(obj) {
    var data = obj.getAttribute('listener'); // because this.data doesn't work...
    console.log('Deselect. My params: ', data.params);
    obj.emit('shrink');
  };

AFRAME.registerComponent('listener', {
  schema: {
    event: {type: 'string' },
    callback: {type: 'string', default: null},
    params: {type: 'array', default: []}
  },
  init: function () {
    this.el.addEventListener('click', function (ev) {
      /*
      var fn = window[callback];
if(typeof fn === 'function') {
    fn(this, params);
}*/
      select(this);
    });
    this.el.addEventListener('mouseenter', function (ev) {
      select(this);
    });
    this.el.addEventListener('mouseleave', function (ev) { /* todo: does it cover the case of fusing? */
      deselect(this);
    });
  },
});