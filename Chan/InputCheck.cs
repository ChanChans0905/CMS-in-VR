using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(354);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) Debug.Log(123);
    }
}
