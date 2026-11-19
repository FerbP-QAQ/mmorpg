using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;

public class UICharterSelcet : MonoBehaviour {

	public GameObject PanelSelect;
	public GameObject PanelCreat;

	public GameObject ButtonCreatCancel;

	public InputField charName;
	CharacterClass charClass;

	public Transform uiCharList;
	public GameObject uiCharInfo;

	public List<GameObject> uiChar = new List<GameObject>();

	public Image[] titles;
	public Text descs;
	public Text[] names;

	private int selcetCharacterIdx = -1;

	public UICharacterView characterview;


	// Use this for initialization
	void Start () 
	{
		InitCharacterSelcet(true);
		DataManager.Instance.Load();
		UserService.Instance.OnCharacterCreat = OnCharacterCreat;
	}

	public void InitCharacterSelcet(bool init)
	{
		PanelSelect .SetActive(true);
		PanelCreat .SetActive(false);

		if (init )
		{
			foreach (var old in uiChar )
			{
				Destroy(old);
			}
			uiChar .Clear ();

			for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
			{
				GameObject go = Instantiate(uiCharInfo, this.uiCharList);
				UICharInfo chrInfo = go.GetComponent<UICharInfo>();
				chrInfo.info = User.Instance.Info.Player.Characters[i];

				Button button = go.GetComponent<Button>();
				int idx = i;
				button.onClick.AddListener(() =>
				{
					OnSelcetCharacter(idx);
				});

				uiChar.Add(go);
				go.SetActive(true);
			}
		}
	}

	public void InitChatacterCreat()
	{
		PanelCreat .SetActive (true);
		PanelSelect .SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickCreat()
	{
		if (string .IsNullOrEmpty (this.charName .text ))
		{
			MessageBox.Show("请输入角色名称");
			return;
		}
		UserService.Instance.SendCharacterCreat(this.charName.text  ,this .charClass );
	}

	public void OnSelcetClass(int charClass)
	{
		this.charClass = (CharacterClass)charClass;

		characterview.CurrectCharacter = charClass - 1;

        for (int i = 0; i < 3; i++)
        {
			titles[i].gameObject.SetActive(i == charClass - 1);
			names[i].text =DataManager .Instance .Characters [i+1].Name ;
			descs.text = DataManager.Instance.Characters[charClass].Description;
        }

    }
	void OnCharacterCreat(Result result ,string message)
	{
		if (result ==Result.Success)
		{
			InitCharacterSelcet(true);
		}
		else
		{
			MessageBox.Show(message, "错误", MessageBoxType.Error);
		}
	}

	public void OnSelcetCharacter(int idx)
	{
		this.selcetCharacterIdx = idx;
		var cha = User.Instance.Info.Player.Characters[idx];
		Debug .LogFormat ("Selcet Char：{0}{1}{2}",cha .Id , cha.Name,cha .Class );
        characterview.CurrectCharacter = ((int)cha.Class - 1);

		for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
		{
			UICharInfo ci = this.uiChar[i].GetComponent<UICharInfo>();
			ci.selceted = idx == i;
		}
	}

	public void OnClickPlay()
	{
		if (selcetCharacterIdx >=0)
		{
			UserService.Instance.SendGameEnter(selcetCharacterIdx);
		}
	}
}
