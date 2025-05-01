using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace generator
{
    class CharGenerator 
    {
        private List<string> syms = new List<string>();
        private List<int> weights = new List<int>();
        private List<int> upper_bounds = new List<int>();
        private char[] data;
        private int size;
        private int summ;
        private Random random = new Random();
        public CharGenerator() 
        {
            using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "biweights.txt"), Encoding.UTF8))
            {
                string input;
                string[] line;
                Int32 sum = 0;
                while ((input = reader.ReadLine()) != null)
                {
                    line = input.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    int i = 0;
                    syms.Add(line[i + 1]);
                    weights.Add(Int32.Parse(line[2]));
                    sum += Int32.Parse(line[2]);
                    upper_bounds.Add(sum);
                }
                summ = sum;
            }
        }
        public string getSym() 
        {
            var ch = random.Next(0, summ);
            for (int i = 0; i < upper_bounds.Count; i++)
            {
                if (ch <= upper_bounds[i])
                {
                    return syms[i];
                }
            }
            return "";
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            CharGenerator gen = new CharGenerator();
            SortedDictionary<string, int> stat = new SortedDictionary<string, int>();
            string ress = "";
            for (int i = 0; i < 1000; i++)
            {
                string ch = gen.getSym();
                ress += ch;
                if (stat.ContainsKey(ch))
                    stat[ch]++;
                else
                    stat.Add(ch, 1); Console.Write(ch);
            }
            Console.Write('\n');
            using (StreamWriter reader = new StreamWriter(Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "../Results/gen-1.txt"), false, Encoding.UTF8))
            {
                reader.WriteLine(ress);
            }

        }
    }
}

