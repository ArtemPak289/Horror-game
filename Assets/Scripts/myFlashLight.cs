using UnityEngine;
using UnityEngine.UI;

public class myFlashLight : MonoBehaviour
{
    [Header("Flashlight")]
    [SerializeField] private KeyCode toggleKey = KeyCode.F;
    [SerializeField] private float maxCharge = 100f;
    [SerializeField] private float lightDrain = 5f;

    [Header("UI")]
    [SerializeField] private Transform indicator;

    private Light flashLight;
    private float currentCharge;
    private bool isOn;

    private void Start()
    {
        flashLight = GetComponent<Light>();
        currentCharge = maxCharge;
        isOn = flashLight != null && flashLight.enabled;

        UpdateIndicator();
    }

    private void Update()
    {
        HandleToggle();
        DrainCharge();
        UpdateIndicator();
    }

    private void HandleToggle()
    {
        if (Input.GetKeyDown(toggleKey) && currentCharge > 0f)
        {
            isOn = !isOn;

            if (flashLight != null)
            {
                flashLight.enabled = isOn;
            }
        }
    }

    private void DrainCharge()
    {
        if (isOn)
        {
            currentCharge -= lightDrain * Time.deltaTime;

            if (currentCharge <= 0f)
            {
                currentCharge = 0f;
                isOn = false;

                if (flashLight != null)
                {
                    flashLight.enabled = false;
                }
            }
        }
    }

    private void UpdateIndicator()
    {
        float percent;

        percent = 0f;

        if (maxCharge > 0f)
        {
            percent = currentCharge / maxCharge;
        }

        if (indicator != null)
        {
            indicator.localScale = new Vector3(percent, 1f, 1f);
        }
    }

    public void AddCharge(float amount)
    {
        currentCharge += amount;

        if (currentCharge > maxCharge)
        {
            currentCharge = maxCharge;
        }

        if (currentCharge > 0f && flashLight != null && isOn)
        {
            flashLight.enabled = true;
        }

        UpdateIndicator();
    }
}
