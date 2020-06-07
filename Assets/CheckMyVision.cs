using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMyVision : MonoBehaviour
{
    //How sensative we are about the vision/line of sight?
    public enum enmSensitivity 
    {
        HIGH,
        LOW
       
    }
    //variable to check sensitivity
    public enmSensitivity Sensitivity = enmSensitivity.HIGH;
    //Are we able to see the target?
    public bool targetInSight = false;
    
    //Field of vision
    public float fieldOfVision = 90f;
    
    // we need a reference to our target here as well

    private Transform target = null;

    // Reference to our eyes-yet to add
    public Transform myEyes = null;

    //My transform component?

    public Transform npcTransform = null;

    //My Sphere Collider

    private SphereCollider sphereCollider = null;

    //last known sighting if object
    public Vector3 lastKnownSighting = Vector3.zero;

    private void Awake()
    {

        npcTransform = GetComponent<Transform>();
        sphereCollider = GetComponent<SphereCollider>();
        lastKnownSighting = npcTransform.position;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); //We shall tag this later we did get component beacuse it was returning an array and we need a single object


    }
    bool InMyFieldOfVision()
    {
        Vector3 dirToTarget = target.position - myEyes.position;
        //Get Angle between forward and view direction
        float angle = Vector3.Angle(myEyes.forward, dirToTarget);
        //check if within the field of view
        if(angle <= fieldOfVision)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //we need a function to check field of sight
    bool ClearLineOfSight()
    {

        RaycastHit hit;
        if (Physics.Raycast(myEyes.position, (target.position - myEyes.position).normalized, out hit, sphereCollider.radius))
        {
            if(hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
     
    void UpdateSight()
    {
        switch(Sensitivity)
        {
            case enmSensitivity.HIGH:
                targetInSight = InMyFieldOfVision() && ClearLineOfSight();
                break;

            case enmSensitivity.LOW:
                targetInSight = InMyFieldOfVision() || ClearLineOfSight();
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        UpdateSight();
        if(targetInSight)
        {
            lastKnownSighting = target.position;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!(other.CompareTag("Player")))
            return;
        targetInSight = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
