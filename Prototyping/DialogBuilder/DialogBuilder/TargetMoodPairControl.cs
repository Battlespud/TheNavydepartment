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
    /// Struct used for containing the necessary elements to display the targets list, moods list, and checkbox for each row in an enclosing dockpanel
    /// </summary>
    public struct TargetMoodPairControl
    {
        public DockPanel tmpPanel;
        public CheckBox selectBox;
        public ComboBox targets;
        public ComboBox moods;

        /// <summary>
        /// Base ctor; pass ref to parent panel and select all checkbox
        /// </summary>
        /// <param name="panel">Parent Panel</param>
        /// <param name="selectAll">Select All checkbox for the TMP section</param>
        public TargetMoodPairControl(StackPanel panel, CheckBox selectAll)
        {
            tmpPanel = new DockPanel();
            selectBox = new CheckBox() { VerticalAlignment = VerticalAlignment.Bottom };
            targets = new ComboBox() { Width = 92 };
            moods = new ComboBox();
            panel.Children.Add(tmpPanel);
            PopulateValues(selectAll);
        }//end of ctor(StackPanel panel, CheckBox selectAll)

        /// <summary>
        /// Populates, per instance, a DialogLines target|mood pairs
        /// </summary>
        /// <param name="selectAll"></param>
        void PopulateValues(CheckBox selectAll)
        {
            TargetMoodPairControl self = this;
            tmpPanel.Children.Add(selectBox);
            tmpPanel.Children.Add(targets);
            tmpPanel.Children.Add(moods);

            selectBox.Checked += (object sender, RoutedEventArgs e) =>
            {
                MainWindow.SelectedTMPs.Add(self);
                if (MainWindow.SelectedTMPs.Count == MainWindow.TMPCount && !(bool)selectAll.IsChecked)
                {
                    selectAll.IsChecked = true;
                }
            };//end of selectBox.Checked

            selectBox.Unchecked += (object sender, RoutedEventArgs e) =>
            {
                MainWindow.SelectedTMPs.Remove(self);
                if (MainWindow.SelectedTMPs.Count <= 0 && (bool)selectAll.IsChecked)
                {
                    selectAll.IsChecked = false;
                }
            };//end of selectBox.Unchecked

            foreach (CharacterNames name in Enum.GetValues(typeof(CharacterNames)))
            {
                targets.Items.Add(name);
            }

            foreach (MoodTypes moodType in Enum.GetValues(typeof(MoodTypes)))
            {
                moods.Items.Add(moodType);
            }
        }//end of PopulateValues
    }//end of TargetMoodPairControl
}//end of namespace
