using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using ScottPlot;
using System.Text;
using ScottPlot.Drawing.Colorsets;

namespace generator
{
    public class CharGenerator 
    {
        public Dictionary<string, double> bigrams = new Dictionary<string, double>();
        private List<string> syms = new List<string>();
        private List<int> weights = new List<int>();
        private List<int> upper_bounds = new List<int>();
        private char[] data;
        private int size;
        private int summ;
        private Random random = new Random();
        public CharGenerator(string name) 
        {
            using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, name), Encoding.UTF8))
            {
                string input;
                string[] line;
                Int32 sum = 0;
                while ((input = reader.ReadLine()) != null)
                {
                    line = input.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    int i = 0;
                    if (bigrams.ContainsKey(line[0]))
                    {
                        bigrams[line[0]] += Double.Parse(line[1]);
                    }
                    else
                    {
                        bigrams.Add(line[0], Double.Parse(line[1]));
                    }
                    syms.Add(line[0]);
                    weights.Add(Int32.Parse(line[1]));
                    sum += Int32.Parse(line[1]);
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
        public int getSize()
        {
            return syms.Count;
        }
    }
    public class WordGenerator
    {
        private List<string> syms = new List<string>();
        public Dictionary<string, double> wordWeights = new Dictionary<string, double>();
        public List<Double> weights = new List<Double>();
        private List<Double> upper_bounds = new List<Double>();
        private Double summ;
        private Random random = new Random();
        public WordGenerator(string n)
        {
            using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, n), Encoding.UTF8))
            {
                string input;
                string[] line;
                Double sum = 0;
                while ((input = reader.ReadLine()) != null)
                {
                    line = input.Replace('.', ',').Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    syms.Add(line[0]);
                    weights.Add(Double.Parse(line[1]));
                    if (wordWeights.ContainsKey(line[0]))
                    {
                        wordWeights[line[0]] += Double.Parse(line[1]);
                    }
                    else
                    {
                        wordWeights.Add(line[0], Double.Parse(line[1]));
                    }
                    sum += Double.Parse(line[1]);
                    upper_bounds.Add(sum);
                }
                summ = sum;
            }
        }
        public string getSym()
        {
            var ch = random.Next(0, (Int32)summ);
            for (int i = 0; i < upper_bounds.Count; i++)
            {
                if (ch <= upper_bounds[i])
                {
                    return syms[i];
                }
            }
            return "";
        }
        public int getSize()
        {
            return syms.Count;
        }
    }
    public static class PlotGenerator
    {
        public static void CreateFrequencyPlot(Dictionary<string, double> expected,
                                             Dictionary<string, int> actual,
                                             string title,
                                             string fileName)
        {
            var plt = new Plot();
            plt.Width = 1200;
            plt.Height = 800;

            var topData = expected
                .OrderByDescending(x => x.Value)
                .Take(50)
                .Select(x => new
                {
                    x.Key,
                    Expected = x.Value,
                    Actual = actual.ContainsKey(x.Key) ? actual[x.Key] : 0
                })
                .ToList();

            double[] positions = topData.Select((x, i) => (double)i).ToArray();
            string[] labels = topData.Select(x => x.Key).ToArray();
            double[] expectedValues = topData.Select(x => x.Expected).ToArray();
            double[] actualValues = topData.Select(x => (double)x.Actual).ToArray();

            double maxActual = actualValues.Max();
            double maxExpected = expectedValues.Max();
            double scaleFactor = maxActual / maxExpected;
            double[] scaledExpected = expectedValues.Select(x => x * scaleFactor).ToArray();

            var actualBars = plt.AddBar(actualValues, positions);
            actualBars.Label = "Фактические частоты";
            actualBars.Color = Color.FromArgb(100, 78, 121, 167);

            var expectedBars = plt.AddBar(scaledExpected, positions.Select(x => x + 0.2).ToArray());
            expectedBars.Label = "Ожидаемые частоты (нормализованные)";
            expectedBars.Color = Color.FromArgb(100, 225, 87, 89);

            plt.Title(title);
            plt.YLabel("Частота");
            plt.XLabel("Элементы");
            plt.Legend();

            plt.XAxis.ManualTickPositions(positions.Select(x => x + 0.1).ToArray(), labels);
            plt.XAxis.TickLabelStyle(rotation: 45);
            plt.XAxis.ManualTickSpacing(1);

            string filePath = Path.Combine(FileHelper.GetResultsDirectory(), fileName);
            plt.SaveFig(filePath);
        }
    }
    public static class FileHelper
    {
        public static string GetResultsDirectory()
        {
            string programDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string resultsDir = Path.Combine(Directory.GetParent(programDir).FullName, "Results");

            return resultsDir;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            CharGenerator gen = new CharGenerator("biweights.txt");
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
            var actualBigrams = new Dictionary<string, int>();
            for (int i = 0; i < ress.Length - 1; i++)
            {
                string bigram = ress.Substring(i, 2).ToLower();
                actualBigrams[bigram] = actualBigrams.ContainsKey(bigram) ? actualBigrams[bigram] + 1 : 1;
            }
            PlotGenerator.CreateFrequencyPlot(
                    gen.bigrams.ToDictionary(x => x.Key, x => x.Value),
                    actualBigrams,
                    "Топ-50 биграмм",
                    "gen-1.png");
            WordGenerator genWord = new WordGenerator("wordweights2.txt");
            SortedDictionary<string, int> statWord = new SortedDictionary<string, int>();
            ress = "";
            for (int i = 0; i < 1000; i++)
            {
                string ch = genWord.getSym();
                ress += ch + " ";
                if (statWord.ContainsKey(ch))
                    statWord[ch]++;
                else
                    statWord.Add(ch, 1); Console.Write(ch + " ");
            }
            Console.Write('\n');
            using (StreamWriter reader = new StreamWriter(Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "../Results/gen-2.txt"), false, Encoding.UTF8))
            {
                reader.WriteLine(ress);
            }
            var actualWords = ress.Split(' ')
                    .GroupBy(x => x.ToLower())
                    .ToDictionary(x => x.Key, x => x.Count());

            PlotGenerator.CreateFrequencyPlot(
                genWord.wordWeights,
                actualWords,
                "Распределение частот слов",
                "gen-2.png");
        }
    }
}

