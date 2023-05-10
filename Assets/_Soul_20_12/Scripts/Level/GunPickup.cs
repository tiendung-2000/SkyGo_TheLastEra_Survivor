﻿using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public Gun theGun;

    public int ammo;

    public float waitToBeCollected = .5f;

    void Update()
    {
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && waitToBeCollected <= 0)
        {
            bool hasGun = false;
            foreach (Gun gunToCheck in PlayerController.Ins.availableGuns)
            {
                //if hasgun
                //print(PlayerController.Ins.availableGuns.Count);
                //Debug.Log(theGun.weaponName + "  -  " + gunToCheck.weaponName);
                if (theGun.weaponName == gunToCheck.weaponName)
                {
                    hasGun = true;
                    theGun.PickupAmmo(ammo);
                }
            }

            if (!hasGun)
            {
                Gun gunClone = Instantiate(theGun);
                gunClone.transform.parent = PlayerController.Ins.theHand;
                gunClone.transform.position = PlayerController.Ins.theHand.position;
                gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                //gunClone.transform.localScale = Vector3.one;

                PlayerController.Ins.availableGuns.Add(gunClone);
                PlayerController.Ins.currentGun = PlayerController.Ins.availableGuns.Count - 1;
                PlayerController.Ins.SwitchGun();
            }

            Destroy(gameObject);

            //AudioManager.instance.PlaySFX(7);
        }
    }
}