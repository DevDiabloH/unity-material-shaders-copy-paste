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

        if (null == masterMaterial)
        {
            if(GUILayout.Button("Set Master Material", GUILayout.Height(40)))
            {
                PushMasterMaterial();
            }
        }
        else
        {
            if (GUILayout.Button("Run Paste", GUILayout.Height(40)))
            {
                Apply();
            }

            if (GUILayout.Button("Clear Material", GUILayout.Height(40)))
            {
                ClearMaterial();
            }
        }
    }

    private void PushMasterMaterial()
    {
        if (Selection.activeGameObject)
        {
            masterMaterial = GetMaterialByGameOjbect();
        }
        else if (Selection.GetFiltered(typeof(Material), SelectionMode.Assets).Length == 1)
        {
            masterMaterial = GetMaterialByProjectFolder();
        }
    }

    private void Apply()
    {
        if (Selection.gameObjects.Length != 0)
        {
            SetPasteInGameObject();
        }
        else if (Selection.GetFiltered(typeof(Material), SelectionMode.Assets).Length != 0)
        {
            SetPasteInProjectFolderSelection();
        }
    }

    private void SetPasteInProjectFolderSelection()
    {
        foreach(Material mt in Selection.GetFiltered(typeof(Material), SelectionMode.Assets))
        {
            mt.shader = masterMaterial.shader;
            mt.CopyPropertiesFromMaterial(masterMaterial);
        }

        Debug.Log("Paste Complete");
        ClearMaterial();
    }

    private void SetPasteInGameObject()
    {
        foreach(GameObject obj in Selection.gameObjects)
        {
            if (obj.GetComponent<MeshRenderer>())
            {
                foreach(Material mt in obj.GetComponent<MeshRenderer>().sharedMaterials)
                {
                    mt.shader = masterMaterial.shader;
                    mt.CopyPropertiesFromMaterial(masterMaterial);
                }
            }
        }

        Debug.Log("Paste Complete");
        ClearMaterial();
    }

    private Material GetMaterialByGameOjbect()
    {
        if (null != Selection.activeGameObject.GetComponent<MeshRenderer>())
        {
            return Selection.activeGameObject.GetComponent<MeshRenderer>().sharedMaterials[0];
        }

        Debug.LogWarning("MeshRenderer Component is not found. -> " + Selection.activeGameObject.name);
        return null;
    }

    private Material GetMaterialByProjectFolder()
    {
        if(Selection.GetFiltered(typeof(Material), SelectionMode.Assets).Length == 1)
        {
            return Selection.GetFiltered(typeof(Material), SelectionMode.Assets)[0] as Material;
        }

        Debug.LogWarning("On selection material is not found.");
        return null;
    }

    private void ClearMaterial()
    {
        masterMaterial = null;
    }
}
