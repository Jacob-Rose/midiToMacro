using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace midiToMacroFinal.KeyFunctions
{
    public abstract class GenericKeyFunction
    {
        public GenericKeyFunction()
        {
            ResetFunctionToDefault();
        }


        public LightControl lightControl = new LightControl();

        public abstract void onButtonPressed();
        public abstract void onButtonReleased();
        public abstract void ResetFunctionToDefault();
        public abstract void EditFunction();

        public void EditLighting()
        {
            LightSettingsWindow window = new LightSettingsWindow(lightControl.redPressed, lightControl.greenPressed, lightControl.redReleased, lightControl.greenReleased);

            /*
            List<Point> buttons = new List<Point>();
            #region "Creates Button Array
            Button[,] launchpadButtonUI = new Button[9, 9];

            Grid buttonGrid = new Grid();
            Border buttonBorder = new Border();
            buttonGrid.Name = "buttonGrid";
            for (int i = 0; i < 9; i++)
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
                    btn.Click += ((sender,e)=> {
                        buttons
                    });
                    btn.HorizontalAlignment = HorizontalAlignment.Stretch;
                    btn.VerticalAlignment = VerticalAlignment.Stretch;
                    btn.Content = MainWindow.intToString(row - 1) + (col + 1);
                    btn.Name = "button" + (row) + (col);
                    btn.Margin = new Thickness(1);
                    btn.Style = Application.Current.Resources["launchpadUISquareButton"] as Style;
                    btn.SetValue(Grid.RowProperty, row);
                    btn.SetValue(Grid.ColumnProperty, col);
                    btn.AllowDrop = true;
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
            buttonBorder.Width = Double.NaN;
            buttonBorder.Height = Double.NaN;
            buttonBorder.Child = buttonGrid;
            #endregion

            Grid mainGrid = new Grid();
            window.Content = mainGrid;

            mainGrid.Children.Add(buttonBorder);
            */

            if (window.ShowDialog() == true)
            {
                lightControl.redPressed = (int)window.slider_RedPressed.Value;
                lightControl.greenPressed = (int)window.slider_GreenPressed.Value;
                lightControl.redReleased = (int)window.slider_RedReleased.Value;
                lightControl.greenReleased = (int)window.slider_GreenReleased.Value;
            }
        }

        public override abstract string ToString();
        public abstract string getToolTip();
    }

    public class LightControl
    {
        public int redPressed;
        public int greenPressed;
        public int redReleased;
        public int greenReleased;
        public byte colorPressed { get { return getColor(redPressed, greenPressed); } }
        public byte colorReleased { get { return getColor(redReleased, greenReleased); } }

        public byte getColor(int red, int green)
        {
            return (byte)(12 + (green * 16) + red);
        }

        public LightControl()
        {
            setToDefault();
        }
        public void setToDefault()
        {
            redPressed = 1;
            greenPressed = 1;
            redReleased = 0;
            greenReleased = 0;
        }
    }
}
