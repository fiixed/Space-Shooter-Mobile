using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimpleTouchAreaButton : MonoBehaviour,IPointerDownHandler, IPointerUpHandler {

    private bool touched;
    private int pointerID;
    private bool canFire;

    private void Awake() {
        touched = false;
    }

    public void OnPointerDown(PointerEventData eventData) {
        // Set our start point
        if (!touched) {
            touched = true;
            pointerID = eventData.pointerId;
            canFire = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        // Reset everything
        if (eventData.pointerId == pointerID) {
            canFire = false;
            touched = false;
        }
    }

    public bool CanFire() {
        return canFire;
    }
}
