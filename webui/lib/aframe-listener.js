AFRAME.registerComponent('listener', {
  schema: {
    callback: {type: 'string', default: "a"},
    params: {type: 'string', default: "x"}
  },
  init: function () {
    this.el.addEventListener('click', function (ev) {
      console.log(this.data); // undefined
      var data = this.getAttribute('listener'); // because this.data doesn't work...
      console.log('My params: ', data.params);
      this.emit('grow'); // lets see
    });
  },
  update: function () {
    console.info(this.data); // works here
  }
});