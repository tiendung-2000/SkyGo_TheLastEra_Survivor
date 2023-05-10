using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCanBreak : MonoBehaviour
{
    [SerializeField] int sortingLayer;
    [SerializeField] int counter = 0;
    [SerializeField] Collider2D colTrigger;
    [SerializeField] Collider2D colBlock;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] List<Sprite> sprite;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            counter++;
            switch (counter)
            {
                case 0:
                    spriteRenderer.sprite = sprite[0];
                    break;
                case 1:
                    spriteRenderer.sprite = sprite[1];
                    break;
                case 2:
                    spriteRenderer.sprite = sprite[2];
                    colTrigger.enabled = false;
                    colBlock.enabled = false;
                    spriteRenderer.sortingOrder = sortingLayer;
                    break;
            }

            //for (int i = sprite.Count; i >= 0; i--)
            //{
            //    spriteRenderer.sprite = sprite[i];
            //    sprite.RemoveAt(i);
            //}


            //if (sprite.Count <= 0)
            //{
            //    colTrigger.enabled = false;
            //    colBlock.enabled = false;
            //}
        }
    }
}
