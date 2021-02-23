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
        switch (MBlock.MType)
        {
            case BlockType.EMPTY:
                mSprite.sprite = null;
                mSprite.material = null;
                return;
            case BlockType.BASIC:
                mSprite.sprite = mConfig.basicBlockSprites;

                break;
            case BlockType.VERTICAL:
            case BlockType.HORIZON:
                mSprite.sprite = mConfig.itemBlockSprites[(int)mBlock.MType - 2];
                break;
        }
        mSprite.material = mConfig.blockMaterials[(int)mBlock.MColor];

    }
}
