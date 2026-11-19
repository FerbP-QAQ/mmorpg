using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
	//Class
	class UIElemet
	{
		public string Resources;
		public bool Cache;
		public GameObject Instance;
	}
	private Dictionary<Type, UIElemet> UIResources = new Dictionary<Type, UIElemet>();

	public UIManager()
	{ 
		UIResources.Add(typeof(UITest), new UIElemet() { Resources = "UI/UITest", Cache = true });
		UIResources.Add(typeof(UIBag),new UIElemet() {Resources = "UI/UIBag",Cache = false});
		UIResources.Add(typeof(UIShop), new UIElemet() { Resources = "UI/UIShop", Cache = false });
        UIResources.Add(typeof(UICharEquip), new UIElemet() { Resources = "UI/UICharEquip", Cache = false });

    }

    public T Show<T>()
	{
		//SoundManager.Instance.PlaySound("ui_open")
		Type type = typeof(T);
		if (UIResources.ContainsKey(type))
		{
			UIElemet info = UIResources[type];
			if (info.Instance != null)
			{
				info.Instance.SetActive(true);
			}
			else
			{
				UnityEngine.Object prefab = Resources.Load(info.Resources);
				if (prefab == null)
				{
					Debug.LogError("[UIManager] 找不到资源: " + info.Resources);
					return default(T);
				}
				info.Instance = (GameObject)GameObject.Instantiate(prefab);
			}
			return info.Instance.GetComponent<T>();
		}
		return default(T);
	}
	public void Close(Type type)
	{
		UIElemet info = UIResources[type];
		if(info.Cache)
		{
			info.Instance.SetActive(false);
		}
		else
		{
			GameObject.Destroy(info.Instance);
			info.Instance = null;
		}
	}
}
