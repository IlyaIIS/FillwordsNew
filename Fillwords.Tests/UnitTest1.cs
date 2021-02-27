using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fillwords.Tests
{
    public class Tests
    {
        [TestCase]
        public void CorrectlyCreateFieldTest()
        {
            File.WriteAllText("TestFile.txt", "один\nдва\nтри\nчетыре\nпять\nшесть\nсемь\nвосемь\nдевять\nдесять\nодинадцать\nдвенадцать\nтринадцеть\nчетырнадцать\nпятнадцать\nшестнадцать\nсемнадцать\nвосемнадцать\nдевятнадцать\nдвадцать");
            DataWorker.ReadWordsFromFile("TestFile.txt");
            File.Delete("TestFile.txt");

            Field field = new Field();
            field.CreateNewField(4,4,DataWorker.WordsSet);

            Assert.AreEqual(4, field.XSize);
            Assert.AreEqual(4, field.YSize);
            Assert.AreEqual(false, field.IsLoaded);

            foreach (char letter in field.CellLetter) Assert.IsFalse(letter == ' ');
            foreach (string word in field.WordsList) Assert.IsFalse(word == string.Empty);
        }
        
        [TestCase("Один\nДва\nТри", ExpectedResult = new string[] { "Один", "Два", "Три" })]
        [TestCase("Один\n\nТри", ExpectedResult = new string[] { "Один","", "Три" })]
        public string[] DictionaryReadingTest(string fileData)
        {
            File.WriteAllText("TestFile.txt", fileData);
            DataWorker.ReadWordsFromFile("TestFile.txt");
            File.Delete("TestFile.txt");
            return DataWorker.WordsSet.AllWords;
        }

        [TestCaseSource(nameof(TestCases))]
        public void CorrectlySnakeDirectionTest(int[] result, int x, int y, bool[,] fieldArr)
        {
            Assert.Contains(new Field().FindDirection(x, y, fieldArr), result);
        }
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(new int[] { 0 }, 1, 1, new bool[,] { { false, false, false }, { false, true, false }, { false, false, false } });
                yield return new TestCaseData(new int[] { 4 }, 1, 1, new bool[,] { { false, false, false }, { false, true, true  }, { false, false, false } });
                yield return new TestCaseData(new int[] { 3 }, 1, 1, new bool[,] { { false, true,  false }, { false, true, false }, { false, false, false } });
                yield return new TestCaseData(new int[] { 2 }, 1, 1, new bool[,] { { false, false, false }, { true,  true, false }, { false, false, false } });
                yield return new TestCaseData(new int[] { 1 }, 1, 1, new bool[,] { { false, false, false }, { false, true, false }, { false, true,  false } });
                yield return new TestCaseData(new int[] { 2, 3 }, 1, 1, new bool[,] { { false, true, false }, { true, true, false }, { false, false, false } });
                yield return new TestCaseData(new int[] { 1, 3, 4 }, 1, 1, new bool[,] { { false, true, false }, { true, true, true }, { false, false, false } });
                yield return new TestCaseData(new int[] { 1, 2, 3, 4 }, 1, 1, new bool[,] { { false, true, false }, { true, true, true }, { false, true, false } });
            }
        }
    }
}