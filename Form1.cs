using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Diagnostics;

namespace SWT_Project_2
{
    public partial class Form1 : Form
    {
        private string player1Name = "Red";
        private string player2Name = "Blue";
        private const int PiecesAtStart = 9;
        private const int boardAtStart = 0;
        private char turn;
        private List<PictureBox> allPictureBoxes;
        private List<List<int>> mills;
        private PictureBox selectedPictureBox;
        private bool capture;
        private int redPieces;
        private int bluePieces;
        private int redPiecesOnBoard;
        private int bluePiecesOnBoard;

        private List<List<int>> adjacencyList = new List<List<int>>()
        {
        new List<int> {1, 9},               // position{0}
        new List<int> {0, 2, 4},            // position{1}
        new List<int> {1, 14},              // position{2}
        new List<int> {4, 10},              // position{3}
        new List<int> {1, 3, 5, 7},         // position{4}
        new List<int> {4, 13},              // position{5}
        new List<int> {7, 11},              // position{6}
        new List<int> {4, 6, 8},            // position{7}
        new List<int> {7, 12},              // position{8}
        new List<int> {0, 10, 21},          // position{9}
        new List<int> {3, 9, 11, 18},       // position{10}
        new List<int> {6, 10, 15},          // position{11}
        new List<int> {8, 13, 17},          // position{12}
        new List<int> {5, 12, 14, 20},      // position{13}
        new List<int> {2, 13, 23},          // position{14}
        new List<int> {11, 16},             // position{15}
        new List<int> {15, 17, 19},         // position{16}
        new List<int> {12, 16},             // position{17}
        new List<int> {10, 19},             // position{18}
        new List<int> {16, 18, 20, 22},     // position{19}
        new List<int> {13, 19},             // position{20}
        new List<int> {9, 22},              // position{21}
        new List<int> {19, 21, 23},         // position{22}
        new List<int> {14, 22} };           // position{23}
        public Form1()
        {
            InitializeComponent();
            SetupPictureBoxes();
            InitializeGame();
            /*
            Button restartButton = new Button();
            restartButton.Text = "Restart Game";
            restartButton.Location = new Point(754, 755); // You can adjust these values for button's position
            restartButton.Click += restartButton_Click;
            this.Controls.Add(restartButton);*/
        }

        private void InitializeGame()
        {
            turn = 'r';
            redPieces = PiecesAtStart;
            bluePieces = PiecesAtStart;

            redPiecesOnBoard = boardAtStart;
            bluePiecesOnBoard = boardAtStart;
            //allPictureBoxes = new List<PictureBox>();

            // all possible mills
            mills = new List<List<int>>()
            {
                new List<int> { 0, 1, 2 },
                new List<int> { 3, 4, 5 },
                new List<int> { 6, 7, 8 },
                new List<int> { 9, 10, 11 },
                new List<int> { 12, 13, 14 },
                new List<int> { 15, 16, 17 },
                new List<int> { 18, 19, 20 },
                new List<int> { 21, 22, 23 },
                new List<int> { 0, 9, 21 },
                new List<int> { 3, 10, 18 },
                new List<int> { 6, 11, 15 },
                new List<int> { 1, 4, 7 },
                new List<int> { 16, 19, 22 },
                new List<int> { 8, 12, 17 },
                new List<int> { 5, 13, 20 },
                new List<int> { 2, 14, 23 }
            };
            capture = false;

            foreach (var pictureBox in allPictureBoxes)
            {
                // Clearing the image and tag of each PictureBox.
                pictureBox.Image = null;
                pictureBox.Tag = null;
            }


            // add PictureBox1 through PictureBox24 to allPictureBoxes list
            /*for (int i = 1; i <= 24; i++) 
            {
                string controlName = "pictureBox" + i;
                Control control = groupBox1.Controls[controlName];

                if (control is PictureBox pictureBox)
                {
                    pictureBox.Click += PictureBox_Click;
                    allPictureBoxes.Add(pictureBox);
                    Debug.WriteLine($"Added {pictureBox.Name} to PictureBoxes list.");
                }
            }*/

            UpdateTurnLabel();
            UpdateModeLabel();
        }

        private void SetupPictureBoxes()
        {
            allPictureBoxes = new List<PictureBox>();

            // Add PictureBox1 through PictureBox24 to allPictureBoxes list
            for (int i = 1; i <= 24; i++)
            {
                string controlName = "pictureBox" + i;
                Control control = groupBox1.Controls[controlName];

                if (control is PictureBox pictureBox)
                {
                    pictureBox.Click += PictureBox_Click;
                    allPictureBoxes.Add(pictureBox);
                    Debug.WriteLine($"Added {pictureBox.Name} to PictureBoxes list.");
                }
            }
        }

        // click event for the 24 PictureBoxes
        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            int position;
            try
            {
                position = int.Parse(pictureBox.Name.Substring(10)) - 1;
            }
            catch (FormatException)
            {
                Debug.WriteLine("Failed to parse position from PictureBox name.");
                return;
            }

            Debug.WriteLine($"*****");
            Debug.WriteLine($"Clicked on position {position}");
            Debug.WriteLine($"*****");

            //capture mode
            if (capture)
            {
                CapturePiece(pictureBox, position);
            }
            else
            {
                // Check if we're still in phase one (placing pieces) or in phase two (moving pieces)
                if (redPieces > 0 || bluePieces > 0)
                {
                    // Phase One
                    if (pictureBox.Image == null)
                    {
                        PhaseOnePlacePiece(pictureBox, position);
  
                    }
                }
                else
                {
                    // Phase Two
                    if (pictureBox.Image == null)
                    {
                        // If a piece is selected and the move is valid, move it
                        if (selectedPictureBox != null && IsValidMove(int.Parse(selectedPictureBox.Name.Substring(10)) - 1, position))
                        {
                            PhaseTwoMovePiece(pictureBox);
                            
                        }
                    }
                    else
                    {
                        // If the piece is from the current player's turn, select it
                        if (pictureBox.Tag != null && pictureBox.Tag.ToString() == turn.ToString())
                        {
                            PhaseTwoSelectPiece(pictureBox);
                        }
                    }
                }
            }
        }



        // Phase 1: Setting pieces
        private void PhaseOnePlacePiece(PictureBox pictureBox, int position)
        {

            PictureBox pb1 = allPictureBoxes[position];

            pb1.Image = turn == 'r' ? Properties.Resources.Red : Properties.Resources.Blue;
            pb1.Tag = turn;

            string tag = $"Tag of position {position} is turned to : {pb1.Tag.ToString()}";
            
            Debug.WriteLine(tag);
            Debug.WriteLine($"*****");
            if (turn == 'r')
            {
                redPieces--;
                redPiecesOnBoard++;
            }

            else
            {
                bluePieces--;
                bluePiecesOnBoard++;
            }

            
            CheckForMillAndCapture(pictureBox, position);

        }

        // check if a mill is formed
        private bool checkMill(int position)
        {
            foreach (List<int> mill in mills)
            {
                if (mill.Contains(position))
                {
                    PictureBox pb1 = allPictureBoxes[mill[0]];
                    PictureBox pb2 = allPictureBoxes[mill[1]];
                    PictureBox pb3 = allPictureBoxes[mill[2]];

                    // Debugging info
                    string debugInfo = $"Checking Mill: {mill[0]}, {mill[1]}, {mill[2]}\n";
                    debugInfo += $"pb1: {(pb1.Tag != null ? pb1.Tag.ToString() : "null")}, ";
                    debugInfo += $"pb2: {(pb2.Tag != null ? pb2.Tag.ToString() : "null")}, ";
                    debugInfo += $"pb3: {(pb3.Tag != null ? pb3.Tag.ToString() : "null")}\n";
                    debugInfo += $"Current Turn: {turn}";
                    Debug.WriteLine(debugInfo);

                    // Check if all three positions are occupied by the current player's pieces
                    if (pb1.Tag != null && pb2.Tag != null && pb3.Tag != null &&
                        pb1.Tag.ToString() == turn.ToString() &&
                        pb2.Tag.ToString() == turn.ToString() &&
                        pb3.Tag.ToString() == turn.ToString())
                    {
                        return true;
                    }
                }
            }
            string debugEnd = $"----------------------------------";
            Debug.WriteLine(debugEnd);
            return false;
        }

        // go check while playing
        private void CheckForMillAndCapture(PictureBox pictureBox, int position)
        {
            if (checkMill(position))
            {
                capture = true;
                UpdateModeLabel();
            }
            else
            {
                NextTurn();
            }
        }

        // Phase 2: selecting pieces
        private void PhaseTwoSelectPiece(PictureBox pictureBox)
        {
            if (selectedPictureBox != null)
            {
                selectedPictureBox.BorderStyle = BorderStyle.None;
            }

            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            selectedPictureBox = pictureBox;
        }

        // Phase 2: moving pieces
        private void PhaseTwoMovePiece(PictureBox pictureBox)
        {
            // Copy piece from selectedPictureBox to pictureBox
            pictureBox.Image = selectedPictureBox.Image;
            pictureBox.Tag = selectedPictureBox.Tag;

            // Clear selectedPictureBox
            selectedPictureBox.Image = null;
            selectedPictureBox.Tag = null;
            selectedPictureBox.BorderStyle = BorderStyle.None;

            // Check if a mill is formed
            int position = int.Parse(pictureBox.Name.Substring(10)) - 1;
            CheckForMillAndCapture(pictureBox, position);

        }


        private void CapturePiece(PictureBox pictureBox, int position)
        {
            // check if there is a piece at the clicked position and it's not the current player's piece
            if (pictureBox.Image != null && pictureBox.Tag.ToString() != turn.ToString())
            {
                pictureBox.Image = null;  // remove the image
                pictureBox.Tag = null;  // clear the tag

                // decrement the number of opponent's pieces
                if (turn == 'r')
                {
                    bluePiecesOnBoard--;
                }
                else
                {
                    redPiecesOnBoard--;
                }

                NextTurn();
            }
        }

        private bool IsValidMove(int from, int to)
        {
            // Check if the player is in Phase 3 (only three pieces left on the board)
            if ((turn == 'r' && redPiecesOnBoard == 3) || (turn == 'b' && bluePiecesOnBoard == 3))
            {
                // In Phase 3, the player can move to any vacant point
                return allPictureBoxes[to].Image == null;
            }

            // If not in Phase 3, the standard adjacency rule applies
            return adjacencyList[from].Contains(to);
        }

        // changing the player trun
        private void NextTurn()
        {
            turn = turn == 'r' ? 'b' : 'r';
            capture = false;
            selectedPictureBox = null;

            foreach (PictureBox pictureBox in allPictureBoxes)
            {
                pictureBox.BorderStyle = BorderStyle.None;
            }

            UpdateTurnLabel();
            UpdateModeLabel();

            if ((redPieces + redPiecesOnBoard) < 3 || (bluePieces + bluePiecesOnBoard) < 3)
            {
                MessageBox.Show($" Game over! {(redPieces + redPiecesOnBoard < 3 ? player2Name : player1Name)} wins! \n {(redPieces + redPiecesOnBoard < 3 ? player1Name : player2Name)} has less than 3 Pieces left", "Game Over");
                InitializeGame();
            }
            else if (redPieces == 0 && bluePieces == 0 && !AnyLegalMoves())
            {
                MessageBox.Show($" Game over! {(turn == 'r' ? player2Name : player1Name)} wins! \n {(turn == 'r' ? player1Name : player2Name)} has no more legal moves", "Game Over");
                InitializeGame();
            }
        }

        // check for legal moves
        private bool AnyLegalMoves()
        {
            bool hasValidMove = false;
            // check if player has any valid moves
            foreach (PictureBox pb in allPictureBoxes)
            {
                if (pb.Tag != null && pb.Tag.ToString() == turn.ToString())
                {
                    int position = int.Parse(pb.Name.Substring(10)) - 1;
                    // check if the piece at this position has any valid moves
                    foreach (PictureBox pbTarget in allPictureBoxes)
                    {
                        if (pbTarget.Image == null && IsValidMove(position, int.Parse(pbTarget.Name.Substring(10)) - 1))
                        {
                            hasValidMove = true;
                            break;
                        }
                    }
                }

                if (hasValidMove)
                {
                    break;
                }
            }

            return hasValidMove;
        }

        // turn status
        private void UpdateTurnLabel()
        {
            labelTurn.Text = $"Turn: {(turn == 'r' ? player1Name : player2Name)}";
            redOffLabel.Text = $"Red pieces: {redPieces}";
            redOnLabel.Text = $"on board: {redPiecesOnBoard}";
            blueOffLabel.Text = $"Blue pieces: {bluePieces}";
            blueOnLabel.Text = $"on board: {bluePiecesOnBoard}";
        }

        // mode status
        private void UpdateModeLabel()
        {
            labelMode.Text = "Mode: Place a piece";
            if (capture)
            {
                labelMode.Text = "Mode: Capture opponent piece";
            }
            else if (redPieces > 0 || bluePieces > 0)
            {
                labelMode.Text = "Mode: Place a piece";
            }
            else
            {
                labelMode.Text = "Mode: Move a piece";
            }
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine($"restart is clicked");
            InitializeGame();
        }

        private void enterNames_Click(object sender, EventArgs e)
        {
            // Prompt for player names
            string tempPlayer1Name = Microsoft.VisualBasic.Interaction.InputBox("Enter name for Player 1 (Red):", "Player 1 Name", "Red");
            string tempPlayer2Name = Microsoft.VisualBasic.Interaction.InputBox("Enter name for Player 2 (Blue):", "Player 2 Name", "Blue");

            // Set player names if provided
            if (!string.IsNullOrWhiteSpace(tempPlayer1Name))
            {
                player1Name = tempPlayer1Name;
            }

            if (!string.IsNullOrWhiteSpace(tempPlayer2Name))
            {
                player2Name = tempPlayer2Name;
            }
        }
    }
}
