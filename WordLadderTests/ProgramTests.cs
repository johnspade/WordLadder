using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordLadder;
using System.Collections.Generic;
using System.Linq;

namespace WordLadderTests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void GetsCorrectVariants_ForNextStep()
        {
            Program.dictionary = new List<string>() { "тон", "рот", "тот", "мот", "пот", "сор" };
            var previousStep = new List<string>() { "кот" };
            char[] alphabet = "абвгдежзийклмнопрстуфхцчшщъыьэюя".ToCharArray();
            var possibleSymbols = new List<List<char>>();
            for (int i = 0; i < 3; i++)
                possibleSymbols.Add(new List<char>(alphabet));
            Program.possibleSymbols = possibleSymbols;

            var expected = (new List<string>() { "рот", "тот", "мот", "пот" }).OrderBy(s => s).ToList();
            var actual = (Program.GetNextStep(previousStep)).OrderBy(s => s).ToList();

            CollectionAssert.AreEqual(expected, actual, "Неверный список вариантов");
        }

        [TestMethod]
        public void GetsCorrectParent()
        {
            var step = new List<string>() { "рот", "тот" };
            string child = "тон";

            Assert.AreEqual<string>("тот", Program.GetParent(step, child), "Неверный родитель");
        }

        [TestMethod]
        public void ComparesWords_IsOnlyOneCharDifferent()
        {
            string str1 = "кот";
            string str2 = "рот";

            Assert.AreEqual(true, Program.IsOnlyOneCharDifferent(str1, str2));

            str1 = "кот";
            str2 = "тон";

            Assert.AreEqual(false, Program.IsOnlyOneCharDifferent(str1, str2));
        }
    }
}