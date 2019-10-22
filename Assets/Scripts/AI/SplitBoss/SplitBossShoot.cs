using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBossShoot : MonoBehaviour
{
    public SplitBossPart TransBody;
    public SplitBossPart TransLeftHand;
    public SplitBossPart TransRightHand;
    
    public float PartShootingTime;

    public bool ShootPart;

    public float ShootForce;

    public PlayerControl1 PlayerCtrl1;

    public GameObject GMObjectFlyObject;

    private Rigidbody2D m_rigShootPart;
    private float m_fCurrentShootingTime;
    private List<SplitBossPart> m_lstShootPart;
    private List<Transform> m_lstExistTarget;
    private List<Transform> m_lstTarget;
    private bool m_bStartMove;

    // Start is called before the first frame update
    void Start()
    {
        m_lstShootPart = new List<SplitBossPart>();
        m_lstShootPart.Add(TransLeftHand);
        m_lstShootPart.Add(TransRightHand);

        m_lstExistTarget = new List<Transform>();
        m_lstExistTarget.Add(TransLeftHand.transform);
        m_lstExistTarget.Add(TransRightHand.transform);

        m_lstTarget = new List<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ShootPart == true)
        {
            ShootPart = false;
            _shootPart();
        }
        if(m_bStartMove == true)
        {
            int nIdxTarget = 0;
            foreach(SplitBossPart _part in m_lstShootPart)
            {
                Vector2 vecDir = new Vector2();
                if (transform.position.x < PlayerCtrl1.transform.position.x)
                {
                    vecDir = Vector2.right;
                }
                else
                {
                    vecDir = Vector2.left;
                }
                Transform _transExist = m_lstExistTarget[nIdxTarget];
                Transform _transProcess = null;
                //if( nIdxTarget < m_lstTarget.Count - 1 || m_lstTarget.Count == 0)
                if( nIdxTarget == m_lstTarget.Count )
                {
                    GameObject objIns = Instantiate(GMObjectFlyObject, _transExist.position, Quaternion.identity);
                    _transProcess = objIns.transform;
                    m_lstTarget.Add(_transProcess);
                    _transProcess.position = _transExist.position;
                }
                else
                {
                    _transProcess = m_lstTarget[nIdxTarget];
                }
                _part.ShootReady();
                _part.RotateTarget(_transProcess);
                _transProcess.transform.Translate(vecDir * ShootForce);
                m_fCurrentShootingTime += Time.deltaTime;
                nIdxTarget++;
            }
            if (m_fCurrentShootingTime >= PartShootingTime)
            {
                m_bStartMove = false;
            }
        }
    }
    public void BossShoot()
    {
        _shootPart();
    }
    private void _shootPart()
    {
        m_bStartMove = true;
        m_fCurrentShootingTime = 0.0f;

        int _idxTarget = 0;
        foreach (SplitBossPart _partBoss in m_lstShootPart)
        {
            Transform _targetTrans = m_lstExistTarget[_idxTarget];
            _targetTrans.position = _partBoss.TransReadySplit.position;
            _idxTarget++;
        }
    }
}
