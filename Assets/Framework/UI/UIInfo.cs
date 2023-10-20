namespace GameFramework.UI
{
    public struct UIInfo
    {
        public int uiID;
        public string uiName;
        public string uiPath;
        public UILayer Layer;

        public UIInfo(int id, string name, string path, UILayer layer)
        {
            uiID = id;
            uiName = name;
            uiPath = path;
            Layer = layer;
        }
    }
}

