using UnityEngine;
public static class CellFactory
{
    public static Cell SpawnCell(StageInfo info, int row, int col)
    {

        return SpawnCell(info.GetCellType(row, col));
    }
    public static Cell SpawnCell(CellType type)
    {
        return new Cell(type);
    }
}