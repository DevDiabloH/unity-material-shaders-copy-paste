using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MaterialParamCopyAndPaste : EditorWindow
{
    private Material masterMaterial = null;

    [MenuItem("CustomTools/MaterialParamCopyAndPaste")]
    static void CopyAndPaste()
    {
        MaterialParamCopyAndPaste window = (MaterialParamCopyAndPaste)EditorWindow.GetWindow(typeof(MaterialParamCopyAndPaste));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Tool", EditorStyles.boldLabel);
        masterMaterial = EditorGUILayout.ObjectField("MasterMaterial", masterMaterial, typeof(Material), true) as Material;

        if (GUILayout.Button("Run Paste"))
        {
            if(masterMaterial != null)
            {
                Paste();
            }
            else
            {
                Debug.LogWarning("MasterMaterial is null.");
            }
        }
    }

    private void Paste()
    {
        foreach(Material mt in Selection.GetFiltered(typeof(Material), SelectionMode.Assets))
        {
            mt.shader = masterMaterial.shader;
            mt.CopyPropertiesFromMaterial(masterMaterial);
        }

        Debug.Log("Paste Complete");
    }
}
