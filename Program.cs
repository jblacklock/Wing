using System;
using System.Collections.Generic;
using System.Linq;

namespace WingStop
{
    class Program
    {
        public static List<List<WingCombo>> possibleWingCombos = new List<List<WingCombo>>();

        public static float cheapestCombo = -1;

        static void Main(string[] args)
        {

            Console.WriteLine("for each combo deal, type '#,$' then click enter.");
            Console.WriteLine("Example: '12,13.69'");
            Console.WriteLine("When you're done, type 'c' and press enter to calculate the optimal subset");
            List<WingCombo> WingCombos = new List<WingCombo>();
            while (true)
            {
                string UserInput = Console.ReadLine();
                if (UserInput == "c")
                {
                    break;
                }
                string[] values = UserInput.Split(',');
                List<string> ListValues = new List<string>(values);
                float f;
                int i;
                if (ListValues.Count == 2 && float.TryParse(ListValues[1], out f) && int.TryParse(ListValues[0], out i))
                {
                    WingCombo nextCombo = new WingCombo(f, i);
                    WingCombos.Add(nextCombo);
                }
                else
                {
                    Console.WriteLine("for each combo deal, type '#,$' then click enter.");
                    Console.WriteLine("Example: '12,13.69'");
                    Console.WriteLine("When you're done, type 'c' and press enter to calculate the optimal subset");
                }
            }
            Console.WriteLine("How many wings would you like to buy?");
            int finalNumberOfWings = 0;
            while (finalNumberOfWings <= 0)
            {
                bool result = int.TryParse(Console.ReadLine(), out finalNumberOfWings);
                if (!result)
                {
                    Console.WriteLine("Input error: How many wings would you like to buy?");
                }
            }

            // for(int i = 151; i < 201; i++){
            // possibleWingCombos.Clear();
            Combination(WingCombos, finalNumberOfWings);


            Console.WriteLine("");
            Console.WriteLine("If you want to get " + finalNumberOfWings.ToString() + " wings, here's what you order...");
            int Counter = 0;
            foreach (List<WingCombo> wc in possibleWingCombos)
            {
                if (Counter == 1)
                {
                    Console.WriteLine("Alternatively you could order...");
                }
                else if (Counter > 1)
                {
                    Console.WriteLine("Or...");
                }
                while (wc.Count() > 0)
                {
                    int thisCombosCount = wc[0].Amount;
                    IEnumerable<WingCombo> typeCount = wc.Where(x => x.Amount == thisCombosCount);
                    if (typeCount.Count() == 1)
                    {
                        Console.Write("1 order of ");
                    }
                    else
                    {
                        Console.Write(typeCount.Count().ToString() + " orders of ");
                    }
                    wc.RemoveAll(x => x.Amount == thisCombosCount);
                    Console.WriteLine(thisCombosCount.ToString() + " piece wings");

                }
                Console.WriteLine("It'll cost $" + Math.Round(cheapestCombo, 2).ToString());
                Counter++;
                // }
            }
        }



        static void Combination(List<WingCombo> A, int K)
        {
            // Sort the given elements 
            List<WingCombo> OrderedWings = A.Where(x => x.Amount <= K).OrderBy(x => x.Amount).ToList();

            // To store combination 
            List<WingCombo> local = new List<WingCombo>();

            unique_combination(0, 0, K, local, OrderedWings);
        }


        // Function to find all unique combination of 
        // given elements such that their sum is K 
        static void unique_combination(int l, int sum, int K, List<WingCombo> local, List<WingCombo> A)
        {
            // If a unique combination is found 
            if (sum == K)
            {
                List<WingCombo> uniqueCombo = new List<WingCombo>();
                float totalprice = 0;
                foreach (WingCombo i in local)
                {
                    uniqueCombo.Add(i);
                    totalprice += i.Cost;
                }
                if (totalprice <= cheapestCombo || cheapestCombo < 0)
                {
                    if (totalprice < cheapestCombo)
                    {
                        possibleWingCombos.Clear();
                    }
                    cheapestCombo = totalprice;
                    possibleWingCombos.Add(uniqueCombo);
                }

                return;
            }

            // For all other combinations 
            for (int i = l; i < A.Count; i++)
            {

                // Check if the sum exceeds K 
                if (sum + A[i].Amount > K)
                    continue;

                // Check if it is repeated or not 
                if (i == 1 &&
                    A[i].Amount == A[i - 1].Amount &&
                    i > l)
                    continue;

                // Take the element into the combination 
                local.Add(A[i]);

                // Recursive call 
                unique_combination(i + 1, sum + A[i].Amount, K, local, A);

                // Remove element from the combination 
                local.RemoveAt(local.Count - 1);
            }
        }

    }

    class WingCombo
    {
        public int Amount { get; set; }
        public float Cost { get; set; }
        public float ValueRatio { get; set; }
        public WingCombo(float cost, int amount)
        {
            Cost = cost;
            Amount = amount;
            ValueRatio = Cost / Amount;
        }
    }
}