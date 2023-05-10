using UnityEngine;

public class FindingEnemy : MonoBehaviour
{
    PlayerController player;

    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    void Update()
    {

        #region Old
        //Finding Enemy
        //GameObject[] gos;
        //float distance = 200f;
        //GameObject[] enemyTag = GameObject.FindGameObjectsWithTag("Enemy");

        //GameObject[] bossTag = GameObject.FindGameObjectsWithTag("Boss");

        //gos = enemyTag.Concat(bossTag).ToArray();

        //GameObject closest = null;
        //Vector3 position = transform.position;
        //foreach (GameObject go in gos)
        //{
        //    Vector3 diff = go.transform.position - position;
        //    float curDistance = diff.sqrMagnitude;
        //    if (curDistance < distance)
        //    {
        //        closest = go;
        //        distance = curDistance;
        //    }
        //}

        //if (closest != null)
        //{
        //    player.theHand.transform.right = closest.transform.position - transform.position;
        //    Debug.DrawLine(gameObject.transform.position, closest.transform.position, Color.red);

        //    if (this.transform.position.x > closest.transform.position.x)
        //    {
        //        transform.localScale = new Vector3(-1f, 1f, 1f);
        //        player.gunArm.localScale = new Vector3(-1f, -1f, 1f); 
        //    }
        //    else
        //    {
        //        transform.localScale = Vector3.one;
        //        player.gunArm.localScale = Vector3.one;
        //    }
        //}
        //else
        //{
        //    if (UltimateJoystick.GetJoystickStateOutSide("Player Movement JoyStick") != false)
        //    {
        //        player.theHand.transform.right = player.moveInput;

        //        //rotate weapon by joystick
        //        if (this.transform.position.x > player.moveInput.x + gameObject.transform.position.x)
        //        {
        //            transform.localScale = new Vector3(-1f, 1f, 1f);
        //            player.gunArm.localScale = new Vector3(-1f, -1f, 1f);
        //        }
        //        else
        //        {
        //            transform.localScale = Vector3.one;
        //            player.gunArm.localScale = Vector3.one;
        //        }
        //    }
        //    Debug.DrawLine(gameObject.transform.position, player.moveInput + new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Color.red);
        #endregion

        GameObject[] enemyTag = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bossTag = GameObject.FindGameObjectsWithTag("Boss");
        GameObject[] gos = new GameObject[enemyTag.Length + bossTag.Length];
        enemyTag.CopyTo(gos, 0);
        bossTag.CopyTo(gos, enemyTag.Length);

        GameObject closest = null;
        float distance = 200f;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        if (closest != null)
        {
            player.theHand.right = closest.transform.position - transform.position;
            //Debug.DrawLine(gameObject.transform.position, closest.transform.position, Color.red);

            if (this.transform.position.x > closest.transform.position.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                player.theHand.localScale = new Vector3(-1f, -1f, 1f);
            }
            else
            {
                transform.localScale = Vector3.one;
                player.theHand.localScale = Vector3.one;
            }
        }

        else if (UltimateJoystick.GetJoystickStateOutSide("Player Movement JoyStick") != false)
        {
            player.theHand.right = player.moveInput;

            //rotate weapon by joystick
            if (this.transform.position.x > player.moveInput.x + gameObject.transform.position.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                player.theHand.localScale = new Vector3(-1f, -1f, 1f);
            }
            else
            {
                transform.localScale = Vector3.one;
                player.theHand.localScale = Vector3.one;
            }
        }
    }
}
