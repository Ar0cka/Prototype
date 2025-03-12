namespace Monsters.Pathfinder
{
    public class Node
    {
        public int X { get; private set; } //X координата
        public int Y { get; private set; } //Y координата
        
        public bool IsWalkable { get; private set; } // Бул переменная которая говорит, можно тут пройти или нет
        
        public int GCost { get; set; } // Стоимость от старта до этого узла
        public int HCost { get; set; } // Эвретическая стоимость
        public int FCost => GCost + HCost; // Общая стоимость пути
        
        public Node Parent { get; set; } //Родитель для восстановления пути

        public Node(int x, int y, bool isWalkable)
        {
            X = x;
            Y = y;
            IsWalkable = isWalkable;
            GCost = 0;
            HCost = 0;
            Parent = null;
        }

        public string ToStringDebug()
        {
            return $"Node ({X},{Y}) - isWalkable = {IsWalkable}, F cost = {FCost}";
        }
    }
}