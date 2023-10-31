using UnityEngine;
using Game.Hotfix.TableData;
using GameFramework.Data;
using GameFramework;

namespace Game.Hotfix
{
    public class DataExample : MonoBehaviour
    {
        SceneData sceneData;
        // Start is called before the first frame update
        void Start()
        {
            sceneData = DataManager.Instance.GetDataTable<SceneData>("SceneData");
            Log.Debug("ID{0} --- Value: {1}", sceneData.dataArray[9].Id, sceneData.dataArray[9].Name);
        }
    }
}

