using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarController : MonoBehaviour
{
    [SerializeField] private GameObject bar;
    [SerializeField] private Image barImage;
    [SerializeField] private ParticleSystem particles;

    public bool IsVisible { get; private set; }

    public void UpdateLifeBar(int currentHealth, int maxHealth)
    {
        if (barImage != null)
            barImage.fillAmount = (float)currentHealth / maxHealth;
    }

    public void SetBarVisible(bool value)
    {
        bar.SetActive(value);
        IsVisible = value;
    }

    public void PlayParticles(bool value)
    {
        if (particles != null)
            particles.gameObject.SetActive(value);
    }
}
