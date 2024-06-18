namespace backend.Helpers
{
	public class PageableList<T>
	{
		public List<T> Items { get; private set; }
		public int PageNumber { get; private set; }
		public int TotalPages { get; private set; }
		public int TotalItems { get; private set; }

		public PageableList(List<T> items, int count, int pageNumber, int pageSize)
		{
			PageNumber = pageNumber;

			TotalPages = count / pageSize;
			if (count % pageSize != 0)
			{
				TotalPages++; // Add an extra page for the remaining items
			}
			TotalItems = count;
			Items = items;
		}

		public static PageableList<T> Create(IList<T> source, int pageNumber, int pageSize)
		{
			var count = source.Count;
			var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
			return new PageableList<T>(items, count, pageNumber, pageSize);
		}
	}
}
