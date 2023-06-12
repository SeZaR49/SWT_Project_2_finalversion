/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWT_Project_2
{
    public class Logic
    {
        int currentPlayer = 1;
        private void AttachEventHandlers()
        {
            foreach (Control control in Controls)
            {
                if (control is PictureBox pictureBox)
                {
                    pictureBox.Click += PictureBox_Click;
                }
            }
        }
        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;

            // Check if the clicked PictureBox is empty
            if (pictureBox.Image == null)
            {
                // Set the image for the current player's piece
                if (currentPlayer == 1)
                {
                    pictureBox.Image = player1Piece;
                }
                else
                {
                    pictureBox.Image = player2Piece;
                }

                // Check for a win condition
                if (CheckForWin(currentPlayer))
                {
                    // Capture one of the opponent's pieces
                    CaptureOpponentPiece(currentPlayer);
                }

                // Switch to the other player
                currentPlayer = (currentPlayer == 1) ? 2 : 1;
            }
        }
        private bool CheckForWin(int player)
        {
            // Implement the logic to check for a win condition
            // Return true if a win condition is met, otherwise return false
            // You can check the positions of the PictureBox controls that the current player has placed pieces on
        }
        private void CaptureOpponentPiece(int player)
        {
            // Implement the logic to capture one of the opponent's pieces
            // You can select one of the opponent's PictureBox controls that contains a piece and set its Image property to null or an empty image
        }



















    }
}*/
