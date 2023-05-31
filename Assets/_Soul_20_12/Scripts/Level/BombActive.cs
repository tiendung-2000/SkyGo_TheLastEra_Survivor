using DG.Tweening;
using UnityEngine;

public class BombActive : MonoBehaviour
{
    public static BombActive Ins;

    public PlayerController player;
    public GameObject bomb;
    public LayerMask whatIsEnemies;
    public Vector2 top_right_corner;
    public Vector2 bottom_left_corner;

    private void Awake()
    {
        Ins = this;
    }

    public void Bomb()
    {


        Collider2D[] hitEnemies = Physics2D.OverlapAreaAll(new Vector2(player.transform.position.x - top_right_corner.x, player.transform.position.y - top_right_corner.y)
                                                         , new Vector2(player.transform.position.x - bottom_left_corner.x, player.transform.position.y - bottom_left_corner.y)
                                                         , whatIsEnemies);
        if (hitEnemies.Length != 0)
        {

            AudioManager.Ins.SoundEffect(8);

            foreach (Collider2D hit in hitEnemies)
            {
                Instantiate(bomb, hit.transform.position, Quaternion.identity);
            }
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        CustomDebug.DrawRectange(new Vector2(player.transform.position.x - top_right_corner.x, player.transform.position.y - top_right_corner.y)
                               , new Vector2(player.transform.position.x - bottom_left_corner.x, player.transform.position.y - bottom_left_corner.y));
    }

#endif
}
