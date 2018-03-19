using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace gabi.checkers
{
    public class Board
    {

        private const int Size = 8;
        private const ulong ValueToShift = 1;


        private Square[,] _squares = new Square[Size, Size];

        public Board()
        {
        }

        public void Print()
        {
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    if (_squares[i, j].Value != null && _squares[i, j].Value.IsChecker)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                    }

                    Console.Write(_squares[i, j]);
                    Console.ResetColor();
                }

                Console.WriteLine("");
            }
            Console.WriteLine("");
        }

        private void ClearBoard()
        {
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    _squares[i, j] = new Square(i, j, (i + j) % 2 == 1 ? Square.SquareColor.White : Square.SquareColor.Black);
                }
            }
        }

        public void NewGame()
        {
            ClearBoard();
            PopulateWhitePieces();
            PopulateBlackPieces();
        }

        private void PopulateBlackPieces()
        {
            var pieces = 12;

            for (var i = 5; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    if (_squares[i, j].SetValue(PieceColor.Black))
                    {
                        if (--pieces == 0)
                        {
                            return;
                        }
                    }
                }
            }
        }

        private void PopulateWhitePieces()
        {
            int pieces = 12;

            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    if (_squares[i, j].SetValue(PieceColor.White))
                    {
                        if (--pieces == 0)
                        {
                            return;
                        }
                    }
                }
            }
        }

        public bool Move(Square square, int newX, int newY)
        {
            if (!IsValidPosition(newX, newY))
            {
                return false;
            }


            if (!IsEmpty(newX, newY))
            {
                return false;
            }

            _squares[newX, newY].SetValue(square.Value);
            square.SetValue(null);

            CheckerValidation(_squares[newX, newY]);

            return true;
        }

        private bool IsValidPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Size && y < Size;
        }

        public Tuple<Square, Square, int, int> Obrigatory(PieceColor pieceColor)
        {
            var rows = GetPieces(pieceColor);
            var verticalDirection = rows.First().GetVerticalDirection();

            foreach (var row in rows)
            {
                var i = row.X;
                var j = row.Y;

                List<Tuple<int, int>> targets = new List<Tuple<int, int>>(2);
                List<Tuple<int, int>> destinations = new List<Tuple<int, int>>(2);

                if (_squares[i, j].Value.IsChecker)
                {
                    targets.Add(new Tuple<int, int>(i + 1, j + 1));
                    targets.Add(new Tuple<int, int>(i + 1, j - 1));
                    targets.Add(new Tuple<int, int>(i - 1, j + 1));
                    targets.Add(new Tuple<int, int>(i - 1, j - 1));


                    destinations.Add(new Tuple<int, int>(i + 2, j + 2));
                    destinations.Add(new Tuple<int, int>(i + 2, j - 2));
                    destinations.Add(new Tuple<int, int>(i - 2, j + 2));
                    destinations.Add(new Tuple<int, int>(i - 2, j - 2));
                }
                else
                {
                    targets.Add(new Tuple<int, int>(i + verticalDirection, j + 1));
                    targets.Add(new Tuple<int, int>(i + verticalDirection, j - 1));

                    destinations.Add(new Tuple<int, int>(i + verticalDirection + verticalDirection, j + 2));
                    destinations.Add(new Tuple<int, int>(i + verticalDirection + verticalDirection, j - 2));
                }

                for (var k = 0; k < targets.Count; k++)
                {
                    if (IsValidPosition(targets[k].Item1, targets[k].Item2) &&
                        IsValidPosition(destinations[k].Item1, destinations[k].Item2) &&
                        !SameColor(pieceColor, targets[k].Item1, targets[k].Item2) &&
                        !IsEmpty(targets[k].Item1, targets[k].Item2) &&
                        IsEmpty(destinations[k].Item1, destinations[k].Item2))
                    {
                        return new Tuple<Square, Square, int, int>(
                            _squares[i, j],
                            _squares[targets[k].Item1, targets[k].Item2],
                            destinations[k].Item1,
                            destinations[k].Item2
                        );
                    }
                }
            }

            return null;
        }

        private bool IsEmpty(int x, int y)
        {
            return _squares[x, y].Value == null;
        }

        private bool SameColor(PieceColor pieceColor, int x, int y)
        {
            return _squares[x, y].Value != null && _squares[x, y].Value.Color == pieceColor;
        }

        public Square[] GetPieces(PieceColor pieceColor)
        {
            List<Square> rows = new List<Square>(12);

            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    if (_squares[i, j].Value != null && _squares[i, j].Value.Color == pieceColor)
                    {
                        rows.Add(_squares[i, j]);
                    }
                }
            }

            return rows.ToArray();
        }

        public void Capture(Square origin, Square target, int newX, int newY)
        {
            var playerPiece = origin.Value;
            target.SetValue(null);
            origin.SetValue(null);

            _squares[newX, newY].SetValue(playerPiece);
            CheckerValidation(_squares[newX, newY]);
        }

        public void CheckerValidation(Square square)
        {
            if (square.Value == null) return;

            var pieceColor = square.Value.Color;

            if ((pieceColor == PieceColor.Black && square.X == 0) ||
                (pieceColor == PieceColor.White && square.X == 7))
            {
                square.Value.IsChecker = true;
            }
        }

        public BoardId GetState()
        {
            BoardId id = new BoardId();
            int count = 0;

            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    if (_squares[i, j].Value != null)
                    {
                        ulong shifted = ValueToShift << count;

                        if (_squares[i, j].Value.Color == PieceColor.Black)
                        {
                            if (!_squares[i, j].Value.IsChecker)
                            {
                                id.Black |= shifted;
                            }
                            else
                            {
                                id.BlackCheckers |= shifted;
                            }
                        }
                        else if (_squares[i, j].Value.Color == PieceColor.White)
                        {
                            if (!_squares[i, j].Value.IsChecker)
                            {
                                id.White |= shifted;
                            }
                            else
                            {
                                id.WhiteCheckers |= shifted;
                            }
                        }
                    }

                    count++;
                }
            }

            return id;
        }

        public void SetState(BoardId id)
        {
            ClearBoard();
            int count = 0;

            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    ulong shifted = ValueToShift << count;

                    if ((id.White & shifted) != 0)
                    {
                        _squares[i, j].SetValue(PieceColor.White);
                    }
                    else if ((id.WhiteCheckers & shifted) != 0)
                    {
                        _squares[i, j].SetValue(new Piece(PieceColor.White)
                        {
                            IsChecker = true
                        });
                    }
                    else if ((id.Black & shifted) != 0)
                    {
                        _squares[i, j].SetValue(PieceColor.Black);
                    }
                    else if ((id.BlackCheckers & shifted) != 0)
                    {
                        _squares[i, j].SetValue(new Piece(PieceColor.Black)
                        {
                            IsChecker = true
                        });
                    }

                    count++;
                }
            }
        }
    }
}