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
		Piece piece = PieceOnClick.selectedPiece.GetComponent<PieceOnClick>().piece;
		Debug.Log("calc: " + Mathf.Abs(Mathf.Abs(piece.cell.location) - Mathf.Abs(cell.location)));
		if (piece.pieceType == PieceManager.PieceType.pawn && Mathf.Abs(Mathf.Abs(piece.cell.location) - Mathf.Abs(cell.location)) == 2)
		{
			Debug.Log("Set en passant: " + piece.cell.location);
			PieceManager.enPassant = piece;
			int direction = (piece.isWhitePiece ? 1 : -1) * (PieceManager.playerIsWhitePieces ? 1 : -1);
			PieceManager.enPassantCell = BoardManager.GetCell(piece.cell.location + direction);
		}
		else if (cell != PieceManager.enPassantCell)
		{
			PieceManager.enPassant = null;
		}

		piece.Move(cell);
		PieceOnClick.selectedPiece = null;
		BoardManager.UnhighlightCells();
	}
}
