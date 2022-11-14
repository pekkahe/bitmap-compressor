using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitmapCompressor;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Microsoft.Win32;

namespace BitmapCompressor.WPF
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        private readonly IBlockCompressor _compressor;

        public MainWindowViewModel(IBlockCompressor compressor)
        {
            _compressor = compressor;

            OpenBitmap = new RelayCommand(OpenFileDialog);
        }

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set { SetField(ref _imageSource, value); }
        }

        public ICommand OpenBitmap { get; }

        public void OpenFileDialog()
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = ".bmp";
            dialog.Filter = "Bitmap images|*.bmp";

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(dialog.FileName, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();

                ImageSource = bitmap;
            }
        }
    }
}
