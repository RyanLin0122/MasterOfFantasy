using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

#if UNITY_EDITOR
//Copy and paste atlas settings to another atlas editor
public class AtlasCopyPaste : EditorWindow
{

    public Texture2D copyFrom;           //Sprite Atlas to copy from settings
    public Texture2D pasteTo;           //Sprite atlas where to paste settings
    public float Off_X;
    public float Off_Y;
    private Sprite[] _sprites;           //Collection of sprites from source texture for faster referencing

    [MenuItem("Window/Atlas CopyPaste Editor")]
    static void Init()
    {
        // Window Set-Up
        AtlasCopyPaste window = EditorWindow.GetWindow(typeof(AtlasCopyPaste), false, "Atlas Editor", true) as AtlasCopyPaste;
        window.minSize = new Vector2(260, 170); window.maxSize = new Vector2(260, 170);
        window.Show();
    }

    //Show UI
    void OnGUI()
    {

        copyFrom = (Texture2D)EditorGUILayout.ObjectField("範例", copyFrom, typeof(Texture2D), true);
        pasteTo = (Texture2D)EditorGUILayout.ObjectField("目標", pasteTo, typeof(Texture2D), true);
        Off_X = EditorGUILayout.FloatField("水平位移:", Off_X);
        Off_Y = EditorGUILayout.FloatField("垂直位移:", Off_Y);

        EditorGUILayout.Space();

        if (GUILayout.Button("Copy Paste"))
        {
            if (copyFrom != null && pasteTo != null)
                CopyPaste();
            else
                Debug.LogWarning("Forgot to set the textures?");
        }

        Repaint();
    }

    //Do the copy paste
    private void CopyPaste()
    {
        if (copyFrom.width != pasteTo.width || copyFrom.height != pasteTo.height)
        {
            //Better a warning if textures doesn't match than a crash or error
            Debug.LogWarning("Unable to proceed, textures size doesn't match.");
            int UpBorder = (pasteTo.height - copyFrom.height) / 2;
            int LeftBorder = (pasteTo.width - copyFrom.width) / 2;
            if (!IsAtlas(copyFrom))
            {
                Debug.LogWarning("Unable to proceed, the source texture is not a sprite atlas.");
                return;
            }

            //Proceed to read all sprites from CopyFrom texture and reassign to a TextureImporter for the end result
            UnityEngine.Object[] _objects_ = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(copyFrom));

            if (_objects_ != null && _objects_.Length > 0)
                _sprites = new Sprite[_objects_.Length];

            for (int i = 0; i < _objects_.Length; i++)
                _sprites[i] = _objects_[i] as Sprite;

            //Get Texture Importer of pasteTo texture for assigning sprite variables (pixxelsToUnit is not counted)
            string _path_ = AssetDatabase.GetAssetPath(pasteTo);
            TextureImporter _importer_ = AssetImporter.GetAtPath(_path_) as TextureImporter;
            //   _importer.isReadable = true;

            //Force settings to Sprite and Multiple just to be sure
            _importer_.textureType = TextureImporterType.Sprite;
            _importer_.spriteImportMode = SpriteImportMode.Multiple;

            //Reassigning to new atlas
            List<SpriteMetaData> _data_ = new List<SpriteMetaData>();

            for (int i = 0; i < _objects_.Length; i++)
            {
                SpriteMetaData _meta = new SpriteMetaData();
                _meta.alignment = 9;
                _meta.pivot = new Vector2(_sprites[i].pivot.x / _sprites[i].rect.width, _sprites[i].pivot.y / _sprites[i].rect.height);
                _meta.name = pasteTo.name + "_" + i.ToString();
                _meta.rect = _sprites[i].rect;
                _meta.rect.position = new Vector2(_sprites[i].rect.position.x + Off_X + LeftBorder, _sprites[i].rect.position.y + UpBorder + Off_Y);
                _meta.border = _sprites[i].border;

                _data_.Add(_meta);
            }

            //Add MetaData back to spriteshet from List to Array
            _importer_.spritesheet = _data_.ToArray();

            //Rebuild asset
            AssetDatabase.ImportAsset(_path_, ImportAssetOptions.ForceUpdate);
            return;
        }

        if (!IsAtlas(copyFrom))
        {
            Debug.LogWarning("Unable to proceed, the source texture is not a sprite atlas.");
            return;
        }

        //Proceed to read all sprites from CopyFrom texture and reassign to a TextureImporter for the end result
        UnityEngine.Object[] _objects = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(copyFrom));

        if (_objects != null && _objects.Length > 0)
            _sprites = new Sprite[_objects.Length];

        for (int i = 0; i < _objects.Length; i++)
            _sprites[i] = _objects[i] as Sprite;

        //Get Texture Importer of pasteTo texture for assigning sprite variables (pixxelsToUnit is not counted)
        string _path = AssetDatabase.GetAssetPath(pasteTo);
        TextureImporter _importer = AssetImporter.GetAtPath(_path) as TextureImporter;
        //   _importer.isReadable = true;

        //Force settings to Sprite and Multiple just to be sure
        _importer.textureType = TextureImporterType.Sprite;
        _importer.spriteImportMode = SpriteImportMode.Multiple;

        //Reassigning to new atlas
        List<SpriteMetaData> _data = new List<SpriteMetaData>();

        for (int i = 0; i < _objects.Length; i++)
        {
            SpriteMetaData _meta = new SpriteMetaData();
            _meta.alignment = 9;
            _meta.pivot = new Vector2(_sprites[i].pivot.x / _sprites[i].rect.width, _sprites[i].pivot.y / _sprites[i].rect.height);
            _meta.name = pasteTo.name + "_" + i.ToString();
            _meta.rect = _sprites[i].rect;
            _meta.rect.position = new Vector2(_sprites[i].rect.position.x, _sprites[i].rect.position.y);
            _meta.border = _sprites[i].border;

            _data.Add(_meta);
        }

        //Add MetaData back to spriteshet from List to Array
        _importer.spritesheet = _data.ToArray();

        //Rebuild asset
        AssetDatabase.ImportAsset(_path, ImportAssetOptions.ForceUpdate);
    }

    //Check that the texture is an actual atlas and not a normal texture
    private bool IsAtlas(Texture2D tex)
    {
        string _path = AssetDatabase.GetAssetPath(tex);
        TextureImporter _importer = AssetImporter.GetAtPath(_path) as TextureImporter;

        return _importer.textureType == TextureImporterType.Sprite && _importer.spriteImportMode == SpriteImportMode.Multiple;
    }
}
#endif