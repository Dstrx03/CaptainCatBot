
using System;

namespace Cat.Common.Helpers
{
    public class MaskDataHelper
    {

        public static string MaskTelegramBotToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return token;

            int prefixShow = token.Length >= 18 ? (int)Math.Floor(token.Length / 9.0) : 0;
            int postfixShow = (int)Math.Floor(token.Length / 9.0);

            var result = "";

            for (var i = 0; i < token.Length; i++)
            {
                if (prefixShow > 0)
                {
                    result += token[i];
                    prefixShow--;
                    continue;
                }

                if (postfixShow > 0 && token.Length - 1 - postfixShow < i)
                {
                    result += token[i];
                    postfixShow--;
                    continue;
                }

                result += '*';
            }

            return result;
        }

        public static string MaskWebhookUrl(string url)
        {
            var splitChar = '/';
            var splits = url.Split(splitChar);
            var result = "";

            for (var i = 0; i < splits.Length; i++)
            {
                string splitCharStr = i == 0 ? "" : splitChar.ToString();
                if (i <= splits.Length - 2)
                {
                    result += splitCharStr + splits[i];
                }
                else
                {
                    result += splitCharStr + MaskTelegramBotToken(splits[i]);
                }
            }

            return result;
        }
    }
}
