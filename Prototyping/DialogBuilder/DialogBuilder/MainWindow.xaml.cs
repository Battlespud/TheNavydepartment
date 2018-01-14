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
using Microsoft.Win32;

namespace DialogBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DialogLine currentSelection;
        public static List<TargetMoodPairControl> SelectedTMPs;
        public static int TMPCount;
        public static List<CheckBox> SelectedResponses;
        public static int ResponsesCount;

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
            SelectedTMPs = new List<TargetMoodPairControl>();
            TMPCount = 0;
            SelectedResponses = new List<CheckBox>();
            ResponsesCount = 0;
            DialogLoader.Initialize();
            InitializeSpeakerOptions();
            InitializeResponsePanel();
        }//end of Initialize

        /// <summary>
        /// Initialize speaker id and speaker mood dropdowns
        /// </summary>
        void InitializeSpeakerOptions()
        {
            foreach (var item in Enum.GetValues(typeof(CharacterNames)))
            {
                SpeakerIDBox.Items.Add(item);
            }
            SpeakerIDBox.SelectedItem = SpeakerIDBox.Items[0];

            foreach (var item in Enum.GetValues(typeof(MoodTypes)))
            {
                SpeakerMoodBox.Items.Add(item);
            }
            SpeakerMoodBox.SelectedItem = SpeakerMoodBox.Items[0];
        }//end of InitializeSpeakerOptions

        /// <summary>
        /// Initializes various Response panel functions/options
        /// </summary>
        void InitializeResponsePanel()
        {
            ResponseLineIDsList.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
            {
                foreach (var response in ResponsesPanel.Children)
                {
                    if (response is CheckBox && (response as CheckBox).Content == (ResponseLineIDsList.SelectedItem as ComboBoxItem).Content)
                    {
                        return;
                    }
                }

                CheckBox item = new CheckBox() { Content = (ResponseLineIDsList.SelectedValue as ComboBoxItem).Content };

                item.Checked += (object subSender, RoutedEventArgs subE) =>
                {
                    SelectedResponses.Add(item);
                    if (SelectedResponses.Count == ResponsesCount && !(bool)SelectAllResponsesBox.IsChecked)
                    {
                        SelectAllResponsesBox.IsChecked = true;
                    }
                };//end of item.Checked

                item.Unchecked += (object subSender, RoutedEventArgs subE) =>
                {
                    SelectedResponses.Remove(item);
                    if (SelectedResponses.Count <= 0 && (bool)SelectAllResponsesBox.IsChecked)
                    {
                        SelectAllResponsesBox.IsChecked = false;
                    }
                };//end of item.Unchecked

                ResponsesPanel.Children.Add(item);
                ResponsesCount++;
            };//ResponseLineIDList.SelectionChanged
        }//end of InitializeResponsePAnel

        private void RemoveResponseButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedResponses.Count <= 0 && ResponsesCount > 0)
            {
                ResponsesPanel.Children.RemoveAt(ResponsesCount);
                ResponsesCount--;
            }
            else
            {
                List<CheckBox> unselected = new List<CheckBox>();
                foreach (CheckBox item in SelectedResponses)
                {
                    ResponsesPanel.Children.Remove(item);
                    unselected.Add(item);
                    ResponsesCount--;
                }

                foreach (CheckBox item in unselected)
                {
                    SelectedResponses.Remove(item);
                }
            }

            if (ResponsesCount <= 0 && (bool)SelectAllResponsesBox.IsChecked)
            {
                SelectAllResponsesBox.IsChecked = false;
            }
        }//end of RemoveResponseButton.Click

        private void SelectAllResponsesBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in ResponsesPanel.Children)
            {
                if (item is CheckBox)
                {
                    (item as CheckBox).IsChecked = ((CheckBox)sender).IsChecked;
                }
            }
        }//end of SelectAllResponsesBox_Checked

        private void SelectAllResponsesBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in ResponsesPanel.Children)
            {
                if (item is CheckBox)
                {
                    (item as CheckBox).IsChecked = ((CheckBox)sender).IsChecked;
                }
            }
        }//end of SelectAllResponsesBox_Unchecked

        private void SelectAllTMPsBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in TMPsPanel.Children)
            {
                if (item is DockPanel && (item as DockPanel).Name != "TMPAddRemovePanel")
                {
                    ((item as DockPanel).Children[0] as CheckBox).IsChecked = ((CheckBox)sender).IsChecked;
                }
            }
        }//end of SelectAllTMPsBox_Checked

        private void SelectAllTMPsBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in TMPsPanel.Children)
            {
                if (item is DockPanel && (item as DockPanel).Name != "TMPAddRemovePanel")
                {
                    ((item as DockPanel).Children[0] as CheckBox).IsChecked = ((CheckBox)sender).IsChecked;
                }
            }
        }//end of SelectAllTMPsBox_Unchecked

        /// <summary>
        /// Adds a new target|mood pair to the list
        /// </summary>
        private void AddTMPButton_Click(object sender, RoutedEventArgs e)
        {
            TargetMoodPairControl item = new TargetMoodPairControl(TMPsPanel, SelectAllTMPsBox);
            TMPCount++;
        }//end of AddTMPButton_Click

        /// <summary>
        /// Removes selected rows of target|mood pairs or the last most ppair if none are selected
        /// </summary>
        private void RemoveTMPButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTMPs.Count <= 0 && TMPCount > 0)
            {
                TMPsPanel.Children.RemoveAt(TMPCount);
                TMPCount--;
            }
            else
            {
                List<TargetMoodPairControl> unselected = new List<TargetMoodPairControl>();
                foreach (TargetMoodPairControl item in SelectedTMPs)
                {
                    TMPsPanel.Children.Remove(item.tmpPanel);
                    unselected.Add(item);
                    TMPCount--;
                }

                foreach (TargetMoodPairControl item in unselected)
                {
                    SelectedTMPs.Remove(item);
                }
            }

            if (TMPCount <= 0 && (bool)SelectAllTMPsBox.IsChecked)
            {
                SelectAllTMPsBox.IsChecked = false;
            }
        }//end of RemoveTMPButton_Click

        /// <summary>
        /// Create a new DialogLine object and add it to the master list to the left, orged alphabetically and named via LineID
        /// </summary>
        private void CreateDialogLineButton_Click(object sender, RoutedEventArgs e)
        {
            if (!DialogLoader.MasterDialogLineList.ContainsKey(LineIDBox.Text))
            {
                DialogLine item = new DialogLine()
                {
                    LineID = LineIDBox.Text,
                    DialogString = DialogBox.Text ?? "null",
                    SpeakerID = SpeakerIDBox.Text ?? "null",
                    SpeakerMood = (MoodTypes)SpeakerMoodBox.SelectedItem,
                    PassTarget = PassBox.Text ?? "null",
                    FailTarget = FailBox.Text ?? "null",
                    CritFailTarget = CritFailBox.Text ?? "null",
                };

                List<string> responses = new List<string>();
                foreach (var response in ResponsesPanel.Children)
                {
                    if (response is CheckBox)
                    {
                        responses.Add((response as CheckBox).Content.ToString());
                    }
                }

                DialogLoader.MasterDialogLineList.Add(item.LineID, item);
            }
        }//end of CreateDialogLineButton_Click

        /// <summary>
        /// Opens dialog text files
        /// </summary>
        private void OpenItem_Click(object sender, RoutedEventArgs e)
        {
            DialogLinesList.Items.Clear();
            ResponseLineIDsList.Items.Clear();
            OpenFileDialog open = new OpenFileDialog();
            open.ShowDialog();
            DialogLoader.LoadDialogLinesFromFile(open.FileName);
            foreach (var kvp in DialogLoader.MasterDialogLineList)
            {
                ListBoxItem item = new ListBoxItem() { Content = kvp.Key };
                item.Selected += (object subSender, RoutedEventArgs subE) => 
                {
                    PopulateForm(kvp.Key);
                };

                DialogLinesList.Items.Add(item);
                ResponseLineIDsList.Items.Add(new ComboBoxItem() { Content = kvp.Key });
            }
        }//end of OpenItem_Click

        /// <summary>
        /// Saves all listed DialogLines (LineIDs) in the list directly beneath, into a text file
        /// </summary>
        private void SaveItem_Click(object sender, RoutedEventArgs e)
        {
            //wip
        }//end of SaveItem_Click

        /// <summary>
        /// Closes application
        /// </summary>
        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }//end of ExitItem_Click

        /// <summary>
        /// Clears the forms
        /// </summary>
        private void ClearItem_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }//end of ClearItem_Click

        /// <summary>
        /// Clears and defaults form values
        /// </summary>
        private void ClearForm()
        {
            LineIDBox.Text = null;
            SpeakerIDBox.SelectedItem = SpeakerIDBox.Items[0];
            SpeakerMoodBox.SelectedItem = SpeakerMoodBox.Items[0];
            IsExtensionBox.IsChecked = false;
            HierarchyTypeLabel.Content = "N/A";
            DialogBox.Text = null;
            PassBox.Text = null;
            FailBox.Text = null;
            CritFailBox.Text = null;

            if (TMPCount > 0)
            {
                TMPsPanel.Children.RemoveRange(1, TMPCount);
                TMPCount = 0;
                SelectedTMPs.Clear();
            }

            if (ResponsesCount > 0)
            {
                ResponsesPanel.Children.RemoveRange(1, ResponsesCount);
                ResponsesCount = 0;
                SelectedResponses.Clear();
            }

            SelectAllTMPsBox.IsChecked = false;
            SelectAllResponsesBox.IsChecked = false;
        }//end of ClearForm()

        /// <summary>
        /// Populates form values with selected DialogLine
        /// </summary>
        /// <param name="lineID"></param>
        private void PopulateForm(string lineID)
        {
            ClearForm();
            DialogLine selected = DialogLoader.MasterDialogLineList[lineID];

            LineIDBox.Text = lineID;
            SpeakerIDBox.SelectedItem = selected.SpeakerID;
            SpeakerMoodBox.SelectedItem = selected.SpeakerMood;
            IsExtensionBox.IsChecked = false;
            HierarchyTypeLabel.Content = "N/A";
            DialogBox.Text = selected.DialogString;
            PassBox.Text = selected.PassTarget;
            FailBox.Text = selected.FailTarget;
            CritFailBox.Text = selected.CritFailTarget;

            foreach (string response in selected.PossibleResponses)
            {
                ResponsesPanel.Children.Add(new CheckBox() { Content = response });
                ResponsesCount++;
            }

            foreach (var kvp in selected.TargetMoods)
            {
                TargetMoodPairControl item = new TargetMoodPairControl(TMPsPanel, SelectAllTMPsBox);

                item.targets.SelectedItem = (CharacterNames)Enum.Parse(typeof(CharacterNames), kvp.Key);
                item.moods.SelectedItem = kvp.Value;
                TMPCount++;
            }

            SelectAllTMPsBox.IsChecked = false;
            SelectAllResponsesBox.IsChecked = false;
        }//end of PopulateForm()
    }//end of MainWindow
}//end of namespace