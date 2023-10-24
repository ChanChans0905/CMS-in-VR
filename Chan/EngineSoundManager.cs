using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSoundManager : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    public AudioSource EngineSound;

    void Update()
    {
        EngineSound.volume = DC.VelocityValue / 27.7f;
    }
}
