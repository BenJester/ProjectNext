using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBossShoot : MonoBehaviour
{
    public SplitBossPart TransBody;
    public SplitBossPart TransLeftHand;
    public SplitBossPart TransRightHand;

    public Transform TransTargetBody;
    public Transform TransTargetLeftHand;
    public Transform TransTargetRightHand;

    public float PartShootingTime;

    public bool ShootPart;

    public float ShootForce;

    public PlayerControl1 PlayerCtrl1;

    private Rigidbody2D m_rigShootPart;
    private float m_fCurrentShootingTime;
    private List<SplitBossPart> m_lstShootPart;
    private List<Transform> m_lstTarget;
    private bool m_bStartMove;
    // Start is called before the first frame update
    void Start()
    {
        m_lstShootPart = new List<SplitBossPart>();
        //m_lstShootPart.Add(TransBody);
        m_lstShootPart.Add(TransLeftHand);
        m_lstShootPart.Add(TransRightHand);

        m_lstTarget = new List<Transform>();
        //m_lstTarget.Add(TransTargetBody);
        m_lstTarget.Add(TransTargetLeftHand);
        m_lstTarget.Add(TransTargetRightHand);
    }

    // Update is called once per frame
    void Update()
    {
        if(ShootPart == true)
        {
            ShootPart = false;
            _shootPart(TransLeftHand,new Vector2());
        }
        if(m_bStartMove == true)
        {
            int nIdxTarget = 0;
            foreach(SplitBossPart _part in m_lstShootPart)
            {
                Vector2 vecDir = new Vector2();
                if (transform.position.x < PlayerCtrl1.transform.position.x)
                {
                    vecDir = Vector2.left;
                }
                else
                {
                    vecDir = Vector2.right;
                }
                Transform _flyTrans = m_lstTarget[nIdxTarget];
                _part.ShootReady();
                _part.RotateTarget(_flyTrans);
                _flyTrans.transform.Translate(vecDir * ShootForce);
                m_fCurrentShootingTime += Time.deltaTime;
                nIdxTarget++;
            }
            if (m_fCurrentShootingTime >= PartShootingTime)
            {
                m_bStartMove = false;
            }
        }
    }
    private void _shootPart(SplitBossPart _part, Vector2 vecDir)
    {
        //_part.ThingEnable(true);
        //m_rigShootPart = _part.GetComponent<Rigidbody2D>();
        m_bStartMove = true;
        m_fCurrentShootingTime = 0.0f;

        int _idxTarget = 0;
        foreach (SplitBossPart _shootPart in m_lstShootPart)
        {
            Transform _targetTrans = m_lstTarget[_idxTarget];
            _targetTrans.position = _shootPart.TransReadySplit.position;
            _idxTarget++;
        }
    }
}
