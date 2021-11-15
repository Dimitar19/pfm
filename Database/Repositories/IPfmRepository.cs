using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using pfm.Commands;
using pfm.Database.Entities;
using pfm.Models;

namespace pfm.Database.Repositories
{
    public interface IPfmRepository
    {
        Task<TransactionEntity> TransactionGet(string id);
        Task<CategoryEntity> CategoryGet(string code);
        Task<TransactionPagedList<TransactionEntity>> Get(TransactionKind? transactionKind, DateTime? startDate, DateTime? endDate, int page = 1, int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.Asc);
        Task<List<CategoryEntity>> Get(string parentId);
        Task<TransactionEntity> CreateTransaction(TransactionEntity transaction);
        Task<CategoryEntity> CreateCategory(CategoryEntity category);
        Task<TransactionEntity> UpdateCategory(string id, TransactionCategorizeCommand command);
        Task<SpendingsByCategory> GetSpendingAnalytics(string catCode, DateTime? startDate, DateTime? endDate, Directions? direction);
    }
}