using Android.OS;
using PortableStorage.Droid;
using File = System.IO.File;
using Debug = System.Diagnostics.Debug;

// SafService.cs Android Platform

namespace SafMaui
{
    public partial class SafService
    {
        public static Uri StorageUri
        {
            get
            {   var Value = Preferences.Get("StorageUri", null);
                return Value != null ? new Uri(Value) : null;
            }
            set => Preferences.Set("StorageUri", value.ToString());
        }

        public partial void ShowUriBrowser()
        {
            try
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                    SafStorageHelper.BrowserFolder(MainActivity.Instance, MainActivity.BROWSE_REQUEST_CODE); // >= API Level 24
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public partial void CopyToExternalStorage(string intPath, string fname)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N) // >= API Level 24
            {
                try
                {
                    // Falls es noch keine Uri gibt                
                    if (StorageUri == null) SafStorageHelper.BrowserFolder(MainActivity.Instance, MainActivity.BROWSE_REQUEST_CODE);

                    var sourcepath = Path.Combine(intPath, fname);

                    if (File.Exists(sourcepath))
                    {
                        var stream = File.Open(sourcepath, FileMode.Open);
                        var sr = new BinaryReader(stream);
                        var allBytes = sr.ReadBytes((int)stream.Length);

                        var externalStorage = SafStorgeProvider.CreateStorage(MainActivity.Instance, StorageUri);
                        externalStorage.WriteAllBytes(fname, allBytes);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }



        public partial void CopyFromExternalStorage(string intPath, string fname)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N) // >= API Level 24
            {
                try
                {
                    if (StorageUri == null) SafStorageHelper.BrowserFolder(MainActivity.Instance, MainActivity.BROWSE_REQUEST_CODE);

                    var externalStorage = SafStorgeProvider.CreateStorage(MainActivity.Instance, StorageUri);
                    var allBytes = externalStorage.ReadAllBytes(fname);

                    var destpath = Path.Combine(intPath, fname);
                    using (var stream = File.Open(destpath, FileMode.Create))
                    {
                        using (var sr = new BinaryWriter(stream))
                        {
                            sr.Write(allBytes);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
    }
}
