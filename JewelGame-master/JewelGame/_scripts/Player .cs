using System.Threading.Tasks;

namespace JewelGame._Scripts
{
    public class Player
    {   
        public string Name { get; set; }
        public int HP { get; set; } = 100;
        public int Shield { get; set; } = 0;
        public int controlMana { get; set; } = 0;
        public bool addShield = false;
        public bool isControl = false;
        public int _damage = 0;
        public bool isDamage = false;
        
     
        public async Task TakeDamage(int damage)
        {
           

            if (addShield && damage != 0)
            {
                _damage = 0;
                HP -= 0;
                Shield = 0;
                addShield = false;
            }
            else if (damage != 0) 
            {   isDamage = true;
                _damage = damage;
                int i = 0;
                while(i != damage )
                {   if (HP == 0) return;

                    HP -= 1;
                    await Task.Delay(10);
                    i++;
                    
                }
            }
        }
        public void Control(int mana)
        {
            controlMana += mana;
            if(controlMana >= 10) { 
                isControl = true;
                controlMana = 0;
            }
        }
        public async Task takeHealth(int health)
        {
            if (HP + health >= 100)
            {

                while (HP <100)
                {
                    HP += 1;
                    await Task.Delay(30);
                    

                }

            }
            else
            {   int i = 0;
                while (i != health)
                {
                    HP += 1;
                    await Task.Delay(30);
                    i++;
                }
            }
            
        }
        public void TakeShield(int s)
        {
          
            Shield += s;
            if (Shield >= 10)
            {
                addShield = true;
                Shield = 10;
            }
           

        }
        public async Task manageTimer()
        {
            await Task.Delay(1000);
            GameManager .canClick = true ;
        }
       
        public bool IsDefeated()
        {
            return HP <= 0;
        }
    }
}
