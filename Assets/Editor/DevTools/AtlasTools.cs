using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEditor;
using UnityEngine;
using static Codice.Client.Commands.WkTree.WorkspaceTreeNode;
using UnityEngine.U2D;
using System.IO;

public class AtlasTools
{
    [MenuItem("HybridCLR/Create All Atlas")]
    public static void CreateAllAtlas()
    {
        string texturePath = Application.dataPath + "/Resources/UI/UITextures";
        string atlasPath = "Assets/Bundle/Atlas/";

        //获取texture下子目录文件，先按子目录文件打图集
        DirectoryInfo di = new DirectoryInfo(texturePath);
        DirectoryInfo[] subDir = di.GetDirectories();
        foreach (var d in subDir)
        {
            CreateAtlas(atlasPath + d.Name + ".spriteatlas", d.FullName);
        }
    }

    public static void CreateAtlas(string atlasPath, string dir)
    {
        var sprite_altas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasPath);
        if (sprite_altas == null)
        {
            sprite_altas = new SpriteAtlas();
            AssetDatabase.CreateAsset(sprite_altas, atlasPath);
        }

        List<string> result_dirs = new List<string>();
        string[] extensions = new string[] { ".png" };
        ToolsUtil.GetDirs(dir, ref result_dirs, extensions);
        foreach (var d in result_dirs)
        {
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(d);
            List<Object> packables = new List<Object>(sprite_altas.GetPackables());
            if (!packables.Contains(sprite))
            {
                sprite_altas.Add(new Object[] { sprite });
            }
        }
        sprite_altas.SetIncludeInBuild(true);
        SpriteAtlasPackingSettings sps = new SpriteAtlasPackingSettings();
        sps.enableRotation = false;
        sps.enableTightPacking = false;
        sps.padding = 4;
        sprite_altas.SetPackingSettings(sps);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

}
