using UnityEngine;
using Game.Hotfix.TableData;
using GameFramework.Resource;
using GameFramework;

namespace Game.Hotfix
{
    public class DataExample : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            SceneData sceneData =ResourceManager.Instance.LoadDataAssetSync<SceneData>("SceneData");
            Log.Debug("ID{0} --- Value: {1}", sceneData.dataArray[9].Id, sceneData.dataArray[9].Name);
        }
    }
}

