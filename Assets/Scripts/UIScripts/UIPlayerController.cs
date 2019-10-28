using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class UIPlayerController : MonoBehaviour
{
    public GameObject GameObjectNotifyRestartGame;
    public KeyCode KeyCodeRestart;

    private UnityAction m_actDelayRestart;
    private bool m_bReadyRestart;
    private PlayerControl1 m_playerCtrl;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bReadyRestart == true)
        {
            if( Input.GetKeyUp(KeyCodeRestart) == true )
            {
                m_bReadyRestart = false;
                GameObjectNotifyRestartGame.gameObject.SetActive(false);
                if (m_playerCtrl!= null)
                {
                    m_playerCtrl.StartCoroutine(m_playerCtrl.DelayLoadScene());
                }
            }
        }
    }
    public void PlayerDieAction(PlayerControl1 _playerCtrl)
    {
        GameObjectNotifyRestartGame.gameObject.SetActive(true);
        m_bReadyRestart = true;
        m_playerCtrl = _playerCtrl;
    }
}
