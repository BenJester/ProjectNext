#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace EModules {
[ExecuteInEditMode]
class DescriptionRegistrator : MonoBehaviour, IDescriptionRegistrator {
    [HideInInspector]
    public string _cachedData;
    
    void OnEnable()
    {   if ((hideFlags & HideFlags.DontSaveInBuild) == 0) hideFlags |= HideFlags.DontSaveInBuild;
        HierarchyExtensions.Utilities.RegistrateDescription(this);
    }
    
    public string cachedData
    {   get { return _cachedData; }
        set { _cachedData = value; }
    }
    
    public Component component
    {   get { return this; }
    }
}
}
#endif
