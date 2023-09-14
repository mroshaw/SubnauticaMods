using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

// Token: 0x02000602 RID: 1538
public class Atlas : ScriptableObject
{
    // Token: 0x1700025A RID: 602
    // (get) Token: 0x06003620 RID: 13856 RVA: 0x001287F7 File Offset: 0x001269F7
    private Dictionary<string, Atlas.Sprite> nameToSprite
    {
        get
        {
            this.PreWarm(false);
            return this._nameToSprite;
        }
    }

    // Token: 0x06003621 RID: 13857 RVA: 0x00128808 File Offset: 0x00126A08
    public void PreWarm(bool forced)
    {
        if (!forced && this._nameToSprite != null)
        {
            return;
        }
        int num = (this.serialData != null) ? this.serialData.Count : 0;
        this._nameToSprite = new Dictionary<string, Atlas.Sprite>(num, StringComparer.InvariantCultureIgnoreCase);
        for (int i = 0; i < num; i++)
        {
            Atlas.SerialData serialData = this.serialData[i];
            string name = serialData.name;
            UnityEngine.Sprite sprite = serialData.sprite;
            if (sprite == null)
            {
                Debug.LogErrorFormat("Sprite with name '{0}' has no UnitySprite", new object[]
                {
                    name
                });
            }
            else
            {
                Atlas.Sprite value = new Atlas.Sprite(sprite, false);
                if (!this._nameToSprite.ContainsKey(name))
                {
                    this._nameToSprite.Add(name, value);
                }
            }
        }
    }

    // Token: 0x06003622 RID: 13858 RVA: 0x001288B4 File Offset: 0x00126AB4
    public Atlas.Sprite GetSprite(string name)
    {
        Atlas.Sprite result;
        if (this.nameToSprite.TryGetValue(name, out result))
        {
            return result;
        }
        return null;
    }

    // Token: 0x06003623 RID: 13859 RVA: 0x001288D4 File Offset: 0x00126AD4
    public void GetNames(ref List<string> names)
    {
        int count = this.nameToSprite.Count;
        if (names == null)
        {
            names = new List<string>(count);
        }
        else if (names.Capacity < count)
        {
            names.Capacity = count;
        }
        foreach (KeyValuePair<string, Atlas.Sprite> keyValuePair in this.nameToSprite)
        {
            names.Add(keyValuePair.Key);
        }
    }

    // Token: 0x06003624 RID: 13860 RVA: 0x0012893C File Offset: 0x00126B3C
    public static void InitAtlases()
    {
        Atlas.nameToAtlas = new Dictionary<string, Atlas>();
        Atlas[] array = Resources.LoadAll<Atlas>("Atlases");
        int i = 0;
        int num = array.Length;
        while (i < num)
        {
            Atlas atlas = array[i];
            Atlas.nameToAtlas.Add(atlas.atlasName, atlas);
            i++;
        }
    }

    // Token: 0x06003625 RID: 13861 RVA: 0x00128984 File Offset: 0x00126B84
    public static Atlas GetAtlas(string atlasName)
    {
        if (Atlas.nameToAtlas == null)
        {
            Atlas.InitAtlases();
        }
        Atlas result;
        Atlas.nameToAtlas.TryGetValue(atlasName, out result);
        return result;
    }

    // Token: 0x06003626 RID: 13862 RVA: 0x001289AC File Offset: 0x00126BAC
    public static Atlas.Sprite GetSprite(string atlasName, string name)
    {
        Atlas atlas = Atlas.GetAtlas(atlasName);
        if (atlas == null)
        {
            return null;
        }
        return atlas.GetSprite(name);
    }

    // Token: 0x06003627 RID: 13863 RVA: 0x001289D4 File Offset: 0x00126BD4
    public static bool GetNames(string atlasName, ref List<string> names)
    {
        Atlas atlas = Atlas.GetAtlas(atlasName);
        if (atlas == null)
        {
            return false;
        }
        atlas.GetNames(ref names);
        return true;
    }

    // Token: 0x04003228 RID: 12840
    public const string sAtlasesFullPath = "Assets/uGUI/Resources/Atlases/";

    // Token: 0x04003229 RID: 12841
    public const string sAtlasesPath = "Atlases";

    // Token: 0x0400322A RID: 12842
    private static Dictionary<string, Atlas> nameToAtlas;

    // Token: 0x0400322B RID: 12843
    [SerializeField]
    private string atlasName;

    // Token: 0x0400322C RID: 12844
    [SerializeField]
    private List<Atlas.SerialData> serialData;

    // Token: 0x0400322D RID: 12845
    private Dictionary<string, Atlas.Sprite> _nameToSprite;

    // Token: 0x02000AB6 RID: 2742
    [Serializable]
    public class SerialData
    {
        // Token: 0x04004BEA RID: 19434
        public string name;

        // Token: 0x04004BEB RID: 19435
        public UnityEngine.Sprite sprite;
    }

    // Token: 0x02000AB7 RID: 2743
    public class Sprite
    {
        // Token: 0x060052A3 RID: 21155 RVA: 0x00195FAC File Offset: 0x001941AC
        public Sprite(UnityEngine.Sprite unitySprite, bool slice9Grid = false)
        {
            this.size = unitySprite.rect.size;
            this.texture = unitySprite.texture;
            this.pixelsPerUnit = unitySprite.pixelsPerUnit;
            this.border = unitySprite.border;
            if (this.border.sqrMagnitude > 0f)
            {
                this.padding = DataUtility.GetPadding(unitySprite);
                this.inner = DataUtility.GetInnerUV(unitySprite);
                this.outer = DataUtility.GetOuterUV(unitySprite);
                this.vertices = null;
                this.uv0 = null;
                this.triangles = null;
            }
            else
            {
                this.padding = (this.inner = (this.outer = Vector4.zero));
                this.vertices = unitySprite.vertices;
                this.uv0 = unitySprite.uv;
                this.triangles = unitySprite.triangles;
            }
            this.slice9Grid = slice9Grid;
        }

        // Token: 0x060052A4 RID: 21156 RVA: 0x00196090 File Offset: 0x00194290
        public Sprite(Texture2D texture)
        {
            if (texture == null)
            {
                texture = Texture2D.whiteTexture;
            }
            this.size = new Vector2((float)texture.width, (float)texture.height);
            this.texture = texture;
            this.pixelsPerUnit = 100f;
            Vector2 vector = new Vector2(0.5f, 0.5f);
            Vector2 vector2 = new Vector2((0f - vector.x) * this.size.x, (0f - vector.y) * this.size.y) / this.pixelsPerUnit;
            Vector2 vector3 = new Vector2((1f - vector.x) * this.size.x, (1f - vector.y) * this.size.y) / this.pixelsPerUnit;
            this.vertices = new Vector2[]
            {
                new Vector2(vector2.x, vector2.y),
                new Vector2(vector2.x, vector3.y),
                new Vector2(vector3.x, vector3.y),
                new Vector2(vector3.x, vector2.y)
            };
            this.uv0 = new Vector2[]
            {
                new Vector2(0f, 0f),
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(1f, 0f)
            };
            this.triangles = new ushort[]
            {
                0,
                1,
                2,
                2,
                3,
                0
            };
            this.slice9Grid = false;
        }

        // Token: 0x04004BEC RID: 19436
        public Vector2 size;

        // Token: 0x04004BED RID: 19437
        public Texture2D texture;

        // Token: 0x04004BEE RID: 19438
        public float pixelsPerUnit;

        // Token: 0x04004BEF RID: 19439
        public Vector2[] vertices;

        // Token: 0x04004BF0 RID: 19440
        public Vector2[] uv0;

        // Token: 0x04004BF1 RID: 19441
        public ushort[] triangles;

        // Token: 0x04004BF2 RID: 19442
        public bool slice9Grid;

        // Token: 0x04004BF3 RID: 19443
        public Vector4 padding;

        // Token: 0x04004BF4 RID: 19444
        public Vector4 border;

        // Token: 0x04004BF5 RID: 19445
        public Vector4 inner;

        // Token: 0x04004BF6 RID: 19446
        public Vector4 outer;
    }
}
