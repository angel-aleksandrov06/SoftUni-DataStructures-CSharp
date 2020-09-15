namespace Problem04.BalancedParentheses
{
    using System;
    using System.Collections.Generic;

    public class BalancedParenthesesSolve : ISolvable
    {
        public bool AreBalanced(string parentheses)
        {
            if (string.IsNullOrEmpty(parentheses) || parentheses.Length % 2 == 1)
            {
                return false;
            }

            Stack<char> openBrackets = new Stack<char>(parentheses.Length / 2);

            foreach (char cureBracket in parentheses)
            {
                char expectedChar = default;

                switch (cureBracket)
                {
                    case ')':
                        expectedChar = '(';
                        break;
                    case '}':
                        expectedChar = '{';
                        break;
                    case ']':
                        expectedChar = '[';
                        break;
                    default:
                        openBrackets.Push(cureBracket);
                        break;
                }

                if (expectedChar == default)
                {
                    continue;
                }

                if (openBrackets.Pop() != expectedChar)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
