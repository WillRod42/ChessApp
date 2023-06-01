using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellOnClick : MonoBehaviour
{
	private Cell cell;

	public void Initialize(Cell cell)
	{
		this.cell = cell;
	}

	public void OnMouseDown()
	{
		PieceOnClick.selectedPiece.GetComponent<PieceOnClick>().piece.Move(cell);
		PieceOnClick.selectedPiece = null;
		BoardManager.UnhighlightCells();
	}
}
