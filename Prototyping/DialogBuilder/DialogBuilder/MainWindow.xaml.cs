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
        /// <summary>
        /// Represents the total amount of target mood pair entries for the current dialog line
        /// </summary>
        public static int TotalTargetMoodPairs;
        /// <summary>
        /// Represents the total amount of response ID entires for the current dialog line
        /// </summary>
        public static int TotalResponseIDs;
        /// <summary>
        /// Stores all the currently selected target mood pair entries for the current dialogline
        /// </summary>
        public static List<CheckBox> SelectedTargetMoodPairs;
        /// <summary>
        /// Stores all the currently selected response id entires for the current dialogline
        /// </summary>
        public static List<CheckBox> SelectedResponseIDs;
        /// <summary>
        /// global toggle for whether all the target mood pair entries are selected or not
        /// </summary>
        public static bool AllTargetMoodPairsSelected;
        /// <summary>
        /// global toggle for whether all the response id entries are selected or not
        /// </summary>
        public static bool AllResponseIDsSelected;
        /// <summary>
        /// Global reference for the currently selected DialogLine from the Active dialogliens list
        /// </summary>
        public DialogLine SelectedDialogLine { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DialogLoader.MasterDialogLinesList = new Dictionary<string, DialogLine>();
            Initialize();
        }//end of MainWindow()
        /// <summary>
        /// Handles initialization of global static members of MainWindow, also used in ClearForm to clear lists and reset values outside the scope of the form
        /// </summary>
        void Initialize()
        {
            TotalTargetMoodPairs = 0;
            TotalResponseIDs = 0;
            SelectedTargetMoodPairs = new List<CheckBox>();
            SelectedResponseIDs = new List<CheckBox>();
            AllTargetMoodPairsSelected = false;
            SelectAllTargetMoodPairsItem.Header = "Select All";
            AllResponseIDsSelected = false;
            SelectAllResponseIDsItem.Header = "Select All";
            PopulateConsistentFormValues();
        }//end of Initialize
        /// <summary>
        /// Clears and resets all  form values
        /// </summary>
        void ClearForm()
        {
            Initialize();
            LineIDTextBox.Text = null;
            ParentDialogLinesComboBox.SelectedItem = null;
            SpeakerIDComboBox.SelectedIndex = 0;
            SpeakerMoodComboBox.SelectedIndex = 0;
            DialogStringTextBox.Text = null;
            PassIDComboBox.SelectedItem = null;
            FailIDComboBox.SelectedItem = null;
            CritFailIDComboBox.SelectedItem = null;
            CleanLists(TargetMoodPairsStackPanel, false);
            CleanLists(ResponseIDsStackPanel, false);
            ResponseIDsComboBox.SelectedItem = null;
        }//end of ClearForm
        /// <summary>
        /// Handles list cleaning (ie right now handles cleaning of Target Mood Pairs and Response IDs lists)
        /// </summary>
        /// <param name="panel"></param>
        void CleanLists(StackPanel panel, bool selectedOnly)
        {
            List<CheckBox> garbage = new List<CheckBox>();
            foreach (var item in panel.Children)
            {
                if (item is CheckBox)
                {
                    if (selectedOnly)
                    {
                        if ((bool)(item as CheckBox).IsChecked)
                        {
                            garbage.Add(item as CheckBox);
                        }
                    }
                    else
                    {
                        garbage.Add(item as CheckBox);
                    }
                }
            }

            foreach (var item in garbage)
            {
                panel.Children.Remove(item);
                TotalResponseIDs--;
            }
        }//end of CleanLists
        /// <summary>
        /// Transliterates a DialogLine object's values onto the form. Should be called when selecting from active dialoglines list
        /// </summary>
        void FromDialogLineToForm(DialogLine selected)
        {
            LineIDTextBox.Text = selected.LineID;
            SpeakerIDComboBox.SelectedItem = selected.SpeakerID;
            SpeakerMoodComboBox.SelectedItem = selected.SpeakerMood;
            DialogStringTextBox.Text = selected.DialogString;
            PassIDComboBox.SelectedValue = selected.PassID;
            FailIDComboBox.SelectedValue = selected.FailID;
            CritFailIDComboBox.SelectedValue = selected.CritFailID;
            PopulateTargetMoodPairEntries(selected.TargetMoodPairs);
            PopulateResponseIDEntries(selected.Responses);
            ResponseIDsComboBox.SelectedIndex = 0;

            int i = 1;
            foreach (var item in selected.TargetMoodPairs)
            {
                for (int j = 0; j < 2; j++)
                {
                    ComboBox list = ((TargetMoodPairsStackPanel.Children[i] as CheckBox).Content as DockPanel).Children[j] as ComboBox;
                    if (j == 0)
                    {
                        list.SelectedValue = item.Key;
                    }
                    else
                    {
                        list.SelectedValue = item.Value;
                    }
                }
                i++;
            }
        }//end of DialogLineToForm
        /// <summary>
        /// Converts form data and values to a DialogLine object
        /// </summary>
        void FromFormToDialogLine()
        {

        }//end of FromFormToDialogLine
         /// <summary>
         /// Handles populating form values that are consistent throughout the dialog creation work flow
         /// </summary>
        void PopulateConsistentFormValues()
        {
            //populate character names into speaker ids list
            foreach (CharacterNames name in Enum.GetValues(typeof(CharacterNames)))
            {
                SpeakerIDComboBox.Items.Add(name);
            }
            SpeakerIDComboBox.SelectedIndex = 0;
            //populate mood types in speaker mood list
            foreach (MoodTypes mood in Enum.GetValues(typeof(MoodTypes)))
            {
                SpeakerMoodComboBox.Items.Add(mood);
            }
            SpeakerMoodComboBox.SelectedIndex = 0;
        }//end of PopulateConsistentFormValues
        /// <summary>
        /// Populates the active dialoglines list with all the entries from the master. This should get called when opening up a new file(s)
        /// </summary>
        void PopulateFromMasterDialogLinesList()
        {
            //This populates all of the Line ID based lists/drop downs with all of the dialog line objects' Line IDs from the master list. I feel like there's a better way to do this but I don't know how right now
            foreach (var item in DialogLoader.MasterDialogLinesList)
            {
                ListBoxItem active = new ListBoxItem() { Content = item.Key };
                active.Selected += (object sender, RoutedEventArgs e) =>
                {
                    ClearForm();
                    SelectedDialogLine = item.Value;
                    FromDialogLineToForm(SelectedDialogLine);
                };

                ActiveDialogLinesListBox.Items.Add(active);
                ParentDialogLinesComboBox.Items.Add(item.Key);
                PassIDComboBox.Items.Add(item.Key);
                FailIDComboBox.Items.Add(item.Key);
                CritFailIDComboBox.Items.Add(item.Key);
                ResponseIDsComboBox.Items.Add(item.Key);
            }
        }//end of PopulateFromMasterDialogLinesList
        /// <summary>
        /// Populates the target mood pairs panel with the appropriate structure of UI elements and corresponding data
        /// </summary>
        /// <param name="targetMoodPairs">The current dialog line's dictionary of CharacterNames|MoodTypes</param>
        void PopulateTargetMoodPairEntries(Dictionary<CharacterNames, MoodTypes> targetMoodPairs)
        {
            int checkBoxWidth = 300;
            int comboBoxWidth = 140;
            foreach (var item in targetMoodPairs)
            {
                TotalTargetMoodPairs++;
                CheckBox entry = new CheckBox()
                {
                    Width = checkBoxWidth,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Content = new DockPanel()
                    {
                        Children =
                        {
                            new ComboBox()
                            {
                                Width = comboBoxWidth,
                                ItemsSource = Enum.GetValues(typeof(CharacterNames))
                            },
                            new ComboBox()
                            {
                                Width = comboBoxWidth,
                                ItemsSource = Enum.GetValues(typeof(MoodTypes))
                            }
                        }
                    }
                };
                InitializeItemEntryControlsOnPopulation(entry, false);
                TargetMoodPairsStackPanel.Children.Add(entry);
            }
        }//end of PopulateTargetMoodPairEntries
        /// <summary>
        /// Populates the response ids panel with the appropriate structure of UI elements and corresponding data
        /// </summary>
        /// <param name="responseIDs">The current dialog line's list of response IDs</param>
        void PopulateResponseIDEntries(List<string> responseIDs)
        {
            foreach (var item in responseIDs)
            {
                TotalResponseIDs++;
                CheckBox entry = new CheckBox() { Content = item };
                InitializeItemEntryControlsOnPopulation(entry, true);
                ResponseIDsStackPanel.Children.Add(entry);
            }
        }//end of PopulateResponseIDEntries
        /// <summary>
        /// Initializes controls handling of ui elements in the entry passed, differentiated by the isResponseIDEntry bool. Helps reduce redundant code, more so than what's already present
        /// </summary>
        /// <param name="entry">The entry from the target mood pairs list or response ids list</param>
        /// <param name="isResponseIDEntry">toggle differentiating from which list the entry belongs to</param>
        void InitializeItemEntryControlsOnPopulation(CheckBox entry, bool isResponseIDEntry)
        {
            entry.Checked += (object sender, RoutedEventArgs e) =>
            {
                if (isResponseIDEntry)
                {
                    SelectedResponseIDs.Add(entry);
                }
                else
                {
                    SelectedTargetMoodPairs.Add(entry);
                }
                CheckSelectAllButton
                (
                    isResponseIDEntry ? SelectAllResponseIDsItem : SelectAllTargetMoodPairsItem,
                    isResponseIDEntry ? SelectedResponseIDs : SelectedTargetMoodPairs,
                    ref isResponseIDEntry ? ref AllResponseIDsSelected : ref AllTargetMoodPairsSelected,
                    ref isResponseIDEntry ? ref TotalResponseIDs : ref TotalTargetMoodPairs
                );
            };//end of Checked

            entry.Unchecked += (object sender, RoutedEventArgs e) =>
            {
                if (isResponseIDEntry)
                {
                    SelectedResponseIDs.Remove(entry);
                }
                else
                {
                    SelectedTargetMoodPairs.Remove(entry);
                }

                CheckSelectAllButton
                (
                    isResponseIDEntry ? SelectAllResponseIDsItem : SelectAllTargetMoodPairsItem,
                    isResponseIDEntry ? SelectedResponseIDs : SelectedTargetMoodPairs,
                    ref isResponseIDEntry ? ref AllResponseIDsSelected : ref AllTargetMoodPairsSelected,
                    ref isResponseIDEntry ? ref TotalResponseIDs : ref TotalTargetMoodPairs
                );
            };//end of Unchecked
        }//end of InitializeResponseIDEntryControlsOnPopulation
        /// <summary>
        /// Checks the relevative select all button and it's corresponding bool and adjust values based off of certain conditions
        /// </summary>
        /// <param name="selectAllItem">The select all button in question</param>
        /// <param name="selectedList">The list of currently selected items</param>
        /// <param name="allSelected">A ref to the corresponding bool of the button in question</param>
        /// <param name="totalEntries">A ref to the total amount of entries</param>
        void CheckSelectAllButton(MenuItem selectAllItem, List<CheckBox> selectedList, ref bool allSelected, ref int totalEntries)
        {
            if (selectedList.Count == totalEntries)
            {
                if (!allSelected)
                {
                    allSelected = true;
                }

                selectAllItem.Header = "Deselect All";
                return;
            }

            if (selectedList.Count == 0)
            {
                if (allSelected)
                {
                    allSelected = false;
                }

                selectAllItem.Header = "Select All";
                return;
            }

            if (totalEntries <= 0)
            {
                selectAllItem.Header = "Select All";
                return;
            }
        }//end of CheckSelectAllButton
        /// <summary>
        /// Removes entries from a collection. If there are entries that are selected then the remove button serves as a remove selected button, else it removes from the bottom
        /// </summary>
        /// <param name="panel">Container or entries</param>
        /// <param name="selectedEntries">Selected entries list if there are any</param>
        /// <param name="totalEntries">ref to total entries for the container</param>
        void RemoveEntry(StackPanel panel, List<CheckBox> selectedEntries, bool isResponsePanel, ref int totalEntries)
        {
            if (totalEntries > 0) //if we have at least one entry
            {
                if (selectedEntries.Count <= 0) //if none are selected
                {
                    panel.Children.RemoveAt(totalEntries);
                    totalEntries--;
                }
                else
                {
                    CleanLists(panel, true);
                }
            }

            CheckSelectAllButton
            (
                isResponsePanel ? SelectAllResponseIDsItem : SelectAllTargetMoodPairsItem,
                selectedEntries,
                ref isResponsePanel ? ref AllResponseIDsSelected : ref AllTargetMoodPairsSelected,
                ref totalEntries
            );
        }//end of RemoveEntry
        //Handles opening of dialog text files
        private void OpenItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.ShowDialog();
            DialogLoader.ReadDialogTextFiles(open.FileNames);
            PopulateFromMasterDialogLinesList();
        }//end of OpenItem_Click
        //Handles saving of dialog text files
        private void SaveItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.ShowDialog();
        }//end of SaveItem_Click
        //Closes the application
        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }//end of ExitItem_Click
        //Calls ClearForm() and resets all values to allow for creation of a new dialogline
        private void NewItem_Click(object sender, RoutedEventArgs e)
        {
            //im not sure if im gonna use this
        }//end of NewItem_Click
        //Calls CheckForm and creates a new dialogline object from the data in the form assuming CheckForm returns true
        private void CreateItem_Click(object sender, RoutedEventArgs e)
        {

        }//end of CreateItem_Click
        //Copies all data, in it's current state, from the current form layout
        private void CopyItem_Click(object sender, RoutedEventArgs e)
        {

        }//end of CopyItem_Click
        //Pastes all data to the current form, overwriting all the current data
        private void PasteItem_Click(object sender, RoutedEventArgs e)
        {

        }//end of PasteItem_Click
        //Applies modifications to a current dialogline
        private void ApplyItem_Click(object sender, RoutedEventArgs e)
        {

        }//end of ApplyItem_Click
        //Redos undone changes to the current dialogline
        private void RedoItem_Click(object sender, RoutedEventArgs e)
        {

        }//end of RedoItem_Click
        //Undoes (redone) changes to the current dialogline
        private void UndoItem_Click(object sender, RoutedEventArgs e)
        {

        }//end of UndoItem_Click
        //Resets all changes done to the current dialogline to its last most saved state
        private void ResetItem_Click(object sender, RoutedEventArgs e)
        {
            FromDialogLineToForm(SelectedDialogLine);
        }//end of ResetItem_Click
        //Calls the ClearForm()
        private void ClearItem_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }//end of ClearItem_Click
        //Searches list of active DialogLine instances and compares against that which is being type where a red background will signify a duplicate ID or that it contains []{}: or one of the ChunkIdentifierTypes
        private void LineIDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }//end of LineIDTextBox_TextChanged
        //Handles (de)selection of all target mood pairs entries
        private void SelectAllTargetMoodPairsItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = ((MenuItem)sender);
            AllTargetMoodPairsSelected = !AllTargetMoodPairsSelected;
            foreach (var entry in TargetMoodPairsStackPanel.Children)
            {
                if (entry is CheckBox)
                {
                    (entry as CheckBox).IsChecked = AllTargetMoodPairsSelected;
                }
            }
            CheckSelectAllButton(item, SelectedTargetMoodPairs, ref AllTargetMoodPairsSelected, ref TotalTargetMoodPairs);
        }//end of SelectAllTargetMoodPairsItem_Click
        //handles adding a new target mood pair entry -> CheckBox (DockPanel(ComboBox, ComboBox))
        private void AddTargetMoodPairItem_Click(object sender, RoutedEventArgs e)
        {

        }//end of AddTargetMoodPairItem_Click
        //handles removing an existing target mood pair entry; If there are selected entries, it will remove all selected, else itll remove from the bottom
        private void RemoveTargetMoodPairItem_Click(object sender, RoutedEventArgs e)
        {
            RemoveEntry(TargetMoodPairsStackPanel, SelectedTargetMoodPairs, false, ref TotalTargetMoodPairs);
        }//end of RemoveTargetMoodPairItem_Click
        //Handles (de)selection of all response ID entries
        private void SelectAllResponseIDsItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = ((MenuItem)sender);
            AllResponseIDsSelected = !AllResponseIDsSelected;
            foreach (var entry in ResponseIDsStackPanel.Children)
            {
                if (entry is CheckBox)
                {
                    (entry as CheckBox).IsChecked = AllResponseIDsSelected;
                }
            }
            CheckSelectAllButton(item, SelectedResponseIDs, ref AllResponseIDsSelected, ref TotalResponseIDs);
        }//end of SelectAllResponseIDsItem_Click
        //handles removing an existing response ID entry; If there are selected entries, it will remove all selected, else itll remove from the bottom
        private void RemoveResponseIDItem_Click(object sender, RoutedEventArgs e)
        {
            RemoveEntry(ResponseIDsStackPanel, SelectedResponseIDs, true, ref TotalResponseIDs);
        }//end of RemoveResponseIDItem_Click
        //Serves as a pseudo add function, when selection is changed, itll add to the list of response ID entries, assuming there's no prexisting one of the same type.
        private void ResponseIDsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }//end of ResponseIDsComboBox_SelectionChanged
    }//end of MainWindow
}//end of namespace
