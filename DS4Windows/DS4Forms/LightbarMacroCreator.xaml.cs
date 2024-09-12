using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using DS4WinWPF.DS4Forms.ViewModels;

namespace DS4WinWPF.DS4Forms;

public partial class LightbarMacroCreator : Window
{
    private LightbarMacroViewModel _lightbarMacroVM;

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
        _lightbarMacroVM.MacroList.Add(new MacroColor(_lightbarMacroVM.CurrentColor, _lightbarMacroVM.CurrentInterval));
    }

    private void DeleteColor_OnClick(object sender, RoutedEventArgs e)
    {
        _lightbarMacroVM.MacroList.Remove((MacroColor)MacroListBox.SelectedItems[0]);
    }

    private void Clear_OnClick(object sender, RoutedEventArgs e)
    {
        _lightbarMacroVM.MacroList.Clear();
    }

    private void Save_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}