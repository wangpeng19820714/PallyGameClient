using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public static partial class Utility
    {
        public static partial class CommonTools
        {
            public static void SetGameObjectActive(GameObject go, bool active)
            {
                if (go == null)
                {
                    return;
                }
                if (go.activeSelf.Equals(active))
                {
                    return;
                }
                go.SetActive(active);
            }
        }
    }
}

