//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AR_Manager : MonoBehaviour
//{
//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (ARbool)
//        {
//            //LC1position = Mathf.Abs(LC1.gameObject.transform.position.z);
//            //FC1Lposition = Mathf.Abs(FC1.carLeft.transform.position.z);
//            //FC1Rposition = Mathf.Abs(FC1.carRight.transform.position.z);    
//            //LC2position = Mathf.Abs(LC2.gameObject.transform.position.z);
//            //FC2Lposition = Mathf.Abs(FC2.carLeft.transform.position.z);
//            //FC2Rposition = Mathf.Abs(FC2.carRight.transform.position.z);
//            DCposition = Mathf.Abs(VolvoCar.transform.position.z);

//            if (MathF.Abs(DCposition - LC1position) <= ARSignalActivateDistance || MathF.Abs(DCposition - LC2position) <= ARSignalActivateDistance) { LCbool = true; } else { LCbool = false; }
//            if (MathF.Abs(DCposition - FC1Lposition) <= ARSignalActivateDistance || MathF.Abs(DCposition - FC2Lposition) <= ARSignalActivateDistance) { FCLbool = true; } else { FCLbool = false; }
//            if (MathF.Abs(DCposition - FC1Rposition) <= ARSignalActivateDistance || MathF.Abs(DCposition - FC2Rposition) <= ARSignalActivateDistance) { FCRbool = true; } else { FCRbool = false; }

//            if (TrialBool)
//            {
//                if (TC1.TC1bool || TC1.TC2bool || TC1.TC3bool) FCLbool = true; else FCLbool = false;
//                if (TC1.TC4bool || TC1.TC5bool || TC1.TC6bool) FCRbool = true; else FCRbool = false;
//            }
//        }
//    }
//}
