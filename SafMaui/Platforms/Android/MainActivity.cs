using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using PortableStorage.Droid;

namespace SafMaui;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public const int BROWSE_REQUEST_CODE = 100;
    internal static MainActivity Instance { get; private set; }


    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Instance = this;

    }


    protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
    {
        base.OnActivityResult(requestCode, resultCode, data);

        try
        {
            if (requestCode == BROWSE_REQUEST_CODE && resultCode == Result.Ok)
                SafService.StorageUri = SafStorageHelper.ResolveFromActivityResult(this, data);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

    }


}
