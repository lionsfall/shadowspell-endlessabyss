using Dogabeey;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class UIElement : MonoBehaviour
{
    [ValueDropdown("GetStructElementsAsStrings", AppendNextDrawer = true)]
    public abstract string FireEvent { get; }

    protected virtual void OnEnable()
    {
        EventManager.StartListening(FireEvent, OnEvent);
    }
    protected virtual void OnDisable()
    {
        EventManager.StopListening(FireEvent, OnEvent);
    }

    public void OnEvent(EventParam e)
    {
        DrawUI();
    }

    public abstract void DrawUI();

    public static List<string> GetStructElementsAsStrings<T>(T structInstance)
    {
        List<string> elements = new List<string>();
        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var field in fields)
        {
            object value = field.GetValue(structInstance);
            elements.Add(value?.ToString() ?? "null");
        }

        return elements;
    }
}
