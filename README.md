
# Cursor Pack Installer

This is a simple installer to be packaged with cursor packs. It reduces the process of installing a 16-cursor pack from 40~ clicks to 1 double click



## Usage/Examples

The expected usage is for the installer and uninstaller to be packaged in a cursor pack in a similar structure to this.

```
    ./MyCursorPack/
    ../Config.json
    ../Installer.exe
    ../Uninstaller.exe
    ../Cursors/
    ../../base.ani
    ../../busy.ani
    .
    .
    .
    etc
```
And with a Config.json file like this. The cursor names are quite important to be exactly like this.

```javascript
{
   "CursorNameToFilePath": {
      "": "My Awesome Cursors",
      "AppStarting": "./Cursors/loading.ani",
      "Arrow": "./Cursors/base.ani",
      "Crosshair": "./Cursors/precision.ani",
      "Hand": "./Cursors/link.ani",
      "Help": "./Cursors/help.ani",
      "IBeam": "./Cursors/text.ani",
      "No": "./Cursors/unavailable.ani",
      "NWPen": "./Cursors/handwriting.ani",
      "Person": "./Cursors/person.ani",
      "Pin": "./Cursors/location.ani",
      "SizeAll": "./Cursors/move.ani",
      "SizeNESW": "./Cursors/resize_d2.ani",
      "SizeNS": "./Cursors/resize_v.ani",
      "SizeNWSE": "./Cursors/resize_d1.ani",
      "SizeWE": "./Cursors/resize_h.ani",
      "UpArrow": "./Cursors/alternate.ani",
      "Wait": "./Cursors/busy.ani"
   }
}
```
Installing or uninstalling your cursor pack after its configured like this is as simple as double clicking either Installer.exe or Uninstaller.exe relatively.

Moving or deleting the cursors after installation is likely to break the install so you are advised to warn your users about that.