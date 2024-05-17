using UnityEngine;

public class ScriptableObjectBase : ScriptableObject
{
    public string Id;
    public string Name;
    public string Description;
    protected virtual void GenerateUniqueId() { }
}

