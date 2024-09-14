using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using DS4Windows;

namespace DS4WinWPF.DS4Forms.ViewModels;

public class LightbarMacroViewModel
{
    public ObservableCollection<LightbarMacroElement> MacroList { get; set; }

    private Color _currentColor;
    public Color CurrentColor
    {
        get => _currentColor;
        set
        {
            _currentColor = value;
            CurrentColorString = value.ToString();
            CurrentColorStringChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public string CurrentColorString { get; private set; }

    public event EventHandler CurrentColorStringChanged;

    private int _currentInterval;
    public int CurrentInterval
    {
        get => _currentInterval;
        set
        {
            _currentInterval = value;
            CurrentIntervalChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler CurrentIntervalChanged;

    public LightbarMacroViewModel()
    {
        CurrentColor = Color.FromRgb(255, 255, 255);
    }
}

public class LightbarMacroElement(DS4Color color, int length)
{
    public DS4Color Color { get; init; } = color;
    public int Length { get; init; } = length;

    public override string ToString()
    {
        return $"{Color.ToString()}, {Length} ms";
    }
}