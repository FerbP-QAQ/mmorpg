using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEquipItem : MonoBehaviour ,IPointerClickHandler
{
	public Image icon;
	public Text title;
	public Text level;
	public Text limitClass;
	public Text limitCategory;

	public Image background;
	public Sprite normalBg;
	public Sprite selectedBg;

	private bool selected;
	public bool Selected
	{
		get {  return selected; }
		set
		{
			selected = value;
			background.overrideSprite = selected ? selectedBg : normalBg;
		}
	}
	public int index { get; set; }
	private UICharEquip owner;

	private Item item;

	// Use this for initialization
	void Start () {
		
	}

	bool isEquiped = false;

    public void SetEquipItem(int index, Item item, UICharEquip owner,bool equiped)
	{
		this.owner = owner;
		this.index = index;
		this.item = item;
		this.isEquiped = equiped;

		if (title != null) title.text = item.Define.Name;
		if (level != null) level.text = item.Define.Level.ToString();
		if (limitClass != null) limitClass.text = item.Define.LimitClass.ToString();
		if (limitCategory != null) limitCategory.text = item.Define.Category.ToString();
		if (icon != null) icon.overrideSprite = Resloader.Load<Sprite>(item.Define.Icon);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (isEquiped)
		{
			UnEquip();
		}
		else
		{
			if (selected)
			{
				DoEquip();
				selected = false;
			}
			else
			{
				selected = true;
			}
		}
	}

	void DoEquip()
	{
		var msg = MessageBox.Show(string.Format("要装备[{0}]吗？", item.Define.Name), "确认", MessageBoxType.Confirm);
		msg.OnYes = () =>
		{
			var oldEquip = EquipManager.Instance.GetEquip(item.EquipInfo.Slot);
			if (oldEquip != null)
			{
				var newmsg = MessageBox.Show(string.Format("要替换掉[{0}]吗？", item.Define.Name), "确认", MessageBoxType.Confirm);
				newmsg.OnYes = () =>
				{
					owner.DoEquip(item);
				};
			}
			else
			{
				owner.DoEquip(item);
			}
		};
	}

	void UnEquip()
	{
		var msg = MessageBox.Show(string.Format("要取下装备[{0}]吗？", item.Define.Name), "确认", MessageBoxType.Confirm);
		msg.OnYes = () =>
		{
			owner.UnEquip(item);
		};
	}
}
