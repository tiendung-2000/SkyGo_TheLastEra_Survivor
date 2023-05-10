using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace API.UI
{
    public class BaseUIMenu : BaseUIComp
    {
        public UILayer UILayer;
        [HideInInspector]
        public string UIID;
        /// <summary>
        /// Run when open
        /// </summary>
        /// <param name="initParams"> Extra Init Value </param>
        public virtual void Init(object[] initParams)
        {

        }
        /// <summary>
        /// Run when close
        /// </summary>
        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
        /// <summary>
        /// Run when press esc or back on android
        /// </summary>
        public virtual void OnBackClick()
        {

        }
    }

    public enum UILayer
    {
        Menu,
        Popup,
        Top
    }
}
