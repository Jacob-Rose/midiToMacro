using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace midiToMacroFinal.KeyFunctions.Macro
{
    class MacroKeyFunction : GenericKeyFunction
    {
        public List<MacroString> macros = new List<MacroString>();
        public bool macroRunning = false;

        public override string ToString()
        {
            return "Macro";
        }

        public override string getToolTip()
        {
            return null;
        }

        public override void ResetFunctionToDefault()
        {
            macros = new List<MacroString>();
        }

        public override void onButtonPressed()
        {

        }

        public override void onButtonReleased()
        {

        }

        public override void EditFunction()
        {
            FunctionSettingsWindow settingsWindow = new FunctionSettingsWindow();

            ListBox macroListBox = new ListBox();
            refreshMacroListBox();
            Button button_AddMacro = new Button();
            Button button_EditMacro = new Button();
            settingsWindow.ShowDialog();
        }

        private void updateMacroListUI()
        {
            /*
            while (macroListBox.Items.Count > 0)
                macroListBox.Items.RemoveAt(0);
            if (currentKeyCol != -1)
            {
                foreach (MacroString macro in launchpadDevice.launchpadKeys[currentKeyRow, currentKeyCol].macros)
                {
                    macroListBox.Items.Add(macro);
                }
            }
            */
        }
        private void refreshMacroListBox()
        {

        }

        private void macroListEdit_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (macroListBox.SelectedIndex != -1)
            {
                MacroString macro = launchpadDevice.launchpadKeys[currentKeyRow, currentKeyCol].macros.ElementAtOrDefault<MacroString>(macroListBox.SelectedIndex);
                Window window = new Window();
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20, GridUnitType.Pixel) });
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.RowDefinitions.Add(new RowDefinition());

                Canvas tooltipGrid = new Canvas()
                {
                    Width = 250,
                    Height = 100,

                };

                tooltipGrid.Children.Add(new TextBlock() { Text = "SHIFT Modifier:  +" });
                tooltipGrid.Children.Add(new TextBlock() { Text = "CTRL Modifier:  ^", Margin = new Thickness(0, 20, 0, 0) });
                tooltipGrid.Children.Add(new TextBlock() { Text = "ALT Modifier:  %", Margin = new Thickness(0, 40, 0, 0) });
                tooltipGrid.Children.Add(new TextBlock() { Text = "Repeat key: {key #} Ex. {Left 42}{h 10}", Margin = new Thickness(0, 60, 0, 0) });
                tooltipGrid.Children.Add(new TextBlock() { Text = "Modify sequence: modifier(keys) Ex. ^(EC)", Margin = new Thickness(0, 80, 0, 0) });

                Button helpButton = new Button();
                helpButton.VerticalAlignment = VerticalAlignment.Stretch;
                helpButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                helpButton.SetValue(Grid.RowProperty, 0);
                helpButton.SetValue(Grid.ColumnProperty, 0);
                helpButton.ToolTip = new ToolTip() { Content = tooltipGrid, StaysOpen = true };

                TextBox macroText = new TextBox() { HorizontalAlignment = HorizontalAlignment.Stretch };
                macroText.SetValue(Grid.RowProperty, 0);
                macroText.SetValue(Grid.ColumnProperty, 1);
                macroText.VerticalAlignment = VerticalAlignment.Stretch;
                macroText.HorizontalAlignment = HorizontalAlignment.Stretch;
                macroText.Margin = new Thickness(20);
                macroText.Text = macro.macro;
                macroText.TextWrapping = TextWrapping.Wrap;

                IntegerUpDown delay = new IntegerUpDown();
                delay.SetValue(Grid.RowProperty, 0);
                delay.SetValue(Grid.ColumnProperty, 2);
                delay.VerticalAlignment = VerticalAlignment.Stretch;
                delay.HorizontalAlignment = HorizontalAlignment.Stretch;
                delay.Margin = new Thickness(20);
                delay.Text = macro.delay.ToString();
                grid.Children.Add(macroText);
                grid.Children.Add(delay);
                grid.Children.Add(helpButton);
                window.Width = 400;
                window.Height = 150;
                window.Content = grid;
                window.ShowDialog();

                launchpadDevice.launchpadKeys[currentKeyRow, currentKeyCol].macros.RemoveAt(macroListBox.SelectedIndex);
                launchpadDevice.launchpadKeys[currentKeyRow, currentKeyCol].macros.Insert(macroListBox.SelectedIndex, new MacroString(macroText.Text, int.Parse(delay.Text)));
            }
            updateMacroListUI();
            */
        }

        private void macroListDelete_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (launchpadDevice.launchpadKeys[currentKeyRow, currentKeyCol].macros.Count > 0)
            {
                launchpadDevice.launchpadKeys[currentKeyRow, currentKeyCol].macros.RemoveAt(macroListBox.SelectedIndex);
            }
            updateMacroListUI();
            */
        }

        /// <summary>
        /// FUCK YEA THIS WORKS SO EASY BITCH
        /// </summary>
        public async void runMacro()
        {
            macroRunning = true;
            foreach (MacroString m in macros)
            {
                await System.Threading.Tasks.Task.Delay(m.delay);
                
            }
            macroRunning = false;
        }

    }
}
