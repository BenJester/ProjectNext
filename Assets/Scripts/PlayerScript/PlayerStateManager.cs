using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerStateDefine.PlayerState_Typ m_curState;
    // Start is called before the first frame update
    void Start()
    {
        m_curState = PlayerStateDefine.PlayerState_Typ.PlayerState_None;
    }
    public void SetPlayerState(PlayerStateDefine.PlayerState_Typ _rState)
    {
        if(_rState != m_curState)
        {
            m_curState = _rState;
            Debug.Log(string.Format("state changed {0}", _rState));
        }
    }
    public PlayerStateDefine.PlayerState_Typ GetPlayerState()
    {
        return m_curState;
    }
}
