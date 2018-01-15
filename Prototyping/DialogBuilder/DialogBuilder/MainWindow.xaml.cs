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
        public object SelectAllTargetMoodPairsHeader
        {
            get { return AllTargetMoodPairsSelected ? "Deselect All" : "Select All"; }
        }
        public object SelectAllResponseIDsHeader
        {
            get { return AllResponseIDsSelected ? "Deselect All" : "Select All"; }
        }
        /// <summary>
        /// Represents the total amount of target mood pair entries for the current dialog line
        /// </summary>
        public int TotalTargetMoodPairs
        {
            get { return ActiveTargetMoodPairEntries.Count; }
        }
        /// <summary>
        /// Represents the total amount of response ID entires for the current dialog line
        /// </summary>
        public int TotalResponseIDs
        {
            get { return ActiveResponseIDEntries.Count; }
        }
        /// <summary>
        /// Easy way to get the total selected target mood pairs count
        /// </summary>
        static int TotalSelectedTargetMoodPairs
        {
            get { return SelectedTargetMoodPairs.Count; }
        }
        /// <summary>
        /// Easy way to get the total selected response ids count
        /// </summary>
        static int TotalSelectedResponseIDs
        {
            get { return SelectedResponseIDs.Count; }
        }
        /// <summary>
        /// Stores all the currently selected target mood pair entries for the current dialogline
        /// </summary>
        public static List<CheckBox> SelectedTargetMoodPairs;
        /// <summary>
        /// Stores all the currently selected response id entires for the current dialogline
        /// </summary>
        public static List<CheckBox> SelectedResponseIDs;
        /// <summary>
        /// Stores all active tmp entries for the current DialogLine
        /// </summary>
        public static Dictionary<CheckBox, KeyValuePair<ComboBox, ComboBox>> ActiveTargetMoodPairEntries;
        /// <summary>
        /// Stores all the active rid entries for the current DialogLine
        /// </summary>
        public static List<CheckBox> ActiveResponseIDEntries;
        /// <summary>
        /// global toggle for whether all the target mood pair entries are selected or not
        /// </summary>
        public bool AllTargetMoodPairsSelected
        {
            get { return TotalSelectedTargetMoodPairs == TotalTargetMoodPairs && TotalTargetMoodPairs > 0 ? true : false; }
        }
        /// <summary>
        /// global toggle for whether all the response id entries are selected or not
        /// </summary>
        public bool AllResponseIDsSelected
        {
            get { return TotalSelectedResponseIDs == TotalResponseIDs && TotalResponseIDs > 0 ? true : false; }
        }
        /// <summary>
        /// Global reference for the currently selected DialogLine from the Active dialogliens list
        /// </summary>
        public DialogLine SelectedDialogLine { get; set; }
        /// <summary>
        /// private master reference for all of the _ID containing combobox elements so that they can be maniuplated easier
        /// </summary>
        static List<ComboBox> MasterListRference;

        public MainWindow()
        {
            InitializeComponent();
            DialogLoader.MasterDialogLinesList = new Dictionary<string, DialogLine>();
            Initialize();
            InitializeComboBoxes();
            ClearForm();
            Loaded += (object sender, RoutedEventArgs e) =>
            {
                CheckSelectAllButtons();
                UpdateDebug();
                LayoutUpdated += (object subSender, EventArgs subE) =>
                {
                    UpdateDebug();
                };
            };
        }//end of MainWindow()
        /// <summary>
        /// Handles initialization of global static members of MainWindow, also used in ClearForm to clear lists and reset values outside the scope of the form
        /// </summary>
        void Initialize()
        {
            SelectedTargetMoodPairs = new List<CheckBox>();
            SelectedResponseIDs = new List<CheckBox>();
            ActiveTargetMoodPairEntries = new Dictionary<CheckBox, KeyValuePair<ComboBox, ComboBox>>();
            ActiveResponseIDEntries = new List<CheckBox>();
            PopulateConsistentFormValues();
        }//end of Initialize
        void InitializeComboBoxes()
        {
            MasterListRference = new List<ComboBox>()
            {
                ParentDialogLinesComboBox,
                SpeakerIDComboBox,
                SpeakerMoodComboBox,
                PassIDComboBox,
                RegFailIDComboBox,
                CritFailIDComboBox,
                ResponseIDsComboBox
            };
            //populate all the drop downs with "Choose One"
            foreach (ItemsControl item in MasterListRference)
            {
                item.Items.Add("Choose Option");
            }
        }
        /// <summary>
        /// Clears and resets all  form values
        /// </summary>
        void ClearForm()
        {
            //Initialize();
            LineIDTextBox.Text = null;
            ParentDialogLinesComboBox.SelectedIndex = 0;
            SpeakerIDComboBox.SelectedIndex = 0;
            SpeakerMoodComboBox.SelectedIndex = 0;
            DialogStringTextBox.Text = null;
            PassIDComboBox.SelectedIndex = 0;
            RegFailIDComboBox.SelectedIndex = 0;
            CritFailIDComboBox.SelectedIndex = 0;
            CleanLists(TargetMoodPairsStackPanel, SelectedTargetMoodPairs, false, false, TotalTargetMoodPairs);
            CleanLists(ResponseIDsStackPanel, SelectedResponseIDs, false, true, TotalResponseIDs);
            ResponseIDsComboBox.SelectedIndex = 0;
        }//end of ClearForm
        /// <summary>
        /// Handles list cleaning (ie right now handles cleaning of Target Mood Pairs and Response IDs lists)
        /// </summary>
        /// <param name="panel">panel to clean</param>
        /// <param name="selectedList">list of elements to clean</param>
        /// <param name="selectedOnly">toggle whether to remove selected elements only</param>
        /// <param name="isResponseIDs">toggle to indicate whether its the tmps section or rids section</param>
        /// <param name="totalEntries">toggle count of entries in the panel</param>
        void CleanLists(StackPanel panel, List<CheckBox> selectedList, bool selectedOnly, bool isResponseIDs, int totalEntries)
        {
            if (selectedOnly)
            {
                List<CheckBox> garbageRIDs = isResponseIDs ? new List<CheckBox>() : null;
                Dictionary<CheckBox, KeyValuePair<ComboBox, ComboBox>> garbageTMPs = !isResponseIDs ? new Dictionary<CheckBox, KeyValuePair<ComboBox, ComboBox>>() : null;
                if (isResponseIDs)
                {
                    foreach (var item in ActiveResponseIDEntries)
                    {
                        if ((bool)item.IsChecked)
                        {
                            garbageRIDs.Add(item);
                        }
                    }

                    foreach (var item in garbageRIDs)
                    {
                        panel.Children.Remove(item);
                        ActiveResponseIDEntries.Remove(item);
                        SelectedResponseIDs.Remove(item);
                    }
                }
                else
                {
                    foreach (var item in ActiveTargetMoodPairEntries)
                    {
                        if ((bool)item.Key.IsChecked)
                        {
                            garbageTMPs.Add(item.Key, item.Value);
                        }
                    }

                    foreach (var item in garbageTMPs)
                    {
                        panel.Children.Remove(item.Key);
                        ActiveTargetMoodPairEntries.Remove(item.Key);
                        SelectedTargetMoodPairs.Remove(item.Key);
                    }
                }
            }
            else
            {
                if (isResponseIDs)
                {
                    ActiveResponseIDEntries.Clear();
                }
                else
                {
                    ActiveTargetMoodPairEntries.Clear();
                }
                panel.Children.RemoveRange(1, totalEntries);
                selectedList.Clear();
            }
        }//end of CleanLists
        /// <summary>
        /// Transliterates a DialogLine object's values onto the form. Should be called when selecting from active dialoglines list
        /// </summary>
        void FromDialogLineToForm(DialogLine selected)
        {
            ClearForm();
            LineIDTextBox.Text = selected.LineID;
            SpeakerIDComboBox.SelectedItem = selected.SpeakerID;
            SpeakerMoodComboBox.SelectedItem = selected.SpeakerMood;
            DialogStringTextBox.Text = selected.DialogString;
            PassIDComboBox.SelectedValue = selected.PassID ?? "Choose Option";
            RegFailIDComboBox.SelectedValue = selected.RegFailID ?? "Choose Option";
            CritFailIDComboBox.SelectedValue = selected.CritFailID ?? "Choose Option";
            PopulateTargetMoodPairEntries(selected.TargetMoodPairs);
            PopulateResponseIDEntries(selected.Responses);
            ResponseIDsComboBox.SelectedIndex = 0;
        }//end of DialogLineToForm
        /// <summary>
        /// Converts form data and values to a DialogLine object
        /// </summary>
        void FromFormToDialogLine()
        {
            DialogLine created = new DialogLine()
            {
                LineID = LineIDTextBox.Text,
                DialogString = DialogStringTextBox.Text,
                SpeakerID = (CharacterNames)SpeakerIDComboBox.SelectedValue,
                SpeakerMood = (MoodTypes)SpeakerMoodComboBox.SelectedValue
            };

            created.PassID = PassIDComboBox.SelectedIndex != 0 ? PassIDComboBox.SelectedValue.ToString() : null;
            created.RegFailID = RegFailIDComboBox.SelectedIndex != 0 ? RegFailIDComboBox.SelectedValue.ToString() : null;
            created.CritFailID = CritFailIDComboBox.SelectedIndex != 0 ? CritFailIDComboBox.SelectedValue.ToString() : null;

            if (TotalTargetMoodPairs > 0)
            {
                foreach (var item in ActiveTargetMoodPairEntries)
                {
                    created.TargetMoodPairs.Add((CharacterNames)item.Value.Key.SelectedValue, (MoodTypes)item.Value.Value.SelectedValue);
                }
            }

            if (TotalResponseIDs > 0)
            {
                foreach (var item in ActiveResponseIDEntries)
                {
                    created.Responses.Add(item.Content.ToString());
                }
            }

            DialogLoader.MasterDialogLinesList.Add(created.LineID, created);
            PopulateFromMasterDialogLinesList();
            ClearForm();
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
            ActiveDialogLinesListBox.Items.Clear();
            foreach (var item in DialogLoader.MasterDialogLinesList)
            {
                ListBoxItem active = new ListBoxItem() { Content = item.Key };
                active.Selected += (object sender, RoutedEventArgs e) =>
                {
                    ClearForm();
                    SelectedDialogLine = DialogLoader.MasterDialogLinesList[item.Key.ToString()];
                    FromDialogLineToForm(SelectedDialogLine);
                };

                ActiveDialogLinesListBox.Items.Add(active);
                ParentDialogLinesComboBox.Items.Add(item.Key);
                PassIDComboBox.Items.Add(item.Key);
                RegFailIDComboBox.Items.Add(item.Key);
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
            KeyValuePair<ComboBox, ComboBox> children;
            foreach (var item in targetMoodPairs)
            {
                children = AddTargetMoodPairEntry();
                children.Key.SelectedValue = item.Key;
                children.Value.SelectedValue = item.Value;
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
                AddResponseIDEntry(item);
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
                MenuItem selectAllButton = isResponseIDEntry ? SelectAllResponseIDsItem : SelectAllTargetMoodPairsItem;

                if (isResponseIDEntry)
                {
                    SelectedResponseIDs.Add(entry);
                }
                else
                {
                    SelectedTargetMoodPairs.Add(entry);
                }

                CheckSelectAllButtons();
            };//end of Checked

            entry.Unchecked += (object sender, RoutedEventArgs e) =>
            {
                MenuItem selectAllButton = isResponseIDEntry ? SelectAllResponseIDsItem : SelectAllTargetMoodPairsItem;

                if (isResponseIDEntry)
                {
                    SelectedResponseIDs.Remove(entry);
                }
                else
                {
                    SelectedTargetMoodPairs.Remove(entry);
                }

                CheckSelectAllButtons();
            };//end of Unchecked
        }//end of InitializeResponseIDEntryControlsOnPopulation
        /// <summary>
        /// Checks the state of the Select all button for TMPs and RIDs. Honestly it's just aesthetic lol
        /// </summary>
        void CheckSelectAllButtons()
        {
            SelectAllTargetMoodPairsItem.Header = SelectAllTargetMoodPairsHeader;
            SelectAllResponseIDsItem.Header = SelectAllResponseIDsHeader;
        }//end of CheckSelectAllButtons
        /// <summary>
        /// Checks the state of the select all button.
        /// </summary>
        /// <param name="panel">panel of the select all button</param>
        /// <param name="allSelected">bool toggle indicator whether all are selected or not</param>
        /// <param name="selectedList">the list of the selected elements (if any)</param>
        void SelectOrDeselectAll(StackPanel panel, bool allSelected, List<CheckBox> selectedList)
        {
            bool value = allSelected; //bc allSelected is a read only
            foreach (var item in panel.Children)
            {
                if (item is CheckBox)
                {
                    (item as CheckBox).IsChecked = !allSelected;
                }
            }
        }//end of SelectOrDeselectAll
        /// <summary>
        /// Add's a new blank tmp entry to the panel (CheckBox(DockPanel(ComboBox, ComboBox)). Returns collection for easier values population when loading reading from DialogLines
        /// </summary>
        /// <returns>KeyValuePair ComboBox|ComboBox</returns>
        KeyValuePair<ComboBox, ComboBox> AddTargetMoodPairEntry()
        {
            int checkBoxWidth = 300;
            int comboBoxWidth = 140;

            ComboBox names = new ComboBox()
            {
                Width = comboBoxWidth,
                ItemsSource = Enum.GetValues(typeof(CharacterNames)),
            };

            ComboBox moods = new ComboBox()
            {
                Width = comboBoxWidth,
                ItemsSource = Enum.GetValues(typeof(MoodTypes)),
            };

            DockPanel panel = new DockPanel()
            {
                Children =
                {
                    names,
                    moods
                }
            };

            CheckBox entry = new CheckBox()
            {
                Width = checkBoxWidth,
                VerticalContentAlignment = VerticalAlignment.Center,
                Content = panel
            };

            InitializeItemEntryControlsOnPopulation(entry, false);
            TargetMoodPairsStackPanel.Children.Add(entry);
            ActiveTargetMoodPairEntries.Add(entry, new KeyValuePair<ComboBox, ComboBox>(names, moods));
            return ActiveTargetMoodPairEntries[entry];
        }//end of AddTargetMoodPairEntry
        /// <summary>
        /// Adds a new RID entry to the panel
        /// </summary>
        /// <param name="responseID">The lineID of the dialogline being added, passed as an object to avoid annoying .ToString() conversions</param>
        void AddResponseIDEntry(object responseID)
        {
            foreach (var item in ActiveResponseIDEntries)
            {
                if (item.Content.ToString() == responseID.ToString())
                {
                    return;
                }
            }

            CheckBox entry = new CheckBox() { Content = responseID };
            InitializeItemEntryControlsOnPopulation(entry, true);
            ResponseIDsStackPanel.Children.Add(entry);
            ActiveResponseIDEntries.Add(entry);
        }//end of AddResponseIDEntry
        /// <summary>
        /// Removes an entry from a panel. Unlike the differentiation between adding a TMP and a RID, removing can be more generic being as its removing from a checkbox from a panel
        /// </summary>
        /// <param name="panel">Panel to remove from</param>
        /// <param name="selectedList">List of selected checkbox elements (if any)</param>
        /// <param name="isResponseIDs">differentiate between the tmps section and the rids section</param>
        /// <param name="totalSelectEntries">Total selected entries (if any)</param>
        /// <param name="totalEntries">Total entries (if any)</param>
        void RemoveEntry(StackPanel panel, List<CheckBox> selectedList, bool isResponseIDs, int totalSelectEntries, int totalEntries)
        {
            if (totalEntries > 0)
            {
                if (totalSelectEntries > 0)
                {
                    CleanLists(panel, selectedList, true, isResponseIDs, totalEntries); //this handles removing selected entries (both partial and full selection)
                }
                else
                {
                    if (isResponseIDs)
                    {
                        ActiveResponseIDEntries.RemoveAt(totalEntries - 1);
                    }
                    else
                    {
                        ActiveTargetMoodPairEntries.Remove(ActiveTargetMoodPairEntries.ElementAt(totalEntries - 1).Key);
                    }
                    panel.Children.RemoveAt(totalEntries);
                }
            }
        }// end of RemoveEntry
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
         //Calls CheckForm and creates a new dialogline object from the data in the form assuming CheckForm returns true
        private void CreateItem_Click(object sender, RoutedEventArgs e)
        {
            FromFormToDialogLine();
        }//end of CreateItem_Click
        #region WIP Later
        //Calls ClearForm() and resets all values to allow for creation of a new dialogline
        private void NewItem_Click(object sender, RoutedEventArgs e)
        {
            //im not sure if im gonna use this
        }//end of NewItem_Click

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
        #endregion
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
            SelectOrDeselectAll(TargetMoodPairsStackPanel, AllTargetMoodPairsSelected, SelectedTargetMoodPairs);
            CheckSelectAllButtons();
        }//end of SelectAllTargetMoodPairsItem_Click
        //handles adding a new target mood pair entry -> CheckBox (DockPanel(ComboBox, ComboBox))
        private void AddTargetMoodPairItem_Click(object sender, RoutedEventArgs e)
        {
            AddTargetMoodPairEntry();
            CheckSelectAllButtons();
        }//end of AddTargetMoodPairItem_Click
        //handles removing an existing target mood pair entry; If there are selected entries, it will remove all selected, else itll remove from the bottom
        private void RemoveTargetMoodPairItem_Click(object sender, RoutedEventArgs e)
        {
            RemoveEntry(TargetMoodPairsStackPanel, SelectedTargetMoodPairs, false, TotalSelectedTargetMoodPairs, TotalTargetMoodPairs);
            CheckSelectAllButtons();
        }//end of RemoveTargetMoodPairItem_Click
        //Handles (de)selection of all response ID entries
        private void SelectAllResponseIDsItem_Click(object sender, RoutedEventArgs e)
        {
            SelectOrDeselectAll(ResponseIDsStackPanel, AllResponseIDsSelected, SelectedResponseIDs);
            CheckSelectAllButtons();
        }//end of SelectAllResponseIDsItem_Click
        //handles removing an existing response ID entry; If there are selected entries, it will remove all selected, else itll remove from the bottom
        private void RemoveResponseIDItem_Click(object sender, RoutedEventArgs e)
        {
            RemoveEntry(ResponseIDsStackPanel, SelectedResponseIDs, true, TotalSelectedResponseIDs, TotalResponseIDs);
            CheckSelectAllButtons();
        }//end of RemoveResponseIDItem_Click
        //Serves as a pseudo add function, when selection is changed, itll add to the list of response ID entries, assuming there's no prexisting one of the same type.
        private void ResponseIDsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex != 0)
            {
                AddResponseIDEntry(((ComboBox)sender).SelectedItem);
            }
            CheckSelectAllButtons();
        }//end of ResponseIDsComboBox_SelectionChanged
        #region debugging
        //Only noticeable bug was that when clicking on one of the drop downs for the target mood pairs entries, because the comboboxes are in that checkbox's content value (a dockpanel), just clicking it or it's child elements counts as "Checked" and itll register as a selected element, however the state reverts and really it doesnt affect anything. It'll only cause the select all button to change when its not supposed to (but then fix itself when the state reverts). So this is just a graphical bug until I notice it affecting something else
        void UpdateDebug()
        {
            SelTMP.Content = "Sel TMP ct: " + TotalSelectedTargetMoodPairs;
            TotTMP.Content = "Tot TMP ct: " + TotalTargetMoodPairs;
            AllSelTMP.Content = "All sel TMP: " + AllTargetMoodPairsSelected;
            SelRID.Content = "Sel RID ct: " + TotalSelectedResponseIDs;
            TotRID.Content = "Tot RID ct: " + TotalResponseIDs;
            AllSelRID.Content = "All sel RID: " + AllResponseIDsSelected;
        }
        #endregion
    }//end of MainWindow
}//end of namespace