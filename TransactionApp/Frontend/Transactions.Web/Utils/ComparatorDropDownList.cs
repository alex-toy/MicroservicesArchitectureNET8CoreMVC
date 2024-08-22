using Microsoft.AspNetCore.Mvc.Rendering;

namespace Transactions.Web.Utils;

public class ComparatorDropDownList
{
	public string Value { get; set; }
	public string Label { get; set; }
	public static SelectList PopulateComparatorDropDownList<T>() where T : ComparatorDropDownList, new()
	{
		List<T> comparators = new()
		{
			new T { Value = "<", Label = "<" },
			new T { Value = "<=", Label = "<=" },
			new T { Value = ">", Label = ">" },
			new T { Value = ">=", Label = ">=" },
		};

		return new SelectList(comparators, nameof(Value), nameof(Label));
	}
}
