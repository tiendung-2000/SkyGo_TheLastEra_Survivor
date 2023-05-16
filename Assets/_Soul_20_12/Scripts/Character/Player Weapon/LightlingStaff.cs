//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LightlingStaff : MonoBehaviour
//{
//    [SerializeField] List<EnemyController> allEnemies;
//    [SerializeField] List<LR_Controller> allLines;

//    [SerializeField] LR_Controller linePrefabs;

//    private bool weaponIsOn;

//    private void Awake()
//    {
//        allLines = new List<LR_Controller>();

//        for (int i = 0; i < allEnemies.Count; i++)
//        {
//            LR_Controller newLine = Instantiate(linePrefabs);
//            allLines.Add(newLine);

//            newLine.AssignTarget(transform.position, allEnemies[i].transform);
//            newLine.gameObject.SetActive(false);
//        }
//    }

//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Escape))
//        {
//            ToggleWeapon();
//        }
//    }

//    private void ToggleWeapon()
//    {
//        if (weaponIsOn)
//        {
//            foreach (var line in allLines)
//            {
//                line.gameObject.SetActive(false);
//            }

//            foreach (var enemy in allEnemies)
//            {
//                Debug.Log("Nohit");
//            }

//            weaponIsOn = false;
//        }
//        else
//        {
//            foreach (var line in allLines)
//            {
//                line.gameObject.SetActive(true);
//            }

//            foreach (var enemy in allEnemies)
//            {
//                Debug.Log("hit");
//            }

//            weaponIsOn = true;
//        }
//    }
//}
