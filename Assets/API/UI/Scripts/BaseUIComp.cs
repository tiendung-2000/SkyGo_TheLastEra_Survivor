using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace API.UI
{
    public class BaseUIComp : MonoBehaviour
    {
#if UNITY_EDITOR
        public void AutoReference()
        {
            bool hasChange = false;
            foreach (var field in this.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)/*.Where(field => field.GetValue(this) == null)*/)
            {
                if (field.IsStatic || field.IsNotSerialized) continue;

                if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    try
                    {
                        Type genericListType = typeof(List<>).MakeGenericType(field.FieldType.GetGenericArguments().Single());
                        var tem = (System.Collections.IList)Activator.CreateInstance(genericListType);
                        if (field.FieldType.GetGenericArguments().Single() == typeof(GameObject))
                        {
                            GameObject[] holder = transform.FindDeepChildsWithStartName(field.Name);
                            for (int i = 0; i < holder.Length; i++)
                            {
                                tem.Add(holder[i]);
                            }
                        }
                        else
                        {
                            var data = transform.GetComponentsInChildren(field.FieldType.GetGenericArguments().Single(), true).ToList();
                            for (int i = 0; i < data.Count; i++)
                            {
                                if (!data[i].name.StartsWith(field.Name))
                                {
                                    data.RemoveAt(i);
                                    --i;
                                }
                            }
                            for (int i = 0; i < data.Count; i++)
                            {
                                tem.Add(data[i]);
                            }
                        }
                        field.SetValue(this, tem);
                        hasChange = true;
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("Error " + e.Message);
                    }
                    continue;
                }

                // Now we are looking for object (self or child) that have same name as a field
                Transform obj;
                /*if (transform.name == field.Name)
                {
                    obj = transform;
                }
                else*/
                {
                    obj = transform.FindDeepChildLower(field.Name);// Or you need to implement recursion to looking into deeper childs
                }

                // If we find object that have same name as field we are trying to get component that will be in type of a field and assign it
                if (obj != null)
                {
                    if (field.FieldType == typeof(GameObject))
                    {
                        field.SetValue(this, obj.gameObject);
                    }
                    else
                    {
                        field.SetValue(this, obj.GetComponent(field.FieldType));
                    }
                    hasChange = true;
                }
            }
            if (hasChange)
            {
                EditorUtility.SetDirty(this);
            }
        }

#endif
    }

    public static class TransformFindExtensions
    {
        public static Transform FindDeepChildLower(this Transform aParent, string aName)
        {
            aName = aName.ToLower();
            foreach (Transform child in aParent)
            {
                if (child.name.ToLower() == aName)
                    return child;
                var result = child.FindDeepChild(aName);
                if (result != null)
                    return result;
            }
            return null;
        }

        public static Transform FindDeepChild(this Transform aParent, string aName)
        {
            aName = aName.ToLower();
            foreach (Transform child in aParent)
            {
                if (child.name.ToLower() == aName)
                    return child;
                var result = child.FindDeepChild(aName);
                if (result != null)
                    return result;
            }
            return null;
        }

        public static GameObject[] FindDeepChildsWithStartName(this Transform aParent, string startName)
        {
            startName = startName.ToLower();
            List<GameObject> result = new List<GameObject>();
            foreach (Transform child in aParent)
            {
                if (child.name.ToLower().StartsWith(startName))
                {
                    result.Add(child.gameObject);
                }
                else
                {
                    var childResult = child.FindDeepChildsWithStartName(startName);
                    result.AddRange(childResult);
                }
            }
            return result.ToArray(); ;
        }
    }

#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(BaseUIComp), true)]
    public class BaseUIComponentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();

            OnCustomInspectorGUI();
        }

        protected virtual void OnCustomInspectorGUI()
        {
            if (GUILayout.Button("Auto Reference"))
            {
                foreach (BaseUIComp gameObject in targets)
                    gameObject.AutoReference();
            }
        }
    }
#endif
}