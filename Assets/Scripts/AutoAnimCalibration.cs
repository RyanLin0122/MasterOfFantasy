using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
#if UNITY_EDITOR
public class AutoAnimCalibration : MonoBehaviour
{
    [Header("移動單位")]
    public float StepRate = 0.01f;
    public Texture2D Body;
    public Texture2D Target;
    public Sprite[] _sprites;
    public SpriteRenderer BodyRenderer;
    public SpriteRenderer TargetRenderer;
    public Text NumText1;
    public Text NumText2;
    [Header("目前Body圖片")]
    public int Num1;
    [Header("目前Target圖片")]
    public int Num2;
    [Header("輸入數字1")]
    public int TempNum1;
    [Header("輸入數字2")]
    public int TempNum2;
    public string _path_ = "";
    public Mode mode = Mode.Suit;
    public enum Mode
    {
        Suit,
        OtherSprite,
    }
    private void Start()
    {
        _path_ = AssetDatabase.GetAssetPath(Body);
        Num1 = 0;
        TempNum1 = 0;
        Num2 = 0;
        TempNum2 = 0;
        _sprites = Resources.LoadAll<Sprite>(ToResourcePath(_path_));
    }
    public string ToResourcePath(string path)
    {
        path = path.Replace("Assets/Resources/", "");
        path = path.Replace(".png", "");
        return path;
    }
    public void Resize_Mitchall()
    {
        TextureImporter _importer_ = AssetImporter.GetAtPath(_path_) as TextureImporter;
        TextureImporterPlatformSettings set = _importer_.GetDefaultPlatformTextureSettings();
        set.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
        _importer_.SetPlatformTextureSettings(set);
        AssetDatabase.ImportAsset(_path_, ImportAssetOptions.ForceUpdate);
        _importer_.SaveAndReimport();
        //Rebuild asset
        //AssetDatabase.ImportAsset(_path_, ImportAssetOptions.ForceUpdate);
    }
    public void Resize_Bilinear()
    {
        TextureImporter _importer_ = AssetImporter.GetAtPath(_path_) as TextureImporter;
        TextureImporterPlatformSettings set = _importer_.GetDefaultPlatformTextureSettings();
        set.resizeAlgorithm = TextureResizeAlgorithm.Bilinear;
        _importer_.SetPlatformTextureSettings(set);
        AssetDatabase.ImportAsset(_path_, ImportAssetOptions.ForceUpdate);
        _importer_.SaveAndReimport();
        //Rebuild asset
        //AssetDatabase.ImportAsset(_path_, ImportAssetOptions.ForceUpdate);
    }
    public void ChangeTargetSprite()
    {
        if (mode.Equals(Mode.Suit))
        {
            BodyRenderer.sprite = Resources.LoadAll<Sprite>(ToResourcePath(_path_))[Num1];
            Debug.Log(BodyRenderer.sprite.pivot.y / BodyRenderer.sprite.rect.height);
        }
        else
        {
            TargetRenderer.sprite = Resources.LoadAll<Sprite>(ToResourcePath(_path_))[Num2];
            Debug.Log(TargetRenderer.sprite.pivot.y / TargetRenderer.sprite.rect.height);
        }
        //Rebuild asset
        AssetDatabase.ImportAsset(_path_, ImportAssetOptions.ForceUpdate);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            mode = Mode.Suit;
            _path_ = AssetDatabase.GetAssetPath(Body);
            _sprites = Resources.LoadAll<Sprite>(ToResourcePath(_path_));
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            mode = Mode.OtherSprite;
            _path_ = AssetDatabase.GetAssetPath(Target);
            _sprites = Resources.LoadAll<Sprite>(ToResourcePath(_path_));
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            UnityEngine.Object[] _objects = AssetDatabase.LoadAllAssetRepresentationsAtPath(_path_);
            List<SpriteMetaData> _data = new List<SpriteMetaData>();
            if (mode.Equals(Mode.Suit))
            {
                for (int i = 0; i < _objects.Length; i++)
                {
                    SpriteMetaData _meta = new SpriteMetaData();
                    _meta.alignment = 9;
                    if (i == Num1)
                    {
                        _meta.pivot = new Vector2(_sprites[i].pivot.x / _sprites[i].rect.width,
                        (_sprites[i].pivot.y / _sprites[i].rect.height) - StepRate);
                    }
                    else
                    {
                        _meta.pivot = new Vector2(_sprites[i].pivot.x / _sprites[i].rect.width, (_sprites[i].pivot.y / _sprites[i].rect.height));
                    }
                    _meta.name = _sprites[i].name;
                    _meta.rect = _sprites[i].rect;
                    _meta.rect.position = _meta.rect.position;
                    _meta.border = _sprites[i].border;

                    _data.Add(_meta);
                }
            }
            else if (mode.Equals(Mode.OtherSprite))
            {
                for (int i = 0; i < _objects.Length; i++)
                {
                    SpriteMetaData _meta = new SpriteMetaData();
                    _meta.alignment = 9;
                    if (i == Num2)
                    {
                        _meta.pivot = new Vector2(_sprites[i].pivot.x / _sprites[i].rect.width,
                        (_sprites[i].pivot.y / _sprites[i].rect.height) - StepRate);
                    }
                    else
                    {
                        _meta.pivot = new Vector2(_sprites[i].pivot.x / _sprites[i].rect.width, (_sprites[i].pivot.y / _sprites[i].rect.height));
                    }
                    _meta.name = _sprites[i].name;
                    _meta.rect = _sprites[i].rect;
                    _meta.rect.position = _meta.rect.position;
                    _meta.border = _sprites[i].border;

                    _data.Add(_meta);
                }
            }
            TextureImporter _importer_ = AssetImporter.GetAtPath(_path_) as TextureImporter;
            _importer_.allowAlphaSplitting = true;
            _importer_.isReadable = true;
            //Add MetaData back to spriteshet from List to Array
            _importer_.spritesheet = _data.ToArray();
            //Rebuild asset
            AssetDatabase.ImportAsset(_path_, ImportAssetOptions.ForceUpdate);
            //ChangeTargetSprite();
        }

        else if (Input.GetKeyDown(KeyCode.G))
        {
            UnityEngine.Object[] _objects = AssetDatabase.LoadAllAssetRepresentationsAtPath(_path_);
            List<SpriteMetaData> _data = new List<SpriteMetaData>();
            if (mode.Equals(Mode.Suit))
            {
                for (int i = 0; i < _objects.Length; i++)
                {
                    SpriteMetaData _meta = new SpriteMetaData();
                    _meta.alignment = 9;
                    if (i == Num1)
                    {
                        _meta.pivot = new Vector2(_sprites[i].pivot.x / _sprites[i].rect.width,
                        (_sprites[i].pivot.y / _sprites[i].rect.height) + StepRate);
                    }
                    else
                    {
                        _meta.pivot = new Vector2(_sprites[i].pivot.x / _sprites[i].rect.width, (_sprites[i].pivot.y / _sprites[i].rect.height));
                    }
                    _meta.name = _sprites[i].name;
                    _meta.rect = _sprites[i].rect;
                    _meta.rect.position = _meta.rect.position;
                    _meta.border = _sprites[i].border;

                    _data.Add(_meta);
                }
            }
            else if (mode.Equals(Mode.OtherSprite))
            {
                for (int i = 0; i < _objects.Length; i++)
                {
                    SpriteMetaData _meta = new SpriteMetaData();
                    _meta.alignment = 9;
                    if (i == Num2)
                    {
                        _meta.pivot = new Vector2(_sprites[i].pivot.x / _sprites[i].rect.width,
                        (_sprites[i].pivot.y / _sprites[i].rect.height) + StepRate);
                    }
                    else
                    {
                        _meta.pivot = new Vector2(_sprites[i].pivot.x / _sprites[i].rect.width, (_sprites[i].pivot.y / _sprites[i].rect.height));
                    }
                    _meta.name = _sprites[i].name;
                    _meta.rect = _sprites[i].rect;
                    _meta.rect.position = _meta.rect.position;
                    _meta.border = _sprites[i].border;

                    _data.Add(_meta);
                }
            }
            TextureImporter _importer_ = AssetImporter.GetAtPath(_path_) as TextureImporter;
            _importer_.allowAlphaSplitting = true;
            _importer_.isReadable = true;
            //Add MetaData back to spriteshet from List to Array
            _importer_.spritesheet = _data.ToArray();
            //Rebuild asset
            AssetDatabase.ImportAsset(_path_, ImportAssetOptions.ForceUpdate);
            //ChangeTargetSprite();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            UnityEngine.Object[] _objects = AssetDatabase.LoadAllAssetRepresentationsAtPath(_path_);
            List<SpriteMetaData> _data = new List<SpriteMetaData>();
            if (mode.Equals(Mode.Suit))
            {
                for (int i = 0; i < _objects.Length; i++)
                {
                    SpriteMetaData _meta = new SpriteMetaData();
                    _meta.alignment = 9;
                    if (i == Num1)
                    {
                        _meta.pivot = new Vector2((_sprites[i].pivot.x / _sprites[i].rect.width) + StepRate,
                        (_sprites[i].pivot.y / _sprites[i].rect.height));
                    }
                    else
                    {
                        _meta.pivot = new Vector2(_sprites[i].pivot.x / _sprites[i].rect.width, (_sprites[i].pivot.y / _sprites[i].rect.height));
                    }
                    _meta.name = _sprites[i].name;
                    _meta.rect = _sprites[i].rect;
                    _meta.rect.position = _meta.rect.position;
                    _meta.border = _sprites[i].border;

                    _data.Add(_meta);
                }
            }
            else if (mode.Equals(Mode.OtherSprite))
            {
                for (int i = 0; i < _objects.Length; i++)
                {
                    SpriteMetaData _meta = new SpriteMetaData();
                    _meta.alignment = 9;
                    if (i == Num2)
                    {
                        _meta.pivot = new Vector2((_sprites[i].pivot.x / _sprites[i].rect.width) + StepRate,
                        (_sprites[i].pivot.y / _sprites[i].rect.height));
                    }
                    else
                    {
                        _meta.pivot = new Vector2(_sprites[i].pivot.x / _sprites[i].rect.width, (_sprites[i].pivot.y / _sprites[i].rect.height));
                    }
                    _meta.name = _sprites[i].name;
                    _meta.rect = _sprites[i].rect;
                    _meta.rect.position = _meta.rect.position;
                    _meta.border = _sprites[i].border;

                    _data.Add(_meta);
                }
            }
            TextureImporter _importer_ = AssetImporter.GetAtPath(_path_) as TextureImporter;

            _importer_.allowAlphaSplitting = true;
            _importer_.isReadable = true;
            //Add MetaData back to spriteshet from List to Array
            _importer_.spritesheet = _data.ToArray();
            //Rebuild asset
            AssetDatabase.ImportAsset(_path_, ImportAssetOptions.ForceUpdate);
            //ChangeTargetSprite();
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            UnityEngine.Object[] _objects = AssetDatabase.LoadAllAssetRepresentationsAtPath(_path_);
            List<SpriteMetaData> _data = new List<SpriteMetaData>();
            if (mode.Equals(Mode.Suit))
            {
                for (int i = 0; i < _objects.Length; i++)
                {
                    SpriteMetaData _meta = new SpriteMetaData();
                    _meta.alignment = 9;
                    if (i == Num1)
                    {
                        _meta.pivot = new Vector2((_sprites[i].pivot.x / _sprites[i].rect.width) - StepRate,
                        (_sprites[i].pivot.y / _sprites[i].rect.height));
                    }
                    else
                    {
                        _meta.pivot = new Vector2(_sprites[i].pivot.x / _sprites[i].rect.width, (_sprites[i].pivot.y / _sprites[i].rect.height));
                    }
                    _meta.name = _sprites[i].name;
                    _meta.rect = _sprites[i].rect;
                    _meta.rect.position = _meta.rect.position;
                    _meta.border = _sprites[i].border;

                    _data.Add(_meta);
                }
            }
            else if (mode.Equals(Mode.OtherSprite))
            {
                for (int i = 0; i < _objects.Length; i++)
                {
                    SpriteMetaData _meta = new SpriteMetaData();
                    _meta.alignment = 9;
                    if (i == Num2)
                    {
                        _meta.pivot = new Vector2((_sprites[i].pivot.x / _sprites[i].rect.width) - StepRate,
                        (_sprites[i].pivot.y / _sprites[i].rect.height));
                    }
                    else
                    {
                        _meta.pivot = new Vector2(_sprites[i].pivot.x / _sprites[i].rect.width, (_sprites[i].pivot.y / _sprites[i].rect.height));
                    }
                    _meta.name = _sprites[i].name;
                    _meta.rect = _sprites[i].rect;
                    _meta.rect.position = _meta.rect.position;
                    _meta.border = _sprites[i].border;

                    _data.Add(_meta);
                }
            }
            TextureImporter _importer_ = AssetImporter.GetAtPath(_path_) as TextureImporter;
            //Add MetaData back to spriteshet from List to Array
            _importer_.allowAlphaSplitting = true;
            _importer_.isReadable = true;
            _importer_.spritesheet = _data.ToArray();
            //Rebuild asset
            AssetDatabase.ImportAsset(_path_, ImportAssetOptions.ForceUpdate);
            //ChangeTargetSprite();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (mode.Equals(Mode.Suit))
            {
                if (TempNum1 <= _sprites.Length - 1)
                {
                    NumText1.text = TempNum1.ToString();
                    Num1 = TempNum1;
                    TempNum1 = 0;
                    ChangeTargetSprite();
                }
            }
            else
            {
                if (TempNum2 <= _sprites.Length - 1)
                {
                    NumText2.text = TempNum2.ToString();
                    Num2 = TempNum2;
                    TempNum2 = 0;
                    ChangeTargetSprite();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (mode.Equals(Mode.Suit))
            {
                if (10 * TempNum1 + 1 <= _sprites.Length - 1)
                {
                    TempNum1 *= 10;
                    TempNum1 += 1;
                }
            }
            else
            {
                if (10 * TempNum2 + 1 <= _sprites.Length - 1)
                {
                    TempNum2 *= 10;
                    TempNum2 += 1;
                }
            }

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (mode.Equals(Mode.Suit))
            {
                if (10 * TempNum1 + 2 <= _sprites.Length - 1)
                {
                    TempNum1 *= 10;
                    TempNum1 += 2;
                }
            }
            else
            {
                if (10 * TempNum2 + 2 <= _sprites.Length - 1)
                {
                    TempNum2 *= 10;
                    TempNum2 += 2;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (mode.Equals(Mode.Suit))
            {
                if (10 * TempNum1 + 3 <= _sprites.Length - 1)
                {
                    TempNum1 *= 10;
                    TempNum1 += 3;
                }
            }
            else
            {
                if (10 * TempNum2 + 3 <= _sprites.Length - 1)
                {
                    TempNum2 *= 10;
                    TempNum2 += 3;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (mode.Equals(Mode.Suit))
            {
                if (10 * TempNum1 + 4 <= _sprites.Length - 1)
                {
                    TempNum1 *= 10;
                    TempNum1 += 4;
                }
            }
            else
            {
                if (10 * TempNum2 + 4 <= _sprites.Length - 1)
                {
                    TempNum2 *= 10;
                    TempNum2 += 4;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (mode.Equals(Mode.Suit))
            {
                if (10 * TempNum1 + 5 <= _sprites.Length - 1)
                {
                    TempNum1 *= 10;
                    TempNum1 += 5;
                }
            }
            else
            {
                if (10 * TempNum2 + 5 <= _sprites.Length - 1)
                {
                    TempNum2 *= 10;
                    TempNum2 += 5;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (mode.Equals(Mode.Suit))
            {
                if (10 * TempNum1 + 6 <= _sprites.Length - 1)
                {
                    TempNum1 *= 10;
                    TempNum1 += 6;
                }
            }
            else
            {
                if (10 * TempNum2 + 6 <= _sprites.Length - 1)
                {
                    TempNum2 *= 10;
                    TempNum2 += 6;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (mode.Equals(Mode.Suit))
            {
                if (10 * TempNum1 + 7 <= _sprites.Length - 1)
                {
                    TempNum1 *= 10;
                    TempNum1 += 7;
                }
            }
            else
            {
                if (10 * TempNum2 + 7 <= _sprites.Length - 1)
                {
                    TempNum2 *= 10;
                    TempNum2 += 7;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (mode.Equals(Mode.Suit))
            {
                if (10 * TempNum1 + 8 <= _sprites.Length - 1)
                {
                    TempNum1 *= 10;
                    TempNum1 += 8;
                }
            }
            else
            {
                if (10 * TempNum2 + 8 <= _sprites.Length - 1)
                {
                    TempNum2 *= 10;
                    TempNum2 += 8;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (mode.Equals(Mode.Suit))
            {
                if (10 * TempNum1 + 9 <= _sprites.Length - 1)
                {
                    TempNum1 *= 10;
                    TempNum1 += 9;
                }
            }
            else
            {
                if (10 * TempNum2 + 9 <= _sprites.Length - 1)
                {
                    TempNum2 *= 10;
                    TempNum2 += 9;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (mode.Equals(Mode.Suit))
            {
                if (10 * TempNum1 <= _sprites.Length - 1)
                {
                    TempNum1 *= 10;

                }
            }
            else
            {
                if (10 * TempNum2 <= _sprites.Length - 1)
                {
                    TempNum2 *= 10;

                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            Resize_Bilinear();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Resize_Mitchall();
        }
    }
}
#endif