
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[Serializable]
public class MPList : ScriptableObject
{
    public List<MaterialPlaceholder> MPLists = new List<MaterialPlaceholder>();
    public List<TrueRendererMateriaList> RMLists = new List<TrueRendererMateriaList>();
}
