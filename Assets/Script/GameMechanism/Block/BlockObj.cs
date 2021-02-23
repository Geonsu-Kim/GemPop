using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockObj : MonoBehaviour
{
    private Block mBlock; public Block MBlock { set { mBlock = value; } get { return mBlock; } }
    private SpriteRenderer mSprite;
    [SerializeField] BlockConfig mConfig;
    void Awake()
    {
        mSprite = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        UpdateView(false);
    }
    public void UpdateView(bool pValueChanged)
    {
        if (mBlock.MType == BlockType.EMPTY)
        {
            mSprite.sprite = null;
            mSprite.material = null;
            return;
        }
        else if(mBlock.MType == BlockType.HORIZON|| mBlock.MType == BlockType.VERTICAL)
        {
            mSprite.sprite = mConfig.itemBlockSprites[(int)mBlock.MType - 2];
        }

        mSprite.material = mConfig.blockMaterials[(int)mBlock.MColor];
    }
}
