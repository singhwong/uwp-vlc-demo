using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using VLC;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace VLCDemo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string FILE_TOKEN = "{1BBC4B94-BE33-4D79-A0CB-E5C6CDB9D107}";
        private const string SUBTITLE_FILE_TOKEN = "{16BA03D6-BCA8-403E-B1E8-166B0020B4A7}";
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void openFile_bt_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OpenFileAsync();
        }

        private async void OpenFileAsync()
        {
            var file = await PickSingleFileAsync("*", FILE_TOKEN);
            if (file != null)
            {
                media_element.MediaSource = VLC.MediaSource.CreateFromUri($"winrt://{FILE_TOKEN}");
            }
        }
        private async Task<StorageFile> PickSingleFileAsync(string fileTypeFilter, string token)
        {
            var fileOpenPicker = new FileOpenPicker()
            {
                SuggestedStartLocation = PickerLocationId.VideosLibrary
            };
            fileOpenPicker.FileTypeFilter.Add(fileTypeFilter);
            var file = await fileOpenPicker.PickSingleFileAsync();
            if (file != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace(token, file);
            }
            return file;
        }

        private void addSubTitle_bt_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OpenSubtitleFileAsync();
        }

        private async void OpenSubtitleFileAsync()
        {
            var source = media_element.MediaSource;
            if (source == null)
            {
                return;
            }
            var file = await PickSingleFileAsync(".srt", SUBTITLE_FILE_TOKEN);
            if (file != null)
            {
                media_element.MediaSource.ExternalTimedTextSources.Add(TimedTextSource.CreateFromUri($"winrt://{SUBTITLE_FILE_TOKEN}"));
            }
        }
    }
}
