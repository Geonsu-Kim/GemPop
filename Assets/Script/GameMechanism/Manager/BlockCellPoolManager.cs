using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCellPoolManager : SingletonBase<BlockCellPoolManager>
{
    private const string nameCell = "Cell";
    private const string nameBlock = "Block";

    public GameObject cellObj;
    public GameObject blockObj;
    public List<GameObject> Pool_Cell;
    public List<GameObject> Pool_Block;

    void Awake()
    {
        Pool_Cell = new List<GameObject>();
        Pool_Block = new List<GameObject>();
        InstantiateBlockCell();
    }
    void InstantiateBlockCell()
    {
        for (int i = 0; i < 81; i++)
        {
            GameObject newCell = Instantiate(cellObj);
            newCell.SetActive(false);
            newCell.name = nameCell;
            Pool_Cell.Add(newCell);
        }
        for (int i = 0; i < 200; i++)
        {
            GameObject newBlock = Instantiate(blockObj);
            newBlock.SetActive(false);
            newBlock.name = nameBlock;
            Pool_Block.Add(newBlock);
        }
    }
    void InstantiateNewBlock()
    {
        GameObject newBlock = Instantiate(blockObj);
        newBlock.SetActive(false);
        newBlock.name = nameBlock;
        Pool_Block.Add(newBlock);
    }
    public GameObject GetBlock()
    {
        for (int i = 0; i < Pool_Block.Count; i++)
        {
            if (!Pool_Block[i].activeSelf)
            { 
                return Pool_Block[i];
            }
            else
            {
                if(i== Pool_Block.Count - 1)
                {
                    InstantiateNewBlock();
                    return Pool_Block[i + 1];
                }
            }
        }
        return null;
    }
    // Update is called once per frame
}
