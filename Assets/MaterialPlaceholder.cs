using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class MaterialPlaceholder
{
    public string ObjectName;
    public List<string> MaterialNameList = new List<string>();
    public List<RendererMateriaList> RendererMateriaList = new List<RendererMateriaList>();
}

[Serializable]
public class RendererMateriaList
{
    public List<string> RendererName = new List<string>();
    public List<string> MaterialNameList = new List<string>();
}

[Serializable]
public class TrueRendererMateriaList
{
    public string RendererName;
    public List<string> MaterialNameList = new List<string>();
}