using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PnC.CasualGameKit;
using DG.Tweening;

public enum BlockType
{
    normal,
    Gem
}

/// <summary>
/// Tower's block.
/// Operations : hits, cascading hit operation etc.
/// </summary>
public class BlockScript : MonoBehaviour
{
    private BlockType _type;

    //Tower 
    // private int _level;
    //private int _index;
    private Tower _tower;
    private List<BlockScript> _levelRef;

    //Hit Calcaulation
    private float _initialPos;
    private float _overlapSphereRadius;
    private bool _isHit;
    private float _fallThreshHold;
    public Vector3 _centerPos;

    //Visual
    private Material _defaultMat;
    private Material _colorMat;

    [Header("Componenets")]
    [SerializeField]
    private MeshRenderer _renderer;
    [SerializeField]
    private Rigidbody _rigidBody;
    [SerializeField]
    private Collider _collider;

    [SerializeField]
    private MeshRenderer _specialMesh;

    #region Unity Events
    private void Start()
    {
        float radius = transform.lossyScale.y * 0.5f;
        _overlapSphereRadius = radius + 0.5f * radius;
        _centerPos = transform.position + Vector3.up * transform.localScale.y * 0.5f;
        _fallThreshHold = 0.2f * transform.lossyScale.y;
    }

    void Update()
    {
        //Detect falling
        if (!_isHit && _rigidBody.velocity.y < 0 && transform.position.y < _initialPos - _fallThreshHold)
        {
            _tower.RemoveBlock(this, _levelRef);

            //Experimetal : if gem type falls disable it.
            if (_type == BlockType.Gem)
            {
                gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region Init
    public void SetUpBlock(Tower tower, List<BlockScript> levelList, BlockType type = BlockType.normal)
    {
        _levelRef = levelList;
        _isHit = false;
        _tower = tower;
        _type = type;
        SetBlockType(_type);

        _initialPos = transform.position.y;
        _rigidBody.isKinematic = true;
        _collider.enabled = true;

        this.enabled = true; // this component is disabled when fallen
    }

    public void SetBlockType(BlockType type)
    {
        _type = type;
        bool isGem = type == BlockType.Gem;
        _renderer.gameObject.SetActive(!isGem);
        _specialMesh.gameObject.SetActive(isGem);
    }

    public void SetMaterials(Material defaultMat, Material colorMat)
    {
        _defaultMat = defaultMat;
        _colorMat = colorMat;
        _renderer.sharedMaterial = colorMat;
    }


    public void SetBlockState(bool state, bool forceColliderState = false)
    {
        if (state)
        {
            _renderer.sharedMaterial = _colorMat;
            _collider.enabled = true;
            _rigidBody.isKinematic = false;
        }
        else
        {
            _renderer.sharedMaterial = _defaultMat;
            _rigidBody.isKinematic = true;
            _collider.enabled = forceColliderState;
        }
    }

    public void SetColliderState(bool state)
    {
        _collider.enabled = state;
    }

    #endregion

    #region Gameplay

    public bool CompareColor(Material material)
    {
        #region Experimental extra feature
        if (_type == BlockType.Gem)
            return true;
        #endregion

        return material == _renderer.sharedMaterial;
    }

    public void Hit(bool isHitSourceBall = true)
    {
        if (_isHit)
        {
            return;
        }
        _isHit = true;

        #region Experimental extra feature
        if (isHitSourceBall && _type == BlockType.Gem)
        {
            GameObject gemBurstFx = ObjectPooler.Instance.GetPooledObject(ObjectPoolItems.GemBurstFx, true);
            gemBurstFx.transform.position = gameObject.transform.position;
            _tower.RemoveBlock(this, _levelRef);
            gameObject.SetActive(false);
            return;
        }
        #endregion


        ///hit cascade calculation:  Get the nearby collider, check if same color
        Collider[] hitColliders = Physics.OverlapSphere(_centerPos, _overlapSphereRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].transform.parent == null)
            {
                i++;
                continue;
            }
            BlockScript adjacentBlock = hitColliders[i].transform.parent.GetComponent<BlockScript>();
            if (adjacentBlock != null && adjacentBlock.CompareColor(_renderer.sharedMaterial)) //._renderer.sharedMaterial == _renderer.sharedMaterial)
            {
                adjacentBlock.Hit(false);
            }
            i++;
        }

        _tower.RemoveBlock(this, _levelRef);
        GameObject burstFx = ObjectPooler.Instance.GetPooledObject(ObjectPoolItems.BurstFx, true);
        burstFx.transform.position = gameObject.transform.position;

        //TODO : cache components in object pooler
        ParticleSystem.MainModule settings = burstFx.GetComponent<ParticleSystem>().main;
        settings.startColor = _renderer.sharedMaterial.color;

        gameObject.SetActive(false);
    }

    #endregion

    #region EXPERIMENTAL/ extra feature

    public void SetSpecialBlock(BlockType type, int dir)
    {
        _type = type;

        bool isGem = type == BlockType.Gem;
        _renderer.gameObject.SetActive(!isGem);
        _specialMesh.gameObject.SetActive(isGem);
        Vector3 __specialrendererScale = _specialMesh.transform.localScale;

        _specialMesh.transform.localScale = Vector3.zero;
        _specialMesh.transform.DOScale(__specialrendererScale, 0.2f);
        FindNextBlockPos(dir);
    }

    private void FindNextBlockPos(int dir)
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(FindNextBlockPosCourouinte(dir));
    }

    IEnumerator FindNextBlockPosCourouinte(int lastDir, bool bothDirChecked = false)
    {
        Collider[] hitColliders = Physics.OverlapSphere(_centerPos, _overlapSphereRadius);
        BlockScript adjacentBlock = null;

        int i = 0;
        //check the adjacent block.
        // if a block is available in the same direction then move in the same direction
        //else check opposite direction and move
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].transform.parent == null || hitColliders[i].transform.parent == transform)
            {
                i++;
                continue;
            }

            //checking if the movement directio is left or right
            Vector3 dir = hitColliders[i].transform.parent.position - transform.position;
            Vector3 perp = Vector3.Cross(dir.normalized, transform.forward);
            float dot = Vector3.Dot(perp, transform.up);
            bool directionCheck = Mathf.Sign(lastDir) == 1 ? dot <= -0.95f : dot >= 0.95f;

            if (directionCheck)
            {
                adjacentBlock = hitColliders[i].transform.parent.GetComponent<BlockScript>();
                break;
            }
            i++;
        }

        if (adjacentBlock == null)
        {
            if (!bothDirChecked)
            {
                StartCoroutine(FindNextBlockPosCourouinte(lastDir * -1, true));
            }
            yield break;
        }
        //TODO : put the delay in scriptable object
        yield return new WaitForSeconds(0.5f);

        adjacentBlock.SetSpecialBlock(BlockType.Gem, lastDir);
        SetBlockType(BlockType.normal);
    }
    #endregion

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position + Vector3.up * transform.localScale.y * 0.5f, _overlapSphereRadius);
    //}

}
