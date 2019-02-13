using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace midiToMacroFinal.KeyFunctions.Sound
{
    class SoundKeyFunction : GenericKeyFunction
    {
        private string audioSource;
        private string fileName { get {
                return Path.GetFileName(audioSource);
                    } }
        private bool stopOnRelease;
        private bool loop;
        private double volume;
        private double pitch;

        private LowLatencySoundPlayer soundPlayer;

        public override string ToString()
        {
            if (soundPlayer == null)
            {
                return "Unset Sound Player";
            }
            else
            {
                return "Sound Player: " + fileName;
            }
        }

        public override void EditFunction()
        {
            FunctionSettingsWindow settingsWindow = new FunctionSettingsWindow();
            settingsWindow.WindowStyle = WindowStyle.ToolWindow;

            Grid g = new Grid();
            g.ColumnDefinitions.Add(new ColumnDefinition());
            g.ColumnDefinitions.Add(new ColumnDefinition());
            g.RowDefinitions.Add(new RowDefinition());
            g.RowDefinitions.Add(new RowDefinition());

            Button Button_openAudioFile = new Button();
            Button_openAudioFile.SetValue(Grid.RowProperty, 0);
            Button_openAudioFile.SetValue(Grid.ColumnProperty, 0);
            Button_openAudioFile.SetValue(Grid.ColumnSpanProperty, 2);
            Button_openAudioFile.Content = "Open Audio File";
            Button_openAudioFile.VerticalAlignment = VerticalAlignment.Stretch;
            Button_openAudioFile.HorizontalAlignment = HorizontalAlignment.Stretch;
            Button_openAudioFile.Click += ((sender, e) => {
                soundPlayer_OpenDialog();
            });

            CheckBox CheckBox_shouldAudioLoop = new CheckBox();
            CheckBox_shouldAudioLoop.SetValue(Grid.RowProperty, 1);
            CheckBox_shouldAudioLoop.SetValue(Grid.ColumnProperty, 0);
            CheckBox_shouldAudioLoop.Content = "Should Loop?";
            CheckBox_shouldAudioLoop.IsChecked = loop;

            CheckBox CheckBox_shouldStopOnRelease = new CheckBox();
            CheckBox_shouldStopOnRelease.SetValue(Grid.RowProperty, 1);
            CheckBox_shouldStopOnRelease.SetValue(Grid.ColumnProperty, 1);
            CheckBox_shouldStopOnRelease.Content = "Should Stop Playing on Release?";
            CheckBox_shouldStopOnRelease.IsChecked = stopOnRelease;

            settingsWindow.Content = g;
            g.Children.Add(Button_openAudioFile);
            g.Children.Add(CheckBox_shouldAudioLoop);
            g.Children.Add(CheckBox_shouldStopOnRelease);

            if (settingsWindow.ShowDialog() == true)
            {
                loop = (bool)CheckBox_shouldAudioLoop.IsChecked;
                stopOnRelease = (bool)CheckBox_shouldStopOnRelease.IsChecked;
            }
        }

        public override string getToolTip()
        {
            throw new NotImplementedException();
        }

        public override void ResetFunctionToDefault()
        {
            soundPlayer = null;
            loop = false;
            stopOnRelease = false;
            volume = 1;
            pitch = 1;
            audioSource = null;
        }

        public override void onButtonPressed()
        {
            if(soundPlayer != null)
            {
                soundPlayer.Play();
            }
        }

        public override void onButtonReleased()
        {
            if (soundPlayer != null)
            {
                if (stopOnRelease)
                {
                    soundPlayer.Stop();
                }
            }
        }

        public void soundPlayer_OpenDialog()
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == true)
            {
                soundPlayer_Open(file.FileName);
            }
        }

        public void soundPlayer_Open(string file)
        {
            soundPlayer = new LowLatencySoundPlayer(file, 3);
            audioSource = file;
        }
    }
}
