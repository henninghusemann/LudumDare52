using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarHarvesterController : MonoBehaviour
{
    public int harvesterIndex;
    public int condenserIndex;

    GameController gameController;

    [SerializeField] float orbitSpeed;
    [SerializeField] float orbitHeight;
    [SerializeField] List<Vector3> orbitList;

    [SerializeField] GameObject BeamObject;
    [SerializeField] GameObject DroneObject;
    
    [SerializeField] MatterCondenserController closestMatterCondenser;

    private bool transfer = false;
    private bool active = false;

    private Vector3 orbitAxis;

    void Start()
    {
        int orbitIndex = Random.Range(0, orbitList.Count);
        orbitAxis = Vector3.Normalize(orbitList[orbitIndex]);

        transform.GetChild(0).transform.localPosition = new Vector3(0f, 0f, orbitHeight + 0.2f*orbitIndex);
        gameController = GameController.instance;


        transform.up = orbitAxis;

        TryActivation();
    }

    private void TryActivation()
    {
        // determine corresponding condenser
        condenserIndex = (harvesterIndex-1) / gameController.HarvesterConvectionDemand;
        if (condenserIndex < gameController.AllMatterCondensers.Count)
        {
            closestMatterCondenser = gameController.AllMatterCondensers[condenserIndex];
            closestMatterCondenser.harvesterCount += 1;
            active = true;
        }
    }

    private void FixedUpdate()
    {
        if (!active)
        {
            TryActivation();
            return;
        }

        RaycastHit hit;
        Vector3 dir = Vector3.Normalize(closestMatterCondenser.meshObject.transform.position - DroneObject.transform.position);
        if(Physics.Raycast(DroneObject.transform.position,dir,out hit,1<<8))
        {
            transfer = false;
        }
        else
        {
            transfer = true;
        }

    }

    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * orbitSpeed, Space.Self);

        // beam rendering
        if (transfer)
        {
            Vector3 d = closestMatterCondenser.meshObject.transform.position - DroneObject.transform.position;
            BeamObject.transform.localScale = new Vector3(1f, 1f, d.magnitude);
            BeamObject.transform.forward = d;
        }
        else
        {
            // check if neighbhour drones are near
            int n = gameController.AllSolarHarvesters.Count;
            Vector3 p = gameController.AllSolarHarvesters[(harvesterIndex + 1)%n].DroneObject.transform.position;
            float distance = Vector3.Distance(p, DroneObject.transform.position);
            if (distance < 8f)
            {
                Vector3 d = p - DroneObject.transform.position;
                BeamObject.transform.localScale = new Vector3(1f, 1f, d.magnitude);
                BeamObject.transform.forward = d;
                return;
            }

            BeamObject.transform.localScale = Vector3.zero;
        }
    }
}

