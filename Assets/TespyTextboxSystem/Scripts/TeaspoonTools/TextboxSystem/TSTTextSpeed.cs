using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeaspoonTools.TextboxSystem
{
    [System.Serializable]
    public enum TextSpeed
    {
        // these values amount to characters printed per second
        verySlow = 8,
        slow = (int)(verySlow * 2.5),
        medium = (int)(slow * 2.5),
        fast = (int)(medium * 2.8),
        instant = 999
    }
}
