using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArm : Arm
{
    // [SerializeField] private Animator animator;

    // Singleton instantiation
    private static LeftArm instance;
    public static LeftArm Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<LeftArm>();
            return instance;
        }
    }
}
