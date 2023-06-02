using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
	[SerializeField] private GameObject wPawn, wBishop, wKnight, wRook, wQueen, wKing, bPawn, bBishop, bKnight, bRook, bQueen, bKing;
	[SerializeField] private GameObject board;

	public static bool whitesTurn;
	public static bool playerIsWhitePieces;
	public static Piece enPassant;
	public static Cell enPassantCell;

	private static List<Piece> whitePieces;
	private static List<Piece> blackPieces;

	public enum PieceType
	{
		pawn,
		bishop,
		knight,
		rook,
		queen,
		king
	}

	public void Initialize()
	{
		whitePieces = new List<Piece>();
		blackPieces = new List<Piece>();
		whitesTurn = true;
;		for (int i = 0; i < BoardManager.BOARD_WIDTH; i++)
		{
			if (playerIsWhitePieces)
			{
				whitePieces.Add(new Piece(Instantiate(wPawn), PieceType.pawn, BoardManager.GetCell(1 + i, 2), true));
				blackPieces.Add(new Piece(Instantiate(bPawn), PieceType.pawn, BoardManager.GetCell(1 + i, 7), false));
				
			}
			else
			{
				whitePieces.Add(new Piece(Instantiate(wPawn), PieceType.pawn, BoardManager.GetCell(1 + i, 7), true));
				blackPieces.Add(new Piece(Instantiate(bPawn), PieceType.pawn, BoardManager.GetCell(1 + i, 2), false));
			}

			blackPieces[i].pieceObj.GetComponent<BoxCollider2D>().enabled = false;
		}
		int whitePiecesLocation;
		int blackPiecesLocation;
		if (playerIsWhitePieces)
		{
			whitePiecesLocation = 11;
			blackPiecesLocation = 18;
		}
		else
		{
			whitePiecesLocation = 18;
			blackPiecesLocation = 11;
		}

		whitePieces.Add(new Piece(Instantiate(wRook), PieceType.rook, BoardManager.GetCell(whitePiecesLocation), true));
		whitePieces.Add(new Piece(Instantiate(wKnight), PieceType.knight, BoardManager.GetCell(whitePiecesLocation + 10), true));
		whitePieces.Add(new Piece(Instantiate(wBishop), PieceType.bishop, BoardManager.GetCell(whitePiecesLocation + 20), true));
		whitePieces.Add(new Piece(Instantiate(wQueen), PieceType.queen, BoardManager.GetCell(whitePiecesLocation + 30), true));
		whitePieces.Add(new Piece(Instantiate(wKing), PieceType.king, BoardManager.GetCell(whitePiecesLocation + 40), true));
		whitePieces.Add(new Piece(Instantiate(wBishop), PieceType.bishop, BoardManager.GetCell(whitePiecesLocation + 50), true));
		whitePieces.Add(new Piece(Instantiate(wKnight), PieceType.knight, BoardManager.GetCell(whitePiecesLocation + 60), true));
		whitePieces.Add(new Piece(Instantiate(wRook), PieceType.rook, BoardManager.GetCell(whitePiecesLocation + 70), true));

		blackPieces.Add(new Piece(Instantiate(bRook), PieceType.rook, BoardManager.GetCell(blackPiecesLocation), false));
		blackPieces.Add(new Piece(Instantiate(bKnight), PieceType.knight, BoardManager.GetCell(blackPiecesLocation + 10), false));
		blackPieces.Add(new Piece(Instantiate(bBishop), PieceType.bishop, BoardManager.GetCell(blackPiecesLocation + 20), false));
		blackPieces.Add(new Piece(Instantiate(bQueen), PieceType.queen, BoardManager.GetCell(blackPiecesLocation + 30), false));
		blackPieces.Add(new Piece(Instantiate(bKing), PieceType.king, BoardManager.GetCell(blackPiecesLocation + 40), false));
		blackPieces.Add(new Piece(Instantiate(bBishop), PieceType.bishop, BoardManager.GetCell(blackPiecesLocation + 50), false));
		blackPieces.Add(new Piece(Instantiate(bKnight), PieceType.knight, BoardManager.GetCell(blackPiecesLocation + 60), false));
		blackPieces.Add(new Piece(Instantiate(bRook), PieceType.rook, BoardManager.GetCell(blackPiecesLocation + 70), false));

		if (!playerIsWhitePieces)
		{
			Piece temp = blackPieces[11];
			blackPieces[11] = blackPieces[12];
			blackPieces[12] = temp;

			Piece temp2 = whitePieces[11];
			whitePieces[11] = whitePieces[12];
			whitePieces[12] = temp;
		}

		foreach(Piece piece in whitePieces)
		{
			piece.pieceObj.AddComponent<PieceOnClick>();
			piece.pieceObj.GetComponent<PieceOnClick>().Initialize(piece);
		}
		foreach(Piece piece in blackPieces)
		{
			piece.pieceObj.AddComponent<PieceOnClick>();
			piece.pieceObj.GetComponent<PieceOnClick>().Initialize(piece);
		}
	}

	public List<Piece> GetWhitePieces()
	{
		return whitePieces;
	}

	public List<Piece> GetBlackPieces()
	{
		return blackPieces;
	}

	public static Piece GetPiece(string name)
	{
		return null;
	}

	public static void CapturePiece(Piece piece)
	{
		piece.pieceObj.SetActive(false);
		Destroy(piece.pieceObj);
		if (piece.isWhitePiece)
		{
			whitePieces.Remove(piece);
		}
		else
		{
			blackPieces.Remove(piece);
		}

		piece.cell.piece = null;
		piece.cell = null;
	}

	public static void TogglePieces()
	{
		if (whitesTurn)
		{
			foreach (Piece piece in whitePieces)
			{
				piece.pieceObj.GetComponent<BoxCollider2D>().enabled = false;
			}
			foreach (Piece piece in blackPieces)
			{
				piece.pieceObj.GetComponent<BoxCollider2D>().enabled = true;
			}
		}
		else
		{
			foreach (Piece piece in whitePieces)
			{
				piece.pieceObj.GetComponent<BoxCollider2D>().enabled = true;
			}
			foreach (Piece piece in blackPieces)
			{
				piece.pieceObj.GetComponent<BoxCollider2D>().enabled = false;
			}
		}
		whitesTurn = !whitesTurn;
	}

	public static List<Cell> GetValidMoves(Piece piece)
	{
		List<Cell> moves = new List<Cell>();
		switch (piece.pieceType)
		{
			case PieceType.pawn: moves = PawnMoves(piece.cell.location, piece.isWhitePiece, piece.hasMoved); break;
			case PieceType.knight: moves = KnightMoves(piece.cell.location, piece.isWhitePiece); break;
			case PieceType.bishop: moves = BoardManager.GetDiagonalCells(piece.cell.location); break;
			case PieceType.rook: moves = BoardManager.GetCellsPlus(piece.cell.location); break;
			case PieceType.queen: moves = QueenMoves(piece.cell.location); break;
			default: moves = BoardManager.GetAdjacentCells(piece.cell.location); break;
		}

		return RemoveBlockedCells(RemoveNullFromMoveList(moves), piece.isWhitePiece);
	}

	// Add in checks for moves putting king in check
	private static List<Cell> PawnMoves(int location, bool isWhitePiece, bool hasMoved)
	{
		List<Cell> moves = new List<Cell>();
		int direction = (isWhitePiece ? 1 : -1) * (playerIsWhitePieces ? 1 : -1);
		Cell moveOne = BoardManager.GetCell(location + direction);
		if (moveOne.piece == null)
		{
			moves.Add(moveOne);
			if (!hasMoved)
			{
				Cell moveTwo = BoardManager.GetCell(location + (direction * 2));
				if (moveTwo.piece == null)
					moves.Add(moveTwo);
			}
		}

		Cell captureLeft = BoardManager.GetCell(location - 10 + direction);
		Cell captureRight = BoardManager.GetCell(location + 10 + direction);
		if (captureLeft != null && captureLeft.piece != null && captureLeft.piece.isWhitePiece != isWhitePiece)
		{
			moves.Add(captureLeft);
		}
		else if (enPassant != null && enPassant.cell == BoardManager.GetCell(location - 10))
		{
			moves.Add(captureLeft);
		}

		if (captureRight != null && captureRight.piece != null && captureRight.piece.isWhitePiece != isWhitePiece)
		{
			moves.Add(captureRight);
		}
		else if (enPassant != null && enPassant.cell == BoardManager.GetCell(location + 10))
		{
			moves.Add(captureRight);
		}		
		return moves;
	}

	private static List<Cell> KnightMoves(int location, bool isWhitePiece)
	{
		List<Cell> moves = new List<Cell>();
		moves.Add(BoardManager.GetCell(location - 8));
		moves.Add(BoardManager.GetCell(location + 12));
		moves.Add(BoardManager.GetCell(location + 21));
		moves.Add(BoardManager.GetCell(location + 19));
		moves.Add(BoardManager.GetCell(location + 8));
		moves.Add(BoardManager.GetCell(location - 12));
		moves.Add(BoardManager.GetCell(location - 21));
		moves.Add(BoardManager.GetCell(location - 19));

		return moves;
	}

	private static List<Cell> QueenMoves(int location)
	{
		List<Cell> moves = BoardManager.GetDiagonalCells(location);
		moves.AddRange(BoardManager.GetCellsPlus(location));
		return moves;
	}

	/// <summary>
	/// Returns false if cell is occupied by friendly piece
	/// </summary>
	private static List<Cell> RemoveBlockedCells(List<Cell> moves, bool isWhitePiece)
	{
		List<Cell> filteredMoves = new List<Cell>();
		foreach (Cell cell in moves)
		{
			if (cell.piece == null || cell.piece.isWhitePiece != isWhitePiece)
			{
				filteredMoves.Add(cell);
			}
		}

		return filteredMoves;
	}

	private static List<Cell> RemoveNullFromMoveList(List<Cell> moves)
	{
		List<Cell> filteredMoves = new List<Cell>();
		foreach (Cell cell in moves)
		{
			if (cell != null)
			{
				filteredMoves.Add(cell);
			}
		}

		return filteredMoves;
	}
}
