namespace GameFramework.UI
{
    public struct UIInfo
    {
        public int uiID;
        public string uiName;
        public string uiPath;
        public int sortingLayer;

        public UIInfo(int id, string name, string path, int layer)
        {
            uiID = id;
            uiName = name;
            uiPath = path;
            sortingLayer = layer;
        }
    }
}

