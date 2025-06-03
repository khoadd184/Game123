using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JewelGame._Scripts
{

    internal class GameManager
    {
        private JewelGrid _jewelGrid;

        public static Player _player1 = new Player();
        public static Player _player2 = new Player();
        public static bool canClick = true;
        public bool _isPlayer1Turn = true;
        private int a = 0, b = 0;

       

        public int[] LastResult { get; private set; }

        public GameManager(JewelGrid jewelGrid)
        {
           
            _jewelGrid = jewelGrid;

           

            _jewelGrid._OnEndTurn += _OnEndTurn;
        }

        private void _OnEndTurn(int[] result)
        {
            int damage = result[0] * 7;
            int heal = result[1] * 5;
            int shield = result[2];
            int buff = result[3] * 3;
            int control = result[4];
           
            
            if (_isPlayer1Turn)
                {   
                    
                    _player1.TakeShield(shield + buff / 6);
                    _player2.TakeDamage(damage + buff);
                    _player1.takeHealth(heal + buff);
                    _player1.Control(control + buff / 6);
                    _player1.manageTimer();
    
            }
                else
                {   
                    _player2.TakeShield(shield + buff / 6);
                    _player1.TakeDamage(damage + buff);
                    _player2.takeHealth(heal + buff);
                    _player2.Control(control + buff / 6);

                _player2.manageTimer();
    
            }

                if (_isPlayer1Turn && _player1.isControl)
                {
                    
                   
                _player1.manageTimer();
            }
                else if (!_isPlayer1Turn && _player2.isControl)
                {
                
           
                _player1.manageTimer();
            }
                else 
                {
                    _isPlayer1Turn = !_isPlayer1Turn;
                _player1.manageTimer();
            }

            

            canClick = false;
            
            
        }
        public void Update(Panel panelCover, int curentPanel)
        {
            int maxMana = 10;
            int fullWidth = panelCover.Parent.Width;
            float ratio = (float)(maxMana - curentPanel) / maxMana;
            panelCover.Width = (int)(fullWidth * ratio);
        }
        public bool playerTurn()
        {
            return _isPlayer1Turn;
        }
    }
}

