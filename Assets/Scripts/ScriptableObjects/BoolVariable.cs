using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolVariable", menuName = "ScriptableObjects/BoolVariable", order = 6)]
public class BoolVariable : Variable<bool>
{
    public override void SetValue(bool value)
    {
        _value = value;
    }
    // overload
    public void SetValue(BoolVariable boolean)
    {
        SetValue(boolean.Value);
    }
    public void Toggle()
    {
        _value = !_value;
    }
}
