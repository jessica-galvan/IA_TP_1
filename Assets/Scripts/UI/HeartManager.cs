using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class HeartManager : MonoBehaviour
{
    [Header("Heart Settings")]
    [SerializeField] private List<GameObject> hearts = new List<GameObject>();
    [SerializeField] private GameObject heart = null;
    private LifeController lifeController;
    private HorizontalLayoutGroup layout;

    void Start()
    {
        GameManager.instance.OnPlayerInit += Initialize;
        layout = GetComponent<HorizontalLayoutGroup>();
        layout.childAlignment = TextAnchor.UpperLeft;
        layout.childForceExpandHeight = true;
    }

    private void Initialize(ITarget player)
    {
        GameManager.instance.OnPlayerInit -= Initialize;
        lifeController = (player as IModel).LifeController;
        lifeController.UpdateLifeBar += UpdateLifeBar;
        //lifeController.OnDie += OnRespawn;
        lifeController.OnDie += DiePlayer;

        for (int i = 0; i < lifeController.MaxLife; i++)
        {
            GameObject newHeart = Instantiate(heart);
            newHeart.transform.parent = gameObject.transform;
            hearts.Add(newHeart);
        }
    }

    private void UpdateLifeBar(int currentLife, int maxLife)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentLife)
                hearts[i].SetActive(true);
            else
                hearts[i].SetActive(false);
        }
    }
    //private void OnRespawn()
    //{
    //    UpdateLifeBar(lifeController.MaxLife, lifeController.MaxLife);
    //}
    private void DiePlayer()
    {
        lifeController.UpdateLifeBar -= UpdateLifeBar;
        lifeController.OnDie -= DiePlayer;
    }
}
