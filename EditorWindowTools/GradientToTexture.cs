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
    public Gradient[] m_Gradient = {new Gradient()};
    private int m_Resolution = 256;
    private SerializedObject m_so;
    private void OnEnable() {
        ScriptableObject target = this;
        m_so = new SerializedObject(target);
    }

    void OnGUI ()
    {
        m_filePath = EditorGUILayout.TextField("File Path",m_filePath);
        m_TextureName = EditorGUILayout.TextField("Texture Name",m_TextureName);
        m_Resolution = EditorGUILayout.IntField("Resoulution",m_Resolution);

        m_so.Update();
        SerializedProperty m_gradProperty = m_so.FindProperty("m_Gradient");
        EditorGUILayout.PropertyField(m_gradProperty, true);
        m_so.ApplyModifiedProperties();

        if(GUILayout.Button("Save Gradient to Texture"))
        {
            SaveGradient();
        }
    }

    public void SaveGradient ()
    {
        Texture2D m_tex = new Texture2D (m_Resolution, m_Gradient.Length);
        for (int j = 0; j < m_Gradient.Length; j++)
        {
            for (int i = 0; i < m_Resolution; i++)
                m_tex.SetPixel (i, j+1, m_Gradient[j].Evaluate (i / (float)m_Resolution));
            m_tex.Apply ();
        }

        byte[] m_data = m_tex.EncodeToPNG ();

        string m_path = "";
        string m_dirPath = "";
        if(m_filePath != "")
        {
            m_path = string.Format ("{0}/{1}/{2}.png", Application.dataPath, m_filePath, m_TextureName);
            m_dirPath = string.Format ("{0}/{1}", Application.dataPath, m_filePath);
        }
        else
        {
            m_path = string.Format ("{0}/{1}.png", Application.dataPath, m_TextureName);
        }

        if(Directory.Exists(m_dirPath))
        {
            File.WriteAllBytes (m_path, m_data);
        }
        else
        {
            Directory.CreateDirectory(m_dirPath);
            File.WriteAllBytes (m_path, m_data);
        }

        AssetDatabase.Refresh();
    }
}