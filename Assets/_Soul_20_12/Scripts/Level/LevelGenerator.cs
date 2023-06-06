using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Ins;

    public Transform generatorParent;

    public GameObject layoutRoom;
    public Color startColor, endColor, bossRoomColor, gunRoomColor;

    public int distanceToEnd;
    public bool includeBoss;
    public int minDistanceToBoss, maxDistanceToBoss;
    public bool includeGunRoom;
    public int minDistanceToGunRoom, maxDistanceToGunRoom;

    public Transform generatorPoint;

    public enum Direction { up, right, down, left };
    public Direction selectedDirection;

    public float xOffset/* = 18f*/, yOffset/* = 10*/;

    public LayerMask whatIsRoom;

    private GameObject endRoom, bossRoom/*, gunRoom*/;

    private List<GameObject> layoutRoomObjects = new List<GameObject>();

    public RoomPrefabs rooms;

    private List<GameObject> generatedOutlines = new List<GameObject>();

    public RoomCenter centerStart, centerEnd/*, centerBoss, centerGunRoom*/;
    public RoomCenter[] potentialCenters;

    public int createCount = 1;

    // Start is called before the first frame update

    private void Awake()
    {
        Ins = this;
    }

    IEnumerator IEPlaySound()
    {
        //AudioManager.Ins.MusicOff();
        yield return new WaitForSeconds(3f);
        AudioManager.Ins.GamePlayBGM();
    }

    void OnEnable()
    {
       StartCoroutine(IEPlaySound());

        PlayerController.Ins.ResetPlayer();

        if (createCount == 1)
        {
            GameObject layoutStart = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);
            layoutStart.transform.parent = generatorParent;

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            for (int i = 0; i < distanceToEnd; i++)
            {
                GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);
                newRoom.transform.parent = generatorParent;

                layoutRoomObjects.Add(newRoom);

                if (i + 1 == distanceToEnd)
                {
                    newRoom.GetComponent<SpriteRenderer>().color = endColor;
                    layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);

                    endRoom = newRoom;
                }

                selectedDirection = (Direction)Random.Range(0, 4);
                MoveGenerationPoint();

                while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
                {
                    MoveGenerationPoint();
                }
            }

            if (includeBoss)
            {
                int bossSelector = Random.Range(minDistanceToBoss, maxDistanceToBoss);
                bossRoom = layoutRoomObjects[bossSelector];
                layoutRoomObjects.RemoveAt(bossSelector);
                bossRoom.GetComponent<SpriteRenderer>().color = bossRoomColor;

            }

            //if (includeGunRoom)
            //{
            //    int grSelector = Random.Range(minDistanceToGunRoom, maxDistanceToGunRoom);
            //    gunRoom = layoutRoomObjects[grSelector];
            //    layoutRoomObjects.RemoveAt(grSelector);
            //    gunRoom.GetComponent<SpriteRenderer>().color = gunRoomColor;
            //}

            //create room outlines
            CreateRoomOutline(Vector3.zero);

            foreach (GameObject room in layoutRoomObjects)
            {
                CreateRoomOutline(room.transform.position);
            }

            CreateRoomOutline(endRoom.transform.position);

            if (includeBoss)
            {
                CreateRoomOutline(bossRoom.transform.position);
            }
            //if (includeGunRoom)
            //{
            //    CreateRoomOutline(gunRoom.transform.position);
            //}

            foreach (GameObject outline in generatedOutlines)
            {
                bool generateCenter = true;

                if (outline.transform.position == Vector3.zero)
                {
                    RoomCenter startCenter = Instantiate(centerStart, outline.transform.position, transform.rotation);
                    startCenter.theRoom = outline.GetComponent<Room>();
                    startCenter.transform.parent = generatorParent;
                    generateCenter = false;
                }

                if (outline.transform.position == endRoom.transform.position)
                {
                    RoomCenter endCenter = Instantiate(centerEnd, outline.transform.position, transform.rotation);
                    endCenter.theRoom = outline.GetComponent<Room>();
                    endCenter.transform.parent = generatorParent;
                    generateCenter = false;
                }

                //if (includeBoss)
                //{
                //    if (outline.transform.position == bossRoom.transform.position)
                //    {
                //        RoomCenter bossCenter = Instantiate(centerBoss, outline.transform.position, transform.rotation);
                //        bossCenter.theRoom = outline.GetComponent<Room>();
                //        bossCenter.transform.parent = generatorParent;
                //        generateCenter = false;
                //    }
                //}

                //if (includeGunRoom)
                //{
                //    if (outline.transform.position == gunRoom.transform.position)
                //    {
                //        RoomCenter gunCenter = Instantiate(centerGunRoom, outline.transform.position, transform.rotation);
                //        gunCenter.theRoom = outline.GetComponent<Room>();
                //        gunCenter.transform.parent = generatorParent;
                //        generateCenter = false;
                //    }
                //}

                if (generateCenter)
                {
                    int centerSelect = Random.Range(0, potentialCenters.Length);

                    RoomCenter centerPotential = Instantiate(potentialCenters[centerSelect], outline.transform.position, transform.rotation);
                    centerPotential.theRoom = outline.GetComponent<Room>();
                    centerPotential.transform.parent = generatorParent;
                }
            }
            createCount = 0;
        }
    }

    private void OnDisable()
    {
        generatorPoint.transform.position = Vector3.zero;
        layoutRoomObjects.Clear();
        generatedOutlines.Clear();
        createCount = 1;
        endRoom = null;
        bossRoom = null;
        //gunRoom = null;
        //Destroy(generatorParent.gameObject);
        ClearChildren();
    }

    public void ClearChildren()
    {
        //Debug.Log(generatorParent.childCount);
        int i = 0;

        //Array to hold all child obj
        GameObject[] allChildren = new GameObject[generatorParent.childCount];

        //Find all child obj and store to that array
        foreach (Transform child in generatorParent)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }

        //Now destroy them
        foreach (GameObject child in allChildren)
        {
            DestroyImmediate(child.gameObject);
        }

        //Debug.Log(generatorParent.childCount);
    }


    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

#endif
    }

    public void MoveGenerationPoint()
    {
        switch (selectedDirection)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;

            case Direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;

            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;

            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
    }

    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), .2f, whatIsRoom);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), .2f, whatIsRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), .2f, whatIsRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), .2f, whatIsRoom);

        int directionCount = 0;
        if (roomAbove)
        {
            directionCount++;
        }
        if (roomBelow)
        {
            directionCount++;
        }
        if (roomLeft)
        {
            directionCount++;
        }
        if (roomRight)
        {
            directionCount++;
        }

        switch (directionCount)
        {
            case 0:
                Debug.LogError("Found no room exists!!");
                break;

            case 1:

                if (roomAbove)
                {
                    GameObject upSingle = Instantiate(rooms.singleUp, roomPosition, transform.rotation);
                    upSingle.transform.parent = generatorParent;
                    generatedOutlines.Add(upSingle);
                }

                if (roomBelow)
                {
                    GameObject downSingle = Instantiate(rooms.singleDown, roomPosition, transform.rotation);
                    downSingle.transform.parent = generatorParent;
                    generatedOutlines.Add(downSingle);
                }

                if (roomLeft)
                {
                    GameObject leftSingle = Instantiate(rooms.singleLeft, roomPosition, transform.rotation);
                    leftSingle.transform.parent = generatorParent;
                    generatedOutlines.Add(leftSingle);
                }

                if (roomRight)
                {
                    GameObject rightSingle = Instantiate(rooms.singleRight, roomPosition, transform.rotation);
                    rightSingle.transform.parent = generatorParent;
                    generatedOutlines.Add(rightSingle);
                }

                break;

            case 2:

                if (roomAbove && roomBelow)
                {
                    GameObject upDownDouble = Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation);
                    upDownDouble.transform.parent = generatorParent;
                    generatedOutlines.Add(upDownDouble);
                }

                if (roomLeft && roomRight)
                {
                    GameObject leftRightDouble = Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation);
                    leftRightDouble.transform.parent = generatorParent;
                    generatedOutlines.Add(leftRightDouble);
                }

                if (roomAbove && roomRight)
                {
                    GameObject upRightDouble = Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation);
                    upRightDouble.transform.parent = generatorParent;
                    generatedOutlines.Add(upRightDouble);
                }

                if (roomRight && roomBelow)
                {
                    GameObject rightDownDouble = Instantiate(rooms.doubleRightDown, roomPosition, transform.rotation);
                    rightDownDouble.transform.parent = generatorParent;
                    generatedOutlines.Add(rightDownDouble);
                }

                if (roomBelow && roomLeft)
                {
                    GameObject downLeftDouble = Instantiate(rooms.doubleDownLeft, roomPosition, transform.rotation);
                    downLeftDouble.transform.parent = generatorParent;
                    generatedOutlines.Add(downLeftDouble);
                }

                if (roomLeft && roomAbove)
                {
                    GameObject leftUpDouble = Instantiate(rooms.doubleLeftUp, roomPosition, transform.rotation);
                    leftUpDouble.transform.parent = generatorParent;
                    generatedOutlines.Add(leftUpDouble);
                }

                break;

            case 3:

                if (roomAbove && roomRight && roomBelow)
                {
                    GameObject upRightDownTriple = Instantiate(rooms.tripleUpRightDown, roomPosition, transform.rotation);
                    upRightDownTriple.transform.parent = generatorParent;
                    generatedOutlines.Add(upRightDownTriple);
                }

                if (roomRight && roomBelow && roomLeft)
                {
                    GameObject rightDownLeftTriple = Instantiate(rooms.tripleRightDownLeft, roomPosition, transform.rotation);
                    rightDownLeftTriple.transform.parent = generatorParent;
                    generatedOutlines.Add(rightDownLeftTriple);
                }

                if (roomBelow && roomLeft && roomAbove)
                {
                    GameObject downLeftUpTriple = Instantiate(rooms.tripleDownLeftUp, roomPosition, transform.rotation);
                    downLeftUpTriple.transform.parent = generatorParent;
                    generatedOutlines.Add(downLeftUpTriple);
                }

                if (roomLeft && roomAbove && roomRight)
                {
                    GameObject leftUpRightTriple = Instantiate(rooms.tripleLeftUpRight, roomPosition, transform.rotation);
                    leftUpRightTriple.transform.parent = generatorParent;
                    generatedOutlines.Add(leftUpRightTriple);
                }

                break;

            case 4:

                if (roomBelow && roomLeft && roomAbove && roomRight)
                {
                    GameObject fourWay = Instantiate(rooms.fourway, roomPosition, transform.rotation);
                    fourWay.transform.parent = generatorParent;
                    generatedOutlines.Add(fourWay);
                }

                break;
        }
    }
}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleUp, singleDown, singleRight, singleLeft,
        doubleUpDown, doubleLeftRight, doubleUpRight, doubleRightDown, doubleDownLeft, doubleLeftUp,
        tripleUpRightDown, tripleRightDownLeft, tripleDownLeftUp, tripleLeftUpRight,
        fourway;
}
