using System;
using System.Linq.Expressions;

namespace SupportWave.Librarian.Data
{
	public interface IRepository<T, TId>
	{
		T? Get(TId id);
		List<T> GetAll();
		List<T> GetWhere(Func<T, bool> where);
		Task<T?> InsertAsync(T item);
		Task<T?> UpdateAsync(TId id, T item);
		Task<bool> DeleteAsync(TId id);
	}
}

