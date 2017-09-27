# Virtual Reality Cross View
See stereoscopic *cross view* images in VR

[Source code](https://github.com/AmadeusW/vr-stereoscopy/)

## Demo
[Early demo](http://amadeusw.com/vr-stereoscopy/webui/)

User controls:

* Controller buttons - next, previous
* N, space - next
* P - preview
* T - slideshow

Dev controls:

* arrow keys - adjust alignment of the image
* C - log current alignment of the image
* R - reset alignment

## Future improvements:
- Go around the cross origin policy and display any image from the internet
- Automatically update the gallery with each month's top picks

## Capabilities:
This project scrapes links from reddit's /r/crossview, resizes to achieve dimensions that are a power of 2, and uploads images to CDN so that WebVR will display it without CORS issues.
imgur, reddit and flickr are supported.

## Credits:
- photographers. *Each photo has a link to the original reddit submission*
- [oscarmarinmiro/aframe-stereo-component](https://github.com/oscarmarinmiro/aframe-stereo-component)
- [aframevr/aframe](https://github.com/aframevr/aframe)
