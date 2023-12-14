using Microsoft.R9.Extensions.Redaction;
using Microsoft.R9.Extensions.Text;
using System.Globalization;
using System;

namespace R9WebApp
{
    public class YellingRedactor : BaseRedactor
    {
        private const string YellingTemplate = "GRRRRRRRR!!!!!!!!!!!!!!!! {0} !!!!!!!!!!!!!!!!!!!!!!!";
        private readonly CompositeFormat _yellingFormat;

        public YellingRedactor()
        {
            _yellingFormat = new CompositeFormat(YellingTemplate);
        }

        public override int GetRedactedLength(ReadOnlySpan<char> source)
        {
            return _yellingFormat.Format(CultureInfo.InvariantCulture, source.ToString()).Length;
        }

        public override int Redact(ReadOnlySpan<char> source, Span<char> destination)
        {
            var result = _yellingFormat.Format(CultureInfo.InvariantCulture, source.ToString());
           
            for (var i = 0; i < result.Length; i++)
            {
                destination[i] = result[i];
            }

            return result.Length;
        }
    }
}
