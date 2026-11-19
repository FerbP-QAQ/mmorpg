using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
{
	public GameObject nameBarPrefab;

	private Dictionary<Transform,GameObject> elements = new Dictionary<Transform, GameObject>();
	// Use this for initialization

	public void AddCharacterNameBar(Transform owner,Character character )
	{
		GameObject goNameBar = Instantiate(nameBarPrefab,transform);
		goNameBar.name = "NameBar"+character.entityId;
		goNameBar.GetComponent<UIWorldElement>().owner = owner;
		goNameBar.GetComponent<UINameBar>().character = character;
		goNameBar.SetActive(true);
		elements[owner] = goNameBar;
	}

	public void RemoveCharacterNameBar(Transform owner)
	{
		if (elements.ContainsKey(owner))
		{
			Destroy(elements[owner]);
			elements.Remove(owner);
		}
	}
}
