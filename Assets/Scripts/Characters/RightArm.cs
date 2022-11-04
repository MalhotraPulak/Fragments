using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightArm : Arm
{
    // [SerializeField] private Animator animator;

    // Singleton instantiation
    private static RightArm instance;
    public static RightArm Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<RightArm>();
            return instance;
        }
    }
}