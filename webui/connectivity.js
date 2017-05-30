var getJSON = function(url, callback) {
    /* from https://stackoverflow.com/questions/12460378/how-to-get-json-from-url-in-javascript */
    var xhr = new XMLHttpRequest();
    xhr.open('GET', url, true);
    xhr.responseType = 'json';
    xhr.onload = function() {
      var status = xhr.status;
      if (status == 200) {
        console.log("success");
        console.log(xhr);
        callback(null, xhr.response);
      } else {
        console.log("error: " + status);
        callback(status);
      }
    };
    console.log("sending...");
    xhr.send();
};