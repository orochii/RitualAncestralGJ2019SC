using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCheck : MonoBehaviour
{
    [SerializeField] UnityEngine.Events.UnityEvent onCheck;

    public void CheckFlag(int v) {
        if (GameManager.GetFlag(v) && onCheck != null) onCheck.Invoke();
    }
}
