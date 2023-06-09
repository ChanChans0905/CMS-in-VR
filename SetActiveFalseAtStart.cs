using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveFalseAtStart : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
}

