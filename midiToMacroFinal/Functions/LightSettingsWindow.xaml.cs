using System.Windows;

namespace midiToMacroFinal.KeyFunctions
{
    /// <summary>
    /// Interaction logic for LightSettingsWindow.xaml
    /// </summary>
    public partial class LightSettingsWindow : Window
    {
        public LightSettingsWindow(int redPressed, int greenPressed, int redReleased, int greenReleased)
        {
            InitializeComponent();
            slider_RedPressed.Value = redPressed;
            slider_GreenPressed.Value = greenPressed;
            slider_RedReleased.Value = redReleased;
            slider_GreenReleased.Value = greenReleased;

            Closing += ((sender, e) => {
                DialogResult = true;
            });
        }
    }
}
