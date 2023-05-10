using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace API.UI
{
    public class CanvasManager : MonoBehaviour
    {
        public static CanvasManager Ins;
        /// <summary>
        /// Set Canvas Manager To Don't Destroy On Load
        /// </summary>
        public bool dontDestroyOnLoad = false;
        /// <summary>
        /// UIs that was loaded
        /// </summary>
        private Dictionary<string, BaseUIMenu> LoadedUI = new Dictionary<string, BaseUIMenu>();
        /// <summary>
        /// UIs that currently opened
        /// </summary>
        private List<List<BaseUIMenu>> OpenedUI = new List<List<BaseUIMenu>>();

        private List<Transform> UILayerParents = new List<Transform>();

        private string UIPath = "UI/";

        private void Awake()
        {
            string[] layers = Enum.GetNames(typeof(UILayer));
            RectTransform uiRect = new GameObject("UI", typeof(RectTransform)).GetComponent<RectTransform>();
            uiRect.SetParent(transform);
            SetStretchAll(uiRect);
            for (int i = 0; i < layers.Length; i++)
            {
                RectTransform layer = new GameObject(layers[i], typeof(RectTransform)).GetComponent<RectTransform>();
                layer.SetParent(uiRect);
                SetStretchAll(layer);
                UILayerParents.Add(layer);
                OpenedUI.Add(new List<BaseUIMenu>());
            }
            if (dontDestroyOnLoad)
            {
                if(Ins == null)
                {
                    Ins = this;
                    DontDestroyOnLoad(transform.root.gameObject);
                }
                else
                {
                    if(Ins != this)
                    {
                        Destroy(gameObject);
                    }
                }
            }
            else
            {
                Ins = this;
            }
        }

        private void SetStretchAll(RectTransform rect)
        {
            rect.transform.localScale = Vector3.one;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
        /// <summary>
        /// Open UI Item
        /// </summary>
        /// <param name="name">UI prefab name in ui resources</param>
        /// <param name="initParams">Extra init value</param>
        /// <param name="overrideChildIndex">UI child index</param>
        public void OpenUI(string name, object[] initParams, int overrideChildIndex = 99)
        {
            BaseUIMenu ui;
            if (LoadedUI.ContainsKey(name))
            {
                ui = LoadedUI[name];
                ui.gameObject.SetActive(true);
                if (overrideChildIndex != 99)
                {
                    Debug.Log("OverrideIndex");
                    ui.transform.SetSiblingIndex(overrideChildIndex);
                }
                else
                {
                    ui.transform.SetAsLastSibling();
                }
                ui.Init(initParams);
            }
            else
            {
                BaseUIMenu UIprefab = Resources.Load<BaseUIMenu>(UIPath + name);
                ui = Instantiate(UIprefab, UILayerParents[(int)UIprefab.UILayer]);
                LoadedUI.Add(name, ui);
                if (overrideChildIndex != 99)
                {
                    Debug.Log("OverrideIndex " + overrideChildIndex);
                    ui.transform.SetSiblingIndex(overrideChildIndex);
                }
                else
                {
                    ui.transform.SetAsLastSibling();
                }
                ui.Init(initParams);
                ui.UIID = name;
            }
            OpenedUI[(int)ui.UILayer].Add(ui);
            //ui.transform.SetAsLastSibling();
        }
        /// <summary>
        /// Close UI Item
        /// </summary>
        /// <param name="ui">UI need to close</param>
        public void CloseUI(BaseUIMenu ui)
        {
            List<BaseUIMenu> curOpenUIs = OpenedUI[(int)ui.UILayer];
            int index = curOpenUIs.FindIndex(u => u == ui);
            if (index > -1)
            {
                ui.Close();
                curOpenUIs.RemoveAt(index);
                return;
            }
            Debug.LogError("No Opened UI Name " + ui.UIID);
        }
        /// <summary>
        /// Close UI Item
        /// </summary>
        /// <param name="name">UI prefab name in ui resources</param>
        public void CloseUI(string name)
        {
            BaseUIMenu ui = null;
            for (int i = 0; i < 3 && ui == null; i++)
            {
                ui = OpenedUI[i].Find(u => u.UIID == name);
            }
            if (ui != null)
            {
                CloseUI(ui);
            }
            else
            {
                Debug.LogError("No Opened UI Name " + name);
            }
        }
        /// <summary>
        /// Init Canvas Manager
        /// </summary>
        /// <param name="firstOpenUI">First open ui after init</param>
        /// <param name="initParams">Extra init value for first open ui</param>
        /// <param name="uiDataPath">Path to the ui folder</param>
        public void Init(string firstOpenUI, object[] initParams, string uiDataPath = "UI/")
        {
            UIPath = uiDataPath;
            OpenUI(firstOpenUI, initParams);
        }
    }
}