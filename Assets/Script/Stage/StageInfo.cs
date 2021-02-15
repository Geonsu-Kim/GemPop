using System;

[Serializable]
public class StageInfo 
{
    public int mRow;
    public int mCol;
    public int[] cells;
    public CellType GetCellType(int row,int col)
    {
        int revisedRow = (mRow -1) - row;
        if(cells.Length> revisedRow * mCol + col)
        {
            return (CellType)cells[revisedRow * mCol + col];
        }
        return CellType.EMPTY;
    }
}
