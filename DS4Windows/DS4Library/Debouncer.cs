using System;
using System.Collections.Generic;
using System.Linq;

namespace DS4Windows;

public class Debouncer(TimeSpan duration)
{
    /// <summary>
    ///     List of last states of the button.
    /// </summary>
    private readonly List<bool> _state = new();
    private bool _previousState;
    private bool _currentlyDebouncing;
    private long _debounceStartTime;

    /// <summary>
    ///     Processes the input and applies debouncing if required.
    /// </summary>
    /// <param name="input"><c>bool</c> indicating the state of the button</param>
    /// <param name="timestamp">Current timestamp in <c>DateTime.Ticks</c></param>
    /// <returns>Processed input with debouncing applied</returns>
    public bool ProcessInput(bool input, long timestamp)
    {
        if (_currentlyDebouncing)
        {
            return Debounce(input, timestamp);
        }

        if (!_currentlyDebouncing && _previousState != input)
        {
            StartDebouncing(input, timestamp);
            return true;
        }

        _previousState = input;
        return input;
    }

    private void StartDebouncing(bool input, long timestamp)
    {
        _currentlyDebouncing = true;
        Debounce(input, timestamp);
        _debounceStartTime = timestamp;
    }

    private void StopDebouncing()
    {
        _currentlyDebouncing = false;
        _state.Clear();
    }

    private bool Debounce(bool reading, long timestamp)
    {
        _state.Add(reading);

        // if the duration hasn't been reached yet, we return true as if the button was pressed all this time
        if (timestamp - _debounceStartTime < duration.TotalMilliseconds * TimeSpan.TicksPerMillisecond) return true;

        // this detects whether the button was actually released, if all values are false in the state window, this
        // function returns false, if any of them are true, a bounce (or hold) is detected, so we return true
        var trueState = _state.Any(x => x);
        StopDebouncing();
        return trueState;
    }
}