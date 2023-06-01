using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
	[SerializeField] private GameObject wPawn, wBishop, wKnight, wRook, wQueen, wKing, bPawn, bBishop, bKnight, bRook, bQueen, bKing;
	[SerializeField] private GameObject board;

	private List<Piece> whitePieces;
	private List<Piece> blackPieces;

	public enum PieceType
	{
		pawn,
		bishop,
		knight,
		rook,
		queen,
		king
	}

	public void Initialize(bool playerIsWhitePieces)
	{
		whitePieces = new List<Piece>();
		blackPieces = new List<Piece>();
		for (int i = 0; i < BoardManager.BOARD_WIDTH; i++)
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

	public static List<Cell> GetValidMoves(Piece piece)
	{
		switch (piece.pieceType)
		{
			case PieceType.pawn: return PawnMoves(piece.cell.location, piece.isWhitePiece);
			case PieceType.knight: return KnightMoves(piece.cell.location);
			case PieceType.bishop: return BishopMoves(piece.cell.location);
			case PieceType.rook: return RookMoves(piece.cell.location);
			case PieceType.queen: return QueenMoves(piece.cell.location);
			default: return KingMoves(piece.cell.location);
		}
	}

	// Add in checks for moves putting king in check
	// Remove moves blocked by other pieces
	private static List<Cell> PawnMoves(int location, bool isWhitePiece)
	{
		//Add diagonal capture
		List<Cell> moves = new List<Cell>();
		moves.Add(BoardManager.GetCell(location + (isWhitePiece ? 1 : -1)));
		return moves;
	}

	private static List<Cell> KnightMoves(int location)
	{
		return null;
	}

	private static List<Cell> BishopMoves(int location)
	{
		return BoardManager.GetDiagonalCells(location);
	}

	private static List<Cell> RookMoves(int location)
	{
		return BoardManager.GetCellsPlus(location);
	}

	private static List<Cell> QueenMoves(int location)
	{
		List<Cell> moves = BoardManager.GetDiagonalCells(location);
		moves.AddRange(BoardManager.GetCellsPlus(location));
		return moves;
	}

	private static List<Cell> KingMoves(int location)
	{
		return BoardManager.GetAdjacentCells(location);
	}
}
