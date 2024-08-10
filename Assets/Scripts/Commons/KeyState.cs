using System.Collections.Generic;
using UnityEngine;

namespace BloodWork.Commons
{
    public static class KeyStates
    {
        private static readonly Dictionary<byte, KeyState> s_Dictionary = new()
        {
            { (byte)KeyState.None,     KeyState.None     },
            { (byte)KeyState.Pressed,  KeyState.Pressed  },
            { (byte)KeyState.Held,     KeyState.Held     },
            { (byte)KeyState.Released, KeyState.Released },
        };

        public static KeyState GetState(KeyCode code)
        {
            if (Input.GetKeyDown(code))
                return KeyState.Pressed;
            
            if (Input.GetKey(code))
                return KeyState.Held;
            
            if (Input.GetKeyUp(code))
                return KeyState.Released;
            
            return KeyState.None;
        }

        public static byte GetValue(this KeyState state) => (byte)state;

        public static KeyState ValueOf(byte value) => s_Dictionary[value];

        public static bool AppliesTo(this KeyState state, KeyCode code) => state == GetState(code);
    }

    public enum KeyState : byte
    {
        None     = 0,
        Pressed  = 1 << 0,
        Held     = 1 << 1,
        Released = 1 << 2
    }
}
