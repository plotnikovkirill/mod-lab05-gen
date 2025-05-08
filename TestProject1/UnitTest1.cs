using System;
using generator;
using Xunit;
namespace TestProject1
{
    public class UnitTest1
    {
        private Random random = new Random();
        [Fact]
        public void Test1()
        {
            var gen = new CharGenerator("..//ProjCharGenerator//biweights.txt");
            Assert.Equal(694, gen.getSize());
        }
        [Fact]
        public void CharGen_Test2()
        {
            var gen = new CharGenerator("..//ProjCharGenerator//biweights.txt");
            var valid = new HashSet<string> { "ст", "ам", "ва" };
            int a = 0;

            for (int i = 0; i < 100; i++)
            {
                if (valid.Contains(gen.getSym()))
                {
                    a = 1; break;

                }
            }
            Assert.True(a == 1);
        }
        [Fact]
        public void CharGen_Test3()
        {

            var gen = new CharGenerator("..//ProjCharGenerator//biweights.txt");
            var stats = new Dictionary<string, int>();

            for (int i = 0; i < 10000; i++)
            {
                var sym = gen.getSym();
                if (stats.ContainsKey(sym))
                    stats[sym]++;
                else
                    stats.Add(sym, 1);
            }
            Assert.True(stats[gen.getSym()] <= stats["ст"]);
            Assert.True(stats[gen.getSym()] <= stats["ст"]);
        }
        [Fact]
        public void WordGen_Test7()
        {
            var gen = new WordGenerator("..//ProjCharGenerator//wordweights2.txt");
            Assert.Equal(1000, gen.getSize());
        }
        [Fact]
        public void WordGen_Test8()
        {
            var gen = new WordGenerator("..//ProjCharGenerator//wordweights2.txt");
            var valid = new HashSet<string> { "и", "в", "не" };
            int a = 0;

            for (int i = 0; i < 100; i++)
            {
                if (valid.Contains(gen.getSym()))
                {
                    a = 1; break;

                }
            }
            Assert.True(a == 1);
        }
        [Fact]
        public void WordGen_Test9()
        {
            var gen = new WordGenerator("..//ProjCharGenerator//wordweights2.txt");
            var stats = new Dictionary<string, int>();

            for (int i = 0; i < 10000; i++)
            {
                var sym = gen.getSym();
                if (stats.ContainsKey(sym))
                    stats[sym]++;
                else
                    stats.Add(sym, 1);
            }
            Assert.True(stats["который"] < stats["и"]);
            Assert.True(stats["для"] < stats["в"]);
        }
        [Fact]
        public void WordGen_Test10()
        {
            var gen = new WordGenerator("..//ProjCharGenerator//wordweights.txt");
            var stats = new Dictionary<string, int>();
            int a = 0;
            for (int i = 0; i < 10000; i++)
            {
                var sym = gen.getSym();
                if (stats.ContainsKey(sym))
                    stats[sym]++;
                else
                    stats.Add(sym, 1);
            }
            if (stats.ContainsKey("Миша"))
            {
                a = 1;
            }
            Assert.Equal(0, a);
        }
        [Fact]
        public void WordGen_Test11()
        {
            var gen = new WordGenerator("..//ProjCharGenerator//wordweights.txt");
            var stats = new Dictionary<string, int>();
            int a = 0;
            for (int i = 0; i < 10000; i++)
            {
                var sym1 = gen.getSym();
                var sym2 = gen.getSym();
                if (sym1 == sym2)
                {
                    a = 1;
                    break;
                }
            }
            Assert.Equal(1, a);
        }
        [Fact]
        public void WordGen_Test12()
        {
            var gen = new WordGenerator("..//ProjCharGenerator//wordweights2.txt");
            var stats = new Dictionary<string, int>();
            int a = 0;
            for (int i = 0; i < 10000; i++)
            {
                var sym1 = gen.getSym();
                var sym2 = gen.getSym();
                var sym3 = gen.getSym();
                if (sym1 == sym3 && sym2 == "и")
                {
                    a = 1;
                    break;
                }
            }
            Assert.Equal(1, a);
        }
        [Fact]
        public void WordGen_Test13()
        {
            var gen = new WordGenerator("..//ProjCharGenerator//wordweights.txt");
            var stats = new Dictionary<string, int>();
            int a = 0;
            for (int i = 0; i < 10000; i++)
            {
                var sym1 = gen.getSym();
                var sym2 = gen.getSym();
                var sym3 = gen.getSym();
                if (sym1 == sym3 && sym2 == sym1)
                {
                    a = 1;
                    break;
                }
            }
            Assert.Equal(1, a);
        }
    }
}