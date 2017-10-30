using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeaspoonTools.Utils
{
    /// <summary>
    /// Lets you pass a value by reference into a coroutine.
    /// </summary>
    /// <typeparam name="T">Type of the value you want to pass.</typeparam>
    class RefVal<T> where T: struct
    {
        public T value;

        public RefVal(ref T value)
        {
            this.value = value;
        }
    }
}
