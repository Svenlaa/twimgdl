# twimgdl

TwimgDL are an extension and C# command line application in order to easily save images on [Twitter](https://twitter.com), including the last write time.

## host (api)

This is a C# command line app that will live on your system. Make sure to update the folder in which you want your images saved. The API downloads the files, and updates the LastWriteTime to the time the image has been created.

## extension

The extension adds a save button to images on twitter. You can see the button if you hover over the image. If you click the save button, a request will be sent over to the `host` C# app, with the image id.
