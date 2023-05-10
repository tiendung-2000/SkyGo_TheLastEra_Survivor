using Spine.Unity;
using UnityEngine;

public class BreakablesBox : MonoBehaviour
{
    public SkeletonAnimation skeleton;

    public Collider2D col;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    private void Start()
    {
        skeleton.AnimationState.Complete += AnimationState_Complete;
    }

    private void AnimationState_Complete(Spine.TrackEntry trackEntry)
    {
        switch (trackEntry.Animation.Name)
        {
            case "Attack":
                Destroy(gameObject);
                break;
        }
    }

    public void Smash()
    {
        col.enabled = false;
        skeleton.AnimationState.SetAnimation(0, "Attack", false);

        //drop items


        if (shouldDropItem)
        {
            float dropChance = Random.Range(0f, 100f);

            if (dropChance < itemDropPercent)
            {
                int randomItem = Random.Range(0, itemsToDrop.Length);

                SmartPool.Ins.Spawn(itemsToDrop[randomItem], transform.position, transform.rotation);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (PlayerSkillManager.instance.player.canMove == false)
            {
                Smash();
            }
        }

        if (other.tag == "PlayerBullet")
        {
            Smash();
        }

        if (other.gameObject.layer == 11)
        {
            Smash();
        }
    }
}
