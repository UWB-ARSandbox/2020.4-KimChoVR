using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Physics;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class AttachToHandScript : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityFocusChangedHandler
{
    public SimpleDemos.SendFloatArray_Example floatScript;

    public void OnBeforeFocusChange(FocusEventData eventData)
    {

    }

    public void OnFocusChanged(FocusEventData eventData)
    {

    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {

    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        if (eventData != null)
        {
            floatScript.m_MyFloats[0] = 1;
            this.transform.position = eventData.Pointer.Position;
        }
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        if (floatScript.m_MyFloats[0] == 1)
        {
            this.transform.position = eventData.Pointer.Position;
        }
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        floatScript.m_MyFloats[0] = 0;
    }
}
