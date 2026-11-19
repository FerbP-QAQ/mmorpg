using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIShopItem : MonoBehaviour, ISelectHandler
{
    public Image icon;
    public Text title;
    public Text price;
    public Text limitClass;
    public Text count;

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
    public int ShopItemID {  get; set; }
    private UIShop shop;

    private ItemDefine item;
    private ShopItemDefine ShopItem {  get; set; }

    public void SetShopItem(int id,ShopItemDefine shopItem,UIShop owner)
    {
        this.shop = owner;
        ShopItemID = id;
        ShopItem = shopItem;
        item = DataManager.Instance.Items[ShopItem.ItemID];

        title.text = item.Name;
        count.text = "x" + ShopItem.Count.ToString();
        price.text = ShopItem.Price.ToString();
        limitClass.text = item.LimitClass.ToString();
        icon.overrideSprite = Resloader.Load<Sprite>(item.Icon);
    }


    public void OnSelect(BaseEventData eventData)
    {
        Selected = true;
        shop.SelectShopItem(this);
    }
}
    

