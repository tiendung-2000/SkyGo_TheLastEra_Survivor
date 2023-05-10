using API.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossShow : BaseUIMenu
{
    [SerializeField] AnimationClip animationClip;
    [SerializeField] Text bossName;
    [SerializeField] Image bossImage;
    [SerializeField] List<string> listBossName;
    [SerializeField] List<Sprite> listBossSprites;

    private void OnEnable()
    {
        switch (DynamicDataManager.Ins.CurLevel)
        {
            case 0:
                bossImage.sprite = listBossSprites[0];
                bossName.text = listBossName[0];
                break;
            case 1:
                bossImage.sprite = listBossSprites[1];
                bossName.text = listBossName[1];
                break;
            case 2:
                bossImage.sprite = listBossSprites[2];
                bossName.text = listBossName[2];
                break;
            case 3:
                bossImage.sprite = listBossSprites[3];
                bossName.text = listBossName[3];
                break;
            case 4:
                bossImage.sprite = listBossSprites[4];
                bossName.text = listBossName[4];
                break;
            case 5:
                bossImage.sprite = listBossSprites[5];
                bossName.text = listBossName[5];
                break;
            case 6:
                bossImage.sprite = listBossSprites[6];
                bossName.text = listBossName[6];
                break;
            case 7:
                bossImage.sprite = listBossSprites[7];
                bossName.text = listBossName[7];
                break;
            case 8:
                bossImage.sprite = listBossSprites[8];
                bossName.text = listBossName[8];
                break;
            case 9:
                bossImage.sprite = listBossSprites[9];
                bossName.text = listBossName[9];
                break;
        }
    }
}
