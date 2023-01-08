using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatterCondenserController : MonoBehaviour
{
    [SerializeField] float orbitSpeed;
    [SerializeField] float orbitHeight;
    [SerializeField] List<Vector3> orbitList;
    [SerializeField] TMPro.TMP_Text EfficiencyLabel;

    private Vector3 orbitAxis;

    private Camera renderCam;
    public GameObject meshObject;
    public int harvesterCount = 0;

    private float currentEfficiency;
    [SerializeField] Gradient colorGradient;

    void Start()
    { 
        int orbitIndex = Random.Range(0, orbitList.Count);
        orbitAxis = orbitList[orbitIndex];
        transform.GetChild(0).transform.position = new Vector3(0f,0f,orbitHeight + orbitIndex);
        meshObject = transform.GetChild(0).gameObject;
        renderCam = Camera.main;
    }

    void Update()
    {
        transform.Rotate(orbitAxis, Time.deltaTime * orbitSpeed);
        EfficiencyLabel.transform.up = Vector3.up;
        EfficiencyLabel.transform.forward =  meshObject.transform.position - renderCam.transform.position;
    }

    public float Efficiency(float demand)
    {
        float x = (float)harvesterCount / demand;
        currentEfficiency = x * x;
        EfficiencyLabel.color = colorGradient.Evaluate(currentEfficiency);
        EfficiencyLabel.text = "Efficiency: " + (currentEfficiency * 100f).ToString("0.00") + " %";
        return currentEfficiency;
    }
}
