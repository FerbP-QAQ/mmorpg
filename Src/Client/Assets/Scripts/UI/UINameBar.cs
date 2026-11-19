using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINameBar : MonoBehaviour 
{
	public Text avatarName;

	public Character character;
	// Use this for initialization
	void Start () {
		if (character!=null)
		{

		}
	}
	
	// Update is called once per frame
	void Update () {
		UpdateInfo();

		this.transform.forward=Camera.main.transform.forward;
    }

	void UpdateInfo()
	{
		if (character != null) 
		{
			string name = character.Name + "Lv." + character.Info.Level;
			if (avatarName != null) 
			{
				avatarName.text = name;
			}
		}
	}
}
