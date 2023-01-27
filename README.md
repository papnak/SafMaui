### How to read/write from/to External Storage with .NET MAUI?  

Definition:

**Internal Storage**: An Android App lives in a sandbox. Only the app can read and write data from/to this sandbox. 
**External Storage**: Is shared storage e.g. the documents or downloads folder and OTG, cloud and SD cards as well.

But how to access External Storage?

This works with the **Storage Access Framework (SAF)**
Google introduced it with Android 4.4 (API level 19)
Starting with API Level 24 Android 7 Nougat the SAF is the only way to access external memory.

Here you can find some infos about SAF

https://commonsware.com/Jetpack/pages/chap-content-001.html

https://developer.android.com/guide/topics/providers/document-provider

https://developer.android.com/training/data-storage/shared/documents-files

I was chaseing the Internet for a SAF wrapper for .Net Maui and found a package of

Mohammad Nikravan (madnik7) on: https://github.com/madnik7/PortableStorage

madnik7 has done a really good job. Unfortunatelly it is barely commented and documented.

In my example I concentrate on two things:

- Copy a file from Internal Storage (e.g. FileSystem.Current.AppDataDirectory) to the external Storage (e.g /storage/emulated/0/Download).
- Copy a file from External Storage to the Internal Storage.

You need to include the NuGet PortableStorage.Android package.

The SAF is URI based. To retrieve an URI the SAF provides a browser. With this browser the user selects a path in the External Storage.
The browser returns an URI which can be stored for later usage. This needs to be done once by the user but can be repeated whenever the destination changes. 
There is no way (afaik) to generate a default URI by code to the e.g. downloads folder to omit usage of the browser. If anybody out there knows a way....

To call the methods in platform code from the shared project the successor of the Dependency Service is used. See both SafService.cs in the shared project and the platform project.

Here you can find some infos about that here:
https://blog.taranissoftware.com/platform-specific-code-using-partial-classes-in-net-maui
https://www.davidbritch.com/search?q=dependencyservice

A Maui built in way to access (only reading) external files is the FilePicker in Maui. No SAF is needed (only Permissions.StorageRead). See my example FilePickerTest