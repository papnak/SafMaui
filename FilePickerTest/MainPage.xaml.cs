using System.Diagnostics;
using System.IO;

namespace FilePickerTest;

public partial class MainPage : ContentPage
{
	
	public MainPage()
	{
		InitializeComponent();
	}

    private async void btnFilePicker_Clicked(object sender, EventArgs e)
    {

        // FilePicker needs Permissions.StorageRead
        PermissionStatus status = await Permissions.RequestAsync<Permissions.StorageRead>();

        //var customFileType = new FilePickerFileType(
        //                new Dictionary<DevicePlatform, IEnumerable<string>>
        //                {
        //                    //{ DevicePlatform.iOS, new[] { "public.my.comic.extension" } }, // UTType values
        //                    { DevicePlatform.Android, new[] { "text/plain", "image/png", "application/vnd.ms-excel" } }, // MIME type
        //                    //{ DevicePlatform.WinUI, new[] { ".cbr", ".cbz" } }, // file extension
        //                    //{ DevicePlatform.Tizen, new[] { "*/*" } },
        //                    //{ DevicePlatform.macOS, new[] { "cbr", "cbz" } }, // UTType values
        //                });

        //PickOptions options = new()
        //{
        //    PickerTitle = "Please select a file",
        //    FileTypes = customFileType,
        //};


        var result = await FilePicker.Default.PickAsync(default);   // options);
        var destpath = Path.Combine(FileSystem.Current.AppDataDirectory, result.FileName);

        var stream = await result.OpenReadAsync();

        // uncomment and set a breakpoint to see what's on the slab
        // var x = System.IO.Directory.GetFiles(FileSystem.Current.AppDataDirectory);

        File.Delete(destpath);

        if (!File.Exists(destpath))
        {
            using (var binaryReader = new BinaryReader(stream))
            {
                var allBytes = binaryReader.ReadBytes((int)stream.Length);
                using (var binaryWriter = new BinaryWriter(new FileStream(destpath, FileMode.Create)))
                {
                    binaryWriter.Write(allBytes);
                }
            }
        }
        else
        {
            Debug.Print(result.FileName + " already saved on device");
        }

        // uncomment and set a breakpoint to see what's on the slab
        //var y = System.IO.Directory.GetFiles(FileSystem.Current.AppDataDirectory);

    }
}

