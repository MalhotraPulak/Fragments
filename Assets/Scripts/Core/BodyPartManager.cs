using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BodyPartManager : MonoBehaviour
{
    // Start is called before the first frame update
    public enum BodyParts { LeftArm, RightArm, Legs, Core };
    public BodyParts activeBodyPart;
    public bool hasLeftArm = true;
    public bool hasRightArm = true;
    public bool hasLegs = true;
    public float ArmLaunchX;
    public float ArmLaunchY;
    public Vector3 legsOffset;
    public float attachDistance = 5f;
    public CinemachineVirtualCamera virtualCamera;
    public static BodyPartManager instance;

    public static BodyPartManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<BodyPartManager>();
            return instance;
        }
    }

    void Start()
    {
        LeftArm.Instance.graphic.SetActive(false);
        RightArm.Instance.graphic.SetActive(false);
        Legs.Instance.graphic.SetActive(false);
        activeBodyPart = BodyParts.Core;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeBodyPart = BodyParts.Core;
            virtualCamera.Follow = Floppy.Instance.transform;

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !hasLeftArm)
        {
            activeBodyPart = BodyParts.LeftArm;
            virtualCamera.Follow = LeftArm.Instance.transform;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !hasRightArm)
        {
            activeBodyPart = BodyParts.RightArm;
            virtualCamera.Follow = RightArm.Instance.transform;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && !hasLegs)
        {
            activeBodyPart = BodyParts.Legs;
            virtualCamera.Follow = Legs.Instance.transform;
        }
        Attach();
        if (activeBodyPart == BodyParts.Core)
            Detach();
    }

    void Detach()
    {
        // arm direction
        if (Input.GetKey(KeyCode.Z))
        {

            if (hasLeftArm)
                detachLeftArm();
            else if (hasRightArm)
                detachRightArm();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (hasLegs)
            {
                // Debug.Log("Legs have detached");
                detachLegs();
            }
        }

    }

    void Attach()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!hasLeftArm && Vector2.Distance(Floppy.Instance.graphic.transform.position, LeftArm.Instance.transform.position) < attachDistance)
            {
                attachLeftArm(); // todo add range condition
            }
            else if (!hasRightArm && Vector2.Distance(Floppy.Instance.graphic.transform.position, RightArm.Instance.transform.position) < attachDistance)
            {
                attachRightArm();
                virtualCamera.Follow = Floppy.Instance.transform;
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (!hasLegs && Vector2.Distance(Floppy.Instance.graphic.transform.position, Legs.Instance.transform.position) < attachDistance)
            {
                Debug.Log("Attach Legs called");
                attachLegs(); // todo add range condition
                virtualCamera.Follow = Floppy.Instance.transform;
            }
        }

    }



    private void attachLeftArm()
    {
        Floppy.Instance.ShowBodyPart("Arm-L");
        hasLeftArm = true;
        LeftArm.Instance.graphic.SetActive(false);
        activeBodyPart = BodyParts.Core;
        virtualCamera.Follow = Floppy.Instance.transform;
    }


    private void attachRightArm()
    {
        Floppy.Instance.ShowBodyPart("Arm-R");
        hasRightArm = true;
        RightArm.Instance.graphic.SetActive(false);
        activeBodyPart = BodyParts.Core;
        virtualCamera.Follow = Floppy.Instance.transform;
    }



    private void attachLegs()
    {
        Floppy.Instance.ShowBodyPart("Leg-L");
        Floppy.Instance.ShowBodyPart("Leg-R");
        hasLegs = true;
        Legs.Instance.graphic.SetActive(false);
        activeBodyPart = BodyParts.Core;
        virtualCamera.Follow = Floppy.Instance.transform;
    }

    private void detachLegs()
    {
        Floppy.Instance.HideBodyPart("Leg-L");
        Floppy.Instance.HideBodyPart("Leg-R");
        hasLegs = false;
        Legs.Instance.graphic.transform.position = Floppy.Instance.graphic.transform.position + legsOffset;
        Legs.Instance.graphic.SetActive(true);
        activeBodyPart = BodyParts.Legs;
        virtualCamera.Follow = Legs.Instance.transform;
    }

    private void detachLeftArm()
    {
        // animator.SetBool("hasRightArm", false);
        Floppy.Instance.HideBodyPart("Arm-L");
        hasLeftArm = false;
        activeBodyPart = BodyParts.LeftArm;
        detachArm(LeftArm.Instance);
    }

    private void detachRightArm()
    {
        // animator.SetBool("hasRightArm", false);
        Floppy.Instance.HideBodyPart("Arm-R");
        hasRightArm = false;
        activeBodyPart = BodyParts.RightArm;
        detachArm(RightArm.Instance);
    }

    private void detachArm(Arm arm)
    {
        float right = Input.GetAxis("Horizontal");
        float up = Input.GetAxis("Vertical");

        arm.graphic.transform.position = Floppy.Instance.graphic.transform.position;

        // float right = 1f;
        // float up = 1f;
        Vector2 dir = new Vector2(right, up);
        dir.Normalize();

        arm.velocity.y = ArmLaunchY * dir.y;
        arm.launch = ArmLaunchX * dir.x;

        virtualCamera.Follow = arm.transform;
        arm.graphic.SetActive(true);
    }
}
