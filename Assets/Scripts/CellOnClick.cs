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
		if (piece.pieceType == PieceManager.PieceType.pawn && Mathf.Abs(Mathf.Abs(piece.cell.location) - Mathf.Abs(cell.location)) == 2)
		{
			PieceManager.enPassant = piece;
			int direction = (piece.isWhitePiece ? 1 : -1) * (PieceManager.playerIsWhitePieces ? 1 : -1);
			PieceManager.enPassantCell = BoardManager.GetCell(piece.cell.location + direction);
		}
		else if (cell != PieceManager.enPassantCell)
		{
			PieceManager.enPassant = null;
		}

		if (PieceManager.castleCells.Count > 0)
		{
			int moveVector = piece.cell.location - cell.location;
			if (BoardManager.GetRow(piece.cell.location) == BoardManager.GetRow(cell.location) && Mathf.Abs(moveVector) > 10)
			{
				Piece castleRook = PieceManager.castleRooks[0];
				Cell castleCell = PieceManager.castleCells[0];
				if (PieceManager.castleCells.Count > 1 && (Mathf.Abs(castleCell.location - cell.location) != 10))
				{
					castleRook = PieceManager.castleRooks[1];
					castleCell = PieceManager.castleCells[1];
				}
				
				castleRook.Move(castleCell);
				PieceManager.TogglePieces();
			}
		}

		piece.Move(cell);
		PieceOnClick.selectedPiece = null;
		BoardManager.UnhighlightCells();
	}
}
