using System.Collections.Generic;

namespace Bro.Client.ConfigParsing
{
    public interface ISheetParser
    {
        string SheetsToJsonString(IEnumerable<(string, IList<IList<object>>)> sheets, string configTypeName, string sheetName, string previousConfig);
    }
}