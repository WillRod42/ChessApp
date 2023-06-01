using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	public const int BOARD_WIDTH = 8;
	
	[SerializeField] private Transform bottomLeftCellLoc;
	[SerializeField] private Transform cellCenterRuler;
	[SerializeField] private GameObject cellObjPrefab;

	private float cellSize;
	private static Cell[][] cells;
	private static List<Cell> highlightedCells;

	public static string IndiciesToChessCoords(int col, int row)
	{
		return "" + (char)(col + 97) + row;
	}

	private void Awake()
	{
		cells = new Cell[BOARD_WIDTH][];
		highlightedCells = new List<Cell>();

		Vector3 cellPos = bottomLeftCellLoc.position;
		Vector3 cellPos2 = cellCenterRuler.position;
		cellSize = cellPos2.y - cellPos.y;

		for (int i = 0; i < BOARD_WIDTH; i++)
		{
			cells[i] = new Cell[BOARD_WIDTH];
			for (int j = 0; j < BOARD_WIDTH; j++)
			{
				Cell newCell = new Cell(((i + 1) * 10) + (j + 1));
				cells[i][j] = newCell;
				newCell.cellObj = Instantiate(cellObjPrefab, new Vector3(cellPos.x + (cellSize * i), cellPos.y + (cellSize * j), 0), Quaternion.identity);
				newCell.cellObj.AddComponent<CellOnClick>();
				newCell.cellObj.GetComponent<CellOnClick>().Initialize(newCell);
			}
		}
	}

	/// <summary>
	/// Column then row, numbered 1 - 8 
	/// </summary>
	public static Cell GetCell(int location)
	{
		return GetCell(GetCol(location), GetRow(location));
	}

	/// <summary>
	/// Column then row, numbered 1 - 8 
	/// </summary>
	public static Cell GetCell(int col, int row)
	{
		if (col < 1 || row < 1 || col > 8 || row > 8)
		{
			return null;
		}
		return cells[col - 1][row - 1];
	}

	/*	
		[0][1][2]
		[3][X][4]
		[5][6][7]
	*/
	public static List<Cell> GetAdjacentCells(int location)
	{
		int col = GetCol(location);
		int row = GetRow(location);

		List<Cell> adjCells = new List<Cell>();
		adjCells.Add(GetCell(col - 1, row + 1));
		adjCells.Add(GetCell(col, row + 1));
		adjCells.Add(GetCell(col + 1, row + 1));
		adjCells.Add(GetCell(col - 1, row));
		adjCells.Add(GetCell(col + 1, row));
		adjCells.Add(GetCell(col - 1, row - 1));
		adjCells.Add(GetCell(col, row - 1));
		adjCells.Add(GetCell(col + 1, row - 1));

		for (int i = 0; i < adjCells.Count; i++)
		{
			if (adjCells[i] == null)
			{
				adjCells.Remove(adjCells[i]); 
				i--;
			}
		}

		return adjCells;
	}

	public static List<Cell> GetDiagonalCells(int location)
	{
		int col = GetCol(location);
		int row = GetRow(location);
		
		List<Cell> diagCells = new List<Cell>();
		while (col > 1 && row > 1)
		{
			col--;
			row--;
			diagCells.Add(GetCell(col, row));
		}

		col = GetCol(location);
		row = GetRow(location);
		while (col < 8 && row > 1)
		{
			col++;
			row--;
			diagCells.Add(GetCell(col, row));
		}

		col = GetCol(location);
		row = GetRow(location);
		while (col < 8 && row < 8)
		{
			col++;
			row++;
			diagCells.Add(GetCell(col, row));
		}

		col = GetCol(location);
		row = GetRow(location);
		while (col > 1 && row < 8)
		{
			col--;
			row++;
			diagCells.Add(GetCell(col, row));
		}

		return diagCells;
	}

	/// <summary>
	/// Get cells on board in plus '+' shape
	/// </summary>
	public static List<Cell> GetCellsPlus(int location)
	{
		int col = GetCol(location);
		int row = GetRow(location);
		
		List<Cell> plusCells = new List<Cell>();
		while (row > 1)
		{
			row--;
			plusCells.Add(GetCell(col, row));
		}

		col = GetCol(location);
		row = GetRow(location);
		while (col < 8)
		{
			col++;
			plusCells.Add(GetCell(col, row));
		}

		col = GetCol(location);
		row = GetRow(location);
		while (row < 8)
		{
			row++;
			plusCells.Add(GetCell(col, row));
		}

		col = GetCol(location);
		row = GetRow(location);
		while (col > 1)
		{
			col--;
			plusCells.Add(GetCell(col, row));
		}

		return plusCells;
	}

	public static void HighlightCells(List<Cell> cells)
	{
		foreach (Cell cell in cells)
		{
			cell.cellObj.SetActive(true);
			highlightedCells.Add(cell);
		}
	}

	public static void UnhighlightCells()
	{
		foreach (Cell cell in highlightedCells)
		{
			cell.cellObj.SetActive(false);
		}

		highlightedCells.Clear();
	}

	private static int GetCol(int location)
	{
		return location / 10;
	}

	private static int GetRow(int location)
	{
		return location % 10;
	}
}