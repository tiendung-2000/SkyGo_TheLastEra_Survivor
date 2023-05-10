using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Level Generator")]
public class LevelSO : ScriptableObject
{
    public int id;
    public GameObject levelPrefab;
    public string levelName;
    public int priceToUnlock;
    public bool isUnlock;
}
