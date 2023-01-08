using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUIController : MonoBehaviour
{
    [SerializeField] AudioSource guiPlayer;
    [SerializeField] AudioClip errorSound;
    [SerializeField] AudioClip clickSound;

    [SerializeField] GameController gameController;
    [SerializeField] Texture2D harvesterImage;
    [SerializeField] Texture2D condenserImage;

    public Button MatterCondenserButton;
    public Button ResearchButton;
    public Button HarvesterButton;

    public Label SolarHarvesterCountLabel;
    public Label MatterCondenserCountLabel;
    public Label EnergyHarvestLabel;
    public Label ConversionRateLabel;
    public Label ProtoMatterUnitsLabel;

    public VisualElement InfoPanelElement;
    public VisualElement InfoPanelImage;
    public Label InfoPanelTitleText;
    public Label InfoPanelDescriptionText;
    public Label InfoPanelCostText;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // steup harvester button

        HarvesterButton = root.Q<Button>("harvester-button");
        HarvesterButton.clicked += HarvesterButtonClicked;

        HarvesterButton.RegisterCallback<MouseEnterEvent>((type) => { ShowInfoPanel(true, EntityType.Harvester); });
        HarvesterButton.RegisterCallback<MouseLeaveEvent>((type) => { ShowInfoPanel(false, EntityType.Harvester); });

        // setup condenser button 

        MatterCondenserButton = root.Q<Button>("matter-condenser-button");
        MatterCondenserButton.clicked += MatterCondenserButtonClicked;

        MatterCondenserButton.RegisterCallback<MouseEnterEvent>((type) => { ShowInfoPanel(true, EntityType.Condenser); });
        MatterCondenserButton.RegisterCallback<MouseLeaveEvent>((type) => { ShowInfoPanel(false, EntityType.Condenser); });


        SolarHarvesterCountLabel = root.Q<Label>("solar-harvester-count-label");
        MatterCondenserCountLabel = root.Q<Label>("matter-condenser-count-label");
        EnergyHarvestLabel = root.Q<Label>("energy-harvest-label");
        ConversionRateLabel = root.Q<Label>("energy-conversion-label");
        ProtoMatterUnitsLabel = root.Q<Label>("proto-matter-units-label");

        InfoPanelElement = root.Q<VisualElement>("info-panel");
        InfoPanelImage = root.Q<VisualElement>("info-panel-image");
        InfoPanelTitleText = root.Q<Label>("info-panel-title");
        InfoPanelDescriptionText = root.Q<Label>("info-panel-text");
        InfoPanelCostText = root.Q<Label>("info-panel-cost-text");

        Button ExitButton = root.Q<Button>("exit-button");
        ExitButton.clicked += ExitGame;

        InfoPanelElement.visible = false;
    }

    private void ExitGame()
    {
        Debug.Log("Quit Application");
        Application.Quit();
    }

    void HarvesterButtonClicked()
    {
        if (gameController.BuildSolarHarvester())
        {
            //success
            guiPlayer.Stop();
            guiPlayer.clip = clickSound;
            guiPlayer.Play();
        }
        else
        {
            guiPlayer.Stop();
            guiPlayer.clip = errorSound;
            guiPlayer.Play();
        }
        UpdateGUI();
    }

    void MatterCondenserButtonClicked()
    {
        if (gameController.BuildMatterCondenser())
        {
            //success
            guiPlayer.Stop();
            guiPlayer.clip = clickSound;
            guiPlayer.Play();
        }
        else
        {
            guiPlayer.Stop();
            guiPlayer.clip = errorSound;
            guiPlayer.Play();
        }
        UpdateGUI();
    }

    void ShowInfoPanel(bool show, EntityType entity)
    {
        if (!show)
        {
            InfoPanelElement.visible = false;
        }
        else
        {
            switch (entity)
            {
                case EntityType.Harvester:
                    InfoPanelImage.style.backgroundImage = harvesterImage;
                    InfoPanelTitleText.text = "Solar Harvester";
                    InfoPanelDescriptionText.text = "Converts solar radiation into electrical energy and transfers it to a Matter Condenser. \n Energy yield: " + gameController.HarvesterEnergyGain + " MW";
                    InfoPanelCostText.text = "Costs: " + gameController.SolarHarvesterCost.ToString() + " T Proto Matter";
                    break;
                case EntityType.Condenser:
                    InfoPanelImage.style.backgroundImage = condenserImage;
                    InfoPanelTitleText.text = "Matter Condenser";
                    InfoPanelDescriptionText.text = "Produces Proto Matter from electrical energy";
                    InfoPanelCostText.text = "Costs: " + gameController.MatterCondenserCost.ToString() + " T Proto Matter";
                    break;
                default:
                    break;
            }
            InfoPanelElement.visible = true;
        }
    }

    public void UpdateGUI()
    {
        EnergyHarvestLabel.text = gameController.currentEnergyGain.ToString() + " MW";
        ProtoMatterUnitsLabel.text = gameController.currentProtoMatterUnits.ToString("0.000") + " T";
        ConversionRateLabel.text = gameController.currentEnergyMatterConversionRate.ToString("0.00") + "T/s";
        MatterCondenserCountLabel.text = gameController.currentMatterCondenserCount.ToString();
        SolarHarvesterCountLabel.text = gameController.currentSolarHarvesterCount.ToString();
    }

}

public enum EntityType
{
    Harvester,
    Condenser
}