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
    public struct TargetMoodPairControl
    {
        public DockPanel tmpPanel;
        public CheckBox selectBox;
        public ComboBox targets;
        public ComboBox moods;

        public TargetMoodPairControl(StackPanel panel, CheckBox selectAll)
        {
            tmpPanel = new DockPanel();
            selectBox = new CheckBox() { VerticalAlignment = VerticalAlignment.Bottom };
            targets = new ComboBox() { Width = 92 };
            moods = new ComboBox();
            panel.Children.Add(tmpPanel);
            PopulateValues(selectAll);
        }

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
            };

            selectBox.Unchecked += (object sender, RoutedEventArgs e) =>
            {
                MainWindow.SelectedTMPs.Remove(self);
                if (MainWindow.SelectedTMPs.Count <= 0 && (bool)selectAll.IsChecked)
                {
                    selectAll.IsChecked = false;
                }
            };

            foreach (CharacterNames name in Enum.GetValues(typeof(CharacterNames)))
            {
                targets.Items.Add(name);
            }

            foreach (MoodTypes moodType in Enum.GetValues(typeof(MoodTypes)))
            {
                moods.Items.Add(moodType);
            }
        }
    }
}
