using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [SerializeField] GameUIController guiController;
    [SerializeField] AudioSource voicePlayer;
    [SerializeField] AudioSource fxPlayer;
    [SerializeField] AudioClip releaseSound;
    [SerializeField] AudioClip requestSoundHarvester;
    [SerializeField] AudioClip requestSoundCondenser;

    [SerializeField] public int SolarHarvesterCost;
    [SerializeField] public int MatterCondenserCost;

    [SerializeField] public int HarvesterEnergyGain; // how much energy does one harvester produce
    [SerializeField] public int EnergyMatterConversionRate; // how much matter do the condensers produce
    [SerializeField] public int HarvesterConvectionDemand; // how many harvester are needed for one condenser to be optimally efficient

    [SerializeField] GameObject MatterCondenserPrefab;
    [SerializeField] GameObject SolarHarvesterPrefab;

    public List<SolarHarvesterController> AllSolarHarvesters; 
    public List<MatterCondenserController> AllMatterCondensers;

    public int currentEnergyGain;
    public float currentEnergyMatterConversionRate;
    public float currentProtoMatterUnits;

    public int currentSolarHarvesterCount { get { return AllSolarHarvesters.Count; } }
    public int currentMatterCondenserCount { get { return AllMatterCondensers.Count; } }


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        AllSolarHarvesters = new List<SolarHarvesterController>();
        AllMatterCondensers = new List<MatterCondenserController>();
        StartCoroutine(EntityUpdater());
        //StartCoroutine(ProbeSpawner());

        BuildMatterCondenser(true);
    }

    public bool BuildSolarHarvester()
    {
        if(SolarHarvesterCost > currentProtoMatterUnits)
        {
            return false;
        }
        else
        {
            voicePlayer.Stop();
            fxPlayer.Stop();
            voicePlayer.PlayOneShot(requestSoundHarvester);
            fxPlayer.clip = releaseSound;
            fxPlayer.PlayDelayed(6f);

            currentProtoMatterUnits -= SolarHarvesterCost;
            
            SolarHarvesterController probe = Instantiate(SolarHarvesterPrefab).GetComponent<SolarHarvesterController>();
            
            AllSolarHarvesters.Add(probe);
            probe.harvesterIndex = AllSolarHarvesters.Count;

            return true;
        }
    }

    public bool BuildMatterCondenser(bool free = false)
    {
        if (free)
        {
            voicePlayer.Stop();
            fxPlayer.Stop();
            AllMatterCondensers.Add(Instantiate(MatterCondenserPrefab).GetComponent<MatterCondenserController>());
            fxPlayer.PlayOneShot(releaseSound);
            return true;
        }

        if(MatterCondenserCost > currentProtoMatterUnits)
        {
            return false;
        }
        else
        {
            voicePlayer.Stop();
            fxPlayer.Stop();
            voicePlayer.PlayOneShot(requestSoundCondenser);
            fxPlayer.clip = releaseSound;
            fxPlayer.PlayDelayed(15f);
            currentProtoMatterUnits -= MatterCondenserCost;
            AllMatterCondensers.Add(Instantiate(MatterCondenserPrefab).GetComponent<MatterCondenserController>());
            return true;
        }
    }

    private IEnumerator ProbeSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            BuildSolarHarvester();
        }
    }


    private IEnumerator EntityUpdater()
    {
        float updateRate = 5f;
        float waitTime = 1f / updateRate;
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            // every harvester produces the same amount of energy
            currentEnergyGain = AllSolarHarvesters.Count * HarvesterEnergyGain;

            // calculate the yield for each condenser
            float accumulatedEfficiency = 0f;
            foreach(MatterCondenserController mcc in AllMatterCondensers)
            {
                accumulatedEfficiency += mcc.Efficiency(HarvesterConvectionDemand);
            }

            currentEnergyMatterConversionRate = accumulatedEfficiency * EnergyMatterConversionRate;

            currentProtoMatterUnits += currentEnergyMatterConversionRate;

            guiController.UpdateGUI();
        }
    }

}
