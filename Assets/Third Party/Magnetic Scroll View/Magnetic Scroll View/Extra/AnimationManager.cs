using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagneticScrollView
{
    [DisallowMultipleComponent]
    public class AnimationManager : MonoBehaviour
    {
        //public MenuUIController menuG;

        public string selectedAnimation;

        //[SerializeField] ParticleSystem FX;

        private Animator lastSelection;

        public bool isCharacterUI;
        public bool isLevelUI;
        [SerializeField] Sprite spriteSelected;
        [SerializeField] Sprite spriteUnSelected;

        public void TriggerAnimation(GameObject gameObject)
        {
            Animator objAnimator = null;
            if (gameObject != null)
            {
                objAnimator = gameObject.GetComponent<Animator>();
            }

            if (isCharacterUI == true)
            {
                gameObject.GetComponent<ImageHandler>().image.sprite = spriteSelected;
            }

            //if (isLevelUI == true)
            //{
            //    FX.Play();
            //}

            if (lastSelection != null && objAnimator != lastSelection)
            {
                    lastSelection.SetBool(selectedAnimation, false);
                if (isCharacterUI == true)
                {
                    lastSelection.gameObject.GetComponent<ImageHandler>().image.sprite = spriteUnSelected;
                }

                //if (isLevelUI == true)
                //{
                //    FX.Stop();
                //}
            }

            if (objAnimator != null)
            {
                objAnimator.SetBool(selectedAnimation, true);
            }

            lastSelection = objAnimator;
            //Debug.Log(lastSelection);
        }

        //public void TriggerAnimation (int index)
        //{

        //}

        public void ActionChoose(GameObject gameObject)
        {
            if (gameObject != null)
            {
                //menuG.panelLsLv.Pick(gameObject);
                Debug.Log(gameObject.name);
            }
        }
    }
}