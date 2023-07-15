namespace Vmp.Services.Extensions
{
    using System.Text;

    public static class HelperClass
    {
        public static string ReplaceBulgarianCharsWithEnglish(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            input = input.ToLower();
            Dictionary<char, char> map = new Dictionary<char, char>();
            map['а'] = 'a';
            map['б'] = 'b';
            map['в'] = 'v';
            map['г'] = 'g';
            map['д'] = 'd';
            map['е'] = 'e';
            map['ж'] = 'j';
            map['з'] = 'z';
            map['и'] = 'i';
            map['й'] = 'y';
            map['к'] = 'k';
            map['л'] = 'l';
            map['м'] = 'm';
            map['н'] = 'n';
            map['о'] = 'o';
            map['п'] = 'p';
            map['р'] = 'r';
            map['с'] = 's';
            map['т'] = 't';
            map['у'] = 'u';
            map['ф'] = 'f';
            map['х'] = 'h';
            map['ц'] = 'c';
            map['ч'] = '%';
            map['ш'] = '#';
            map['щ'] = '$';
            map['ъ'] = '+';
            map['ь'] = '-';
            map['ю'] = '&';
            map['я'] = '*';

            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                if (map.ContainsKey(c))
                {
                    sb.Append(map[c]);
                }
                else if (char.IsDigit(c))
                {
                    sb.Append(c);
                }
            }


            return sb.ToString();
        }

        public static string ReplaceEnglishCharsWithBulgarian(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            input = input.ToLower();
            Dictionary<char, char> map = new Dictionary<char, char>();
            map['a'] = 'а';
            map['b'] = 'б';
            map['v'] = 'в';
            map['g'] = 'г';
            map['d'] = 'д';
            map['e'] = 'е';
            map['j'] = 'ж';
            map['z'] = 'з';
            map['i'] = 'и';
            map['y'] = 'й';
            map['k'] = 'к';
            map['l'] = 'л';
            map['m'] = 'м';
            map['n'] = 'н';
            map['o'] = 'о';
            map['p'] = 'п';
            map['r'] = 'р';
            map['s'] = 'с';
            map['t'] = 'т';
            map['u'] = 'у';
            map['f'] = 'ф';
            map['h'] = 'х';
            map['c'] = 'ц';
            map['%'] = 'ч';
            map['#'] = 'ш';
            map['$'] = 'щ';
            map['+'] = 'ъ';
            map['-'] = 'ь';
            map['&'] = 'ю';
            map['*'] = 'я';

            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                if (map.ContainsKey(c))
                {
                    sb.Append(map[c]);
                }
                else if (char.IsDigit(c))
                {
                    sb.Append(c);
                }
            }


            return sb.ToString();
        }    
    }
}
