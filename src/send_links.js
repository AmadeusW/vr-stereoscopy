// Copyright (c) 2012 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

// Send back to the popup a sorted deduped list of valid link URLs on this page.
// The popup injects this script into all frames in the active tab.

var links = [].slice
.apply(document.getElementsByTagName('a'))
.filter(function(element) {
  console.log("examining a. " + element.className);
  var self = element.className.indexOf('self') >= 0 
  var thumbnail = element.className.indexOf('thumbnail') >= 0
  return !self && thumbnail;
})
.map(function(element) {
  // Return an anchor's href attribute, stripping any URL fragment (hash '#').
  // If the html specifies a relative path, chrome converts it to an absolute
  // URL.
  var href = element.href;
  var hashIndex = href.indexOf('#');
  if (hashIndex >= 0) {
    href = href.substr(0, hashIndex);
  }
  return href;
});

chrome.extension.sendRequest(links);
