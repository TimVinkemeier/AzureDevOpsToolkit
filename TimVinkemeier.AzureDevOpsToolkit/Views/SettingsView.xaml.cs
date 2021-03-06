﻿using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;

using TimVinkemeier.AzureDevOpsToolkit.Core.Services;
using TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels.Settings;

namespace TimVinkemeier.AzureDevOpsToolkit.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    [MvxViewFor(typeof(SettingsViewModel))]
    [OutletBasedPresentation(OutletIdentifier = "RouterOutlet")]
    public partial class SettingsView : MvxWpfView
    {
        public SettingsView()
        {
            InitializeComponent();
        }
    }
}