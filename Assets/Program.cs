using System;

namespace GameJing_console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            GameJing game = new GameJing(15);
            game.getOkSet();
            game.getNumUsed();
            int g, n;
            while (!game.gameOver)
            {
                g = -1;
                while ((g < 0) || (g > 8))
                {
                    Console.WriteLine("请输入你想填写的位置[0-8]:");
                    g = Convert.ToInt16(Console.ReadLine());
                    if (game.GameGrid[g,0] != 0)
                    {
                        Console.WriteLine("{0}位置已经占用!",g);
                        g = -1;
                    }
                }
                n = -1;
                while ((n < 1) || (n > 9))
                {
                    Console.WriteLine("请输入你想填写的数字[1-9]:");
                    n = Convert.ToInt16(Console.ReadLine());
                    if (game.IsUsed(n))
                    {
                        Console.WriteLine("{0}数字已经被使用!");
                        n = -1;
                    }
                }
                int row = g / 3;
                int col = g % 3;
                game.GameGrid[g,0] = n;
                game.GameGrid[g,1] = 0;
                if (game.checkWon(row, col))
                {
                    Console.WriteLine("真聪明，你赢了！");
                    game.gameOver = true;
                }
                else
                {
                    game.AIGo();
                    if (game.aiWon)
                    {
                        Console.WriteLine("哈哈，我赢了，你怎么连个CPU都不如？！");
                    }
                }
                Console.WriteLine(game.getGridText());
                if (game.checkDeadlock())
                {
                    Console.WriteLine("出现僵局！");
                    game.gameOver = true;
                }
            }


                
        }
    }
}
