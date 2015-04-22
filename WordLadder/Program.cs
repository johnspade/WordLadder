using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace WordLadder
{
    public class Program
    {
        const string inputFile = @"input.txt";
        const string dictionaryFile = @"dictionary.txt";
        const int maxStepsCount = 50;
        public static List<string> dictionary;
        public static List<List<char>> possibleSymbols;

        static void Main()
        {
            Console.WriteLine("***** Программа для решения задачи \"Как из мухи сделать слона\" *****\n");

            if (!File.Exists(inputFile) || !File.Exists(dictionaryFile))
                ShowError(String.Format("Файл {0} и/или {1} отсутствует", inputFile, dictionaryFile));

            string[] input = File.ReadAllLines(inputFile);
            if (input.Count() < 2)
                ShowError("Введите два слова");

            string start = input[0].Trim().ToLower();
            string end = input[1].Trim().ToLower();
            if (start.Length != end.Length)
                ShowError("Введены слова разной длины");

            int length = start.Length;

            dictionary = (from w in File.ReadAllLines(dictionaryFile) where w.Length == length select w).ToList();
            dictionary.Remove(start);

            // Cписок возможных символов для каждой позиции (для уменьшения количества проходов по словарю)
            possibleSymbols = new List<List<char>>(length);
            for (int i = 0; i < length; i++)
                possibleSymbols.Add(new List<char>());

            foreach (string word in dictionary)
            {
                for (int i = 0; i < length; i++)
                {
                    if (!possibleSymbols[i].Contains(word[i]))
                        possibleSymbols[i].Add(word[i]);
                }     
            }

            // В шаге цепочки хранятся все варианты слов, образованные из слов предыдущего шага
            var steps = new List<List<string>>();
            var step = new List<string>() { start };
            bool ladderCompleted = false;

            while (steps.Count <= maxStepsCount)
            {
                step = GetNextStep(step);
                if (step.Count == 0)
                    break;

                steps.Add(step);
                if (step.Contains(end))
                {
                    ladderCompleted = true;
                    break;
                } 
            }

            if (!ladderCompleted)
                ShowError("Построить цепочку не удалось");

            string ladder = end;
            string child = end;

            for (int i = steps.Count - 2; i >= 0; i--)
            {
                child = GetParent(steps[i], child);
                ladder = child + "\n" + ladder;
            }

            Console.WriteLine(start + "\n" + ladder);
            
            Console.ReadLine();
        }

        static void ShowError(string message)
        {
            Console.WriteLine("ОШИБКА: " + message);
            Console.ReadLine();
            Environment.Exit(0);
        }

        /// <summary>
        /// Функция составляет общий список всех вариантов слов, отличающихся от слов предыдущего шага на один символ
        /// </summary>
        /// <param name="previousStep">Предыдущий шаг</param>
        /// <returns>Возвращает список вариантов слов</returns>
        public static List<string> GetNextStep(List<string> previousStep)
        {
            char[] w;
            string current;
            var step = new List<string>();

            foreach (string word in previousStep)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    w = word.ToCharArray();
                    foreach (char letter in possibleSymbols[i])
                    {
                        w[i] = letter;
                        current = new string(w);
                        if (dictionary.Contains(current))
                        {
                            step.Add(current);
                            dictionary.Remove(current);
                        }
                    }
                }
            }

            return step;
        }

        /// <summary>
        /// Функция находит в списке вариантов предыдущего шага цепочки слово, отличающееся от данного на один символ
        /// </summary>
        /// <param name="step">Предыдущий шаг</param>
        /// <param name="child">Слово, для которого нужно найти родителя</param>
        /// <returns>Возвращает "родителя" данного слова</returns>
        public static string GetParent(List<string> step, string child)
        {
            foreach (string word in step)
            {
                if (IsOnlyOneCharDifferent(word, child))
                    return word;
            }

            return null;
        }

        public static bool IsOnlyOneCharDifferent(string str1, string str2)
        {
            return str1.Where((t, i) => !t.Equals(str2[i])).Count() == 1;
        }
    }
}
