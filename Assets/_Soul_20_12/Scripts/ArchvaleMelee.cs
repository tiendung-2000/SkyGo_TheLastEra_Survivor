//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ArchvaleMelee : MonoBehaviour
//{
//    [SerializeField] Vector2 weaponOffset;  // Recommended: (1, 0, 0)
//    [SerializeField] float weaponRot;                                  // Recommended: 135
//    [SerializeField] float swingSpeed;                                          // Recommended: 10

//    int swing = 1;
//    GameObject anchor;
//    Vector3 target;
//    float swingAngle;
//    bool swinging;

//    void Start()
//    {
//        anchor = transform.parent.gameObject;
//    }

//    void Update()
//    {
//        // Anchor rotation
//        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
//        Vector3 rot = anchor.transform.eulerAngles;
//        swingAngle = Mathf.Lerp(swingAngle, swing * 90, Time.deltaTime * swingSpeed);
//        rot.z = angle + swingAngle;
//        anchor.transform.eulerAngles = rot;

//        // Weapon rotation
//        float t = swing == 1 ? 45 : -225;
//        target.z = Mathf.Lerp(target.z, t, Time.deltaTime * swingSpeed);
//        if (Mathf.Abs(t - target.z) < 5) swinging = false;
//        transform.localRotation = Quaternion.Euler(target);

//        if (Input.GetKeyDown(KeyCode.Mouse0))
//        {
//            if (swinging) return;

//            // Attack
//            swing *= -1;
//            swinging = true;
//        }
//    }
//}
