using Common.Data;
using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UIWindow
{
	public Text title;
	public Text money;

	public GameObject shopItem;
	ShopDefine shop;
	public Transform[] itemRoot;

	// Use this for initialization
	void Start () 
	{
	}

	IEnumerator InitItem()
	{
		int count = 0;
		int page = 0;
		ClearItems();
		foreach (var kv in DataManager.Instance.ShopItems[shop.ID])
		{
			if (kv.Value.Status>0)
			{
				GameObject go = Instantiate(shopItem, itemRoot[page]);
				UIShopItem uI = go.GetComponent<UIShopItem>();
				uI.SetShopItem(kv.Key, kv.Value,this);
				count++;
				if (count>=10)
				{
					count = 0;
					page++;
					itemRoot[page].gameObject.SetActive(true);
				}
			}
		}
		yield return null;
	}

	void ClearItems()
	{
		foreach (var root in itemRoot)
		{
			if (root == null) continue;
			for (int i = root.childCount - 1; i >= 0; i--)
			{
				Destroy(root.GetChild(i).gameObject);
			}
		}
		selectedItem = null;
	}

	public void SetShop(ShopDefine shop)
	{
		this.shop = shop;
		title.text = shop.Name;
		money.text = User.Instance.CurrentCharacter.Gold.ToString();
		StartCoroutine(InitItem());
	}

	private UIShopItem selectedItem;
	public void SelectShopItem (UIShopItem item)
	{
		if (selectedItem != null)
		{
			selectedItem.Selected = false;
		}
        selectedItem = item;
    }

    public void OnClickBuy()
	{
		if (selectedItem == null)
		{
			MessageBox.Show("请选择要购买的道具", "购买提示");
			return;
		}
		ShopManager.Instance.BuyItem(shop.ID, selectedItem.ShopItemID);
	}

}
