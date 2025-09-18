using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/Resource", order = 1)]
public class Resource : ScriptableObject
{
    [SerializeField] private string _saveKey;

    public bool AnimateAtCollection;
    public GameObject ResourcePrefab;
    public Sprite Sprite;
    public AudioClip CollectedSFX;
    public int DivideBy;
    public string SaveKey
    {
        get
        {
            // Uses Objects Name if _saveKey is empty
            if (string.IsNullOrEmpty(_saveKey))
            {
                _saveKey = name;

#if UNITY_EDITOR
                EditorUtility.SetDirty(this); 
                AssetDatabase.SaveAssets();   
#endif
            }
            return _saveKey;
        }
        set
        {
            _saveKey = value;

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();   
#endif
        }
    }

    public int InitialAmount;
    public bool IsConsumable;
    public List<ResourceRequirement> UnlockRequirements;

    private int currentAmount;

    public int CurrentAmount
    {
        get => currentAmount;
        set => currentAmount = Mathf.Max(0, value); 
    }

    private void OnEnable()
    {
        CurrentAmount = InitialAmount;
    }
}
