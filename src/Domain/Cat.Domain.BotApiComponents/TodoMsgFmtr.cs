using Cat.Domain.BotApiComponents.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cat.Domain
{
    public static class TodoMsgFmtr // todo: name, members names, design
    {
        // todo: consider to make Bot API Component names (Fake, Telegram, etc.) easy to maintain in messages - messages feature to generate component name etc. based on the type of the component

        private const string NullString = "*null*";
        private const string EmptyString = "*empty*";

        public static string FooBar(this BotApiComponentState source)
        {
            return string.IsNullOrEmpty(source.Description) ? source.State.ToString() : $"{source.State} - {source.Description}";
        }

        public static string DetailsMessage(string detailsTitle, (string title, object value)[] details)
        {
            var stringBuilder = new StringBuilder($"{detailsTitle}.{Environment.NewLine}");
            for (var i = 0; i < details.Length; i++)
            {
                var (title, value) = details[i];
                stringBuilder.Append($"{null,3}{title}: {value}{(i == details.Length - 1 ? string.Empty : Environment.NewLine)}");
            }
            return stringBuilder.ToString();
        }

        public static string Bar(this string source)
        {
            if (source == null) return NullString;
            if (string.IsNullOrEmpty(source) || string.IsNullOrWhiteSpace(source)) return EmptyString;
            return source;
        }

        public static string BarEnumerable<T>(this IEnumerable<T> source)
        {
            if (source == null) return NullString;
            if (!source.Any()) return EmptyString;
            return null;
        }

        public static string BarEnumerableToStrFmt(this IEnumerable<string> source)
        {
            var src = source.ToList();
            var barEnumerable = BarEnumerable(src);
            return barEnumerable ?? string.Join(", ", src);
        }

        public static string BarBar<T>(this T source, Func<T, string> action)
        {
            return source != null ? action(source).Bar() : NullString;
        }
    }
}
