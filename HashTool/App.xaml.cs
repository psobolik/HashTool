﻿using GalaSoft.MvvmLight.Threading;

namespace HashTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App //: Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
