using UnityEngine;
using UnityEngine.UI;

public class OnClickCoolDownSetup : MonoBehaviour
{
    [SerializeField] Button button;
    void OnEnable()
    {
        button.onClick.AddListener(PlayerSkillManager.instance.UpdateSkillCoolDown);
    }
}

