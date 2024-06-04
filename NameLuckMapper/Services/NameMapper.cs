using NameLuckMapper.Models;
using Newtonsoft.Json.Linq;

using System;
using System.Globalization;

namespace NameLuckMapper.Services
{
    public class NameMapper
    {

        public static JObject  Mapper(NameDobModel payload)
        {
            JObject Finalres = new JObject();
var datetime = payload.Datetime.ToString();
DateTime dt = DateTime.ParseExact(datetime, "dd-MM-yyyy", CultureInfo.InvariantCulture);
//DateTime.TryParse(payload.Datetime.ToString(), out DateTime parsedDate);
Dictionary<char, int> letterToNumberMap = new Dictionary<char, int>
{
{'A', 1}, {'B', 2}, {'C', 3}, {'D', 4}, {'E', 5}, {'U', 6}, {'O', 7}, {'F', 8},
{'I', 1}, {'K', 2}, {'G', 3}, {'M', 4}, {'H', 5}, {'V', 6}, {'Z', 7}, {'P', 8},
{'J', 1}, {'R', 2}, {'L', 3}, {'T', 4}, {'N', 5}, {'W', 6},
{'Y', 1},                               {'X', 5},
{'Q', 1},           {'S', 3}
};

string input = payload.Name;
List<int> numbers = MapLettersToNumbers(input, letterToNumberMap);
int SumofName = GetSingleDigitSum(numbers);
JArray pyramidStructure = new JArray();
pyramidStructure.Add(new JArray(numbers));
int dobSum = 0;
dobSum = SumAndReduceDateDigits(dt);            while (numbers.Count > 1)
            {
                List<int> newRow = SumAdjacentDigits(numbers);
                pyramidStructure.Add(new JArray(newRow));
                numbers = newRow;
            }

            JObject result = new JObject();
            Finalres["input"] = new JArray(input);
            Finalres["Datetime"] = new JArray(datetime);
            Finalres["SumofName"] = new JArray(SumofName);
            Finalres["DobSum"] = dobSum;
            Finalres["pyramid"] = pyramidStructure;
            Finalres["finalResult"] = numbers[0];

            return Finalres;


            //List<int> result = MapLettersToNumbers(input, letterToNumberMap);
            //int dobSum = 0;
            //dobSum = SumAndReduceDateDigits(parsedDate);
            //JObject Pyrami =NumbertoPyrmaid(result);




            //Console.WriteLine("Input: " + input);
            //Console.WriteLine("Output: " + string.Join(",", result));
            //Console.WriteLine("DOb Sum: " + dobSum);
            //Console.WriteLine("");
            //return string.Join(",", result);
        }
         public static int GetSingleDigitSum(List<int> numbers)
 {
     // Calculate the sum of the numbers
     int sum = numbers.Sum();

     // Reduce the sum to a single digit
     while (sum >= 10)
     {
         sum = sum.ToString().Select(c => int.Parse(c.ToString())).Sum();
     }

     return sum;
 }
        
        public static List<int> MapLettersToNumbers(string input, Dictionary<char, int> letterToNumberMap)
        {
            List<int> result = new List<int>();

            foreach (char c in input.ToUpper())
            {
                if (letterToNumberMap.ContainsKey(c))
                {
                    result.Add(letterToNumberMap[c]);
                }
                else
                {
                    Console.WriteLine($"Character '{c}' not found in the mapping.");
                }
            }

            return result;
        }
        public static JObject  NumbertoPyrmaid(List<int> result)
        {
            List<int> lastRow = result;
            //Console.WriteLine("Starting list: " + string.Join(", ", lastRow));
            //int finalResult = ReduceToSingleDigit(lastRow);
            //Console.WriteLine("Final single digit: " + finalResult);
            
            JObject pyramid = GeneratePyramid(lastRow);
            Console.WriteLine(pyramid.ToString());
            return pyramid;
        }

        static JObject GeneratePyramid(List<int> numbers)
        {
            JArray pyramidStructure = new JArray();
            pyramidStructure.Add(new JArray(numbers));

            while (numbers.Count > 1)
            {
                List<int> newRow = SumAdjacentDigits(numbers);
                pyramidStructure.Add(new JArray(newRow));
                numbers = newRow;
            }

            JObject result = new JObject();
            result["pyramid"] = pyramidStructure;
            result["finalResult"] = numbers[0];

            return result;
        }
        public static List<int> SumAdjacentDigits(List<int> numList)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < numList.Count - 1; i++)
            {
                int sum = numList[i] + numList[i + 1];
                result.Add(ReduceToSingleDigit(sum));
            }
            return result;
        }

        public static int ReduceToSingleDigit(int number)
        {
            while (number >= 10)
            {
                number = SumDigits(number);
            }
            return number;
        }


        public static int SumAndReduceDateDigits(DateTime date)
        {
            int daySum = SumDigits(date.Day);
            int monthSum = SumDigits(date.Month);
            int yearSum = SumDigits(date.Year);

            int totalSum = daySum + monthSum + yearSum;

            return ReduceToSingleDigits(totalSum);
        }

        private static int SumDigits(int number)
        {
            int sum = 0;
            while (number > 0)
            {
                sum += number % 10;
                number /= 10;
            }
            return sum;
        }

        private static int ReduceToSingleDigits(int number)
        {
            while (number >= 10)
            {
                number = SumDigits(number);
            }
            return number;
        }
    }

}
    
