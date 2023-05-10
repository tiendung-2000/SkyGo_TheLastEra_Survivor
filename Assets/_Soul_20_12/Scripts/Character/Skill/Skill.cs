using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/NewSkill")]

public class Skill : ScriptableObject
{
    public float skillDuration;
    public float cooldownSkill;
}
