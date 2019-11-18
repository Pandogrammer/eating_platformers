using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenHandler : MonoBehaviour
{
    [SerializeField] private Button switchCamera;
    [SerializeField] private Button spawnerRatio;
    [SerializeField] private TextMeshProUGUI spawnerRatioText;
    private int lastIndex;
    private Camera[] cameras;
    
    private FoodSpawner foodSpawner;
    private int spawnerCooldown = 1;

    void Start()
    {
        Display.displays[0].SetRenderingResolution(480,800); 
        cameras = FindObjectsOfType<Camera>();
        foodSpawner = FindObjectOfType<FoodSpawner>();
        ChangeDisplay();

        SetSpawnerText();
        switchCamera.onClick.AddListener(ChangeDisplay);
        spawnerRatio.onClick.AddListener(AdvanceSpawner);
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
        foodSpawner.cooldown = spawnerCooldown == 0 ? 0 : 8.5f - spawnerCooldown * 2f;
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

        camera.gameObject.SetActive(true);
    }
}