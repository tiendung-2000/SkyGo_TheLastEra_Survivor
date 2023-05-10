using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager Ins;

    public PlayerController activePlayer;

    private void Awake()
    {
        Ins = this;
    }
}
