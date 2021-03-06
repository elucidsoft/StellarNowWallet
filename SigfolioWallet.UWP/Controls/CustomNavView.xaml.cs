﻿using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using SigfolioWallet.Utilities;

namespace SigfolioWallet.Controls
{
    public sealed partial class CustomNavView : UserControl
    {
        private TextBlock _title;

        private Ellipse _ellName;
        private Grid _ccNameGrid;
        private ContentControl _ccName;
        private TextBlock _txtName;
        private FontIcon _txtNameChevron;
        private TextBlock _txtAsset;
        private TextBlock _txtAmount;

        private Thickness _txtNameChevronOriginalMargin;
        private Visibility _txtNameChevronOriginalVisibility;
        private HorizontalAlignment _ccNameGridOriginalHorizontalAlignment;
        private HorizontalAlignment _ccNameOriginalHorizontalAlignment;
        private Thickness _ccNameOriginalMargin;
        private double _ellNameOriginalWidth;
        private double _ellNameOriginalHeight;
        private double _txtNameOriginalFontSize;
        private Visibility _txtAssetOriginalVisibility;
        private Visibility _txtAmountOriginalVisibility;

        private Grid _paneContentGrid;
        private IEnumerable<Rectangle> _selectionIndicators;

        private readonly AcrylicBrush _acrylicBrush = new AcrylicBrush();
        private readonly UISettings _uiSettings = new UISettings();

        public CustomNavView()
        {

            InitializeComponent();

            _acrylicBrush.TintOpacity = 0.8;
            _acrylicBrush.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
            
            _uiSettings.ColorValuesChanged += UiSettings_ColorValuesChanged;

            NavView.Loaded += NavView_Loaded;
        }

        private void UiSettings_ColorValuesChanged(UISettings sender, object args)
        {
            SetColors();
        }

        private async void SetColors()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                _acrylicBrush.TintColor = UIUtility.GetAccentColorLow();
                _acrylicBrush.FallbackColor = UIUtility.GetAccentColorLow();

                btnLockUnlockWallet.Background = new SolidColorBrush(UIUtility.GetAccentColorHigh());

                foreach (var si in _selectionIndicators)
                {
                    if (Application.Current.RequestedTheme == ApplicationTheme.Light)
                    {
                        si.Fill = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0));
                    }
                    else
                    {
                        si.Fill = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
                    }
                }

            });
        }

        private void NavView_PaneOpening(NavigationView sender, object args)
        {
            TogglePane(false);
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            _title = UWPUtilities.FindControlWithName<TextBlock>("tbTitle", MyNavView);
            
            _ellName = UWPUtilities.FindControlWithName<Ellipse>("ellName", MyNavView);
            _ccNameGrid = UWPUtilities.FindControlWithName<Grid>("ccNameGrid", MyNavView);
            _ccName = UWPUtilities.FindControlWithName<ContentControl>("ccName", MyNavView);
            _txtName = UWPUtilities.FindControlWithName<TextBlock>("txtName", MyNavView);
            _txtNameChevron = UWPUtilities.FindControlWithName<FontIcon>("txtNameChevron", MyNavView);
            _txtAsset = UWPUtilities.FindControlWithName<TextBlock>("txtAsset", MyNavView);
            _txtAmount = UWPUtilities.FindControlWithName<TextBlock>("txtAmount", MyNavView);

            _paneContentGrid = UWPUtilities.FindControlWithName<Grid>("PaneContentGrid", MyNavView);
            _selectionIndicators = UWPUtilities.FindControlsWithName<Rectangle>("SelectionIndicator", MyNavView);

            _paneContentGrid.Background = _acrylicBrush;

            _txtNameChevronOriginalMargin = _txtNameChevron.Margin;
            _txtNameChevronOriginalVisibility = _txtNameChevron.Visibility;
            _ccNameGridOriginalHorizontalAlignment = _ccNameGrid.HorizontalAlignment;
            _ccNameOriginalHorizontalAlignment = _ccName.HorizontalAlignment;
            _ccNameOriginalMargin = _ccName.Margin;
            _ellNameOriginalWidth = _ellName.Width;
            _ellNameOriginalHeight = _ellName.Height;
            _txtNameOriginalFontSize = _txtName.FontSize;
            _txtAssetOriginalVisibility = _txtAsset.Visibility;
            _txtAmountOriginalVisibility = _txtAmount.Visibility;

            NavView.PaneClosing += NavView_PaneClosing;
            NavView.PaneOpening += NavView_PaneOpening;
            SetColors();
            TogglePane(!NavView.IsPaneOpen);
        }

        private void NavView_PaneClosing(NavigationView sender, NavigationViewPaneClosingEventArgs args)
        {
            TogglePane(true);
        }

        private void TogglePane(bool closing)
        {
            if (closing)
            {
                _txtNameChevron.Margin = new Thickness(0, 18, 0, 0);
                _txtNameChevron.Visibility = Visibility.Collapsed;

                _ccNameGrid.HorizontalAlignment = HorizontalAlignment.Left;
                _ccName.HorizontalAlignment = HorizontalAlignment.Left;
                _ccName.Margin = new Thickness(7, 0, 16, 0);

                _ellName.Width = 30;
                _ellName.Height = 30;

                _txtName.FontSize = 10;

                _txtAsset.Visibility = Visibility.Collapsed;
                _txtAmount.Visibility = Visibility.Collapsed;

                txtLockUnlockWalletLabel.Visibility = Visibility.Collapsed;

                daLockUnlockStoryBoardAnimation.From = NavView.OpenPaneLength;
                daLockUnlockStoryBoardAnimation.To = NavView.CompactPaneLength;
                btnLockUnlockWalletStoryboard.Begin();
            }
            else
            {
                daLockUnlockStoryBoardAnimation.To = NavView.OpenPaneLength;
                daLockUnlockStoryBoardAnimation.From = NavView.CompactPaneLength;
                btnLockUnlockWalletStoryboard.Begin();

                _txtNameChevron.Margin = _txtNameChevronOriginalMargin;
                _txtNameChevron.Visibility = _txtNameChevronOriginalVisibility;

                _ccNameGrid.HorizontalAlignment = _ccNameGridOriginalHorizontalAlignment;
                _ccName.HorizontalAlignment = _ccNameOriginalHorizontalAlignment;
                _ccName.Margin = _ccNameOriginalMargin;

                _ellName.Width = _ellNameOriginalWidth;
                _ellName.Height = _ellNameOriginalHeight;

                _txtName.FontSize = _txtNameOriginalFontSize;

                _txtAsset.Visibility = _txtAssetOriginalVisibility;
                _txtAmount.Visibility = _txtAmountOriginalVisibility;

                btnLockUnlockWallet.Width = NavView.OpenPaneLength;
                txtLockUnlockWalletLabel.Visibility = Visibility.Visible;


            }
        }

        public void SetBalanceText(string amount)
        {
            var txtAmount = UWPUtilities.FindControlWithName<TextBlock>("txtAmount", MyNavView);
            txtAmount.Text = amount;
        }

        public void SetName(string publicKey)
        {
            var txtName = UWPUtilities.FindControlWithName<TextBlock>("txtName", MyNavView);
            txtName.Text = publicKey.Substring(publicKey.Length - 4);
        }

        public Frame AppFrame => PageContent;

        public NavigationView NavView => MyNavView;

        public void SetTitle(string viewModelTitle)
        {
            _title.Text = viewModelTitle;
        }
    }
}
