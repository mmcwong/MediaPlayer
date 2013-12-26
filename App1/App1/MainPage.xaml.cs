using App1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<String> SUPPORTED_FILES;
        ObservableCollection<Song> mySongs;
        List<StorageFile> mFiles;
        int mIndex;
        bool mIsPaused;

        public MainPage() 
        {
            this.InitializeComponent();

            SUPPORTED_FILES = new List<String>();
            SUPPORTED_FILES.Add(".mp3");
            SUPPORTED_FILES.Add(".wav");
            SUPPORTED_FILES.Add(".wmv");

            mySongs = new ObservableCollection<Song>();
            mFiles = new List<StorageFile>();


            mIndex = 0;
            mIsPaused = false;

            ReadSongs();
       }

        private void mElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            NextSong();
        }

        async private void PlaySong ()
        {
            if (!mIsPaused)
            {
                StorageFile file = mFiles[mIndex];
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                _mElement.SetSource(stream, file.ContentType);
            }
                _mElement.Play();
                mIsPaused = false;
        }

        private void StopSong ()
        {
            _mElement.Stop();
        }

        private void PauseSong()
        {
            mIsPaused = true;
            _mElement.Pause();
        }

        private void NextSong()
        {
            if (mIndex < mFiles.Count)
            {
                mIndex++;
            }
            PlaySong();
        }

        private void PrevSong()
        {
            if (mIndex > 0 )
            {
                mIndex--;
            }
            PlaySong();

        }

        private void ShuffleSongs()
        {
            Random rnd = new Random();
            mFiles = mFiles.OrderBy(item => rnd.Next()).ToList();
        }
        async private void ReadSongs() 
        {
            StorageFolder music = KnownFolders.MusicLibrary;
            IReadOnlyList<StorageFile> files = await music.GetFilesAsync();

            foreach (StorageFile file in files)
            {
                if (SUPPORTED_FILES.Contains(file.FileType))
                {
                    var properties = await file.Properties.GetMusicPropertiesAsync();
                    mFiles.Add(file);
                    mySongs.Add(new Song(properties.Title, properties.Artist));
                }
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton button = sender as AppBarButton;
            if(button.Label == "Play")
            {
                PlaySong();
            }
            else if (button.Label == "Stop")
            {
                StopSong();
            }
            else if (button.Label == "Pause")
            {
                PauseSong();
            }
            else if (button.Label == "Next")
            {
                NextSong();
            }
            else if (button.Label == "Prev")
            {
                PrevSong();
            }

            else if (button.Label == "Shuffle")
            {
                ShuffleSongs();
            }
        }

        public ObservableCollection<Song> songs 
        {
            get
            {
                return mySongs;
            }

            private set
            {
                
            }
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Slider slider = sender as Slider;
            double volume = slider.Value/100.0;

            if (_mElement != null)
            {
                _mElement.Volume = volume;
            }
        }

    }
}
