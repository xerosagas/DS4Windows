using System.Linq;

namespace DS4Windows;

// TODO: 1. use time instead of tick
// 2. add an option to change the debounce time in the ui
public class Debouncer(int maxChecks)
{
    /// <summary>
    ///     Last <see cref="maxChecks"/> of key states.
    /// </summary>
    private readonly bool[] _state = new bool[maxChecks];

    /// <summary>
    ///     Pointer to state array.
    /// </summary>
    private int _index;

    public bool Check(bool reading)
    {
        _state[_index] = reading;

        ++_index;
        if (_index != maxChecks) return true;

        _index = 0;
        return _state.Any(x => x);
    }
}