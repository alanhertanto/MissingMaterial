using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEditor.AssetImporters;

public class CreateMaterialFromDescription : AssetPostprocessor
{
    public List<string> TestMaterialName = new List<string>();
    public MaterialPlaceholder MaterialPlaceholderx;
    public RendererMateriaList RendererMateriaLists;
    public TrueRendererMateriaList TrueRendererMateriaList;
    public MPList MpList;
    public Renderer Renderer;
    public List<Material> Materials;

    public List<string> RendererMatPrev = new List<string>();
    public List<int> ListOfNumber = new List<int>();
    
    private int TotalObjek = 0;
    private void OnPreprocessAsset()
    {
        //ubah 2-2nya scriptable yang ngehold data saja, bukan scriptable nya
    }

    private void OnPreprocessModel()
    {
/*        MaterialPlaceholderx = ScriptableObject.CreateInstance<MaterialPlaceholder>();*/
        MpList = (MPList)AssetDatabase.LoadAssetAtPath("Assets/MPList.asset",typeof(ScriptableObject));
        TotalObjek = MpList.MPLists.Count;
/*        if (MpList.MPLists.Count > 0)
        {*/
/*        }
        else
        {
            MpList = ScriptableObject.CreateInstance<MPList>();
        }*/
        MaterialPlaceholderx = new MaterialPlaceholder();     
        RendererMateriaLists = new RendererMateriaList();
        TrueRendererMateriaList = new TrueRendererMateriaList();
    }


    void OnPostprocessModel(GameObject g)
    {
        Apply(g.transform);
        List<Renderer> Rende = new List<Renderer>();
    }
    void OnPostprocessPrefab(GameObject g)
    {

    }
    private Material OnAssignMaterialModel(Material material, Renderer renderer)
    {
        RendererMateriaLists.RendererName.Add(renderer.name);
        RendererMateriaLists.MaterialNameList.Add(material.name);
            return null;
    }

    private void OnPostprocessMeshHierarchy(GameObject root)
    {
    }

    void Apply(Transform t)
    {    
        MaterialPlaceholderList.ObjectName = t.name;
        MaterialPlaceholderx.ObjectName = MaterialPlaceholderList.ObjectName;
        MpList.MPLists.Add(MaterialPlaceholderx);
        MpList.MPLists[TotalObjek].RendererMateriaList.Add(RendererMateriaLists);

/*        if (MpList.MPLists.Count == 0)
        {
        UnityEditor.AssetDatabase.CreateAsset(MpList, "Assets/MPList.asset");
        }*/
        TotalObjek += 1;

        RendererMatPrev = new HashSet<string>(RendererMateriaLists.RendererName).ToList();
        foreach (var uniqueName in RendererMatPrev)
        {
            TrueRendererMateriaList = new TrueRendererMateriaList();
            TrueRendererMateriaList.RendererName = uniqueName;
            for (int i = 0; i < RendererMateriaLists.RendererName.Count; i++)
            {
                if (uniqueName == RendererMateriaLists.RendererName[i])
                {
                    TrueRendererMateriaList.MaterialNameList.Add(RendererMateriaLists.MaterialNameList[i]);
                }            
            }
            MpList.RMLists.Add(TrueRendererMateriaList);
        }

        
        /*for (int i = 0; i < RendererMateriaLists.RendererName.Count-1; i++)
        {
        }*/
        Debug.Log(JsonConvert.SerializeObject(MpList.RMLists));
        EditorUtility.SetDirty(MpList);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void OnPreprocessMaterialDescription(MaterialDescription description, Material material, AnimationClip[] materialAnimation)
    {
        TestMaterialName.Add(material.name);
        MaterialPlaceholderList.MaterialNameList = TestMaterialName; 
        MaterialPlaceholderx.MaterialNameList = MaterialPlaceholderList.MaterialNameList;
    }
}