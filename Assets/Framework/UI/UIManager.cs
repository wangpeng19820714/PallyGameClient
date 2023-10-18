using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;
using GameFramework.Resource;

namespace GameFramework.UI
{
    public enum UIName
    {
        UITest,
        _None = -1,
    }

    public class _UIData
    {
        public _UIData(string prefabPath, UILayer layer)
        {
            _PrefabPath = prefabPath;
            _Layer = layer;
        }
        public string _PrefabPath;
        public UILayer _Layer;
    }

    public enum UILayer
    {
        SceneLayer = 0,//废弃
        NormalLayer = 1,//Base,只能存在一个UI
        TopLayer,//弹出UI层
        DialogueLayer,//剧情对话层
        TipsLayer,//提示ui
        LoadingLayer,
    }
    public class UIManager : MonoSingleton<UIManager>
    {
        public delegate void UILoadFinishDel(object param);

        //ui根节点
        private Transform m_UIRoot;

        private Dictionary<int, Transform> m_Layers;
        private Dictionary<int, GameObject> m_AllWindows;
        private Dictionary<int, Transform> UIDefine;

        //开启新界面时来源UI，用于关闭之后，重新打开
        UIName _OpenFromID = UIName._None;

        //ui摄像机
        private Camera m_UICamera;

        protected override void Init()
        {
            base.Init();

            m_UIRoot = transform;

            m_UICamera = m_UIRoot.Find("UICamera")?.GetComponent<Camera>();

            InitLayers();

            m_AllWindows = new Dictionary<int, GameObject>();
            UIDefine = new Dictionary<int, Transform>();
        }

        public void CleanUp()
        {
            CloseAllUIByLayer(UILayer.NormalLayer, true, true);
            CloseAllUIByLayer(UILayer.TopLayer, true, true);
            CloseAllUIByLayer(UILayer.DialogueLayer, true, true);
            CloseAllUIByLayer(UILayer.TipsLayer, true, true);

        }

        /// <summary>
        /// 初始化所有layer
        /// </summary>
		void InitLayers()
        {
            m_Layers = new Dictionary<int, Transform>();

            if (m_UIRoot == null)
            {
                return;
            }
            int layerIdx = 0;
            InitOneLayer(UILayer.SceneLayer, layerIdx++);
            InitOneLayer(UILayer.NormalLayer, layerIdx++);
            InitOneLayer(UILayer.TopLayer, layerIdx++);
            InitOneLayer(UILayer.DialogueLayer, layerIdx++);
            InitOneLayer(UILayer.TipsLayer, layerIdx++);
            InitOneLayer(UILayer.LoadingLayer, layerIdx++);
        }

        private void InitOneLayer(UILayer layer, int LayerOrder)
        {
            var t = m_UIRoot.Find(layer.ToString());
            if (t != null)
            {
                m_Layers.Add((int)layer, t);
            }
            Canvas c = t.GetComponent<Canvas>();
            if (c == null)
            {
                return;
            }

            CanvasScaler cs = t.GetComponent<CanvasScaler>();
            if (cs == null)
            {
                return;
            }

            c.sortingOrder = LayerOrder * 1000;
            //这里将不同层级plane分开，避免z轴重合时出现顺序不明
            c.planeDistance = (LayerOrder + 1) * 10;
            //
            if (m_UICamera == null)
            {
                c.renderMode = RenderMode.ScreenSpaceOverlay;
            }
            else
            {
                c.renderMode = RenderMode.ScreenSpaceCamera;
                c.worldCamera = m_UICamera;
            }

            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;


        }

        public UnityEngine.Transform GetLayer(int layer)
        {
            if (!m_Layers.ContainsKey(layer))
            {
                return null;
            }
            return m_Layers[layer];
        }

        void Start()
        {

        }

        void Update()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiName"></param>
        /// <param name="del"></param>
        /// <param name="param"></param>
        /// <returns></returns>
		public bool OpenUI(UIInfo uiInfo, UILoadFinishDel del = null, object param = null)
        {
            //有的话就显示出来
            if (m_AllWindows.ContainsKey(uiInfo.uiID))
            {
                Utils.SetGameObjectActive(m_AllWindows[uiInfo.uiID], true);
                if (del != null)
                {
                    del(param);
                }
                return true;
            }
            string path = uiInfo.uiPath;
#if SCREEN_H
            path = "UI_H/" + d._PrefabPath;
#endif
            SortingLayer. layer = SortingLayer.NameToID(uiInfo.sortingLayer);
            UILayer layer = d._Layer;
            ResourceManager.Instance.LoadUIPrefab(path, (obj, p) =>
            {
                GameObject go = obj as GameObject;
                if (go == null)
                {
                    return;
                }
                //加到对应layer
                var parent = m_Layers[(int)layer];
                go.transform.SetParent(parent);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                Utils.SetGameObjectActive(go, true);
                if (!m_AllWindows.ContainsKey(uiName))
                {
                    m_AllWindows.Add(uiName, go);
                }
                if (del != null)
                {
                    del(p);
                }
            }, param);

            return true;
        }

        public void PreloadUI(UIInfo uiInfo)
        {
            if (!UIDefine.ContainsKey(uiInfo.uiID))
            {
                return;
            }
            if (m_AllWindows.ContainsKey(uiInfo.uiID))
            {
                return;
            }
            string path = uiInfo.uiPath;
#if SCREEN_H
            path = "UI_H/" + d._PrefabPath;
#endif
            UILayer layer = d._Layer;
            m_AllWindows.Add(uiInfo.uiID, ResourceManager.Instance.LoadUIPrefabSync(path));
            CloseUI(uiInfo.uiID);
        }

        public void CloseUI(UIName uiName, bool bDestroy = false)
        {
            CloseUI((int)uiName, bDestroy);
        }

        /// <summary>
        /// 默认是隐藏
        /// </summary>
        /// <param name="uiName"></param>
		public void CloseUI(int uiName, bool bDestroy = false)
        {
            if (!m_AllWindows.ContainsKey(uiName))
            {
                return;
            }
            if (bDestroy)
            {
                GameObject.Destroy(m_AllWindows[uiName]);
                m_AllWindows.Remove(uiName);
                return;
            }
            Utils.SetGameObjectActive(m_AllWindows[uiName], false);
        }

        //只关闭normal层和top
        public void CloseAllUI()
        {
            foreach (var ui in m_AllWindows.Keys)
            {
                var d = UIDefine[ui];
                if (d._Layer == UILayer.NormalLayer || d._Layer == UILayer.TopLayer)
                    CloseUI(ui);
            }
        }

        /// <summary>
        /// 保留来源UI，关闭所有UI，打开新UI
        /// </summary>
        /// <param name="新UI"></param>
        /// <param name="来源UI"></param>
        /// <param name="del"></param>
        /// <param name="param"></param>
        public void OpenUIAndStayOld(UIName uiName, UIName fromName, UILoadFinishDel del = null, object param = null)
        {
            CloseAllUI();
            _OpenFromID = fromName;
            OpenUI((int)uiName, del, param);
        }

        public void CloseOpenStayUI(UIName uiName, bool bDestroy = false)
        {
            if (_OpenFromID != UIName._None)
            {
                OpenUI(_OpenFromID);
                _OpenFromID = UIName._None;
            }
            CloseUI(uiName, bDestroy);
        }


        public void CloseAllUIByLayer(UILayer layer, bool bDestroy = false, bool bDontAnim = false)
        {
            CloseAllUIByLayer((int)layer, bDestroy, bDontAnim);
        }

        public void CloseAllUIByLayer(int layer, bool bDestroy = false, bool bDontAnim = false)
        {
            List<int> closeList = new List<int>();
            foreach (var ui in m_AllWindows.Keys)
            {
                var d = UIDefine.UIDic[ui];
                if (d._Layer == (UILayer)layer)
                    closeList.Add(ui);

            }
            foreach (var k in closeList)
            {
                CloseUI(k, bDestroy);
            }
        }
    }

}

