using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessGame
{
    public partial class Form1 : Form
    {

        // Picture file constants

        Bitmap whiteBishopWhite = new Bitmap(ChessGame.Properties.Resources.White_bishop_white);
        Bitmap whiteKingWhite = new Bitmap(ChessGame.Properties.Resources.White_king_white);
        Bitmap whiteKnightWhite = new Bitmap(ChessGame.Properties.Resources.White_knight_white);
        Bitmap whitePawnWhite = new Bitmap(ChessGame.Properties.Resources.White_pawn_white);
        Bitmap whiteQueenWhite = new Bitmap(ChessGame.Properties.Resources.White_queen_white);
        Bitmap whiteRookWhite = new Bitmap(ChessGame.Properties.Resources.White_rook_white);

        Bitmap whiteBishopBlack = new Bitmap(ChessGame.Properties.Resources.White_bishop_black);
        Bitmap whiteKingBlack = new Bitmap(ChessGame.Properties.Resources.White_king_black);
        Bitmap whiteKnightBlack = new Bitmap(ChessGame.Properties.Resources.White_knight_black);
        Bitmap whitePawnBlack = new Bitmap(ChessGame.Properties.Resources.White_pawn_black);
        Bitmap whiteQueenBlack = new Bitmap(ChessGame.Properties.Resources.White_queen_black);
        Bitmap whiteRookBlack = new Bitmap(ChessGame.Properties.Resources.White_rook_black);

        Bitmap blackBishopWhite = new Bitmap(ChessGame.Properties.Resources.Black_bishop_white);
        Bitmap blackKingWhite = new Bitmap(ChessGame.Properties.Resources.Black_king_white);
        Bitmap blackKnightWhite = new Bitmap(ChessGame.Properties.Resources.Black_knight_white);
        Bitmap blackPawnWhite = new Bitmap(ChessGame.Properties.Resources.Black_pawn_white);
        Bitmap blackQueenWhite = new Bitmap(ChessGame.Properties.Resources.Black_queen_white);
        Bitmap blackRookWhite = new Bitmap(ChessGame.Properties.Resources.Black_rook_white);

        Bitmap blackBishopBlack = new Bitmap(ChessGame.Properties.Resources.Black_bishop_black);
        Bitmap blackKingBlack = new Bitmap(ChessGame.Properties.Resources.Black_king_black);
        Bitmap blackKnightBlack = new Bitmap(ChessGame.Properties.Resources.Black_knight_black);
        Bitmap blackPawnBlack = new Bitmap(ChessGame.Properties.Resources.Black_pawn_black);
        Bitmap blackQueenBlack = new Bitmap(ChessGame.Properties.Resources.Black_queen_black);
        Bitmap blackRookBlack = new Bitmap(ChessGame.Properties.Resources.Black_rook_black);

        Bitmap emptyWhite = new Bitmap(ChessGame.Properties.Resources.White_piece);
        Bitmap emptyBlack = new Bitmap(ChessGame.Properties.Resources.Black_piece);


        // Arrays

        private PictureBox[,] fieldPic = new PictureBox[8, 8];
        private byte[,] fieldByte = new byte[8, 8];
        private Point[,] figureCoord = new Point[8, 8];


        // Other variables

        // 0 – the game hasn't yet started; 1 – White to move; 2 – Black to move
        private int moveOrder = 0;
        // 0 – no En Passant, 1 – En Passant for White, 2 – En Passant for Black
        private int enPassant = 0;
        private Point enPassantCoord;

        public Form1()
        {
            InitializeComponent();
        }

        private void alignFigure(PictureBox fieldPic)
        {
            fieldPic.SizeMode = PictureBoxSizeMode.StretchImage;
            fieldPic.BringToFront();
        }

        private void setFigure(PictureBox fieldPic, Bitmap picName)
        {
            fieldPic.Image = picName;
            fieldPic.SizeMode = PictureBoxSizeMode.StretchImage;
            fieldPic.BringToFront();
        }

        private void setFigurePic(PictureBox fieldPic, Bitmap picName)
        {
            fieldPic.Image = picName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // Filling the coord array

            int boardX = pictureBoxBoard.Location.X + 14;               // X coordinate of the board plus the border width
            int boardY = pictureBoxBoard.Location.Y + 14;               // Y coordinate of the board plus the border width
            int figureSize = 48;                                        // Figure picture size constant
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    figureCoord[i, j] = new Point(boardX + figureSize * j, boardY + figureSize * i);


            // Filling the PictureBox array

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    PictureBox pic = new PictureBox();
                    pic.Size = new Size(figureSize, figureSize);
                    pic.Location = figureCoord[i, j];
                    this.Controls.Add(pic);
                    fieldPic[i, j] = pic;
                    alignFigure(pic);
                    if ((i+j) % 2 == 0) setFigurePic(fieldPic[i, j], emptyWhite);
                    else setFigurePic(fieldPic[i, j], emptyBlack);
                }

        }

        private void buttonStartGame_Click(object sender, EventArgs e)
        {

            // Filling the figure byte array
            // 11 – white pawn, 12 – white rook, 13 – white knight, 14 – white bishop, 15 – white queen, 16 – white king
            // 21 – black pawn, 22 – black rook, 23 – black knight, 24 – black bishop, 25 – black queen, 26 – black king
            // 0 – empty

            for (int i = 1; i < 7; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (i == 1) fieldByte[i, j] = 21;
                    else if (i == 6) fieldByte[i, j] = 11;
                    else fieldByte[i, j] = 0;
                }
            fieldByte[0, 0] = 22; fieldByte[0, 7] = 22;
            fieldByte[0, 1] = 23; fieldByte[0, 6] = 23;
            fieldByte[0, 2] = 24; fieldByte[0, 5] = 24;
            fieldByte[0, 3] = 25;
            fieldByte[0, 4] = 26;
            fieldByte[7, 0] = 12; fieldByte[7, 7] = 12;
            fieldByte[7, 1] = 13; fieldByte[7, 6] = 13;
            fieldByte[7, 2] = 14; fieldByte[7, 5] = 14;
            fieldByte[7, 3] = 15;
            fieldByte[7, 4] = 16;


            // Loading figures into pics

            setFigurePic(fieldPic[0, 0], blackRookWhite);
            setFigurePic(fieldPic[0, 1], blackKnightBlack);
            setFigurePic(fieldPic[0, 2], blackBishopWhite);
            setFigurePic(fieldPic[0, 3], blackQueenBlack);
            setFigurePic(fieldPic[0, 4], blackKingWhite);
            setFigurePic(fieldPic[0, 5], blackBishopBlack);
            setFigurePic(fieldPic[0, 6], blackKnightWhite);
            setFigurePic(fieldPic[0, 7], blackRookBlack);
            for (int i = 0; i < 8; i++)
            {
                if (i % 2 == 0)
                {
                    setFigurePic(fieldPic[1, i], blackPawnBlack);
                    setFigurePic(fieldPic[6, i], whitePawnWhite);
                }
                else
                {
                    setFigurePic(fieldPic[1, i], blackPawnWhite);
                    setFigurePic(fieldPic[6, i], whitePawnBlack);
                }
            }
            setFigurePic(fieldPic[7, 0], whiteRookBlack);
            setFigurePic(fieldPic[7, 1], whiteKnightWhite);
            setFigurePic(fieldPic[7, 2], whiteBishopBlack);
            setFigurePic(fieldPic[7, 3], whiteQueenWhite);
            setFigurePic(fieldPic[7, 4], whiteKingBlack);
            setFigurePic(fieldPic[7, 5], whiteBishopWhite);
            setFigurePic(fieldPic[7, 6], whiteKnightBlack);
            setFigurePic(fieldPic[7, 7], whiteRookWhite);


            // Enabling the game controls

            moveOrder = 1;
            labelTurn.Text = "Хід білих";
            label1.Enabled = true;
            textBox1.Enabled = true;
            label2.Enabled = true;
            textBox2.Enabled = true;
            buttonMove.Enabled = true;

        }

        private void cleanField(Point coords)
        {
            if ((coords.X + coords.Y) % 2 == 0) fieldPic[coords.X, coords.Y].Image = emptyWhite;
            else fieldPic[coords.X, coords.Y].Image = emptyBlack;
        }

        private void takeField(Point coords, int figureCode)
        {
            if ((coords.X + coords.Y) % 2 == 0)
            {
                switch (figureCode)
                {
                    case 11:
                        setFigurePic(fieldPic[coords.X, coords.Y], whitePawnWhite);
                        break;
                    case 12:
                        setFigurePic(fieldPic[coords.X, coords.Y], whiteRookWhite);
                        break;
                    case 13:
                        setFigurePic(fieldPic[coords.X, coords.Y], whiteKnightWhite);
                        break;
                    case 14:
                        setFigurePic(fieldPic[coords.X, coords.Y], whiteBishopWhite);
                        break;
                    case 15:
                        setFigurePic(fieldPic[coords.X, coords.Y], whiteQueenWhite);
                        break;
                    case 16:
                        setFigurePic(fieldPic[coords.X, coords.Y], whiteKingWhite);
                        break;
                    case 21:
                        setFigurePic(fieldPic[coords.X, coords.Y], blackPawnWhite);
                        break;
                    case 22:
                        setFigurePic(fieldPic[coords.X, coords.Y], blackRookWhite);
                        break;
                    case 23:
                        setFigurePic(fieldPic[coords.X, coords.Y], blackKnightWhite);
                        break;
                    case 24:
                        setFigurePic(fieldPic[coords.X, coords.Y], blackBishopWhite);
                        break;
                    case 25:
                        setFigurePic(fieldPic[coords.X, coords.Y], blackQueenWhite);
                        break;
                    case 26:
                        setFigurePic(fieldPic[coords.X, coords.Y], blackKingWhite);
                        break;
                }
            }
            else
            {
                switch (figureCode)
                {
                    case 11:
                        setFigurePic(fieldPic[coords.X, coords.Y], whitePawnBlack);
                        break;
                    case 12:
                        setFigurePic(fieldPic[coords.X, coords.Y], whiteRookBlack);
                        break;
                    case 13:
                        setFigurePic(fieldPic[coords.X, coords.Y], whiteKnightBlack);
                        break;
                    case 14:
                        setFigurePic(fieldPic[coords.X, coords.Y], whiteBishopBlack);
                        break;
                    case 15:
                        setFigurePic(fieldPic[coords.X, coords.Y], whiteQueenBlack);
                        break;
                    case 16:
                        setFigurePic(fieldPic[coords.X, coords.Y], whiteKingBlack);
                        break;
                    case 21:
                        setFigurePic(fieldPic[coords.X, coords.Y], blackPawnBlack);
                        break;
                    case 22:
                        setFigurePic(fieldPic[coords.X, coords.Y], blackRookBlack);
                        break;
                    case 23:
                        setFigurePic(fieldPic[coords.X, coords.Y], blackKnightBlack);
                        break;
                    case 24:
                        setFigurePic(fieldPic[coords.X, coords.Y], blackBishopBlack);
                        break;
                    case 25:
                        setFigurePic(fieldPic[coords.X, coords.Y], blackQueenBlack);
                        break;
                    case 26:
                        setFigurePic(fieldPic[coords.X, coords.Y], blackKingBlack);
                        break;
                }
            }
        }

        private string checkWhiteCheck(byte[,] board, bool white)
        {

            // Check check

            bool isCheck;


            // Getting king's coord

            Point kingCoord;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if ((white && board[i, j] == 16) || (!white && board[i, j] == 26))
                        kingCoord = new Point(i, j);


            // Bishop check
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if ((kingCoord.X + kingCoord.Y) % 2 == 0                                //king is on the white field
                        && (i + j) % 2 == 0                                                 //current position is white
                        && white                                                            //king is white
                        && board[i, j] == 24                                                //figure on the current position is black bishop
                        && Math.Abs(i - kingCoord.X) == Math.Abs(j - kingCoord.Y))          //the king and the bishop are on the same diagonal
                        

        }

        private void buttonMove_Click(object sender, EventArgs e)
        {

            // Reading input coordinates

            string from = textBox1.Text;
            string to = textBox2.Text;
            Point fromCoords = MoveCodeParser.getDigitCoords(from);
            Point toCoords = MoveCodeParser.getDigitCoords(to);


            // Check if the "from" field is empty

            if (fieldByte[fromCoords.X, fromCoords.Y] == 0)
            {
                MessageBox.Show("Обране поле не має фігури.");
                return;
            }


            // Check if the "from" and the "to" coords match

            if (fromCoords.X == toCoords.X && fromCoords.Y == toCoords.Y)
            {
                MessageBox.Show("Початкові й кінцеві координати співпадають.");
                return;
            }


            // Getting the figure code

            byte figureCode = fieldByte[fromCoords.X, fromCoords.Y];
            byte figureKindCode = (byte)((int)figureCode % 10);


            // Case scenarios

            switch (figureKindCode)
            {
                // Pawn cases
                case 1:
                    if (Math.Abs(fromCoords.Y - toCoords.Y) > 1)
                    {
                        MessageBox.Show("Недопустимий хід пішаком (різниця по-горизонталі)");
                        return;
                    }
                    else
                    {
                        int figureColorCode = (int)figureCode / 10;
                        if (figureKindCode == 1)
                        {
                            if (figureColorCode == 1)
                            {
                                // Check check
                                if (checkCheck)
                                {

                                }

                                // Check check successful
                                if (fromCoords.Y == toCoords.Y)
                                {
                                    if (fromCoords.X < toCoords.X)
                                    {
                                        MessageBox.Show("Недопустимий хід пішаком (не можна ходити назад).");
                                        return;
                                    }
                                    else if (fromCoords.X - toCoords.X > 2)
                                    {
                                        MessageBox.Show("Недопустимий хід пішаком (занадто далекий хід вперед).");
                                        return;
                                    }
                                    else if (fromCoords.X - toCoords.X == 2)
                                    {
                                        if (fromCoords.X != 6)
                                        {
                                            MessageBox.Show("Недопустимий хід пішаком (хід на дві клітинки можливий лише зі стартової позиції).");
                                            return;
                                        }
                                        else if (fieldByte[fromCoords.X - 1, fromCoords.Y] != 0
                                            || fieldByte[toCoords.X, toCoords.Y] != 0)
                                        {
                                            MessageBox.Show("Недопустимий хід пішаком (шлях перекритий).");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        if (fieldByte[toCoords.X, toCoords.Y] != 0)
                                        {
                                            MessageBox.Show("Недопустимий хід пішаком (шлях перекритий).");
                                            return;
                                        }
                                    }
                                }
                                else if (fromCoords.X - toCoords.X == 1 && Math.Abs(fromCoords.Y - toCoords.Y) == 1)
                                {
                                    if (fieldByte[toCoords.X, toCoords.Y] == 0 || fieldByte[toCoords.X, toCoords.Y] == 26
                                        || fieldByte[toCoords.X, toCoords.Y] == 11 || fieldByte[toCoords.X, toCoords.Y] == 12
                                        || fieldByte[toCoords.X, toCoords.Y] == 13 || fieldByte[toCoords.X, toCoords.Y] == 14
                                        || fieldByte[toCoords.X, toCoords.Y] == 15 || fieldByte[toCoords.X, toCoords.Y] == 16)
                                    {
                                        MessageBox.Show("Недопустимий хід пішаком (фігура під боєм є королем, союзником або відсутня).");
                                        return;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Недопустимий хід пішаком (неправильна траєкторія).");
                                    return;
                                }
                            }
                            else
                            {
                                if (fromCoords.Y == toCoords.Y)
                                {
                                    if (fromCoords.X > toCoords.X)
                                    {
                                        MessageBox.Show("Недопустимий хід пішаком (не можна ходити назад).");
                                        return;
                                    }
                                    else if (toCoords.X - fromCoords.X > 2)
                                    {
                                        MessageBox.Show("Недопустимий хід пішаком (занадто далекий хід вперед).");
                                        return;
                                    }
                                    else if (toCoords.X - fromCoords.X == 2)
                                    {
                                        if (fromCoords.X != 1)
                                        {
                                            MessageBox.Show("Недопустимий хід пішаком (хід на дві клітинки можливий лише зі стартової позиції).");
                                            return;
                                        }
                                        else if (fieldByte[fromCoords.X + 1, fromCoords.Y] != 0
                                            || fieldByte[toCoords.X, toCoords.Y] != 0)
                                        {
                                            MessageBox.Show("Недопустимий хід пішаком (шлях перекритий).");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        if (fieldByte[toCoords.X, toCoords.Y] != 0)
                                        {
                                            MessageBox.Show("Недопустимий хід пішаком (шлях перекритий).");
                                            return;
                                        }
                                    }
                                }
                                else if (fromCoords.X - toCoords.X == 1 && Math.Abs(fromCoords.Y - toCoords.Y) == 1)
                                {
                                    if (fieldByte[toCoords.X, toCoords.Y] == 0 || fieldByte[toCoords.X, toCoords.Y] == 26
                                        || fieldByte[toCoords.X, toCoords.Y] == 11 || fieldByte[toCoords.X, toCoords.Y] == 12
                                        || fieldByte[toCoords.X, toCoords.Y] == 13 || fieldByte[toCoords.X, toCoords.Y] == 14
                                        || fieldByte[toCoords.X, toCoords.Y] == 15 || fieldByte[toCoords.X, toCoords.Y] == 16)
                                    {
                                        MessageBox.Show("Недопустимий хід пішаком (фігура під боєм є королем, союзником або відсутня).");
                                        return;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Недопустимий хід пішаком (неправильна траєкторія).");
                                    return;
                                }
                            }
                        }
                    }

                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
            }


            // Moving the figure in the byte matrix

            fieldByte[fromCoords.X, fromCoords.Y] = 0;
            fieldByte[toCoords.X, toCoords.Y] = figureCode;


            // Moving the figure visually

            cleanField(fromCoords);
            takeField(toCoords, figureCode);

            
            // Change order
            
            if (moveOrder == 1)
            {
                moveOrder = 2;
                labelTurn.Text = "Хід чорних";
            }
            else
            {
                moveOrder = 1;
                labelTurn.Text = "Хід білих";
            }
        }
    }
}
