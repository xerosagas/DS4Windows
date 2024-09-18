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

public class LightbarMacro(LightbarMacroElement[] elements, bool active, LightbarMacroTrigger trigger)
{
    public LightbarMacroElement[] Elements = elements;
    public bool Active = active;
    public LightbarMacroTrigger Trigger = trigger;
}

public class LightbarMacroElement(DS4Color color, int length)
{
    public DS4Color Color { get; init; } = color;
    public int Length { get; init; } = length;

    public override string ToString()
    {
        return $"{Color.ToString()}, {Length} ms";
    }

    public override bool Equals(object obj)
    {
        var casted = (LightbarMacroElement)obj;
        if (casted is null) return false;
        return casted.Color.Equals(Color) && casted.Length.Equals(Length);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + Color.GetHashCode();
            hash = hash * 23 + Length.GetHashCode();
            return hash;
        }
    }
}

public enum LightbarMacroTrigger
{
    Press,
    Release
}