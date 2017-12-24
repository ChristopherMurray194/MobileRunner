using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Player player;
    Text text;

	void Start ()
    {
        text = GetComponent<Text>();
	}
	
	void Update ()
    {
        text.text = player.currentHealth.ToString();
	}
}
