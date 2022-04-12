using UnityEngine;
using UnityEditor;
using System.IO;

public class GradientToTexture : EditorWindow
{
    [MenuItem("Window/GradientToTexture")]
    static void Open ()
    {
        GetWindow<GradientToTexture> ();
    }


    private string m_filePath = "";
    private string m_TextureName = "Gradient";
    private Gradient m_Gradient = new Gradient ();
    private int m_Resolution = 256;

    void OnGUI ()
    {
        GUILayoutOption[] m_option = new GUILayoutOption[]
        {
            GUILayout.ExpandWidth(true),
            GUILayout.ExpandHeight(true),
        };
        GUILayout.Width(0);
        GUILayout.Height(0);
        m_filePath = EditorGUILayout.TextField("File Path",m_filePath,m_option);
        m_TextureName = EditorGUILayout.TextField("Texture Name",m_TextureName,m_option);
        m_Gradient = EditorGUILayout.GradientField("Gradient",m_Gradient,m_option);
        m_Resolution = EditorGUILayout.IntField("Resoulution",m_Resolution,m_option);
        if(GUILayout.Button("Save Gradient to Texture",m_option))
        {
            SaveGradient();
        }
    }

    public void SaveGradient ()
    {
        Texture2D tex = new Texture2D (m_Resolution, 1);
        for (int i = 0; i < m_Resolution; i++)
            tex.SetPixel (i, 0, m_Gradient.Evaluate (i / (float)m_Resolution));
        tex.Apply ();

        byte[] data = tex.EncodeToPNG ();

        string path = "";
        string dirPath = "";
        if(m_filePath != "")
        {
            path = string.Format ("{0}/{1}/{2}.png", Application.dataPath, m_filePath, m_TextureName);
            dirPath = string.Format ("{0}/{1}", Application.dataPath, m_filePath);
        }
        else
        {
            path = string.Format ("{0}/{1}.png", Application.dataPath, m_TextureName);
        }

        if(Directory.Exists(dirPath))
        {
            File.WriteAllBytes (path, data);
        }
        else
        {
            Directory.CreateDirectory(dirPath);
            File.WriteAllBytes (path, data);
        }

        #if UNITY_EDITOR
        AssetDatabase.Refresh();
        #endif
    }
}