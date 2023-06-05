using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece
{
	public GameObject pieceObj;
	public PieceManager.PieceType pieceType;
	public Cell cell;
	public bool isWhitePiece;
	public bool hasMoved;

	public Piece(GameObject pieceObj, PieceManager.PieceType pieceType, Cell cell, bool isWhitePiece)
	{
		this.pieceObj = pieceObj;
		this.pieceType = pieceType;
		this.cell = cell;
		cell.piece = this;
		this.isWhitePiece = isWhitePiece;
		hasMoved = false;

		CenterOnCell();
	}

	public void Move(Cell toMove)
	{
		if (toMove.piece != null)
		{
			PieceManager.CapturePiece(this, toMove.piece);
		}
		else if (toMove == PieceManager.enPassantCell && pieceType == PieceManager.PieceType.pawn)
		{
			PieceManager.CapturePiece(this, PieceManager.enPassant);
		}

		cell.piece = null;
		cell = toMove;
		toMove.piece = this;

		CenterOnCell();
		PieceManager.TogglePieces();
		hasMoved = true;
	}

	private void CenterOnCell()
	{
		pieceObj.transform.position = cell.cellObj.transform.position;
	}
}
