using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCount.Parsers;

public interface ITextParser
{
    /// <summary>
    /// Returns each word that composes the file text
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when an unsuported file by the parser is passed as an argument.</exception>
    public List<string> GetTokens(string source);
}
