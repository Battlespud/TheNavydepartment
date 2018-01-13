using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DialogBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<CheckBox> SelectedResponses;
        public int ResponsesCount;

        public MainWindow()
        {
            InitializeComponent();
            Initialize();

        }//end of ctor

        /// <summary>
        /// Main routing initialize function
        /// </summary>
        void Initialize()
        {
            SelectedResponses = new List<CheckBox>();
            ResponsesCount = 0;
            InitializeSpeakerIDList();
        }

        void InitializeSpeakerIDList()
        {
            foreach (var item in Enum.GetValues(typeof(CharacterNames)))
            {
                SpeakerIDBox.Items.Add(new ComboBoxItem() { Content = item.ToString() });
            }
            SpeakerIDBox.SelectedItem = SpeakerIDBox.Items[0];
        }

        /// <summary>
        /// add item testing button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = new ListBoxItem() { Content = string.Format("DebugDialogLine{0}", DialogLinesList.Items.Count) };
            item.GotFocus += (object subSender, RoutedEventArgs subE) =>
            {
                //display total dialogline data function implementation here
            };
            DialogLinesList.Items.Add(item);
        }//end of MenuItemClick

        private void AddResponseButton_Click(object sender, RoutedEventArgs e)
        {
            CheckBox item = new CheckBox() { Content = string.Format("ResponseNumber{0}", ResponsesPanel.Children.Count - 1) };
            ResponsesCount++;

            item.Checked += (object subSender, RoutedEventArgs subE) =>
            {
                SelectedResponses.Add((CheckBox)subSender);
                if (SelectedResponses.Count == ResponsesCount && !(bool)SelectAllBox.IsChecked)
                {
                    SelectAllBox.IsChecked = true;
                }
            };

            item.Unchecked += (object subSender, RoutedEventArgs subE) =>
            {
                SelectedResponses.Remove((CheckBox)subSender);
                if (SelectedResponses.Count <= 0 && (bool)SelectAllBox.IsChecked)
                {
                    SelectAllBox.IsChecked = false;
                }
            };

            ResponsesPanel.Children.Add(item);
        }

        private void RemoveResponseButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedResponses.Count <= 0 && ResponsesCount > 0 )
            {
                ResponsesPanel.Children.RemoveAt(ResponsesCount);
                ResponsesCount--;
            }
            else
            {
                List<CheckBox> unselected = new List<CheckBox>();
                foreach (CheckBox item in SelectedResponses)
                {
                    //int index = ResponsesPanel.Children.IndexOf(item);
                    ResponsesPanel.Children.Remove(item);
                    unselected.Add(item);
                    ResponsesCount--;
                }

                foreach (CheckBox item in unselected)
                {
                    SelectedResponses.Remove(item);
                }
            }

            if (ResponsesCount <= 0 && (bool)SelectAllBox.IsChecked)
            {
                SelectAllBox.IsChecked = false;
            }
        }

        private void SelectAllBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in ResponsesPanel.Children)
            {
                if (item is CheckBox)
                {
                    (item as CheckBox).IsChecked = ((CheckBox)sender).IsChecked;
                }
            }
        }

        private void SelectAllBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in ResponsesPanel.Children)
            {
                if (item is CheckBox)
                {
                    (item as CheckBox).IsChecked = ((CheckBox)sender).IsChecked;
                }
            }
        }
    }
}