using System;
using chess_console;
using board;
using chess;

Board board = new Board(8, 8);

board.InsertPiece(new Tower(Color.White, board), new Position(0, 0));
board.InsertPiece(new Tower(Color.White, board), new Position(1,3));
board.InsertPiece(new King(Color.White, board), new Position(2,4));

Screen.WriteBoard(board);

Console.ReadLine();

