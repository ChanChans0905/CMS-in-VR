using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSoundManager : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    public AudioSource EngineSound;

    void FixedUpdate()
    {
        EngineSound.volume = DC.VelocityValue / 27.7f;
    }
}
