AFRAME.registerComponent('listener', {
  multiple: true,
  schema: {
    event: {type: 'string' },
    callback: {type: 'string', default: null},
    params: {type: 'array', default: []}
  },
  init: function () {
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