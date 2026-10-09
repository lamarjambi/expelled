using System.Collections.Generic;
using UnityEngine;

namespace Expelled.Player
{
    public static class PlayerState
    {
        public static Vector3 Position;
        public static float Health;
        public static bool HasData;
        public static bool TextTriggered;
        public static bool DoorOpened;
        public static List<GameObject> PersistentRagdolls = new List<GameObject>();

        public static void DestroyRagdolls()
        {
            // :desc: destroy all tracked ragdolls and clear the list
            foreach (var r in PersistentRagdolls)
                if (r != null) Object.Destroy(r);
            PersistentRagdolls.Clear();
        }

        public static void Reset()
        {
            // :desc: wipe all persistent state back to defaults (called on full game reset)
            DestroyRagdolls();
            Position = Vector3.zero;
            Health = 0f;
            HasData = false;
            TextTriggered = false;
            DoorOpened = false;
        }
    }
}
