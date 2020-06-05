using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.Linq;
using System.Text;
using System.Xml;
[System.Serializable]
public class ReplaceString
{
    public string strSource;
    public string strReplace;
}
public class CountTilemap : MonoBehaviour
{
    public List<Tilemap> LstAssignedTitleMap;
    public List<ReplaceString> LstReplaceString;
    public bool NeedReplace;
    public string GenerateFileName;
    public string TsxFileName;

    private Dictionary<string, string> m_dicReplace;
    private int m_nMaxWidth;
    private int m_nMaxHeight;
    public static Dictionary<string, int> m_dicSpriteIndex = new Dictionary<string, int>();
    public static bool s_InitSprite;
    // Start is called before the first frame update
    void Start()
    {
        _replaceInit();
        int a = 0;
    }
    private void _replaceInit()
    {
        if (NeedReplace == true)
        {
            m_dicReplace = new Dictionary<string, string>();
            foreach (ReplaceString str in LstReplaceString)
            {
                if (m_dicReplace.ContainsKey(str.strSource) != true)
                {
                    if (m_dicReplace.ContainsValue(str.strReplace) != true)
                    {
                        m_dicReplace[str.strSource] = str.strReplace;
                    }
                    else
                    {
                        Debug.Assert(false, string.Format("contain value"));
                    }
                }
                else
                {
                    Debug.Assert(false, string.Format("contain key"));
                }
            }
        }
    }
    [ContextMenu("GenerateXmlFile")]
    void GenerateXmlFile()
    {
        _replaceInit();

        int nFileIdx = 0;
        foreach(Tilemap _mapTile in LstAssignedTitleMap)
        {
            if (_mapTile != null)
            {
                string strFileName = string.Format("{0}_{1}", GenerateFileName,nFileIdx.ToString("000"));
                _generateTileContent(_mapTile, strFileName);
                nFileIdx++;
            }
        }
    }
    //width="1312" height="96" tilewidth="16" tileheight="16" infinite="0" nextlayerid="2" nextobjectid="1">

    private void makeXml(int width, int height, string strContent, string strFileName,string strTsxFileName = "MeowTileSet", int tilewidth = 16, int tileheight = 16, int infinite = 0, int nextlayerid = 2, int nextobjectid = 1)
    {

        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.IndentChars = "\t";
        if(strFileName.Length == 0)
        {
            strFileName = "SceneLevelBlock";
        }
        XmlWriter writer = XmlWriter.Create(strFileName+".tmx", settings);

        //map element
        writer.WriteStartElement("map");
        writer.WriteStartAttribute("version");
        writer.WriteValue("1.2");
        writer.WriteEndAttribute();

        writer.WriteStartAttribute("tiledversion");
        writer.WriteValue("1.3.1");
        writer.WriteEndAttribute();

        writer.WriteStartAttribute("orientation");
        writer.WriteValue("orthogonal");
        writer.WriteEndAttribute();

        writer.WriteStartAttribute("renderorder");
        writer.WriteValue("right-down");
        writer.WriteEndAttribute();

        writer.WriteStartAttribute("compressionlevel");
        writer.WriteValue("-1");
        writer.WriteEndAttribute();


        writer.WriteStartAttribute("width");
        writer.WriteValue(width.ToString());
        writer.WriteEndAttribute();

        writer.WriteStartAttribute("height");
        writer.WriteValue(height.ToString());
        writer.WriteEndAttribute();

        writer.WriteStartAttribute("tilewidth");
        writer.WriteValue(tilewidth.ToString());
        writer.WriteEndAttribute();

        writer.WriteStartAttribute("tileheight");
        writer.WriteValue(tileheight.ToString());
        writer.WriteEndAttribute();

        writer.WriteStartAttribute("infinite");
        writer.WriteValue(infinite.ToString());
        writer.WriteEndAttribute();

        writer.WriteStartAttribute("nextlayerid");
        writer.WriteValue(nextlayerid.ToString());
        writer.WriteEndAttribute();

        writer.WriteStartAttribute("nextobjectid");
        writer.WriteValue(nextobjectid.ToString());
        writer.WriteEndAttribute();

        //tileset
        writer.WriteStartElement("tileset");
        writer.WriteStartAttribute("firstgid");
        writer.WriteValue("1");
        writer.WriteEndAttribute();

        writer.WriteStartAttribute("source");
        string strTsx = string.Format("{0}.tsx", strTsxFileName);
        writer.WriteValue(strTsx);
        writer.WriteEndAttribute();

        writer.WriteEndElement();//tileset end

        //layer
        writer.WriteStartElement("layer");
        writer.WriteStartAttribute("id");
        writer.WriteValue("1");
        writer.WriteEndAttribute();

        writer.WriteStartAttribute("name");
        writer.WriteValue("Tile Layer 1");
        writer.WriteEndAttribute();

        writer.WriteStartAttribute("width");
        writer.WriteValue(width.ToString());
        writer.WriteEndAttribute();

        writer.WriteStartAttribute("height");
        writer.WriteValue(height.ToString());
        writer.WriteEndAttribute();

        //< data encoding = "csv" >
        writer.WriteStartElement("data");
        writer.WriteStartAttribute("encoding");
        writer.WriteValue("csv");
        writer.WriteEndAttribute();
        writer.WriteString(strContent);
        writer.WriteEndElement();//data end

        writer.WriteEndElement();//layer end


        writer.WriteEndElement();//map end
        writer.Flush();
        writer.Close();

    }
    private void _generateTileContent(Tilemap _tile,string strGenerateFileName)
    {
        BoundsInt bounds = _tile.cellBounds;
        TileBase[] allTiles = _tile.GetTilesBlock(bounds);
        StringBuilder strBuf = new StringBuilder();
        StringBuilder strNotInclude = new StringBuilder();
        HashSet<string> setNotInclude = new HashSet<string>();
        for(int y = bounds.size.y - 1; y >= 0 ; y--)
        {
            for(int x = 0; x < bounds.size.x; x++)
            {
                int nIdx = y * bounds.size.x + x;
                TileBase _base = allTiles[nIdx];
                Vector3Int vecPos = new Vector3Int(x + _tile.origin.x, y + _tile.origin.y, 0);
                Sprite sp = _tile.GetSprite(vecPos);
                if( sp != null )
                {
                    int nIdxSprite = _getIndex(sp) + 1;
                    string str = string.Format("{0}", nIdxSprite);
                    strBuf.Append(str);
                    Debug.Log(sp.name);
                }
                else
                {
                    strBuf.Append("0");
                }
                //if (_base != null)
                //{
                //    if( _base is RuleTile)
                //    {
                //        RuleTile _ruleSpec = _base as RuleTile;
                //        //_rule.m_DefaultSprite;
                //        if (NeedReplace == true)
                //        {
                //            RuleTile _rule = _base as RuleTile;
                //            if (m_dicReplace.ContainsKey(_base.name) == true)
                //            {
                //                strBuf.Append(string.Format("{0}", m_dicReplace[_base.name]));
                //            }
                //            else
                //            {
                //                //Debug.Assert(false);
                //                setNotInclude.Add(_rule.name);
                //                strBuf.Append(string.Format("{0}", _rule.name));
                //                //strNotInclude.Append(string.Format("{0}, ", _rule.name));
                //            }
                //        }
                //        else
                //        {
                //            int nIdxSp = GetSpriteIndexFromDic(_ruleSpec.m_DefaultSprite);
                //            //Debug.Log(_ruleSpec.m_DefaultSprite.name);
                //            string str = string.Format("{0}", nIdxSp);
                //            int nIdxSprite = _getIndex(_ruleSpec.m_DefaultSprite);
                //            str = string.Format("{0}", nIdxSprite);
                //            strBuf.Append(str);
                //        }
                //    }
                //    else
                //    {
                //        if( _base is Tile)
                //        {
                //            Tile _tileIns = _base as Tile;
                //            int nIdxSprite = _getIndex(_tileIns.sprite);
                //            string str = string.Format("{0}", nIdxSprite);
                //            strBuf.Append(str);
                //        }
                //        else
                //        {
                //            Debug.Assert(false);
                //        }
                //    }
                //}
                //else
                //{
                //    strBuf.Append("0");
                //}

                if (x == bounds.size.x - 1 && y == 0)
                {
                }
                else
                {
                    strBuf.Append(", ");
                }
            }
            strBuf.Append("\n");
        }
        Debug.Log(string.Format("[width:{0}][height:{1}]",_tile.size.x, _tile.size.y));
        Debug.Log(strBuf.ToString());
        foreach(string str in setNotInclude)
        {
            strNotInclude.Append(string.Format("{0}, ", str));
        }
        Debug.Log(strNotInclude.ToString());

        string strTsxFile = "";
        if(TsxFileName.Length > 0)
        {
            strTsxFile = TsxFileName + ".tsx";
        }
        else
        {
            strTsxFile = "MeowTileSet";
        }
        makeXml(bounds.size.x, bounds.size.y, strBuf.ToString(), strGenerateFileName, TsxFileName);
    }
    private int _getIndex(Sprite sp)
    {
        int nRowCounts = (int)(sp.texture.width / sp.textureRect.width);
        int nRow = (int)((sp.texture.height - sp.textureRect.min.y) / (sp.textureRect.height) - 1);
        int nCol = (int)((sp.textureRect.min.x) / (sp.textureRect.width));
        int nIdxSprite = nRowCounts * nRow + nCol;
        return nIdxSprite;
    }
    public static int GetSpriteIndexFromDic(Sprite spIns)
    {
        if(s_InitSprite == false)
        {
            s_InitSprite = true;
            string spriteSheet = AssetDatabase.GetAssetPath(spIns.texture);
            Object[] _objArray = AssetDatabase.LoadAllAssetRepresentationsAtPath(spriteSheet);
            Sprite[] sprites = _objArray.OfType<Sprite>().ToArray();
            int nIdxSp = 0;
            foreach(Sprite _sp in sprites)
            {
                m_dicSpriteIndex[_sp.name] = nIdxSp;
                nIdxSp++;
            }
        }
        return m_dicSpriteIndex[spIns.name];
    }
    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyUp(KeyCode.Space) == true )
        {
            //if(AssignedTitleMap != null)
            //{
            //    _generateTileContent(AssignedTitleMap, GenerateFileName);
            //}
        }
        if (Input.GetKeyUp(KeyCode.H) == true)
        {
            Tilemap[] _array = FindObjectsOfType<Tilemap>();
            foreach (Tilemap _tile in _array)
            {
                if (m_nMaxWidth < _tile.size.x)
                {
                    m_nMaxWidth = _tile.size.x;
                }
                if (m_nMaxHeight < _tile.size.y)
                {
                    m_nMaxHeight = _tile.size.y;
                }
            }
            Debug.Log(string.Format("[MaxWidth:{0}],[MaxHeight:{1}]", m_nMaxWidth, m_nMaxHeight));
        }
    }
}
