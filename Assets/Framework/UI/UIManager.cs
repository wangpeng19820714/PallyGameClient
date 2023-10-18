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
        SceneLayer = 0,//����
        NormalLayer = 1,//Base,ֻ�ܴ���һ��UI
        TopLayer,//����UI��
        DialogueLayer,//����Ի���
        TipsLayer,//��ʾui
        LoadingLayer,
    }
    public class UIManager : MonoSingleton<UIManager>
    {
        public delegate void UILoadFinishDel(object param);

        //ui���ڵ�
        private Transform m_UIRoot;

        private Dictionary<int, Transform> m_Layers;
        private Dictionary<int, GameObject> m_AllWindows;
        private Dictionary<int, Transform> UIDefine;

        //�����½���ʱ��ԴUI�����ڹر�֮�����´�
        UIName _OpenFromID = UIName._None;

        //ui�����
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
        /// ��ʼ������layer
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
            //���ｫ��ͬ�㼶plane�ֿ�������z���غ�ʱ����˳����
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
            //�еĻ�����ʾ����
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
                //�ӵ���Ӧlayer
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
        /// Ĭ��������
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

        //ֻ�ر�normal���top
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
        /// ������ԴUI���ر�����UI������UI
        /// </summary>
        /// <param name="��UI"></param>
        /// <param name="��ԴUI"></param>
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

