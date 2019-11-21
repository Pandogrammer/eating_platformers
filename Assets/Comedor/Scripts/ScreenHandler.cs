using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenHandler : MonoBehaviour
{
    [SerializeField] private Button switchCamera;
    [SerializeField] private Button spawnerRatio;
    [SerializeField] private TextMeshProUGUI spawnerRatioText;
    [SerializeField] private TextMeshProUGUI agentScore;
    private int lastIndex;
    private Camera[] cameras;

    private FoodSpawner[] foodSpawners;
    private int spawnerCooldown = 1;

    private bool agentSelected;
    private PlatformerAgent agent;
    
    void Start()
    {
        Display.displays[0].SetRenderingResolution(480, 800);
        cameras = FindObjectsOfType<Camera>();
        foodSpawners = FindObjectsOfType<FoodSpawner>();
        ChangeDisplay();

        SetSpawnerText();
        switchCamera.onClick.AddListener(ChangeDisplay);
        spawnerRatio.onClick.AddListener(AdvanceSpawner);
    }

    public void Update()
    {
        if (agentSelected)
        {
            agentScore.text =  agent.totalFoodEaten + "/10";
        }
    }

    private void SetSpawnerText()
    {
        spawnerRatioText.text = spawnerCooldown.ToString();
    }

    private void AdvanceSpawner()
    {
        spawnerCooldown++;
        if (spawnerCooldown > 4) spawnerCooldown = 0;
        SetSpawnerCooldown();
        SetSpawnerText();
    }

    private void SetSpawnerCooldown()
    {
        for (var i = 0; i < foodSpawners.Length; i++)
        {
            foodSpawners[i].cooldown = spawnerCooldown == 0 ? 0 : 8.5f + i * 2f - spawnerCooldown * 2f;
        }
    }

    private void ChangeDisplay()
    {
        if (cameras == null) return;
        if (lastIndex >= cameras.Length) lastIndex = 0;
        SetDisplay(cameras[lastIndex]);
        lastIndex += 1;
    }


    private void SetDisplay(Camera camera)
    {
        foreach (var cam in cameras)
        {
            cam.gameObject.SetActive(false);
        }

        agentSelected = false;
        agentScore.gameObject.SetActive(false);
        camera.gameObject.SetActive(true);
        if (camera.gameObject.CompareTag("agent_camera"))
        {
            agent = camera.gameObject.transform.parent.parent.GetComponentInChildren<PlatformerAgent>();
            agentSelected = true;
            agentScore.gameObject.SetActive(true);
        }
    }
}