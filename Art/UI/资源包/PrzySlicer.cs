// PrzySlicer.cs —— 一键切分 przy.png 图集
// 用法：
//   1. 把 przy.png 拖进 Unity 工程的 Assets 下任意位置（例如 Assets/Art/UI/）
//   2. 把本脚本放到工程的 Assets/Editor/ 文件夹里（没有就新建 Editor 文件夹）
//   3. 菜单栏点 Tools → 切分 przy.png 图集
//   4. 完成后选中 przy.png，在 Project 窗口展开就能看到全部子图，直接拖到 UI 上用
// 坐标若有 1~2 像素偏差，选中图片打开 Sprite Editor 微调即可。
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class PrzySlicer
{
    // (名字, x, y, w, h, 边框L, 边框R, 边框T, 边框B)
    // x/y 是【左上角】像素坐标（和截图工具一致），脚本内部会自动换算成 Unity 的左下角坐标
    // 边框即九宫格 border，非 0 的元素拉伸时四个角不会变形
    private static readonly object[][] Defs =
    {
        new object[] { "btn_enter_game",   1,   2, 287,  82,  20, 20, 22, 22 }, // 进入游戏按钮
        new object[] { "input_bar",      288,   7, 189,  36,  18, 18, 14, 14 }, // 输入条(顶部)
        new object[] { "txt_labels_row",   0,  88, 418,  40,   0,  0,  0,  0 }, // 登陆邮箱/设置密码/确认密码 整条
        new object[] { "btn_register",     1, 126, 171,  82,  20, 20, 22, 22 }, // 注册按钮
        new object[] { "btn_login",      172, 126, 177,  81,  20, 20, 22, 22 }, // 登陆按钮
        new object[] { "txt_account",      0, 212,  68,  37,   0,  0,  0,  0 }, // "账号"
        new object[] { "txt_password",    69, 212,  69,  37,   0,  0,  0,  0 }, // "密码"
        new object[] { "checkbox",       139, 223,  20,  20,   0,  0,  0,  0 }, // 空勾选框
        new object[] { "checkmark",      164, 224,  25,  19,   0,  0,  0,  0 }, // 绿色对勾(选中态)
        new object[] { "input_bar_2",    194, 209, 237,  37,  18, 18, 14, 14 }, // 输入条(中部)
        new object[] { "circle_gold",    118, 244, 326, 262,   0,  0,  0,  0 }, // 金色大圆(底部与横幅重叠)
        new object[] { "grass_left",       0, 449, 116,  50,   0,  0,  0,  0 }, // 草地装饰(左)
        new object[] { "grass_right",    394, 449,  91,  50,   0,  0,  0,  0 }, // 草地装饰(右)
        new object[] { "banner",           0, 466, 485,  92, 120, 120, 30, 45 }, // 缎带横幅(九宫格)
        new object[] { "panel_beige",     37, 552, 409,  20,  15, 15, 15, 15 }, // 底部米色面板(图集底边被裁断)
        new object[] { "strip_right",    486,   1,  36, 431,  16, 16, 20, 30 }, // 右侧竖边条/滚动条
    };

    [MenuItem("Tools/切分 przy.png 图集")]
    public static void Slice()
    {
        string[] guids = AssetDatabase.FindAssets("przy t:Texture2D");
        if (guids == null || guids.Length == 0)
        {
            Debug.LogError("[PrzySlicer] 找不到名为 przy 的贴图，请先把 przy.png 拖进 Assets。");
            return;
        }
        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        var importer = (TextureImporter)AssetImporter.GetAtPath(path);
        if (importer == null) { Debug.LogError("[PrzySlicer] " + path + " 导入失败"); return; }

        int texH = 572; // przy.png 的高度，用于坐标换算

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.mipmapEnabled = false;
        importer.alphaIsTransparency = true;
        importer.SaveAndReimport();

#if UNITY_2021_2_OR_NEWER
        var factory = new SpriteDataProviderFactories();
        factory.Init();
        var dataProvider = factory.GetSpriteEditorDataProviderFromObject(importer);
        dataProvider.InitSpriteEditorDataProvider();

        var sprites = new List<SpriteRect>();
        foreach (var d in Defs)
        {
            string name = (string)d[0];
            int x = (int)d[1], y = (int)d[2], w = (int)d[3], h = (int)d[4];
            var sr = new SpriteRect
            {
                name = name,
                spriteID = new GUID(System.Guid.NewGuid().ToString("N")),
                rect = new Rect(x, texH - y - h, w, h), // 左上角原点 → 左下角原点
                border = new Vector4((int)d[5], (int)d[8], (int)d[6], (int)d[7]) // (L,B,R,T)
            };
            sprites.Add(sr);
        }
        dataProvider.SetSprites(sprites);
        dataProvider.Apply();
#else
        var sheet = new SpriteMetaData[Defs.Length];
        for (int i = 0; i < Defs.Length; i++)
        {
            var d = Defs[i];
            int x = (int)d[1], y = (int)d[2], w = (int)d[3], h = (int)d[4];
            sheet[i] = new SpriteMetaData
            {
                name = (string)d[0],
                rect = new Rect(x, texH - y - h, w, h),
                border = new Vector4((int)d[5], (int)d[8], (int)d[6], (int)d[7]),
                alignment = (int)SpriteAlignment.Center
            };
        }
        importer.spritesheet = sheet;
#endif
        importer.SaveAndReimport();
        Debug.Log("[PrzySlicer] przy.png 切分完成，共 " + Defs.Length + " 个子图 → " + path);
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
    }
}
