using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBag : UIWindow
{
	public Text money;

	public Transform[] pages;

	public GameObject bagItem;

	List<Image> slots;

	// Use this for initialization
	void Start () 
	{
		if (slots == null)
		{
			slots = new List<Image>();
            foreach (var page in pages)
            {
                if (!page.gameObject.activeInHierarchy) continue;   // 只收当前激活页
                foreach (Transform t in page.GetComponentsInChildren<Transform>(true))
                {
                    if (t.CompareTag("BagBox"))
                        slots.Add(t.GetComponent<Image>());
                }
            }
            // 排序保留：层级顺序 ≠ 视觉顺序，不加会把道具排错格
            slots.Sort((a, b) =>
            {
                int r = b.transform.localPosition.y.CompareTo(a.transform.localPosition.y);
                return r != 0 ? r : a.transform.localPosition.x.CompareTo(b.transform.localPosition.x);
            });
        }
		StartCoroutine(InitBags());
	}
	IEnumerator InitBags()
	{
		SetGold();
		for(int i = 0; i<BagManager.Instance.Items.Length && i < slots.Count; i++)
		{
			var item = BagManager.Instance.Items[i];
			if (item.ItemId>0)
			{
				GameObject go = Instantiate(bagItem, slots[i].transform);
				var ui = go.GetComponent<UIIconItem>();
				var def = ItemManager.Instance.Items[item.ItemId].Define;
				ui.SetMainIcon(def.Icon,item.Count.ToString());
			}
		}
		for (int i = BagManager.Instance.Items.Length;i<slots.Count;i++)
		{
			slots[i].color = Color.gray;
		}
		yield return null;
	}

	void Clear()
	{
		for(int i = 0; i < slots.Count; i++)
		{
			if (slots[i].transform.childCount > 0)
			{
				Destroy(slots[i].transform.GetChild(0).gameObject);
			}
		}
	}

	public void SetGold()
	{
		money.text = User.Instance.CurrentCharacter.Gold.ToString();
	}

	public void OnRest()
	{
		BagManager.Instance.Reset();
		Clear();
		StartCoroutine(InitBags());
	}
}
