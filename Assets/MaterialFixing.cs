using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using Object = UnityEngine.Object;

public class MaterialFixing : MonoBehaviour
{
    public string MaterialName;
    
    public List<Renderer> Renderers;
    public List<Material> Materials;
    public List<Material> Materialx;

    public List<GameObject> FBX;

    public List<Material> EffectMaterial;

    public MPList MpList;

    public List<TrueRendererMateriaList> TrueRendererMateriaLists = new List<TrueRendererMateriaList>();

    [Button]
    public void GetTheMaterialName()
    {
    }
    
    private void Start()
    {
        GetAllMaterialFromAssets();
        GetMaterial();
    }

    [Button]
    public void FixTheMaterial()
    {
        MpList = (MPList)AssetDatabase.LoadAssetAtPath("Assets/MPList.asset",typeof(ScriptableObject));
        TrueRendererMateriaLists = MpList.RMLists;
        GetAllMaterialFromAssets();
        FixAllMaterialFromAssets();
        GetMaterial();
    }

    [Button]
    public void GetAllFBXFromAssets()
    {
        FBX = new List<GameObject>();
        string[] aMaterialFiles = Directory.GetFiles(Application.dataPath, "*.fbx", SearchOption.AllDirectories);
        foreach(string matFile in aMaterialFiles)
        {
            string assetPath = "Assets" + matFile.Replace(Application.dataPath, "").Replace('\\', '/');
            GameObject sourceMat = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));

            if (sourceMat.GetComponent<Renderer>()==null)
            {
                Debug.Log("KOSONG MASHE");
            }
            else
            {
                Debug.Log(sourceMat.GetComponent<Renderer>().sharedMaterial.name);
            }
            FBX.Add(sourceMat);
            
            Debug.Log(sourceMat.name);
        }
    }

    [Button]
    public void MassRemap()
    {
       
    }
    
    public void GetAllMaterialFromAssets()
    {
        Materialx = new List<Material>();
        string[] aMaterialFiles = Directory.GetFiles(Application.dataPath, "*.mat", SearchOption.AllDirectories);
        foreach(string matFile in aMaterialFiles)
        {
            string assetPath = "Assets" + matFile.Replace(Application.dataPath, "").Replace('\\', '/');
            Material sourceMat = (Material)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Material));
            Materialx.Add(sourceMat);
        }
    }
    public void FixAllMaterialFromAssets()
    {
        foreach (var mat in Materialx)
        {
            if (mat.shader.name == "Hidden/InternalErrorShader")
            {
                mat.shader = Shader.Find("Standard 2-Sided");
            }
        }        
    }
    
    public void GetMaterial()
    {
        Renderers = new List<Renderer>();
        Materials = new List<Material>();
        Renderers = GameObject.FindObjectsOfType<Renderer>().ToList();
        foreach (var ren in Renderers)
        {
            int j = 0;
            foreach (var listOriginalFBXName in MpList.RMLists)
            {
                if (ren.name == listOriginalFBXName.RendererName)
                {
                    
                }
                foreach (var mats in ren.sharedMaterials)
                {
                    EffectMaterial = new List<Material>();
                    if (mats == null)
                    {
                        int k = 0;
                        Debug.Log("Missing Material at Renderer: " + ren.name + "!");
                        foreach (var listOriginalFBX in MpList.RMLists)
                        {
                            if (listOriginalFBX.RendererName == ren.name)
                            {
                                int indexke = k;
                                Debug.Log(indexke);
                                foreach (var jumlahMaterial in MpList.RMLists[indexke].MaterialNameList)
                                {
                                    var mate = Materialx.Where(x => x.name == jumlahMaterial).FirstOrDefault();
                                    if (mate == null)
                                    {
                                        Debug.Log("MATERIAL MISSING!: " + jumlahMaterial);
                                    }
                                    else
                                    {
                                        Debug.Log(jumlahMaterial);
                                        EffectMaterial.Add(mate);
                                    }
                                }
                            }

                            k++;
                        }

                        j++;
                    }
                    else
                    {
                        Materials.Add(mats);
                    }

                    if (mats == null)
                    {
                        ren.materials = EffectMaterial.ToArray();
                    }
                }
            }
        }
    }
}
