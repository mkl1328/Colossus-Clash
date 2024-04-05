using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerController playerController;
    public Text cooldownText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController != null && cooldownText != null)
        {
            cooldownText.text = $"Dash: {Mathf.Max(playerController.DashCooldownTimer, 0):N1}s";
        }
    }
}
