namespace NemsisImport.Models;

public class TagCriteriaModel : TagModel
{
    public DateTime? TagStartDate { get; set; }
    public DateTime? TagEndDate { get; set; }

    public string ToQuery()
    {
        string quote(string value)
        {
            var quotedValue = "\"" + value + "\"";
            return quotedValue;

        }

        List<string> expressions = new();
        if (!string.IsNullOrEmpty(TagBlobType))
        {
            expressions.Add(quote("TagBlobType") + $" = '{TagBlobType}'");
        }
        if (TagStartDate.HasValue)
        {
            expressions.Add(quote("TagCreated") + $" >= '{TagStartDate.Value.ToString(TagConstants.Tag_Date_Format)}'");
        }
        if (TagEndDate.HasValue)
        {
            expressions.Add(quote("TagCreated") + $" < '{TagEndDate.Value.Date.AddDays(1).ToString(TagConstants.Tag_Date_Format)}'");
        }

        if (!expressions.Any())
        {
            return string.Empty;
        }

        string query = string.Join(" AND ", expressions);

        return query;

    }
}
