using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterStats))]
public class HealthUI : MonoBehaviour
{
    public GameObject uiPrefab;
    public Transform target;    // Position for UI to follow
    float visibleTime = 5;

    float lastMadeVisibleTime;
    Transform ui;               // Position for UI to spawn in
    Image healthSlider;         // Green bar that changes based on fill amount
    Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;

        foreach(Canvas c in FindObjectsOfType<Canvas>())
        {
            if(c.renderMode == RenderMode.WorldSpace)
            {
                ui = Instantiate(uiPrefab, c.transform).transform;
                healthSlider = ui.GetChild(0).GetComponent<Image>();
                ui.gameObject.SetActive(false);
                break;
            }
        }

        GetComponent<CharacterStats>().OnHealthChanged += OnHealthChanged;
    }

    void OnHealthChanged(int maxHealth, int currentHealth)
    {
        if (ui != null)
        {
            ui.gameObject.SetActive(true);
            lastMadeVisibleTime = Time.time;

            float healthPercent = (float)currentHealth / maxHealth;
            healthSlider.fillAmount = healthPercent;
            if (currentHealth <= 0)
            {
                Destroy(ui.gameObject);
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (ui != null)
        {
            ui.position = target.position;
            // Allign UI with the camera
            ui.forward = -cam.forward;

            if(Time.time - lastMadeVisibleTime > visibleTime)
            {
                ui.gameObject.SetActive(false);
            }
        }
    }
}
