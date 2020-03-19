using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Text;
[System.Serializable]
public class ReplaceString
{
    public string strSource;
    public string strReplace;
}
public class CountTilemap : MonoBehaviour
{
    public Tilemap AssignedTitleMap;
    public List<ReplaceString> LstReplaceString;
    public bool NeedReplace;
    private Dictionary<string, string> m_dicReplace;
    private int m_nMaxWidth;
    private int m_nMaxHeight;
    // Start is called before the first frame update
    void Start()
    {
        if(NeedReplace == true)
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
        int a = 0;
    }
    private void _generateTileContent(Tilemap _tile)
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
                if(_base != null)
                {
                    if( _base is RuleTile)
                    {
                        if(NeedReplace == true)
                        {
                            RuleTile _rule = _base as RuleTile;
                            if (m_dicReplace.ContainsKey(_base.name) == true)
                            {
                                strBuf.Append(string.Format("{0}", m_dicReplace[_base.name]));
                            }
                            else
                            {
                                //Debug.Assert(false);
                                setNotInclude.Add(_rule.name);
                                strBuf.Append(string.Format("{0}", _rule.name));
                                //strNotInclude.Append(string.Format("{0}, ", _rule.name));
                            }
                        }
                        else
                        {
                            strBuf.Append("1");
                        }
                    }
                    else
                    {
                        if( _base is Tile)
                        {
                            strBuf.Append("1");
                        }
                        else
                        {
                            Debug.Assert(false);
                        }
                    }
                }
                else
                {
                    strBuf.Append("0");
                }

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
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyUp(KeyCode.Space) == true )
        {
            if(AssignedTitleMap != null)
            {
                _generateTileContent(AssignedTitleMap);
            }
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
