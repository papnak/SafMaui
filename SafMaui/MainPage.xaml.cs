using System.Diagnostics;
using File = System.IO.File;

namespace SafMaui;
public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}
    

    private void btnBrowse_Clicked(object sender, EventArgs e)
    {
#if ANDROID
        SafService service = new SafService();
        service.ShowUriBrowser();
#endif
    }

    private void btnCopyToExternalStorage_Clicked(object sender, EventArgs e)
    {
#if ANDROID
        string fileName = "AboutAssets.txt"; 
        String path = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, fileName);

        //File.Delete(path);
        //Example: If the file does not exist in the Internal Storage, get it from the Bundle-Resources
        if (!File.Exists(path))  CopyFileFromAppBundle(FileSystem.Current.AppDataDirectory, fileName);
        //var y = System.IO.Directory.GetFiles(FileSystem.Current.AppDataDirectory);

        SafService service = new SafService();
        service.CopyToExternalStorage(FileSystem.Current.AppDataDirectory, fileName);
#endif
    }

    private async void btnCopyFromExternalStorage_Clicked(object sender, EventArgs e)
    {
#if ANDROID
        var picked =  await PickAndShow(default);

        if (picked != null)
        {
            string targetFileName = picked.FileName;
            string targetPath = FileSystem.Current.AppDataDirectory;
            File.Delete(System.IO.Path.Combine(targetPath, targetFileName));
            SafService service = new SafService();
            service.CopyFromExternalStorage(FileSystem.Current.AppDataDirectory, targetFileName);
            //var x = File.Exists(System.IO.Path.Combine(targetPath, targetFileName));
        }
#endif
    }

    // Copy a (binary) file from the app bundle (Resources/Raw) to destPath
    public async Task CopyFileFromAppBundle(string destPath, string targetFileName)
    {
        var destpath = Path.Combine(destPath, targetFileName);
        using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(targetFileName);

        //var x = System.IO.Directory.GetFiles(FileSystem.Current.AppDataDirectory);

        File.Delete(destpath);

        if (!File.Exists(destpath))
        {
            using (var binaryReader = new BinaryReader(fileStream))
            {
                var allBytes = binaryReader.ReadBytes((int)fileStream.Length);
                using (var binaryWriter = new BinaryWriter(new FileStream(destpath, FileMode.Create)))
                {
                    binaryWriter.Write(allBytes);
                }

                //using (var binaryWriter = new BinaryWriter(new FileStream(destpath, FileMode.Create)))
                //{
                //    byte[] buffer = new byte[2048];
                //    int length = 0;
                //    while ((length = binaryReader.Read(buffer, 0, buffer.Length)) > 0)
                //    {
                //        binaryWriter.Write(buffer, 0, length);
                //    }
                //}
            }

            //var y = System.IO.Directory.GetFiles(FileSystem.Current.AppDataDirectory);
        }
        else
        {
            Debug.Print(destpath + " already saved on device");
        }
    }


    public async Task<FileResult> PickAndShow(PickOptions options)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(options);
            //if (result != null)
            //{
            //    if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
            //        result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
            //    {
            //        using var stream = await result.OpenReadAsync();
            //        var image = ImageSource.FromStream(() => stream);
            //    }
            //}

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return null;
    }

    //private async void btnPickAndShow_Clicked(object sender, EventArgs e)
    //{
    //    await PickAndShow(default);

    //}

}

