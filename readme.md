# Virtual Reality Cross View
See stereoscopic *cross view* images in VR

[Check it out](http://amadeusw.com/vr-stereoscopy/webui/)
[Source code](https://github.com/AmadeusW/vr-stereoscopy/)

Controls:

* Next: `Controller buttons` `N`, `space`
* Previous: `other controller button`, `P`
* Slideshow: `T`

Dev controls:

* Adjust relative positioning of left and right images: `arrow keys`
* Log relative positions: `C`
* Reset relative positions: `R`

## Future improvements:
- Go around the cross origin policy and display any image from the internet, without the need of hosting it
- Automatically update the gallery with each month's top voted images

## Capabilities:
This project scrapes links from reddit's /r/crossview, resizes to achieve dimensions that are a power of 2, and uploads images to CDN to prevent CORS issues.
imgur, reddit and flickr are supported.

## Credits:
- photographers. *Each photo has a link to the original reddit submission*
- [oscarmarinmiro/aframe-stereo-component](https://github.com/oscarmarinmiro/aframe-stereo-component)
- [aframevr/aframe](https://github.com/aframevr/aframe)
