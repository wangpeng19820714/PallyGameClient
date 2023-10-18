using System.Collections.Generic;
using UnityEngine;
using GameFramework.UI;

namespace Game
{
    public class UIDefine
    {
        public static Vector2 DefaultResolution = new Vector2(1080, 1920);


        /// <summary>
        /// 需要动态载入的ui 定义
        /// </summary>
        public static Dictionary<int, _UIData> UIDic = new Dictionary<int, _UIData>()
        {
            {(int)UIName.UITest, new _UIData("UITest",UILayer.NormalLayer) },
        };
    }
}


