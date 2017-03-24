﻿using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WindowsPhoneApp.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace WindowsPhoneApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewTaskPage : Page
    {
        private App app = (Application.Current as App);
        private TaskViewModel taskViewModel = null;
        private TaskViewModel passedData = null;

        public ViewTaskPage()
        {
            this.InitializeComponent();
            taskViewModel = new TaskViewModel();
            passedData = new TaskViewModel();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            passedData = (e.Parameter as TaskViewModel);
            TitlePageTextBlock.Text = passedData.Title;
            TitleValueTextBlock.Text = passedData.Title;
            DateValueTextBlock.Text = passedData.Date;
            DurationValueTextBlock.Text = passedData.Duration;
            DescriptionValueTextBlock.Text = passedData.Description;
            CostsValueTextBlock.Text = "€ " + passedData.Costs;

            if (passedData.Remarks == string.Empty)
            {
                RemarksValueTextBlock.Text = "Geen opmerkingen opgegeven."; 
            }
            else
            {
                RemarksValueTextBlock.Text = passedData.Remarks;
            }
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EditTaskPage), passedData);
        }
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog msg = new MessageDialog("Weet je het zeker?");
            msg.Commands.Add(new UICommand("Ja"));
            msg.Commands.Add(new UICommand("Nee"));
            var result = await msg.ShowAsync();
            if (result.Label == "Ja")
            {
                TaskViewModel taskViewModel = new TaskViewModel();
                taskViewModel.DeleteTask(passedData.TaskId);
                app.DisplayMessageBox("Taak is succesvol verwijderd.");
                Frame.Navigate(typeof(StartPage));
            }
        }
    }
}
