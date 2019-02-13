using midiToMacroFinal.KeyFunctions;
using midiToMacroFinal.KeyFunctions.Macro;
using midiToMacroFinal.KeyFunctions.ModeChange;
using midiToMacroFinal.KeyFunctions.Sound;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace midiToMacroFinal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        Button[,] launchpadButtonUI = new Button[9,9]; //UI Button Grid

        //keeps track of current launchpad key being edited
        int currentKeyRow = -1;
        int currentKeyCol = -1;
        bool UIUpdating = false;

        public MainWindow()
        {
            InitializeComponent();
            UIUpdating = true;
            GlobalVariables.currentMode = 0;
            GlobalVariables.ModeChangedEvent += ((sender, e) => {
                
            });

            #region "Creates Button Array
            Grid buttonGrid = new Grid();
            buttonGrid.Name = "buttonGrid";
            for(int i = 0; i < 9; i++)
            {
                buttonGrid.ColumnDefinitions.Add(new ColumnDefinition());
                buttonGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int col = 0; col < 9; col++)
            {
                for (int row = 0; row < 9; row++)
                {
                    double size = Double.NaN;
                    Button btn = new Button();
                    btn.Width = size;
                    btn.Height = size;
                    btn.Click += buttonClick;
                    btn.HorizontalAlignment = HorizontalAlignment.Stretch;
                    btn.VerticalAlignment = VerticalAlignment.Stretch;
                    btn.Content = intToString(row-1) + (col+1);
                    btn.Name = "button" + (row) + (col);
                    btn.Margin = new Thickness(1);
                    btn.Style = Application.Current.Resources["launchpadUISquareButton"] as Style;
                    btn.SetValue(Grid.RowProperty, row);
                    btn.SetValue(Grid.ColumnProperty, col);
                    btn.AllowDrop = true;
                    btn.Drop += Btn_Drop;
                    if (row == 0 || col == 8)
                    {
                        if (row == 0 && col == 8)
                        {
                            btn.IsEnabled = false;
                            btn.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            btn.Style = Application.Current.Resources["launchpadUICircleButton"] as Style;
                            if (row == 0)
                            {
                                btn.Content = "";
                            }
                        }
                    }
                    launchpadButtonUI[row, col] = btn;
                    buttonGrid.Children.Add(btn);

                }
            }
            //set here as i have a reference in the designer... see im smart
            buttonBoarder.Width = Double.NaN;
            buttonBoarder.Height = Double.NaN;
            buttonBoarder.Child = buttonGrid;
            #endregion
            
            MidiDeviceEngine.launchpadDevice = new LaunchpadDevice(); //initilize main launchpad device
            refreshButtonUI();
            UIUpdating = false;
            Loaded += MainWindow_Loaded;
            Closing += ((sender, e) =>
            {
                MidiDeviceEngine.launchpadDevice.resetLaunchpad();
            });
        }

        /// <summary>
        /// Sends the files to the key
        /// </summary>
        /// <param name="sender">button that had recieved a drop</param>
        /// <param name="e">contains files that were dropped</param>
        private void Btn_Drop(object sender, DragEventArgs e)
        {
            //to-do: add functionality for function files that load a specific function, key files that load a specific key with many functions
            Button btn = ((Button)sender);
            int row = (int)btn.GetValue(Grid.RowProperty);
            int col = (int)btn.GetValue(Grid.ColumnProperty);
            LaunchpadKey key = MidiDeviceEngine.launchpadDevice.launchpadKeyArrays[modeComboBox.SelectedIndex][row, col];
            key.fileDropHandler(e);
            openKeyInfo(row, col);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MidiDeviceEngine.midi = new Midi();
            MidiDeviceEngine.midi.OutputDevicesEnumerated += MidiDeviceEngine.Midi_OutputDevicesEnumerated;
            MidiDeviceEngine.midi.InputDevicesEnumerated += MidiDeviceEngine.Midi_InputDevicesEnumerated;

            MidiDeviceEngine.midi.Initialize();
        }

        

        private void buttonClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            openKeyInfo((int)btn.GetValue(Grid.RowProperty), (int)btn.GetValue(Grid.ColumnProperty));
        }

        private void openKeyInfo(int row, int col)
        {
            UIUpdating = true;
            currentKeyCol = col;
            currentKeyRow = row;
            functionsList.ItemsSource = MidiDeviceEngine.launchpadDevice.launchpadKeyArrays[modeComboBox.SelectedIndex][currentKeyRow, currentKeyCol].functions;

            buttonNameTextBox.Text = ((string)launchpadButtonUI[row, col].Content);
            UIUpdating = false;
        }

        public void refreshFunctionList()
        {
            functionsList.ItemsSource = null;
            for(int i = 0; i < functionsList.Items.Count; i++)
            {
                functionsList.Items[i] = functionsList.Items[i];
            }
            functionsList.ItemsSource = MidiDeviceEngine.launchpadDevice.launchpadKeyArrays[modeComboBox.SelectedIndex][currentKeyRow, currentKeyCol].functions;
        }

        private void menuSaveProfile_Click(object sender, RoutedEventArgs e)
        {
            /*
            SaveFileDialog file = new SaveFileDialog();

            if (file.ShowDialog() == true)
            {
                System.IO.StreamWriter writer = new System.IO.StreamWriter(file.FileName);
                foreach (var x in launchpadDevice.launchpadKeys)
                {
                    writer.Write(x.row);
                    writer.WriteLine(x.col);
                    writer.WriteLine(launchpadButtonUI[x.row, x.col].Content);
                    writer.Write(x.redPressed);
                    writer.Write(x.redReleased);
                    writer.Write(x.greenPressed);
                    writer.WriteLine(x.greenReleased);
                    writer.WriteLine(x.activatedFunction);

                    switch (x.activatedFunction)
                    {
                        case 0:
                            break;
                        case 1:
                            if(x.audioSource != null)
                            {
                                writer.WriteLine(x.audioSource);
                            }
                            else
                            {
                                writer.WriteLine();
                            }
                            writer.Write(Convert.ToInt32(x.soundStopOnRelease));
                            writer.Write(Convert.ToInt32(x.soundLoop));
                            
                            break;
                        case 2:
                            writer.WriteLine(x.macros.Count);
                            for(int i = 0; i < x.macros.Count; i++)
                            {
                                writer.WriteLine(x.macros.ElementAt<MacroString>(i).macro);
                                writer.WriteLine(x.macros.ElementAt<MacroString>(i).delay);
                            }
                            break;
                    }
                    writer.WriteLine();
                }
                writer.WriteLine("END");
                writer.Close();
            }
            */
        }

        private void menuOpenProfile_Click(object sender, RoutedEventArgs e)
        {
            /*
            OpenFileDialog file = new OpenFileDialog();

            if (file.ShowDialog() == true)
            {
                launchpadDevice.resetLaunchpad();
                System.IO.StreamReader reader = new StreamReader(file.FileName);
                bool shouldContinue = true;
                while (shouldContinue)
                {
                    string  pos = reader.ReadLine();
                    if (!pos.Equals("END")) {
                        int row = int.Parse(pos.ElementAt<char>(0).ToString());
                        int col = int.Parse(pos.ElementAt<char>(1).ToString());

                        LaunchpadKey key = launchpadDevice.launchpadKeys[row, col];

                        launchpadButtonUI[row, col].Content = reader.ReadLine();
                        key.redPressed = int.Parse(((char)reader.Read()).ToString());
                        key.redReleased = int.Parse(((char)reader.Read()).ToString());
                        key.greenPressed = int.Parse(((char)reader.Read()).ToString());
                        key.greenReleased = int.Parse(((char)reader.Read()).ToString());
                        reader.ReadLine();
                        int function = int.Parse(reader.ReadLine());
                        key.activatedFunction = function;
                        switch (function)
                        {
                            case 0:
                                break;
                            case 1:
                                string path = reader.ReadLine();
                                if (path != "")
                                    key.soundPlayer_Open(path);
                                key.soundStopOnRelease = Convert.ToBoolean(int.Parse(((char)reader.Read()).ToString()));
                                key.soundLoop = Convert.ToBoolean(int.Parse(((char)reader.Read()).ToString()));
                                break;
                            case 2:
                                int macroCount = int.Parse(reader.ReadLine());
                                for (int i = 0; i < macroCount; i++)
                                {
                                    MacroString macro = new MacroString();
                                    macro.macro = reader.ReadLine();
                                    macro.delay = int.Parse(reader.ReadLine());
                                    key.macros.Add(macro);
                                }
                                break;
                        }


                    }
                    else
                    {
                        shouldContinue = false;
                    }
                    reader.ReadLine();   
                }
                reader.Close();
                updateUI();
                launchpadDevice.updateButtons();
            }
            */
        }

        private void menuResetProfile_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Are you sure you wish to restart?", "You will lose everything.", MessageBoxButton.YesNo).Equals(MessageBoxResult.Yes))
            {
                currentKeyRow = -1;
                currentKeyCol = -1;
                MidiDeviceEngine.launchpadDevice.resetLaunchpad();
                refreshButtonUI();
            }

        }

        private void refreshButtonUI(int row, int col)
        {
            LaunchpadKey key = MidiDeviceEngine.launchpadDevice.launchpadKeyArrays[modeComboBox.SelectedIndex][row, col];
            if (key.currentKeyColorRed == 0 && key.currentKeyColorGreen == 0)
            {
                launchpadButtonUI[row, col].Background = new SolidColorBrush(Color.FromRgb(180, 180, 180));
            }
            else
            {
                int redBG = 255;
                int rGreenB = 255;
                double power = key.functions[key.currentFunction].lightControl.greenPressed + key.functions[key.currentFunction].lightControl.redPressed;
                if (key.functions[key.currentFunction].lightControl.greenPressed > key.functions[key.currentFunction].lightControl.redPressed)
                {
                    redBG = 85 * key.functions[key.currentFunction].lightControl.redPressed;
                }
                else if (key.functions[key.currentFunction].lightControl.greenPressed < key.functions[key.currentFunction].lightControl.redPressed)
                {
                    rGreenB = 70 * key.functions[key.currentFunction].lightControl.greenPressed;
                }
                else
                {
                    redBG = 255;
                    rGreenB = 255;
                }
                if (key.functions[key.currentFunction].lightControl.greenPressed == 3 || key.functions[key.currentFunction].lightControl.redPressed == 3)
                {
                    power = 2;
                }
                else if (key.functions[key.currentFunction].lightControl.greenPressed == 2 || key.functions[key.currentFunction].lightControl.redPressed == 2)
                {
                    power = 1.3;
                }
                else
                {
                    power = 0.9;
                    rGreenB = 200;
                }
                RadialGradientBrush rad = new RadialGradientBrush();
                rad.GradientStops.Add(new GradientStop(Color.FromRgb((byte)redBG, (byte)rGreenB, 0), 0));
                rad.GradientStops.Add(new GradientStop(Color.FromRgb(180, 180, 180), power));
                launchpadButtonUI[row, col].Background = rad;
            }
        }

        private void refreshButtonUI()
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    refreshButtonUI(i, j);
                }
            }
        }

        private void refreshUI()
        {
            refreshButtonUI();

            refreshFunctionList();

            refreshModeComboBox();
            
        }

        

        private void addFunctionButton_Click(object sender, RoutedEventArgs e)
        {
            if(currentKeyRow > -1 && currentKeyCol > -1)
            {
                Window window = new Window();
                window.Width = 400;
                window.Height = 100;
                window.WindowStyle = WindowStyle.ToolWindow;
                ComboBox functions = new ComboBox();
                functions.IsTextSearchEnabled = true;
                functions.Margin = new Thickness(10);
                functions.SetValue(Grid.ColumnProperty, 0);
                Button okButton = new Button();
                okButton.Content = "OK";
                okButton.Margin = new Thickness(10);
                okButton.SetValue(Grid.ColumnProperty, 1);
                okButton.Click += ((s, d) =>
                {
                    window.DialogResult = true;
                    window.Close();
                });
                List<GenericKeyFunction> funcList = new List<GenericKeyFunction>() { new LightKeyFunction(), new MacroKeyFunction(), new SoundKeyFunction(), new ModeChangeKeyFunction() };
                functions.ItemsSource = funcList;
                functions.SelectedIndex = 0;

                Grid g = new Grid();
                g.RowDefinitions.Add(new RowDefinition());
                g.ColumnDefinitions.Add(new ColumnDefinition());
                g.ColumnDefinitions.Add(new ColumnDefinition() {
                    Width = new GridLength(100, GridUnitType.Pixel)
                });
                g.Children.Add(okButton);
                g.Children.Add(functions);

                window.Content = g;
                if (window.ShowDialog() == true)
                {
                    MidiDeviceEngine.launchpadDevice.launchpadKeyArrays[modeComboBox.SelectedIndex][currentKeyRow, currentKeyCol].functions.Add((GenericKeyFunction) functions.SelectedItem);
                    refreshFunctionList();

                } 
            }
        }

        private void editFunctionSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MidiDeviceEngine.launchpadDevice.launchpadKeyArrays[modeComboBox.SelectedIndex][currentKeyRow, currentKeyCol].functions[functionsList.SelectedIndex].EditFunction();
            MidiDeviceEngine.launchpadDevice.updateButtonLight(currentKeyRow, currentKeyCol);
            refreshFunctionList();
        }

        private void editFunctionLightsButton_Click(object sender, RoutedEventArgs e)
        {
            MidiDeviceEngine.launchpadDevice.launchpadKeyArrays[modeComboBox.SelectedIndex][currentKeyRow, currentKeyCol].functions[functionsList.SelectedIndex].EditLighting();
            MidiDeviceEngine.launchpadDevice.updateButtonLight(currentKeyRow, currentKeyCol);
        }

        private void removeFunctionButton_Click(object sender, RoutedEventArgs e)
        {
            if(functionsList.HasItems)
            {
                MidiDeviceEngine.launchpadDevice.launchpadKeyArrays[modeComboBox.SelectedIndex][currentKeyRow, currentKeyCol].functions.Remove((GenericKeyFunction)functionsList.SelectedItem);
                refreshFunctionList();
            }
            
        }

        private void buttonNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!UIUpdating)
            launchpadButtonUI[currentKeyRow, currentKeyCol].Content = ((TextBox)sender).Text;
        }

        private void functionsList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
            {
                removeFunctionButton_Click(sender, null);
            }
        }

        private void buttonAddMode_Click(object sender, RoutedEventArgs e)
        {
            MidiDeviceEngine.launchpadDevice.launchpadKeyArrays.Add(new LaunchpadKey[9, 9]);
            refreshModeComboBox();
        }

        private void refreshModeComboBox()
        {
            //List<string> modes = new List<string>();
            modeComboBox.Items.Clear();
            foreach(LaunchpadKey[,] keyArray in MidiDeviceEngine.launchpadDevice.launchpadKeyArrays)
            {
                
            }
        }

        private void buttonEditMode_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menuInfoHelp_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.google.com"); //goto help page
        }

        private void menuDocsHelp_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.google.com"); //goto doc site
        }
    }
}
