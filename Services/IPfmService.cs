using Microsoft.AspNetCore.Http;
using pfm.Commands;
using pfm.Database.Entities;
using pfm.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pfm.Services
{
    public interface IPfmService 
    {
        //Task<Models.Transaction> ImportTransactions(CreateTransactionCommand command);
        Task<TransactionPagedList<Transaction>> GetTransactions(TransactionKind? transactionKind, DateTime? startDate, DateTime? endDate, int page = 1, int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.Asc);
        Task<List<Category>> GetCategories(string parentId);
        Task<Transaction> ImportTransactions(IFormFile file);
        Task<Category> ImportCategories(IFormFile file);
        Task<Transaction> CategorizeTransaction(string id, TransactionCategorizeCommand command);
        Task<SpendingsByCategory> GetSpendingAnalytics(string catCode, DateTime? startDate, DateTime? endDate, Directions? direction);
    }
}