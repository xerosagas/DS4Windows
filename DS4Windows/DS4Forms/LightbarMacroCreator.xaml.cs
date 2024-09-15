using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using DS4Windows;
using DS4WinWPF.DS4Forms.ViewModels;

namespace DS4WinWPF.DS4Forms;

public partial class LightbarMacroCreator : Window
{
    private LightbarMacroViewModel _lightbarMacroVM;

    public event EventHandler<LightbarMacroSaveEventArgs> Save;

    public LightbarMacroCreator()
    {
        InitializeComponent();
        _lightbarMacroVM = new LightbarMacroViewModel();
        DataContext = _lightbarMacroVM;
        _lightbarMacroVM.MacroList = new();
    }

    private void SelectColor_Click(object sender, RoutedEventArgs e)
    {
        ColorPickerWindow dialog = new()
        {
            Owner = Application.Current.MainWindow
        };
        dialog.ShowDialog();
        _lightbarMacroVM.CurrentColor = dialog.colorPicker.SelectedColor.GetValueOrDefault();
    }

    private void AddColor_OnClick(object sender, RoutedEventArgs e)
    {
        var color = new DS4Color
        {
            red = _lightbarMacroVM.CurrentColor.R, green = _lightbarMacroVM.CurrentColor.G,
            blue = _lightbarMacroVM.CurrentColor.B
        };
        _lightbarMacroVM.MacroList.Add(new LightbarMacroElement(color, _lightbarMacroVM.CurrentInterval));
    }

    private void DeleteColor_OnClick(object sender, RoutedEventArgs e)
    {
        // TODO show a popup window saying you must select an item to delete
        if (MacroListBox.SelectedItems.Count == 0) return;
        _lightbarMacroVM.MacroList.Remove((LightbarMacroElement)MacroListBox.SelectedItems[0]);
    }

    private void Clear_OnClick(object sender, RoutedEventArgs e)
    {
        _lightbarMacroVM.MacroList.Clear();
    }

    private void Save_OnClick(object sender, RoutedEventArgs e)
    {
        Save?.Invoke(this, new LightbarMacroSaveEventArgs(_lightbarMacroVM.MacroList.ToArray()));
    }
}

public class LightbarMacroSaveEventArgs(LightbarMacroElement[] macroElements) : EventArgs
{
    public LightbarMacroElement[] MacroElements { get; init; } = macroElements;
}