using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class ToolsUtil
{
    public static void GetDirs(string dirPath, ref List<string> dirs, string[] extensions = null)
    {
        foreach (string path in Directory.GetFiles(dirPath))
        {
            if (extensions != null && extensions.Length > 0)
            {
                for (int i = 0; i < extensions.Length; i++)
                {
                    //��ȡ�����ļ����а�����׺ ��·��
                    if (System.IO.Path.GetExtension(path).ToLower().Equals(extensions[i]))
                    {
                        dirs.Add(path.Substring(path.IndexOf("Assets")));
                        Log.Debug(path.Substring(path.IndexOf("Assets")));
                        break;
                    }
                }
            }
            else
            {
                //�����˺�׺
                dirs.Add(path.Substring(path.IndexOf("Assets")));
                Log.Debug(path.Substring(path.IndexOf("Assets")));
            }
        }

        if (Directory.GetDirectories(dirPath).Length > 0)  //���������ļ���
        {
            foreach (string path in Directory.GetDirectories(dirPath))
            {
                GetDirs(path, ref dirs, extensions);
            }
        }
    }
}
